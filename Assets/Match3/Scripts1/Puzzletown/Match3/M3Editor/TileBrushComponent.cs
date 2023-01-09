namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x02000536 RID: 1334
	public class TileBrushComponent : IBrushComponent
	{
		// Token: 0x060023A3 RID: 9123 RVA: 0x0009E943 File Offset: 0x0009CD43
		public TileBrushComponent(int count)
		{
			this.count = count;
		}

		// Token: 0x060023A4 RID: 9124 RVA: 0x0009E952 File Offset: 0x0009CD52
		public void PaintField(Field field, Fields fields)
		{
			field.numTiles = this.count;
		}

		// Token: 0x04004F50 RID: 20304
		private readonly int count;
	}
}
