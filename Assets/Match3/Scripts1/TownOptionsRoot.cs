using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Match3.Scripts1.Audio;
using Match3.Scripts1.Localization;
using Match3.Scripts1.Puzzletown.Build;
using Match3.Scripts1.Puzzletown.Datasources;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Puzzletown.UI;
using Match3.Scripts1.UnityEngine;
using Wooga.Coroutines;
using Wooga.UnityFramework;
using Match3.Scripts2.Network;
using Match3.Scripts2.PlayerData;
using Match3.Scripts2.PromptBar;
using TMPro;
using UnityEngine;
using UnityEngine.UI; //using Facebook.Unity;

// Token: 0x020009E9 RID: 2537
namespace Match3.Scripts1
{
	public class TownOptionsRoot : APtSceneRoot, IHandler<PopupOperation>, IHandler<TownOptionsCommand>, IHandler<ToggleSetting, bool>, IHandler<WoogaSystemLanguage>, IHandler<TownOptionsInboxOperation, string>, IHandler<TownOptionsFriendOperation, string>, IPersistentDialog
	{
		// Token: 0x06003D36 RID: 15670 RVA: 0x00134D0A File Offset: 0x0013310A
		public void SetHarmonyObserver(VillageRankHarmonyObserver harmonyObserver)
		{
			this._harmonyObserver = harmonyObserver;
		}

		// Token: 0x06003D37 RID: 15671 RVA: 0x00134D14 File Offset: 0x00133114
		public static IEnumerable<T> GetEnumValues<T>()
		{
			IEnumerator enumerator = Enum.GetValues(typeof(T)).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object value = enumerator.Current;
					yield return (T)((object)value);
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
			yield break;
		}

		// Token: 0x06003D38 RID: 15672 RVA: 0x00134D30 File Offset: 0x00133130
		private void Start()
		{
			// 显示版本号和玩家id
			if (string.IsNullOrEmpty(ToServer.userId))
			{
				Debug.LogError("userid is null");
			}
			else
			{
				userId.text = ToServer.userId;
			}

			version.text = BuildVersion.Version;
		
			base.gameObject.SetActive(false);
		}

		// Token: 0x06003D39 RID: 15673 RVA: 0x00134D40 File Offset: 0x00133140
		protected override void OnDisable()
		{
			base.OnDisable();
			base.StopCoroutine(this.RefreshUIRoutine());
			if (this.facebookService != null)
			{
				// this.facebookService.RemoveRequestSentListener(new Action<FacebookRequest, IAppRequestResult>(this.OnMessageSent));
				// this.facebookService.Friends.UserRequestsSent.RemoveListener(new Action<IEnumerable<string>>(this.UserRequestsSent));
			}
		}

		// Token: 0x06003D3A RID: 15674 RVA: 0x00134DA4 File Offset: 0x001331A4
		protected override void OnEnable()
		{
			if (this.facebookService != null)
			{
				// this.facebookService.AddRequestSentListener(new Action<FacebookRequest, IAppRequestResult>(this.OnMessageSent));
				// this.facebookService.Friends.UserRequestsSent.AddListener(new Action<IEnumerable<string>>(this.UserRequestsSent));
			}
			base.StartCoroutine(this.RefreshUIRoutine());
		}

		// Token: 0x06003D3B RID: 15675 RVA: 0x00134E04 File Offset: 0x00133204
		protected override void Go()
		{
			this.ShowOnChildren(this.facebookService, true, true);
			foreach (UiToggle uiToggle in base.GetComponentsInChildren<UiToggle>(true))
			{
				bool value = false;
				if (uiToggle is UiToggleSetting)
				{
					value = this.settings.GetToggle(((UiToggleSetting)uiToggle).GetSetting());
				}
				else if (uiToggle is UiToggleLanguage)
				{
					UiToggleLanguage uiToggleLanguage = (UiToggleLanguage)uiToggle;
					bool active = this.localisation.IsLanguageAvailable(uiToggleLanguage.language);
					uiToggleLanguage.gameObject.SetActive(active);
					value = (this.localisation.Language == uiToggleLanguage.language);
				}
				uiToggle.Show(value);
			}
		}

