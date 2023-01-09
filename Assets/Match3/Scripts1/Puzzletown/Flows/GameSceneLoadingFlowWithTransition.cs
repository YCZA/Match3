using System.Collections;
using Match3.Scripts1.Puzzletown.Match3;
using Match3.Scripts1.Shared.Flows;
using Wooga.Coroutines;
using Wooga.UnityFramework;

namespace Match3.Scripts1.Puzzletown.Flows
{
	// Token: 0x020004B7 RID: 1207
	public class GameSceneLoadingFlowWithTransition : AFlowR<GameSceneLoadingFlowWithTransition.Input, Wooroutine<M3_LevelRoot>>
	{
		// Token: 0x06002205 RID: 8709 RVA: 0x00091DB4 File Offset: 0x000901B4
		protected override IEnumerator FlowRoutine(GameSceneLoadingFlowWithTransition.Input input)
		{
			yield return LoadingScreenRoot.PrepareAnimatedLoadingScreen(input.loadingScreenConfig, false);
			Wooroutine<M3_LevelRoot> gameSceneLoad = SceneManager.Instance.LoadSceneWithParams<M3_LevelRoot, LevelRootInput>(input.levelRootInput, null);
			gameSceneLoad.ShowLoadingScreen();
			yield return gameSceneLoad;
			M3_LevelRoot m3core = gameSceneLoad.ReturnValue;
			yield return m3core.onBoardSetup.Await();
			yield return WooroutineRunner.StartCoroutine(LoadingScreenRoot.Hide(), null);
			yield return gameSceneLoad;
			yield break;
		}

		// Token: 0x020004B8 RID: 1208
		public struct Input
		{
			// Token: 0x06002206 RID: 8710 RVA: 0x00091DCF File Offset: 0x000901CF
			public Input(LevelRootInput levelRootInput, LoadingScreenConfig loadingScreenConfig)
			{
				this.levelRootInput = levelRootInput;
				this.loadingScreenConfig = loadingScreenConfig;
			}

			// Token: 0x04004D5A RID: 19802
			public readonly LevelRootInput levelRootInput;

			// Token: 0x04004D5B RID: 19803
			public readonly LoadingScreenConfig loadingScreenConfig;
		}
	}
}
