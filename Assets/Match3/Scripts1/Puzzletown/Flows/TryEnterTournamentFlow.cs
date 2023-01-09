using System;
using System.Collections;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Puzzletown.UI;
using Match3.Scripts1.Shared.Flows;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.Signals;
using Wooga.UnityFramework;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Flows
{
	// Token: 0x020009D9 RID: 2521
	public class TryEnterTournamentFlow : AFlow<LeagueModel>
	{
		// Token: 0x06003D09 RID: 15625 RVA: 0x00133A4C File Offset: 0x00131E4C
		protected override IEnumerator FlowRoutine(LeagueModel input)
		{
			if (TryEnterTournamentFlow.flowRunning)
			{
				yield break;
			}
			TryEnterTournamentFlow.flowRunning = true;
			yield return ServiceLocator.Instance.Inject(this);
			yield return SceneManager.Instance.Inject(this);
			this.StartSessionLifeCycle();
			this.tournamentService.Status.IsUserInterestedInTournament = true;
			yield return this.ShowQualifyingPopup(input);
			this.EndSessionLifeCycle();
			yield break;
		}

		// Token: 0x06003D0A RID: 15626 RVA: 0x00133A70 File Offset: 0x00131E70
		private bool ShouldAbort()
		{
			int currentContextID = this.tournamentService.CurrentContextID;
			return currentContextID != this.startContextID || CoreGameFlow.flowRunning || SceneManager.IsPlayingMatch3 || SceneManager.IsPopupOrTutorialShown();
		}

		// Token: 0x06003D0B RID: 15627 RVA: 0x00133AB1 File Offset: 0x00131EB1
		private void StartSessionLifeCycle()
		{
			this.startContextID = this.tournamentService.CurrentContextID;
			this.sessionService.onRestart.AddListenerOnce(new Action(this.HandleSessionOver));
		}

		// Token: 0x06003D0C RID: 15628 RVA: 0x00133AE0 File Offset: 0x00131EE0
		private void EndSessionLifeCycle()
		{
			TryEnterTournamentFlow.flowRunning = false;
			this.sessionService.onRestart.RemoveListener(new Action(this.HandleSessionOver));
		}

		// Token: 0x06003D0D RID: 15629 RVA: 0x00133B04 File Offset: 0x00131F04
		private void HandleSessionOver()
		{
			TryEnterTournamentFlow.flowRunning = false;
		}

		// Token: 0x06003D0E RID: 15630 RVA: 0x00133B0C File Offset: 0x00131F0C
		protected IEnumerator ShowQualifyingPopup(LeagueModel leagueModel)
		{
			Wooroutine<TournamentQualifyingRoot> tourneyPopup = SceneManager.Instance.LoadSceneWithParams<TournamentQualifyingRoot, LeagueModel>(leagueModel, null);
			yield return tourneyPopup;
			AwaitSignal<TournamentPopupFlowOperation> tryEnterLeagueAfterPopupClose = tourneyPopup.ReturnValue.onClose.Await<TournamentPopupFlowOperation>();
			yield return tryEnterLeagueAfterPopupClose;
			TournamentPopupFlowOperation shouldTryEnterLeague = tryEnterLeagueAfterPopupClose.Dispatched;
			if (shouldTryEnterLeague == TournamentPopupFlowOperation.TryEnterTournament)
			{
				Wooroutine<LoadingSpinnerRoot> loadingSpinner = null;
				if (this.tournamentService.SHOW_FULLSCREEN_SPINNER)
				{
					loadingSpinner = SceneManager.Instance.LoadScene<LoadingSpinnerRoot>(null);
					yield return loadingSpinner;
					yield return new WaitForSeconds(loadingSpinner.ReturnValue.dialog.open.length + 0.33f);
				}
				Wooroutine<LeagueModel> enterRoutine = this.tournamentService.TryEnterLeague(leagueModel.config.id);
				yield return enterRoutine;
				LeagueModel stateAfterEntering = enterRoutine.ReturnValue;
				if (loadingSpinner != null)
				{
					loadingSpinner.ReturnValue.Close();
				}
				if (stateAfterEntering != null && stateAfterEntering.couldFetchFromServer)
				{
					this.TrackTournamentEnter(stateAfterEntering);
					this.externalGamesService.ShowJoinTournamentAchievement();
				}
				if (stateAfterEntering == null || this.ShouldAbort())
				{
					this.EndSessionLifeCycle();
					yield break;
				}
				if (stateAfterEntering.couldFetchFromServer)
				{
					yield return this.ShowStandings(stateAfterEntering);
				}
				else
				{
					yield return ConnectionErrorPopup.ShowAndWaitForClose();
				}
			}
			if (leagueModel.playerStatus == PlayerLeagueStatus.NotEnteredButQualified && shouldTryEnterLeague == TournamentPopupFlowOperation.Nothing)
			{
				WoogaDebug.Log(new object[]
				{
					"Tournament entry popup dismissed."
				});
				this.tournamentService.Status.IsUserInterestedInTournament = false;
			}
			this.townBottomPanel.UpdateTournamentStatus();
			if (shouldTryEnterLeague == TournamentPopupFlowOperation.OpenLevelMap)
			{
				new CoreGameFlow().Start(default(CoreGameFlow.Input));
			}
			yield break;
		}

		// Token: 0x06003D0F RID: 15631 RVA: 0x00133B30 File Offset: 0x00131F30
		protected IEnumerator ShowStandings(LeagueModel leagueStateAfterEntering)
		{
			Coroutine showPopup = new ShowTournamentStandingsPopupFlow().Start(leagueStateAfterEntering);
			yield return showPopup;
			yield break;
		}

		// Token: 0x06003D10 RID: 15632 RVA: 0x00133B4C File Offset: 0x00131F4C
		private void TrackTournamentEnter(LeagueModel input)
		{
			this.trackingService.TrackTournament(input.config.id, input.tier, "enter", input.config.config.tournamentType.ToString(), input.config.config.pointsToQualify, input.config.start, input.config.end, input.playerCurrentPoints, input.GetPlayerNameInLeague(), input.GetPlayerPosition(), input.sortedStandings.Length);
		}

		// Token: 0x040065B7 RID: 26039
		private static bool flowRunning;

		// Token: 0x040065B8 RID: 26040
		[WaitForService(true, true)]
		private TournamentService tournamentService;

		// Token: 0x040065B9 RID: 26041
		[WaitForService(true, true)]
		private TrackingService trackingService;

		// Token: 0x040065BA RID: 26042
		[WaitForService(true, true)]
		private SessionService sessionService;

		// Token: 0x040065BB RID: 26043
		[WaitForService(true, true)]
		private ExternalGamesService externalGamesService;

		// Token: 0x040065BC RID: 26044
		[WaitForRoot(false, false)]
		private TownBottomPanelRoot townBottomPanel;

		// Token: 0x040065BD RID: 26045
		private int startContextID;
	}
}
