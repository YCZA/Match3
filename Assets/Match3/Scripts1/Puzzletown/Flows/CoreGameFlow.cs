// using Firebase.Analytics;
using System.Collections;
using System.Collections.Generic;
using Match3.Scripts1.Puzzletown.Match3;
using Match3.Scripts1.Puzzletown.Match3.Scoring;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Puzzletown.Tutorials;
using Match3.Scripts1.Shared.Flows;
using Match3.Scripts1.Shared.M3Engine;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.Signals;
using Wooga.UnityFramework;
using Match3.Scripts2.Android.DataStatistics;
using Match3.Scripts2.PlayerData;
using Match3.Scripts2.Shop;
using UnityEngine;
using static Wooga.UnityFramework.ServiceLocator;

namespace Match3.Scripts1.Puzzletown.Flows
{
	// eli key point: core game flow
	// Token: 0x020004B3 RID: 1203
	public class CoreGameFlow : AFlowR<CoreGameFlow.Input, CoreGameFlow.Result>
	{
		// Token: 0x1700053A RID: 1338
		// (get) Token: 0x060021E9 RID: 8681 RVA: 0x000908C3 File Offset: 0x0008ECC3
		public override bool ThrowImmediate
		{
			get
			{
				return true;
			}
		}
    
		// 启动后会执行这里
		protected override IEnumerator FlowRoutine(CoreGameFlow.Input input)
		{
			Debug.Log("核心流程开始");
			if (CoreGameFlow.flowRunning)
			{
				WoogaDebug.LogWarning(new object[]
				{
					"Core Game Flow already running"
				});
				yield break;
			}
			CoreGameFlow.flowRunning = true;
			CoreGameFlow.Result result = new CoreGameFlow.Result();
			Instance.InjectImmediate(this);
			if (this.sbs.SbsConfig.feature_switches.skip_levelmap_during_tutorial && this.progressionService.UnlockedLevel < 11)
			{
				input.selectedLevel = this.progressionService.UnlockedLevel;
				input.skipLevelMap = true;
				TutorialRunner tutorialRunner = global::UnityEngine.Object.FindObjectOfType<TutorialRunner>();
				if (tutorialRunner)
				{
					tutorialRunner.FinishCurrentTutorial();
				}
			}
			// this.sessionService.onRestart.AddListenerOnce(new Action(this.OnRestart));
			LevelConfig config = input.config;
			if (input.selectedLevel == 0)
			{
				if (CoreGameFlow.lastLevel >= this.progressionService.UnlockedLevel - 1)
				{
					CoreGameFlow.lastLevel = this.progressionService.UnlockedLevel;
				}
				input.selectedLevel = CoreGameFlow.lastLevel;
			}
			bool showLevelSelectionUI = !input.skipLevelMap || input.levelPlayMode == LevelPlayMode.LevelOfTheDay;
			if (showLevelSelectionUI)
			{
				// eli key point 和获取关卡配置信息有关
				Wooroutine<LevelConfig> levelSelection = new LevelSelectionFlow().Start(new LevelSelectionFlow.Input
				{
					levelToSnap = input.selectedLevel,
					levelPlayMode = input.levelPlayMode,
					tier = input.tier
				});
				Debug.Log("打开关卡列表");
				// 打开关卡列表
				yield return levelSelection;
				if (levelSelection.ReturnValue == null)
				{
					// 关闭关卡列表
					Debug.Log("关卡列表已关闭");
					CoreGameFlow.flowRunning = false;
					// yield return result;
					yield return null;
					yield break;
				}
				config = levelSelection.ReturnValue;
				// buried point: 预先使用道具
				List<string> boosterList = new List<string>();
				if(config.preBoostConfig.UseRainbow) boosterList.Add(Boosts.boost_pre_rainbow.ToString());
				if(config.preBoostConfig.UseDoubleFish) boosterList.Add(Boosts.boost_pre_double_fish.ToString());
				if(config.preBoostConfig.UseBombAndLinGem) boosterList.Add(Boosts.boost_pre_bomb_linegem.ToString());
				foreach (var boosterName in boosterList)
				{
					DataStatistics.Instance.TriggerUseBooster(config.Level.level, boosterName);
				}
				
				if (input.levelPlayMode == LevelPlayMode.Regular)
				{
					CoreGameFlow.lastLevel = config.Level.level;
				}
			}
			if (config == null)
			{
				bool isWeeklyEvent = input.levelPlayMode == LevelPlayMode.DiveForTreasure || input.levelPlayMode == LevelPlayMode.PirateBreakout;
				if (isWeeklyEvent)
				{
					Wooroutine<LevelConfig> c = this.m3ConfigService.GetLevelConfig(M3ConfigService.AREA_NOT_SET, input.selectedLevel, input.levelPlayMode, AreaConfig.Tier.undefined);
					yield return c;
					config = c.ReturnValue;
				}
				else
				{
					int area = this.m3ConfigService.GetAreaForLevel(input.selectedLevel);
					int tier = Mathf.Clamp(this.progressionService.GetTier(input.selectedLevel), 0, 2);
					Wooroutine<LevelConfig> c2 = this.m3ConfigService.GetLevelConfig(area, input.selectedLevel, input.levelPlayMode, (AreaConfig.Tier)tier);
					yield return c2;
					config = c2.ReturnValue;
				}
			}
			bool hasLoadedOnce = false;
			// this.tournamentService.NotifyContextChange(SceneContext.InGame);
			// this.levelOfDayService.NotifyContextChange(SceneContext.InGame);
			AwaitSignal<Match3Score> score;
			do
			{
				// this.trackingService.StartOfRound(config);
				this.progressionService.CurrentLevel = config.LevelCollectionConfig.level;
				LoadingScreenConfig loadingScreenConfig = this.ConfigureLoadingScreen(true, hasLoadedOnce);
				LevelRootInput levelRootInput = new LevelRootInput(config, input.levelPlayMode);
				GameSceneLoadingFlowWithTransition.Input loadingScreenInput = new GameSceneLoadingFlowWithTransition.Input(levelRootInput, loadingScreenConfig);
				// eli key point 载入关卡场景
				Wooroutine<Wooroutine<M3_LevelRoot>> gameSceneLoadingFlow = new GameSceneLoadingFlowWithTransition().Start(loadingScreenInput);
				yield return gameSceneLoadingFlow;
				Wooroutine<M3_LevelRoot> gameScene = gameSceneLoadingFlow.ReturnValue;
				this.AddOnCompleteListeners(input.levelPlayMode, gameScene);
				score = gameScene.ReturnValue.onCompleted;
				AwaitSignal<Match3Score> beforeRewards = gameScene.ReturnValue.onBeforeRewardsScreen.Await<Match3Score>();
				yield return beforeRewards;
				this.GiveRewardsAndUpdateTournamentScores(beforeRewards.Dispatched, config);
				yield return score;
				result.score = score.Dispatched;
				if (score.Dispatched.success)
				{
					this.HandleLevelWon(input, config.Level.level);
				}
				if (score.Dispatched.cheated && score.Dispatched.MovesTaken > 0 && !score.Dispatched.success && input.levelPlayMode != LevelPlayMode.LevelOfTheDay)
				{
					this.livesService.UseLife();
				}
				if (score.Dispatched.wantsRetry)
				{
					if (input.levelPlayMode != LevelPlayMode.LevelOfTheDay)
					{
						Wooroutine<bool> checkLives = new CheckLivesJourney(new TownDiamondsPanelRoot.TownDiamondsPanelRootParameters
						{
							source1 = "post_moves"
						}).Start<bool>();
						yield return checkLives;
						score.Dispatched.wantsRetry = checkLives.ReturnValue;
					}
					if (score.Dispatched.wantsRetry)
					{
						if (input.levelPlayMode == LevelPlayMode.Regular)
						{
							Wooroutine<M3_LevelStartRoot> levelStartScene = SceneManager.Instance.LoadSceneWithParams<M3_LevelStartRoot, LevelConfig>(config, null);
							yield return levelStartScene;
							yield return levelStartScene.ReturnValue.OnInitialized;
							levelStartScene.ReturnValue.Hide(false);
							yield return levelStartScene.ReturnValue.onCompleted;
							score.Dispatched.wantsRetry = levelStartScene.ReturnValue.onCompleted.Dispatched;
						}
						else if (input.levelPlayMode == LevelPlayMode.LevelOfTheDay)
						{
							Wooroutine<M3_LevelOfDayStartRoot> levelStartScene2 = SceneManager.Instance.LoadSceneWithParams<M3_LevelOfDayStartRoot, LevelConfig>(config, null);
							yield return levelStartScene2;
							yield return levelStartScene2.ReturnValue.onCompleted;
							score.Dispatched.wantsRetry = levelStartScene2.ReturnValue.onCompleted.Dispatched;
						}
						else if (input.levelPlayMode == LevelPlayMode.PirateBreakout)
						{
							Wooroutine<M3_PirateBreakoutStartRoot> levelStartScene3 = SceneManager.Instance.LoadSceneWithParams<M3_PirateBreakoutStartRoot, LevelConfig>(config, null);
							yield return levelStartScene3;
							yield return levelStartScene3.ReturnValue.onCompleted;
							score.Dispatched.wantsRetry = levelStartScene3.ReturnValue.onCompleted.Dispatched;
						}
						else
						{
							score.Dispatched.wantsRetry = false;
						}
					}
					this.TrackEndOfRound(score.Dispatched, this.GetLevelOfTheDayTryCount(input.levelPlayMode), this.GetLevelOfTheDayStreak());
				}
				hasLoadedOnce = true;
			}
			while (score.Dispatched.wantsRetry);
			this.TrackEndOfRound(score.Dispatched, this.GetLevelOfTheDayTryCount(input.levelPlayMode), this.GetLevelOfTheDayStreak());
			if (input.levelPlayMode == LevelPlayMode.DiveForTreasure)
			{
				yield return new LoadDiveForTreasureFlow().Start();
				yield return result;
			}
			else if (input.levelPlayMode == LevelPlayMode.PirateBreakout)
			{
				yield return new LoadPirateBreakoutFlow(score.Dispatched).Start();
				yield return result;
			}
			else if (!score.Dispatched.showLevelMap)
			{
				yield return this.ReturnToIsland(score.Dispatched);
				yield return result;
			}
			else
			{
				CoreGameFlow.flowRunning = false;
				Wooroutine<CoreGameFlow.Result> flow = new CoreGameFlow().Start(default(CoreGameFlow.Input));
				yield return flow;
				if (!flow.ReturnValue.levelCompleted)
				{
					yield return this.ReturnToIsland(score.Dispatched);
					yield return result;
				}
				else
				{
					yield return flow.ReturnValue;
				}
			}
			CoreGameFlow.flowRunning = false;
			
			Debug.Log("核心流程结束");
		}

