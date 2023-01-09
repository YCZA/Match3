using System;
using System.Collections;
using System.Collections.Generic;
using Match3.Scripts1.Puzzletown.Flows;
using Match3.Scripts1.Puzzletown.Match3;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.Signals;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x020007D2 RID: 2002
	public class LevelOfDayService : AService
	{
		// Token: 0x06003141 RID: 12609 RVA: 0x000E7759 File Offset: 0x000E5B59
		public LevelOfDayService()
		{
			WooroutineRunner.StartCoroutine(this.InitRoutine(), null);
		}

		// Token: 0x170007BC RID: 1980
		// (get) Token: 0x06003142 RID: 12610 RVA: 0x000E7779 File Offset: 0x000E5B79
		// (set) Token: 0x06003143 RID: 12611 RVA: 0x000E7781 File Offset: 0x000E5B81
		public bool IsEnabled { get; protected set; }

		// Token: 0x170007BD RID: 1981
		// (get) Token: 0x06003144 RID: 12612 RVA: 0x000E778A File Offset: 0x000E5B8A
		// (set) Token: 0x06003145 RID: 12613 RVA: 0x000E7792 File Offset: 0x000E5B92
		public bool IsUnlocked { get; protected set; }

		// Token: 0x170007BE RID: 1982
		// (get) Token: 0x06003146 RID: 12614 RVA: 0x000E779B File Offset: 0x000E5B9B
		// (set) Token: 0x06003147 RID: 12615 RVA: 0x000E77A3 File Offset: 0x000E5BA3

		protected LevelOfDayModel CurrentLevelOfDayModel { get; set; } = new LevelOfDayModel();

		// Token: 0x170007BF RID: 1983
		// (get) Token: 0x06003148 RID: 12616 RVA: 0x000E77AC File Offset: 0x000E5BAC
		// (set) Token: 0x06003149 RID: 12617 RVA: 0x000E77C7 File Offset: 0x000E5BC7
		public bool NotificationSeen
		{
			get
			{
				return this.CurrentLevelOfDayModel != null && this.CurrentLevelOfDayModel.notificationSeen;
			}
			set
			{
				if (this.CurrentLevelOfDayModel != null)
				{
					this.CurrentLevelOfDayModel.notificationSeen = value;
					this.SaveLevelOfDayModel();
				}
			}
		}

		// Token: 0x0600314A RID: 12618 RVA: 0x000E77E6 File Offset: 0x000E5BE6
		public Wooroutine<LevelConfig> GetCurrentLevelOfDayConfig(LevelOfDayModel modelOverride = null)
		{
			return WooroutineRunner.StartWooroutine<LevelConfig>(this.GetCurrentLevelOfDayConfigRoutine(modelOverride));
		}

		// Token: 0x0600314B RID: 12619 RVA: 0x000E77F4 File Offset: 0x000E5BF4
		public int GetCurrentTryCount()
		{
			return this.CurrentLevelOfDayModel.GetTryCount();
		}

		// Token: 0x0600314C RID: 12620 RVA: 0x000E7801 File Offset: 0x000E5C01
		public int GetCurrentStreak()
		{
			return this.CurrentLevelOfDayModel.currentDay;
		}

		// Token: 0x0600314D RID: 12621 RVA: 0x000E780E File Offset: 0x000E5C0E
		public int GetRemainingSeconds()
		{
			return this.CurrentLevelOfDayModel.GetRemainingSeconds(this.timeService.Now.ToUnixTimeStamp());
		}

		// Token: 0x0600314E RID: 12622 RVA: 0x000E782B File Offset: 0x000E5C2B
		public bool CanPlayerStillTry()
		{
			return this.GetRemainingSeconds() > 0 && this.CurrentLevelOfDayModel.HasTriesLeft() && !this.CurrentLevelOfDayModel.isCompleted;
		}

		// Token: 0x0600314F RID: 12623 RVA: 0x000E785A File Offset: 0x000E5C5A
		public void SaveNewTryToWin()
		{
			if (this.CurrentLevelOfDayModel != null)
			{
				this.CurrentLevelOfDayModel.triesSoFar++;
				this.SaveLevelOfDayModel();
			}
		}

		// Token: 0x06003150 RID: 12624 RVA: 0x000E7880 File Offset: 0x000E5C80
		public void SaveLevelWon()
		{
			if (this.CurrentLevelOfDayModel != null)
			{
				this.CurrentLevelOfDayModel.isCompleted = true;
				if (this.sbsService.SbsConfig.feature_switches.level_of_day_streak)
				{
					this.CurrentLevelOfDayModel.currentDay++;
				}
				this.SaveLevelOfDayModel();
			}
		}

		// Token: 0x06003151 RID: 12625 RVA: 0x000E78D8 File Offset: 0x000E5CD8
		public void NotifyContextChange(SceneContext newContext)
		{
			if (!this.IsEnabled)
			{
				return;
			}
			this.currentContext = newContext;
			SceneContext sceneContext = this.currentContext;
			if (sceneContext == SceneContext.MetaGame)
			{
				this.StopTicking();
				this.CheckIfUnlocked();
				this.TryUpdateLevelOfDayModel();
				this.StartTicking();
			}
		}

		// Token: 0x06003152 RID: 12626 RVA: 0x000E7930 File Offset: 0x000E5D30
		public override string ToString()
		{
			string text = this.TryGetLODPoolInfo();
			return string.Format("LOD enabled: {0} // Unlocked: {1} // {2}\n{3}", new object[]
			{
				this.IsEnabled,
				this.IsUnlocked,
				this.CurrentLevelOfDayModel.AsString(),
				text
			});
		}

		// Token: 0x06003153 RID: 12627 RVA: 0x000E7982 File Offset: 0x000E5D82
		public void CheatToggleUnlocked()
		{
			this.IsUnlocked = !this.IsUnlocked;
			if (this.IsUnlocked)
			{
				this.StopTicking();
				this.TryUpdateLevelOfDayModel();
				this.StartTicking();
			}
		}

		// Token: 0x06003154 RID: 12628 RVA: 0x000E79B0 File Offset: 0x000E5DB0
		public void CheatComplete(bool state)
		{
			if (this.CurrentLevelOfDayModel != null && this.CurrentLevelOfDayModel.isCompleted != state)
			{
				this.CurrentLevelOfDayModel.isCompleted = state;
				this.SaveLevelOfDayModel();
			}
		}

		// Token: 0x06003155 RID: 12629 RVA: 0x000E79E0 File Offset: 0x000E5DE0
		public void CheatChangeTries(int delta)
		{
			if (this.CurrentLevelOfDayModel != null)
			{
				int num = this.CurrentLevelOfDayModel.triesSoFar;
				num = Mathf.Clamp(num + delta, 0, 3);
				if (this.CurrentLevelOfDayModel.triesSoFar != num)
				{
					this.CurrentLevelOfDayModel.triesSoFar = num;
					this.SaveLevelOfDayModel();
				}
			}
		}

		// Token: 0x06003156 RID: 12630 RVA: 0x000E7A32 File Offset: 0x000E5E32
		public void CheatReset()
		{
			this.StopTicking();
			this.CurrentLevelOfDayModel = this.PickNewLevelOfDay();
			this.CurrentLevelOfDayModel.currentDay = 1;
			this.SaveLevelOfDayModel();
			this.StartTicking();
		}

		// Token: 0x06003157 RID: 12631 RVA: 0x000E7A60 File Offset: 0x000E5E60
		public void CheatEndSoon()
		{
			if (this.CurrentLevelOfDayModel != null)
			{
				DateTime time = this.timeService.Now.Add(TimeSpan.FromSeconds(80.0));
				this.CurrentLevelOfDayModel.endUTCTime = time.ToUnixTimeStamp();
				this.SaveLevelOfDayModel();
			}
		}

		// Token: 0x06003158 RID: 12632 RVA: 0x000E7AB4 File Offset: 0x000E5EB4
		public void CheatStartSoon()
		{
			if (this.CurrentLevelOfDayModel == null)
			{
				this.CurrentLevelOfDayModel = this.PickNewLevelOfDay();
			}
			DateTime time = this.timeService.Now.Add(TimeSpan.FromSeconds(86410.0));
			this.CurrentLevelOfDayModel.endUTCTime = time.ToUnixTimeStamp();
			this.SaveLevelOfDayModel();
		}

		// Token: 0x06003159 RID: 12633 RVA: 0x000E7B11 File Offset: 0x000E5F11
		public void CheatModifyStreak(int dayDelta)
		{
			if (this.CurrentLevelOfDayModel != null)
			{
				this.CurrentLevelOfDayModel.currentDay += dayDelta;
			}
			this.SaveLevelOfDayModel();
		}

		// Token: 0x0600315A RID: 12634 RVA: 0x000E7B38 File Offset: 0x000E5F38
		private string TryGetLODPoolInfo()
		{
			if (this.eligibleLevels == null)
			{
				this.LoadListOfEligibleLevels();
			}
			if (this.eligibleLevels == null)
			{
				return "WARNING: Couldn't load pool!";
			}
			int num = this.eligibleLevels[0];
			int num2 = this.eligibleLevels[this.eligibleLevels.Count - 1];
			return string.Format("Pool: levels: {0} -> {1}; count: {2}", num, num2, this.eligibleLevels.Count);
		}

		// Token: 0x0600315B RID: 12635 RVA: 0x000E7BB3 File Offset: 0x000E5FB3
		private void CheckIfUnlocked()
		{
			this.IsUnlocked = (this.progressionService.UnlockedLevel >= this.sbsService.SbsConfig.level_of_day.general.unlock_level);
		}

		// Token: 0x0600315C RID: 12636 RVA: 0x000E7BE8 File Offset: 0x000E5FE8
		private IEnumerator InitRoutine()
		{
			yield return ServiceLocator.Instance.Inject(this);
			this.IsEnabled = this.sbsService.SbsConfig.feature_switches.level_of_day;
			// 审核版禁用levelOfDay(禁用后，无法从关卡正常退出, 从报错看不出什么)
			// #if REVIEW_VERSION
			// 	IsEnabled = false;
			// #endif
			this.CheckIfUnlocked();
			if (this.IsEnabled)
			{
				this.currentContext = SceneContext.MetaGame;
				this.LoadListOfEligibleLevels();
				this.TryUpdateLevelOfDayModel();
				this.StartTicking();
			}
			base.OnInitialized.Dispatch();
			yield break;
		}

		// Token: 0x0600315D RID: 12637 RVA: 0x000E7C03 File Offset: 0x000E6003
		private void StartTicking()
		{
			if (this.tickRoutine != null)
			{
				this.StopTicking();
			}
			this.tickRoutine = WooroutineRunner.StartCoroutine(this.TickRoutine(), null);
		}

		// Token: 0x0600315E RID: 12638 RVA: 0x000E7C28 File Offset: 0x000E6028
		private void StopTicking()
		{
			if (this.tickRoutine != null)
			{
				WooroutineRunner.Stop(this.tickRoutine);
				this.tickRoutine = null;
			}
		}

		// Token: 0x0600315F RID: 12639 RVA: 0x000E7C48 File Offset: 0x000E6048
		private IEnumerator TickRoutine()
		{
			WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();
			WaitForSeconds waitForOneSecond = new WaitForSeconds(1f);
			while (this.IsUnlocked && this.IsEnabled)
			{
				yield return waitForEndOfFrame;
				yield return waitForOneSecond;
				int utcNowAsInt = this.timeService.Now.ToUnixTimeStamp();
				int remainingSeconds = this.CurrentLevelOfDayModel.GetRemainingSeconds(utcNowAsInt);
				if (this.currentContext == SceneContext.MetaGame && remainingSeconds <= 0)
				{
					this.TryUpdateLevelOfDayModel();
				}
				if (this.CurrentLevelOfDayModel.CanPlayerStillTry(utcNowAsInt, out remainingSeconds))
				{
					this.OnTimerChanged.Dispatch(remainingSeconds);
				}
				else
				{
					this.OnTimerChanged.Dispatch(-1);
				}
			}
			if (!this.IsUnlocked)
			{
				this.OnTimerChanged.Dispatch(-1);
			}
			this.tickRoutine = null;
			yield break;
		}

		// Token: 0x06003160 RID: 12640 RVA: 0x000E7C64 File Offset: 0x000E6064
		private void TryUpdateLevelOfDayModel()
		{
			if (CoreGameFlow.flowRunning || !this.IsEnabled)
			{
				return;
			}
			if (this.CurrentLevelOfDayModel == null)
			{
				this.TryLoadSavedLevelOfDayModel();
			}
			int utcNow = this.timeService.Now.ToUnixTimeStamp();
			if (this.CurrentLevelOfDayModel.GetRemainingSeconds(utcNow) <= 0)
			{
				this.CurrentLevelOfDayModel = this.PickNewLevelOfDay();
				this.SaveLevelOfDayModel();
			}
		}

		// Token: 0x06003161 RID: 12641 RVA: 0x000E7CD0 File Offset: 0x000E60D0
		private bool TryLoadSavedLevelOfDayModel()
		{
			LevelOfDayModel levelOfDayModel;
			if (this.gameStateService.LevelOfDayData.TryGetSavedLevelOfDayModel(out levelOfDayModel))
			{
				this.CurrentLevelOfDayModel = levelOfDayModel;
			}
			return levelOfDayModel != null;
		}

		// Token: 0x06003162 RID: 12642 RVA: 0x000E7D02 File Offset: 0x000E6102
		private void SaveLevelOfDayModel()
		{
			this.gameStateService.LevelOfDayData.SaveLevelOfDayModel(this.CurrentLevelOfDayModel);
			this.gameStateService.Save(false);
		}

		// Token: 0x06003163 RID: 12643 RVA: 0x000E7D28 File Offset: 0x000E6128
		private LevelOfDayModel PickNewLevelOfDay()
		{
			List<int> history = this.CurrentLevelOfDayModel.GetHistory();
			int randomLevelOfDayIndex = this.GetRandomLevelOfDayIndex(history);
			int endUTCTime = this.CalculateLODEndTimeStamp();
			int num = (!this.CurrentLevelOfDayModel.isCompleted) ? 1 : this.CurrentLevelOfDayModel.currentDay;
			double totalDays = (this.timeService.Now - Scripts1.DateTimeExtensions.FromUnixTimeStamp(this.CurrentLevelOfDayModel.endUTCTime, DateTimeKind.Utc)).TotalDays;
			num = ((totalDays <= 1.0) ? num : 1);
			this.AddSelectedLevelToHistory(randomLevelOfDayIndex, history);
			return new LevelOfDayModel
			{
				level = randomLevelOfDayIndex,
				triesSoFar = 0,
				isCompleted = false,
				endUTCTime = endUTCTime,
				notificationSeen = false,
				lodHistory = history,
				currentDay = num
			};
		}

		// Token: 0x06003164 RID: 12644 RVA: 0x000E7E00 File Offset: 0x000E6200
		private void AddSelectedLevelToHistory(int selectedLevel, List<int> previousLevels)
		{
			if (previousLevels.Count >= 5)
			{
				for (int i = 0; i < previousLevels.Count - 1; i++)
				{
					previousLevels[i] = previousLevels[i + 1];
				}
				previousLevels[previousLevels.Count - 1] = selectedLevel;
			}
			else
			{
				previousLevels.Add(selectedLevel);
			}
		}

		// Token: 0x06003165 RID: 12645 RVA: 0x000E7E60 File Offset: 0x000E6260
		private int CalculateLODEndTimeStamp()
		{
			DateTime localNow = this.timeService.LocalNow;
			int start_hour = this.sbsService.SbsConfig.level_of_day.general.start_hour;
			DateTime d = new DateTime(localNow.Year, localNow.Month, localNow.Day, start_hour, 0, 0);
			TimeSpan value = d - localNow;
			DateTime time = this.timeService.Now.Add(value).Add(TimeSpan.FromDays(1.0));
			return time.ToUnixTimeStamp();
		}

		// Token: 0x06003166 RID: 12646 RVA: 0x000E7EF0 File Offset: 0x000E62F0
		private IEnumerator GetCurrentLevelOfDayConfigRoutine(LevelOfDayModel modelOverride)
		{
			if (modelOverride != null)
			{
				this.CurrentLevelOfDayModel = modelOverride;
			}
			int levelIndex = (this.CurrentLevelOfDayModel != null) ? this.CurrentLevelOfDayModel.level : 540;
			AreaConfig.Tier levelTier = AreaConfig.Tier.c;
			Wooroutine<LevelConfig> levelConfigGetRoutine = this.m3ConfigService.GetLevelConfig(this.m3ConfigService.GetAreaForLevel(levelIndex), levelIndex, LevelPlayMode.LevelOfTheDay, levelTier);
			yield return levelConfigGetRoutine;
			LevelConfig config = levelConfigGetRoutine.ReturnValue;
			yield return this.UpdateLevelOfDayRewards(config);
			yield break;
		}

		// Token: 0x06003167 RID: 12647 RVA: 0x000E7F14 File Offset: 0x000E6314
		protected int GetRandomLevelOfDayIndex(List<int> previousLevelsOfDay)
		{
			if (this.eligibleLevels == null)
			{
				this.LoadListOfEligibleLevels();
			}
			int unlockedLevel = this.progressionService.UnlockedLevel;
			int value = unlockedLevel + 80;
			int num = this.eligibleLevels.GetLowestIndexForValueLargerThan(value);
			int num2 = this.eligibleLevels.Count - 1;
			if (num == -1 || num2 - num < 20)
			{
				num = 0;
			}
			int num3 = -1;
			int num4 = num2 - num + 1;
			if (num4 > 5)
			{
				while (num3 == -1 || previousLevelsOfDay.Contains(this.eligibleLevels[num3]))
				{
					num3 = global::UnityEngine.Random.Range(num, num2 + 1);
				}
			}
			if (num3 == -1)
			{
				WoogaDebug.LogWarning(new object[]
				{
					"LoD: Couldn't pick an eligible level; going with fallback."
				});
				return 540;
			}
			// Debug.LogError("random: " + eligibleLevels[num3]);
			return this.eligibleLevels[num3];
		}

		// Token: 0x06003168 RID: 12648 RVA: 0x000E7FE4 File Offset: 0x000E63E4
		protected void LoadListOfEligibleLevels()
		{
			AreasConfig areas = this.configService.areas;
			IEnumerable<AreaConfig> allLevels = areas.AllLevels;
			// eli key point 审核版本只有330关(405关可用，只开放前5个area, 正好330关)
			// #if REVIEW_VERSION
			// allLevels = allLevels.ToList().GetRange(0,330);
			// #endif
				
			List<int> list = new List<int>();
			foreach (AreaConfig areaConfig in allLevels)
			{
				if (areaConfig.lod)
				{
					list.Add(areaConfig.level);
				}
			}
			this.eligibleLevels = new SortedLevelList(list);
			if (this.eligibleLevels == null || this.eligibleLevels.Count < 1)
			{
				this.eligibleLevels = this.CreateFallbackLevelList();
			}
		}

		// Token: 0x06003169 RID: 12649 RVA: 0x000E809C File Offset: 0x000E649C
		protected SortedLevelList CreateFallbackLevelList()
		{
			return new SortedLevelList(new List<int>
			{
				83,
				93,
				102,
				119,
				129,
				136,
				148,
				158,
				164,
				176,
				187,
				197,
				202,
				219,
				227,
				237,
				248,
				252,
				264,
				277,
				292,
				303,
				318,
				324,
				331,
				342,
				360,
				491,
				572,
				636
			});
		}

		// Token: 0x0600316A RID: 12650 RVA: 0x000E81F8 File Offset: 0x000E65F8
		protected LevelConfig UpdateLevelOfDayRewards(LevelConfig config)
		{
			config.collectable = string.Empty;
			this.CurrentLevelOfDayModel.currentDay = Math.Max(this.CurrentLevelOfDayModel.currentDay, 1);
			int day = (this.CurrentLevelOfDayModel.currentDay - 1) % 15 + 1;
			List<MaterialAmount> levelOfDayRewards = this.sbsService.SbsConfig.level_of_day.GetRewardsForDay(day);
			if (!this.sbsService.SbsConfig.feature_switches.level_of_day_streak)
			{
				levelOfDayRewards = new List<MaterialAmount>
				{
					new MaterialAmount(this.sbsService.SbsConfig.level_of_day.general.control_reward_1_type, this.sbsService.SbsConfig.level_of_day.general.control_reward_1_amount, MaterialAmountUsage.Undefined, 0),
					new MaterialAmount(this.sbsService.SbsConfig.level_of_day.general.control_reward_2_type, this.sbsService.SbsConfig.level_of_day.general.control_reward_2_amount, MaterialAmountUsage.Undefined, 0)
				};
			}
			config.levelOfDayRewards = levelOfDayRewards;
			config.coinMultiplier = this.configService.general.tier_factor[2].coin_multiplier;
			// config.tournamentMultiplier = this.tournamentService.GetCurrentScoreMultiplierForTier(2);
			return config;
		}

		// Token: 0x040059EE RID: 23022
		public const int LOD_HISTORY_COUNT = 5;

		// Token: 0x040059EF RID: 23023
		public const int LOOK_AHEAD_LEVEL_COUNT = 80;

		// Token: 0x040059F0 RID: 23024
		public const int REQUIRED_LEVEL_VARIETY_COUNT = 20;

		// Token: 0x040059F1 RID: 23025
		public const int REWARDS_WRAP_AROUND = 15;

		// Token: 0x040059F2 RID: 23026
		public const int MAX_TRIES = 3;

		// Token: 0x040059F3 RID: 23027
		private const int FALLBACK_LEVEL = 540;

		// Token: 0x040059F4 RID: 23028
		[WaitForService(true, true)]
		private SBSService sbsService;

		// Token: 0x040059F5 RID: 23029
		[WaitForService(true, true)]
		private ConfigService configService;

		// Token: 0x040059F6 RID: 23030
		[WaitForService(true, true)]
		private M3ConfigService m3ConfigService;

		// Token: 0x040059F7 RID: 23031
		[WaitForService(true, true)]
		private GameStateService gameStateService;

		// Token: 0x040059F8 RID: 23032
		[WaitForService(true, true)]
		private ProgressionDataService.Service progressionService;

		// Token: 0x040059F9 RID: 23033
		[WaitForService(true, true)]
		private TimeService timeService;

		// Token: 0x040059FA RID: 23034
		// [WaitForService(true, true)]
		// private TournamentService tournamentService;

		// Token: 0x040059FE RID: 23038
		public Signal<int> OnTimerChanged = new Signal<int>();

		// Token: 0x040059FF RID: 23039
		private SortedLevelList eligibleLevels;

		// Token: 0x04005A00 RID: 23040
		private Coroutine tickRoutine;

		// Token: 0x04005A01 RID: 23041
		private SceneContext currentContext;
	}
}
