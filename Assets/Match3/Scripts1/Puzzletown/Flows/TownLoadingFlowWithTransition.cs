using System.Collections;
using Match3.Scripts1.Puzzletown.Town;
using Match3.Scripts1.Shared.Flows;
using Wooga.Coroutines;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;

namespace Match3.Scripts1.Puzzletown.Flows
{
	// Token: 0x020004CD RID: 1229
	public class TownLoadingFlowWithTransition : AFlowR<TownLoadingFlowWithTransition.Input, TownMainRoot>
	// public class TownLoadingFlowWithTransition : AFlowR<TownLoadingFlowWithTransition.Input, GameLauncherRoot>
	{
		// eli key point 关卡结束后退出场景
		// eli todo 不要调用Level模块外面的东西
		// Token: 0x06002264 RID: 8804 RVA: 0x00097B3C File Offset: 0x00095F3C
		protected override IEnumerator FlowRoutine(TownLoadingFlowWithTransition.Input input)
		{
			yield return LoadingScreenRoot.PrepareAnimatedLoadingScreen(input.loadingScreenConfig, true);
			yield return ServiceLocator.Instance.Inject(this);
			Wooroutine<TownMainRoot> townScene = TownMainRoot.Load(this.state.Buildings.CurrentIsland, null);
			townScene.ShowLoadingScreen();
			yield return townScene;
			yield return townScene.ReturnValue;
			yield break;
		}

		// Token: 0x04004DCE RID: 19918
		[WaitForService(true, true)]
		private GameStateService state;

		// Token: 0x020004CE RID: 1230
		public struct Input
		{
			// Token: 0x06002265 RID: 8805 RVA: 0x00097B5E File Offset: 0x00095F5E
			public Input(LoadingScreenConfig loadingScreenConfig)
			{
				this.loadingScreenConfig = loadingScreenConfig;
			}

			// Token: 0x04004DCF RID: 19919
			public readonly LoadingScreenConfig loadingScreenConfig;
		}
	}
}
