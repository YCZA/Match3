using System.Collections;
using System.Collections.Generic;
using Match3.Scripts1.Puzzletown.Match3;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Puzzletown.Town;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.Signals;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;

// Token: 0x02000937 RID: 2359
namespace Match3.Scripts1
{
	public class QuestService : AService
	{
		// Token: 0x0600394F RID: 14671 RVA: 0x0011A370 File Offset: 0x00118770
		public QuestService()
		{
			WooroutineRunner.StartCoroutine(this.LoadRoutine(), null);
		}

		// Token: 0x170008D3 RID: 2259
		// (get) Token: 0x06003950 RID: 14672 RVA: 0x0011A390 File Offset: 0x00118790
		// (set) Token: 0x06003951 RID: 14673 RVA: 0x0011A398 File Offset: 0x00118798
		public QuestManager questManager { get; private set; }

		// Token: 0x170008D4 RID: 2260
		// (get) Token: 0x06003952 RID: 14674 RVA: 0x0011A3A1 File Offset: 0x001187A1
		// (set) Token: 0x06003953 RID: 14675 RVA: 0x0011A3A9 File Offset: 0x001187A9
		public CollectTaskObserver CollectTask { get; protected set; }

		// Token: 0x170008D5 RID: 2261
		// (get) Token: 0x06003954 RID: 14676 RVA: 0x0011A3B2 File Offset: 0x001187B2
		// (set) Token: 0x06003955 RID: 14677 RVA: 0x0011A3BA File Offset: 0x001187BA
		public ReachTaskObserver ReachTask { get; protected set; }

		// Token: 0x170008D6 RID: 2262
		// (get) Token: 0x06003956 RID: 14678 RVA: 0x0011A3C3 File Offset: 0x001187C3
		// (set) Token: 0x06003957 RID: 14679 RVA: 0x0011A3CB File Offset: 0x001187CB
		public DecoLikeTaskObserver DecoLikeTask { get; protected set; }

		// Token: 0x170008D7 RID: 2263
		// (get) Token: 0x06003958 RID: 14680 RVA: 0x0011A3D4 File Offset: 0x001187D4
		// (set) Token: 0x06003959 RID: 14681 RVA: 0x0011A3DC File Offset: 0x001187DC
		public LevelComplete_CollectObserver LevelCompleteCollectTask { get; protected set; }

		// Token: 0x170008D8 RID: 2264
		// (get) Token: 0x0600395A RID: 14682 RVA: 0x0011A3E5 File Offset: 0x001187E5
		// (set) Token: 0x0600395B RID: 14683 RVA: 0x0011A3ED File Offset: 0x001187ED
		public LevelComplete_ReachObserver LevelCompleteReachTask { get; protected set; }

		// Token: 0x0600395C RID: 14684 RVA: 0x0011A3F6 File Offset: 0x001187F6
		public bool IsCollected(string questId)
		{
			return this.stateService.Quests.Quests.IsCollected(questId);
		}

		// Token: 0x0600395D RID: 14685 RVA: 0x0011A40E File Offset: 0x0011880E
		public bool IsCompleted(string questId)
		{
			return this.stateService.Quests.Quests.IsComplete(questId);
		}

