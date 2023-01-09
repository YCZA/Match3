using System;

// Token: 0x0200083E RID: 2110
namespace Match3.Scripts1
{
	public class BannerEndOfContentRoot : APtSceneRoot, IDisposableDialog, IHandler<PopupOperation>
	{
		// Token: 0x06003463 RID: 13411 RVA: 0x000FA679 File Offset: 0x000F8A79
		protected override void Go()
		{
			this.dialog.Show();
			BackButtonManager.Instance.AddAction(new Action(this.CloseViaBackButton));
		}

		// Token: 0x06003464 RID: 13412 RVA: 0x000FA69C File Offset: 0x000F8A9C
		protected void CloseViaBackButton()
		{
			this.Handle(PopupOperation.Close);
		}

		// Token: 0x06003465 RID: 13413 RVA: 0x000FA6A8 File Offset: 0x000F8AA8
		public void Handle(PopupOperation op)
		{
			if (this.dialog.IsClosing)
			{
				return;
			}
			if (op == PopupOperation.Close)
			{
				BackButtonManager.Instance.RemoveAction(new Action(this.CloseViaBackButton));
				this.dialog.Hide();
			}
		}

		// Token: 0x04005C3F RID: 23615
		public AnimatedUi dialog;
	}
}
