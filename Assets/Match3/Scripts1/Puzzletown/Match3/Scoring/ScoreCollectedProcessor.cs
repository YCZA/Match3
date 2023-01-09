namespace Match3.Scripts1.Puzzletown.Match3.Scoring
{
	// Token: 0x02000640 RID: 1600
	public class ScoreCollectedProcessor : AScoreProcessor<IScoreCollectedResult>
	{
		// Token: 0x060028B2 RID: 10418 RVA: 0x000B5D1F File Offset: 0x000B411F
		public ScoreCollectedProcessor(Match3Score score) : base(score)
		{
		}

		// Token: 0x060028B3 RID: 10419 RVA: 0x000B5D28 File Offset: 0x000B4128
		protected override void DoProcess(IScoreCollectedResult input)
		{
			this.score.AddAmount(new MaterialAmount(input.Type, 1, MaterialAmountUsage.Undefined, 0));
		}
	}
}
