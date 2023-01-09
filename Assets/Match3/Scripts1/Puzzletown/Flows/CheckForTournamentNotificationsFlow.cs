using System;
using System.Collections;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Shared.Flows;
using Wooga.Coroutines;
using Wooga.UnityFramework;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Flows
{
	// Token: 0x020009CD RID: 2509
	public class CheckForTournamentNotificationsFlow : AFlow
	{
		// Token: 0x06003CC2 RID: 15554 RVA: 0x00130CC8 File Offset: 0x0012F0C8
		protected override IEnumerator FlowRoutine()
		{
			if (CheckForTournamentNotificationsFlow.flowRunning)
			{
				yield break;
			}
			CheckForTournamentNotificationsFlow.flowRunning = true;
			yield return ServiceLocator.Instance.Inject(this);
			this.StartSessionLifeCycle();
			if (this.tournamentService.Status.IsUnlocked)
			{
				bool refreshedRightNow = this.tournamentService.Status.IsBeingRefreshed;
				Wooroutine<LeagueModel> waitForRefresh = this.tournamentService.Status.WaitForActiveLeagueStateRefresh(2f);
				yield return waitForRefresh;
				yield return this.WaitForLoadingScreenDisappearRoutine();
				if (this.ShouldAbort())
				{
					this.EndSessionLifeCycle();
					yield break;
				}
				LeagueModel thisMightNeedPlayersAttention = waitForRefresh.ReturnValue;
				if (thisMightNeedPlayersAttention != null && !thisMightNeedPlayersAttention.fetchInProgress && this.tournamentService.NeedsPlayersAttention(thisMightNeedPlayersAttention))
				{
					if (this.IsTournamentOver(thisMightNeedPlayersAttention))
					{
						yield return this.TryShowTournamentResultsRoutine(thisMightNeedPlayersAttention, refreshedRightNow);
					}
					else if (!CheckForTournamentNotificationsFlow.canQualifyPopupWasShown)
					{
						CheckForTournamentNotificationsFlow.canQualifyPopupWasShown = true;
						CheckForTournamentNotificationsFlow.tournamentOverPopupWasShown = false;
						yield return new TournamentPopupFlow().Start(thisMightNeedPlayersAttention);
					}
				}
			}
			this.EndSessionLifeCycle();
			yield break;
		}

		// Token: 0x06003CC3 RID: 15555 RVA: 0x00130CE4 File Offset: 0x0012F0E4
		private IEnumerator WaitForLoadingScreenDisappearRoutine()
		{
			float elapsedTime = 0f;
			WaitForSeconds waitInterval = new WaitForSeconds(0.1f);
			while (elapsedTime < 1f && SceneManager.IsLoadingScreenShown())
			{
				yield return waitInterval;
				elapsedTime += 0.1f;
			}
			yield break;
		}

		// Token: 0x06003CC4 RID: 15556 RVA: 0x00130CF8 File Offset: 0x0012F0F8
		private bool IsTournamentOver(LeagueModel thisMightNeedPlayersAttention)
		{
			return this.tournamentService.GetTimeState(thisMightNeedPlayersAttention) == LeagueTimeState.Past;
		}

		// Token: 0x06003CC5 RID: 15557 RVA: 0x00130D0C File Offset: 0x0012F10C
		private IEnumerator TryShowTournamentResultsRoutine(LeagueModel model, bool refreshedRightNow)
		{
			if (!CheckForTournamentNotificationsFlow.tournamentOverPopupWasShown && Application.internetReachability != NetworkReachability.NotReachable)
			{
				CheckForTournamentNotificationsFlow.tournamentOverPopupWasShown = true;
				CheckForTournamentNotificationsFlow.canQualifyPopupWasShown = false;
				yield return new ShowTournamentResultsFlow(false, refreshedRightNow).Start(model.config.id, model.config.end);
			}
			yield break;
		}

		// Token: 0x06003CC6 RID: 15558 RVA: 0x00130D30 File Offset: 0x0012F130
		private bool ShouldAbort()
		{
			int currentContextID = this.tournamentService.CurrentContextID;
			return currentContextID != this.startContextID || CoreGameFlow.flowRunning || SceneManager.IsPlayingMatch3 || SceneManager.IsPopupOrTutorialShown();
		}

		// Token: 0x06003CC7 RID: 15559 RVA: 0x00130D71 File Offset: 0x0012F171
		private void StartSessionLifeCycle()
		{
			this.startContextID = this.tournamentService.CurrentContextID;
			this.sessionService.onRestart.AddListenerOnce(new Action(this.HandleSessionOver));
		}

		// Token: 0x06003CC8 RID: 15560 RVA: 0x00130DA0 File Offset: 0x0012F1A0
		private void EndSessionLifeCycle()
		{
			CheckForTournamentNotificationsFlow.flowRunning = false;
			this.sessionService.onRestart.RemoveListener(new Action(this.HandleSessionOver));
		}

		// Token: 0x06003CC9 RID: 15561 RVA: 0x00130DC4 File Offset: 0x0012F1C4
		private void HandleSessionOver()
		{
			CheckForTournamentNotificationsFlow.flowRunning = false;
		}

		// Token: 0x0400657F RID: 25983
		public const float STATUS_REFRESH_TIMEOUT_SECONDS = 2f;

		// Token: 0x04006580 RID: 25984
		private static bool canQualifyPopupWasShown;

		// Token: 0x04006581 RID: 25985
		private static bool tournamentOverPopupWasShown;

		// Token: 0x04006582 RID: 25986
		private static bool flowRunning;

		// Token: 0x04006583 RID: 25987
		[WaitForService(true, true)]
		private TournamentService tournamentService;

		// Token: 0x04006584 RID: 25988
		[WaitForService(true, true)]
		private SessionService sessionService;

		// Token: 0x04006585 RID: 25989
		private const float waitForLoadingScreenTimeoutDurationSeconds = 1f;

		// Token: 0x04006586 RID: 25990
		private const float loadingScreenCheckIntervalSecs = 0.1f;

		// Token: 0x04006587 RID: 25991
		protected int startContextID;
	}
}