		// Token: 0x06003D3C RID: 15676 RVA: 0x00134EB9 File Offset: 0x001332B9
		private void UserRequestsSent(IEnumerable<string> users)
		{
			this.RefreshExistingFriends();
		}

		// Token: 0x06003D3D RID: 15677 RVA: 0x00134EC1 File Offset: 0x001332C1
		private void OnFBStatusChanged(FacebookService.Status status)
		{
			this.Refresh(this.selectedTab);
		}

		// Token: 0x06003D3E RID: 15678 RVA: 0x00134ED0 File Offset: 0x001332D0
		public void Open(TownOptionsCommand tab)
		{
			this.ExecuteOnChild(delegate(Canvas c)
			{
				c.enabled = true;
			});
			base.Enable();
			this.Handle(tab);
			this.dialog.Show();
			this.Refresh(tab);
			BackButtonManager.Instance.AddAction(new Action(this.Close));
			this.audioService.PlaySFX(AudioId.MenuSlideOpen, false, false, false);
			this.helpShift.TriggerRefresh();
		}

		// Token: 0x06003D3F RID: 15679 RVA: 0x00134F54 File Offset: 0x00133354
		private IEnumerator RefreshInboxRoutine()
		{
			Wooroutine<IEnumerable<FacebookData.Request>> requests = this.StartWooroutine<IEnumerable<FacebookData.Request>>(this.facebookService.FetchRequests());
			yield return requests;
			this.inboxData = (from r in requests.ReturnValue
				where !r.deleted
				select r).Select(delegate(FacebookData.Request r)
			{
				FacebookData.Friend friend = new FacebookData.Friend(r.fromID, r.fromName);
				return new TownOptionsInboxData
				{
					friend = friend,
					enableSend = !r.isResponse,
					avatar = this.facebookService.GetProfilePicture(friend),
					requestId = r.ID
				};
			}).ToArray<TownOptionsInboxData>();
			this.inboxDataSource.Show(this.inboxData);
			foreach (TownOptionsInboxData cInboxItem in this.inboxData)
			{
				if (this.facebookService.Friends.GetRequestFromID(cInboxItem.friend.ID) != null)
				{
					Wooroutine<FacebookService.BoxedSprite> cPicture = this.facebookService.LoadProfilePicture(cInboxItem.friend);
					yield return cPicture;
					if (cPicture.ReturnValue.spr != null)
					{
						cInboxItem.avatar = cPicture.ReturnValue.spr;
						cInboxItem.OnAvatarAvailable.Dispatch(cInboxItem.friend.ID, cPicture.ReturnValue.spr);
					}
				}
			}
			this.handledFacebookRequestIDs.Clear();
			yield break;
		}

		// Token: 0x06003D40 RID: 15680 RVA: 0x00134F6F File Offset: 0x0013336F
		//private void OnMessageSent(FacebookRequest request, IAppRequestResult result)
		//{
		//	this.RefreshExistingFriends();
		//}

		// Token: 0x06003D41 RID: 15681 RVA: 0x00134F78 File Offset: 0x00133378
		private void RefreshExistingFriends()
		{
			foreach (TownOptionsFriendData townOptionsFriendData in this.friendData)
			{
				townOptionsFriendData.nextLifeAvailable = this.facebookService.Friends.TimeLifeRequestAvailable(townOptionsFriendData.friend.ID);
			}
			if (this.friendData != null)
			{
				this.friendsDataSource.Show(this.friendData);
			}
		}

