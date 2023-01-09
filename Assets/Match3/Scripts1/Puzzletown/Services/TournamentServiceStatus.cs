using System.Collections;
using Wooga.Coroutines;
using Match3.Scripts2.PlayerData;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x02000816 RID: 2070
	public class TournamentServiceStatus : ITournamentServiceStatus
	{
		// Token: 0x0600333B RID: 13115 RVA: 0x000F2D71 File Offset: 0x000F1171
		public TournamentServiceStatus(TournamentService tService, ConfigService configService, ProgressionDataService.Service progService, GameStateService gameStateService)
		{
			this.configService = configService;
			this.progressionService = progService;
			this.gameStateService = gameStateService;
			this.tournamentService = tService;
		}

		// Token: 0x0600333C RID: 13116 RVA: 0x000F2DA8 File Offset: 0x000F11A8
		public override string ToString()
		{
			return string.Format("[TournamentServiceStatus: IsUnlocked={0}, IsTeased={1}, IsUserInterestedInTournament={2},\nIsBeingRefreshed={3}]", new object[]
			{
				this.IsUnlocked,
				this.IsTeased,
				this.IsUserInterestedInTournament,
				this.IsBeingRefreshed
			});
		}

		// Token: 0x17000813 RID: 2067
		// (get) Token: 0x0600333D RID: 13117 RVA: 0x000F2E00 File Offset: 0x000F1200
		public bool IsUnlocked
		{
			get
			{
				if (!this.cheatUnlocked && this.configService != null && this.progressionService != null)
				{
					return this.gameStateService.Tournaments.HasAnyLocalStateStarted || !this.progressionService.IsLocked(this.configService.general.tournaments.unlock_at_level);
				}
				return this.cheatUnlocked;
			}
		}

		// Token: 0x17000814 RID: 2068
		// (get) Token: 0x0600333E RID: 13118 RVA: 0x000F2E70 File Offset: 0x000F1270
		public bool IsTeased
		{
			get
			{
				return !this.IsUnlocked && this.configService != null && this.progressionService != null && !this.progressionService.IsLocked(this.configService.general.tournaments.tease_at_level);
			}
		}

		// Token: 0x17000815 RID: 2069
		// (get) Token: 0x0600333F RID: 13119 RVA: 0x000F2EC3 File Offset: 0x000F12C3
		// (set) Token: 0x06003340 RID: 13120 RVA: 0x000F2ECB File Offset: 0x000F12CB
		public bool IsUserInterestedInTournament { get; set; }

		// Token: 0x17000816 RID: 2070
		// (get) Token: 0x06003341 RID: 13121 RVA: 0x000F2ED4 File Offset: 0x000F12D4
		// (set) Token: 0x06003342 RID: 13122 RVA: 0x000F2EDC File Offset: 0x000F12DC
		public bool IsBeingRefreshed { get; set; }

		// Token: 0x06003343 RID: 13123 RVA: 0x000F2EE5 File Offset: 0x000F12E5
		public void CheatUnlock()
		{
			if (!this.IsUnlocked)
			{
				WoogaDebug.LogWarning(new object[]
				{
					"TournamentService: Tournament unlocked."
				});
				this.cheatUnlocked = true;
			}
			else
			{
				WoogaDebug.LogWarning(new object[]
				{
					"TournamentService: Tournament is already unlocked."
				});
			}
		}

		// Token: 0x06003344 RID: 13124 RVA: 0x000F2F24 File Offset: 0x000F1324
		public Wooroutine<LeagueModel> WaitForActiveLeagueStateRefresh(float timeoutInSeconds)
		{
			return WooroutineRunner.StartWooroutine<LeagueModel>(this.WaitForRefreshRoutine(timeoutInSeconds));
		}

		// Token: 0x06003345 RID: 13125 RVA: 0x000F2F34 File Offset: 0x000F1334
		private IEnumerator WaitForRefreshRoutine(float timeoutInSeconds)
		{
			float elapsedSeconds = 0f;
			while (elapsedSeconds < timeoutInSeconds && this.IsBeingRefreshed)
			{
				elapsedSeconds += Time.deltaTime;
				yield return this.checkRefreshInterval;
			}
			yield return this.tournamentService.GetActiveLeagueState();
			yield break;
		}

		// Token: 0x04005B6F RID: 23407
		private ConfigService configService;

		// Token: 0x04005B70 RID: 23408
		private ProgressionDataService.Service progressionService;

		// Token: 0x04005B71 RID: 23409
		private GameStateService gameStateService;

		// Token: 0x04005B72 RID: 23410
		private TournamentService tournamentService;

		// Token: 0x04005B73 RID: 23411
		private WaitForSeconds checkRefreshInterval = new WaitForSeconds(0.2f);

		// Token: 0x04005B74 RID: 23412
		private bool cheatUnlocked;
	}
}
