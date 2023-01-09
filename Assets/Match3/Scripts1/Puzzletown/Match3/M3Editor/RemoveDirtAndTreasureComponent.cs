namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x02000550 RID: 1360
	public class RemoveDirtAndTreasureComponent : IBrushComponent
	{
		// Token: 0x060023DC RID: 9180 RVA: 0x0009F094 File Offset: 0x0009D494
		public void PaintField(Field field, Fields fields)
		{
			if (field.gem.IsCoveredByDirt)
			{
				field.gem.modifier = GemModifier.Undefined;
			}
			if (field.gem.color == GemColor.Dirt || field.gem.color == GemColor.Treasure)
			{
				field.gem.color = GemColor.Random;
			}
		}
	}
}