		// Token: 0x06003D42 RID: 15682 RVA: 0x00134FE1 File Offset: 0x001333E1
		private void RefreshInboxCount()
		{
			this.inboxMessagesGameObjects.ForEach(delegate(GameObject inboxNotification)
			{
				inboxNotification.SetActive(this.helpShift.inboxCount > 0);
			});
			this.inboxMessagesCounts.ForEach(delegate(TMP_Text textField)
			{
				textField.text = this.helpShift.inboxCount.ToString();
			});
		}

		// Token: 0x06003D43 RID: 15683 RVA: 0x00135014 File Offset: 0x00133414
		private void RefreshGooglePlayGamesUI()
		{
			// this.googlePlayGamesLogInButton.SetActive(!this.externalGamesService.IsLoggedIn);
			// this.googlePlayGamesLogOutButton.SetActive(this.externalGamesService.IsLoggedIn);
			// this.googlePlayGamesShowAchievementsButton.SetActive(this.externalGamesService.IsLoggedIn);
		}

		// Token: 0x06003D44 RID: 15684 RVA: 0x00135068 File Offset: 0x00133468
		private IEnumerator RefreshFriendsFromFacebookRoutine()
		{
			if (!this.facebookService.LoggedIn())
			{
				yield break;
			}
			this.friendData = new TownOptionsFriendData[0];
			if (this.friendData.Length == 0)
			{
				Wooroutine<FacebookAPIRunner.FriendListData> friends = this.facebookService.GetFriends(FacebookData.Friend.Type.Playing);
				yield return friends;
				this.friendData = (from r in friends.ReturnValue.friends
					select new TownOptionsFriendData
					{
						friend = r,
						nextLifeAvailable = this.facebookService.Friends.TimeLifeRequestAvailable(r.ID),
						avatar = this.facebookService.GetProfilePicture(r),
						harmony = this.facebookService.FriendProgress(r.ID).Harmony,
						level = this._harmonyObserver.RankForHarmony(this.facebookService.FriendProgress(r.ID).Harmony).village_rank,
						enableSend = true
					}).ToArray<TownOptionsFriendData>();
			}
			List<TownOptionsFriendData> cFriends = this.friendData.ToList<TownOptionsFriendData>();
			cFriends.Add(new TownOptionsFriendData
			{
				friend = new FacebookData.Friend(this.facebookService.FB_MY_ID, this.localisation.GetText("ui.friendslist.me", new LocaParam[0])),
				avatar = this.facebookService.GetProfilePicture(this.facebookService.Me),
				harmony = this.gameState.Resources.GetAmount("harmony"),
				level = this._harmonyObserver.RankForHarmony(this.gameState.Resources.GetAmount("harmony")).village_rank
			});
			TownOptionsFriendData[] sortedFriends = (from r in cFriends
				orderby r.harmony descending
				select r).ToArray<TownOptionsFriendData>();
			for (int i = 0; i < sortedFriends.Length; i++)
			{
				sortedFriends[i].index = i + 1;
			}
			this.friendData = sortedFriends.ToArray<TownOptionsFriendData>();
			this.friendsDataSource.Show(this.friendData);
			foreach (TownOptionsFriendData cFriend in this.friendData)
			{
				if (cFriend.avatar == null)
				{
					Wooroutine<FacebookService.BoxedSprite> cPicture = this.facebookService.LoadProfilePicture(cFriend.friend);
					yield return cPicture;
					if (cPicture.ReturnValue.spr != null)
					{
						cFriend.avatar = cPicture.ReturnValue.spr;
						cFriend.OnAvatarAvailable.Dispatch(cFriend.friend.ID, cPicture.ReturnValue.spr);
					}
				}
			}
			yield break;
		}

