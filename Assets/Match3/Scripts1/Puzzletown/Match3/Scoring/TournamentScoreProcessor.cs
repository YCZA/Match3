namespace Match3.Scripts1.Puzzletown.Match3.Scoring
{
	// Token: 0x0200063E RID: 1598
	public class TournamentScoreProcessor : AScoreProcessor<ITournamentScoreMatch>
	{
		// Token: 0x060028AE RID: 10414 RVA: 0x000B5CC4 File Offset: 0x000B40C4
		public TournamentScoreProcessor(Match3Score score) : base(score)
		{
		}

		// Token: 0x060028AF RID: 10415 RVA: 0x000B5CCD File Offset: 0x000B40CD
		protected override void DoProcess(ITournamentScoreMatch input)
		{
			if (!this.score.success)
			{
				this.score.tournamentScore.Increment(1);
			}
		}
	}
}
