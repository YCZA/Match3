using System.Collections;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Shared.Flows;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.Signals;
using Wooga.UnityFramework;

namespace Match3.Scripts1.Puzzletown.Flows
{
	// Token: 0x020009D1 RID: 2513
	public class M3_TournamentLossAversionFlow : AFlowR<TournamentScore, bool>
	{
		// Token: 0x06003CDC RID: 15580 RVA: 0x00131AD8 File Offset: 0x0012FED8
		protected override IEnumerator FlowRoutine(TournamentScore input)
		{
			yield return ServiceLocator.Instance.Inject(this);
			Wooroutine<PopupTournamentLevelFailRoot> lossAversionPopupLoading = SceneManager.Instance.LoadSceneWithParams<PopupTournamentLevelFailRoot, TournamentScore>(input, null);
			yield return lossAversionPopupLoading;
			PopupTournamentLevelFailRoot popup = lossAversionPopupLoading.ReturnValue;
			if (input.Multiplier > 1)
			{
				string substring = string.Format("_x{0}", input.Multiplier);
				popup.tournamentMultiplierIcon.sprite = popup.multiplierSprites.GetSimilar(substring);
			}
			popup.Show();
			AwaitSignal<bool> result = popup.onCompleted;
			yield return result;
			yield return result.Dispatched;
			yield break;
		}

		// Token: 0x04006591 RID: 26001
		[WaitForService(true, true)]
		private TournamentService tournamentService;
	}
}
