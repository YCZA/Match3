using System;
using System.Collections;
using Match3.Scripts1.Audio;
using Match3.Scripts1.Puzzletown.Services;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.Signals;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.UI
{
	// Token: 0x02000869 RID: 2153
	[LoadOptions(true, true, false)]
	public class PopupFacebookConnectRoot : APtSceneRoot, IHandler<PopupOperation>, IDisposableDialog
	{
		// Token: 0x06003523 RID: 13603 RVA: 0x000FEEA0 File Offset: 0x000FD2A0
		protected override void Go()
		{
			this.dialog.Show();
			BackButtonManager.Instance.AddAction(new Action(this.Close));
			this.audioService.PlaySFX(AudioId.PopupShowDefault, false, false, false);
			this.TrackFBConnect("open");
			this.diamondsPanel.SetActive(!this.gameStateService.Facebook.receivedLoginReward);
		}

		// Token: 0x06003524 RID: 13604 RVA: 0x000FEF0C File Offset: 0x000FD30C
		public void Close()
		{
			this.dialog.Hide();
			BackButtonManager.Instance.RemoveAction(new Action(this.Close));
			this.audioService.PlaySFX(AudioId.PopupHideDefault, false, false, false);
			this.onClose.Dispatch();
		}

		// Token: 0x06003525 RID: 13605 RVA: 0x000FEF5C File Offset: 0x000FD35C
		public void Handle(PopupOperation evt)
		{
			if (evt != PopupOperation.Close)
			{
				if (evt != PopupOperation.OK)
				{
					if (evt == PopupOperation.Skip)
					{
						this.gameStateService.SetSeenFlag("fb_connect");
						this.TrackFBConnect("no_thanks");
						this.Close();
					}
				}
				else
				{
					this.TrackFBConnect("connect");
					base.StartCoroutine(this.facebookLogin());
				}
			}
			else
			{
				this.TrackFBConnect("not_now");
				this.Close();
			}
		}

		// Token: 0x06003526 RID: 13606 RVA: 0x000FEFE0 File Offset: 0x000FD3E0
		private IEnumerator facebookLogin()
		{
			Wooroutine<FacebookLoginContext> loginFlow = new FacebookLoginFlow(FacebookLoginContext.title_screen).Start<FacebookLoginContext>();
			yield return loginFlow;
			this.Close();
			yield break;
		}

		// Token: 0x06003527 RID: 13607 RVA: 0x000FEFFB File Offset: 0x000FD3FB
		private void TrackFBConnect(string action)
		{
			this.trackingService.TrackUi("fb_connect", "island", action, string.Empty, new object[0]);
		}

		// Token: 0x04005CFB RID: 23803
		[WaitForService(true, true)]
		private GameStateService gameStateService;

		// Token: 0x04005CFC RID: 23804
		[WaitForService(true, true)]
		private TrackingService trackingService;

		// Token: 0x04005CFD RID: 23805
		[WaitForService(true, true)]
		private AudioService audioService;

		// Token: 0x04005CFE RID: 23806
		[WaitForService(true, true)]
		private FacebookService facebookService;

		// Token: 0x04005CFF RID: 23807
		public const string FACEBOOK_CONNECT = "fb_connect";

		// Token: 0x04005D00 RID: 23808
		public readonly AwaitSignal onClose = new AwaitSignal();

		// Token: 0x04005D01 RID: 23809
		public AnimatedUi dialog;

		// Token: 0x04005D02 RID: 23810
		public GameObject diamondsPanel;
	}
}
