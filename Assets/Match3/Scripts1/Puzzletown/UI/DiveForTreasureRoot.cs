using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Match3.Scripts1.Audio;
using Match3.Scripts1.Puzzletown.Flows;
using Match3.Scripts1.Puzzletown.Match3;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Puzzletown.Town;
using Match3.Scripts1.Puzzletown.Tutorials;
using Wooga.Coroutines;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Match3.Scripts1.Puzzletown.UI
{
	// Token: 0x020004AF RID: 1199
	[LoadOptions(false, true, false)]
	public class DiveForTreasureRoot : APtSceneRoot, IDisposableDialog
	{
		// Token: 0x060021B3 RID: 8627 RVA: 0x0008D908 File Offset: 0x0008BD08
		protected override IEnumerator GoRoutine()
		{
			if (this.gameStateService.DiveForTreasure.Level < 1)
			{
				this.gameStateService.DiveForTreasure.Level = 1;
			}
			if (!base.registeredFirst)
			{
				this.activeLevel = this.gameStateService.DiveForTreasure.Level;
			}
			else
			{
				this.gameStateService.DiveForTreasure.Level = this.activeLevel;
			}
			this.InitAudio();
			this.closeButton.onClick.AddListener(new UnityAction(this.Close));
			Wooroutine<M3_DiveForTreasureStartRoot> levelStartSceneLoadRoutine = SceneManager.Instance.LoadSceneWithParams<M3_DiveForTreasureStartRoot, LevelConfig>(null, null);
			yield return levelStartSceneLoadRoutine;
			levelStartSceneLoadRoutine.ReturnValue.Hide(true);
			levelStartSceneLoadRoutine.ReturnValue.dftRoot = this;
			if (this.dialog != null)
			{
				this.dialog.Show();
			}
			this.SetupViews();
			this.SnapUiToLevel(this.activeLevel);
			bool isEvenSet = this.gameStateService.DiveForTreasure.Set % 2 == 0;
			foreach (GameObject gameObject in this.firstVariationItems)
			{
				gameObject.gameObject.SetActive(isEvenSet);
			}
			foreach (GameObject gameObject2 in this.secondVariationItems)
			{
				gameObject2.gameObject.SetActive(!isEvenSet);
			}
			this.countdownTimer.SetTargetTime(this.gameStateService.DiveForTreasure.EndTime, false, null);
			BackButtonManager.Instance.AddAction(new Action(this.Close));
			this.audioService.PlaySFX(AudioId.PopupShowDefault, false, false, false);
			base.StartCoroutine(this.RefreshFriendsFromFacebookRoutine());
			this.tutorialRunner = base.GetComponent<TutorialRunner>();
			this.tutorialRunner.Run();
			yield return this.tutorialRunner.onInitialized;
			this.tutorialMarkerObject.SetActive(true);
			base.OnInitialized.Dispatch();
			this.gameStateService.SetSeenFlagWithTimestamp("weeklyEventSeen", DateTime.UtcNow);
			base.StartCoroutine(this.SetupChestsRoutine());
			yield break;
		}

		// Token: 0x060021B4 RID: 8628 RVA: 0x0008D923 File Offset: 0x0008BD23
		private void InitAudio()
		{
			this.audioService.LoadSettings();
			this.musicPlayer = base.GetComponent<MusicPlayer>();
			this.musicPlayer.Init(this.audioService);
		}

		// Token: 0x060021B5 RID: 8629 RVA: 0x0008D94D File Offset: 0x0008BD4D
		public void UpdateStartViewVisibility(bool isVisible)
		{
			this.topUI.gameObject.SetActive(!isVisible);
		}

		// Token: 0x060021B6 RID: 8630 RVA: 0x0008D963 File Offset: 0x0008BD63
		public void ShowBubbles()
		{
			this.firstChestView.ShowBubble();
		}

		// Token: 0x060021B7 RID: 8631 RVA: 0x0008D970 File Offset: 0x0008BD70
		private void SetupViews()
		{
			int i = 0;
			int num = 1;
			while (i < this.buttonPlacements.Count)
			{
				DiveForTreasureLevelView diveForTreasureLevelView;
				if (num < this.activeLevel)
				{
					diveForTreasureLevelView = this.previousLevelPrefab;
				}
				else if (num == this.activeLevel)
				{
					diveForTreasureLevelView = this.currentLevelPrefab;
				}
				else
				{
					diveForTreasureLevelView = this.futureLevelPrefab;
				}
				diveForTreasureLevelView = global::UnityEngine.Object.Instantiate<DiveForTreasureLevelView>(diveForTreasureLevelView, this.buttonPlacements[i]);
				bool hasRewardItem = this.gameStateService.DiveForTreasure.IsRewardedLevel(num);
				diveForTreasureLevelView.SetupView(num, hasRewardItem, this.gameStateService.DiveForTreasure.EndTime < DateTime.Now.ToLocalTime());
				i++;
				num++;
			}
		}

		// Token: 0x060021B8 RID: 8632 RVA: 0x0008DA28 File Offset: 0x0008BE28
		private IEnumerator SetupChestsRoutine()
		{
			this.firstChestView.SetupView(false, this.gameStateService);
			if (this.tutorialRunner.IsRunning)
			{
				this.firstChestView.CloseBubble();
			}
			if (this.activeLevel == this.gameStateService.DiveForTreasure.FirstChestLevel() + 1)
			{
				if (this.gameStateService.Resources.HasPendingRewards())
				{
					yield return this.ShowPending(this.firstChestView);
					this.UpdateStartViewVisibility(false);
					this.secondTutorialMarkerObject.SetActive(true);
				}
				else
				{
					this.firstChestView.SetupView(true, this.gameStateService);
				}
			}
			else if (this.activeLevel > this.gameStateService.DiveForTreasure.FirstChestLevel())
			{
				this.firstChestView.SetupView(true, this.gameStateService);
			}
			if (this.activeLevel == this.gameStateService.DiveForTreasure.SecondChestLevel() + 1)
			{
				if (this.gameStateService.Resources.HasPendingRewards())
				{
					yield return this.ShowPending(this.secondChestView);
					yield return new WaitForSeconds(1f);
					this.UpdateStartViewVisibility(false);
					this.Close();
				}
				else
				{
					this.secondChestView.SetupView(true, this.gameStateService);
				}
			}
			else
			{
				this.secondChestView.SetupView(false, this.gameStateService);
			}
			yield break;
		}

		// Token: 0x060021B9 RID: 8633 RVA: 0x0008DA44 File Offset: 0x0008BE44
		private IEnumerator ShowPending(ChestView chestView)
		{
			BackButtonManager.Instance.RemoveAction(new Action(this.Close));
			this.eventSystem.Disable();
			this.UpdateStartViewVisibility(true);
			chestView.SetupView(false, this.gameStateService);
			yield return new WaitForSeconds(this.chestAnimationDelay);
			chestView.PlayOpenAnimation();
			yield return new WaitForSeconds(chestView.AnimationTime * 1.25f);
			Wooroutine<TownRewardsRoot> scene = SceneManager.Instance.LoadSceneWithParams<TownRewardsRoot, List<TownReward>>(null, null);
			yield return scene;
			this.eventSystem.Enable();
			yield return scene.ReturnValue.onDestroyed;
			BackButtonManager.Instance.AddAction(new Action(this.Close));
			chestView.CloseBubble();
			yield break;
		}

		// Token: 0x060021BA RID: 8634 RVA: 0x0008DA66 File Offset: 0x0008BE66
		private void SnapUiToLevel(int level)
		{
			this.inputController.ScrollToLevel(level);
		}

		// Token: 0x060021BB RID: 8635 RVA: 0x0008DA74 File Offset: 0x0008BE74
		private void Close()
		{
			this.audioService.PlaySFX(AudioId.PopupHideDefault, false, false, false);
			BackButtonManager.Instance.RemoveAction(new Action(this.Close));
			if (this.dialog != null)
			{
				this.dialog.Hide();
			}
			bool placeTrophy = this.activeLevel == this.gameStateService.DiveForTreasure.SecondChestLevel() + 1;
			WooroutineRunner.StartCoroutine(this.ReturnToIslandFlow(placeTrophy), null);
		}

		// Token: 0x060021BC RID: 8636 RVA: 0x0008DAF0 File Offset: 0x0008BEF0
		private IEnumerator RefreshFriendsFromFacebookRoutine()
		{
			if (!this.facebookService.LoggedIn())
			{
				yield break;
			}
			Wooroutine<FacebookAPIRunner.FriendListData> friends = this.facebookService.GetFriends(FacebookData.Friend.Type.Playing);
			yield return friends;
			this.friendsData = this.ConvertFriends(friends.ReturnValue.friends).ToList<TownOptionsFriendData>();
			foreach (DiveForTreasureFriendView diveForTreasureFriendView in this.friendViews)
			{
				diveForTreasureFriendView.UpdateFacebookFriends(this.friendsData);
			}
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

		// Token: 0x060021BD RID: 8637 RVA: 0x0008DB0C File Offset: 0x0008BF0C
		private IEnumerable<TownOptionsFriendData> ConvertFriends(IEnumerable<FacebookData.Friend> friends)
		{
			yield return this.ConvertFriend(this.facebookService.Me, true);
			foreach (FacebookData.Friend friend in friends)
			{
				yield return this.ConvertFriend(friend, false);
			}
			yield break;
		}

		// Token: 0x060021BE RID: 8638 RVA: 0x0008DB38 File Offset: 0x0008BF38
		private TownOptionsFriendData ConvertFriend(FacebookData.Friend friend, bool isMe)
		{
			bool flag = this.facebookService.FriendProgress(friend.ID).DftLevel.eventId == this.gameStateService.DiveForTreasure.EventId;
			int num = (!flag) ? 1 : this.facebookService.FriendProgress(friend.ID).DftLevel.progress;
			return new TownOptionsFriendData
			{
				friend = friend,
				avatar = this.facebookService.GetProfilePicture(friend),
				level = ((!isMe) ? num : this.activeLevel),
				type = ((!isMe) ? TownOptionsFriendData.Type.Friend : TownOptionsFriendData.Type.Me)
			};
		}

		// Token: 0x060021BF RID: 8639 RVA: 0x0008DBF8 File Offset: 0x0008BFF8
		private IEnumerator ReturnToIslandFlow(bool placeTrophy)
		{
			TownLoadingFlowWithTransition.Input lScreenInput = new TownLoadingFlowWithTransition.Input(LoadingScreenConfig.Random);
			Wooroutine<TownMainRoot> returnToTownFlow = new TownLoadingFlowWithTransition().Start(lScreenInput);
			yield return returnToTownFlow;
			this.tournamentService.NotifyContextChange(SceneContext.MetaGame);
			returnToTownFlow.ReturnValue.StartView(true, false);
			if (placeTrophy)
			{
				BuildingConfig buildingConfig = this.configService.buildingConfigList.GetConfig("iso_trophy_treasure_dive");
				buildingConfig.TrackingDetail = "treasure_diving";
				yield return new ForceUserPlaceDecoFlow(buildingConfig).Start();
			}
			yield break;
		}

		// Token: 0x04004CE2 RID: 19682
		[WaitForService(true, true)]
		private AudioService audioService;

		// Token: 0x04004CE3 RID: 19683
		[WaitForService(true, true)]
		private GameStateService gameStateService;

		// Token: 0x04004CE4 RID: 19684
		[WaitForService(true, true)]
		private SBSService sbsService;

		// Token: 0x04004CE5 RID: 19685
		[WaitForService(true, true)]
		private TrackingService trackingService;

		// Token: 0x04004CE6 RID: 19686
		[WaitForService(true, true)]
		private ProgressionDataService.Service progressionService;

		// Token: 0x04004CE7 RID: 19687
		[WaitForService(true, true)]
		private FacebookService facebookService;

		// Token: 0x04004CE8 RID: 19688
		[WaitForService(true, true)]
		private TournamentService tournamentService;

		// Token: 0x04004CE9 RID: 19689
		[WaitForService(true, true)]
		private ConfigService configService;

		// Token: 0x04004CEA RID: 19690
		[WaitForRoot(false, false)]
		private EventSystemRoot eventSystem;

		// Token: 0x04004CEB RID: 19691
		[SerializeField]
		private AnimatedUi dialog;

		// Token: 0x04004CEC RID: 19692
		[SerializeField]
		private Button closeButton;

		// Token: 0x04004CED RID: 19693
		[SerializeField]
		private DiveForTreasureCameraInputController inputController;

		// Token: 0x04004CEE RID: 19694
		[SerializeField]
		private DiveForTreasureLevelView previousLevelPrefab;

		// Token: 0x04004CEF RID: 19695
		[SerializeField]
		private DiveForTreasureLevelView currentLevelPrefab;

		// Token: 0x04004CF0 RID: 19696
		[SerializeField]
		private DiveForTreasureLevelView futureLevelPrefab;

		// Token: 0x04004CF1 RID: 19697
		[SerializeField]
		private List<Transform> buttonPlacements;

		// Token: 0x04004CF2 RID: 19698
		[SerializeField]
		private List<DiveForTreasureFriendView> friendViews;

		// Token: 0x04004CF3 RID: 19699
		[SerializeField]
		private CountdownTimer countdownTimer;

		// Token: 0x04004CF4 RID: 19700
		[SerializeField]
		private GameObject tutorialMarkerObject;

		// Token: 0x04004CF5 RID: 19701
		[SerializeField]
		private GameObject secondTutorialMarkerObject;

		// Token: 0x04004CF6 RID: 19702
		[SerializeField]
		private GameObject topUI;

		// Token: 0x04004CF7 RID: 19703
		[SerializeField]
		private ChestView firstChestView;

		// Token: 0x04004CF8 RID: 19704
		[SerializeField]
		private ChestView secondChestView;

		// Token: 0x04004CF9 RID: 19705
		[SerializeField]
		private List<GameObject> firstVariationItems;

		// Token: 0x04004CFA RID: 19706
		[SerializeField]
		private List<GameObject> secondVariationItems;

		// Token: 0x04004CFB RID: 19707
		[SerializeField]
		private float chestAnimationDelay = 0.5f;

		// Token: 0x04004CFC RID: 19708
		[Header("Testing")]
		[SerializeField]
		private int activeLevel = 8;

		// Token: 0x04004CFD RID: 19709
		private TutorialRunner tutorialRunner;

		// Token: 0x04004CFE RID: 19710
		private List<TownOptionsFriendData> friendsData;

		// Token: 0x04004CFF RID: 19711
		private MusicPlayer musicPlayer;
	}
}