		// Token: 0x060021EB RID: 8683 RVA: 0x000908EC File Offset: 0x0008ECEC
		private void AddOnCompleteListeners(LevelPlayMode levelPlayMode, Wooroutine<M3_LevelRoot> gameScene)
		{
			switch (levelPlayMode)
			{
			case LevelPlayMode.Regular:
			case LevelPlayMode.PirateBreakout:
				gameScene.ReturnValue.Loader.MatchEngine.onStepCompleted.AddListenerOnce(delegate(List<List<IMatchResult>> res)
				{
					this.livesService.UseLife();
				});
				break;
			case LevelPlayMode.LevelOfTheDay:
				gameScene.ReturnValue.Loader.MatchEngine.onStepCompleted.AddListenerOnce(delegate(List<List<IMatchResult>> res)
				{
					// this.levelOfDayService.SaveNewTryToWin();
				});
				break;
			case LevelPlayMode.DiveForTreasure:
				gameScene.ReturnValue.Loader.MatchEngine.onStepCompleted.AddListenerOnce(delegate(List<List<IMatchResult>> res)
				{
					this.livesService.UseLife();
				});
				gameScene.ReturnValue.Loader.MatchEngine.onStepCompleted.AddListenerOnce(delegate(List<List<IMatchResult>> res)
				{
					// this.diveForTreasureService.ResetToCheckPoint();
				});
				break;
			}
		}

