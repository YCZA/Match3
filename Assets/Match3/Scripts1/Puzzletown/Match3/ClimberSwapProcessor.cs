namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020005B8 RID: 1464
	public class ClimberSwapProcessor : ISwapProcessor
	{
		// Token: 0x06002632 RID: 9778 RVA: 0x000AACC8 File Offset: 0x000A90C8
		public ClimberSwapProcessor(FieldMappings mappings)
		{
			this.fieldMappings = mappings;
		}

		// Token: 0x06002633 RID: 9779 RVA: 0x000AACD8 File Offset: 0x000A90D8
		public void ProcessSwap(Fields fields, Move move)
		{
			if (!move.isSwap || !fields.IsSwapPossible(move.from, move.to))
			{
				return;
			}
			Gem gem = fields[move.from].gem;
			Gem gem2 = fields[move.to].gem;
			if (gem.color == GemColor.Climber || gem2.color == GemColor.Climber)
			{
				this.fieldMappings.UpdateFieldMappings(fields);
			}
		}

		// Token: 0x0400511E RID: 20766
		private FieldMappings fieldMappings;
	}
}
