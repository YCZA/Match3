namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020005A7 RID: 1447
	public class BoostLineGemHorizontalCreation : ABoostSupergemCreation
	{
		// Token: 0x060025DA RID: 9690 RVA: 0x000A8E02 File Offset: 0x000A7202
		public BoostLineGemHorizontalCreation(Fields fields, IntVector2 position) : base(fields, position)
		{
		}

		// Token: 0x060025DB RID: 9691 RVA: 0x000A8E0C File Offset: 0x000A720C
		protected override Gem CreateGem(Gem oldGem)
		{
			oldGem.type = GemType.LineHorizontal;
			return oldGem;
		}
	}
}
