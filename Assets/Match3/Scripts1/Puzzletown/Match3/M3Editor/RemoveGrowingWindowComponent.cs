namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x02000547 RID: 1351
	public class RemoveGrowingWindowComponent : IBrushComponent
	{
		// Token: 0x060023CA RID: 9162 RVA: 0x0009EF8D File Offset: 0x0009D38D
		public void PaintField(Field field, Fields fields)
		{
			field.isGrowingWindow = false;
		}
	}
}
