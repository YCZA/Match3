using System;
using System.Collections.Generic;
using System.Linq;
using Match3.Scripts1.Puzzletown.Config;
using Match3.Scripts1.Puzzletown.Match3.Scoring;
using Match3.Scripts1.Wooga.Signals;
using Match3.Scripts2.PlayerData;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x020007BB RID: 1979
	public class TournamentDataService : ADataService
	{
		// Token: 0x060030B5 RID: 12469 RVA: 0x000E44AA File Offset: 0x000E28AA
		public TournamentDataService(Func<GameState> getState) : base(getState)
		{
		}

		// Token: 0x170007AD RID: 1965
		// (get) Token: 0x060030B6 RID: 12470 RVA: 0x000E44C0 File Offset: 0x000E28C0
		public bool HasAnyLocalStateStarted
		{
			get
			{
				if (base.state.leagueStates == null || base.state.leagueStates.Count < 1)
				{
					return false;
				}
				return base.state.leagueStates.Any((SerializedLeagueState state) => state.currentPoints > 0);
			}
		}

		// Token: 0x060030B7 RID: 12471 RVA: 0x000E452A File Offset: 0x000E292A
		public void CreateLocalStateForLeague(TournamentEventConfig config)
		{
			if (base.state.leagueStates == null)
			{
				base.state.leagueStates = new List<SerializedLeagueState>();
			}
			base.state.leagueStates.Add(new SerializedLeagueState(config));
		}

		// Token: 0x060030B8 RID: 12472 RVA: 0x000E4564 File Offset: 0x000E2964
		public void OverwriteLocalWithServerLeagueState(LeagueModel serverState)
		{
			WoogaDebug.LogWarning(new object[]
			{
				"Overwriting local w/ server league state"
			});
			base.state.leagueStates.RemoveAll((SerializedLeagueState st) => st.config.id == serverState.config.id);
			SerializedLeagueState item = SerializedLeagueState.FromServerState(serverState);
			base.state.leagueStates.Add(item);
		}

		// Token: 0x060030B9 RID: 12473 RVA: 0x000E45CC File Offset: 0x000E29CC
		public bool NeedsSaveAfterUpdatingLocalState(LeagueModel serverState)
		{
			if (!serverState.couldFetchFromServer)
			{
				return false;
			}
			bool flag = true;
			SerializedLeagueState serializedLeagueState;
			if (this.TryGetLeagueState(serverState.config.id, out serializedLeagueState))
			{
				flag = (serializedLeagueState.Status != serverState.playerStatus || serializedLeagueState.currentPoints != serverState.playerCurrentPoints);
			}
			if (flag)
			{
				this.OverwriteLocalWithServerLeagueState(serverState);
				return true;
			}
			return false;
		}

		// Token: 0x060030BA RID: 12474 RVA: 0x000E463C File Offset: 0x000E2A3C
		public bool TryGetLocalStateFor(TournamentEventConfig config, out LeagueModel model)
		{
			model = null;
			List<SerializedLeagueState> currentPlayerLeagueStates = this.GetCurrentPlayerLeagueStates();
			if (currentPlayerLeagueStates != null)
			{
				SerializedLeagueState serializedLeagueState = currentPlayerLeagueStates.FirstOrDefault((SerializedLeagueState state) => state.config.id == config.id);
				if (serializedLeagueState != null)
				{
					model = LeagueModelFactory.FromSavedState(serializedLeagueState);
				}
			}
			return model != null;
		}

		// Token: 0x060030BB RID: 12475 RVA: 0x000E4690 File Offset: 0x000E2A90
		public PlayerStateInLeague GetPlayerStateInLeague(string leagueID)
		{
			SerializedLeagueState serializedLeagueState = null;
			if (this.TryGetLeagueState(leagueID, out serializedLeagueState))
			{
				return new PlayerStateInLeague
				{
					status = serializedLeagueState.Status,
					currentPoints = serializedLeagueState.currentPoints,
					previousPoints = serializedLeagueState.previousPoints,
					tier = serializedLeagueState.tier
				};
			}
			return PlayerStateInLeague.None();
		}

		// Token: 0x060030BC RID: 12476 RVA: 0x000E46F0 File Offset: 0x000E2AF0
		public void UpdateScores(Match3Score score, int currentTier, bool shouldIgnoreMultiplier)
		{
			List<SerializedLeagueState> currentPlayerLeagueStates = this.GetCurrentPlayerLeagueStates();
			if (currentPlayerLeagueStates != null)
			{
				foreach (SerializedLeagueState serializedLeagueState in currentPlayerLeagueStates)
				{
					serializedLeagueState.Update(score, currentTier, shouldIgnoreMultiplier);
				}
				this.onTournamentPointsUpdated.Dispatch();
			}
		}

		// Token: 0x060030BD RID: 12477 RVA: 0x000E4764 File Offset: 0x000E2B64
		public bool TryEnterLeague(TournamentEventConfig config, string userID, int points, string tier)
		{
			SerializedLeagueState serializedLeagueState = null;
			if (this.IsPlayerInLeague(config.id))
			{
				WoogaDebug.LogWarning(new object[]
				{
					"Player is already in league " + config.id
				});
				return false;
			}
			if (this.TryGetLeagueState(config.id, out serializedLeagueState))
			{
				serializedLeagueState.hasEntered = true;
				serializedLeagueState.currentPoints = points;
				serializedLeagueState.previousPoints = points;
				serializedLeagueState.registeredUserID = userID;
				serializedLeagueState.tier = tier;
				return true;
			}
			serializedLeagueState = SerializedLeagueState.EnteredWithPoints(config, userID, points, tier);
			base.state.leagueStates.Add(serializedLeagueState);
			WoogaDebug.Log(new object[]
			{
				"TournamentDataService: Player entered league! ",
				config.id
			});
			return true;
		}

		// Token: 0x060030BE RID: 12478 RVA: 0x000E4818 File Offset: 0x000E2C18
		public bool IsPlayerInLeague(string leagueID)
		{
			SerializedLeagueState serializedLeagueState;
			return this.TryGetLeagueState(leagueID, out serializedLeagueState) && serializedLeagueState.PlayerHasEntered;
		}

		// Token: 0x060030BF RID: 12479 RVA: 0x000E483C File Offset: 0x000E2C3C
		private bool TryGetLeagueState(string leagueID, out SerializedLeagueState leagueState)
		{
			foreach (SerializedLeagueState serializedLeagueState in base.state.leagueStates)
			{
				if (serializedLeagueState.config.id == leagueID)
				{
					leagueState = serializedLeagueState;
					return true;
				}
			}
			leagueState = null;
			return false;
		}

		// Token: 0x060030C0 RID: 12480 RVA: 0x000E48BC File Offset: 0x000E2CBC
		public void RemoveLocalState(string id)
		{
			if (base.state.leagueStates != null)
			{
				base.state.leagueStates.RemoveAll((SerializedLeagueState leagueState) => leagueState.config.id == id);
			}
		}

		// Token: 0x060030C1 RID: 12481 RVA: 0x000E4903 File Offset: 0x000E2D03
		public List<SerializedLeagueState> GetCurrentPlayerLeagueStates()
		{
			return base.state.leagueStates ?? new List<SerializedLeagueState>();
		}

		// Token: 0x060030C2 RID: 12482 RVA: 0x000E491C File Offset: 0x000E2D1C
		public void SetCurrentPlayerLeagueStates(IEnumerable<SerializedLeagueState> playerLeagueStates)
		{
			List<SerializedLeagueState> leagueStates = new List<SerializedLeagueState>(playerLeagueStates);
			base.state.leagueStates.Clear();
			base.state.leagueStates = leagueStates;
		}

		// Token: 0x04005980 RID: 22912
		public Signal onTournamentPointsUpdated = new Signal();
	}
}
