using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Match3.Scripts1.Audio;
using Match3.Scripts1.Localization;
using Match3.Scripts1.Puzzletown.Flows;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.UnityEngine;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.UI;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;
using Match3.Scripts2.PromptBar;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020006F1 RID: 1777
	public class M3_LevelSelectionUiRoot : APtSceneRoot<int, Level>, IHandler<Level>, IHandler<PopupOperation>, IPersistentDialog, IFacebookFriendsList
	{
		// Token: 0x170006F5 RID: 1781
		// (get) Token: 0x06002C1D RID: 11293 RVA: 0x000CB2A9 File Offset: 0x000C96A9
		// (set) Token: 0x06002C1E RID: 11294 RVA: 0x000CB2B1 File Offset: 0x000C96B1
		public bool SkipReloadOnEnable { get; set; }

		// Token: 0x170006F6 RID: 1782
		// (get) Token: 0x06002C1F RID: 11295 RVA: 0x000CB2BA File Offset: 0x000C96BA
		public IEnumerable<TownOptionsFriendData> FacebookFriends
		{
			get
			{
				return this.friendsData;
			}
		}

		// Token: 0x06002C20 RID: 11296 RVA: 0x000CB2C2 File Offset: 0x000C96C2
		public void HideCloseButton(bool state)
		{
			this.buttonClose.gameObject.SetActive(!state);
		}

		// Token: 0x06002C21 RID: 11297 RVA: 0x000CB2D8 File Offset: 0x000C96D8
		public void GoAgain(int levelToSnap = -1)
		{
			if (levelToSnap >= 0)
			{
				this.parameters = levelToSnap;
			}
			base.gameObject.SetActive(true);
			this.Snap(this.parameters);
		}

		// Token: 0x06002C22 RID: 11298 RVA: 0x000CB300 File Offset: 0x000C9700
		private void Start()
		{
			this.Close();
		}

		// Token: 0x06002C23 RID: 11299 RVA: 0x000CB308 File Offset: 0x000C9708
		protected override void Go()
		{
			this.isInitialized = true;
			this.chaptersView.Init(this.locaService, this.configService, this.questService);
			this.chaptersView.onChapterSelected.AddListener(new Action<ChapterInfo>(this.HandleChapterSelected));
			AUiAdjuster.OnScreenOrientationChange.AddListener(new Action<ScreenOrientation>(this.OnOrientationChange));
			this.OnOrientationChange(AUiAdjuster.SimilarOrientation);
			this.CorrectPadding();
			if (base.registeredFirst)
			{
				new CoreGameFlow().Start(default(CoreGameFlow.Input));
				return;
			}
			if (base.isActiveAndEnabled)
			{
				this.InvokeAtEndOfFrame(new Action(this.Reload));
			}
		}

		// Token: 0x06002C24 RID: 11300 RVA: 0x000CB3BC File Offset: 0x000C97BC
		private void CorrectPadding()
		{
			VerticalLayoutGroup componentInChildren = this.dataSource.tableView.GetComponentInChildren<VerticalLayoutGroup>();
			RectOffset padding = componentInChildren.padding;
			M3LevelSelectionMapItem m3LevelSelectionMapItem = this.dataSource.prototypeCells[0];
			LayoutElement componentInChildren2 = m3LevelSelectionMapItem.GetComponentInChildren<LayoutElement>();
			padding.top -= Mathf.CeilToInt(5f * componentInChildren2.preferredHeight);
			componentInChildren.padding = padding;
		}

		// Token: 0x06002C25 RID: 11301 RVA: 0x000CB41B File Offset: 0x000C981B
		public void Snap(int level)
		{
			if (level > 0)
			{
				this.parameters = level;
				this.SnapToLevel(level);
			}
			else
			{
				this.SnapToLatestLevel();
			}
		}

		// Token: 0x06002C26 RID: 11302 RVA: 0x000CB440 File Offset: 0x000C9840
		private void Reload()
		{
			BackButtonManager.Instance.AddAction(new Action(this.HandleBackButton));
			this.audioService.PlaySFX(AudioId.LevelMapOpen, false, false, false);
			this.dialog.Show();
			if (!this.SkipReloadOnEnable)
			{
				this.dataSource = base.GetComponent<M3_LevelSelectionDataSource>();
				this.dataSource.Init(this.configService, this.questService, this.progressionService, this.m3ConfigService, this, this.sbsService, this.contentUnlockService);
				int currentLevel = this.progressionService.CurrentLevel;
				if (currentLevel != 3)
				{
					if (currentLevel != 4)
					{
						if (currentLevel == 5)
						{
							this.TrackLevelMapFunnelEvent(230);
						}
					}
					else
					{
						this.TrackLevelMapFunnelEvent(205);
					}
				}
				else
				{
					this.TrackLevelMapFunnelEvent(170);
				}
				Canvas.ForceUpdateCanvases();
				this.Snap(this.parameters);
				base.StartCoroutine(this.RefreshFriendsFromFacebookRoutine());
			}
			this.SkipReloadOnEnable = false;
		}

		// Token: 0x06002C27 RID: 11303 RVA: 0x000CB544 File Offset: 0x000C9944
		private void TrackLevelMapFunnelEvent(int id)
		{
			this.tracking.TrackFunnelEvent(string.Format("{0}_level_map_open", id), id, null);
		}

		// Token: 0x06002C28 RID: 11304 RVA: 0x000CB563 File Offset: 0x000C9963
		protected override void OnEnable()
		{
			base.OnEnable();
			if (base.OnInitialized.WasDispatched)
			{
				this.InvokeAtEndOfFrame(new Action(this.Reload));
			}
		}

		// Token: 0x06002C29 RID: 11305 RVA: 0x000CB58D File Offset: 0x000C998D
		private bool IsLocked(AreaConfig level)
		{
			return this.progressionService.IsLocked(level.level) || this.m3ConfigService.IsLockedByQuest(level);
		}

		// Token: 0x06002C2A RID: 11306 RVA: 0x000CB5B4 File Offset: 0x000C99B4
		private float GetLevelsSnapValue(float level)
		{
			if (this.dataSource.GetNumberOfCellsForTableView() == 0)
			{
				return 0f;
			}
			int num = this.configService.chapter.chapters.Count((ChapterData ch) => (float)ch.first_level <= level + float.Epsilon) - 1;
			return (level + (float)num - 1f) / Mathf.Max(1f, (float)this.dataSource.GetNumberOfCellsForTableView());
		}

		// Token: 0x06002C2B RID: 11307 RVA: 0x000CB62E File Offset: 0x000C9A2E
		private void SnapToLatestLevel()
		{
			this.SnapToLevel(this.questService.UnlockedLevelWithQuestAndEndOfContent);
		}

		// Token: 0x06002C2C RID: 11308 RVA: 0x000CB641 File Offset: 0x000C9A41
		private void SnapToLevel(int level)
		{
			this.levelsSnapper.Snap(this.GetLevelsSnapValue((float)level));
		}

		// Token: 0x06002C2D RID: 11309 RVA: 0x000CB656 File Offset: 0x000C9A56
		public void ScrollToLevel(int level, float time = 0.5f)
		{
			this.levelsSnapper.ScrollTo(this.GetLevelsSnapValue((float)level), time);
		}

		// Token: 0x06002C2E RID: 11310 RVA: 0x000CB66C File Offset: 0x000C9A6C
		public void Close()
		{
			// eli key point 关闭关卡列表
			this.dialog.Hide();
		}

		public void Handle(Level level)
		{
			// todo 临时使用提示横条
			if (this.m3ConfigService.IsLockedByQuest(level.level))
			{
				PopupPromptBarRoot.ShowPrompt(game.prompt_please_complete_tasks);
				return;
			}
			// eli key point 打开关卡开始界面, 点击关卡按钮时执行
			if (this.questService.UnlockedAreaWithQuestAndEndOfContent > 1 || this.progressionService.UnlockedLevel == level.level || this.m3ConfigService.IsLockedByQuest(level.level))
			{
				this.onCompleted.Dispatch(level);
			}
			WoogaDebug.Log(new object[]
			{
				"Handle level select!",
				"tier:",
				level.tier,
				"level:",
				level.level
			});
		}

		// Token: 0x06002C30 RID: 11312 RVA: 0x000CB718 File Offset: 0x000C9B18
		public void HandleChapterSelected(ChapterInfo chapter)
		{
			int num = this.questService.UnlockedLevelWithQuestAndEndOfContent;
			if (!chapter.isCurrent)
			{
				num = chapter.firstlevel;
			}
			this.levelsSnapper.Snap(this.GetLevelsSnapValue((float)num));
		}

		// Token: 0x06002C31 RID: 11313 RVA: 0x000CB758 File Offset: 0x000C9B58
		public void Handle(PopupOperation op)
		{
			if (op == PopupOperation.Close)
			{
				this.audioService.PlaySFX(AudioId.LevelMapClose, false, false, false);
				this.onCompleted.Dispatch(default(Level));
				BackButtonManager.Instance.RemoveAction(new Action(this.HandleBackButton));
			}
		}

		// Token: 0x06002C32 RID: 11314 RVA: 0x000CB7B4 File Offset: 0x000C9BB4
		public void HandleBackButton()
		{
			this.Handle(PopupOperation.Close);
		}

		// Token: 0x06002C33 RID: 11315 RVA: 0x000CB7C0 File Offset: 0x000C9BC0
		private void SetSortOrderOfChapterView(bool placeInFront = true)
		{
			if (this.chaptersView == null || this.multipleHideThis == null || this.multipleHideThis.Count < 1)
			{
				return;
			}
			Canvas componentInParent = this.chaptersView.GetComponentInParent<Canvas>();
			Canvas component = this.multipleHideThis[0].GetComponent<Canvas>();
			if (componentInParent != null && component != null)
			{
				if (placeInFront)
				{
					componentInParent.sortingOrder = component.sortingOrder + 1;
				}
				else
				{
					componentInParent.sortingOrder = component.sortingOrder - 1;
				}
			}
		}

		// Token: 0x06002C34 RID: 11316 RVA: 0x000CB858 File Offset: 0x000C9C58
		private void OnOrientationChange(ScreenOrientation screenOrientation)
		{
			switch (screenOrientation)
			{
			case ScreenOrientation.Portrait:
			case ScreenOrientation.PortraitUpsideDown:
				this.SetSortOrderOfChapterView(true);
				break;
			case ScreenOrientation.LandscapeLeft:
			case ScreenOrientation.LandscapeRight:
				this.SetSortOrderOfChapterView(false);
				break;
			}
		}

		// Token: 0x06002C35 RID: 11317 RVA: 0x000CB890 File Offset: 0x000C9C90
		private bool CheckSnapChapter(ChapterData chapter)
		{
			float verticalNormalizedPosition = this.levelsSnapper.scrollRect.verticalNormalizedPosition;
			return this.levelsSnapper.GetInternalSnapPosition(this.GetLevelsSnapValue((float)chapter.first_level + -0.5f)) < verticalNormalizedPosition;
		}

		// Token: 0x06002C36 RID: 11318 RVA: 0x000CB8D0 File Offset: 0x000C9CD0
		private void FixedUpdate()
		{
			if (!this.isInitialized)
			{
				return;
			}
			int num = 1;
			int unlockedLevelWithQuestAndEndOfContent = this.questService.UnlockedLevelWithQuestAndEndOfContent;
			foreach (ChapterData chapterData in this.configService.chapter.chapters)
			{
				if (unlockedLevelWithQuestAndEndOfContent < chapterData.first_level || !this.CheckSnapChapter(chapterData))
				{
					break;
				}
				num = chapterData.chapter;
			}
			if (num == this.chaptersView.SelectedChapter)
			{
				return;
			}
			this.chaptersView.SnapToChapter(num);
		}

		// Token: 0x06002C37 RID: 11319 RVA: 0x000CB96C File Offset: 0x000C9D6C
		private IEnumerator RefreshFriendsFromFacebookRoutine()
		{
			if (!this.facebookService.LoggedIn())
			{
				yield break;
			}
			Wooroutine<FacebookAPIRunner.FriendListData> friends = this.facebookService.GetFriends(FacebookData.Friend.Type.Playing);
			yield return friends;
			this.friendsData = this.ConvertFriends(friends.ReturnValue.friends).ToList<TownOptionsFriendData>();
			this.ShowOnChildren(this, false, true);
			foreach (TownOptionsFriendData friend in this.friendsData)
			{
				Wooroutine<FacebookService.BoxedSprite> picture = this.facebookService.LoadProfilePicture(friend.friend);
				yield return picture;
				if (picture.ReturnValue.spr != null)
				{
					friend.avatar = picture.ReturnValue.spr;
					friend.OnAvatarAvailable.Dispatch(friend.friend.ID, picture.ReturnValue.spr);
				}
			}
			yield break;
		}

		// Token: 0x06002C38 RID: 11320 RVA: 0x000CB988 File Offset: 0x000C9D88
		private TownOptionsFriendData ConvertFriend(FacebookData.Friend friend, bool isMe)
		{
			return new TownOptionsFriendData
			{
				friend = friend,
				avatar = this.facebookService.GetProfilePicture(friend),
				level = ((!isMe) ? this.facebookService.FriendProgress(friend.ID).Level : this.progressionService.UnlockedLevel),
				type = ((!isMe) ? TownOptionsFriendData.Type.Friend : TownOptionsFriendData.Type.Me)
			};
		}

		// Token: 0x06002C39 RID: 11321 RVA: 0x000CBA00 File Offset: 0x000C9E00
		private IEnumerable<TownOptionsFriendData> ConvertFriends(IEnumerable<FacebookData.Friend> friends)
		{
			yield return this.ConvertFriend(this.facebookService.Me, true);
			foreach (FacebookData.Friend friend in friends)
			{
				yield return this.ConvertFriend(friend, false);
			}
			yield break;
		}

		// Token: 0x0400554B RID: 21835
		private const int FILLER_CELLS_COUNT = 5;

		// Token: 0x0400554C RID: 21836
		private const float LEVEL_SNAP_CORRECTION = -0.5f;

		// Token: 0x0400554D RID: 21837
		[WaitForService(true, true)]
		private ConfigService configService;

		// Token: 0x0400554E RID: 21838
		[WaitForService(true, true)]
		private ProgressionDataService.Service progressionService;

		// Token: 0x0400554F RID: 21839
		[WaitForService(true, true)]
		private GameStateService gameStateService;

		// Token: 0x04005550 RID: 21840
		[WaitForService(true, true)]
		private QuestService questService;

		// Token: 0x04005551 RID: 21841
		[WaitForService(true, true)]
		private ILocalizationService locaService;

		// Token: 0x04005552 RID: 21842
		[WaitForService(true, true)]
		private AudioService audioService;

		// Token: 0x04005553 RID: 21843
		[WaitForService(true, true)]
		private TrackingService tracking;

		// Token: 0x04005554 RID: 21844
		[WaitForService(true, true)]
		private M3ConfigService m3ConfigService;

		// Token: 0x04005555 RID: 21845
		[WaitForService(true, true)]
		private FacebookService facebookService;

		// Token: 0x04005556 RID: 21846
		[WaitForService(true, true)]
		private SBSService sbsService;

		// Token: 0x04005557 RID: 21847
		[WaitForService(true, true)]
		private ContentUnlockService contentUnlockService;

		// Token: 0x04005558 RID: 21848
		[WaitForRoot(false, false)]
		private EventSystemRoot eventSystem;

		// Token: 0x04005559 RID: 21849
		[FormerlySerializedAs("snapper")]
		[SerializeField]
		private TableViewSnapper levelsSnapper;

		// Token: 0x0400555A RID: 21850
		[SerializeField]
		private Button buttonClose;

		// Token: 0x0400555B RID: 21851
		[SerializeField]
		private M3_LevelSelectionChaptersView chaptersView;

		// Token: 0x0400555C RID: 21852
		[SerializeField]
		private M3_LevelSelectionDataSource dataSource;

		// Token: 0x0400555D RID: 21853
		private List<TownOptionsFriendData> friendsData = new List<TownOptionsFriendData>();

		// Token: 0x0400555E RID: 21854
		public AnimatedUi dialog;

		// Token: 0x0400555F RID: 21855
		private bool isInitialized;
	}
}