		// Token: 0x06003D45 RID: 15685 RVA: 0x00135084 File Offset: 0x00133484
		public void Refresh(TownOptionsCommand tab)
		{
			this.SetupNewIslandUiLayoutABTest();
			this.tabsDataSource.Show(this.GetTabs());
			if (tab == TownOptionsCommand.Inbox)
			{
				this.townBottomPanelRoot.Refresh();
				base.StartCoroutine(this.RefreshInboxRoutine());
			}
			else if (tab == TownOptionsCommand.Friends)
			{
				base.StartCoroutine(this.RefreshFriendsFromFacebookRoutine());
			}
			this.audioService.PlaySFX(AudioId.TabClick, false, false, false);
			this.ExecuteOnChildren(delegate(TownOptionsPanelView view)
			{
				view.Show(tab);
			}, true);
			foreach (UiToggleDetails uiToggleDetails in base.GetComponentsInChildren<UiToggleDetails>())
			{
				uiToggleDetails.Show(false);
			}
			this.RefreshInboxCount();
			this.RefreshGooglePlayGamesUI();
		}

		// Token: 0x06003D46 RID: 15686 RVA: 0x00135150 File Offset: 0x00133550
		private void SetupNewIslandUiLayoutABTest()
		{
			bool new_island_ui_layout = this.configService.FeatureSwitchesConfig.new_island_ui_layout;
			bool flag = new_island_ui_layout && this.progression.UnlockedLevel >= 31;
			this.tropicamButtonFriends.SetActive(flag);
			this.tropicamNoFriendsNew.SetActive(flag);
			this.tropicamNoFriendsOld.SetActive(!flag);
		}

		// Token: 0x06003D47 RID: 15687 RVA: 0x001351B4 File Offset: 0x001335B4
		private IEnumerator RefreshUIRoutine()
		{
			WaitForSeconds waitForHalfSecond = new WaitForSeconds(0.5f);
			while (base.gameObject.activeInHierarchy)
			{
				yield return waitForHalfSecond;
				this.RefreshInboxCount();
				this.RefreshGooglePlayGamesUI();
			}
			yield break;
		}

		// Token: 0x06003D48 RID: 15688 RVA: 0x001351CF File Offset: 0x001335CF
		private TownOptionsTabData[] GetTabs()
		{
			return Array.ConvertAll<TownOptionsCommand, TownOptionsTabData>(this.tabs, (TownOptionsCommand tab) => this.GetTabData(tab));
		}

		// Token: 0x06003D49 RID: 15689 RVA: 0x001351E8 File Offset: 0x001335E8
		private void DiscardInbox(string uniqueId)
		{
			this.handledFacebookRequestIDs.Add(uniqueId);
			this.facebookService.DeleteRequest(uniqueId);
		}

		// Token: 0x06003D4A RID: 15690 RVA: 0x00135204 File Offset: 0x00133604
		private TownOptionsTabData GetTabData(TownOptionsCommand tab)
		{
			return new TownOptionsTabData
			{
				data = tab,
				state = ((tab != this.selectedTab) ? UiTabState.Inactive : UiTabState.Active)
			};
		}

		// Token: 0x06003D4B RID: 15691 RVA: 0x00135238 File Offset: 0x00133638
		public void Close()
		{
			this.townBottomPanelRoot.Refresh();
			this.dialog.Hide();
			BackButtonManager.Instance.RemoveAction(new Action(this.Close));
			this.audioService.PlaySFX(AudioId.MenuSlideClose, false, false, false);
		}

		// Token: 0x06003D4C RID: 15692 RVA: 0x00135285 File Offset: 0x00133685
		public void Handle(PopupOperation op)
		{
			if (op == PopupOperation.Close)
			{
				this.Close();
			}
		}

		// Token: 0x06003D4D RID: 15693 RVA: 0x001352A0 File Offset: 0x001336A0
		private IEnumerator RunFlowRoutine(Coroutine flowRoutine)
		{
			yield return flowRoutine;
			this.Close();
			yield break;
		}

		// Token: 0x06003D4E RID: 15694 RVA: 0x001352C2 File Offset: 0x001336C2
		private void StartFlow(Coroutine flowRoutine)
		{
			base.StartCoroutine(this.RunFlowRoutine(flowRoutine));
		}

