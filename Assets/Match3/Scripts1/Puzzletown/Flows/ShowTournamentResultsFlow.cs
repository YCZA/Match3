using System;
using System.Collections;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Puzzletown.UI;
using Match3.Scripts1.Shared.Flows;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.Signals;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Flows
{
	// Token: 0x020009D4 RID: 2516
	public class ShowTournamentResultsFlow : AFlow2<string, int>
	{
		// Token: 0x06003CE5 RID: 15589 RVA: 0x00132459 File Offset: 0x00130859
		public ShowTournamentResultsFlow(bool shouldShowErrorPopup, bool wasRefreshedRightNow)
		{
			this.shouldShowErrorPopup = shouldShowErrorPopup;
			this.shouldFetchAgain = !wasRefreshedRightNow;
		}

		// Token: 0x06003CE6 RID: 15590 RVA: 0x00132474 File Offset: 0x00130874
		protected override IEnumerator FlowRoutine(string input1, int input2)
		{
			if (ShowTournamentResultsFlow.flowRunning)
			{
				yield break;
			}
			ShowTournamentResultsFlow.flowRunning = true;
			yield return ServiceLocator.Instance.Inject(this);
			yield return SceneManager.Instance.Inject(this);
			this.StartSessionLifeCycle();
			Wooroutine<LoadingSpinnerRoot> loadingSpinner = null;
			if (this.tournamentService.SHOW_FULLSCREEN_SPINNER)
			{
				loadingSpinner = SceneManager.Instance.LoadScene<LoadingSpinnerRoot>(null);
				yield return loadingSpinner;
				yield return new WaitForSeconds(loadingSpinner.ReturnValue.dialog.open.length + 0.33f);
			}
			yield return this.WaitForLeagueModel(input1, input2, 4f);
			if (loadingSpinner != null)
			{
				loadingSpinner.ReturnValue.Close();
			}
			if (this.ShouldAbort())
			{
				if (this.tournamentService.CurrentContextID == this.startContextID && this.townBottomPanel != null && this.townBottomPanel.gameObject != null)
				{
					this.townBottomPanel.UpdateTournamentStatus();
				}
				this.EndSessionLifeCycle();
				yield break;
			}
			if (this._leagueModel.couldFetchFromServer || this.NotQualifiedButTournamentIsOver(this._leagueModel))
			{
				yield return this.TryGiveTournamentRewards(this._leagueModel);
				if (this._leagueModel.playerStatus == PlayerLeagueStatus.Entered)
				{
					this.TrackTournamentEnd(this._leagueModel);
				}
			}
			else
			{
				yield return this.HandleOfflineTournamentResultQueryRoutine();
			}
			this.EndSessionLifeCycle();
			yield break;
		}

		// Token: 0x06003CE7 RID: 15591 RVA: 0x0013249D File Offset: 0x0013089D
		private bool NotQualifiedButTournamentIsOver(LeagueModel lm)
		{
			return lm.playerStatus != PlayerLeagueStatus.Entered && this.tournamentService.GetTimeState(lm) == LeagueTimeState.Past;
		}

		// Token: 0x06003CE8 RID: 15592 RVA: 0x001324C0 File Offset: 0x001308C0
		private IEnumerator HandleOfflineTournamentResultQueryRoutine()
		{
			if (this.shouldShowErrorPopup)
			{
				yield return ConnectionErrorPopup.ShowAndWaitForClose();
			}
			else
			{
				this.townBottomPanel.Show(true);
			}
			this.townBottomPanel.UpdateTournamentStatus();
			yield break;
		}

		// Token: 0x06003CE9 RID: 15593 RVA: 0x001324DC File Offset: 0x001308DC
		private IEnumerator WaitForLeagueModel(string leagueID, int leagueEndTimestamp, float timeoutSeconds)
		{
			if (this.tournamentService.Status.IsBeingRefreshed || !this.shouldFetchAgain)
			{
				Wooroutine<LeagueModel> waitForRefresh = this.tournamentService.Status.WaitForActiveLeagueStateRefresh(timeoutSeconds);
				yield return waitForRefresh;
				this._leagueModel = waitForRefresh.ReturnValue;
			}
			else
			{
				if (this.ShouldOpenAfterRandomDelay(leagueEndTimestamp, this.tournamentService.Now))
				{
					yield return new WaitForSeconds(this.GetRandomDelayAmountInSeconds());
				}
				Wooroutine<LeagueModel> fetchRoutine = this.tournamentService.FetchLeagueState(false, leagueID, true);
				yield return fetchRoutine;
				this._leagueModel = fetchRoutine.ReturnValue;
			}
			yield break;
		}

		// Token: 0x06003CEA RID: 15594 RVA: 0x0013250C File Offset: 0x0013090C
		private bool ShouldAbort()
		{
			int currentContextID = this.tournamentService.CurrentContextID;
			return currentContextID != this.startContextID || CoreGameFlow.flowRunning || SceneManager.IsPlayingMatch3 || SceneManager.IsPopupOrTutorialShown();
		}

		// Token: 0x06003CEB RID: 15595 RVA: 0x0013254D File Offset: 0x0013094D
		private void StartSessionLifeCycle()
		{
			this.startContextID = this.tournamentService.CurrentContextID;
			this.sessionService.onRestart.AddListenerOnce(new Action(this.HandleSessionOver));
		}

		// Token: 0x06003CEC RID: 15596 RVA: 0x0013257C File Offset: 0x0013097C
		private void EndSessionLifeCycle()
		{
			ShowTournamentResultsFlow.flowRunning = false;
			this.sessionService.onRestart.RemoveListener(new Action(this.HandleSessionOver));
		}

		// Token: 0x06003CED RID: 15597 RVA: 0x001325A0 File Offset: 0x001309A0
		private void HandleSessionOver()
		{
			ShowTournamentResultsFlow.flowRunning = false;
		}

		// Token: 0x06003CEE RID: 15598 RVA: 0x001325A8 File Offset: 0x001309A8
		private bool ShouldOpenAfterRandomDelay(int end, int now)
		{
			return now - end < 3;
		}

		// Token: 0x06003CEF RID: 15599 RVA: 0x001325B0 File Offset: 0x001309B0
		private float GetRandomDelayAmountInSeconds()
		{
			float max = (float)this.configService.general.tournaments.league_over_query_max_delay_seconds;
			return global::UnityEngine.Random.Range(0f, max);
		}

		// Token: 0x06003CF0 RID: 15600 RVA: 0x001325E0 File Offset: 0x001309E0
		private IEnumerator TryGiveTournamentRewards(LeagueModel leagueModel)
		{
			if (leagueModel.playerStatus == PlayerLeagueStatus.Entered)
			{
				if (this.CanGiveOutRewards(leagueModel))
				{
					yield return this.ShowResultsPopup(leagueModel);
					Materials rewards = leagueModel.GetRewards();
					this.tournamentService.CollectRewardsAndRemoveLocalState(leagueModel, rewards);
				}
				else
				{
					yield return new ShowTournamentStandingsPopupFlow().Start(leagueModel);
				}
			}
			else
			{
				WoogaDebug.Log(new object[]
				{
					"Player hasn't qualified for the league, or league doesn't exist: ",
					leagueModel.config.id
				});
				this.tournamentService.CollectRewardsAndRemoveLocalState(leagueModel, null);
			}
			this.townBottomPanel.UpdateTournamentStatus();
			yield break;
		}

		// Token: 0x06003CF1 RID: 15601 RVA: 0x00132602 File Offset: 0x00130A02
		private bool CanGiveOutRewards(LeagueModel leagueModel)
		{
			return this.tournamentService.GetTimeState(leagueModel) == LeagueTimeState.Past && leagueModel.confirmedOver && leagueModel.sortedStandings != null;
		}

		// Token: 0x06003CF2 RID: 15602 RVA: 0x00132630 File Offset: 0x00130A30
		protected IEnumerator ShowResultsPopup(LeagueModel leagueModel)
		{
			Wooroutine<TournamentResultsV2Root> tourneyPopup = SceneManager.Instance.LoadSceneWithParams<TournamentResultsV2Root, LeagueModel>(leagueModel, null);
			yield return tourneyPopup;
			tourneyPopup.ReturnValue.Show();
			AwaitSignal<TournamentPopupFlowOperation> waitForTourneyPopupClose = tourneyPopup.ReturnValue.onClose.Await<TournamentPopupFlowOperation>();
			yield return waitForTourneyPopupClose;
			DecoTrophyItemWon decoItemWon = leagueModel.GetDecoTrophyWon();
			if (decoItemWon != DecoTrophyItemWon.None)
			{
				BuildingConfig buildingConfig = this.configService.buildingConfigList.GetConfig(decoItemWon);
				this.gameStateService.Buildings.StoreBuilding(buildingConfig, 1);
				this.gameStateService.Save(false);
				yield return new ForceUserPlaceDecoFlow(buildingConfig).Start();
			}
			yield break;
		}

		// Token: 0x06003CF3 RID: 15603 RVA: 0x00132654 File Offset: 0x00130A54
		private void TrackTournamentEnd(LeagueModel input)
		{
			this.trackingService.TrackTournament(input.config.id, input.tier, "end", input.config.config.tournamentType.ToString(), input.config.config.pointsToQualify, input.config.start, input.config.end, input.playerCurrentPoints, input.GetPlayerNameInLeague(), input.GetPlayerPosition(), input.sortedStandings.Length);
		}

		// Token: 0x0400659B RID: 26011
		public const float RESULT_FETCH_TIMEOUT_SECONDS = 4f;

		// Token: 0x0400659C RID: 26012
		public const int RANDOM_DELAY_REQUIRED = 3;

		// Token: 0x0400659D RID: 26013
		private static bool flowRunning;

		// Token: 0x0400659E RID: 26014
		[WaitForService(true, true)]
		private TournamentService tournamentService;

		// Token: 0x0400659F RID: 26015
		[WaitForService(true, true)]
		private TrackingService trackingService;

		// Token: 0x040065A0 RID: 26016
		[WaitForService(true, true)]
		private SessionService sessionService;

		// Token: 0x040065A1 RID: 26017
		[WaitForService(true, true)]
		private ConfigService configService;

		// Token: 0x040065A2 RID: 26018
		[WaitForService(true, true)]
		private GameStateService gameStateService;

		// Token: 0x040065A3 RID: 26019
		[WaitForRoot(false, false)]
		private TownBottomPanelRoot townBottomPanel;

		// Token: 0x040065A4 RID: 26020
		private readonly bool shouldShowErrorPopup;

		// Token: 0x040065A5 RID: 26021
		private readonly bool shouldFetchAgain;

		// Token: 0x040065A6 RID: 26022
		private LeagueModel _leagueModel;

		// Token: 0x040065A7 RID: 26023
		private int startContextID;
	}
}
