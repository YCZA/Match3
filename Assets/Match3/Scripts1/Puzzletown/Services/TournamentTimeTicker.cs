using System;
using System.Collections;
using System.Collections.Generic;
using Match3.Scripts1.Puzzletown.Config;
using Match3.Scripts1.Wooga.Signals;
using Match3.Scripts2.PlayerData;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x02000817 RID: 2071
	public class TournamentTimeTicker
	{
		// Token: 0x06003347 RID: 13127 RVA: 0x000F308C File Offset: 0x000F148C
		public IEnumerator Tick(TournamentService tournamentService, GameStateService gameStateService, List<TournamentEventConfig> activeConfigs)
		{
			int tickStarted = tournamentService.Now;
			WaitForSeconds waitForOneSecond = new WaitForSeconds(1f);
			this.configsToUpdate.Clear();
			if (activeConfigs != null && activeConfigs.Count > 0)
			{
				this.configsToUpdate.AddRange(activeConfigs);
			}
			this.onLeagueOverNotificationsSent.Clear();
			while (this.configsToUpdate != null && this.configsToUpdate.Count > 0)
			{
				int currentTime = tournamentService.Now;
				this.DoTick(currentTime - tickStarted, currentTime, gameStateService);
				yield return waitForOneSecond;
			}
			yield break;
		}

		// Token: 0x06003348 RID: 13128 RVA: 0x000F30BC File Offset: 0x000F14BC
		private void DoTick(int elapsed, int now, GameStateService gameStateService)
		{
			if (this.secondsElapsed != elapsed)
			{
				this.secondsElapsed = elapsed;
				if (this.onRemainingSecondsChanged.HasListener() || this.onTournamentOver.HasListener())
				{
					this.DispatchSignals(now, gameStateService);
				}
			}
		}

		// Token: 0x06003349 RID: 13129 RVA: 0x000F30FC File Offset: 0x000F14FC
		private void DispatchSignals(int now, GameStateService gameStateService)
		{
			foreach (TournamentEventConfig tournamentEventConfig in this.configsToUpdate)
			{
				TimeSpan val = TimeSpan.FromSeconds((double)(tournamentEventConfig.end - now));
				this.onRemainingSecondsChanged.Dispatch(tournamentEventConfig.id, val);
				LeagueModel leagueModel;
				if (val.TotalSeconds < 0.0 && !this.onLeagueOverNotificationsSent.Contains(tournamentEventConfig.id) && gameStateService.Tournaments.TryGetLocalStateFor(tournamentEventConfig, out leagueModel))
				{
					this.onLeagueOverNotificationsSent.Add(leagueModel.config.id);
					this.onTournamentOver.Dispatch(leagueModel);
				}
			}
		}

		// Token: 0x04005B77 RID: 23415
		public Signal<string, TimeSpan> onRemainingSecondsChanged = new Signal<string, TimeSpan>();

		// Token: 0x04005B78 RID: 23416
		public Signal<LeagueModel> onTournamentOver = new Signal<LeagueModel>();

		// Token: 0x04005B79 RID: 23417
		private List<TournamentEventConfig> configsToUpdate = new List<TournamentEventConfig>();

		// Token: 0x04005B7A RID: 23418
		private HashSet<string> onLeagueOverNotificationsSent = new HashSet<string>();

		// Token: 0x04005B7B RID: 23419
		private int secondsElapsed;
	}
}
