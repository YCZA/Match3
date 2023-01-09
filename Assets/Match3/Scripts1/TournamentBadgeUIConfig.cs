using System;
using Match3.Scripts1.Puzzletown.Config;
using Match3.Scripts1.Puzzletown.Services;

namespace Match3.Scripts1
{
	// Token: 0x02000A4D RID: 2637
	public struct TournamentBadgeUIConfig
	{
		// Token: 0x06003F28 RID: 16168 RVA: 0x00142E80 File Offset: 0x00141280
		public TournamentBadgeUIConfig(LeagueModel model, TimeSpan timeLeft, bool showBlocked, TournamentType apparentTournamentTypeOverride = TournamentType.Undefined, bool showTeaser = false)
		{
			this.leagueModel = model;
			this.timeLeft = timeLeft;
			this.playerStatus = ((this.leagueModel == null) ? PlayerLeagueStatus.None : this.leagueModel.playerStatus);
			this.playerCurrentPosition = ((this.leagueModel == null) ? -1 : this.leagueModel.GetPlayerPosition());
			this.showBlocked = showBlocked;
			this.showTeaser = showTeaser;
			this.apparentTournamentType = ((apparentTournamentTypeOverride != TournamentType.Undefined || this.leagueModel == null) ? apparentTournamentTypeOverride : this.leagueModel.config.config.tournamentType);
		}

		// Token: 0x17000939 RID: 2361
		// (get) Token: 0x06003F29 RID: 16169 RVA: 0x00142F22 File Offset: 0x00141322
		public bool IsNotBeingRefreshed
		{
			get
			{
				return this.leagueModel != null;
			}
		}

		// Token: 0x06003F2A RID: 16170 RVA: 0x00142F30 File Offset: 0x00141330
		public static TournamentBadgeUIConfig CreateConfigForLeagueUnderRefresh(TournamentType tType)
		{
			return new TournamentBadgeUIConfig(null, TimeSpan.Zero, true, tType, false);
		}

		// Token: 0x06003F2B RID: 16171 RVA: 0x00142F40 File Offset: 0x00141340
		public static TournamentBadgeUIConfig CreateConfigForTeaser()
		{
			return new TournamentBadgeUIConfig(null, TimeSpan.Zero, false, TournamentType.Undefined, true);
		}

		// Token: 0x040068AD RID: 26797
		public readonly PlayerLeagueStatus playerStatus;

		// Token: 0x040068AE RID: 26798
		public readonly TimeSpan timeLeft;

		// Token: 0x040068AF RID: 26799
		public readonly int playerCurrentPosition;

		// Token: 0x040068B0 RID: 26800
		public readonly LeagueModel leagueModel;

		// Token: 0x040068B1 RID: 26801
		public readonly bool showBlocked;

		// Token: 0x040068B2 RID: 26802
		public readonly bool showTeaser;

		// Token: 0x040068B3 RID: 26803
		public readonly TournamentType apparentTournamentType;
	}
}
