namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x02000552 RID: 1362
	public class RemoveGemBrushComponent : IBrushComponent
	{
		// Token: 0x060023E0 RID: 9184 RVA: 0x0009F15A File Offset: 0x0009D55A
		public void PaintField(Field field, Fields fields)
		{
			field.HasGem = false;
		}
	}
}
