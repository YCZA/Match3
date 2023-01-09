using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Match3.Scripts1.Audio;
using Match3.Scripts1.Puzzletown.Datasources;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.UnityEngine;
using Wooga.Coroutines;
using Wooga.UnityFramework;
using TMPro;
using UnityEngine;

// Token: 0x020008AD RID: 2221
namespace Match3.Scripts1
{
	public class MultiFriendsSelectorRoot : APtSceneRoot<string>, IHandler<MultiFriendsSelectorFriendData>, IHandler<MultiFriendsSelectOperation>, IHandler<PopupOperation>, IDisposableDialog
	{
		// Token: 0x06003636 RID: 13878 RVA: 0x00106898 File Offset: 0x00104C98
		public void Handle(PopupOperation op)
		{
			if (op != PopupOperation.Close)
			{
				WoogaDebug.LogWarning(new object[]
				{
					"Unhandled PopupOperation ",
					op.ToString()
				});
			}
			else
			{
				this.CloseWithTracking();
			}
		}

		// Token: 0x06003637 RID: 13879 RVA: 0x001068E4 File Offset: 0x00104CE4
		public void Handle(MultiFriendsSelectOperation op)
		{
			if (this.friendData == null || this.friendData == MultiFriendsSelectorRoot.noFriends)
			{
				return;
			}
			switch (op)
			{
				case MultiFriendsSelectOperation.SelectAll:
					this.SelectAll();
					return;
				case MultiFriendsSelectOperation.AskSelected:
					this.InviteSelected();
					this.CloseWithoutTracking();
					return;
				case MultiFriendsSelectOperation.AskAnyone:
					this.facebookService.AddFriends(MultiFriendsSelectorRoot.FriendSelectorType.inviteFriends);
					return;
			}
			WoogaDebug.LogWarning(new object[]
			{
				"Unhandled MultiFriendsSelectOperation ",
				op.ToString()
			});
		}

		// Token: 0x06003638 RID: 13880 RVA: 0x00106980 File Offset: 0x00104D80
		private void InviteSelected()
		{
			IEnumerable<string> enumerable = from v in this.friendData
				where v.CurrentState == MultiFriendsSelectorFriendData.State.Selected
				select v into f
				select f.friend.ID;
			this.Track("send", enumerable.Count<string>().ToString());
			string requestTitle = (this.currentType != MultiFriendsSelectorRoot.FriendSelectorType.askForLives) ? this.localisation.GetText("ui.social.send_lives", new LocaParam[0]) : this.localisation.GetText("ui.lives.ask_for_more", new LocaParam[0]);
			this.facebookService.Friends.SendRequestLives(requestTitle, this.localisation.GetText("ui.lives.ask_for_more.body", new LocaParam[0]), enumerable, this.parameters);
		}

		// Token: 0x06003639 RID: 13881 RVA: 0x00106A64 File Offset: 0x00104E64
		private void SelectAll()
		{
			MultiFriendsSelectorFriendData.State state = MultiFriendsSelectorFriendData.State.Unselected;
			foreach (MultiFriendsSelectorFriendData multiFriendsSelectorFriendData in this.friendData)
			{
				if (multiFriendsSelectorFriendData.CurrentState == MultiFriendsSelectorFriendData.State.Unselected)
				{
					state = MultiFriendsSelectorFriendData.State.Selected;
				}
			}
			foreach (MultiFriendsSelectorFriendData multiFriendsSelectorFriendData2 in this.friendData)
			{
				multiFriendsSelectorFriendData2.CurrentState = state;
			}
			this.checkMark.gameObject.SetActive(state == MultiFriendsSelectorFriendData.State.Selected);
			this.RefreshData(false);
		}

		// Token: 0x0600363A RID: 13882 RVA: 0x00106AEB File Offset: 0x00104EEB
		public void Handle(MultiFriendsSelectorFriendData friend)
		{
			friend.CurrentState = ((friend.CurrentState != MultiFriendsSelectorFriendData.State.Selected) ? MultiFriendsSelectorFriendData.State.Selected : MultiFriendsSelectorFriendData.State.Unselected);
			this.RefreshData(false);
		}