		// Token: 0x06003D4F RID: 15695 RVA: 0x001352D4 File Offset: 0x001336D4
		public void Handle(TownOptionsCommand tab)
		{
			switch (tab)
			{
				case TownOptionsCommand.AddFriends:
					this.Close();
					if (Application.internetReachability != NetworkReachability.NotReachable)
					{
						MultiFriendsSelectorRoot.FriendSelectorType selectorType = MultiFriendsSelectorRoot.FriendSelectorType.inviteFriends;
						TownOptionsCommand townOptionsCommand = this.selectedTab;
						if (townOptionsCommand != TownOptionsCommand.Friends)
						{
							if (townOptionsCommand != TownOptionsCommand.Inbox)
							{
								if (townOptionsCommand == TownOptionsCommand.Settings)
								{
									selectorType = MultiFriendsSelectorRoot.FriendSelectorType.inviteFriendsSettings;
								}
							}
							else
							{
								selectorType = MultiFriendsSelectorRoot.FriendSelectorType.invteFriendsInbox;
							}
						}
						else
						{
							selectorType = MultiFriendsSelectorRoot.FriendSelectorType.inviteFriends;
						}
						this.facebookService.AddFriends(selectorType);
					}
					else
					{
						SceneManager.Instance.LoadSceneWithParams<PopupFacebookRoot, FacebookPopupParameters>(new FacebookPopupParameters(FacebookPopupState.NoConnection, FacebookLoginContext.settings), null);
					}
					break;
				case TownOptionsCommand.AcceptAllInbox:
					this.AcceptAllAndSend();
					break;
				case TownOptionsCommand.LogIn:
					this.StartFlow(new FacebookLoginFlow(FacebookLoginContext.settings).Start());
					break;
				case TownOptionsCommand.LogOut:
					this.StartFlow(new FacebookLogoutFlow().Start());
					break;
				case TownOptionsCommand.RequestSupport:
					this.helpShift.ShowFAQ();
					this.helpShift.TriggerRefresh();
					break;
				case TownOptionsCommand.SendLives:
					this.Close();
					if (Application.internetReachability != NetworkReachability.NotReachable)
					{
						new SendLivesJourney().Start();
					}
					else
					{
						SceneManager.Instance.LoadSceneWithParams<PopupFacebookRoot, FacebookPopupParameters>(new FacebookPopupParameters(FacebookPopupState.NoConnection, FacebookLoginContext.settings), null);
					}
					break;
				case TownOptionsCommand.ChangeName:
					SceneManager.Instance.LoadScene<EnterNameDialogRoot>(null);
					break;
				case TownOptionsCommand.News:
					this.helpShift.ShowInbox();
					break;
				case TownOptionsCommand.GooglePlayGamesLogIn:
					this.externalGamesService.LogIn(false, false);
					break;
				case TownOptionsCommand.GooglePlayGamesLogOut:
					this.externalGamesService.LogOut();
					break;
				case TownOptionsCommand.GooglePlayGamesShowAchievements:
					this.externalGamesService.ShowAchievementsUi();
					break;
				case TownOptionsCommand.Tropicam:
					this.Close();
					new TropicamFlow(this.townBottomPanelRoot, this.progression).Start();
					break;
				case TownOptionsCommand.CopyEmail:
					GUIUtility.systemCopyBuffer = "morekurumi@gmail.com";
					PopupPromptBarRoot.ShowPrompt(game.prompt_copy_successful);
					break;
			}
			if (Array.IndexOf<TownOptionsCommand>(this.tabs, tab) != -1)
			{
				if (this.selectedTab != tab)
				{
					this.selectedTab = tab;
					this.Refresh(this.selectedTab);
				}
			}
			else
			{
				WoogaDebug.LogFormatted("Received an operation {0}", new object[]
				{
					tab
				});
			}
		}

