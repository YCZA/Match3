namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x02000535 RID: 1333
	public class BlockerBrushComponent : IBrushComponent
	{
		// Token: 0x060023A1 RID: 9121 RVA: 0x0009E91B File Offset: 0x0009CD1B
		public BlockerBrushComponent(int count)
		{
			this.count = count;
		}

		// Token: 0x060023A2 RID: 9122 RVA: 0x0009E92A File Offset: 0x0009CD2A
		public void PaintField(Field field, Fields fields)
		{
			if (!field.IsColorWheel)
			{
				field.blockerIndex = this.count;
			}
		}

		// Token: 0x04004F4F RID: 20303
		private readonly int count;
	}
}
