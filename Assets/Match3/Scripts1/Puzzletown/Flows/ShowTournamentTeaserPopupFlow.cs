using System;
using System.Collections;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Shared.Flows;
using Wooga.Coroutines;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;

namespace Match3.Scripts1.Puzzletown.Flows
{
	// Token: 0x020009D6 RID: 2518
	public class ShowTournamentTeaserPopupFlow : AFlow
	{
		// Token: 0x06003CFB RID: 15611 RVA: 0x0013323C File Offset: 0x0013163C
		protected override IEnumerator FlowRoutine()
		{
			// 审核版不显示tournament
			// #if REVIEW_VERSION
			// 	yield break;
			// #endif
			if (ShowTournamentTeaserPopupFlow.flowRunning)
			{
				yield break;
			}
			ShowTournamentTeaserPopupFlow.flowRunning = true;
			yield return ServiceLocator.Instance.Inject(this);
			this.StartSessionLifeCycle();
			Wooroutine<TournamentTeaserRoot> teaserPopup = SceneManager.Instance.LoadScene<TournamentTeaserRoot>(null);
			yield return teaserPopup;
			yield return teaserPopup.ReturnValue.onDestroyed;
			this.gameStateService.SetSeenFlag(TournamentTeaserRoot.SEEN_FLAG);
			this.EndSessionLifeCycle();
		}

		// Token: 0x06003CFC RID: 15612 RVA: 0x00133257 File Offset: 0x00131657
		private void StartSessionLifeCycle()
		{
			this.sessionService.onRestart.AddListenerOnce(new Action(this.HandleSessionOver));
		}

		// Token: 0x06003CFD RID: 15613 RVA: 0x00133275 File Offset: 0x00131675
		private void EndSessionLifeCycle()
		{
			ShowTournamentTeaserPopupFlow.flowRunning = false;
			this.sessionService.onRestart.RemoveListener(new Action(this.HandleSessionOver));
		}

		// Token: 0x06003CFE RID: 15614 RVA: 0x00133299 File Offset: 0x00131699
		private void HandleSessionOver()
		{
			ShowTournamentTeaserPopupFlow.flowRunning = false;
		}

		// Token: 0x040065AA RID: 26026
		[WaitForService(true, true)]
		private GameStateService gameStateService;

		// Token: 0x040065AB RID: 26027
		[WaitForService(true, true)]
		private SessionService sessionService;

		// Token: 0x040065AC RID: 26028
		private static bool flowRunning;
	}
}
