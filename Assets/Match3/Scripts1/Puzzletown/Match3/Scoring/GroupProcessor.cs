using Match3.Scripts1.Puzzletown.Config;

namespace Match3.Scripts1.Puzzletown.Match3.Scoring
{
	// Token: 0x0200063D RID: 1597
	public class GroupProcessor : AScoreProcessor<IMatchGroup>
	{
		// Token: 0x060028AC RID: 10412 RVA: 0x000B5BB6 File Offset: 0x000B3FB6
		public GroupProcessor(Match3Score score, TournamentType tournamentType) : base(score)
		{
			this.currentOngoingTournament = tournamentType;
		}

		// Token: 0x060028AD RID: 10413 RVA: 0x000B5BC8 File Offset: 0x000B3FC8
		protected override void DoProcess(IMatchGroup result)
		{
			foreach (Gem gem in result.Group)
			{
				this.score.AddAmount(new MaterialAmount(gem.color.ToString().ToLower(), 1, MaterialAmountUsage.Undefined, 0));
				if (!this.score.success)
				{
					this.score.AddAmount(new MaterialAmount(gem.color.ToString().ToLower() + "_before_hurray", 1, MaterialAmountUsage.Undefined, 0));
					if (TournamentConfig.IsFruitTournament(this.currentOngoingTournament) && TournamentConfig.IsGemColorMatchingFruitTournament(this.currentOngoingTournament, gem.color))
					{
						this.score.tournamentScore.Increment(1);
					}
				}
			}
		}

		// Token: 0x04005291 RID: 21137
		public readonly TournamentType currentOngoingTournament;
	}
}
