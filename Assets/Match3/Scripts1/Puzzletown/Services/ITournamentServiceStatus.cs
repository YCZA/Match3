using Wooga.Coroutines;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x02000808 RID: 2056
	public interface ITournamentServiceStatus
	{
		// Token: 0x17000803 RID: 2051
		// (get) Token: 0x060032C9 RID: 13001
		bool IsUnlocked { get; }

		// Token: 0x17000804 RID: 2052
		// (get) Token: 0x060032CA RID: 13002
		bool IsTeased { get; }

		// Token: 0x17000805 RID: 2053
		// (get) Token: 0x060032CB RID: 13003
		// (set) Token: 0x060032CC RID: 13004
		bool IsUserInterestedInTournament { get; set; }

		// Token: 0x17000806 RID: 2054
		// (get) Token: 0x060032CD RID: 13005
		bool IsBeingRefreshed { get; }

		// Token: 0x060032CE RID: 13006
		void CheatUnlock();

		// Token: 0x060032CF RID: 13007
		Wooroutine<LeagueModel> WaitForActiveLeagueStateRefresh(float timeoutInSeconds);
	}
}
