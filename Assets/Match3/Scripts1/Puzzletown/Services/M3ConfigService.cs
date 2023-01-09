using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Match3.Scripts1.Puzzletown.Match3;
using Wooga.Coroutines;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;
using UnityEngine;
using Wooga.Foundation.Json;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x020007E1 RID: 2017
	public class M3ConfigService : AService
	{
		// Token: 0x060031C1 RID: 12737 RVA: 0x000EA912 File Offset: 0x000E8D12
		public M3ConfigService()
		{
			WooroutineRunner.StartCoroutine(this.InitRoutine(), null);
		}

		// Token: 0x170007CE RID: 1998
		// (get) Token: 0x060031C2 RID: 12738 RVA: 0x000EA928 File Offset: 0x000E8D28
		public int NumLevels
		{
			get
			{
				int last_area = this.configService.general.tier_unlocked.last_area;
				return this.configService.areas.areas.Take(last_area).Sum((Area area) => area.levels.Count);
			}
		}

		// Token: 0x060031C3 RID: 12739 RVA: 0x000EA984 File Offset: 0x000E8D84
		private IEnumerator InitRoutine()
		{
			yield return ServiceLocator.Instance.Inject(this);
			base.OnInitialized.Dispatch();
			yield break;
		}

		public Wooroutine<LevelConfig> GetLevelConfig(int area, int level, LevelPlayMode playMode, AreaConfig.Tier tier)
		{
			Debug.Log("<color=red>获取level config: </color>" + area + " " + level + " " + playMode + " " + tier);
			return WooroutineRunner.StartWooroutine<LevelConfig>(this.GetLevelConfigRoutine(area, level, playMode, tier));
		}

		// Token: 0x060031C5 RID: 12741 RVA: 0x000EA9B4 File Offset: 0x000E8DB4
		public PirateBreakoutSetConfig GetPirateBreakoutLevelConfig(int set, int level)
		{
			return this.sbsService.SbsConfig.piratebreakoutsets.sets.First((PirateBreakoutSetConfig c) => c.set == set && c.level == level);
		}

		// Token: 0x060031C6 RID: 12742 RVA: 0x000EA9FC File Offset: 0x000E8DFC
		private IEnumerator GetLevelConfigRoutine(int area, int level, LevelPlayMode playMode, AreaConfig.Tier tier)
		{
			bool isWeeklyEvent = playMode == LevelPlayMode.DiveForTreasure || playMode == LevelPlayMode.PirateBreakout;
			ALevelCollectionConfig collectionConfig;
			string levelName;
			string levelFileName;
			string alternateLevelFileName;
			string bundleName;
			if (playMode == LevelPlayMode.DiveForTreasure)
			{
				collectionConfig = this.GetDiveForTreasureLevelConfig(this.gameStateService.DiveForTreasure.Set, level);
				levelName = ((DiveForTreasureSetConfig)collectionConfig).file_name;
				levelFileName = this.GetDiveForTreasureLevelFileName(levelName, this.gameStateService.DiveForTreasure.Set);
				alternateLevelFileName = this.GetDiveForTreasureLevelFileName(levelName, this.gameStateService.DiveForTreasure.Set);
				DiveForTreasureSetConfig diveForTreasureSetConfig = (DiveForTreasureSetConfig)collectionConfig;
				collectionConfig.tiers = new TierConfig[3];
				TierConfig tierConfig = new TierConfig(diveForTreasureSetConfig.Tier, 0, diveForTreasureSetConfig.IsEasy);
				collectionConfig.tiers[0] = tierConfig;
				collectionConfig.tiers[1] = tierConfig;
				collectionConfig.tiers[2] = tierConfig;
				bundleName = "dive_for_treasure_level";
				tier = tierConfig.tier;
			}
			else if (playMode == LevelPlayMode.PirateBreakout)
			{
				collectionConfig = this.GetPirateBreakoutLevelConfig(this.gameStateService.PirateBreakout.Set, level);
				levelName = ((PirateBreakoutSetConfig)collectionConfig).file_name;
				levelFileName = this.GetPirateBreakoutLevelFileName(levelName, this.gameStateService.PirateBreakout.Set);
				alternateLevelFileName = this.GetPirateBreakoutLevelFileName(levelName, this.gameStateService.PirateBreakout.Set);
				PirateBreakoutSetConfig pirateBreakoutSetConfig = (PirateBreakoutSetConfig)collectionConfig;
				collectionConfig.tiers = new TierConfig[3];
				TierConfig tierConfig2 = new TierConfig(pirateBreakoutSetConfig.Tier, 0, pirateBreakoutSetConfig.IsEasy);
				collectionConfig.tiers[0] = tierConfig2;
				collectionConfig.tiers[1] = tierConfig2;
				collectionConfig.tiers[2] = tierConfig2;
				bundleName = "pirate_breakout_level";
				tier = tierConfig2.tier;
			}
			else
			{
				collectionConfig = this.GetAreaConfig(area, level);
				levelName = this.GetLevelName((AreaConfig)collectionConfig, tier);
				levelFileName = this.GetLevelFileName(area, ((AreaConfig)collectionConfig).file_name, tier, false);
				alternateLevelFileName = this.GetLevelFileName(area, ((AreaConfig)collectionConfig).file_name, tier, true);
				// eli key point 关卡配置文件
				bundleName = M3ConfigService.GetBundleName(area);
			}
			bool inTestGroup = this.sbsService.SbsConfig.feature_switches.enable_hard_levels;
			Wooroutine<TextAsset> ta = this.abs.LoadAsset<TextAsset>(bundleName, levelFileName);
			bool usingABLevel = false;
			TextAsset levelAsset = null;
			if (inTestGroup)
			{
				Wooroutine<TextAsset> taAlternate = this.abs.LoadAsset<TextAsset>(bundleName, alternateLevelFileName);
				yield return taAlternate;
				if (taAlternate.ReturnValue == null)
				{
					yield return ta;
					levelAsset = ta.ReturnValue;
				}
				else
				{
					usingABLevel = true;
					levelAsset = taAlternate.ReturnValue;
				}
			}
			else
			{
				yield return ta;
				levelAsset = ta.ReturnValue;
			}
			Debug.Log("GetLevelConfigRoutine:" + levelFileName);
			collectionConfig.isABLevel = usingABLevel;
			// eli key point 反序列化关卡文件json
			LevelConfig config = JSON.Deserialize<LevelConfig>(levelAsset.text);
			// Debug.LogError("levelasset.objective:" + config.LevelCollectionConfig.objective);
			config.LevelCollectionConfig = collectionConfig;
			// Debug.LogError("levelasset.objective:" + config.LevelCollectionConfig.objective);
			if (!isWeeklyEvent)
			{
				this.SetupRewardsForAbTest(config);
				config.IsCompleted = this.progression.IsCompleted(level);
			}
			else if (playMode == LevelPlayMode.DiveForTreasure)
			{
				bool flag = this.gameStateService.DiveForTreasure.IsRewardedLevel(config.Level.level);
				config.diveForTreasureRewards = new List<MaterialAmount>
				{
					new MaterialAmount("key", (!flag) ? 0 : 1, MaterialAmountUsage.Undefined, 0)
				};
				if (flag)
				{
					config.collectable = "key";
				}
				config.IsCompleted = false;
				config.levelSet = new int?(this.gameStateService.DiveForTreasure.Set);
			}
			else if (playMode == LevelPlayMode.PirateBreakout)
			{
				bool flag2 = this.gameStateService.PirateBreakout.IsRewardedLevel(config.Level.level);
				config.pirateBreakoutRewards = new List<MaterialAmount>
				{
					new MaterialAmount("key", (!flag2) ? 0 : 1, MaterialAmountUsage.Undefined, 0)
				};
				if (flag2)
				{
					config.collectable = "key";
				}
				config.IsCompleted = false;
				config.levelSet = new int?(this.gameStateService.PirateBreakout.Set);
			}
			config.UpdateObjectives();
			config.SelectedTier = tier;
			config.coinMultiplier = this.configService.general.tier_factor[(int)tier].coin_multiplier;
			// config.tournamentMultiplier = this.tournamentService.GetCurrentScoreMultiplierForTier((int)tier);
			// config.seasonalRewards = this.seasonService.GetEarnedMaterialForTier((int)tier);
			// if (this.questService.OnInitialized.WasDispatched)
			// {
				// QuestData currentQuestData = this.questService.questManager.CurrentQuestData;
				// if (currentQuestData != null)
				// {
				// 	for (int i = 0; i < currentQuestData.Tasks.Count; i++)
				// 	{
				// 		QuestTaskData questTaskData = currentQuestData.Tasks[i];
				// 		if (questTaskData.levels.Contains(levelName))
				// 		{
				// 			config.collectable = questTaskData.item;
				// 			break;
				// 		}
				// 	}
				// }
			// }
			// else
			// {
				// WoogaDebug.LogWarning(new object[]
				// {
					// "Warning: QuestService not initialized"
				// });
			// }
			if (config.IsCompleted)
			{
				config.coinMultiplier = 1;
				config.tournamentMultiplier = 1;
			}
			yield return config;
			yield break;
		}

		// Token: 0x060031C7 RID: 12743 RVA: 0x000EAA34 File Offset: 0x000E8E34
		public int GetFirstLevelOfArea(int area)
		{
			return this.configService.areas.areas[area - 1].levels.First<AreaConfig>().level;
		}

		// Token: 0x060031C8 RID: 12744 RVA: 0x000EAA5D File Offset: 0x000E8E5D
		public int GetAreaForLevel(int level)
		{
			return this.configService.areas.AreaForLevel(level);
		}

		// Token: 0x060031C9 RID: 12745 RVA: 0x000EAA70 File Offset: 0x000E8E70
		public string GetTierName(int tier)
		{
			string key = "ui.stage.easy";
			if (tier == 1)
			{
				key = "ui.stage.medium";
			}
			if (tier == 2)
			{
				key = "ui.stage.hard";
			}
			return this.locaService.GetText(key, new LocaParam[0]);
		}

		// Token: 0x060031CA RID: 12746 RVA: 0x000EAAB0 File Offset: 0x000E8EB0
		public bool IsLockedByQuest(int level)
		{
			AreaConfig areaConfig = this.GetAreaConfig(this.GetAreaForLevel(level), level);
			return this.IsLockedByQuest(areaConfig);
		}

		// Token: 0x060031CB RID: 12747 RVA: 0x000EAAD3 File Offset: 0x000E8ED3
		public bool IsLockedByQuest(AreaConfig level)
		{
			// return !level.unlocked_at_quest_completed.IsNullOrEmpty() && !this.questService.IsCollected(level.unlocked_at_quest_completed);
			return false;
		}

		// Token: 0x060031CC RID: 12748 RVA: 0x000EAAFC File Offset: 0x000E8EFC
		private AreaConfig GetAreaConfig(int area, int level)
		{
			return this.configService.areas.areas[area - 1].levels.First((AreaConfig c) => c.level == level);
		}

		// Token: 0x060031CD RID: 12749 RVA: 0x000EAB44 File Offset: 0x000E8F44
		private DiveForTreasureSetConfig GetDiveForTreasureLevelConfig(int set, int level)
		{
			return this.sbsService.SbsConfig.divefortreasuresets.sets.First((DiveForTreasureSetConfig c) => c.set == set && c.level == level);
		}

		// eli key point 获取关卡文件名
		private string GetLevelFileName(int area, string fileName, AreaConfig.Tier tier, bool alternate = false)
		{
			if (alternate)
			{
				return string.Format("Assets/Puzzletown/Match3/Levels/Alternate/Area {0}/{1}{2}.json", area, fileName, tier.ToString());
			}
			return string.Format("Assets/Puzzletown/Match3/Levels/Area {0}/{1}{2}.json", area, fileName, tier.ToString());
		}

		// Token: 0x060031CF RID: 12751 RVA: 0x000EABDD File Offset: 0x000E8FDD
		private string GetLevelName(AreaConfig areaConfig, AreaConfig.Tier tier)
		{
			return areaConfig.level + tier.ToString();
		}

		// Token: 0x060031D0 RID: 12752 RVA: 0x000EABFC File Offset: 0x000E8FFC
		private static string GetBundleName(int area)
		{
			return string.Format("levels_area_{0}", area);
		}

		// Token: 0x060031D1 RID: 12753 RVA: 0x000EAC0E File Offset: 0x000E900E
		private string GetDiveForTreasureLevelFileName(string levelName, int set)
		{
			return string.Format("Assets/Puzzletown/Match3/Levels/DiveForTreasure/Set {0}/{1}.json", set, levelName);
		}

		// Token: 0x060031D2 RID: 12754 RVA: 0x000EAC21 File Offset: 0x000E9021
		private string GetPirateBreakoutLevelFileName(string levelName, int set)
		{
			return string.Format("Assets/Puzzletown/Match3/Levels/PirateBreakout/Set {0}/{1}.json", set, levelName);
		}

		// Token: 0x060031D3 RID: 12755 RVA: 0x000EAC34 File Offset: 0x000E9034
		private void SetupRewardsForAbTest(LevelConfig config)
		{
			string installVersion = this.gameStateService.InstallVersion;
			if (installVersion.IsNullOrEmpty())
			{
				return;
			}
			Puzzletown.Build.Version version = new Puzzletown.Build.Version(installVersion);
			if (version.major == 0 && version.minor < 28)
			{
				return;
			}
			TierConfig[] tiers = config.LevelCollectionConfig.tiers;
			for (int i = 0; i < tiers.Length; i++)
			{
				tiers[i].diamonds = this.sbsService.SbsConfig.level_rewards.rewards[i];
			}
		}

		// Token: 0x04005A5C RID: 23132
		[WaitForService(true, true)]
		private ConfigService configService;

		// Token: 0x04005A5D RID: 23133
		[WaitForService(true, true)]
		private GameStateService gameStateService;

		// Token: 0x04005A5E RID: 23134
		[WaitForService(true, true)]
		private AssetBundleService abs;

		// Token: 0x04005A5F RID: 23135
		[WaitForService(true, true)]
		private ILocalizationService locaService;

		// Token: 0x04005A60 RID: 23136
		[WaitForService(false, false)]
		private ProgressionDataService.Service progression;

		// Token: 0x04005A61 RID: 23137
		// [WaitForService(false, false)]
		// private QuestService questService;

		// Token: 0x04005A62 RID: 23138
		// [WaitForService(false, false)]
		// private TournamentService tournamentService;

		// Token: 0x04005A63 RID: 23139
		// [WaitForService(false, false)]
		// private SeasonService seasonService;

		// Token: 0x04005A64 RID: 23140
		[WaitForService(true, true)]
		private SBSService sbsService;

		// Token: 0x04005A65 RID: 23141
		public static int AREA_NOT_SET = -1;
	}
}
