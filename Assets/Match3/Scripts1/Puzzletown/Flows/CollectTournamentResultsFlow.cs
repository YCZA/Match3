using System;
using System.Collections;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Shared.Flows;
using Wooga.Coroutines;
using Wooga.UnityFramework;

namespace Match3.Scripts1.Puzzletown.Flows
{
	// Token: 0x020009CE RID: 2510
	public class CollectTournamentResultsFlow : AFlow<Materials>
	{
		// Token: 0x06003CCB RID: 15563 RVA: 0x001311D8 File Offset: 0x0012F5D8
		protected override IEnumerator FlowRoutine(Materials input)
		{
			if (CollectTournamentResultsFlow.flowRunning)
			{
				yield break;
			}
			CollectTournamentResultsFlow.flowRunning = true;
			yield return ServiceLocator.Instance.Inject(this);
			this.StartSessionLifeCycle();
			Wooroutine<TournamentRewardsRoot> rewardPopup = SceneManager.Instance.LoadSceneWithParams<TournamentRewardsRoot, Materials>(input, null);
			yield return rewardPopup;
			rewardPopup.ReturnValue.Show();
			yield return rewardPopup.ReturnValue.onDisabled.Await();
			this.EndSessionLifeCycle();
			yield break;
		}

		// Token: 0x06003CCC RID: 15564 RVA: 0x001311FA File Offset: 0x0012F5FA
		private void StartSessionLifeCycle()
		{
			this.sessionService.onRestart.AddListenerOnce(new Action(this.HandleSessionOver));
		}

		// Token: 0x06003CCD RID: 15565 RVA: 0x00131218 File Offset: 0x0012F618
		private void EndSessionLifeCycle()
		{
			CollectTournamentResultsFlow.flowRunning = false;
			this.sessionService.onRestart.RemoveListener(new Action(this.HandleSessionOver));
		}

		// Token: 0x06003CCE RID: 15566 RVA: 0x0013123C File Offset: 0x0012F63C
		private void HandleSessionOver()
		{
			CollectTournamentResultsFlow.flowRunning = false;
		}

		// Token: 0x04006588 RID: 25992
		private static bool flowRunning;

		// Token: 0x04006589 RID: 25993
		[WaitForService(true, true)]
		private SessionService sessionService;
	}
}
