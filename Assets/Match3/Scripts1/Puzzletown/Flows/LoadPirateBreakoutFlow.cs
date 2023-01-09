using System.Collections;
using Match3.Scripts1.Puzzletown.Match3.Scoring;
using Match3.Scripts1.Puzzletown.UI;
using Match3.Scripts1.Shared.Flows;
using Wooga.Coroutines;
using Wooga.UnityFramework;

namespace Match3.Scripts1.Puzzletown.Flows
{
	// Token: 0x020004BD RID: 1213
	public class LoadPirateBreakoutFlow : AFlow
	{
		// Token: 0x06002219 RID: 8729 RVA: 0x00093A00 File Offset: 0x00091E00
		public LoadPirateBreakoutFlow(Match3Score score = null)
		{
			this.score = score;
		}

		// Token: 0x0600221A RID: 8730 RVA: 0x00093A10 File Offset: 0x00091E10
		protected override IEnumerator FlowRoutine()
		{
			yield return LoadingScreenRoot.PrepareAnimatedLoadingScreen(LoadingScreenConfig.Random, true);
			yield return ServiceLocator.Instance.Inject(this);
			Wooroutine<PirateBreakoutRoot> pirateBreakoutRoot = SceneManager.Instance.LoadSceneWithParams<PirateBreakoutRoot, Match3Score>(this.score, null);
			yield return pirateBreakoutRoot;
			yield return pirateBreakoutRoot.ReturnValue.OnInitialized;
			yield return LoadingScreenRoot.Hide();
			yield break;
		}

		// Token: 0x04004D6C RID: 19820
		private Match3Score score;
	}
}
