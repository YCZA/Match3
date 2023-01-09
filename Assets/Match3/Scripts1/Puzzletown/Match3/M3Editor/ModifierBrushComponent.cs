namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x02000533 RID: 1331
	public class ModifierBrushComponent : IBrushComponent
	{
		// Token: 0x0600239D RID: 9117 RVA: 0x0009E8BB File Offset: 0x0009CCBB
		public ModifierBrushComponent(GemModifier modifier)
		{
			this.modifier = modifier;
		}

		// Token: 0x0600239E RID: 9118 RVA: 0x0009E8CA File Offset: 0x0009CCCA
		public virtual void PaintField(Field field, Fields fields)
		{
			field.gem.modifier = this.modifier;
		}

		// Token: 0x04004F4E RID: 20302
		protected readonly GemModifier modifier;
	}
}