		// Token: 0x06003D50 RID: 15696 RVA: 0x001354FC File Offset: 0x001338FC
		public void Handle(ToggleSetting setting, bool value)
		{
			this.settings.SetToggle(setting, value);
			switch (setting)
			{
				case ToggleSetting.Music:
					this.audioService.ChangeSetting(setting, value);
					break;
				case ToggleSetting.Sound:
					this.audioService.ChangeSetting(setting, value);
					break;
				case ToggleSetting.InactivityNotifications:
					this.localNotificationService.ChangeSetting(setting, value);
					break;
				case ToggleSetting.LivesFullNotification:
					this.localNotificationService.ChangeSetting(setting, value);
					break;
				case ToggleSetting.NewLevelsAvailableNotification:
					this.localNotificationService.ChangeSetting(setting, value);
					break;
				case ToggleSetting.HarvestBuildNotifications:
					this.localNotificationService.ChangeSetting(setting, value);
					break;
				case ToggleSetting.SpinAvailableNotification:
					this.localNotificationService.ChangeSetting(setting, value);
					break;
				case ToggleSetting.FriendActionsNotification:
					this.pushNotificationService.SetNotificationsEnabled(value);
					break;
				case ToggleSetting.AdPersonalisation:
					this.videoAdService.ChangeSetting(setting, value);
					break;
			}
		}

		// Token: 0x06003D51 RID: 15697 RVA: 0x001355EA File Offset: 0x001339EA
		public void Handle(WoogaSystemLanguage language)
		{
			if (this.localisation.Language != language)
			{
				this.localisation.ChangeLanguage(language);
			}
		}

		// Token: 0x06003D52 RID: 15698 RVA: 0x0013560C File Offset: 0x00133A0C
		private void AcceptAndSend(string inboxID)
		{
			FacebookData.Request requestFromID = this.facebookService.Friends.GetRequestFromID(inboxID);
			this.livesService.AddLives(1);
			this.DiscardInbox(inboxID);
			if (requestFromID == null)
			{
				return;
			}
			this.facebookService.Friends.SendGiftLives(new string[]
			{
				requestFromID.fromID
			}, true, "inbox", "accept_send");
			requestFromID.type = "send_life";
			this.trackingService.TrackRequestResponse(requestFromID, "inbox", "accept_send");
		}

		// Token: 0x06003D53 RID: 15699 RVA: 0x00135690 File Offset: 0x00133A90
		private void AcceptOnly(string inboxID)
		{
			FacebookData.Request requestFromID = this.facebookService.Friends.GetRequestFromID(inboxID);
			this.livesService.AddLives(1);
			this.DiscardInbox(inboxID);
			if (requestFromID == null)
			{
				return;
			}
			requestFromID.type = "send_life";
			this.trackingService.TrackRequestResponse(requestFromID, "inbox", "accept");
		}

		// Token: 0x06003D54 RID: 15700 RVA: 0x001356EC File Offset: 0x00133AEC
		private void AcceptAllAndSend()
		{
			foreach (TownOptionsInboxData townOptionsInboxData in this.inboxData)
			{
				FacebookData.Request requestFromID = this.facebookService.Friends.GetRequestFromID(townOptionsInboxData.requestId);
				if (requestFromID != null)
				{
					if (requestFromID.isResponse)
					{
						this.AcceptOnly(townOptionsInboxData.requestId);
					}
					else
					{
						this.AcceptAndSend(townOptionsInboxData.requestId);
					}
				}
			}
			base.StartCoroutine(this.RefreshInboxRoutine());
		}

		// Token: 0x06003D55 RID: 15701 RVA: 0x00135770 File Offset: 0x00133B70
		private void Update()
		{
			if (this.helpShift != null && this.notificationCount != this.helpShift.notificationCount)
			{
				this.notificationCount = this.helpShift.notificationCount;
				this.HelpshiftMessageCount.text = this.notificationCount.ToString();
				this.HelpshiftMessages.gameObject.SetActive(this.notificationCount > 0);
			}
		}