		// Token: 0x060021EC RID: 8684 RVA: 0x000909BC File Offset: 0x0008EDBC
		private void HandleLevelWon(CoreGameFlow.Input input, int level)
		{
			if (input.levelPlayMode != LevelPlayMode.LevelOfTheDay)
			{
				this.livesService.AddLives(1, "通关");
			}
			AWeeklyEventDataService dataServiceForPlayMode = this.gameStateService.GetDataServiceForPlayMode(input.levelPlayMode);
			if (dataServiceForPlayMode != null)
			{
				dataServiceForPlayMode.Level = input.selectedLevel + 1;
				if (dataServiceForPlayMode.IsRewardedLevel(level))
				{
					Materials rewards = dataServiceForPlayMode.GetRewards(level);
					// this.gameStateService.Resources.AddPendingRewards(rewards, this.trackingService.GetWeeklyEventRewardCall(dataServiceForPlayMode, rewards, level));
					this.gameStateService.Resources.AddPendingRewards(rewards, null);
					string trophyForLevel = dataServiceForPlayMode.GetTrophyForLevel(level);
					if (!string.IsNullOrEmpty(trophyForLevel))
					{
						BuildingConfig config = this.configService.buildingConfigList.GetConfig(trophyForLevel);
						this.gameStateService.Buildings.StoreBuilding(config, 1);
					}
				}
			}
		}

