namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x0200052C RID: 1324
	public class WindowBrushComponent : IBrushComponent
	{
		// Token: 0x0600238E RID: 9102 RVA: 0x0009E6A6 File Offset: 0x0009CAA6
		public void PaintField(Field field, Fields fields)
		{
			field.isWindow = true;
		}
	}
}
