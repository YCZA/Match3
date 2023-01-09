using System;
using System.Collections;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Puzzletown.UI;
using Match3.Scripts1.Shared.Flows;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.Signals;
using Wooga.UnityFramework;

namespace Match3.Scripts1.Puzzletown.Flows
{
	// Token: 0x020009D5 RID: 2517
	public class ShowTournamentStandingsPopupFlow : AFlow<LeagueModel>
	{
		// Token: 0x06003CF5 RID: 15605 RVA: 0x00132F48 File Offset: 0x00131348
		protected override IEnumerator FlowRoutine(LeagueModel input)
		{
			if (ShowTournamentStandingsPopupFlow.flowRunning)
			{
				yield break;
			}
			ShowTournamentStandingsPopupFlow.flowRunning = true;
			yield return ServiceLocator.Instance.Inject(this);
			this.StartSessionLifeCycle();
			if (!input.couldFetchFromServer || input.sortedStandings == null || input.playerStatus != PlayerLeagueStatus.Entered)
			{
				yield return ConnectionErrorPopup.ShowAndWaitForClose();
			}
			else
			{
				yield return this.ShowStandingsPopup(input);
			}
			this.EndSessionLifeCycle();
			yield break;
		}

		// Token: 0x06003CF6 RID: 15606 RVA: 0x00132F6A File Offset: 0x0013136A
		private void StartSessionLifeCycle()
		{
			this.sessionService.onRestart.AddListenerOnce(new Action(this.HandleSessionOver));
		}

		// Token: 0x06003CF7 RID: 15607 RVA: 0x00132F88 File Offset: 0x00131388
		private void EndSessionLifeCycle()
		{
			ShowTournamentStandingsPopupFlow.flowRunning = false;
			this.sessionService.onRestart.RemoveListener(new Action(this.HandleSessionOver));
		}

		// Token: 0x06003CF8 RID: 15608 RVA: 0x00132FAC File Offset: 0x001313AC
		private void HandleSessionOver()
		{
			ShowTournamentStandingsPopupFlow.flowRunning = false;
		}

		// Token: 0x06003CF9 RID: 15609 RVA: 0x00132FB4 File Offset: 0x001313B4
		protected IEnumerator ShowStandingsPopup(LeagueModel param)
		{
			Wooroutine<TournamentStandingsRoot> standingsPopup = SceneManager.Instance.LoadSceneWithParams<TournamentStandingsRoot, LeagueModel>(param, null);
			yield return standingsPopup;
			standingsPopup.ReturnValue.Show();
			AwaitSignal<bool> close = standingsPopup.ReturnValue.onClose.Await<bool>();
			yield return close;
			bool shouldShowLevelMapAfterClose = close.Dispatched;
			if (shouldShowLevelMapAfterClose)
			{
				new CoreGameFlow().Start(default(CoreGameFlow.Input));
			}
			yield break;
		}

		// Token: 0x040065A8 RID: 26024
		private static bool flowRunning;

		// Token: 0x040065A9 RID: 26025
		[WaitForService(true, true)]
		private SessionService sessionService;
	}
}
