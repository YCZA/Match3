using System;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Wooga.Legal;
using Wooga.UnityFramework;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Match3.Scripts1.Puzzletown.UI
{
	// Token: 0x02000831 RID: 2097
	[LoadOptions(true, true, false)]
	public class PopupGDPRRoot : APtSceneRoot, IDisposableDialog
	{
		// Token: 0x0600342D RID: 13357 RVA: 0x000F8A40 File Offset: 0x000F6E40
		protected override void Go()
		{
			this.eventSystem.Enable();
			this.acceptButton.onClick.AddListener(new UnityAction(this.Close));
			TextMeshProLinkHandler.LinkHandler = (Action<string>)Delegate.Combine(TextMeshProLinkHandler.LinkHandler, new Action<string>(this.OnLinkClicked));
			this.dialog.Show();
			this.trackingService.TrackGdprPopup("0");
		}

		// Token: 0x0600342E RID: 13358 RVA: 0x000F8AB0 File Offset: 0x000F6EB0
		private void Close()
		{
			TextMeshProLinkHandler.LinkHandler = (Action<string>)Delegate.Remove(TextMeshProLinkHandler.LinkHandler, new Action<string>(this.OnLinkClicked));
			Terms.OnAccepted(this.sbsService.IsAuthenticated);
			this.trackingService.TrackGdprPopup("1");
			this.dialog.Hide();
		}

		// Token: 0x0600342F RID: 13359 RVA: 0x000F8B08 File Offset: 0x000F6F08
		private void OnLinkClicked(string link)
		{
			this.helpshiftService.ShowSingleFAQ(link);
		}

		// Token: 0x04005C04 RID: 23556
		public const string TERMS_FAQ_ID = "2512";

		// Token: 0x04005C05 RID: 23557
		public const string POLICY_FAQ_ID = "2425";

		// Token: 0x04005C06 RID: 23558
		private const string GDPR_SHOWN = "0";

		// Token: 0x04005C07 RID: 23559
		private const string GDPR_ACCEPTED = "1";

		// Token: 0x04005C08 RID: 23560
		[WaitForService(true, true)]
		private TrackingService trackingService;

		// Token: 0x04005C09 RID: 23561
		[WaitForService(true, true)]
		private HelpshiftService helpshiftService;

		// Token: 0x04005C0A RID: 23562
		[WaitForService(true, true)]
		private SBSService sbsService;

		// Token: 0x04005C0B RID: 23563
		[WaitForRoot(false, false)]
		private EventSystemRoot eventSystem;

		// Token: 0x04005C0C RID: 23564
		[SerializeField]
		private AnimatedUi dialog;

		// Token: 0x04005C0D RID: 23565
		[SerializeField]
		private Button acceptButton;
	}
}
