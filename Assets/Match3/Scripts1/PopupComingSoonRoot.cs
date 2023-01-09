using System;
using UnityEngine;

// Token: 0x02000847 RID: 2119
namespace Match3.Scripts1
{
	public class PopupComingSoonRoot : APtSceneRoot<DateTime>, IDisposableDialog, IHandler<PopupOperation>
	{
		// Token: 0x0600347F RID: 13439 RVA: 0x000FB4C2 File Offset: 0x000F98C2
		protected override void Go()
		{
			this.countdownTimer.SetTargetTime(this.parameters, false, null);
			this.dialog.Show();
			BackButtonManager.Instance.AddAction(new Action(this.Close));
		}

		// Token: 0x06003480 RID: 13440 RVA: 0x000FB4F8 File Offset: 0x000F98F8
		protected void Close()
		{
			BackButtonManager.Instance.RemoveAction(new Action(this.Close));
			this.dialog.Hide();
		}

		// Token: 0x06003481 RID: 13441 RVA: 0x000FB51B File Offset: 0x000F991B
		public void Handle(PopupOperation op)
		{
			if (this.dialog.IsClosing)
			{
				return;
			}
			if (op == PopupOperation.Close)
			{
				this.Close();
			}
		}

		// Token: 0x04005C73 RID: 23667
		public AnimatedUi dialog;

		// Token: 0x04005C74 RID: 23668
		[SerializeField]
		private CountdownTimer countdownTimer;
	}
}
