namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020006A4 RID: 1700
	public class DefinedSpawnerView : ABlinkingAnimatedView
	{
		// Token: 0x06002A5F RID: 10847 RVA: 0x000C2076 File Offset: 0x000C0476
		public void AnimateShellState(bool setClosed)
		{
			this.animator.SetBool("Closed", setClosed);
		}

		// Token: 0x06002A60 RID: 10848 RVA: 0x000C2089 File Offset: 0x000C0489
		protected override void OnEnable()
		{
			base.OnEnable();
			this.AnimateShellState(false);
		}

		// Token: 0x040053B7 RID: 21431
		private const string CLOSING_BOOL_EVENT_NAME = "Closed";
	}
}