		// Token: 0x0600395E RID: 14686 RVA: 0x0011A428 File Offset: 0x00118828
		private IEnumerator LoadRoutine()
		{
			yield return ServiceLocator.Instance.Inject(this);
			this.questManager = new QuestManager(this.sbsService.SbsConfig.questconfig.quests, this.stateService.Quests, this);
			VillageRankHarmonyObserver harmonyObserver = new VillageRankHarmonyObserver(this.sbsService.SbsConfig.villagerankconfig, this.stateService.Resources);
			this.questManager.RegisterQuestTaskHandler(new CollectTaskHandler(this.questManager));
			this.questManager.RegisterQuestTaskHandler(new ReachTaskHandler(this.questManager));
			this.questManager.RegisterQuestTaskHandler(new ReachRankTaskHandler(this.questManager, harmonyObserver));
			this.questManager.RegisterQuestTaskHandler(new BuildingUpgradeTaskHandler(this.questManager));
			this.questManager.RegisterQuestTaskHandler(new DecoLikeTaskHandlerHandler(this.questManager, this.configService.villager));
			this.questManager.RegisterQuestTaskHandler(new CompleteTierReachTaskHandler(this.questManager));
			this.questManager.RegisterQuestTaskHandler(new CompleteTierCollectTaskHandler(this.questManager, this.progressionService));
			this.questManager.RegisterQuestTaskHandler(new CollectAndInteractTaskHandler(this.questManager, this.progressionService));
			this.questManager.RegisterQuestTaskHandler(new CollectAndRepairTaskHandler(this.questManager, this.progressionService));
			this.CollectTask = new CollectTaskObserver(this.questManager, this.stateService.Resources);
			this.ReachTask = new ReachTaskObserver(this.questManager, this.stateService.Resources);
			this.DecoLikeTask = new DecoLikeTaskObserver(this.questManager);
			this.LevelCompleteCollectTask = new LevelComplete_CollectObserver(this.questManager, this.progressionService);
			this.LevelCompleteReachTask = new LevelComplete_ReachObserver(this.questManager, this.progressionService);
			this.questManager.OnQuestCollected.AddListener(delegate(QuestProgress progress)
			{
				QuestData configData = progress.configData;
				this.stateService.Resources.AddMaterial(configData.rewardItem, configData.rewardCount, true, "章节任务");
				this.OnQuestCollected.Dispatch(progress);
			});
			base.OnInitialized.Dispatch();
			yield return null;
			yield break;
		}

