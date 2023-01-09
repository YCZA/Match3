using Match3.Scripts1.Puzzletown.Config;
using Wooga.Coroutines;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x02000809 RID: 2057
	public interface ITournamentService
	{
		// Token: 0x17000807 RID: 2055
		// (get) Token: 0x060032D0 RID: 13008
		int Now { get; }

		// Token: 0x17000808 RID: 2056
		// (get) Token: 0x060032D1 RID: 13009
		ITournamentServiceStatus Status { get; }

		// Token: 0x060032D2 RID: 13010
		TournamentEventConfig GetActiveTournamentEventConfig();

		// Token: 0x060032D3 RID: 13011
		bool HasPlayerEntered(TournamentEventConfig config);

		// Token: 0x060032D4 RID: 13012
		LeagueModel GetActiveLeagueState();

		// Token: 0x060032D5 RID: 13013
		Wooroutine<LeagueModel> TryEnterLeague(string leagueID);

		// Token: 0x060032D6 RID: 13014
		TournamentType GetApparentOngoingTournamentType();

		// Token: 0x060032D7 RID: 13015
		int GetCurrentScoreMultiplierForTier(int zeroBasedTier);

		// Token: 0x060032D8 RID: 13016
		void CollectRewardsAndRemoveLocalState(LeagueModel leaguemodel, Materials rewards);

		// Token: 0x060032D9 RID: 13017
		void NotifyContextChange(SceneContext newContext);
	}
}
