using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Match3.Scripts1.Audio;
using Match3.Scripts1.Puzzletown.Flows;
using Match3.Scripts1.Puzzletown.Match3;
using Match3.Scripts1.Puzzletown.Match3.Scoring;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Puzzletown.Town;
using Match3.Scripts1.Puzzletown.Tutorials;
using Match3.Scripts1.UnityEngine;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.UI;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Match3.Scripts1.Puzzletown.UI
{
	// Token: 0x020004B2 RID: 1202
	[LoadOptions(false, true, false)]
	public class PirateBreakoutRoot : APtSceneRoot<Match3Score>, IDisposableDialog, IHandler<PopupOperation>, IHandler<Level>, IFacebookFriendsList
	{
		// Token: 0x17000539 RID: 1337
		// (get) Token: 0x060021D1 RID: 8657 RVA: 0x0008F36B File Offset: 0x0008D76B
		public IEnumerable<TownOptionsFriendData> FacebookFriends
		{
			get
			{
				return this.friendsData;
			}
		}

		// Token: 0x060021D2 RID: 8658 RVA: 0x0008F373 File Offset: 0x0008D773
		public void Handle(PopupOperation operation)
		{
			if (operation == PopupOperation.Close)
			{
				this.Close();
			}
		}

		// Token: 0x060021D3 RID: 8659 RVA: 0x0008F38C File Offset: 0x0008D78C
		public void Handle(Level level)
		{
			new CoreGameFlow().Start(new CoreGameFlow.Input(level.level, false, null, LevelPlayMode.PirateBreakout));
		}

		// Token: 0x060021D4 RID: 8660 RVA: 0x0008F3A8 File Offset: 0x0008D7A8
		protected override IEnumerator GoRoutine()
		{
			if (this.gameStateService.PirateBreakout.Level < 1)
			{
				this.gameStateService.PirateBreakout.Level = 1;
			}
			if (!base.registeredFirst)
			{
				this.activeLevel = this.gameStateService.PirateBreakout.Level;
			}
			else
			{
				this.gameStateService.PirateBreakout.Level = this.activeLevel;
			}
			int set = (this.gameStateService.PirateBreakout.Set - 1) % 3 + 1;
			yield return this.SetupIslandSet(set);
			Wooroutine<M3_PirateBreakoutStartRoot> levelStartSceneLoadRoutine = SceneManager.Instance.LoadSceneWithParams<M3_PirateBreakoutStartRoot, LevelConfig>(null, null);
			yield return levelStartSceneLoadRoutine;
			levelStartSceneLoadRoutine.ReturnValue.Hide(true);
			this.countdownTimer.SetTargetTime(this.gameStateService.PirateBreakout.EndTime, false, null);
			this.audioService.PlaySFX(AudioId.PopupShowDefault, false, false, false);
			this.dialog.Show();
			this.SetupLevels();
			this.SetupGates();
			this.SetupBlacktail();
			this.story.Setup(this.locaService);
			base.StartCoroutine(this.RefreshFriendsFromFacebookRoutine());
			this.tutorialRunner.Run();
			yield return this.tutorialRunner.onInitialized;
			base.OnInitialized.Dispatch();
			this.tutorialMarkerObject.SetActive(true);
			BackButtonManager.Instance.AddAction(new Action(this.Close));
			this.levelsSnapper.onBeginDrag.AddListener(new Action<PointerEventData>(this.CloseBubbles));
			if (this.parameters != null && this.parameters.success && this.IsTriggerDialogueLevel(this.parameters.Config.Level.level))
			{
				int level = this.parameters.Config.Level.level;
				base.StartCoroutine(this.ShowDialogueWithPending(level, this.DialogueForLevel(level), this.gameStateService.Resources.HasPendingRewards()));
			}
			else if (this.gameStateService.PirateBreakout.EndTime < DateTime.Now)
			{
				this.Close();
			}
			else if (!this.tutorialRunner.IsRunning)
			{
				this.SnapToLevel(this.GetSnapLevel(this.activeLevel));
			}
			this.gameStateService.SetSeenFlagWithTimestamp("weeklyEventSeen", DateTime.UtcNow);
			yield break;
		}

		// Token: 0x060021D5 RID: 8661 RVA: 0x0008F3C4 File Offset: 0x0008D7C4
		private IEnumerator SetupIslandSet(int set)
		{
			List<string> setItems = new List<string>
			{
				"Gate1",
				"Gate2",
				"Fort"
			};
			int i = 0;
			while (i < this.setContainers.Count && i < setItems.Count)
			{
				Wooroutine<GameObject> loadingRoutine = this.assetBundleService.LoadAsset<GameObject>("pirate_breakout_assets", string.Format("Assets/Puzzletown/Town/Ui/PirateBreakout/Assets/Set {0}/{1}_Set_{0}.prefab", set, setItems[i]));
				yield return loadingRoutine;
				GameObject gate = global::UnityEngine.Object.Instantiate<GameObject>(loadingRoutine.ReturnValue, this.setContainers[i], false);
				gate.transform.localPosition = Vector3.zero;
				this.gateViews.Add(gate.GetComponent<GateView>());
				i++;
			}
			Wooroutine<GameObject> loadingRoutineIslands = this.assetBundleService.LoadAsset<GameObject>("pirate_breakout_assets", string.Format("Assets/Puzzletown/Town/Ui/PirateBreakout/Assets/Set {0}/{1}_Set_{0}.prefab", set, "Islands"));
			yield return loadingRoutineIslands;
			GameObject island = global::UnityEngine.Object.Instantiate<GameObject>(loadingRoutineIslands.ReturnValue, this.islandContainer, false);
			island.transform.localPosition = Vector3.zero;
			yield break;
		}

		// Token: 0x060021D6 RID: 8662 RVA: 0x0008F3E8 File Offset: 0x0008D7E8
		private void Close()
		{
			if (!this.topPanel.activeSelf)
			{
				return;
			}
			this.audioService.PlaySFX(AudioId.PopupHideDefault, false, false, false);
			BackButtonManager.Instance.RemoveAction(new Action(this.Close));
			this.dialog.HideAndDisable(false);
			bool placeTrophy = this.activeLevel == 11;
			WooroutineRunner.StartCoroutine(this.ReturnToIslandFlow(placeTrophy), null);
		}

		// Token: 0x060021D7 RID: 8663 RVA: 0x0008F458 File Offset: 0x0008D858
		private IEnumerator ReturnToIslandFlow(bool placeTrophy)
		{
			TownLoadingFlowWithTransition.Input lScreenInput = new TownLoadingFlowWithTransition.Input(LoadingScreenConfig.Random);
			Wooroutine<TownMainRoot> returnToTownFlow = new TownLoadingFlowWithTransition().Start(lScreenInput);
			yield return returnToTownFlow;
			this.tournamentService.NotifyContextChange(SceneContext.MetaGame);
			returnToTownFlow.ReturnValue.StartView(true, false);
			if (placeTrophy)
			{
				BuildingConfig buildingConfig = this.configService.buildingConfigList.GetConfig("iso_trophy_pirate_breakout");
				buildingConfig.TrackingDetail = "pirate_breakout";
				yield return new ForceUserPlaceDecoFlow(buildingConfig).Start();
			}
			yield break;
		}

		// Token: 0x060021D8 RID: 8664 RVA: 0x0008F47C File Offset: 0x0008D87C
		private void SetupGates()
		{
			for (int i = 0; i < this.gateViews.Count; i++)
			{
				GateView gateView = this.gateViews[i];
				int num = this.gameStateService.PirateBreakout.ChestLevel(i);
				if (this.activeLevel > num)
				{
					gateView.SetupView(true, null);
				}
				else
				{
					gateView.SetupView(false, this.gameStateService.PirateBreakout.GetRewards(num));
				}
			}
		}

		// Token: 0x060021D9 RID: 8665 RVA: 0x0008F4F8 File Offset: 0x0008D8F8
		public IEnumerator ShowDialogueWithPending(int level, List<string> storyDialog, bool showPending = false)
		{
			int gateIndex = Mathf.FloorToInt((float)(level / this.gateViews.Count)) - 1;
			GateView gateView = this.gateViews[gateIndex];
			this.SetDialogueModeEnabled(true);
			gateView.SetFocusMode(true);
			this.FocusOn(gateView.transform, this.focusPoint.transform);
			if (showPending)
			{
				this.eventSystem.Disable();
				yield return gateView.PlayCloseAnimation();
				yield return new WaitForSeconds(0.5f);
				yield return gateView.PlayOpen1Animation();
				Wooroutine<TownRewardsRoot> scene = SceneManager.Instance.LoadSceneWithParams<TownRewardsRoot, List<TownReward>>(null, null);
				yield return scene;
				this.eventSystem.Enable();
				yield return scene.ReturnValue.onDestroyed;
			}
			this.dialogueAnimation.PlayQueued("PirateBreakoutDialogTransitionFadeIn");
			yield return this.story.ShowDialog(storyDialog);
			this.dialogueAnimation.Play("PirateBreakoutDialogTransitionFadeOut");
			if (showPending)
			{
				yield return gateView.PlayOpen2Animation();
				if (level == 10)
				{
					yield return new WaitForSeconds(1f);
					this.SetDialogueModeEnabled(false);
					this.Close();
				}
			}
			gateView.SetFocusMode(false);
			this.SetDialogueModeEnabled(false);
			yield break;
		}

		// Token: 0x060021DA RID: 8666 RVA: 0x0008F528 File Offset: 0x0008D928
		private void FocusOn(Transform target, Transform focusPoint)
		{
			Canvas.ForceUpdateCanvases();
			Vector3 position = this.scrollRect.content.transform.TransformPoint(this.scrollRect.content.transform.InverseTransformPoint(focusPoint.position));
			Vector2 vector = this.scrollRect.transform.InverseTransformPoint(position) - this.scrollRect.transform.InverseTransformPoint(target.position);
			this.scrollRect.content.anchoredPosition += new Vector2(0f, vector.y);
		}

		// Token: 0x060021DB RID: 8667 RVA: 0x0008F5D0 File Offset: 0x0008D9D0
		private void CloseBubbles(PointerEventData eventData)
		{
			foreach (GateView gateView in this.gateViews)
			{
				gateView.CloseBubble();
			}
		}

		// Token: 0x060021DC RID: 8668 RVA: 0x0008F62C File Offset: 0x0008DA2C
		private void SetupBlacktail()
		{
			int num = Mathf.Min(this.activeLevel, this.blacktailPlacements.Count);
			GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(this.blacktailPrefab, this.blacktailPlacements[num - 1]);
			gameObject.transform.localPosition = Vector3.zero;
		}

		// Token: 0x060021DD RID: 8669 RVA: 0x0008F67A File Offset: 0x0008DA7A
		private void SetDialogueModeEnabled(bool enabled)
		{
			this.scrollRect.enabled = !enabled;
			this.topPanel.gameObject.SetActive(!enabled);
		}

		// Token: 0x060021DE RID: 8670 RVA: 0x0008F6A0 File Offset: 0x0008DAA0
		private List<string> DialogueForLevel(int level)
		{
			List<string> list = new List<string>
			{
				string.Format("piratebreakout.set1_level{0}.1", level),
				string.Format("piratebreakout.set1_level{0}.2", level)
			};
			if (level == 10)
			{
				list.RemoveAt(1);
			}
			return list;
		}

		// Token: 0x060021DF RID: 8671 RVA: 0x0008F6F1 File Offset: 0x0008DAF1
		private bool IsTriggerDialogueLevel(int level)
		{
			return this.gameStateService.PirateBreakout.IsRewardedLevel(level) || level == 9;
		}

		// Token: 0x060021E0 RID: 8672 RVA: 0x0008F711 File Offset: 0x0008DB11
		public void SnapToLevel(int level)
		{
			this.levelsSnapper.Snap(this.GetLevelsSnapValue((float)level));
		}

		// Token: 0x060021E1 RID: 8673 RVA: 0x0008F726 File Offset: 0x0008DB26
		public void ScrollToLevel(int level, float time = 1f)
		{
			this.levelsSnapper.ScrollTo(this.GetLevelsSnapValue((float)level), time);
		}

		// Token: 0x060021E2 RID: 8674 RVA: 0x0008F73C File Offset: 0x0008DB3C
		private int GetSnapLevel(int activeLevel)
		{
			return Mathf.Clamp(activeLevel, 1, 10);
		}

		// Token: 0x060021E3 RID: 8675 RVA: 0x0008F747 File Offset: 0x0008DB47
		private float GetLevelsSnapValue(float level)
		{
			return level / 12f;
		}

		// Token: 0x060021E4 RID: 8676 RVA: 0x0008F750 File Offset: 0x0008DB50
		private void SetupLevels()
		{
			LevelSelectionData.LevelMode levelMode = LevelSelectionData.LevelMode.Normal;
			for (int i = 1; i <= this.buttons.Count; i++)
			{
				LevelSelectionData.LockReason lockReason = LevelSelectionData.LockReason.NotReached;
				LevelSelectionData.LevelOrder levelOrder = LevelSelectionData.LevelOrder.Normal;
				LevelMapActiveView original = this.lockedButton;
				int num = 0;
				M3LevelSelectionItemView m3LevelSelectionItemView = this.buttons[i - 1];
				Button componentInChildren = m3LevelSelectionItemView.GetComponentInChildren<Button>();
				componentInChildren.interactable = false;
				string text = string.Empty;
				if (this.gameStateService.PirateBreakout.IsRewardedLevel(i))
				{
					text = "key";
				}
				if (i < this.gameStateService.PirateBreakout.Level)
				{
					lockReason = LevelSelectionData.LockReason.NotLocked;
					original = this.masteredButton;
					num = 3;
				}
				else if (i == this.gameStateService.PirateBreakout.Level)
				{
					lockReason = LevelSelectionData.LockReason.NotLocked;
					original = this.focusButton;
					levelOrder = LevelSelectionData.LevelOrder.Latest;
					componentInChildren.interactable = true;
				}
				PirateBreakoutSetConfig pirateBreakoutLevelConfig = this.m3ConfigService.GetPirateBreakoutLevelConfig(this.gameStateService.PirateBreakout.Set, i);
				PirateBreakoutSetConfig setConfig = pirateBreakoutLevelConfig;
				int tier = num;
				bool isHollow = false;
				bool isSeparator = false;
				string collectable = text;
				LevelSelectionData.LockReason lockReason2 = lockReason;
				LevelSelectionData.LevelOrder levelOrder2 = levelOrder;
				LevelSelectionData.LevelMode levelMode2 = levelMode;
				FeatureSwitchesConfig feature_switches = this.sbsService.SbsConfig.feature_switches;
				LevelSelectionData data = new LevelSelectionData(setConfig, tier, isHollow, isSeparator, collectable, lockReason2, levelOrder2, levelMode2, this, null, feature_switches, null);
				LevelMapActiveView levelMapActiveView = global::UnityEngine.Object.Instantiate<LevelMapActiveView>(original, m3LevelSelectionItemView.transform);
				levelMapActiveView.transform.localPosition = Vector3.zero;
				levelMapActiveView.transform.SetAsFirstSibling();
				m3LevelSelectionItemView.Show(data);
				levelMapActiveView.Show(data);
			}
		}

		// Token: 0x060021E5 RID: 8677 RVA: 0x0008F8D4 File Offset: 0x0008DCD4
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

		// Token: 0x060021E6 RID: 8678 RVA: 0x0008F8F0 File Offset: 0x0008DCF0
		private IEnumerable<TownOptionsFriendData> ConvertFriends(IEnumerable<FacebookData.Friend> friends)
		{
			yield return this.ConvertFriend(this.facebookService.Me, true);
			foreach (FacebookData.Friend friend in friends)
			{
				yield return this.ConvertFriend(friend, false);
			}
			yield break;
		}

		// Token: 0x060021E7 RID: 8679 RVA: 0x0008F91C File Offset: 0x0008DD1C
		private TownOptionsFriendData ConvertFriend(FacebookData.Friend friend, bool isMe)
		{
			bool flag = this.facebookService.FriendProgress(friend.ID).PbLevel.eventId == this.gameStateService.PirateBreakout.EventId;
			int num = (!flag) ? 1 : this.facebookService.FriendProgress(friend.ID).PbLevel.progress;
			return new TownOptionsFriendData
			{
				friend = friend,
				avatar = this.facebookService.GetProfilePicture(friend),
				level = ((!isMe) ? num : this.gameStateService.PirateBreakout.Level),
				type = ((!isMe) ? TownOptionsFriendData.Type.Friend : TownOptionsFriendData.Type.Me)
			};
		}

		// Token: 0x04004D12 RID: 19730
		public const string WEEKLY_EVENT_SEEN_FLAG_NAME = "weeklyEventSeen";

		// Token: 0x04004D13 RID: 19731
		private const string DIALOGUE_START_ANIMATION_NAME = "PirateBreakoutDialogTransitionFadeIn";

		// Token: 0x04004D14 RID: 19732
		private const string DIALOGUE_END_ANIMATION_NAME = "PirateBreakoutDialogTransitionFadeOut";

		// Token: 0x04004D15 RID: 19733
		private const int LEVEL_POSITION_OFFSET = 2;

		// Token: 0x04004D16 RID: 19734
		private const int SET_VARIATIONS_COUNT = 3;

		// Token: 0x04004D17 RID: 19735
		private const string SET_PATH = "Assets/Puzzletown/Town/Ui/PirateBreakout/Assets/Set {0}/{1}_Set_{0}.prefab";

		// Token: 0x04004D18 RID: 19736
		private const string SET_GATE1 = "Gate1";

		// Token: 0x04004D19 RID: 19737
		private const string SET_GATE2 = "Gate2";

		// Token: 0x04004D1A RID: 19738
		private const string SET_FORT = "Fort";

		// Token: 0x04004D1B RID: 19739
		private const string SET_ISLANDS = "Islands";

		// Token: 0x04004D1C RID: 19740
		[WaitForService(true, true)]
		private GameStateService gameStateService;

		// Token: 0x04004D1D RID: 19741
		[WaitForService(true, true)]
		private AudioService audioService;

		// Token: 0x04004D1E RID: 19742
		[WaitForService(true, true)]
		private SBSService sbsService;

		// Token: 0x04004D1F RID: 19743
		[WaitForService(true, true)]
		private FacebookService facebookService;

		// Token: 0x04004D20 RID: 19744
		[WaitForService(true, true)]
		private M3ConfigService m3ConfigService;

		// Token: 0x04004D21 RID: 19745
		[WaitForService(true, true)]
		private ConfigService configService;

		// Token: 0x04004D22 RID: 19746
		[WaitForService(true, true)]
		private TournamentService tournamentService;

		// Token: 0x04004D23 RID: 19747
		[WaitForService(true, true)]
		private ILocalizationService locaService;

		// Token: 0x04004D24 RID: 19748
		[WaitForService(true, true)]
		private AssetBundleService assetBundleService;

		// Token: 0x04004D25 RID: 19749
		[WaitForRoot(false, false)]
		private EventSystemRoot eventSystem;

		// Token: 0x04004D26 RID: 19750
		[WaitForRoot(false, false)]
		private TownResourcePanelRoot resourcePanelRoot;

		// Token: 0x04004D27 RID: 19751
		[SerializeField]
		private AnimatedUi dialog;

		// Token: 0x04004D28 RID: 19752
		[SerializeField]
		private CountdownTimer countdownTimer;

		// Token: 0x04004D29 RID: 19753
		[SerializeField]
		private TableViewSnapper levelsSnapper;

		// Token: 0x04004D2A RID: 19754
		[SerializeField]
		private GameObject topPanel;

		// Token: 0x04004D2B RID: 19755
		[SerializeField]
		private ScrollRect scrollRect;

		// Token: 0x04004D2C RID: 19756
		[SerializeField]
		private RectTransform focusPoint;

		// Token: 0x04004D2D RID: 19757
		[SerializeField]
		private Transform islandContainer;

		// Token: 0x04004D2E RID: 19758
		[SerializeField]
		private List<Transform> setContainers;

		// Token: 0x04004D2F RID: 19759
		[Header("Level Buttons")]
		[SerializeField]
		private LevelMapActiveView masteredButton;

		// Token: 0x04004D30 RID: 19760
		[SerializeField]
		private LevelMapActiveView focusButton;

		// Token: 0x04004D31 RID: 19761
		[SerializeField]
		private LevelMapActiveView lockedButton;

		// Token: 0x04004D32 RID: 19762
		[SerializeField]
		private List<M3LevelSelectionItemView> buttons;

		// Token: 0x04004D33 RID: 19763
		[Header("Story Dialogue")]
		[SerializeField]
		private PirateBreakOutLevelStory story;

		// Token: 0x04004D34 RID: 19764
		[SerializeField]
		private List<Transform> blacktailPlacements;

		// Token: 0x04004D35 RID: 19765
		[SerializeField]
		private GameObject blacktailPrefab;

		// Token: 0x04004D36 RID: 19766
		[SerializeField]
		private Animation dialogueAnimation;

		// Token: 0x04004D37 RID: 19767
		[Header("Tutorial")]
		[SerializeField]
		private GameObject tutorialMarkerObject;

		// Token: 0x04004D38 RID: 19768
		[SerializeField]
		private TutorialRunner tutorialRunner;

		// Token: 0x04004D39 RID: 19769
		[Header("Testing")]
		[SerializeField]
		private int activeLevel = 8;

		// Token: 0x04004D3A RID: 19770
		private List<TownOptionsFriendData> friendsData = new List<TownOptionsFriendData>();

		// Token: 0x04004D3B RID: 19771
		private List<GateView> gateViews = new List<GateView>();
	}
}
