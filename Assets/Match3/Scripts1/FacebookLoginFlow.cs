using System;
using System.Collections;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Puzzletown.Town;
using Match3.Scripts1.Puzzletown.UI;
using Wooga.Coroutines;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000791 RID: 1937
namespace Match3.Scripts1
{
	public class FacebookLoginFlow : BaseFacebookFlow
	{
		// Token: 0x06002F88 RID: 12168 RVA: 0x000DE97B File Offset: 0x000DCD7B
		public FacebookLoginFlow(FacebookLoginContext context)
		{
			this.loginContext = context;
		}

		// Token: 0x06002F89 RID: 12169 RVA: 0x000DE98C File Offset: 0x000DCD8C
		protected override IEnumerator FBFlowRoutine()
		{
			if (FacebookLoginFlow.flowRunning)
			{
				yield break;
			}
			FacebookLoginFlow.flowRunning = true;
			yield return ServiceLocator.Instance.Inject(this);
			yield return SceneManager.Instance.Inject(this);
			this.sessionService.onRestart.AddListener(new Action(this.HandleReload));
			this.eventSystemRoot.Disable();
			if (!this.sbsService.IsAuthenticated || Application.internetReachability == NetworkReachability.NotReachable)
			{
				yield return this.ShowFacebookPopup(FacebookPopupState.NoConnection, null);
			}
			else
			{
				this.gameState.savingBlocked = true;
				Wooroutine<FacebookService.FBLoginResult> loginRoutine = WooroutineRunner.StartWooroutine<FacebookService.FBLoginResult>(this.facebook.DoLogin());
				yield return loginRoutine;
				FacebookService.FBLoginResult loginResult = loginRoutine.ReturnValue;
				FacebookLoginFlow.DooberInfo dooberInfo = null;
				if (this.facebook.LoggedIn())
				{
					if (!this.gameState.Facebook.receivedLoginReward && !loginResult.sbsUserChanged)
					{
						MaterialAmount amount = new MaterialAmount("diamonds", this.configService.general.balance.facebook_login_reward, MaterialAmountUsage.Undefined, 0);
						dooberInfo = new FacebookLoginFlow.DooberInfo(amount);
						this.gameState.Resources.AddMaterial(amount.type, amount.amount, true);
						this.gameState.Facebook.receivedLoginReward = true;
						this.adjustService.TrackFbLogin();
						this.trackingService.TrackRewards("fb_connect", this.loginContext.ToString(), string.Empty, string.Empty, 0, this.configService.general.balance.facebook_login_reward);
						this.pushNotificationService.SendFBFriendsInitialMessage();
					}
					yield return this.ShowFacebookPopup(FacebookPopupState.Connected, dooberInfo);
				}
				else
				{
					yield return this.ShowFacebookPopup(FacebookPopupState.FailedToConnect, null);
				}
				if (loginResult.sbsUserChanged)
				{
					yield return this.gameState.ReloadGameDataWithSelect();
				}
				this.gameState.savingBlocked = false;
				this.gameState.Save(false);
			}
			FacebookLoginFlow.flowRunning = false;
			if (this.sessionService != null)
			{
				this.sessionService.onRestart.RemoveListener(new Action(this.HandleReload));
			}
			yield break;
		}

		// Token: 0x06002F8A RID: 12170 RVA: 0x000DE9A7 File Offset: 0x000DCDA7
		private void HandleReload()
		{
			FacebookLoginFlow.flowRunning = false;
		}

		// Token: 0x06002F8B RID: 12171 RVA: 0x000DE9B0 File Offset: 0x000DCDB0
		private IEnumerator ShowFacebookPopup(FacebookPopupState state, FacebookLoginFlow.DooberInfo info = null)
		{
			Wooroutine<PopupFacebookRoot> facebookPopup = SceneManager.Instance.LoadSceneWithParams<PopupFacebookRoot, FacebookPopupParameters>(new FacebookPopupParameters(state, this.loginContext), null);
			yield return facebookPopup;
			this.eventSystemRoot.Enable();
			if (info != null)
			{
				yield return this.ShowDoobers(facebookPopup.ReturnValue, info);
			}
			yield return facebookPopup.ReturnValue.onClose;
			if (this.shownResourcePanelState != null)
			{
				this.resourcePanel.RemoveState(this.shownResourcePanelState);
				this.shownResourcePanelState = null;
			}
			yield break;
		}

		// Token: 0x06002F8C RID: 12172 RVA: 0x000DE9DC File Offset: 0x000DCDDC
		private IEnumerator ShowDoobers(PopupFacebookRoot fbPopup, FacebookLoginFlow.DooberInfo info)
		{
			this.shownResourcePanelState = this.ShowResourcePanelForLoginRewards(fbPopup, info);
			yield return new WaitForEndOfFrame();
			Image img = this.resourcePanel.FindIconByMaterial(info.amount.type);
			Transform source = fbPopup.continueButton.transform;
			if (img != null)
			{
				yield return new WaitForSeconds(this.doobers.SpawnDoobers(info.amount, source, img.transform, null));
			}
			yield break;
		}

		// Token: 0x06002F8D RID: 12173 RVA: 0x000DEA08 File Offset: 0x000DCE08
		private TownResourcePanelRoot.State ShowResourcePanelForLoginRewards(PopupFacebookRoot fbPopup, FacebookLoginFlow.DooberInfo info)
		{
			TownResourceElement elements = (TownResourceElement)Enum.Parse(typeof(TownResourceElement), info.amount.type, true);
			return this.resourcePanel.Show(fbPopup.canvas, elements, false);
		}

		// Token: 0x040058AB RID: 22699
		private static bool flowRunning;

		// Token: 0x040058AC RID: 22700
		[WaitForRoot(false, false)]
		private TownResourcePanelRoot resourcePanel;

		// Token: 0x040058AD RID: 22701
		[WaitForRoot(false, false)]
		private DoobersRoot doobers;

		// Token: 0x040058AE RID: 22702
		[WaitForService(true, true)]
		private GameStateService gameState;

		// Token: 0x040058AF RID: 22703
		[WaitForService(true, true)]
		private ILocalizationService locaService;

		// Token: 0x040058B0 RID: 22704
		[WaitForService(true, true)]
		private ConfigService configService;

		// Token: 0x040058B1 RID: 22705
		[WaitForService(true, true)]
		private TrackingService trackingService;

		// Token: 0x040058B2 RID: 22706
		[WaitForService(true, true)]
		private IAdjustService adjustService;

		// Token: 0x040058B3 RID: 22707
		[WaitForService(true, true)]
		private SBSService sbsService;

		// Token: 0x040058B4 RID: 22708
		[WaitForService(true, true)]
		private PushNotificationService pushNotificationService;

		// Token: 0x040058B5 RID: 22709
		[WaitForService(true, true)]
		private SessionService sessionService;

		// Token: 0x040058B6 RID: 22710
		[WaitForRoot(false, false)]
		private EventSystemRoot eventSystemRoot;

		// Token: 0x040058B7 RID: 22711
		private FacebookLoginContext loginContext;

		// Token: 0x040058B8 RID: 22712
		private TownResourcePanelRoot.State shownResourcePanelState;

		// Token: 0x02000792 RID: 1938
		private class DooberInfo
		{
			// Token: 0x06002F8E RID: 12174 RVA: 0x000DEA49 File Offset: 0x000DCE49
			public DooberInfo(MaterialAmount amount)
			{
				this.amount = amount;
			}

			// Token: 0x040058B9 RID: 22713
			public MaterialAmount amount;
		}
	}
}
