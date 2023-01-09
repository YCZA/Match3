namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x02000532 RID: 1330
	public class CannonBrushComponent : IBrushComponent
	{
		// Token: 0x0600239A RID: 9114 RVA: 0x0009E856 File Offset: 0x0009CC56
		public CannonBrushComponent(GemModifier modifier)
		{
			this.modifier = modifier;
		}

		// Token: 0x0600239B RID: 9115 RVA: 0x0009E865 File Offset: 0x0009CC65
		private int CalculateChargeFromModifier(GemModifier modifier)
		{
			if (Match3ConfigAmounts.modifierValueMap.ContainsKey(modifier))
			{
				return Match3ConfigAmounts.modifierValueMap[modifier];
			}
			return 0;
		}

		// Token: 0x0600239C RID: 9116 RVA: 0x0009E884 File Offset: 0x0009CC84
		public void PaintField(Field field, Fields fields)
		{
			field.gem.modifier = this.modifier;
			field.gem.parameter = this.CalculateChargeFromModifier(this.modifier);
			field.gem.type = GemType.Cannon;
		}

		// Token: 0x04004F4D RID: 20301
		private readonly GemModifier modifier;
	}
}
