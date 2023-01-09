using System;
using Match3.Scripts1.Puzzletown.Config;
using Match3.Scripts1.Puzzletown.Match3.Scoring;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x020007B8 RID: 1976
	[Serializable]
	public class SerializedLeagueState
	{
		// Token: 0x060030AC RID: 12460 RVA: 0x000E42C8 File Offset: 0x000E26C8
		public SerializedLeagueState(TournamentEventConfig config)
		{
			this.config = config.Copy();
		}

		// Token: 0x170007AA RID: 1962
		// (get) Token: 0x060030AD RID: 12461 RVA: 0x000E42DC File Offset: 0x000E26DC
		public bool PlayerHasEntered
		{
			get
			{
				return this.HasQualified && this.hasEntered;
			}
		}

		// Token: 0x170007AB RID: 1963
		// (get) Token: 0x060030AE RID: 12462 RVA: 0x000E42F2 File Offset: 0x000E26F2
		public bool HasQualified
		{
			get
			{
				return this.config.config.pointsToQualify <= this.currentPoints;
			}
		}

		// Token: 0x170007AC RID: 1964
		// (get) Token: 0x060030AF RID: 12463 RVA: 0x000E430F File Offset: 0x000E270F
		public PlayerLeagueStatus Status
		{
			get
			{
				if (this.PlayerHasEntered)
				{
					return PlayerLeagueStatus.Entered;
				}
				if (this.HasQualified)
				{
					return PlayerLeagueStatus.NotEnteredButQualified;
				}
				return PlayerLeagueStatus.NotQualified;
			}
		}

		// Token: 0x060030B0 RID: 12464 RVA: 0x000E432C File Offset: 0x000E272C
		public void SetFinished(int endTimeStamp)
		{
			this.config.end = endTimeStamp;
		}

		// Token: 0x060030B1 RID: 12465 RVA: 0x000E433C File Offset: 0x000E273C
		public void Update(Match3Score score, int m3tier, bool shouldIgnoreMultiplier)
		{
			if (score.tournamentScore.TournamentType == this.config.config.tournamentType)
			{
				int scoreMultiplierForZeroBasedTier = this.config.config.GetScoreMultiplierForZeroBasedTier(m3tier, shouldIgnoreMultiplier);
				this.currentPoints += score.tournamentScore.CollectedPoints * scoreMultiplierForZeroBasedTier;
			}
		}

		// Token: 0x060030B2 RID: 12466 RVA: 0x000E4398 File Offset: 0x000E2798
		public static SerializedLeagueState EnteredWithPoints(TournamentEventConfig config, string userID, int points, string tier)
		{
			return new SerializedLeagueState(config)
			{
				hasEntered = true,
				currentPoints = points,
				previousPoints = points,
				registeredUserID = userID,
				tier = tier
			};
		}

		// Token: 0x060030B3 RID: 12467 RVA: 0x000E43D0 File Offset: 0x000E27D0
		public static SerializedLeagueState FromServerState(LeagueModel serverState)
		{
			SerializedLeagueState serializedLeagueState = new SerializedLeagueState(serverState.config);
			if (serverState.playerStatus == PlayerLeagueStatus.Entered)
			{
				serializedLeagueState.currentPoints = serverState.playerCurrentPoints;
				serializedLeagueState.previousPoints = serverState.playerCurrentPoints;
				serializedLeagueState.registeredUserID = serverState.userID;
				serializedLeagueState.hasEntered = true;
				serializedLeagueState.tier = serverState.tier;
			}
			else
			{
				serializedLeagueState.currentPoints = serverState.playerCurrentPoints;
				serializedLeagueState.previousPoints = 0;
				serializedLeagueState.registeredUserID = serverState.userID;
				serializedLeagueState.hasEntered = false;
				serializedLeagueState.tier = serverState.tier;
			}
			return serializedLeagueState;
		}

		// Token: 0x04005970 RID: 22896
		public TournamentEventConfig config;

		// Token: 0x04005971 RID: 22897
		public int currentPoints;

		// Token: 0x04005972 RID: 22898
		public int previousPoints;

		// Token: 0x04005973 RID: 22899
		public bool hasEntered;

		// Token: 0x04005974 RID: 22900
		public string tier;

		// Token: 0x04005975 RID: 22901
		public string registeredUserID;
	}
}
