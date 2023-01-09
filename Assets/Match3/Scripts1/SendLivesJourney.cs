using System.Collections;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Puzzletown.UI;
using Match3.Scripts1.Shared.Flows;
using Wooga.Coroutines;
using Wooga.UnityFramework;
using UnityEngine;

// Token: 0x020009DC RID: 2524
namespace Match3.Scripts1
{
	public class SendLivesJourney : AFlow
	{
		// Token: 0x06003D1C RID: 15644 RVA: 0x0013473C File Offset: 0x00132B3C
		protected override IEnumerator FlowRoutine()
		{
			yield return ServiceLocator.Instance.Inject(this);
			if (this.facebookService.LoggedIn())
			{
				if (Application.internetReachability != NetworkReachability.NotReachable)
				{
					Wooroutine<MultiFriendsSelectorRoot> gameScene = SceneManager.Instance.LoadSceneWithParams<MultiFriendsSelectorRoot, string>("settings", null);
					yield return gameScene;
					gameScene.ReturnValue.Enable();
					yield return gameScene.ReturnValue.TryToSend(MultiFriendsSelectorRoot.FriendSelectorType.sendLives);
				}
				else
				{
					SceneManager.Instance.LoadSceneWithParams<PopupFacebookRoot, FacebookPopupParameters>(new FacebookPopupParameters(FacebookPopupState.NoConnection, FacebookLoginContext.settings), null);
				}
			}
			else
			{
				new FacebookLoginFlow(FacebookLoginContext.friend_list).Start();
				yield return null;
			}
			yield break;
		}

		// Token: 0x040065C8 RID: 26056
		[WaitForService(true, true)]
		private LivesService livesService;

		// Token: 0x040065C9 RID: 26057
		[WaitForService(true, true)]
		private FacebookService facebookService;
	}
}
