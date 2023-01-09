namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020005A8 RID: 1448
	public class BoostLineGemVerticalCreation : ABoostSupergemCreation
	{
		// Token: 0x060025DC RID: 9692 RVA: 0x000A8E17 File Offset: 0x000A7217
		public BoostLineGemVerticalCreation(Fields fields, IntVector2 position) : base(fields, position)
		{
		}

		// Token: 0x060025DD RID: 9693 RVA: 0x000A8E21 File Offset: 0x000A7221
		protected override Gem CreateGem(Gem oldGem)
		{
			oldGem.type = GemType.LineVertical;
			return oldGem;
		}
	}
}
