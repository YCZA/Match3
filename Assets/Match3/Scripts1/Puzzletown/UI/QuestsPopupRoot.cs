using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Match3.Scripts1.Audio;
using Match3.Scripts1.Puzzletown.Flows;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Puzzletown.Town;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.Services.ErrorAnalytics;
using Match3.Scripts1.Wooga.Signals;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.UI
{
	// Token: 0x02000A03 RID: 2563
	[LoadOptions(true, true, false)]
	public class QuestsPopupRoot : APtSceneRoot, IHandler<PopupOperation>, IHandler<QuestDataSource.CurrentLevelNumber>, IHandler<QuestData>, IPersistentDialog
	{
		// Token: 0x06003DAC RID: 15788 RVA: 0x00138160 File Offset: 0x00136560
		protected override void Go()
		{
			// #if REVIEW_VERSION
			// 	_levelSectionChaptersView.gameObject.SetActive(false);
			// #endif
			this._levelSectionChaptersView.Init(this.loc, this.config, this.quests);
			// this._levelSectionChaptersView.Init(this.loc, this.config, this.quests);
			this._levelSectionChaptersView.onChapterSelected.AddListener(new Action<ChapterInfo>(this.PanToChapter));
			// 和滑动相关
			this.swipePanel.onSwipe.AddListener(new Action<RelativeDirection>(this.OnSwipe));
			this.initialized = true;
		}

		// Token: 0x06003DAD RID: 15789 RVA: 0x001381C9 File Offset: 0x001365C9
		protected override void OnEnable()
		{
			Debug.Log("QuestsPopup.Init:" + initialized);
			base.OnEnable();
			if (!this.initialized)
			{
				return;
			}
			this.cancelButton.SetActive(true);
			this.audioService.PlaySFX(AudioId.MenuSlideOpen, false, false, false);
		}

		// Token: 0x06003DAE RID: 15790 RVA: 0x00138200 File Offset: 0x00136600
		public void Show()
		{
			this.questList = this.quests.GetQuestListWithChapterIntro();
			this.currentQuest = this.quests.questManager.CurrentQuestData;
			QuestData item = this.quests.QuestForLevel(this.contentUnlockService.EndOfContentLevel());
			this.questList.Add(item);
			if (this.selectedQuestView != null)
			{
				global::UnityEngine.Object.Destroy(this.selectedQuestView.gameObject);
			}
			int num = 1;
			if (this.currentQuest == null)
			{
				this.currentQuest = item;
				if (this.contentUnlockService.IsNextQuestLockedByDate())
				{
					QuestProgress nextAvailableQuest = this.quests.questManager.GetNextAvailableQuest();
					if (nextAvailableQuest != null)
					{
						this.currentQuest = nextAvailableQuest.configData;
					}
				}
				num = 0;
				this.rightArrow.gameObject.SetActive(false);
			}
			this.currentQuestIndex = this.questList.FindIndex((QuestData q) => q.id == this.currentQuest.id);
			this.selectedQuestIndex = this.currentQuestIndex;
			this.maxQuestIndex = this.selectedQuestIndex + num;
			// 审核版不显示最后一页(完成关卡以解锁章节)
			// #if REVIEW_VERSION
				// maxQuestIndex--;
			// #endif
			this.selectedQuestView = this.InstantiateQuestView(this.selectedQuestIndex);
			this.Handle(new QuestDataSource.CurrentLevelNumber
			{
				levelNumber = this.currentQuest.level
			});
			QuestProgress progress = this.quests.questManager.QuestAndProgress().First((QuestUIData kvp) => kvp.data.id == "chp2_q1").progress;
			if (progress != null)
			{
				if (progress.status == QuestProgress.Status.completed)
				{
					this.tracking.TrackFunnelEvent("490_diary_open_call_tribe_meeting", 490, null);
				}
				else if (progress.tasksProgress[1].progress == 1)
				{
					this.tracking.TrackFunnelEvent("480_diary_open_bring_crabby_home", 480, null);
				}
			}
			this.audioService.PlaySFX(AudioId.PopupShowDefault, false, false, false);
			this.dialog.Show();
			BackButtonManager.Instance.AddAction(new Action(this.HandleBackButtonTap));
			
			// 如果是最后一页，则不显示rightArrow
			if (this.selectedQuestIndex == this.maxQuestIndex)
			{
				this.rightArrow.gameObject.SetActive(false);
			}
			// 如果是第一页, 不显示leftArrow
			if (this.selectedQuestIndex == 0)
			{
				this.leftArrow.gameObject.SetActive(false);
			}
			
			// #if REVIEW_VERSION
			// 	this.rightArrow.gameObject.SetActive(false);
			// 	this.leftArrow.gameObject.SetActive(false);
			// #endif
		}

		// Token: 0x06003DAF RID: 15791 RVA: 0x00138404 File Offset: 0x00136804
		public void Close(QuestTaskType selectedType = QuestTaskType.unknown, bool taskCompleted = false)
		{
			QuestsPopupRoot.Output output = new QuestsPopupRoot.Output(selectedType, taskCompleted, 0);
			this.Close(output);
		}

		// Token: 0x06003DB0 RID: 15792 RVA: 0x00138424 File Offset: 0x00136824
		public void ShowTitle(bool showTitle)
		{
			AnimationClip animationClip = (!showTitle) ? this.collapseTitle : this.openTitle;
			if (this.titleAnimation.clip != animationClip)
			{
				this.titleAnimation.enabled = false;
				this.titleAnimation.enabled = true;
				this.titleAnimation.clip = animationClip;
				this.titleAnimation.Play();
			}
		}

		// Token: 0x06003DB1 RID: 15793 RVA: 0x00138490 File Offset: 0x00136890
		private void Close(QuestsPopupRoot.Output output)
		{
			this.dialog.Hide();
			BackButtonManager.Instance.RemoveAction(new Action(this.HandleBackButtonTap));
			this.audioService.PlaySFX(AudioId.PopupHideDefault, false, false, false);
			this.onClose.Dispatch(output);
		}

		// Token: 0x06003DB2 RID: 15794 RVA: 0x001384DE File Offset: 0x001368DE
		private bool AllowedToCancel()
		{
			return this.cancelButton.gameObject.activeInHierarchy;
		}

		// eli key point 生成QuestView
		private QuestView InstantiateQuestView(int questIndex)
		{
			QuestData questData = this.questList[questIndex];
			
			bool isEndOfContent = this.contentUnlockService.IsLockedByEndOfContent(questData.level) && this.quests.questManager.CurrentQuestData == null;
			// eli key point 审核版本只有前10章的任务
			// #if REVIEW_VERSION
			// 	isEndOfContent = this.contentUnlockService.IsLockedByEndOfContent(questData.level);
			// #endif
			DateTime? unlockDate = this.contentUnlockService.UnlockDateForQuest(questData.id);
			bool isDateLocked = unlockDate != null && this.quests.questManager.CurrentQuestData == null;
			QuestUIData questUIData = null;
			QuestView.CellType viewType;
			QuestProgress.Status desiredStatus;
			if (isEndOfContent)
			{
				viewType = QuestView.CellType.endOfContent;
				desiredStatus = QuestProgress.Status.locked;
				questUIData = this.quests.questManager.CurrentQuestUIData;
			}
			else if (isDateLocked)
			{
				viewType = QuestView.CellType.dateLocked;
				desiredStatus = QuestProgress.Status.locked;
				questUIData = this.quests.questManager.CurrentQuestUIData;
				questUIData.unlockDate = unlockDate;
			}
			else if (this.currentQuestIndex == questIndex || QuestsPopupRoot.debugShowAllQuests)
			{
				// 此页面任务未完成时执行这里（当前任务）
				viewType = QuestView.CellType.current;
				desiredStatus = QuestProgress.Status.started;
				if (this.quests.questManager.CurrentQuestUIData != null)
				{
					questUIData = this.quests.questManager.CurrentQuestUIData;
				}
			}
			else if (questIndex < this.currentQuestIndex)
			{
				// 此页面任务已完成时执行这里
				bool isIntro = questData.id == "chapter_intro";
				QuestView.CellType cellType = isIntro ? QuestView.CellType.chapterIntro : QuestView.CellType.done;
				QuestProgress.Status status = isIntro ? QuestProgress.Status.chapterIntro : QuestProgress.Status.completed;
				viewType = cellType;
				desiredStatus = status;
			}
			else
			{
				viewType = QuestView.CellType.pending;
				desiredStatus = QuestProgress.Status.locked;
			}
			if (questUIData == null && questIndex < this.questList.Count)
			{
				// 此页面任务已完成时执行这里
				questUIData = new QuestUIData(QuestProgress.CreateWithStatus(this.questList[questIndex], desiredStatus), this.questList[questIndex]);
			}
			QuestView questView = this.InstantiateQuestView(viewType, this.viewsParent.transform);
			questView.Show(questUIData);
			return questView;
		}

		// Token: 0x06003DB4 RID: 15796 RVA: 0x001386BC File Offset: 0x00136ABC
		private QuestView InstantiateQuestView(QuestView.CellType viewType, Transform parent)
		{
			return global::UnityEngine.Object.Instantiate<QuestView>(this.questViewPrefabs.First((QuestView view) => view.questStatus == viewType), parent);
		}

		// 翻页
		private void OnSwipe(RelativeDirection relativeDirection)
		{
			// #if REVIEW_VERSION
			// 	return;
			// #endif
			if (relativeDirection == RelativeDirection.Left || relativeDirection == RelativeDirection.Right)
			{
				base.StartCoroutine(this.SwipeRoutine(relativeDirection, -1));
			}
		}

		// Token: 0x06003DB6 RID: 15798 RVA: 0x00138714 File Offset: 0x00136B14
		private IEnumerator SwipeRoutine(RelativeDirection relativeDirection, int toIndex = -1)
		{
			QuestView oldView = this.selectedQuestView;
			SwipeAnimationDirection oldViewAnimationDirection = SwipeAnimationDirection.None;
			SwipeAnimationDirection newViewAnimationDirection = SwipeAnimationDirection.None;
			if (toIndex >= 0)
			{
				if (this.selectedQuestIndex == toIndex)
				{
					yield break;
				}
				if (this.selectedQuestIndex > toIndex)
				{
					oldViewAnimationDirection = SwipeAnimationDirection.CenterToRight;
					newViewAnimationDirection = SwipeAnimationDirection.LeftToCenter;
				}
				else
				{
					oldViewAnimationDirection = SwipeAnimationDirection.CenterToLeft;
					newViewAnimationDirection = SwipeAnimationDirection.RightToCenter;
				}
				this.selectedQuestIndex = toIndex;
			}
			else
			{
				if (relativeDirection != RelativeDirection.Left)
				{
					if (relativeDirection == RelativeDirection.Right)
					{
						this.leftArrow.gameObject.SetActive(true);
						// #if REVIEW_VERSION
						// 	this.leftArrow.gameObject.SetActive(false);
						// #endif
						if (QuestsPopupRoot.debugShowAllQuests)
						{
							this.maxQuestIndex = this.questList.Count - 1;
						}
						if (this.selectedQuestIndex >= this.maxQuestIndex)
						{
							this.selectedQuestIndex = this.maxQuestIndex;
							yield break;
						}
						this.selectedQuestIndex++;
						oldViewAnimationDirection = SwipeAnimationDirection.CenterToLeft;
						newViewAnimationDirection = SwipeAnimationDirection.RightToCenter;
					}
				}
				else
				{
					if (this.selectedQuestIndex <= 0)
					{
						this.selectedQuestIndex = 0;
						yield break;
					}
					this.selectedQuestIndex--;
					oldViewAnimationDirection = SwipeAnimationDirection.CenterToRight;
					newViewAnimationDirection = SwipeAnimationDirection.LeftToCenter;
				}
				this.transitionBySwipe = true;
			}
			this.rightArrow.gameObject.SetActive(true);
			this.leftArrow.gameObject.SetActive(true);
			// #if REVIEW_VERSION
			// 	this.rightArrow.gameObject.SetActive(false);
			// 	this.leftArrow.gameObject.SetActive(false);
			// #endif
			if (this.selectedQuestIndex == 0)
			{
				this.leftArrow.gameObject.SetActive(false);
			}
			if (this.selectedQuestIndex == this.maxQuestIndex)
			{
				this.rightArrow.gameObject.SetActive(false);
			}
			this.selectedQuestView.swipeView.AnimateViewDirection(oldViewAnimationDirection);
			oldView = this.selectedQuestView;
			this.selectedQuestView = this.InstantiateQuestView(this.selectedQuestIndex);
			this.selectedQuestView.swipeView.AnimateViewDirection(newViewAnimationDirection);
			yield return new WaitForSeconds(this.selectedQuestView.swipeView.AnimationLength);
			global::UnityEngine.Object.Destroy(oldView.gameObject);
			if (toIndex < 0)
			{
				this.Handle(new QuestDataSource.CurrentLevelNumber
				{
					levelNumber = ((this.selectedQuestView.Quest != null) ? this.selectedQuestView.Quest.level : this.currentQuest.level)
				});
			}
		}

		// Token: 0x06003DB7 RID: 15799 RVA: 0x00138740 File Offset: 0x00136B40
		public void CollectAndCloseMaterials(QuestProgress progress)
		{
			if (progress.configData.level < 600)
			{
				BlockerManager.global.Append(new QuestCompleteFlow(progress.configData, false));
				this.quests.questManager.CollectQuest(progress);
				this.Close(QuestTaskType.unknown, false);
			}
			else
			{
				WooroutineRunner.StartCoroutine(this.TryCollectAndCloseMaterialsRoutine(progress), null);
			}
		}

		// Token: 0x06003DB8 RID: 15800 RVA: 0x001387A4 File Offset: 0x00136BA4
		private IEnumerator TryCollectAndCloseMaterialsRoutine(QuestProgress progress)
		{
			QuestData nextQuestConfig = this.GetNextQuestConfig();
			bool isLandAvailable = true;
			if (nextQuestConfig.id != "end_of_content")
			{
				Wooroutine<bool> isLandAvailableRoutine = this.quests.IsIslandBundleForQuestCompletionAvailable(progress, nextQuestConfig.level);
				yield return isLandAvailableRoutine;
				isLandAvailable = isLandAvailableRoutine.ReturnValue;
			}
			if (isLandAvailable)
			{
				this.quests.questManager.CollectQuest(progress);
				bool shouldForceNewAreaBanner = BannerNewAreaRoot.ShouldShow(this.quests, this.progressionService);
				BlockerManager.global.Append(new QuestCompleteFlow(progress.configData, shouldForceNewAreaBanner));
				this.Close(QuestTaskType.unknown, false);
			}
			else
			{
				this.Close(QuestTaskType.unknown, false);
				this.uiRoot.ShowUi(true);
				string missingScene = TownMainRoot.GetSceneNameForIsland(1);
				Log.Warning("[QuestPopupRoot] Missing scene", missingScene, null);
				yield return PopupMissingAssetsRoot.TryShowRoutine(missingScene);
			}
			yield break;
		}

		// Token: 0x06003DB9 RID: 15801 RVA: 0x001387C8 File Offset: 0x00136BC8
		private QuestData GetNextQuestConfig()
		{
			int index = Mathf.Min(this.selectedQuestIndex + 1, this.questList.Count - 1);
			return this.questList[index];
		}

		// Token: 0x06003DBA RID: 15802 RVA: 0x001387FC File Offset: 0x00136BFC
		protected override void OnDisable()
		{
			base.OnDisable();
			BackButtonManager.Instance.RemoveAction(new Action(this.HandleBackButtonTap));
		}

		// Token: 0x06003DBB RID: 15803 RVA: 0x0013881C File Offset: 0x00136C1C
		private int GetLevelForTask(QuestViewCurrent.TaskProgressViewData data)
		{
			List<Level> list = new List<Level>();
			foreach (string name in data.taskData.levels)
			{
				Level item = Level.Parse(name);
				if (this.progressionService.GetTier(item.level) < item.tier && !this.progressionService.IsLocked(item.level))
				{
					list.Add(item);
				}
			}
			if (list.IsNullOrEmptyCollection())
			{
				return this.progressionService.UnlockedLevel;
			}
			list = (from l in list
			orderby l.tier
			select l).ToList<Level>();
			return list[0].level;
		}

		// 翻页?
		public void PanToChapter(ChapterInfo chapter)
		{
			if (!this.transitionBySwipe && this.initialized)
			{
				int num = this.config.chapter.ChapterForLevel(this.currentQuest.level);
				int i;
				for (i = 0; i < this.questList.Count; i++)
				{
					if (this.questList[i].level >= chapter.firstlevel)
					{
						if (chapter.id != num || this.questList[i].level == this.currentQuest.level)
						{
							break;
						}
					}
				}
				base.StartCoroutine(this.SwipeRoutine(RelativeDirection.None, i));
			}
			else
			{
				this.transitionBySwipe = false;
			}
		}

		// Token: 0x06003DBD RID: 15805 RVA: 0x001389C0 File Offset: 0x00136DC0
		public void Handle(PopupOperation op)
		{
			if (op != PopupOperation.Close)
			{
				if (op != PopupOperation.Skip)
				{
					if (op == PopupOperation.Back)
					{
						this.OnSwipe(RelativeDirection.Left);
					}
				}
				else
				{
					this.OnSwipe(RelativeDirection.Right);
				}
			}
			else
			{
				this.Close(QuestTaskType.unknown, false);
			}
		}

		// Token: 0x06003DBE RID: 15806 RVA: 0x00138A10 File Offset: 0x00136E10
		public void Handle(QuestDataSource.CurrentLevelNumber levelNumber)
		{
			int chapter = this.config.chapter.ChapterForLevel(levelNumber.levelNumber);
			this._levelSectionChaptersView.SnapToChapter(chapter);
		}

		// Token: 0x06003DBF RID: 15807 RVA: 0x00138A44 File Offset: 0x00136E44
		public void Handle(QuestData questData)
		{
			int num = this.config.chapter.ChapterForLevel(questData.level) - 1;
			ChapterData data = this.config.chapter.chapters[num];
			this.PanToChapter(M3_LevelSelectionChaptersView.CreateChapterInfo(this.quests, data));
		}

		// Token: 0x06003DC0 RID: 15808 RVA: 0x00138A90 File Offset: 0x00136E90
		public void HandleShowMeButton(QuestViewCurrent.TaskProgressViewData data)
		{
			QuestsPopupRoot.Output output = new QuestsPopupRoot.Output(data.taskData.type, false, 0);
			QuestTaskType type = data.taskData.type;
			if (type != QuestTaskType.reach_rank)
			{
				if (type == QuestTaskType.collect_and_repair || type == QuestTaskType.collect_and_interact)
				{
					output.selectedLevel = this.GetLevelForTask(data);
				}
			}
			this.Close(output);
		}

		// Token: 0x06003DC1 RID: 15809 RVA: 0x00138AF4 File Offset: 0x00136EF4
		public void HandleBackButtonTap()
		{
			if (this.cancelButton.gameObject != null)
			{
				if (this.AllowedToCancel())
				{
					this.Close(QuestTaskType.unknown, false);
				}
				else
				{
					BackButtonManager.Instance.AddAction(new Action(this.HandleBackButtonTap));
				}
			}
		}

		// Token: 0x06003DC2 RID: 15810 RVA: 0x00138B48 File Offset: 0x00136F48
		public void CollectTask(QuestProgress progress, int taskIdx)
		{
			QuestTaskType selectedTaskType = (QuestTaskType)Enum.Parse(typeof(QuestTaskType), progress.configData.task_type[taskIdx]);
			// eli key point 完成一个小任务后, 关闭窗口
			bool hasTaskCompletedDialog = config.storyDialogue.FindDialogue(DialogueTrigger.quest_task_completed, progress.configData.id + "." + (taskIdx + 1)) != null;
			bool hasTutorialDialog = config.storyDialogue.FindDialogue(DialogueTrigger.tutorial, progress.configData.id + "." + (taskIdx + 1)) != null;
			if (hasTutorialDialog || hasTaskCompletedDialog)
			{
				this.Close(new QuestsPopupRoot.Output(selectedTaskType, true, 0)
				{
					islandAction = delegate()
					{
						// 窗口关闭时执行这个
						this.quests.questManager.CollectQuestTask(progress, taskIdx);
					}
				});
			}
			else
			{
				this.quests.questManager.CollectQuestTask(progress, taskIdx);
			}
		}

		// Token: 0x06003DC3 RID: 15811 RVA: 0x00138BBE File Offset: 0x00136FBE
		public void HideCancel()
		{
			this.cancelButton.SetActive(false);
		}

		// Token: 0x0400668B RID: 26251
		[WaitForService(true, true)]
		public QuestService quests;

		// Token: 0x0400668C RID: 26252
		[WaitForService(true, true)]
		public ILocalizationService loc;

		// Token: 0x0400668D RID: 26253
		[WaitForService(true, true)]
		public ConfigService config;

		// Token: 0x0400668E RID: 26254
		[WaitForService(true, true)]
		private QuestsDataService questsDataService;

		// Token: 0x0400668F RID: 26255
		[WaitForService(true, true)]
		private GameStateService gameStateService;

		// Token: 0x04006690 RID: 26256
		[WaitForService(true, true)]
		private ProgressionDataService.Service progressionService;

		// Token: 0x04006691 RID: 26257
		[WaitForService(true, true)]
		private AudioService audioService;

		// Token: 0x04006692 RID: 26258
		[WaitForService(true, true)]
		private TrackingService tracking;

		// Token: 0x04006693 RID: 26259
		[WaitForService(true, true)]
		private ContentUnlockService contentUnlockService;

		// Token: 0x04006694 RID: 26260
		[SerializeField]
		private M3_LevelSelectionChaptersView _levelSectionChaptersView;

		// Token: 0x04006695 RID: 26261
		[SerializeField]
		private GameObject cancelButton;

		// Token: 0x04006696 RID: 26262
		[SerializeField]
		private List<QuestView> questViewPrefabs;

		// Token: 0x04006697 RID: 26263
		[SerializeField]
		private GameObject viewsParent;

		// Token: 0x04006698 RID: 26264
		[SerializeField]
		private SwipePanel swipePanel;

		// Token: 0x04006699 RID: 26265
		[SerializeField]
		private GameObject rightArrow;

		// Token: 0x0400669A RID: 26266
		[SerializeField]
		private GameObject leftArrow;

		// Token: 0x0400669B RID: 26267
		[SerializeField]
		private Animation titleAnimation;

		// Token: 0x0400669C RID: 26268
		[SerializeField]
		private AnimationClip openTitle;

		// Token: 0x0400669D RID: 26269
		[SerializeField]
		private AnimationClip collapseTitle;

		// Token: 0x0400669E RID: 26270
		[WaitForRoot(false, false)]
		private TownUiRoot uiRoot;

		// Token: 0x0400669F RID: 26271
		public Signal<QuestsPopupRoot.Output> onClose = new Signal<QuestsPopupRoot.Output>();

		// Token: 0x040066A0 RID: 26272
		public static bool debugShowAllQuests;

		// Token: 0x040066A1 RID: 26273
		private MaterialAmount[] reward;

		// Token: 0x040066A2 RID: 26274
		public AnimatedUi dialog;

		// Token: 0x040066A3 RID: 26275
		private bool initialized;

		// Token: 0x040066A4 RID: 26276
		private QuestView selectedQuestView;

		// Token: 0x040066A5 RID: 26277
		private int selectedQuestIndex;

		// Token: 0x040066A6 RID: 26278
		private int maxQuestIndex;

		// Token: 0x040066A7 RID: 26279
		private int currentQuestIndex;

		// Token: 0x040066A8 RID: 26280
		private bool transitionBySwipe;

		// Token: 0x040066A9 RID: 26281
		private List<QuestData> questList;

		// Token: 0x040066AA RID: 26282
		private QuestData currentQuest;

		// Token: 0x02000A04 RID: 2564
		public class Output
		{
			// Token: 0x06003DC7 RID: 15815 RVA: 0x00138C04 File Offset: 0x00137004
			public Output(QuestTaskType selectedTaskType, bool isTaskCompleted, int selectedLevel)
			{
				this.selectedTaskType = selectedTaskType;
				this.isTaskCompleted = isTaskCompleted;
				this.selectedLevel = selectedLevel;
			}

			// Token: 0x040066AD RID: 26285
			public QuestTaskType selectedTaskType;

			// Token: 0x040066AE RID: 26286
			public bool isTaskCompleted;

			// Token: 0x040066AF RID: 26287
			public int selectedLevel;

			// Token: 0x040066B0 RID: 26288
			public Action islandAction;
		}
	}
}
