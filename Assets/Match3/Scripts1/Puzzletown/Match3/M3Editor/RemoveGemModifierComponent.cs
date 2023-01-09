namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x0200054A RID: 1354
	public class RemoveGemModifierComponent : IBrushComponent
	{
		// Token: 0x060023D0 RID: 9168 RVA: 0x0009EFC0 File Offset: 0x0009D3C0
		public void PaintField(Field field, Fields fields)
		{
			if (!field.gem.IsCannon)
			{
				field.gem.modifier = GemModifier.Undefined;
			}
		}
	}
}
