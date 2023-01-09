using System.Collections;
using Match3.Scripts1.Puzzletown.Town;
using Match3.Scripts1.Shared.Flows;
using Wooga.UnityFramework;
using Match3.Scripts2.Shop;
using UnityEngine;

// Token: 0x0200089E RID: 2206
namespace Match3.Scripts1
{
	public class GetLivesJourneyTown : AFlow
	{
		// Token: 0x06003600 RID: 13824 RVA: 0x00104D74 File Offset: 0x00103174
		protected override IEnumerator FlowRoutine()
		{
			yield return SceneManager.Instance.Inject(this);
			Coroutine livesJourney = new GetLivesJourney("lives_hud", new TownDiamondsPanelRoot.TownDiamondsPanelRootParameters
			{
				source1 = "out_of_lives",
				source2 = "lives_hud"
			}).Start();
			yield return livesJourney;
			yield break;
		}

		// Token: 0x04005DFE RID: 24062
		[WaitForRoot(false, false)]
		private TownUiRoot uiRoot;
	}
}
