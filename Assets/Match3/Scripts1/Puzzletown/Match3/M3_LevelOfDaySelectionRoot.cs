using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Match3.Scripts1.Audio;
using Match3.Scripts1.Puzzletown.Flows;
using Match3.Scripts1.Puzzletown.Services;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.UI;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;
using UnityEngine;
using UnityEngine.UI;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020006E5 RID: 1765
	[LoadOptions(true, true, false)]
	public class M3_LevelOfDaySelectionRoot : ASceneRoot<bool>, IDisposableDialog, IHandler<PopupOperation>
	{
		// Token: 0x06002BCF RID: 11215 RVA: 0x000C9118 File Offset: 0x000C7518
		protected override void Go()
		{
			this.successfullyWonLevel = this.parameters;
			this.friendsData = new List<TownOptionsFriendData>();
			this.dialog.Show();
			BackButtonManager.Instance.AddAction(new Action(this.Close));
			this.tableViewSnapper = base.GetComponentInChildren<TableViewSnapper>();
			this.SetupViews();
			base.StartCoroutine(this.RefreshFriendsFromFacebookRoutine());
		}

		// Token: 0x06002BD0 RID: 11216 RVA: 0x000C917C File Offset: 0x000C757C
		private void SetupViews()
		{
			LevelOfDayModel levelOfDayModel = null;
			this.gameStateService.LevelOfDayData.TryGetSavedLevelOfDayModel(out levelOfDayModel);
			if (levelOfDayModel != null)
			{
				this.timer.SetTargetTime(Scripts1.DateTimeExtensions.FromUnixTimeStamp(levelOfDayModel.endUTCTime, DateTimeKind.Utc), true, null);
				this.currentDayNumber = levelOfDayModel.currentDay;
			}
			if (base.registeredFirst)
			{
				this.currentDayNumber = this.mockCurrentDay;
			}
			if (this.successfullyWonLevel)
			{
				this.currentDayNumber--;
			}
			this.genericMessagePanel.SetActive(!this.successfullyWonLevel);
			this.closeButton.gameObject.SetActive(!this.successfullyWonLevel);
			this.confirmationPanel.SetActive(this.successfullyWonLevel);
			LevelOfDaySelectionView original = this.inactivePrototype;
			this.currentDayNumber = Math.Max(this.currentDayNumber, 1);
			int num = (this.currentDayNumber - 1) / 15 * 15 + 1;
			int num2 = (this.currentDayNumber - 1) % 15;
			for (int i = 0; i < this.friendViews.Count; i++)
			{
				if (i < num2)
				{
					original = this.inactivePrototype;
				}
				else if (i == num2)
				{
					if (this.successfullyWonLevel)
					{
						original = this.masteredPrototype;
					}
					else
					{
						original = this.focusPrototype;
					}
				}
				else
				{
					original = this.focusInactivePrototype;
				}
				LevelOfDaySelectionView levelOfDaySelectionView = global::UnityEngine.Object.Instantiate<LevelOfDaySelectionView>(original, this.friendViews[i].transform);
				levelOfDaySelectionView.transform.SetAsFirstSibling();
				levelOfDaySelectionView.transform.localPosition = Vector3.zero;
				this.friendViews[i].day = num;
				levelOfDaySelectionView.dayNumberLabel.text = num.ToString();
				levelOfDaySelectionView.materialAmountViews[0].Show(new MaterialAmount(this.sbsService.SbsConfig.level_of_day.rewards[i].reward_1_type, this.sbsService.SbsConfig.level_of_day.rewards[i].reward_1_amount, MaterialAmountUsage.Undefined, 0));
				levelOfDaySelectionView.materialAmountViews[1].Show(new MaterialAmount(this.sbsService.SbsConfig.level_of_day.rewards[i].reward_2_type, this.sbsService.SbsConfig.level_of_day.rewards[i].reward_2_amount, MaterialAmountUsage.Undefined, 0));
				num++;
			}
			float position = (float)num2 / (float)this.friendViews.Count;
			this.tableViewSnapper.ScrollTo(position, 0.5f);
		}

		// Token: 0x06002BD1 RID: 11217 RVA: 0x000C9413 File Offset: 0x000C7813
		private void Close()
		{
			BackButtonManager.Instance.RemoveAction(new Action(this.Close));
			this.dialog.Hide();
			this.audioService.PlaySFX(AudioId.PopupHideDefault, false, false, false);
		}

		// Token: 0x06002BD2 RID: 11218 RVA: 0x000C944C File Offset: 0x000C784C
		private IEnumerator RefreshFriendsFromFacebookRoutine()
		{
			if (!this.facebookService.LoggedIn())
			{
				yield break;
			}
			Wooroutine<FacebookAPIRunner.FriendListData> friends = this.facebookService.GetFriends(FacebookData.Friend.Type.Playing);
			yield return friends;
			this.friendsData = this.ConvertFriends(friends.ReturnValue.friends).ToList<TownOptionsFriendData>();
			foreach (LevelOfDayFriendView levelOfDayFriendView in this.friendViews)
			{
				levelOfDayFriendView.UpdateFacebookFriends(this.friendsData);
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

		// Token: 0x06002BD3 RID: 11219 RVA: 0x000C9468 File Offset: 0x000C7868
		private TownOptionsFriendData ConvertFriend(FacebookData.Friend friend, bool isMe)
		{
			int num = (!this.successfullyWonLevel) ? this.currentDayNumber : (this.currentDayNumber + 1);
			return new TownOptionsFriendData
			{
				friend = friend,
				avatar = this.facebookService.GetProfilePicture(friend),
				level = ((!isMe) ? this.facebookService.FriendProgress(friend.ID).LodStreak : num),
				type = ((!isMe) ? TownOptionsFriendData.Type.Friend : TownOptionsFriendData.Type.Me)
			};
		}

		// Token: 0x06002BD4 RID: 11220 RVA: 0x000C94F4 File Offset: 0x000C78F4
		private IEnumerable<TownOptionsFriendData> ConvertFriends(IEnumerable<FacebookData.Friend> friends)
		{
			yield return this.ConvertFriend(this.facebookService.Me, true);
			foreach (FacebookData.Friend friend in friends)
			{
				yield return this.ConvertFriend(friend, false);
			}
			yield break;
		}

		// Token: 0x06002BD5 RID: 11221 RVA: 0x000C951E File Offset: 0x000C791E
		public void Handle(PopupOperation evt)
		{
			if (evt != PopupOperation.Close)
			{
				if (evt == PopupOperation.OK)
				{
					new CoreGameFlow().Start(new CoreGameFlow.Input(0, false, null, LevelPlayMode.LevelOfTheDay));
				}
			}
			else
			{
				this.Close();
			}
		}

		// Token: 0x040054EC RID: 21740
		private const float SCROLL_TIME = 0.5f;

		// Token: 0x040054ED RID: 21741
		[WaitForService(true, true)]
		private AudioService audioService;

		// Token: 0x040054EE RID: 21742
		[WaitForService(true, true)]
		private GameStateService gameStateService;

		// Token: 0x040054EF RID: 21743
		[WaitForService(true, true)]
		private FacebookService facebookService;

		// Token: 0x040054F0 RID: 21744
		[WaitForService(true, true)]
		private SBSService sbsService;

		// Token: 0x040054F1 RID: 21745
		[SerializeField]
		private AnimatedUi dialog;

		// Token: 0x040054F2 RID: 21746
		[SerializeField]
		private Button closeButton;

		// Token: 0x040054F3 RID: 21747
		[SerializeField]
		private LevelOfDaySelectionView inactivePrototype;

		// Token: 0x040054F4 RID: 21748
		[SerializeField]
		private LevelOfDaySelectionView focusInactivePrototype;

		// Token: 0x040054F5 RID: 21749
		[SerializeField]
		private LevelOfDaySelectionView focusPrototype;

		// Token: 0x040054F6 RID: 21750
		[SerializeField]
		private LevelOfDaySelectionView masteredPrototype;

		// Token: 0x040054F7 RID: 21751
		[SerializeField]
		private GameObject genericMessagePanel;

		// Token: 0x040054F8 RID: 21752
		[SerializeField]
		private GameObject confirmationPanel;

		// Token: 0x040054F9 RID: 21753
		[SerializeField]
		private List<LevelOfDayFriendView> friendViews;

		// Token: 0x040054FA RID: 21754
		[SerializeField]
		private CountdownTimer timer;

		// Token: 0x040054FB RID: 21755
		[Header("Testing")]
		[SerializeField]
		private int mockCurrentDay;

		// Token: 0x040054FC RID: 21756
		private bool successfullyWonLevel;

		// Token: 0x040054FD RID: 21757
		private int currentDayNumber;

		// Token: 0x040054FE RID: 21758
		private TableViewSnapper tableViewSnapper;

		// Token: 0x040054FF RID: 21759
		private List<TownOptionsFriendData> friendsData;
	}
}
