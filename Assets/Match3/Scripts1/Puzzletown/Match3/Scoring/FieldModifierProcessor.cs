namespace Match3.Scripts1.Puzzletown.Match3.Scoring
{
	// Token: 0x0200063F RID: 1599
	public class FieldModifierProcessor : AScoreProcessor<IFieldModifierExplosion>
	{
		// Token: 0x060028B0 RID: 10416 RVA: 0x000B5CF0 File Offset: 0x000B40F0
		public FieldModifierProcessor(Match3Score score) : base(score)
		{
		}

		// Token: 0x060028B1 RID: 10417 RVA: 0x000B5CF9 File Offset: 0x000B40F9
		protected override void DoProcess(IFieldModifierExplosion input)
		{
			if (input.CountForObjective)
			{
				this.score.AddAmount(new MaterialAmount(input.Type, 1, MaterialAmountUsage.Undefined, 0));
			}
		}
	}
}
