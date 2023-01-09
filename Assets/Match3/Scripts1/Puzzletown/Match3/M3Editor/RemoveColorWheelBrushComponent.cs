namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x0200054E RID: 1358
	public class RemoveColorWheelBrushComponent : IBrushComponent
	{
		// Token: 0x060023D8 RID: 9176 RVA: 0x0009F061 File Offset: 0x0009D461
		public void PaintField(Field field, Fields fields)
		{
			if (field.IsColorWheel)
			{
				fields.RemoveColorWheel(field.gridPosition);
			}
		}
	}
}
