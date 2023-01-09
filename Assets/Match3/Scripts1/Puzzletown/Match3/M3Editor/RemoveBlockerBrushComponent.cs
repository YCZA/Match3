namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x0200054C RID: 1356
	public class RemoveBlockerBrushComponent : IBrushComponent
	{
		// Token: 0x060023D4 RID: 9172 RVA: 0x0009F024 File Offset: 0x0009D424
		public void PaintField(Field field, Fields fields)
		{
			if (!field.IsColorWheel)
			{
				field.blockerIndex = 0;
			}
		}
	}
}