		// Token: 0x06003D56 RID: 15702 RVA: 0x001357E4 File Offset: 0x00133BE4
		public void Handle(TownOptionsInboxOperation op, string inboxID)
		{
			if (this.handledFacebookRequestIDs.Contains(inboxID))
			{
				WoogaDebug.LogWarning(new object[]
				{
					"[TownOptionsRoot] Request already handled",
					inboxID
				});
				return;
			}
			if (op != TownOptionsInboxOperation.AcceptAndSend)
			{
				if (op != TownOptionsInboxOperation.AcceptOnly)
				{
					if (op == TownOptionsInboxOperation.Discard)
					{
						this.DiscardInbox(inboxID);
					}
				}
				else
				{
					this.AcceptOnly(inboxID);
				}
			}
			else
			{
				this.AcceptAndSend(inboxID);
			}
			this.townBottomPanelRoot.Refresh();
			this.Refresh(this.selectedTab);
		}

		// Token: 0x06003D57 RID: 15703 RVA: 0x00135870 File Offset: 0x00133C70
		private void RequestLives(string inboxID)
		{
			this.facebookService.Friends.SendRequestLives(this.localisation.GetText("ui.social.send_lives", new LocaParam[0]), this.localisation.GetText("ui.lives.ask_for_more.body", new LocaParam[0]), new string[]
			{
				inboxID
			}, "friendlist");
		}

		// Token: 0x06003D58 RID: 15704 RVA: 0x001358C8 File Offset: 0x00133CC8
		private void StartVisitFriendFlow(string inboxID)
		{
			new VisitFriendFlow().Start(inboxID);
		}

		// Token: 0x06003D59 RID: 15705 RVA: 0x001358D6 File Offset: 0x00133CD6
		private void SaveCameraPosition()
		{
			TownOptionsRoot.s_cameraPosition = new Vector3?(CameraInputController.CameraPosition);
		}

		// Token: 0x06003D5A RID: 15706 RVA: 0x001358E8 File Offset: 0x00133CE8
		public void Handle(TownOptionsFriendOperation op, string inboxID)
		{
			if (op != TownOptionsFriendOperation.SendLife)
			{
				if (op != TownOptionsFriendOperation.Visit)
				{
					WoogaDebug.LogFormatted("Facebook friend visit: Friend operation requested - {0}", new object[]
					{
						op
					});
				}
				else
				{
					this.SaveCameraPosition();
					this.StartVisitFriendFlow(inboxID);
				}
			}
			else
			{
				this.RequestLives(inboxID);
			}
		}

		// Token: 0x040065F6 RID: 26102
		[WaitForRoot(false, false)]
		private EventSystemRoot eventSystem;

		// Token: 0x040065F7 RID: 26103
		[WaitForRoot(false, false)]
		private TownBottomPanelRoot townBottomPanelRoot;

		// Token: 0x040065F8 RID: 26104
		[WaitForService(true, true)]
		private GameSettingsService settings;

		// Token: 0x040065F9 RID: 26105
		[WaitForService(true, true)]
		private FacebookService facebookService;

		// Token: 0x040065FA RID: 26106
		[WaitForService(true, true)]
		private SBSService sbsService;

		// Token: 0x040065FB RID: 26107
		[WaitForService(true, true)]
		private GameStateService gameState;

		// Token: 0x040065FC RID: 26108
		[WaitForService(true, true)]
		private ConfigService configService;

		// Token: 0x040065FD RID: 26109
		[WaitForService(true, true)]
		private AudioService audioService;

		// Token: 0x040065FE RID: 26110
		[WaitForService(true, true)]
		private TrackingService trackingService;

		// Token: 0x040065FF RID: 26111
		[WaitForService(true, true)]
		private LivesService livesService;

