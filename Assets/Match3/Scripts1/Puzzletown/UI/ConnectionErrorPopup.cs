using System.Collections;

namespace Match3.Scripts1.Puzzletown.UI
{
	// Token: 0x02000B2F RID: 2863
	public static class ConnectionErrorPopup
	{
		// Token: 0x06004338 RID: 17208 RVA: 0x00157BF0 File Offset: 0x00155FF0
		public static IEnumerator ShowAndWaitForClose()
		{
			
			yield return PopupMissingAssetsRoot.TryShowRoutine("");
			// Wooroutine<PopupFacebookRoot> noConnectionPopup = SceneManager.Instance.LoadSceneWithParams<PopupFacebookRoot, FacebookPopupParameters>(new FacebookPopupParameters(FacebookPopupState.NoConnection, FacebookLoginContext.settings), null);
			// yield return noConnectionPopup;
			// yield return noConnectionPopup.ReturnValue.onDestroyed;
			yield break;
		}
	}
}
