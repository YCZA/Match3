namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x0200053C RID: 1340
	public class DropItemExitBrushComponent : IBrushComponent
	{
		// Token: 0x060023AF RID: 9135 RVA: 0x0009EA43 File Offset: 0x0009CE43
		public DropItemExitBrushComponent(bool isExit)
		{
			this.isExit = isExit;
		}

		// Token: 0x060023B0 RID: 9136 RVA: 0x0009EA52 File Offset: 0x0009CE52
		public void PaintField(Field field, Fields fields)
		{
			field.isDropExit = this.isExit;
		}

		// Token: 0x04004F56 RID: 20310
		private readonly bool isExit;
	}
}
