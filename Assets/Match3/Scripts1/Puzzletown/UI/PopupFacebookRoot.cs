using System;
using System.Collections;
using Match3.Scripts1.Audio;
using Match3.Scripts1.Puzzletown.Services;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.Signals;
using Wooga.UnityFramework;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Match3.Scripts1.Puzzletown.UI
{
	// Token: 0x0200086A RID: 2154
	[LoadOptions(true, false, false)]
	public class PopupFacebookRoot : APtSceneRoot<FacebookPopupParameters>, IDisposableDialog
	{
		// Token: 0x06003529 RID: 13609 RVA: 0x000FF0D6 File Offset: 0x000FD4D6
		private void OnValidate()
		{
		}

		// Token: 0x0600352A RID: 13610 RVA: 0x000FF0D8 File Offset: 0x000FD4D8
		protected override void Go()
		{
			this.dialog.Show();
			BackButtonManager.Instance.AddAction(new Action(this.OnClose));
			this.audioService.PlaySFX(AudioId.PopupShowDefault, false, false, false);
			this.ShowFacebookDialog();
		}

		// Token: 0x0600352B RID: 13611 RVA: 0x000FF116 File Offset: 0x000FD516
		public Coroutine ShowFacebookDialog()
		{
			return WooroutineRunner.StartCoroutine(this.ShowFacebookDialogRoutine(this.parameters.popupState), null);
		}

		// Token: 0x0600352C RID: 13612 RVA: 0x000FF130 File Offset: 0x000FD530
		private void AddButtonListeners()
		{
			this.closeButton.onClick.AddListener(new UnityAction(this.OnClose));
			this.continueButton.onClick.AddListener(new UnityAction(this.OnClose));
			this.tryAgainButton.onClick.AddListener(new UnityAction(this.OnRetry));
			this.yesButton.onClick.AddListener(new UnityAction(this.OnYes));
			this.noButton.onClick.AddListener(new UnityAction(this.OnClose));
		}

		// Token: 0x0600352D RID: 13613 RVA: 0x000FF1CC File Offset: 0x000FD5CC
		private void RemoveButtonListeners()
		{
			this.closeButton.onClick.RemoveListener(new UnityAction(this.OnClose));
			this.continueButton.onClick.RemoveListener(new UnityAction(this.OnClose));
			this.tryAgainButton.onClick.RemoveListener(new UnityAction(this.OnRetry));
			this.yesButton.onClick.RemoveListener(new UnityAction(this.OnYes));
			this.noButton.onClick.RemoveListener(new UnityAction(this.OnClose));
		}

		// Token: 0x0600352E RID: 13614 RVA: 0x000FF268 File Offset: 0x000FD668
		private IEnumerator ShowFacebookDialogRoutine(FacebookPopupState state)
		{
			string title = string.Empty;
			string body = string.Empty;
			this.connectedView.SetActive(false);
			this.disconnectedView.SetActive(false);
			this.failedToConnectView.SetActive(false);
			this.logOutView.SetActive(false);
			this.noConnectionView.SetActive(false);
			this.continueButton.gameObject.SetActive(false);
			this.tryAgainButton.gameObject.SetActive(false);
			this.closeButton.gameObject.SetActive(false);
			this.yesNoPanel.SetActive(false);
			switch (state)
			{
			case FacebookPopupState.Connected:
				title = this.localizationService.GetText("ui.social.connected.title", new LocaParam[0]);
				body = this.localizationService.GetText("ui.social.connected.body", new LocaParam[0]);
				this.continueButton.gameObject.SetActive(true);
				this.connectedView.SetActive(true);
				break;
			case FacebookPopupState.Disconnected:
				title = this.localizationService.GetText("ui.social.disconnected.title", new LocaParam[0]);
				body = this.localizationService.GetText("ui.social.disconnected.body", new LocaParam[0]);
				this.disconnectedView.SetActive(true);
				this.continueButton.gameObject.SetActive(true);
				break;
			case FacebookPopupState.FailedToConnect:
				title = this.localizationService.GetText("ui.social.failed.title", new LocaParam[0]);
				body = this.localizationService.GetText("ui.social.failed.body", new LocaParam[0]);
				this.tryAgainButton.gameObject.SetActive(true);
				this.closeButton.gameObject.SetActive(true);
				this.failedToConnectView.SetActive(true);
				break;
			case FacebookPopupState.LogOut:
				title = this.localizationService.GetText("ui.social.logout.title", new LocaParam[0]);
				body = this.localizationService.GetText("ui.social.logout.body", new LocaParam[0]);
				this.logOutView.SetActive(true);
				this.yesNoPanel.SetActive(true);
				break;
			case FacebookPopupState.NoConnection:
				title = this.localizationService.GetText("ui.social.no_connection.title", new LocaParam[0]);
				body = this.localizationService.GetText("ui.social.no_connection.body", new LocaParam[0]);
				this.noConnectionView.SetActive(true);
				this.continueButton.gameObject.SetActive(true);
				break;
			}
			this.titleLabel.text = title;
			this.bodyLabel.text = body;
			this.AddButtonListeners();
			yield return this.onClose;
			yield break;
		}

		// Token: 0x0600352F RID: 13615 RVA: 0x000FF28C File Offset: 0x000FD68C
		private void OnClose()
		{
			this.dialog.Hide();
			BackButtonManager.Instance.RemoveAction(new Action(this.OnClose));
			this.audioService.PlaySFX(AudioId.PopupHideDefault, false, false, false);
			this.RemoveButtonListeners();
			this.onClose.Dispatch(this.returnValue);
		}

		// Token: 0x06003530 RID: 13616 RVA: 0x000FF2E5 File Offset: 0x000FD6E5
		private void OnRetry()
		{
			this.OnClose();
			new FacebookLoginFlow(this.parameters.loginContext).Start();
		}

		// Token: 0x06003531 RID: 13617 RVA: 0x000FF303 File Offset: 0x000FD703
		private void OnYes()
		{
			this.returnValue = true;
			this.OnClose();
		}

		// Token: 0x04005D03 RID: 23811
		[WaitForRoot(false, false)]
		private EventSystemRoot eventSystem;

		// Token: 0x04005D04 RID: 23812
		[WaitForService(true, true)]
		private AudioService audioService;

		// Token: 0x04005D05 RID: 23813
		[WaitForService(true, true)]
		private ILocalizationService localizationService;

		// Token: 0x04005D06 RID: 23814
		public readonly AwaitSignal<bool> onClose = new AwaitSignal<bool>();

		// Token: 0x04005D07 RID: 23815
		public AnimatedUi dialog;

		// Token: 0x04005D08 RID: 23816
		public GameObject connectedView;

		// Token: 0x04005D09 RID: 23817
		public GameObject disconnectedView;

		// Token: 0x04005D0A RID: 23818
		public GameObject failedToConnectView;

		// Token: 0x04005D0B RID: 23819
		public GameObject logOutView;

		// Token: 0x04005D0C RID: 23820
		public GameObject noConnectionView;

		// Token: 0x04005D0D RID: 23821
		public TextMeshProUGUI titleLabel;

		// Token: 0x04005D0E RID: 23822
		public TextMeshProUGUI bodyLabel;

		// Token: 0x04005D0F RID: 23823
		public Button closeButton;

		// Token: 0x04005D10 RID: 23824
		public Button continueButton;

		// Token: 0x04005D11 RID: 23825
		public Button tryAgainButton;

		// Token: 0x04005D12 RID: 23826
		public GameObject yesNoPanel;

		// Token: 0x04005D13 RID: 23827
		public Button yesButton;

		// Token: 0x04005D14 RID: 23828
		public Button noButton;

		// Token: 0x04005D15 RID: 23829
		public Canvas canvas;

		// Token: 0x04005D16 RID: 23830
		private bool returnValue;
	}
}
