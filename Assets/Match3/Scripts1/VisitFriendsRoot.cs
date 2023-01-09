using System.Collections;
using Match3.Scripts1.Puzzletown;
using Match3.Scripts1.Puzzletown.Flows;
using Match3.Scripts1.Puzzletown.Town;
using Wooga.Coroutines;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;

// Token: 0x02000A69 RID: 2665
namespace Match3.Scripts1
{
	public class VisitFriendsRoot : APtSceneRoot, IHandler<PopupOperation>
	{
		// Token: 0x06003FD5 RID: 16341 RVA: 0x00147131 File Offset: 0x00145531
		public void Handle(PopupOperation op)
		{
			if (op == PopupOperation.Back)
			{
				this.ReloadTownWithAnimatedLoadingScreen();
			}
		}

		// Token: 0x06003FD6 RID: 16342 RVA: 0x0014714A File Offset: 0x0014554A
		public void ReloadTownWithAnimatedLoadingScreen()
		{
			if (!this.returningHome)
			{
				this.returningHome = true;
				this.gameState.RestoreGameState();
				WooroutineRunner.StartCoroutine(this.DoReloadTown(), null);
			}
		}

		// Token: 0x06003FD7 RID: 16343 RVA: 0x00147178 File Offset: 0x00145578
		private IEnumerator DoReloadTown()
		{
			TownLoadingFlowWithTransition.Input loadingScreenConfig = this.ConfigureLoadingScreen();
			Wooroutine<TownMainRoot> townLoadingFlow = new TownLoadingFlowWithTransition().Start(loadingScreenConfig);
			yield return townLoadingFlow;
			TownMainRoot townScene = townLoadingFlow.ReturnValue;
			townScene.StartView(true, false);
			yield break;
		}

		// Token: 0x06003FD8 RID: 16344 RVA: 0x00147193 File Offset: 0x00145593
		private TownLoadingFlowWithTransition.Input ConfigureLoadingScreen()
		{
			return new TownLoadingFlowWithTransition.Input(LoadingScreenConfig.Random);
		}

		// Token: 0x04006978 RID: 27000
		[WaitForService(true, true)]
		private GameStateService gameState;

		// Token: 0x04006979 RID: 27001
		private bool returningHome;
	}
}