		// Token: 0x060021ED RID: 8685 RVA: 0x00090A74 File Offset: 0x0008EE74
		private int GetLevelOfTheDayTryCount(LevelPlayMode levelPlayMode)
		{
			if (levelPlayMode != LevelPlayMode.LevelOfTheDay)
			{
				return 0;
			}
			// return this.levelOfDayService.GetCurrentTryCount();
			return 0;
		}

		// Token: 0x060021EE RID: 8686 RVA: 0x00090A8F File Offset: 0x0008EE8F
		private int GetLevelOfTheDayStreak()
		{
			// return this.levelOfDayService.GetCurrentStreak();
			return 0;
		}

		// Token: 0x060021EF RID: 8687 RVA: 0x00090A9C File Offset: 0x0008EE9C
		private Coroutine ReturnToIsland(Match3Score score)
		{
			return new ReturnToIslandFlow(score, null).Execute();
		}

		// Token: 0x060021F0 RID: 8688 RVA: 0x00090AAA File Offset: 0x0008EEAA
		private LoadingScreenConfig ConfigureLoadingScreen(bool toGameScene, bool hasLoadedOnce = false)
		{
			return LoadingScreenConfig.Random;
		}

		// Token: 0x060021F1 RID: 8689 RVA: 0x00090AAD File Offset: 0x0008EEAD
		private void OnRestart()
		{
			CoreGameFlow.flowRunning = false;
			CoreGameFlow.lastLevel = 0;
		}

		// Token: 0x060021F2 RID: 8690 RVA: 0x00090ABC File Offset: 0x0008EEBC
		private void TrackEndOfRound(Match3Score score, int tryCountForLevelOfTheDay, int levelOfDayStreak)
		{
			TrackingService.RoundOutcome outcome = TrackingService.RoundOutcome.Won;
			if (score.cancelled)
			{
				outcome = TrackingService.RoundOutcome.Cancelled;
			}
			else if (!score.success)
			{
				outcome = TrackingService.RoundOutcome.Lost;
			}
			if (!this.sbs.SbsConfig.feature_switches.level_of_day_streak)
			{
				levelOfDayStreak = 0;
			}
			if (!score.cheated)
			{
				// this.trackingService.EndOfRound(outcome, score, score.Config, this.tournamentService.GetActiveLeagueState(), tryCountForLevelOfTheDay, levelOfDayStreak);
			}
			else
			{
				// this.trackingService.CheatEndOfRound();
			}
			// this.sessionService.roundsPlayed++;
			// this.sessionService.wasLastRoundSuccesfull = score.success;
		}

