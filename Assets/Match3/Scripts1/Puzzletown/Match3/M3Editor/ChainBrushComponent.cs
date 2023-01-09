namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x02000537 RID: 1335
	public class ChainBrushComponent : IBrushComponent
	{
		// Token: 0x060023A5 RID: 9125 RVA: 0x0009E960 File Offset: 0x0009CD60
		public ChainBrushComponent(int count)
		{
			this.count = count;
		}

		// Token: 0x060023A6 RID: 9126 RVA: 0x0009E96F File Offset: 0x0009CD6F
		public void PaintField(Field field, Fields fields)
		{
			if (!field.IsColorWheel)
			{
				field.numChains = this.count;
			}
		}

		// Token: 0x04004F51 RID: 20305
		private readonly int count;
	}
}
