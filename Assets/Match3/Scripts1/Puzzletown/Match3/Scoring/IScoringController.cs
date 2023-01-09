using System.Collections.Generic;
using Match3.Scripts1.Puzzletown.Config;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1.Puzzletown.Match3.Scoring
{
	// Token: 0x02000638 RID: 1592
	public interface IScoringController
	{
		// Token: 0x1700069E RID: 1694
		// (get) Token: 0x0600286B RID: 10347
		Materials ObjectivesLeft { get; }

		// Token: 0x1700069F RID: 1695
		// (get) Token: 0x0600286C RID: 10348
		int MovesLeft { get; }

		// Token: 0x170006A0 RID: 1696
		// (get) Token: 0x0600286D RID: 10349
		TournamentType CurrentOngoingTournament { get; }

		// Token: 0x0600286E RID: 10350
		void HandleStepCompleted(List<List<IMatchResult>> results);

		// Token: 0x0600286F RID: 10351
		void HandleTournamentScoreIncreased(TournamentType eventType);

		// Token: 0x06002870 RID: 10352
		void UpdateScoreStatusAndDispatchGameOverIfNeeded();

		// Token: 0x06002871 RID: 10353
		TournamentScore GetTournamentScore();
	}
}
