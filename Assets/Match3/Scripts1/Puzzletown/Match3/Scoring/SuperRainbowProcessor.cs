namespace Match3.Scripts1.Puzzletown.Match3.Scoring
{
	// Token: 0x02000642 RID: 1602
	public class SuperRainbowProcessor : AScoreProcessor<SuperRainbowExplosion>
	{
		// Token: 0x060028B6 RID: 10422 RVA: 0x000B5D76 File Offset: 0x000B4176
		public SuperRainbowProcessor(Match3Score score) : base(score)
		{
		}

		// Token: 0x060028B7 RID: 10423 RVA: 0x000B5D7F File Offset: 0x000B417F
		protected override void DoProcess(SuperRainbowExplosion input)
		{
			if (!this.score.success)
			{
				this.score.AddAmount(new MaterialAmount("rainbow_rainbow", 1, MaterialAmountUsage.Undefined, 0));
			}
		}
	}
}
