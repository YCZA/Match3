using System.Collections;
using Match3.Scripts1.Puzzletown.UI;
using Match3.Scripts1.Shared.Flows;
using Wooga.Coroutines;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;

namespace Match3.Scripts1.Puzzletown.Flows
{
	// Token: 0x020004BC RID: 1212
	public class LoadDiveForTreasureFlow : AFlow
	{
		// Token: 0x06002218 RID: 8728 RVA: 0x000938A8 File Offset: 0x00091CA8
		protected override IEnumerator FlowRoutine()
		{
			yield return LoadingScreenRoot.PrepareAnimatedLoadingScreen(LoadingScreenConfig.Random, true);
			yield return ServiceLocator.Instance.Inject(this);
			Wooroutine<DiveForTreasureRoot> diveRoot = SceneManager.Instance.LoadScene<DiveForTreasureRoot>(null);
			yield return diveRoot;
			yield return diveRoot.ReturnValue.OnInitialized;
			yield return LoadingScreenRoot.Hide();
			yield break;
		}

		// Token: 0x04004D6B RID: 19819
		[WaitForService(true, true)]
		private GameStateService state;
	}
}
