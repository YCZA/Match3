using System.Collections;
using System.Linq;
using Match3.Scripts1.Puzzletown.Flows;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Puzzletown.Town;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.Services.ErrorAnalytics;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown
{
	// Token: 0x020004C9 RID: 1225
	public class StartNewContentFlow : IBlocker
	{
		// Token: 0x06002243 RID: 8771 RVA: 0x00095D26 File Offset: 0x00094126
		public StartNewContentFlow(bool forceNewAreaBanner = false, bool allowPleaseGoOnline = true)
		{
			this.forceNewAreaBanner = forceNewAreaBanner;
			this.allowPleaseGoOnline = allowPleaseGoOnline;
		}

		// Token: 0x17000542 RID: 1346
		// (get) Token: 0x06002244 RID: 8772 RVA: 0x00095D3C File Offset: 0x0009413C
		public bool BlockInput
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06002245 RID: 8773 RVA: 0x00095D40 File Offset: 0x00094140
		private IEnumerator IsMissingHighestUnlockedIsland()
		{
			int islandId = this.configs.SbsConfig.islandareaconfig.IslandForArea(this.quests.UnlockedAreaWithQuestAndEndOfContent);
			string bundleName = SceneManager.Instance.GetSceneBundleName(TownMainRoot.GetSceneNameForIsland(islandId));
			Wooroutine<bool> isAvailable = this.abs.IsBundleAvailable(bundleName);
			yield return isAvailable;
			yield return !isAvailable.ReturnValue;
			yield break;
		}

		// Token: 0x06002246 RID: 8774 RVA: 0x00095D5C File Offset: 0x0009415C
		private IEnumerator ShowPleaseGetOnlinePopupRoutine()
		{
			StartNewContentFlow.hasSeenMissingContentWarning = true;
			int islandId = this.configs.SbsConfig.islandareaconfig.IslandForArea(this.quests.UnlockedAreaWithQuestAndEndOfContent);
			string bundleName = SceneManager.Instance.GetSceneBundleName(TownMainRoot.GetSceneNameForIsland(islandId));
			yield return PopupMissingAssetsRoot.TryShowRoutine("islandBundle " + bundleName);
			yield break;
		}

		// Token: 0x06002247 RID: 8775 RVA: 0x00095D78 File Offset: 0x00094178
		public IEnumerator ExecuteRoutine()
		{
			yield return ServiceLocator.Instance.Inject(this);
			yield return SceneManager.Instance.Inject(this);
			if (this.TryFixQuestBug() || !this.HasNewQuestForUnlock())
			{
				yield break;
			}
			Wooroutine<bool> isMissingHighestUnlockedIsland = WooroutineRunner.StartWooroutine<bool>(this.IsMissingHighestUnlockedIsland());
			yield return isMissingHighestUnlockedIsland;
			if (isMissingHighestUnlockedIsland.ReturnValue)
			{
				if (!StartNewContentFlow.hasSeenMissingContentWarning && this.allowPleaseGoOnline)
				{
					yield return this.ShowPleaseGetOnlinePopupRoutine();
				}
				yield break;
			}
			this.townMain.uiRoot.RefreshNewBuildingIndicator();
			this.quests.questManager.TryStartNextQuest();
			yield return this.CenterOnNewArea();
			this.townMain.EnableUserInput();
			int currentIsland = this.gameState.Buildings.CurrentIsland;
			StoryController storyController = this.villagerController.storyController;
			Debug.Log("quest_start, questDataId:" + this.quests.questManager.CurrentQuestData.id);
			yield return storyController.StartAndAwaitStoryFlow(new BlockerManager(), DialogueTrigger.quest_start, this.quests.questManager.CurrentQuestData.id);
			if (currentIsland != this.gameState.Buildings.CurrentIsland)
			{
				yield return ServiceLocator.Instance.Inject(this);
				yield return SceneManager.Instance.Inject(this);
			}
			if (this.townMain != null)
			{
				this.townMain.DisableUserInput();
			}
			if (this.townMain != null)
			{
				// 进入新的一章时，原游戏会显示章节介绍
				// #if !REVIEW_VERSION
					yield return this.ShowChapterIntro();
				// #endif
			}
			if (this.townMain != null)
			{
				this.townMain.EnableUserInput();
			}
		}

		// Token: 0x06002248 RID: 8776 RVA: 0x00095D94 File Offset: 0x00094194
		private bool TryFixQuestBug()
		{
			if (!this.IsCurrentQuestCollected())
			{
				return false;
			}
			Log.Warning("FixGameState", "Player was stuck due to no active quest bug.", null);
			QuestProgress currentQuestProgress = this.quests.questManager.CurrentQuestProgress;
			currentQuestProgress.status = QuestProgress.Status.completed;
			BlockerManager.global.Append(new QuestCompleteFlow(currentQuestProgress.configData, false));
			this.quests.questManager.CollectQuest(currentQuestProgress);
			return true;
		}

		// Token: 0x06002249 RID: 8777 RVA: 0x00095E00 File Offset: 0x00094200
		private bool HasNewQuestForUnlock()
		{
			bool flag = this.quests.questManager.CurrentQuestData == null;
			bool flag2 = this.quests.questManager.GetNextAvailableQuest() != null;
			bool flag3 = !this.contentUnlockService.IsNextQuestLockedByDate();
			return flag && flag2 && flag3;
		}

		// Token: 0x0600224A RID: 8778 RVA: 0x00095E57 File Offset: 0x00094257
		private bool IsCurrentQuestCollected()
		{
			return this.quests.questManager.CurrentQuestProgress != null && this.quests.questManager.CurrentQuestProgress.status == QuestProgress.Status.collected;
		}

		// Token: 0x0600224B RID: 8779 RVA: 0x00095E8C File Offset: 0x0009428C
		private IEnumerator CenterOnNewArea()
		{
			bool showNewAreaBanner = BannerNewAreaRoot.ShouldShow(this.quests, this.progression);
			if (!showNewAreaBanner && this.forceNewAreaBanner)
			{
				Log.Warning("Info about forceNewAreaBanner", "ForceNewAreaBanner will force the banner", null);
			}
			if (showNewAreaBanner || this.forceNewAreaBanner)
			{
				int islandId = this.configs.SbsConfig.islandareaconfig.IslandForArea(this.quests.UnlockedAreaWithQuestAndEndOfContent);
				yield return new SwitchIslandFlow().Start(new SwitchIslandFlow.SwitchIslandFlowData(islandId, false));
				yield return new SceneLoaderFlow<BannerNewAreaRoot>(true).ExecuteRoutine();
				yield return SceneManager.Instance.Inject(this);
				yield return ServiceLocator.Instance.Inject(this);
			}
			if (this.progression.LastUnlockedArea != this.quests.UnlockedAreaWithQuestAndEndOfContent)
			{
				Log.Warning("Info on Areas repair", string.Format("Repair areas: {0} LastUnlockArea: {1}, UnlockedAreaWithQuestAndEndOfContent: {2}", this.sbs.SbsConfig.feature_switches.repair_areas, this.progression.LastUnlockedArea, this.quests.UnlockedAreaWithQuestAndEndOfContent), null);
			}
			yield break;
		}

		// Token: 0x0600224C RID: 8780 RVA: 0x00095EA8 File Offset: 0x000942A8
		private IEnumerator ShowChapterIntro()
		{
			int chapter = this.GetChapterToStart();
			if (chapter >= 2)
			{
				this.townMain.uiRoot.ShowUi(false);
				Wooroutine<BannerNewChapterRoot> intro = SceneManager.Instance.LoadSceneWithParams<BannerNewChapterRoot, int>(chapter, null);
				yield return intro;
				yield return intro.ReturnValue.onDestroyed;
				yield return new WaitForSeconds(1f);
				this.townMain.uiRoot.ShowUi(true);
			}
			yield break;
		}

		// Token: 0x0600224D RID: 8781 RVA: 0x00095EC4 File Offset: 0x000942C4
		private int GetChapterToStart()
		{
			string id = this.quests.questManager.CurrentQuestData.id;
			ChapterData[] chapters = this.configs.chapter.chapters;
			ChapterData chapterData = chapters.FirstOrDefault((ChapterData c) => c.started_by_quest == id);
			return (chapterData == null) ? -1 : chapterData.chapter;
		}

		// Token: 0x04004DAB RID: 19883
		[WaitForService(true, true)]
		private QuestService quests;

		// Token: 0x04004DAC RID: 19884
		[WaitForService(true, true)]
		private ProgressionDataService.Service progression;

		// Token: 0x04004DAD RID: 19885
		[WaitForService(true, true)]
		private GameStateService gameState;

		// Token: 0x04004DAE RID: 19886
		[WaitForService(true, true)]
		private ConfigService configs;

		// Token: 0x04004DAF RID: 19887
		[WaitForService(true, true)]
		private AssetBundleService abs;

		// Token: 0x04004DB0 RID: 19888
		[WaitForService(true, true)]
		private ContentUnlockService contentUnlockService;

		// Token: 0x04004DB1 RID: 19889
		[WaitForService(true, true)]
		private SBSService sbs;

		// Token: 0x04004DB2 RID: 19890
		[WaitForRoot(false, false)]
		private VillagersControllerRoot villagerController;

		// Token: 0x04004DB3 RID: 19891
		[WaitForRoot(false, false)]
		private TownMainRoot townMain;

		// Token: 0x04004DB4 RID: 19892
		private readonly bool forceNewAreaBanner;

		// Token: 0x04004DB5 RID: 19893
		private readonly bool allowPleaseGoOnline;

		// Token: 0x04004DB6 RID: 19894
		private static bool hasSeenMissingContentWarning;
	}
}
