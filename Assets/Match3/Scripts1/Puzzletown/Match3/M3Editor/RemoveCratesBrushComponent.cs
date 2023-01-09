namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x02000549 RID: 1353
	public class RemoveCratesBrushComponent : IBrushComponent
	{
		// Token: 0x060023CE RID: 9166 RVA: 0x0009EFAF File Offset: 0x0009D3AF
		public void PaintField(Field field, Fields fields)
		{
			field.cratesIndex = 0;
		}
	}
}
