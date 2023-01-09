using System.Text;
using Match3.Scripts1.Puzzletown.Config;
using Match3.Scripts1.Wooga.Leagues;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x0200080C RID: 2060
	public class LeagueModel
	{
		// Token: 0x060032DC RID: 13020 RVA: 0x000EFB50 File Offset: 0x000EDF50
		public LeagueModel(string sbsUserID, string tier, bool confirmedOver = false)
		{
			this.userID = sbsUserID;
			this.tier = tier;
			this.confirmedOver = confirmedOver;
		}

		// Token: 0x060032DD RID: 13021 RVA: 0x000EFB6D File Offset: 0x000EDF6D
		private bool CanCollectRewards()
		{
			return this.playerStatus == PlayerLeagueStatus.Entered && this.config != null && this.config.config != null;
		}

		// Token: 0x060032DE RID: 13022 RVA: 0x000EFB9A File Offset: 0x000EDF9A
		public TournamentRewardCategory GetPlayersRewardCategory()
		{
			return (!this.CanCollectRewards()) ? TournamentRewardCategory.None : this.config.config.rewards.GetRewardCategoryFor(this.GetPlayerPosition());
		}

		// Token: 0x060032DF RID: 13023 RVA: 0x000EFBC8 File Offset: 0x000EDFC8
		public Materials GetRewards()
		{
			return (!this.CanCollectRewards()) ? null : this.config.config.rewards.GetRewards(this.GetPlayerPosition());
		}

		// Token: 0x060032E0 RID: 13024 RVA: 0x000EFBF8 File Offset: 0x000EDFF8
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("ID: {0} // ", this.config.id);
			stringBuilder.AppendFormat("Tier: {0} // ", this.tier);
			stringBuilder.AppendFormat("Player status: {0} // ", this.playerStatus);
			stringBuilder.AppendFormat("Player rank: {0} // ", this.GetPlayerPosition());
			int num = (this.sortedStandings != null) ? this.sortedStandings.Length : 0;
			stringBuilder.AppendFormat("Playercount: {0} // ", num);
			return stringBuilder.ToString();
		}

		// Token: 0x060032E1 RID: 13025 RVA: 0x000EFC94 File Offset: 0x000EE094
		public int GetPlayerPosition()
		{
			LeagueEntry leagueEntry;
			int result;
			if (this.TryGetPlayerStanding(out leagueEntry, out result))
			{
				return result;
			}
			return -1;
		}

		// Token: 0x060032E2 RID: 13026 RVA: 0x000EFCB4 File Offset: 0x000EE0B4
		public bool TryGetPlayerStanding(out LeagueEntry standing, out int position)
		{
			position = -1;
			standing = null;
			if (!this.couldFetchFromServer || this.sortedStandings == null || this.playerStatus != PlayerLeagueStatus.Entered)
			{
				return false;
			}
			for (int i = 0; i < this.sortedStandings.Length; i++)
			{
				if (this.sortedStandings[i].sbs_user_id == this.userID)
				{
					position = i + 1;
					standing = this.sortedStandings[i];
					return true;
				}
			}
			return false;
		}

		// Token: 0x060032E3 RID: 13027 RVA: 0x000EFD34 File Offset: 0x000EE134
		public string GetPlayerNameInLeague()
		{
			LeagueEntry leagueEntry;
			int num;
			if (this.TryGetPlayerStanding(out leagueEntry, out num))
			{
				return leagueEntry.name;
			}
			return string.Empty;
		}

		// Token: 0x04005B39 RID: 23353
		public const int NOT_IN_LEAGUE = -1;

		// Token: 0x04005B3A RID: 23354
		public bool couldFetchFromServer;

		// Token: 0x04005B3B RID: 23355
		public TournamentEventConfig config;

		// Token: 0x04005B3C RID: 23356
		public PlayerLeagueStatus playerStatus;

		// Token: 0x04005B3D RID: 23357
		public int playerCurrentPoints;

		// Token: 0x04005B3E RID: 23358
		public LeagueEntry[] sortedStandings;

		// Token: 0x04005B3F RID: 23359
		public readonly string userID;

		// Token: 0x04005B40 RID: 23360
		public readonly string tier;

		// Token: 0x04005B41 RID: 23361
		public bool fetchInProgress;

		// Token: 0x04005B42 RID: 23362
		public readonly bool confirmedOver;
	}
}