		// Token: 0x04006600 RID: 26112
		[WaitForService(true, true)]
		private HelpshiftService helpShift;

		// Token: 0x04006601 RID: 26113
		[WaitForService(true, true)]
		private ILocalizationService localisation;

		// Token: 0x04006602 RID: 26114
		[WaitForService(true, true)]
		private LocalNotificationService localNotificationService;

		// Token: 0x04006603 RID: 26115
		[WaitForService(true, true)]
		private PushNotificationService pushNotificationService;

		// Token: 0x04006604 RID: 26116
		[WaitForService(true, true)]
		private ExternalGamesService externalGamesService;

		// Token: 0x04006605 RID: 26117
		[WaitForService(true, true)]
		private IVideoAdService videoAdService;

		// Token: 0x04006606 RID: 26118
		[WaitForService(true, true)]
		private ProgressionDataService.Service progression;

		// Token: 0x04006607 RID: 26119
		private TownOptionsCommand selectedTab;

		// Token: 0x04006608 RID: 26120
		[SerializeField]
		private AnimatedUi dialog;

		// Token: 0x04006609 RID: 26121
		[SerializeField]
		private TownOptionsCommand[] tabs;

		// Token: 0x0400660A RID: 26122
		[SerializeField]
		private TownOptionsTabsDataSource tabsDataSource;

		// Token: 0x0400660B RID: 26123
		[Header("Inbox")]
		[SerializeField]
		private TownOptionsInboxDataSource inboxDataSource;

		// Token: 0x0400660C RID: 26124
		[SerializeField]
		private TownOptionsFriendsDataSource friendsDataSource;

		// Token: 0x0400660D RID: 26125
		[SerializeField]
		private List<GameObject> inboxMessagesGameObjects;

		// Token: 0x0400660E RID: 26126
		[SerializeField]
		private List<TMP_Text> inboxMessagesCounts;

		// Token: 0x0400660F RID: 26127
		[Header("Google Play")]
		[SerializeField]
		private GameObject googlePlayGamesPanel;

		// Token: 0x04006610 RID: 26128
		[SerializeField]
		private GameObject googlePlayGamesLogInButton;

		// Token: 0x04006611 RID: 26129
		[SerializeField]
		private GameObject googlePlayGamesLogOutButton;

		// Token: 0x04006612 RID: 26130
		[SerializeField]
		private GameObject googlePlayGamesShowAchievementsButton;

		// Token: 0x04006613 RID: 26131
		[Header("Helpshift")]
		[SerializeField]
		private TMP_Text HelpshiftMessageCount;

		// Token: 0x04006614 RID: 26132
		[SerializeField]
		private GameObject HelpshiftMessages;

		// Token: 0x04006615 RID: 26133
		[Header("Tropicam")]
		[SerializeField]
		private GameObject tropicamButtonFriends;

		// Token: 0x04006616 RID: 26134
		[SerializeField]
		private GameObject tropicamNoFriendsNew;

		// Token: 0x04006617 RID: 26135
		[SerializeField]
		private GameObject tropicamNoFriendsOld;

		// Token: 0x04006618 RID: 26136
		public static Vector3? s_cameraPosition;

		// Token: 0x04006619 RID: 26137
		private HashSet<string> handledFacebookRequestIDs = new HashSet<string>();

		// Token: 0x0400661A RID: 26138
		private VillageRankHarmonyObserver _harmonyObserver;

		// Token: 0x0400661B RID: 26139
		private int notificationCount = -1;

		// Token: 0x0400661C RID: 26140
		private TownOptionsFriendData[] friendData = new TownOptionsFriendData[0];

		// Token: 0x0400661D RID: 26141
		private TownOptionsInboxData[] inboxData = new TownOptionsInboxData[0];

		[Header("ClientInfo")] 
		public Text userId;
		public Text version;
	}
}
