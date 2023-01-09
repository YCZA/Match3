namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x02000538 RID: 1336
	public class CratesBrushComponent : IBrushComponent
	{
		// Token: 0x060023A7 RID: 9127 RVA: 0x0009E988 File Offset: 0x0009CD88
		public CratesBrushComponent(int hp)
		{
			this.hp = hp;
		}

		// Token: 0x060023A8 RID: 9128 RVA: 0x0009E998 File Offset: 0x0009CD98
		public void PaintField(Field field, Fields fields)
		{
			GemColor color = Crate.GetColor(field.cratesIndex);
			field.SetCrates(color, this.hp);
		}

		// Token: 0x04004F52 RID: 20306
		private readonly int hp;
	}
}
