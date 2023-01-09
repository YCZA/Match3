namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x0200053E RID: 1342
	public class ClimberExitBrushComponent : IBrushComponent
	{
		// Token: 0x060023B3 RID: 9139 RVA: 0x0009EA88 File Offset: 0x0009CE88
		public ClimberExitBrushComponent(bool isExit)
		{
			this.isExit = isExit;
		}

		// Token: 0x060023B4 RID: 9140 RVA: 0x0009EA97 File Offset: 0x0009CE97
		public void PaintField(Field field, Fields fields)
		{
			if (!field.IsColorWheel)
			{
				field.isClimberExit = this.isExit;
			}
		}

		// Token: 0x04004F58 RID: 20312
		private readonly bool isExit;
	}
}