		// Token: 0x060021F3 RID: 8691 RVA: 0x00090B68 File Offset: 0x0008EF68
		private void GiveRewardsAndUpdateTournamentScores(Match3Score score, LevelConfig config)
		{
			if (score.success)
			{
				// this.adjust.TrackLevelComplete(config.Level);
				this.GiveRewards(score);
				int currentTier;
				int previousTier;
				this.UpdateProgression(score, out currentTier, out previousTier);
				this.UpdateTournamentScore(score, currentTier, previousTier);
				this.UpdateBankedDiamonds(score, config);
				if (score.levelPlayMode == LevelPlayMode.LevelOfTheDay)
				{
					this.UpdateLevelOfTheDay(score);
				}
				this.gameStateService.Save(false);
				this.TrackRoundRewards(score, config);
			}
		}

		// Token: 0x060021F4 RID: 8692 RVA: 0x00090BDC File Offset: 0x0008EFDC
		private void GiveRewards(Match3Score score)
		{
			if (score.Rewards != null)
			{
				foreach (MaterialAmount material in score.Rewards)
				{
					if (material.type != "UnlimitedLives" && material.type != "unlimited_lives" && material.type != "lives_unlimited")
					{
						this.gameStateService.Resources.AddMaterial(material, false, "关卡");
					}
					else
					{
						this.livesService.StartUnlimitedLives(material.amount);
					}
				}
			}
			foreach (MaterialAmount materialAmount in score.AllCollected)
			{
				if (!(materialAmount.type == "coins") && !(materialAmount.type == "UnlimitedLives"))
				{
					string type = materialAmount.type;
					this.gameStateService.Resources.AddMaterial(type, materialAmount.amount, false);
				}
			}
		}

		// Token: 0x060021F5 RID: 8693 RVA: 0x00090D3C File Offset: 0x0008F13C
		private void UpdateLevelOfTheDay(Match3Score score)
		{
			if (score.success)
			{
				// this.levelOfDayService.SaveLevelWon();
			}
		}

		// Token: 0x060021F6 RID: 8694 RVA: 0x00090D54 File Offset: 0x0008F154
		private void UpdateProgression(Match3Score score, out int currentTier, out int previousTier)
		{
			previousTier = this.progressionService.GetTier(score.Config.LevelCollectionConfig.level);
			currentTier = (int)score.Config.SelectedTier;
			if (score.levelPlayMode == LevelPlayMode.Regular)
			{
				this.progressionService.CompleteTier(score.Config.LevelCollectionConfig.level, currentTier);
			}
		}

		// Token: 0x060021F7 RID: 8695 RVA: 0x00090DB4 File Offset: 0x0008F1B4
		private void UpdateBankedDiamonds(Match3Score score, LevelConfig config)
		{
			// int bankedDiamondRewardForLevel = this.bankService.GetBankedDiamondRewardForLevel(config);
			// if (bankedDiamondRewardForLevel > 0)
			// {
				// this.bankService.UpdateBankedDiamonds(config);
				// score.bankedDiamonds = bankedDiamondRewardForLevel;
			// }
		}

		// Token: 0x060021F8 RID: 8696 RVA: 0x00090DE8 File Offset: 0x0008F1E8
		private void UpdateTournamentScore(Match3Score score, int currentTier, int previousTier)
		{
			bool flag = previousTier == currentTier;
			bool shouldIgnoreMultiplier = score.levelPlayMode == LevelPlayMode.Regular && flag;
			this.gameStateService.Tournaments.UpdateScores(score, currentTier, shouldIgnoreMultiplier);
		}

