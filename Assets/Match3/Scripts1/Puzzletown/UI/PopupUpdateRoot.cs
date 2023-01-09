using System;
using Match3.Scripts1.Audio;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Wooga.Signals;
using Wooga.UnityFramework;
using TMPro;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.UI
{
	// Token: 0x0200087F RID: 2175
	[LoadOptions(true, true, false)]
	public class PopupUpdateRoot : APtSceneRoot<bool>, IHandler<PopupOperation>, IPersistentDialog
	{
		// Token: 0x0600356F RID: 13679 RVA: 0x001004B8 File Offset: 0x000FE8B8
		protected override void Go()
		{
			this.dialog.Show();
			BackButtonManager.Instance.AddAction(new Action(this.Close));
			this.audioService.PlaySFX(AudioId.PopupShowDefault, false, false, false);
			this.yesButton.SetActive(!this.parameters);
			this.noButton.SetActive(!this.parameters);
			this.alrightButton.SetActive(this.parameters);
			this.bodyLabel.text = this.localizationService.GetText((!this.parameters) ? "ui.forceupdate.reminder.body" : "ui.forceupdate.required.body", new LocaParam[0]);
			this.titleLabel.text = this.localizationService.GetText((!this.parameters) ? "ui.forceupdate.reminder.title" : "ui.forceupdate.required.title", new LocaParam[0]);
		}

		// Token: 0x06003570 RID: 13680 RVA: 0x001005A0 File Offset: 0x000FE9A0
		public void Close()
		{
			if (this.parameters)
			{
				return;
			}
			this.dialog.Hide();
			BackButtonManager.Instance.RemoveAction(new Action(this.Close));
			this.audioService.PlaySFX(AudioId.PopupHideDefault, false, false, false);
			this.onClose.Dispatch();
		}

		// Token: 0x06003571 RID: 13681 RVA: 0x001005F9 File Offset: 0x000FE9F9
		public void Handle(PopupOperation evt)
		{
			if (evt != PopupOperation.OK)
			{
				if (evt == PopupOperation.Close)
				{
					this.Close();
				}
			}
			else
			{
				ForceUpdate.GoToShop();
				if (!this.parameters)
				{
					this.Close();
				}
			}
		}

		// Token: 0x04005D4C RID: 23884
		[WaitForService(true, true)]
		private AudioService audioService;

		// Token: 0x04005D4D RID: 23885
		[WaitForService(true, true)]
		private ILocalizationService localizationService;

		// Token: 0x04005D4E RID: 23886
		[SerializeField]
		private GameObject yesButton;

		// Token: 0x04005D4F RID: 23887
		[SerializeField]
		private GameObject noButton;

		// Token: 0x04005D50 RID: 23888
		[SerializeField]
		private GameObject alrightButton;

		// Token: 0x04005D51 RID: 23889
		[SerializeField]
		private TextMeshProUGUI titleLabel;

		// Token: 0x04005D52 RID: 23890
		[SerializeField]
		private TextMeshProUGUI bodyLabel;

		// Token: 0x04005D53 RID: 23891
		[SerializeField]
		private AnimatedUi dialog;

		// Token: 0x04005D54 RID: 23892
		public readonly AwaitSignal onClose = new AwaitSignal();
	}
}