		// Token: 0x0600363B RID: 13883 RVA: 0x00106B0C File Offset: 0x00104F0C
		private void RefreshData(bool closeIfEmpty)
		{
			MultiFriendsSelectorRoot.ViewData viewData = new MultiFriendsSelectorRoot.ViewData
			{
				FriendsData = MultiFriendsSelectorRoot.noFriends,
				SelectedCount = 0,
				type = this.currentType
			};
			DateTime now = DateTime.UtcNow;
			IEnumerable<MultiFriendsSelectorFriendData> enumerable = from r in this.friendData
				where now > this.facebookService.Friends.TimeLifeRequestAvailable(r.friend.ID)
				select r;
			viewData.FriendsData = enumerable;
			viewData.TotalCount = enumerable.Count<MultiFriendsSelectorFriendData>();
			viewData.SelectedCount = enumerable.CountIf((MultiFriendsSelectorFriendData v) => v.CurrentState == MultiFriendsSelectorFriendData.State.Selected);
			if (closeIfEmpty && viewData.TotalCount == 0)
			{
				this.CloseWithTracking();
				return;
			}
			this.ShowOnChildren(viewData, true, true);
			this.friendsData.Show(viewData.FriendsData);
			BackButtonManager.Instance.AddAction(new Action(this.CloseWithTracking));
		}

		// Token: 0x0600363C RID: 13884 RVA: 0x00106BF4 File Offset: 0x00104FF4
		private IEnumerator RefreshFriendsFromFacebook()
		{
			this.friendsData.Show(null);
			FacebookData.Friend.Type friendType = FacebookData.Friend.Type.Playing;
			MultiFriendsSelectorRoot.FriendSelectorType friendSelectorType = this.currentType;
			if (friendSelectorType != MultiFriendsSelectorRoot.FriendSelectorType.inviteFriends)
			{
				if (friendSelectorType == MultiFriendsSelectorRoot.FriendSelectorType.inviteFriendsAll)
				{
					friendType = FacebookData.Friend.Type.All;
				}
			}
			else
			{
				friendType = FacebookData.Friend.Type.Invitable;
			}
			Wooroutine<FacebookAPIRunner.FriendListData> friends = this.facebookService.GetFriends(friendType);
			yield return friends;
			this.friendData = (from r in friends.ReturnValue.friends
				select new MultiFriendsSelectorFriendData
				{
					friend = r,
					avatar = this.facebookService.GetProfilePicture(r)
				}).ToArray<MultiFriendsSelectorFriendData>();
			this.RefreshData(false);
			foreach (MultiFriendsSelectorFriendData cFriend in this.friendData)
			{
				Wooroutine<FacebookService.BoxedSprite> cPicture = this.facebookService.LoadProfilePicture(cFriend.friend);
				yield return cPicture;
				if (cPicture.ReturnValue.spr != null)
				{
					cFriend.avatar = cPicture.ReturnValue.spr;
					cFriend.OnAvatarAvailable.Dispatch(cFriend.friend.ID, cPicture.ReturnValue.spr);
				}
			}
			yield break;
		}

		// Token: 0x0600363D RID: 13885 RVA: 0x00106C0F File Offset: 0x0010500F
		protected override void Go()
		{
			this.dialog.Show();
			this.RefreshData(false);
		}

		// Token: 0x0600363E RID: 13886 RVA: 0x00106C23 File Offset: 0x00105023
		public Coroutine TryToSend(MultiFriendsSelectorRoot.FriendSelectorType requestType)
		{
			return WooroutineRunner.StartCoroutine(this.TryToSendRoutine(requestType), null);
		}

		// Token: 0x0600363F RID: 13887 RVA: 0x00106C34 File Offset: 0x00105034
		private IEnumerator TryToSendRoutine(MultiFriendsSelectorRoot.FriendSelectorType requestType)
		{
			this.currentType = requestType;
			this.Track("open", string.Empty);
			this.TitleText.text = ((this.currentType != MultiFriendsSelectorRoot.FriendSelectorType.askForLives) ? this.localisation.GetText("ui.social.send_lives", new LocaParam[0]) : this.localisation.GetText("ui.lives.ask_for_more", new LocaParam[0]));
			this.Enable();
			this.friendData = MultiFriendsSelectorRoot.noFriends;
			base.StartCoroutine(this.RefreshFriendsFromFacebook());
			yield return this.onDisabled.Await();
			yield break;
		}

		// Token: 0x06003640 RID: 13888 RVA: 0x00106C56 File Offset: 0x00105056
		private void UserRequestsSent(IEnumerable<string> users)
		{
			this.RefreshData(true);
		}

