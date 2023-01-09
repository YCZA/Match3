using System.Collections;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Shared.Flows;
using Wooga.Coroutines;
using Wooga.UnityFramework;

// Token: 0x020009DB RID: 2523
namespace Match3.Scripts1
{
	public class InviteFriendsFlow : AFlow
	{
		// Token: 0x06003D18 RID: 15640 RVA: 0x0013451C File Offset: 0x0013291C
		public InviteFriendsFlow(MultiFriendsSelectorRoot.FriendSelectorType type)
		{
			this.type = type;
		}

		// Token: 0x06003D19 RID: 15641 RVA: 0x0013452C File Offset: 0x0013292C
		protected override IEnumerator FlowRoutine()
		{
			yield return ServiceLocator.Instance.Inject(this);
			string context = this.FindOutContextBasedOnType();
			if (this.facebookService.LoggedIn())
			{
				Wooroutine<MultiFriendsSelectorRoot> gameScene = SceneManager.Instance.LoadSceneWithParams<MultiFriendsSelectorRoot, string>(context, null);
				yield return gameScene;
				gameScene.ReturnValue.Enable();
				yield return gameScene.ReturnValue.TryToSend(this.type);
			}
			else
			{
				new FacebookLoginFlow(FacebookLoginContext.friend_list).Start();
				yield return null;
			}
			yield break;
		}

		// Token: 0x06003D1A RID: 15642 RVA: 0x00134548 File Offset: 0x00132948
		protected string FindOutContextBasedOnType()
		{
			string result;
			switch (this.type)
			{
				case MultiFriendsSelectorRoot.FriendSelectorType.inviteFriends:
					result = "friendlist";
					break;
				case MultiFriendsSelectorRoot.FriendSelectorType.invteFriendsInbox:
					result = "inbox";
					this.type = MultiFriendsSelectorRoot.FriendSelectorType.inviteFriends;
					break;
				case MultiFriendsSelectorRoot.FriendSelectorType.inviteFriendsSettings:
					result = "settings";
					this.type = MultiFriendsSelectorRoot.FriendSelectorType.inviteFriends;
					break;
				case MultiFriendsSelectorRoot.FriendSelectorType.inviteFriendsAll:
					result = "island";
					break;
				default:
					result = "settings";
					break;
			}
			return result;
		}

		// Token: 0x040065C4 RID: 26052
		[WaitForService(true, true)]
		private LivesService livesService;

		// Token: 0x040065C5 RID: 26053
		[WaitForService(true, true)]
		private FacebookService facebookService;

		// Token: 0x040065C6 RID: 26054
		[WaitForService(true, true)]
		private TrackingService tracking;

		// Token: 0x040065C7 RID: 26055
		private MultiFriendsSelectorRoot.FriendSelectorType type;
	}
}
