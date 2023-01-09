namespace Match3.Scripts1.Puzzletown.Match3.Scoring
{
	// Token: 0x02000643 RID: 1603
	public class LinegemBombProcessor : AScoreProcessor<LinegemBombExplosion>
	{
		// Token: 0x060028B8 RID: 10424 RVA: 0x000B5DA9 File Offset: 0x000B41A9
		public LinegemBombProcessor(Match3Score score) : base(score)
		{
		}

		// Token: 0x060028B9 RID: 10425 RVA: 0x000B5DB2 File Offset: 0x000B41B2
		protected override void DoProcess(LinegemBombExplosion input)
		{
			if (!this.score.success)
			{
				this.score.AddAmount(new MaterialAmount("linegem_bomb", 1, MaterialAmountUsage.Undefined, 0));
			}
		}
	}
}
