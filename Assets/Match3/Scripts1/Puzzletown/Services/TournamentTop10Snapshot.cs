using System.Collections.Generic;
using Match3.Scripts2.Env;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x02000812 RID: 2066
	public class TournamentTop10Snapshot
	{
		// Token: 0x06003304 RID: 13060 RVA: 0x000F0AD8 File Offset: 0x000EEED8
		public TournamentTop10Snapshot(LeagueModel model, int topPlayersCount)
		{
			this.isPlayerOutOfTop10 = true;
			this.endAsUnixTimestamp = 0;
			this.leagueID = string.Empty;
			if (model.IsValid() && model.sortedStandings != null && model.couldFetchFromServer)
			{
				this.leagueID = model.config.id;
				this.endAsUnixTimestamp = model.config.end;
				this.topPlayerSbsIDs = new HashSet<string>();
				int num = 0;
				while (num < topPlayersCount && num < model.sortedStandings.Length)
				{
					string sbs_user_id = model.sortedStandings[num].sbs_user_id;
					this.topPlayerSbsIDs.Add(sbs_user_id);
					if (sbs_user_id == model.userID)
					{
						this.isPlayerOutOfTop10 = false;
					}
					num++;
				}
			}
		}

		// Token: 0x1700080C RID: 2060
		// (get) Token: 0x06003305 RID: 13061 RVA: 0x000F0BA8 File Offset: 0x000EEFA8
		public static int TopPlayersCount
		{
			get
			{
				GameEnvironment.Environment currentEnvironment = GameEnvironment.CurrentEnvironment;
				if (currentEnvironment == GameEnvironment.Environment.STAGING)
				{
					return 5;
				}
				if (currentEnvironment != GameEnvironment.Environment.PRODUCTION)
				{
					return 2;
				}
				return 10;
			}
		}

		// Token: 0x06003306 RID: 13062 RVA: 0x000F0BD4 File Offset: 0x000EEFD4
		public bool IsValid(int now)
		{
			return this.topPlayerSbsIDs != null && this.endAsUnixTimestamp > now;
		}

		// Token: 0x06003307 RID: 13063 RVA: 0x000F0BED File Offset: 0x000EEFED
		public bool ContainsUserID(string uid)
		{
			return this.topPlayerSbsIDs != null && this.topPlayerSbsIDs.Contains(uid);
		}

		// Token: 0x04005B4F RID: 23375
		public const int TOP_PLAYER_COUNT_CI = 2;

		// Token: 0x04005B50 RID: 23376
		public const int TOP_PLAYER_COUNT_STAGING = 5;

		// Token: 0x04005B51 RID: 23377
		public const int TOP_PLAYER_COUNT_PRODUCTION = 10;

		// Token: 0x04005B52 RID: 23378
		public readonly string leagueID;

		// Token: 0x04005B53 RID: 23379
		public readonly int endAsUnixTimestamp;

		// Token: 0x04005B54 RID: 23380
		public readonly bool isPlayerOutOfTop10;

		// Token: 0x04005B55 RID: 23381
		private readonly HashSet<string> topPlayerSbsIDs;
	}
}
