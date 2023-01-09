using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Match3.Scripts1.Puzzletown.Config;
using Match3.Scripts1.Puzzletown.Match3;
using Match3.Scripts1.Puzzletown.Town;
using Wooga.Coroutines;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x02000745 RID: 1861
	public class AssetBundlePreloadService : AService
	{
		// Token: 0x06002E06 RID: 11782 RVA: 0x000D665B File Offset: 0x000D4A5B
		public AssetBundlePreloadService()
		{
			WooroutineRunner.StartCoroutine(this.InitRoutine(), null);
		}

		// Token: 0x06002E07 RID: 11783 RVA: 0x000D6670 File Offset: 0x000D4A70
		private IEnumerator InitRoutine()
		{
			yield return ServiceLocator.Instance.Inject(this);
			this.preloader = new AssetBundlePreloader(this.abs);
			this.AddAudioBundle();
			this.AddSeasonalBundle();
			this.ConfigureChapterIntros();
			this.ConfigureQuestIllustrations();
			this.ConfigureChallengeBundle();
			this.AddAdventureIslandBundles();
			this.AddDiveForTreasureBundle();
			this.AddPirateBreakoutBundle();
			this.AddAlphabetBundle();
			this.AddFriendDecoBundle();
			base.OnInitialized.Dispatch();
			yield break;
		}

		// Token: 0x06002E08 RID: 11784 RVA: 0x000D668C File Offset: 0x000D4A8C
		private void AddAudioBundle()
		{
			this.preloader.Preload(new List<string>
			{
				"audio"
			});
		}

		// Token: 0x06002E09 RID: 11785 RVA: 0x000D66B8 File Offset: 0x000D4AB8
		private void ConfigureChapterIntros()
		{
			foreach (ChapterData chapterData in this.configs.chapter.chapters)
			{
				// UnlockedLevelPreloadTrigger item = new UnlockedLevelPreloadTrigger("chapter_intro_" + (chapterData.chapter + 1), chapterData.first_level, this.quests);
				// this.preloader.triggers.Add(item);
			}
		}

		// Token: 0x06002E0A RID: 11786 RVA: 0x000D6728 File Offset: 0x000D4B28
		private void ConfigureQuestIllustrations()
		{
			// foreach (ChapterData chapterData in this.configs.chapter.chapters)
			// {
			// 	UnlockedLevelPreloadTrigger item = new UnlockedLevelPreloadTrigger("quest_illustrations_chapter_" + (chapterData.chapter + 1), chapterData.first_level, this.quests);
			// 	this.preloader.triggers.Add(item);
			// }
		}

		// Token: 0x06002E0B RID: 11787 RVA: 0x000D6798 File Offset: 0x000D4B98
		private void ConfigureChallengeBundle()
		{
			// UnlockedLevelPreloadTrigger item = new UnlockedLevelPreloadTrigger("buildings_challenges_2018", this.challengeService.Balancing.play_minimum_level - 10, this.quests);
			// this.preloader.triggers.Add(item);
		}

		// Token: 0x06002E0C RID: 11788 RVA: 0x000D67DC File Offset: 0x000D4BDC
		private void AddAdventureIslandBundles()
		{
			int num = 0;
			for (int i = 0; i < this.configs.areas.areas.Count; i++)
			{
				// 审核版本只保留前5个区域
				// #if REVIEW_VERSION
				// if (i > 4)
				// {
				// 	continue;
				// }
				// #endif
				int num2 = this.configs.SbsConfig.islandareaconfig.IslandForArea(i + 1);
				if (num2 != num)
				{
					this.AddNewIslandBundlePreload(num2, this.configs.areas.areas[i]);
					num = num2;
				}
			}
		}

		// Token: 0x06002E0D RID: 11789 RVA: 0x000D6850 File Offset: 0x000D4C50
		private void AddNewIslandBundlePreload(int islandID, Area area)
		{
			if (area != null && area.levels != null && area.levels.Count > 0)
			{
				int level = area.levels[0].level;
				int level2 = Math.Max(1, level - 50);
				string sceneBundleName = SceneManager.Instance.GetSceneBundleName(TownMainRoot.GetSceneNameForIsland(islandID));
				// this.preloader.triggers.Add(new UnlockedLevelPreloadTrigger(sceneBundleName, level2, this.quests));
				string bundleName = string.Format("buildings_adventure_island_{0:00}", islandID);
				// this.preloader.triggers.Add(new UnlockedLevelPreloadTrigger(bundleName, level2, this.quests));
			}
		}

		// Token: 0x06002E0E RID: 11790 RVA: 0x000D68F8 File Offset: 0x000D4CF8
		private void AddDiveForTreasureBundle()
		{
			// this.preloader.triggers.Add(new UnlockedLevelPreloadTrigger("scene_dive_for_treasure", this.state.DiveForTreasure.UnlockLevel() - 15, this.quests));
			// this.preloader.triggers.Add(new UnlockedLevelPreloadTrigger("dive_for_treasure_level", this.state.DiveForTreasure.UnlockLevel() - 15, this.quests));
		}

		// Token: 0x06002E0F RID: 11791 RVA: 0x000D696C File Offset: 0x000D4D6C
		private void AddPirateBreakoutBundle()
		{
			// this.preloader.triggers.Add(new UnlockedLevelPreloadTrigger("pirate_breakout_level", this.state.PirateBreakout.UnlockLevel() - 15, this.quests));
			// this.preloader.triggers.Add(new UnlockedLevelPreloadTrigger("scene_pirate_breakout", this.state.PirateBreakout.UnlockLevel() - 15, this.quests));
			// this.preloader.triggers.Add(new UnlockedLevelPreloadTrigger("pirate_breakout_assets", this.state.PirateBreakout.UnlockLevel() - 15, this.quests));
		}

		// Token: 0x06002E10 RID: 11792 RVA: 0x000D6A14 File Offset: 0x000D4E14
		private void AddAlphabetBundle()
		{
			this.preloader.Preload(new List<string>
			{
				this.configs.SbsConfig.promo_popup.AlphabetAssetBundleName
			});
		}

		// Token: 0x06002E11 RID: 11793 RVA: 0x000D6A50 File Offset: 0x000D4E50
		private void AddFriendDecoBundle()
		{
			this.preloader.Preload(new List<string>
			{
				"buildings_friendship_2018"
			});
		}

		// Token: 0x06002E12 RID: 11794 RVA: 0x000D6A7C File Offset: 0x000D4E7C
		private void AddSeasonalBundle()
		{
			// SeasonConfig activeSeason = this.seasonsService.GetActiveSeason();
			// if (activeSeason != null)
			// {
				// IEnumerable<string> bundleNames = activeSeason.BundleNames;
				// foreach (string bundleName in bundleNames)
				// {
					// this.preloader.triggers.Add(new UnlockedLevelPreloadTrigger(bundleName, 1, this.quests));
				// }
			// }
			// SeasonPrizeInfo previousSeasonPrizeInfo = this.seasonsService.GetPreviousSeasonPrizeInfo();
			// if (previousSeasonPrizeInfo != null)
			// {
				// IEnumerable<string> bundleNames2 = previousSeasonPrizeInfo.BundleNames;
				// foreach (string bundleName2 in bundleNames2)
				// {
					// this.preloader.triggers.Add(new UnlockedLevelPreloadTrigger(bundleName2, 1, this.quests));
				// }
			// }
		}

		// Token: 0x06002E13 RID: 11795 RVA: 0x000D6B80 File Offset: 0x000D4F80
		public void Preload(IEnumerable<string> specificBundlesToPreload = null)
		{
			this.preloader.Preload(specificBundlesToPreload);
		}

		// Token: 0x06002E14 RID: 11796 RVA: 0x000D6B90 File Offset: 0x000D4F90
		public void TryStartPreloadNewAreaBundles(BuildingToBundleMap buildingToBundleMap)
		{
			if (this.state.Progression == null)
			{
				return;
			}
			int num = this.state.Progression.LastUnlockedArea + 1;
			IEnumerable<string> bundlesForArea = this.GetBundlesForArea(num + 1, buildingToBundleMap);
			if (bundlesForArea != null)
			{
				WoogaDebug.Log(new object[]
				{
					"AssetBundlePreloadService: Trying to preload: ",
					bundlesForArea
				});
				this.preloader.Preload(bundlesForArea);
			}
		}

		// Token: 0x06002E15 RID: 11797 RVA: 0x000D6BF8 File Offset: 0x000D4FF8
		public void TryStartPreloadMatch3Themes()
		{
			if (this.state.Progression == null)
			{
				return;
			}
			string path = Path.Combine("SbsConfigService", "levelforeshadowingconfig");
			string text = Resources.Load<TextAsset>(path).text;
			LevelForeshadowingConfig levelForeshadowingConfig = JsonUtility.FromJson<LevelForeshadowingConfig>(text);
			int unlockedLevel = this.state.Progression.UnlockedLevel;
			HashSet<string> hashSet = new HashSet<string>();
			foreach (LevelForeshadowingConfig.ForeshadowingLevelConfig foreshadowingLevelConfig in levelForeshadowingConfig.level_config)
			{
				int level = foreshadowingLevelConfig.level;
				if (foreshadowingLevelConfig.type.Equals("level_goal") && level - 50 < unlockedLevel)
				{
					string feature = foreshadowingLevelConfig.feature;
					bool flag = LevelTheme.THEMES_FIRST_BUNDLE.Contains(feature);
					string item = (!flag) ? string.Format("m3_themes_{0}", feature) : "m3_themes";
					if (!hashSet.Contains(item))
					{
						hashSet.Add(item);
					}
				}
			}
			if (hashSet.Count > 0)
			{
				this.preloader.Preload(hashSet);
			}
		}

		// Token: 0x06002E16 RID: 11798 RVA: 0x000D6D30 File Offset: 0x000D5130
		protected IEnumerable<string> GetBundlesForArea(int area, BuildingToBundleMap buildingToBundleMap)
		{
			HashSet<string> hashSet = new HashSet<string>();
			foreach (BuildingConfig buildingConfig in this.configs.buildingConfigList.buildings)
			{
				BuildingToBundleMap.Entry entry;
				if (!buildingConfig.IsSeasonal() && this.configs.SbsConfig.islandareaconfig.GetAreaForChapter(buildingConfig.chapter_id) == area && buildingToBundleMap.Map.TryGetValue(buildingConfig.name, out entry))
				{
					if (!entry.prefabBundle.IsNullOrEmpty())
					{
						hashSet.Add(entry.prefabBundle);
					}
					if (!entry.destroyedBundle.IsNullOrEmpty())
					{
						hashSet.Add(entry.prefabBundle);
					}
					if (!entry.iconBundle.IsNullOrEmpty())
					{
						hashSet.Add(entry.iconBundle);
					}
				}
			}
			return hashSet;
		}

		// Token: 0x04005783 RID: 22403
		public const int ISLAND_PRELOAD_BUFFER_LEVEL_COUNT = 50;

		// Token: 0x04005784 RID: 22404
		[WaitForService(true, true)]
		private GameStateService state;

		// Token: 0x04005785 RID: 22405
		[WaitForService(true, true)]
		private ConfigService configs;

		// Token: 0x04005786 RID: 22406
		// [WaitForService(true, true)]
		// private SeasonService seasonsService;

		// Token: 0x04005787 RID: 22407
		[WaitForService(false, true)]
		private SBSService sbsService;

		// Token: 0x04005788 RID: 22408
		// [WaitForService(true, true)]
		// private QuestService quests;

		// Token: 0x04005789 RID: 22409
		// [WaitForService(true, true)]
		// private ChallengeService challengeService;

		// Token: 0x0400578A RID: 22410
		[WaitForService(true, true)]
		private AssetBundleService abs;

		// Token: 0x0400578B RID: 22411
		private AssetBundlePreloader preloader;
	}
}
