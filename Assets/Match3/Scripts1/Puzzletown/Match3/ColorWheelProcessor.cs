namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x0200060C RID: 1548
	public class ColorWheelProcessor : AMatchProcessor
	{
		// Token: 0x060027A2 RID: 10146 RVA: 0x000B023C File Offset: 0x000AE63C
		protected override void CheckField(Field field, IntVector2 createdFrom)
		{
			if (field.IsColorWheel)
			{
				IntVector2 colorWheelCorner = this.fields.GetColorWheelCorner(field.gridPosition);
				if (this.fields.colorWheelModels[colorWheelCorner].colors.Contains(this.matchColor))
				{
					this.results.Add(new ColorWheelMatch(this.matchColor, this.fields.colorWheelModels[colorWheelCorner], field.gridPosition, createdFrom));
					this.fields.colorWheelModels[colorWheelCorner].colors.Remove(this.matchColor);
				}
			}
		}

		// Token: 0x060027A3 RID: 10147 RVA: 0x000B02E4 File Offset: 0x000AE6E4
		protected override void ProcessMatch(IMatchGroup match, Fields fields)
		{
			this.matchColor = match.Group.Color;
			this.fields = fields;
			if (match is RainbowSuperGemExplosion)
			{
				foreach (IntVector2 pos in ((RainbowSuperGemExplosion)match).ShowSupergemPositions)
				{
					this.CheckSurroundings(pos, fields);
				}
				return;
			}
			base.ProcessMatch(match, fields);
		}

		// Token: 0x04005207 RID: 20999
		private GemColor matchColor;

		// Token: 0x04005208 RID: 21000
		private Fields fields;
	}
}
