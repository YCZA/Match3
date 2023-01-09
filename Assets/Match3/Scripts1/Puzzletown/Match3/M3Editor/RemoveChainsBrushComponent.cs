namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x02000548 RID: 1352
	public class RemoveChainsBrushComponent : IBrushComponent
	{
		// Token: 0x060023CC RID: 9164 RVA: 0x0009EF9E File Offset: 0x0009D39E
		public void PaintField(Field field, Fields fields)
		{
			field.numChains = 0;
		}
	}
}
