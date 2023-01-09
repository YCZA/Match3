using System;
using System.Collections;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Puzzletown.UI;
using Match3.Scripts1.Shared.Flows;
using Wooga.Coroutines;
using Wooga.UnityFramework;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Flows
{
	// Token: 0x020009D7 RID: 2519
	public class TournamentPopupFlow : AFlow<LeagueModel>
	{
		// Token: 0x06003D00 RID: 15616 RVA: 0x001333E0 File Offset: 0x001317E0
		protected override IEnumerator FlowRoutine(LeagueModel input)
		{
			if (input == null || TournamentPopupFlow.flowRunning)
			{
				yield break;
			}
			TournamentPopupFlow.flowRunning = true;
			yield return ServiceLocator.Instance.Inject(this);
			yield return SceneManager.Instance.Inject(this);
			this.StartSessionLifeCycle();
			LeagueTimeState currentTimeState = this.tournamentService.GetTimeState(input);
			if (!input.couldFetchFromServer && input.playerStatus == PlayerLeagueStatus.Entered && currentTimeState != LeagueTimeState.Past && Application.internetReachability != NetworkReachability.NotReachable)
			{
				Wooroutine<LoadingSpinnerRoot> loadingSpinner = null;
				if (this.tournamentService.SHOW_FULLSCREEN_SPINNER)
				{
					loadingSpinner = SceneManager.Instance.LoadScene<LoadingSpinnerRoot>(null);
					yield return loadingSpinner;
					yield return new WaitForSeconds(loadingSpinner.ReturnValue.dialog.open.length + 0.33f);
				}
				Wooroutine<LeagueModel> fetch = this.tournamentService.FetchLeagueState(true, input.config.id, true);
				yield return fetch;
				input = fetch.ReturnValue;
				if (loadingSpinner != null)
				{
					loadingSpinner.ReturnValue.Close();
				}
			}
			if (this.ShouldAbort())
			{
				this.EndSessionLifeCycle();
				yield break;
			}
			if (input.couldFetchFromServer)
			{
				currentTimeState = this.tournamentService.GetTimeState(input);
				this.townBottomPanel.UpdateTournamentStatus();
			}
			if (input.playerStatus != PlayerLeagueStatus.Entered && currentTimeState == LeagueTimeState.Running)
			{
				this.TrackTournamentOpen(input);
				Coroutine tryEnterLeagueFlow = new TryEnterTournamentFlow().Start(input);
				yield return tryEnterLeagueFlow;
			}
			else if (currentTimeState == LeagueTimeState.Running)
			{
				yield return this.TryShowStandingsPopup(input);
			}
			else if (currentTimeState == LeagueTimeState.Past)
			{
				yield return this.TryShowResultsPopup(input);
			}
			this.EndSessionLifeCycle();
			yield break;
		}

		// Token: 0x06003D01 RID: 15617 RVA: 0x00133404 File Offset: 0x00131804
		private bool ShouldAbort()
		{
			int currentContextID = this.tournamentService.CurrentContextID;
			return currentContextID != this.startContextID || CoreGameFlow.flowRunning || SceneManager.IsPlayingMatch3 || SceneManager.IsPopupOrTutorialShown();
		}

		// Token: 0x06003D02 RID: 15618 RVA: 0x00133445 File Offset: 0x00131845
		private void StartSessionLifeCycle()
		{
			this.startContextID = this.tournamentService.CurrentContextID;
			this.sessionService.onRestart.AddListenerOnce(new Action(this.HandleSessionOver));
		}

		// Token: 0x06003D03 RID: 15619 RVA: 0x00133474 File Offset: 0x00131874
		private void EndSessionLifeCycle()
		{
			TournamentPopupFlow.flowRunning = false;
			this.sessionService.onRestart.RemoveListener(new Action(this.HandleSessionOver));
		}

		// Token: 0x06003D04 RID: 15620 RVA: 0x00133498 File Offset: 0x00131898
		private void HandleSessionOver()
		{
			TournamentPopupFlow.flowRunning = false;
		}

		// Token: 0x06003D05 RID: 15621 RVA: 0x001334A0 File Offset: 0x001318A0
		private IEnumerator TryShowStandingsPopup(LeagueModel leagueModel)
		{
			Coroutine showStandingsPopupFlow = new ShowTournamentStandingsPopupFlow().Start(leagueModel);
			yield return showStandingsPopupFlow;
			yield break;
		}

		// Token: 0x06003D06 RID: 15622 RVA: 0x001334BC File Offset: 0x001318BC
		private IEnumerator TryShowResultsPopup(LeagueModel leagueModel)
		{
			Coroutine showTournamentResults = new ShowTournamentResultsFlow(true, false).Start(leagueModel.config.id, leagueModel.config.end);
			yield return showTournamentResults;
			yield break;
		}

		// Token: 0x06003D07 RID: 15623 RVA: 0x001334D8 File Offset: 0x001318D8
		private void TrackTournamentOpen(LeagueModel input)
		{
			this.trackingService.TrackTournament(input.config.id, string.Empty, "open", input.config.config.tournamentType.ToString(), input.config.config.pointsToQualify, input.config.start, input.config.end, 1, input.GetPlayerNameInLeague(), 0, 0);
		}

		// Token: 0x040065AD RID: 26029
		private static bool flowRunning;

		// Token: 0x040065AE RID: 26030
		[WaitForService(true, true)]
		private TournamentService tournamentService;

		// Token: 0x040065AF RID: 26031
		[WaitForService(true, true)]
		private TrackingService trackingService;

		// Token: 0x040065B0 RID: 26032
		[WaitForService(true, true)]
		private SessionService sessionService;

		// Token: 0x040065B1 RID: 26033
		[WaitForRoot(false, false)]
		private TownBottomPanelRoot townBottomPanel;

		// Token: 0x040065B2 RID: 26034
		private int startContextID;
	}
}
