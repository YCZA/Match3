using System;
using System.Collections;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts2.PlayerData;

// Token: 0x020008AC RID: 2220
namespace Match3.Scripts1
{
	public class MFSTrigger : PopupManager.Trigger
	{
		// Token: 0x06003632 RID: 13874 RVA: 0x0010664A File Offset: 0x00104A4A
		public MFSTrigger(ConfigService configs, GameStateService gameState, FacebookService facebook)
		{
			this.configs = configs;
			this.gameState = gameState;
			this.facebook = facebook;
		}

		// Token: 0x06003633 RID: 13875 RVA: 0x00106668 File Offset: 0x00104A68
		public override bool ShouldTrigger()
		{
			if (!this.configs.FeatureSwitchesConfig.invite_mfs_on_app_start)
			{
				return false;
			}
			int num = DateTime.UtcNow.ToUnixTimeStamp();
			int coolDown = this.configs.general.invite_friends.coolDown;
			bool flag = num - this.gameState.Facebook.LastSeenInviteFriends >= coolDown;
			bool flag2 = this.gameState.Progression.UnlockedLevel >= this.configs.general.invite_friends.minLevel;
			bool flag3 = this.facebook.HasCachedFriends(FacebookData.Friend.Type.Playing);
			bool flag4 = flag && this.facebook.LoggedIn() && this.facebook.FriendsPlayingCount > 0 && flag2 && (!this.gameState.Transactions.IsBuyer || this.configs.general.invite_friends.showForPayingUser);
			if (flag4 && !flag3)
			{
				this.facebook.GetFriends(FacebookData.Friend.Type.Playing);
			}
			return flag3 && flag4;
		}

		// Token: 0x06003634 RID: 13876 RVA: 0x00106788 File Offset: 0x00104B88
		public override IEnumerator Run()
		{
			while (BlockerManager.global.HasBlockers)
			{
				yield return null;
			}
			this.gameState.Facebook.LastSeenInviteFriends = DateTime.UtcNow.ToUnixTimeStamp();
			yield return new InviteFriendsFlow(MultiFriendsSelectorRoot.FriendSelectorType.inviteFriendsAll).Start();
			yield break;
		}

		// Token: 0x04005E33 RID: 24115
		private ConfigService configs;

		// Token: 0x04005E34 RID: 24116
		private GameStateService gameState;

		// Token: 0x04005E35 RID: 24117
		private FacebookService facebook;
	}
}