		// Token: 0x0600395F RID: 14687 RVA: 0x0011A444 File Offset: 0x00118844
		public int GetNumberOpenTasks()
		{
			if (this.questManager.CurrentQuestProgress == null || this.questManager.CurrentQuestProgress.tasksProgress == null)
			{
				return 0;
			}
			int num = 0;
			for (int i = 0; i < this.questManager.CurrentQuestProgress.tasksProgress.Length; i++)
			{
				if (!this.questManager.GetTaskComplete(this.questManager.CurrentQuestProgress, i))
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06003960 RID: 14688 RVA: 0x0011A4C0 File Offset: 0x001188C0
		public Wooroutine<bool> IsIslandBundleForQuestCompletionAvailable(QuestProgress progress, int nextQuestFirstLevel = -1)
		{
			int level = progress.configData.level;
			int globalArea = this.configService.areas.AreaForLevel(level);
			List<int> list = new List<int>
			{
				this.configService.SbsConfig.islandareaconfig.IslandForArea(globalArea)
			};
			if (nextQuestFirstLevel > -1)
			{
				int globalArea2 = this.configService.areas.AreaForLevel(nextQuestFirstLevel);
				int num = this.configService.SbsConfig.islandareaconfig.IslandForArea(globalArea2);
				if (num != list[0])
				{
					list.Add(num);
				}
			}
			List<string> list2 = new List<string>();
			foreach (int island in list)
			{
				string sceneBundleName = SceneManager.Instance.GetSceneBundleName(TownMainRoot.GetSceneNameForIsland(island));
				list2.Add(sceneBundleName);
			}
			return this.assetBundleService.AreAllBundlesAvailable(list2);
		}

		// Token: 0x06003961 RID: 14689 RVA: 0x0011A5D0 File Offset: 0x001189D0
		public bool HasOpenVRTask()
		{
			QuestData currentQuestData = this.questManager.CurrentQuestData;
			QuestProgress currentQuestProgress = this.questManager.CurrentQuestProgress;
			if (currentQuestData == null || currentQuestProgress == null)
			{
				return false;
			}
			string b = QuestTaskType.reach_rank.ToString();
			for (int i = 0; i < currentQuestProgress.tasksProgress.Length; i++)
			{
				if (currentQuestData.task_type[i] == b && !currentQuestProgress.tasksProgress[i].collected)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003962 RID: 14690 RVA: 0x0011A65C File Offset: 0x00118A5C
		public bool HasNotification()
		{
			QuestProgress currentQuestProgress = this.questManager.CurrentQuestProgress;
			if (currentQuestProgress == null)
			{
				return false;
			}
			int num = 0;
			foreach (QuestTaskData taskData in currentQuestProgress.configData.Tasks)
			{
				bool taskComplete = this.questManager.GetTaskComplete(currentQuestProgress, num);
				QuestDataProgress questDataProgress = new QuestDataProgress(taskData, this.questManager.CurrentQuestProgress, taskComplete);
				if (questDataProgress.isComplete && !questDataProgress.IsCollected)
				{
					return true;
				}
				num++;
			}
			return false;
		}

		// Token: 0x06003963 RID: 14691 RVA: 0x0011A718 File Offset: 0x00118B18
		public void CollectAllQuestsForLevel(int unlockLevel)
		{
			IEnumerable<AreaConfig> allLevels = this.configService.areas.AllLevels;
			foreach (AreaConfig areaConfig in allLevels)
			{
				if (areaConfig.level > unlockLevel)
				{
					break;
				}
				if (!areaConfig.unlocked_at_quest_completed.IsNullOrEmpty())
				{
					this.stateService.Quests.Quests.SetCollected(areaConfig.unlocked_at_quest_completed);
				}
			}
			if (this.questManager.CurrentQuestData == null)
			{
				this.questManager.TryStartNextQuest();
			}
		}

		// Token: 0x06003964 RID: 14692 RVA: 0x0011A7D0 File Offset: 0x00118BD0
		public bool IsIslandFirstQuestUnlocked(int islandId)
		{
			int num = this.configService.SbsConfig.islandareaconfig.FirstGlobalArea(islandId);
			if (num > this.configService.general.tier_unlocked.last_area)
			{
				return false;
			}
			int firstLevelOfArea = this.m3ConfigService.GetFirstLevelOfArea(num);
			string id = this.QuestForLevel(firstLevelOfArea).id;
			return this.IsCollected(id) || (this.questManager.CurrentQuestData != null && this.questManager.CurrentQuestData.id == id);
		}

		// Token: 0x06003965 RID: 14693 RVA: 0x0011A864 File Offset: 0x00118C64
		public bool IsLevelUnlocked(int levelIndex)
		{
			if (levelIndex > this.progressionService.UnlockedLevel)
			{
				return false;
			}
			int num = this.configService.areas.AreaIndexForLevel(levelIndex);
			if (num == -1)
			{
				return false;
			}
			List<AreaConfig> levels = this.configService.areas.areas[num].levels;
			AreaConfig areaConfig = levels.Find((AreaConfig l) => l.level == levelIndex);
			if (areaConfig == null)
			{
				WoogaDebug.LogWarning(new object[]
				{
					"Could not find level",
					this.progressionService.UnlockedLevel
				});
				return false;
			}
			bool flag = true;
			string unlocked_at_quest_completed = areaConfig.unlocked_at_quest_completed;
			if (!unlocked_at_quest_completed.IsNullOrEmpty())
			{
				flag = this.IsCollected(unlocked_at_quest_completed);
				if (!areaConfig.wait_for_dialog.IsNullOrEmpty() && this.progressionService.UnlockedLevel <= areaConfig.level)
				{
					flag &= this.stateService.Quests.Quests.completedQuestDialogs.Contains(areaConfig.wait_for_dialog);
				}
			}
			return flag;
		}

		// Token: 0x06003966 RID: 14694 RVA: 0x0011A984 File Offset: 0x00118D84
		public QuestData QuestForLevel(int levelNumber)
		{
			if (levelNumber >= this.contentUnlockService.EndOfContentLevel())
			{
				return new QuestData
				{
					id = "end_of_content",
					level = this.contentUnlockService.EndOfContentLevel(),
					task_count = new int[1]
				};
			}
			int num = QuestConfig.QuestIndexForLevel(levelNumber);
			return this.sbsService.SbsConfig.questconfig.quests[num];
		}

		// eli key point 获取任务页面的每一页
		public List<QuestData> GetQuestListWithChapterIntro()
		{
			IEnumerable<QuestData> questConfigs = this.questManager.QuestConfigs;
			List<QuestData> list = new List<QuestData>();
			int last_chapter = 0;
			foreach (QuestData questData in questConfigs)
			{
				int chapter_id = this.config.chapter.ChapterForLevel(questData.level) - 1;
				int chapter = this.config.chapter.chapters[chapter_id].chapter;
				if (chapter > last_chapter)
				{
					// 添加每章任务的第一页(intro)
// #if REVIEW_VERSION
					// 审核关卡不添加intro页面
// #else
					list.Add(this.QuestForChapterIntro(questData.level, chapter));
// #endif
					last_chapter = chapter;
				}
				list.Add(questData);
			}
			return list;
		}

		// Token: 0x06003968 RID: 14696 RVA: 0x0011AAB4 File Offset: 0x00118EB4
		private QuestData QuestForChapterIntro(int levelNumber, int chapter)
		{
			return new QuestData
			{
				id = "chapter_intro",
				level = levelNumber,
				task_count = new int[1]
			};
		}

		// Token: 0x170008D9 RID: 2265
		// (get) Token: 0x06003969 RID: 14697 RVA: 0x0011AAE8 File Offset: 0x00118EE8
		private int UnlockedLevelWithQuest
		{
			get
			{
				int num = this.progressionService.UnlockedLevel;
				while (!this.IsLevelUnlocked(num))
				{
					num--;
				}
				return num;
			}
		}

		// Token: 0x170008DA RID: 2266
		// (get) Token: 0x0600396A RID: 14698 RVA: 0x0011AB18 File Offset: 0x00118F18
		public int UnlockedLevelWithQuestAndEndOfContent
		{
			get
			{
				int unlockedLevelWithQuest = this.UnlockedLevelWithQuest;
				int num = this.configService.areas.AreaForLevel(unlockedLevelWithQuest);
				int last_area = this.configService.general.tier_unlocked.last_area;
				bool flag = num >= last_area || num == 0;
				return (!flag) ? unlockedLevelWithQuest : (unlockedLevelWithQuest - 1);
			}
		}

		// Token: 0x170008DB RID: 2267
		// (get) Token: 0x0600396B RID: 14699 RVA: 0x0011AB74 File Offset: 0x00118F74
		public int UnlockedAreaWithQuestAndEndOfContent
		{
			get
			{
				int num = this.configService.areas.AreaForLevel(this.UnlockedLevelWithQuest);
				int last_area = this.configService.general.tier_unlocked.last_area;
				bool flag = num >= last_area || num == 0;
				return (!flag) ? num : last_area;
			}
		}

		// Token: 0x170008DC RID: 2268
		// (get) Token: 0x0600396C RID: 14700 RVA: 0x0011ABCC File Offset: 0x00118FCC
		public int Chapter
		{
			get
			{
				int unlockedLevelWithQuest = this.UnlockedLevelWithQuest;
				ChapterData[] chapters = this.configService.chapter.chapters;
				for (int i = 1; i < chapters.Length; i++)
				{
					if (unlockedLevelWithQuest < chapters[i].first_level)
					{
						return i - 1;
					}
				}
				return chapters.Length - 1;
			}
		}

		// Token: 0x040061A7 RID: 24999
		[WaitForService(true, true)]
		public ConfigService configService;

		// Token: 0x040061A8 RID: 25000
		[WaitForService(true, true)]
		public ILocalizationService localizationService;

		// Token: 0x040061A9 RID: 25001
		[WaitForService(true, true)]
		private GameStateService stateService;

		// Token: 0x040061AA RID: 25002
		[WaitForService(true, true)]
		private ProgressionDataService.Service progressionService;

		// Token: 0x040061AB RID: 25003
		[WaitForService(true, true)]
		private AssetBundleService assetBundleService;

		// Token: 0x040061AC RID: 25004
		[WaitForService(true, true)]
		private ContentUnlockService contentUnlockService;

		// Token: 0x040061AD RID: 25005
		[WaitForService(true, true)]
		private M3ConfigService m3ConfigService;

		// Token: 0x040061AE RID: 25006
		[WaitForService(true, true)]
		private SBSService sbsService;

		// Token: 0x040061AF RID: 25007
		[WaitForService(true, true)]
		private ConfigService config;

		// Token: 0x040061B6 RID: 25014
		public readonly Signal<QuestProgress> OnQuestCollected = new Signal<QuestProgress>();
	}
}
