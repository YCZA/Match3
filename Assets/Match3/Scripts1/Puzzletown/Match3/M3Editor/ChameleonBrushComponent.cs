namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x02000546 RID: 1350
	public class ChameleonBrushComponent : IBrushComponent
	{
		// Token: 0x060023C7 RID: 9159 RVA: 0x0009EF3F File Offset: 0x0009D33F
		public ChameleonBrushComponent(ChameleonVariant chameleonVariant, GemDirection chameleonDirection)
		{
			this.chameleonVariant = chameleonVariant;
			this.chameleonDirection = chameleonDirection;
		}

		// Token: 0x060023C8 RID: 9160 RVA: 0x0009EF55 File Offset: 0x0009D355
		public void PaintField(Field field, Fields fields)
		{
			field.gem.color = GemColor.Red;
			field.gem.type = (GemType)this.chameleonVariant;
			field.gem.direction = this.chameleonDirection;
		}

		// Token: 0x04004F60 RID: 20320
		private readonly ChameleonVariant chameleonVariant;

		// Token: 0x04004F61 RID: 20321
		private readonly GemDirection chameleonDirection;
	}
}
