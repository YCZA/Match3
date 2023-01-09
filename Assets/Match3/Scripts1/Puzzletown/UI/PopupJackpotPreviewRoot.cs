using System;
using Match3.Scripts1.Audio;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Wooga.Signals;
using Wooga.UnityFramework;

namespace Match3.Scripts1.Puzzletown.UI
{
	// Token: 0x02000872 RID: 2162
	[LoadOptions(true, false, false)]
	public class PopupJackpotPreviewRoot : APtSceneRoot, IHandler<PopupOperation>, IDisposableDialog
	{
		// Token: 0x0600354A RID: 13642 RVA: 0x001001F4 File Offset: 0x000FE5F4
		protected override void Go()
		{
			this.dialog.Show();
			BackButtonManager.Instance.AddAction(new Action(this.Close));
			this.audioService.PlaySFX(AudioId.PopupShowDefault, false, false, false);
		}

		// Token: 0x0600354B RID: 13643 RVA: 0x0010022C File Offset: 0x000FE62C
		public void Close()
		{
			this.dialog.Hide();
			BackButtonManager.Instance.RemoveAction(new Action(this.Close));
			this.audioService.PlaySFX(AudioId.PopupHideDefault, false, false, false);
			this.onClose.Dispatch();
		}

		// Token: 0x0600354C RID: 13644 RVA: 0x00100279 File Offset: 0x000FE679
		public void Handle(PopupOperation evt)
		{
			this.Close();
		}

		// Token: 0x04005D39 RID: 23865
		[WaitForService(true, true)]
		private AudioService audioService;

		// Token: 0x04005D3A RID: 23866
		public readonly AwaitSignal onClose = new AwaitSignal();

		// Token: 0x04005D3B RID: 23867
		public AnimatedUi dialog;
	}
}