		// Token: 0x06003641 RID: 13889 RVA: 0x00106C60 File Offset: 0x00105060
		public new void Enable()
		{
			if (!this.dialog.IsVisible)
			{
				base.gameObject.SetActive(true);
			}
			this.checkMark.gameObject.SetActive(true);
			this.facebookService.Friends.UserRequestsSent.AddListener(new Action<IEnumerable<string>>(this.UserRequestsSent));
		}

		// Token: 0x06003642 RID: 13890 RVA: 0x00106CBB File Offset: 0x001050BB
		private void CloseWithoutTracking()
		{
			this.Close(new Action(this.CloseWithoutTracking));
		}

		// Token: 0x06003643 RID: 13891 RVA: 0x00106CCF File Offset: 0x001050CF
		private void CloseWithTracking()
		{
			this.Close(new Action(this.CloseWithTracking));
			this.Track("close", string.Empty);
		}

		// Token: 0x06003644 RID: 13892 RVA: 0x00106CF4 File Offset: 0x001050F4
		private void Close(Action closeHandler)
		{
			this.facebookService.Friends.UserRequestsSent.RemoveListener(new Action<IEnumerable<string>>(this.UserRequestsSent));
			BackButtonManager.Instance.RemoveAction(closeHandler);
			this.audioService.PlaySFX(AudioId.PopupHideDefault, false, false, false);
			this.dialog.Hide();
		}

		// Token: 0x06003645 RID: 13893 RVA: 0x00106D4C File Offset: 0x0010514C
		private void Track(string action, string action_det = "")
		{
			if (this.currentType == MultiFriendsSelectorRoot.FriendSelectorType.inviteFriendsAll)
			{
				this.tracking.TrackUi("mfs", "island", action, action_det, new object[0]);
			}
		}

		// Token: 0x04005E36 RID: 24118
		[WaitForRoot(false, false)]
		private EventSystemRoot events;

		// Token: 0x04005E37 RID: 24119
		[WaitForService(true, true)]
		private FacebookService facebookService;

		// Token: 0x04005E38 RID: 24120
		[WaitForService(true, true)]
		private ILocalizationService localisation;

		// Token: 0x04005E39 RID: 24121
		[WaitForService(true, true)]
		private AudioService audioService;

		// Token: 0x04005E3A RID: 24122
		[WaitForService(true, true)]
		private TrackingService tracking;

		// Token: 0x04005E3B RID: 24123
		[SerializeField]
		private MultiFriendsSelectorDataSource friendsData;

		// Token: 0x04005E3C RID: 24124
		[SerializeField]
		private AnimatedUi dialog;

		// Token: 0x04005E3D RID: 24125
		[SerializeField]
		private TMP_Text TitleText;

		// Token: 0x04005E3E RID: 24126
		[SerializeField]
		private GameObject checkMark;

		// Token: 0x04005E3F RID: 24127
		[SerializeField]
		private MultiFriendsSelectorRoot.FriendSelectorType currentType;

		// Token: 0x04005E40 RID: 24128
		private static MultiFriendsSelectorFriendData[] noFriends = new MultiFriendsSelectorFriendData[0];

		// Token: 0x04005E41 RID: 24129
		private MultiFriendsSelectorFriendData[] friendData = MultiFriendsSelectorRoot.noFriends;

		// Token: 0x020008AE RID: 2222
		public enum FriendSelectorType
		{
			// Token: 0x04005E46 RID: 24134
			askForLives,
			// Token: 0x04005E47 RID: 24135
			sendLives,
			// Token: 0x04005E48 RID: 24136
			inviteFriends,
			// Token: 0x04005E49 RID: 24137
			invteFriendsInbox,
			// Token: 0x04005E4A RID: 24138
			inviteFriendsSettings,
			// Token: 0x04005E4B RID: 24139
			inviteFriendsAll,
			// Token: 0x04005E4C RID: 24140
			friends_deco
		}

		// Token: 0x020008AF RID: 2223
		public class ViewData
		{
			// Token: 0x04005E4D RID: 24141
			public MultiFriendsSelectorRoot.FriendSelectorType type;

			// Token: 0x04005E4E RID: 24142
			public IEnumerable<MultiFriendsSelectorFriendData> FriendsData;

			// Token: 0x04005E4F RID: 24143
			public int SelectedCount;

			// Token: 0x04005E50 RID: 24144
			public int TotalCount;
		}
	}
}
