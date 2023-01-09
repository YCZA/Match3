using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Match3.Scripts1.Audio;
using Match3.Scripts1.Puzzletown.Match3.Scoring;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Puzzletown.Tutorials;
using Match3.Scripts1.Shared.M3Engine;
using Match3.Scripts1.UnityEngine;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.Signals;
using Wooga.UnityFramework;
using Match3.Scripts2.Android.DataStatistics;
using Match3.Scripts2.PlayerData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Wooga.Foundation.Json;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020006C9 RID: 1737
	[LoadOptions(false, false, false)]
	[RequireComponent(typeof(MusicPlayer))]
	public class M3_LevelRoot : APtSceneRoot<LevelRootInput, Match3Score>
	{
		// Token: 0x170006D6 RID: 1750
		// (get) Token: 0x06002B44 RID: 11076 RVA: 0x000C5CFF File Offset: 0x000C40FF
		// (set) Token: 0x06002B45 RID: 11077 RVA: 0x000C5D07 File Offset: 0x000C4107
		public TutorialRunner TutorialRunner { get; private set; }

		// Token: 0x170006D7 RID: 1751
		// (get) Token: 0x06002B46 RID: 11078 RVA: 0x000C5D10 File Offset: 0x000C4110
		public LevelLoader Loader
		{
			get
			{
				return base.GetComponentInChildren<LevelLoader>();
			}
		}

		// Token: 0x170006D8 RID: 1752
		// (get) Token: 0x06002B47 RID: 11079 RVA: 0x000C5D18 File Offset: 0x000C4118
		// (set) Token: 0x06002B48 RID: 11080 RVA: 0x000C5D20 File Offset: 0x000C4120
		public M3_DebugUiRoot DebugUi { get; private set; }

		public void OnApplicationQuit()
		{
			gameStateService.Save();
		}

		// Token: 0x06002B49 RID: 11081 RVA: 0x000C5D29 File Offset: 0x000C4129
		public override void Setup(LevelRootInput parameters)
		{
			base.StartCoroutine(this.FlowRoutine(parameters));
			base.Setup(parameters);
		}

		// Token: 0x06002B4A RID: 11082 RVA: 0x000C5D40 File Offset: 0x000C4140
		public void AnimateCharacter(int sumCascades)
		{
			this.objectivesRoot.AnimateCharacter(this.Loader.ScoringController.IsObjectiveReached(), this.Loader.ScoringController.MovesLeft, sumCascades);
		}

		// Token: 0x06002B4B RID: 11083 RVA: 0x000C5D6E File Offset: 0x000C416E
		public void PlayCongratulationsAnimation(int sumCascades)
		{
			if (!this.Loader.ScoringController.IsObjectiveReached() && !this.Loader.ScoringController.OutOfMoves && sumCascades >= 4)
			{
				this.ShowCongratulation();
			}
		}

		// Token: 0x06002B4C RID: 11084 RVA: 0x000C5DA8 File Offset: 0x000C41A8
		protected override IEnumerator GoRoutine()
		{
			this.levelCompletedEffect.SetActive(false);
			if (this.gameStateService.Debug.ShowCheatMenus)
			{
				// Wooroutine<M3_DebugUiRoot> debugUi = SceneManager.Instance.LoadScene<M3_DebugUiRoot>(null);
				// yield return debugUi;
				// this.DebugUi = debugUi.ReturnValue;
				// this.lastHurray.onStarted.AddListener(delegate
				// {
				// 	this.DebugUi.Disable();
				// });
			}
			this.celebrationTexts = this.locaService.GetKeysWithSubstring("ui.level.celebration_text");
			if (base.registeredFirst)
			{
				this.Setup(null);
			}
			this.InitAudio();
			this.InitDoTween();
			Application.targetFrameRate = FPSService.TargetFrameRate;
			this.preloadService.TryStartPreloadMatch3Themes();
			yield break;
		}

		// Token: 0x06002B4D RID: 11085 RVA: 0x000C5DC4 File Offset: 0x000C41C4
		private void InitDoTween()
		{
			DOTween.Init(new bool?(true), new bool?(false), new LogBehaviour?(LogBehaviour.ErrorsOnly)).SetCapacity(100, 2);
			for (int i = 0; i < 50; i++)
			{
				base.transform.DOLocalMove(base.transform.position, 1f, false);
			}
		}

		// Token: 0x06002B4E RID: 11086 RVA: 0x000C5E24 File Offset: 0x000C4224
		private void Update()
		{
			if (this.canSpeedUp && !this.isSpedUp && Input.GetButtonDown("Fire1"))
			{
				this.isSpedUp = true;
				Time.timeScale = 2f;
			}
			if (this.isAutoPlayMode && this.startNextAutoPlayMove)
			{
				this.startNextAutoPlayMove = false;
				Move nextBestMove = this.Loader.MatchEngine.GetNextBestMove();
				this.Loader.BoardView.HandleSwapped(nextBestMove);
			}
		}

		// Token: 0x06002B4F RID: 11087 RVA: 0x000C5EA8 File Offset: 0x000C42A8
		private LevelConfig DoSetup(LevelConfig config, LevelPlayMode levelPlayMode)
		{
			if (config == null)
			{
				config = JSON.Deserialize<LevelConfig>(this.Loader.testConfig.text);
				config.UpdateObjectives();
			}
			this.Loader.Setup(config, base.registeredFirst, this.banners, levelPlayMode);
			this.boosterUi.Init(this.Loader.ScoringController, config.layout.HasWaterAndUnwateredFields);
			this.Loader.ScoringController.onMovesChanged.AddListener(delegate(int i)
			{
				this.objectivesRoot.UpdateMovesView(i);
			});
			this.Loader.ScoringController.onScoreChanged.AddListener(new Action(this.HandleScoreChanged));
			this.isAutoPlayMode = config.IsAutoPlayMode;
			// eli key point 设置自动玩
			// this.isAutoPlayMode = true;
			if (this.isAutoPlayMode)
			{
				this.Loader.BoardView.onAnimationFinished.AddListener(new Action(this.HandleAnimationFinished));
			}
			if (config.IsEditMode)
			{
				this.objectivesRoot.UpdateTitleView(config.LevelCollectionConfig.level.ToString());
			}
			else
			{
				string level;
				if (config.LevelCollectionConfig != null)
				{
					level = config.LevelCollectionConfig.level.ToString();
				}
				else
				{
					level = this.Loader.testConfig.name;
				}
				this.objectivesRoot.UpdateTitleView(level);
			}
			this.HandleScoreChanged();
			this.objectivesRoot.UpdateMovesView(this.Loader.ScoringController.MovesLeft);
			return config;
		}

		// Token: 0x06002B50 RID: 11088 RVA: 0x000C6023 File Offset: 0x000C4423
		private void HandleAnimationFinished()
		{
			this.startNextAutoPlayMove = true;
		}

		// Token: 0x06002B51 RID: 11089 RVA: 0x000C602C File Offset: 0x000C442C
		private void HandleOrientationChange(ScreenOrientation screenOrientation)
		{
			this.HandleScoreChangedWithForcedRefresh(true);
		}

		// Token: 0x06002B52 RID: 11090 RVA: 0x000C6035 File Offset: 0x000C4435
		private void HandleScoreChanged()
		{
			this.HandleScoreChangedWithForcedRefresh(false);
		}

		// Token: 0x06002B53 RID: 11091 RVA: 0x000C6040 File Offset: 0x000C4440
		private void HandleScoreChangedWithForcedRefresh(bool forceRefresh = false)
		{
			MaterialAmount[] objectives = this.Loader.ScoringController.ObjectivesLeft.ToArray<MaterialAmount>();
			bool hasCoins = this.Loader.ScoringController.AreCoinsCollectable();
			MaterialAmount coinsCollected = this.Loader.ScoringController.CoinsCollected;
			this.objectivesRoot.UpdateObjectivesView(objectives, hasCoins, coinsCollected, forceRefresh);
			this.objectivesRoot.UpdateTournamentScoreView(this.Loader.ScoringController.GetTournamentScore().CollectedPoints);
			this.objectivesRoot.AnimateCharacter(this.Loader.ScoringController.IsObjectiveReached(), this.Loader.ScoringController.MovesLeft, 0);
		}

		// Token: 0x06002B54 RID: 11092 RVA: 0x000C60E4 File Offset: 0x000C44E4
		private void ShowCongratulation()
		{
			string text = RandomHelper.Next<string>(this.celebrationTexts);
			AudioId audioForCelebrationText = this.GetAudioForCelebrationText(text);
			if (text != null)
			{
				string text2 = this.locaService.GetText(text, new LocaParam[0]);
				this.celebrationText.text = text2;
				this.celebrationTextLayer2.text = text2;
				this.celebrationTextLayer3.text = text2;
			}
			this.celebrationAnimation.gameObject.SetActive(true);
			this.audioService.PlaySFX(AudioId.ShowCongratulations, false, false, false);
			if (audioForCelebrationText != AudioId.Default)
			{
				this.audioService.PlaySFX(audioForCelebrationText, false, false, false);
			}
			this.celebrationAnimation.Play();
		}

		// Token: 0x06002B55 RID: 11093 RVA: 0x000C618C File Offset: 0x000C458C
		private AudioId GetAudioForCelebrationText(string celebrationKey)
		{
			if (celebrationKey == "ui.level.celebration_text_01")
			{
				return AudioId.Purrrfect;
			}
			if (celebrationKey == "ui.level.celebration_text_02")
			{
				return AudioId.Pawsome;
			}
			if (celebrationKey == "ui.level.celebration_text_03")
			{
				return AudioId.Tropicool;
			}
			if (celebrationKey == "ui.level.celebration_text_04")
			{
				return AudioId.Supurrr;
			}
			if (celebrationKey == "ui.level.celebration_text_05")
			{
				return AudioId.Furrrtastic;
			}
			global::UnityEngine.Debug.LogWarning("Requested unknown voiceover for key: " + celebrationKey);
			return AudioId.Default;
		}

		// Token: 0x06002B56 RID: 11094 RVA: 0x000C6218 File Offset: 0x000C4618
		public void HandleHiddenItemFound(HiddenItemView item, float delay)
		{
			RectTransform dooberDestination = this.objectivesRoot.GetDooberDestination(this.Loader.Config.data.hiddenItemName);
			if (dooberDestination)
			{
				this.doobers.SpawnDoober("m3-hiddenitem", item.sprite, item.transform, dooberDestination, delay);
			}
			else
			{
				WoogaDebug.LogWarningFormatted("Could not find destination for hidden item", new object[0]);
			}
		}

		// Token: 0x06002B57 RID: 11095 RVA: 0x000C6284 File Offset: 0x000C4684
		public void HandleTournamentItemCollected(TournamentItemCollectedView item)
		{
			Transform tournamentItemDestination = this.objectivesRoot.GetTournamentItemDestination();
			this.doobers.SpawnDoober("m3-tournamentitem", item.sprite, item.transform, tournamentItemDestination, 0f);
		}

		// Token: 0x06002B58 RID: 11096 RVA: 0x000C62C0 File Offset: 0x000C46C0
		public void HandleGemCollected(Transform start, GemColor color, GemType gemType)
		{
			string text = color.ToString();
			RectTransform dooberDestination = this.objectivesRoot.GetDooberDestination(text);
			if (!dooberDestination)
			{
				WoogaDebug.LogWarningFormatted("Could not find destination for {0}", new object[]
				{
					text
				});
				return;
			}
			Sprite sprite = null;
			string type = string.Empty;
			if (color == GemColor.Treasure)
			{
				type = "treasure";
			}
			else if (color == GemColor.Droppable)
			{
				type = "droppable";
			}
			else if (color == GemColor.Climber)
			{
				type = "climber";
			}
			else
			{
				if (gemType == GemType.Chameleon)
				{
					return;
				}
				type = "m3-gem";
				sprite = dooberDestination.GetComponent<Image>().sprite;
			}
			this.doobers.SpawnDoober(type, sprite, start, dooberDestination, 0f);
		}

		// Token: 0x06002B59 RID: 11097 RVA: 0x000C6378 File Offset: 0x000C4778
		public void HandleModifierCollected(Transform start, string type)
		{
			RectTransform dooberDestination = this.objectivesRoot.GetDooberDestination(type);
			if (!dooberDestination)
			{
				WoogaDebug.LogWarningFormatted("Could not find destination for {0}", new object[]
				{
					type
				});
				return;
			}
			Sprite sprite = null;
			if (!(type == "resistant_blocker") && !(type == "chameleon"))
			{
				sprite = dooberDestination.GetComponent<Image>().sprite;
			}
			this.doobers.SpawnDoober(type, sprite, start, dooberDestination, 0f);
		}

		// Token: 0x06002B5A RID: 11098 RVA: 0x000C63F9 File Offset: 0x000C47F9
		public void HandleBackButtonPressed()
		{
			this.boosterUi.HandleOptionsClicked();
		}

		// Token: 0x06002B5B RID: 11099 RVA: 0x000C6408 File Offset: 0x000C4808
		private void ApplyPreGameBoosts()
		{
			if (this.useDoubleFish)
			{
				this.PayBoost(Boosts.boost_pre_double_fish);
			}
			List<IBoost> list = new List<IBoost>();
			List<IntVector2> replaceablePositions = this.Loader.Fields.GetReplaceablePositions();
			if (this.useBombAndLinGem)
			{
				if (replaceablePositions.Count > 1)
				{
					IntVector2 andRemoveRandomPosition = this.GetAndRemoveRandomPosition(replaceablePositions);
					IntVector2 andRemoveRandomPosition2 = this.GetAndRemoveRandomPosition(replaceablePositions);
					PreBoostBombAndLinegem preBoostBombAndLinegem = new PreBoostBombAndLinegem(this.Loader.Fields, andRemoveRandomPosition, andRemoveRandomPosition2);
					if (preBoostBombAndLinegem.IsValid())
					{
						list.Add(preBoostBombAndLinegem);
						this.PayBoost(Boosts.boost_pre_bomb_linegem);
					}
				}
				else
				{
					WoogaDebug.LogWarning(new object[]
					{
						"Could not place pre game boost bomb and line"
					});
				}
			}
			if (this.useRainbow)
			{
				if (replaceablePositions.Count > 0)
				{
					IntVector2 andRemoveRandomPosition3 = this.GetAndRemoveRandomPosition(replaceablePositions);
					PreBoostRainbow preBoostRainbow = new PreBoostRainbow(this.Loader.Fields, andRemoveRandomPosition3);
					if (preBoostRainbow.IsValid())
					{
						list.Add(preBoostRainbow);
						this.PayBoost(Boosts.boost_pre_rainbow);
					}
				}
				else
				{
					WoogaDebug.LogWarning(new object[]
					{
						"Could not place pre game boost rainbow"
					});
				}
			}
			if (!list.IsNullOrEmptyCollection())
			{
				this.Loader.MatchEngine.ApplyBoosts(list);
			}
		}

		// Token: 0x06002B5C RID: 11100 RVA: 0x000C6534 File Offset: 0x000C4934
		private IntVector2 GetAndRemoveRandomPosition(List<IntVector2> positions)
		{
			int index = RandomHelper.Next(positions.Count);
			IntVector2 result = positions[index];
			positions.RemoveAt(index);
			return result;
		}

		// Token: 0x06002B5D RID: 11101 RVA: 0x000C6560 File Offset: 0x000C4960
		private void PayBoost(Boosts boostType)
		{
			string name = Enum.GetName(typeof(Boosts), boostType);
			this.gameStateService.Resources.Pay(new MaterialAmount(name, 1, MaterialAmountUsage.Undefined, 0));
			this.Loader.MatchEngine.HandleBoostUsed(boostType);
		}

		// Token: 0x06002B5E RID: 11102 RVA: 0x000C65AE File Offset: 0x000C49AE
		private void InitAudio()
		{
			this.audioService.LoadSettings();
			this.musicPlayer = base.GetComponent<MusicPlayer>();
			this.musicPlayer.Init(this.audioService);
		}

		// Token: 0x06002B5F RID: 11103 RVA: 0x000C65D8 File Offset: 0x000C49D8
		private void InitPreGameBoosts(LevelConfig config)
		{
			if (config == null)
			{
				return;
			}
			this.useDoubleFish = config.preBoostConfig.UseDoubleFish;
			this.useBombAndLinGem = config.preBoostConfig.UseBombAndLinGem;
			this.useRainbow = config.preBoostConfig.UseRainbow;
		}

		// Token: 0x06002B60 RID: 11104 RVA: 0x000C6614 File Offset: 0x000C4A14
		private IEnumerator WaitForLoadingScreenHide()
		{
			yield return new WaitForSeconds(LoadingScreenRoot.HideAnimDuration);
			yield break;
		}

		// Token: 0x06002B61 RID: 11105 RVA: 0x000C6628 File Offset: 0x000C4A28
		public Wooroutine<LevelTheme> TryLoadThemeRoutine(string themeName)
		{
			if (!themeName.Equals("climber"))
			{
				string bundleName = "m3_themes";
				if (!LevelTheme.THEMES_FIRST_BUNDLE.Contains(themeName))
				{
					bundleName = string.Format("m3_themes_{0}", themeName);
				}
				string assetName = string.Format("Assets/Puzzletown/Match3/Art/Themes/Configs/{0}.asset", themeName);
				return this.abs.LoadAsset<LevelTheme>(bundleName, assetName);
			}
			return null;
		}

		// Token: 0x06002B62 RID: 11106 RVA: 0x000C6684 File Offset: 0x000C4A84
		private IEnumerator FlowRoutine(LevelRootInput input)
		{
			yield return base.OnInitialized;
			LevelConfig config = input.config;
			
			// 开始关卡
			DataStatistics.Instance.levelData.startTime = (int)Time.time;
			DataStatistics.Instance.TriggerLevel(LevelPlayMode.Regular.ToString(), config.Level.level, config.Level.tier, 0, LevelState.开始.ToString(), 0, 0, 0, 0, 0, 0);
				
			this.InitPreGameBoosts(config);
			if (config.LevelCollectionConfig != null && !config.IsEditMode)
			{
				string themeName = config.LevelCollectionConfig.objective;
				Wooroutine<LevelTheme> tryLoadThemeRoutine = this.TryLoadThemeRoutine(themeName);
				yield return tryLoadThemeRoutine;
				LevelTheme theme = null;
				try
				{
					theme = tryLoadThemeRoutine.ReturnValue;
				}
				catch (Exception ex)
				{
					WoogaDebug.LogWarning(new object[]
					{
						ex.Message
					});
				}
				if (theme != null)
				{
					this.ShowOnChildren(theme, false, true);
				}
			}
			config = this.DoSetup(config, input.levelPlayMode);
			this.onBoardSetup.Dispatch();
			yield return this.WaitForLoadingScreenHide();
			yield return base.StartCoroutine(this.banners.ShowWelcome(config, 2f));

			debug = new M3_LevelDebug(Loader, doobers);
			
			this.TutorialRunner = base.GetComponent<TutorialRunner>();
			this.TutorialRunner.Run(config);
			if (!this.TutorialRunner.IsRunning)
			{
				BackButtonManager.Instance.SetEnabled(true);
			}
			BackButtonManager.Instance.AddAction(new Action(this.HandleBackButtonPressed));
			this.ApplyPreGameBoosts();
			config.preBoostConfig.ResetBoosts();
			AUiAdjuster.OnScreenOrientationChange.AddListener(new Action<ScreenOrientation>(this.HandleOrientationChange));
			if (config.LevelCollectionConfig != null && !config.IsEditMode)
			{
				this.Loader.BoardView.HighlightObjective(this.highlightObjectiveDelay, config.LevelCollectionConfig.objective);
			}
			if (this.isAutoPlayMode)
			{
				this.startNextAutoPlayMove = true;
			}
			AwaitSignal<Match3Score> score = null;
			// Debug.LogError("testlevel0"); // 开始
			// do
			// {
				this.overlay.Disable();
				score = this.Loader.ScoringController.onGameOver.Await<Match3Score>();
				yield return score;
			// }
			// while (config.IsEditMode && !score.Dispatched.success);
			if (score.Dispatched.movesLeft == config.data.moves)
			{
				this.onBeforeRewardsScreen.Dispatch(score.Dispatched);
			}
			this.isAutoPlayMode = false;
			this.overlay.Enable();
			BackButtonManager.Instance.RemoveAction(new Action(this.HandleBackButtonPressed));
			// Debug.LogError("testlevel5"); // 结束
			if (!config.IsEditMode && (score.Dispatched.movesLeft != config.data.moves || score.Dispatched.success))
			{
				// 没有操作就退出(没有输出)
				// 放弃1 6 9 13  重试/退出 14
				// 失败1 6 9 13 重试/退出 14 (没看到买步)
				// 通关1 2 3 显示"youwon"4 播放消除动画，弹出领奖界面5 6 7 领取奖励后8
				
				// Debug.LogError("testEnd1");
				if (score.Dispatched.success && !score.Dispatched.cheated)
				{
					// Debug.LogError("testEnd2");
					yield return new WaitUntil(() => !this.boosterUi.options.gameObject.activeSelf);
					// 通关
					DataStatistics.Instance.TriggerLevel(LevelPlayMode.Regular.ToString(), config.Level.level, config.Level.tier,
						(int)Time.time - DataStatistics.Instance.levelData.startTime, LevelState.通关.ToString(), 0,
						score.Dispatched.ingameHammerUsed, score.Dispatched.ingameStarUsed, score.Dispatched.ingameRainbowUsed,
						score.Dispatched.ingameHammerUsed + score.Dispatched.ingameStarUsed + score.Dispatched.ingameRainbowUsed,
						score.Dispatched.movesLeft);
					// Debug.LogError("testEnd3");
					
					this.boosterUi.Hide();
					this.levelCompletedEffect.SetActive(true);
					this.audioService.PlaySFX(AudioId.BannerShowLevelComplete, false, false, false);
					this.audioService.PlaySFX(AudioId.LevelWonFireworks, false, false, false);
					this.canSpeedUp = true;
					yield return new WaitForSeconds(this.pauseForShowingLevelComplete);
					// Debug.LogError("testEnd4");
					Time.timeScale = 1f;
					this.canSpeedUp = false;
					if (config.LevelCollectionConfig.level >= this.lastHurray.minLevelWithLastHurray || config.SelectedTier > AreaConfig.Tier.a || input.levelPlayMode == LevelPlayMode.DiveForTreasure)
					{
						int tmpMovesLeft = score.Dispatched.movesLeft;
						this.Loader.MatchEngine.SwitchToLastHurray();
						this.Loader.BoardView.SetLastHurrayStarted();
						yield return this.lastHurray.ApplyLastHurray(score.Dispatched.movesLeft, this.isSpedUp);
						// Debug.LogError("testEnd5");
						score.Dispatched.movesLeft = tmpMovesLeft;
					}
					this.isSpedUp = false;
				}
				score.Dispatched.levelCascades = this.Loader.MatchEngine.sumLevelCascades;
				this.onBeforeRewardsScreen.Dispatch(score.Dispatched); 
				// Debug.LogError("testEnd6");
				if (score.Dispatched.success)
				{
					Wooroutine<M3_CompletedWonRoot> completedScreen = SceneManager.Instance.LoadSceneWithParams<M3_CompletedWonRoot, Match3Score>(score.Dispatched, null);
					yield return completedScreen;
					// Debug.LogError("testEnd7");
					AwaitSignal<bool> result = completedScreen.ReturnValue.onCompleted;
					yield return result;
					// Debug.LogError("testEnd8");
					if (score.Dispatched.levelPlayMode == LevelPlayMode.LevelOfTheDay && this.sbsService.SbsConfig.feature_switches.level_of_day_streak)
					{
						Wooroutine<M3_LevelOfDaySelectionRoot> lodSelection = SceneManager.Instance.LoadSceneWithParams<M3_LevelOfDaySelectionRoot, bool>(true, null);
						yield return lodSelection;
						yield return lodSelection.ReturnValue.onDestroyed;
						yield return result;
					}
					score.Dispatched.showLevelMap = result.Dispatched;
				}
				else
				{
					// Debug.LogError("testEnd9");
					AwaitSignal<bool> wantsRetry;
					if (score.Dispatched.levelPlayMode == LevelPlayMode.DiveForTreasure)
					{
						Wooroutine<M3_DiveForTreasureFailedRoot> diveForTreasureFailedScreen = SceneManager.Instance.LoadSceneWithParams<M3_DiveForTreasureFailedRoot, Match3Score>(score.Dispatched, null);
						yield return diveForTreasureFailedScreen;
						// Debug.LogError("testEnd10");
						wantsRetry = diveForTreasureFailedScreen.ReturnValue.onCompleted;
					}
					else if (score.Dispatched.levelPlayMode == LevelPlayMode.PirateBreakout)
					{
						Wooroutine<M3_PirateBreakoutFailedRoot> pirateBreakoutFailedScreen = SceneManager.Instance.LoadSceneWithParams<M3_PirateBreakoutFailedRoot, Match3Score>(score.Dispatched, null);
						yield return pirateBreakoutFailedScreen;
						// Debug.LogError("testEnd11");
						wantsRetry = pirateBreakoutFailedScreen.ReturnValue.onCompleted;
					}
					else if (score.Dispatched.levelPlayMode == LevelPlayMode.LevelOfTheDay)
					{
						Wooroutine<M3_LevelOfDayFailedRoot> levelOfTheDayFailedScreen = SceneManager.Instance.LoadSceneWithParams<M3_LevelOfDayFailedRoot, Match3Score>(score.Dispatched, null);
						yield return levelOfTheDayFailedScreen;
						// Debug.LogError("testEnd12");
						wantsRetry = levelOfTheDayFailedScreen.ReturnValue.onCompleted;
					}
					else
					{
						Wooroutine<M3_CompletedFailedRoot> completedScreen2 = SceneManager.Instance.LoadSceneWithParams<M3_CompletedFailedRoot, Match3Score>(score.Dispatched, null);
						yield return completedScreen2;
						DataStatistics.Instance.TriggerLevel(LevelPlayMode.Regular.ToString(), config.Level.level, config.Level.tier,
						(int)Time.time - DataStatistics.Instance.levelData.startTime, 
						score.Dispatched.movesLeft == 0 ?LevelState.失败.ToString():LevelState.放弃.ToString(), 0,
						score.Dispatched.ingameHammerUsed, score.Dispatched.ingameStarUsed, score.Dispatched.ingameRainbowUsed,
						score.Dispatched.ingameHammerUsed + score.Dispatched.ingameStarUsed + score.Dispatched.ingameRainbowUsed,
						score.Dispatched.movesLeft);
						// Debug.LogError("testEnd13");	// 失败或放弃
						
						wantsRetry = completedScreen2.ReturnValue.onCompleted;
					}
					yield return wantsRetry;
					// Debug.LogError("testEnd14");
					score.Dispatched.wantsRetry = (wantsRetry.Dispatched && score.Dispatched.levelPlayMode != LevelPlayMode.DiveForTreasure);
				}
			}
			else if (config.IsEditMode)
			{
				// if (this.DebugUi.GetComponent<M3DebugMenu>().IsVideoRecordMode)
				// {
					// yield return new WaitForSeconds(this.returnToEditorDelay);
				// }
				score.Dispatched.levelCascades = this.Loader.MatchEngine.sumLevelCascades;
			}
			DOTween.Clear(false);
			DOTween.ClearCachedTweens();
			this.onCompleted.Dispatch(score.Dispatched);
			yield break;
		}

		// Token: 0x06002B63 RID: 11107 RVA: 0x000C66A6 File Offset: 0x000C4AA6
		protected override void OnDisable()
		{
			base.OnDisable();
			AUiAdjuster.OnScreenOrientationChange.RemoveListener(new Action<ScreenOrientation>(this.HandleOrientationChange));
		}

		// Token: 0x0400545C RID: 21596
		[WaitForService(true, true)]
		public AssetBundleService abs;

		// Token: 0x0400545D RID: 21597
		[WaitForService(true, true)]
		public GameStateService gameStateService;

		// Token: 0x0400545E RID: 21598
		[WaitForService(true, true)]
		public AudioService audioService;

		// Token: 0x0400545F RID: 21599
		[WaitForService(true, true)]
		public ILocalizationService locaService;

		// Token: 0x04005460 RID: 21600
		// [WaitForService(true, true)]
		// public TournamentService tournamentService;

		// Token: 0x04005461 RID: 21601
		[WaitForService(true, true)]
		private AssetBundlePreloadService preloadService;

		// Token: 0x04005462 RID: 21602
		[WaitForService(true, true)]
		private SBSService sbsService;

		// Token: 0x04005463 RID: 21603
		[WaitForRoot(false, false)]
		[HideInInspector]
		public M3_ObjectivesUiRoot objectivesRoot;

		// Token: 0x04005464 RID: 21604
		[WaitForRoot(false, false)]
		[HideInInspector]
		public BoostsUiRoot boosterUi;

		// Token: 0x04005465 RID: 21605
		[WaitForRoot(false, false)]
		[HideInInspector]
		public M3_BannersRoot banners;

		// Token: 0x04005466 RID: 21606
		[WaitForRoot(false, false)]
		[HideInInspector]
		public OverlayRoot overlay;

		// Token: 0x04005467 RID: 21607
		private const int CASCADES_TO_PLAY_CONGRATULATIONS = 4;

		// Token: 0x04005468 RID: 21608
		[WaitForRoot(false, false)]
		private EventSystemRoot eventSystem;

		// Token: 0x04005469 RID: 21609
		[WaitForRoot(false, false)]
		private DoobersRoot doobers;

		// Token: 0x0400546A RID: 21610
		[SerializeField]
		private LastHurray lastHurray;

		// Token: 0x0400546B RID: 21611
		[SerializeField]
		private Animation celebrationAnimation;

		// Token: 0x0400546C RID: 21612
		[SerializeField]
		private TextMeshProUGUI celebrationText;

		// Token: 0x0400546D RID: 21613
		[SerializeField]
		private TextMeshProUGUI celebrationTextLayer2;

		// Token: 0x0400546E RID: 21614
		[SerializeField]
		private TextMeshProUGUI celebrationTextLayer3;

		// Token: 0x0400546F RID: 21615
		[SerializeField]
		private GameObject levelCompletedEffect;

		// Token: 0x04005470 RID: 21616
		[SerializeField]
		private float pauseForShowingLevelComplete = 3.5f;

		// Token: 0x04005471 RID: 21617
		[SerializeField]
		private float highlightObjectiveDelay = 0.5f;

		// Token: 0x04005472 RID: 21618
		[SerializeField]
		private float returnToEditorDelay = 8f;

		// Token: 0x04005473 RID: 21619
		private bool canSpeedUp;

		// Token: 0x04005474 RID: 21620
		private bool isSpedUp;

		// Token: 0x04005475 RID: 21621
		private bool startNextAutoPlayMove;

		// Token: 0x04005476 RID: 21622
		private bool isAutoPlayMode;

		// Token: 0x04005477 RID: 21623
		public readonly Signal onBoardSetup = new Signal();

		// Token: 0x04005478 RID: 21624
		public readonly Signal<Match3Score> onBeforeRewardsScreen = new Signal<Match3Score>();

		// Token: 0x04005479 RID: 21625
		private bool useDoubleFish;

		// Token: 0x0400547A RID: 21626
		private bool useBombAndLinGem;

		// Token: 0x0400547B RID: 21627
		private bool useRainbow;

		// Token: 0x0400547C RID: 21628
		private MusicPlayer musicPlayer;

		// Token: 0x0400547D RID: 21629
		private List<string> celebrationTexts;
		
		public static M3_LevelDebug debug;
	}
}
