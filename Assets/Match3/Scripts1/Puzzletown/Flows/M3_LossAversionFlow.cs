using System.Collections;
using Match3.Scripts1.Puzzletown.Match3;
using Match3.Scripts1.Puzzletown.Match3.Scoring;
using Match3.Scripts1.Shared.Flows;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.Signals;
using Wooga.UnityFramework;

namespace Match3.Scripts1.Puzzletown.Flows
{
	// Token: 0x020009D0 RID: 2512
	public class M3_LossAversionFlow : AFlowR<ScoringController, bool>
	{
		// Token: 0x06003CDA RID: 15578 RVA: 0x00131854 File Offset: 0x0012FC54
		protected override IEnumerator FlowRoutine(ScoringController input)
		{
			bool hasTournamentRewards = input.GetTournamentScore().CollectedPoints > 0;
			bool hasCollectable = !string.IsNullOrEmpty(input.GetCollectable());
			bool isDiveForTreasure = input.GetLevelPlayMode() == LevelPlayMode.DiveForTreasure;
			if (!hasTournamentRewards && !hasCollectable && !isDiveForTreasure)
			{
				yield return true;
				yield break;
			}
			yield return ServiceLocator.Instance.Inject(this);
			Wooroutine<M3_DontGiveUpRoot> lossAversionPopupLoading = SceneManager.Instance.LoadSceneWithParams<M3_DontGiveUpRoot, ScoringController>(input, null);
			yield return lossAversionPopupLoading;
			M3_DontGiveUpRoot popup = lossAversionPopupLoading.ReturnValue;
			if (input.GetTournamentScore().Multiplier > 1)
			{
				string substring = string.Format("_x{0}", input.GetTournamentScore().Multiplier);
				popup.bonusMultiplier.image.sprite = popup.bonusMultiplier.manager.GetSimilar(substring);
			}
			popup.Show();
			AwaitSignal<bool> result = popup.onCompleted;
			yield return result;
			yield return result.Dispatched;
			yield break;
		}
	}
}
