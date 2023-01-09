namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x0200062B RID: 1579
	public class PTEditorBoardFactory : APTBoardFactory
	{
		// Token: 0x0600282C RID: 10284 RVA: 0x000B28E7 File Offset: 0x000B0CE7
		public PTEditorBoardFactory(LevelConfig config) : base(config)
		{
		}

		// Token: 0x0600282D RID: 10285 RVA: 0x000B28F0 File Offset: 0x000B0CF0
		public PTEditorBoardFactory()
		{
		}

		// Token: 0x0600282E RID: 10286 RVA: 0x000B28F8 File Offset: 0x000B0CF8
		protected override void SetupGem(Field field)
		{
			base.SetupGem(field);
			int parameter;
			if (Match3ConfigAmounts.modifierValueMap.TryGetValue(field.gem.modifier, out parameter))
			{
				field.gem.parameter = parameter;
			}
		}
	}
}