		// Token: 0x060021F9 RID: 8697 RVA: 0x00090E20 File Offset: 0x0008F220
		private void TrackRoundRewards(Match3Score score, LevelConfig config)
		{
			int num = 0;
			int num2 = 0;
			foreach (MaterialAmount materialAmount in score.Rewards)
			{
				if (materialAmount.type == "coins")
				{
					num += materialAmount.amount;
				}
				if (materialAmount.type == "diamonds")
				{
					num2 += materialAmount.amount;
				}
			}
			string reward_info = string.Empty;
			LevelPlayMode levelPlayMode = score.levelPlayMode;
			if (levelPlayMode != LevelPlayMode.LevelOfTheDay)
			{
				if (levelPlayMode != LevelPlayMode.DiveForTreasure)
				{
					if (levelPlayMode == LevelPlayMode.PirateBreakout)
					{
						reward_info = "pirate_breakout";
					}
				}
				else
				{
					reward_info = "treasure_diving";
				}
			}
			else
			{
				reward_info = "level_of_day";
			}
			// this.trackingService.TrackRewards("round_rewards", config.Name, config.LevelCollectionConfig.level.ToString(), reward_info, num, num2);
		}

		// Token: 0x04004D3C RID: 19772
		public static bool flowRunning;

		// Token: 0x04004D3D RID: 19773
		private static int lastLevel;

		// Token: 0x04004D3E RID: 19774
		[WaitForService(true, true)]
		private GameStateService gameStateService;

		// Token: 0x04004D3F RID: 19775
		[WaitForService(true, true)]
		private ConfigService configService;

		// Token: 0x04004D40 RID: 19776
		[WaitForService(true, true)]
		private M3ConfigService m3ConfigService;

		// Token: 0x04004D41 RID: 19777
		[WaitForService(true, true)]
		private LivesService livesService;

		// Token: 0x04004D42 RID: 19778
		// [WaitForService(true, true)]
		// private QuestService questsService;

		// Token: 0x04004D43 RID: 19779
		[WaitForService(true, true)]
		private ProgressionDataService.Service progressionService;

		// Token: 0x04004D44 RID: 19780
		// [WaitForService(true, true)]
		// private TrackingService trackingService;

		// Token: 0x04004D45 RID: 19781
		// [WaitForService(true, true)]
		// private SessionService sessionService;

		// Token: 0x04004D46 RID: 19782
		// [WaitForService(true, true)]
		// private IAdjustService adjust;

		// Token: 0x04004D47 RID: 19783
		// [WaitForService(true, true)]
		// private TournamentService tournamentService;

		// Token: 0x04004D48 RID: 19784
		[WaitForService(true, true)]
		private SBSService sbs;

		// Token: 0x04004D49 RID: 19785
		[WaitForService(true, true)]
		private AssetBundleService assetBundleService;

		// Token: 0x04004D4A RID: 19786
		// [WaitForService(true, true)]
		// private BankService bankService;

		// Token: 0x04004D4B RID: 19787
		// [WaitForService(true, true)]
		// private LevelOfDayService levelOfDayService;

		// Token: 0x04004D4C RID: 19788
		// [WaitForService(true, true)]
		// private DiveForTreasureService diveForTreasureService;

		// Token: 0x04004D4D RID: 19789
		// [WaitForService(true, true)]
		// private PirateBreakoutService pirateBreakoutService;

		// Token: 0x020004B4 RID: 1204
		public class Result
		{
			// Token: 0x04004D4E RID: 19790
			public bool levelCompleted;

			// Token: 0x04004D4F RID: 19791
			public Match3Score score;
		}

		// Token: 0x020004B5 RID: 1205
		public struct Input
		{
			// Token: 0x06002200 RID: 8704 RVA: 0x00090F78 File Offset: 0x0008F378
			public Input(int selectedLevel = 0, bool skipLevelMap = false, LevelConfig config = null, LevelPlayMode levelPlayMode = LevelPlayMode.Regular, AreaConfig.Tier tier = AreaConfig.Tier.a)
			{
				this.skipLevelMap = skipLevelMap;
				this.config = config;
				this.selectedLevel = selectedLevel;
				this.tier = tier;
				this.levelPlayMode = levelPlayMode;
			}

			// Token: 0x04004D50 RID: 19792
			public LevelConfig config;

			// Token: 0x04004D51 RID: 19793
			public int selectedLevel;

			// Token: 0x04004D52 RID: 19794
			public LevelPlayMode levelPlayMode;

			// Token: 0x04004D53 RID: 19795
			public bool skipLevelMap;

			public AreaConfig.Tier tier;
		}
	}
}
