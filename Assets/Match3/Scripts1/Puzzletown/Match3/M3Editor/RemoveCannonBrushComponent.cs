namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x0200054B RID: 1355
	public class RemoveCannonBrushComponent : IBrushComponent
	{
		// Token: 0x060023D2 RID: 9170 RVA: 0x0009EFE6 File Offset: 0x0009D3E6
		public void PaintField(Field field, Fields fields)
		{
			if (field.gem.IsCannon)
			{
				field.gem.type = GemType.Undefined;
				field.gem.parameter = 0;
				field.gem.modifier = GemModifier.Undefined;
			}
		}
	}
}
