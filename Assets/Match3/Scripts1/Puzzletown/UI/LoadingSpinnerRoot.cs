using Wooga.UnityFramework;

namespace Match3.Scripts1.Puzzletown.UI
{
	// Token: 0x0200085E RID: 2142
	[LoadOptions(true, true, false)]
	public class LoadingSpinnerRoot : APtSceneRoot, IDisposableDialog
	{
		// Token: 0x060034F0 RID: 13552 RVA: 0x000FE320 File Offset: 0x000FC720
		protected override void Go()
		{
			this.dialog.Show();
		}

		// Token: 0x060034F1 RID: 13553 RVA: 0x000FE32D File Offset: 0x000FC72D
		public void Close()
		{
			this.dialog.Hide();
		}

		// Token: 0x04005CE7 RID: 23783
		public AnimatedUi dialog;
	}
}
