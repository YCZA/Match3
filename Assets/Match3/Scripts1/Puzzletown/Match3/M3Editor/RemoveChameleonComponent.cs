namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x02000556 RID: 1366
	public class RemoveChameleonComponent : IBrushComponent
	{
		// Token: 0x060023E8 RID: 9192 RVA: 0x0009F1BF File Offset: 0x0009D5BF
		public void PaintField(Field field, Fields fields)
		{
			field.gem.type = GemType.Undefined;
			field.gem.direction = GemDirection.Undefined;
		}
	}
}
