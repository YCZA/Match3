namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x02000539 RID: 1337
	public class ColorCratesBrushComponent : IBrushComponent
	{
		// Token: 0x060023A9 RID: 9129 RVA: 0x0009E9BE File Offset: 0x0009CDBE
		public ColorCratesBrushComponent(GemColor color)
		{
			this.color = color;
		}

		// Token: 0x060023AA RID: 9130 RVA: 0x0009E9D0 File Offset: 0x0009CDD0
		public void PaintField(Field field, Fields fields)
		{
			int num = Crate.GetHp(field.cratesIndex);
			if (num == 0)
			{
				num = 1;
			}
			field.SetCrates(this.color, num);
		}

		// Token: 0x04004F53 RID: 20307
		private readonly GemColor color;
	}
}
