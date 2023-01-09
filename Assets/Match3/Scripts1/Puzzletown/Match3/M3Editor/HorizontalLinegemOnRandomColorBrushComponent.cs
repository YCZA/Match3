namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x02000531 RID: 1329
	public class HorizontalLinegemOnRandomColorBrushComponent : IBrushComponent
	{
		// Token: 0x06002399 RID: 9113 RVA: 0x0009E827 File Offset: 0x0009CC27
		public void PaintField(Field field, Fields fields)
		{
			if (field.gem.color == GemColor.Random && field.gem.type == GemType.Undefined)
			{
				field.gem.type = GemType.LineHorizontal;
			}
		}
	}
}
