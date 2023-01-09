using System.Collections;
using Match3.Scripts1.Puzzletown.UI;
using Wooga.Coroutines;
using Wooga.UnityFramework;

// Token: 0x02000794 RID: 1940
namespace Match3.Scripts1
{
	public class FacebookLogoutFlow : BaseFacebookFlow
	{
		// Token: 0x06002F90 RID: 12176 RVA: 0x000DF0EC File Offset: 0x000DD4EC
		protected override IEnumerator FBFlowRoutine()
		{
			if (!this.facebook.LoggedIn())
			{
				yield break;
			}
			Wooroutine<PopupFacebookRoot> faceBookChoiceDialog = SceneManager.Instance.LoadSceneWithParams<PopupFacebookRoot, FacebookPopupParameters>(new FacebookPopupParameters(FacebookPopupState.LogOut, FacebookLoginContext.settings), null);
			yield return faceBookChoiceDialog;
			yield return faceBookChoiceDialog.ReturnValue.onClose;
			if (faceBookChoiceDialog.ReturnValue.onClose.Dispatched)
			{
				faceBookChoiceDialog.ReturnValue.Destroy();
				this.facebook.Logout();
				Wooroutine<PopupFacebookRoot> facebookPopup = SceneManager.Instance.LoadSceneWithParams<PopupFacebookRoot, FacebookPopupParameters>(new FacebookPopupParameters(FacebookPopupState.Disconnected, FacebookLoginContext.settings), null);
				yield return facebookPopup;
			}
			yield break;
		}
	}
}
