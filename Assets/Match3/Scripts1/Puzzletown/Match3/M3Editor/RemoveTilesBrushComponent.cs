namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x0200054F RID: 1359
	public class RemoveTilesBrushComponent : IBrushComponent
	{
		// Token: 0x060023DA RID: 9178 RVA: 0x0009F082 File Offset: 0x0009D482
		public void PaintField(Field field, Fields fields)
		{
			field.numTiles = 0;
		}
	}
}
