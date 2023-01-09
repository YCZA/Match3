namespace Match3.Scripts1.Puzzletown.Match3.Scoring
{
	// Token: 0x02000644 RID: 1604
	public class StarProcessor : AScoreProcessor<StarExplosion>
	{
		// Token: 0x060028BA RID: 10426 RVA: 0x000B5DDC File Offset: 0x000B41DC
		public StarProcessor(Match3Score score) : base(score)
		{
		}

		// Token: 0x060028BB RID: 10427 RVA: 0x000B5DE5 File Offset: 0x000B41E5
		protected override void DoProcess(StarExplosion input)
		{
			if (!this.score.success)
			{
				this.score.AddAmount(new MaterialAmount("linegem_linegem", 1, MaterialAmountUsage.Undefined, 0));
			}
		}
	}
}
