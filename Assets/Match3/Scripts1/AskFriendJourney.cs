using System.Collections;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Puzzletown.UI;
using Match3.Scripts1.Shared.Flows;
using Wooga.Coroutines;
using Wooga.UnityFramework;
using UnityEngine;

// Token: 0x020008A1 RID: 2209
namespace Match3.Scripts1
{
	public class AskFriendJourney : AFlow
	{
		// Token: 0x06003606 RID: 13830 RVA: 0x00105114 File Offset: 0x00103514
		protected override IEnumerator FlowRoutine()
		{
			yield return ServiceLocator.Instance.Inject(this);
			if (this.facebookService.LoggedIn())
			{
				if (Application.internetReachability != NetworkReachability.NotReachable)
				{
					Wooroutine<MultiFriendsSelectorRoot> gameScene = SceneManager.Instance.LoadSceneWithParams<MultiFriendsSelectorRoot, string>("out_of_lives", null);
					yield return gameScene;
					gameScene.ReturnValue.gameObject.SetActive(true);
					yield return gameScene.ReturnValue.TryToSend(MultiFriendsSelectorRoot.FriendSelectorType.askForLives);
				}
				else
				{
					SceneManager.Instance.LoadSceneWithParams<PopupFacebookRoot, FacebookPopupParameters>(new FacebookPopupParameters(FacebookPopupState.NoConnection, FacebookLoginContext.settings), null);
				}
			}
			else
			{
				new FacebookLoginFlow(FacebookLoginContext.get_more_lives).Start();
				yield return null;
			}
			yield break;
		}

		// Token: 0x04005E03 RID: 24067
		[WaitForService(true, true)]
		private LivesService livesService;

		// Token: 0x04005E04 RID: 24068
		[WaitForService(true, true)]
		private FacebookService facebookService;
	}
}
