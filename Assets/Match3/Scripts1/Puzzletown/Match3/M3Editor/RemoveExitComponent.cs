namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x02000555 RID: 1365
	public class RemoveExitComponent : IBrushComponent
	{
		// Token: 0x060023E6 RID: 9190 RVA: 0x0009F1A7 File Offset: 0x0009D5A7
		public void PaintField(Field field, Fields fields)
		{
			field.isDropExit = false;
			field.isClimberExit = false;
		}
	}
}
