namespace Match3.Scripts1.Wooga.Leagues
{
	// Token: 0x0200041E RID: 1054
	public class StandingsQueryResponse
	{
		// Token: 0x170004B9 RID: 1209
		// (get) Token: 0x06001F02 RID: 7938 RVA: 0x00082F2C File Offset: 0x0008132C
		public bool IsConfirmedOver
		{
			get
			{
				return this.couldFetchFromServer && this.playerIsMemberOfLeague && !this.is_running;
			}
		}

		// Token: 0x06001F03 RID: 7939 RVA: 0x00082F50 File Offset: 0x00081350
		public static StandingsQueryResponse Failure(bool couldReachServer = true)
		{
			return new StandingsQueryResponse
			{
				couldFetchFromServer = couldReachServer,
				standings = new LeagueEntry[0]
			};
		}

		// Token: 0x04004AAF RID: 19119
		public bool couldFetchFromServer;

		// Token: 0x04004AB0 RID: 19120
		public bool playerIsMemberOfLeague;

		// Token: 0x04004AB1 RID: 19121
		public bool playerHadAlreadyRegisteredBefore;

		// Token: 0x04004AB2 RID: 19122
		public LeagueEntry user;

		// Token: 0x04004AB3 RID: 19123
		public bool is_running;

		// Token: 0x04004AB4 RID: 19124
		public string tier;

		// Token: 0x04004AB5 RID: 19125
		public LeagueEntry[] standings;
	}
}
