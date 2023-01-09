namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x02000545 RID: 1349
	public class GrowingWindowBrushComponent : IBrushComponent
	{
		// Token: 0x060023C6 RID: 9158 RVA: 0x0009EF36 File Offset: 0x0009D336
		public void PaintField(Field field, Fields fields)
		{
			field.isGrowingWindow = true;
		}
	}
}
