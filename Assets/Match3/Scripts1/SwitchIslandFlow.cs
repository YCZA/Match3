using System.Collections;
using Match3.Scripts1.Puzzletown;
using Match3.Scripts1.Puzzletown.Flows;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Puzzletown.Town;
using Match3.Scripts1.Shared.Flows;
using Wooga.Coroutines;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;

// Token: 0x020004CB RID: 1227
namespace Match3.Scripts1
{
	public class SwitchIslandFlow : AFlow<SwitchIslandFlow.SwitchIslandFlowData>
	{
		// Token: 0x06002261 RID: 8801 RVA: 0x0009799C File Offset: 0x00095D9C
		protected override IEnumerator FlowRoutine(SwitchIslandFlow.SwitchIslandFlowData islandData)
		{
			yield return ServiceLocator.Instance.Inject(this);
			if (this.gameState.Buildings.CurrentIsland != islandData.islandId)
			{
				bool shouldShowNewBanner = BannerNewAreaRoot.ShouldShow(this.questService, this.progressionService);
				this.gameState.Buildings.CurrentIsland = islandData.islandId;
				TownLoadingFlowWithTransition.Input input = new TownLoadingFlowWithTransition.Input(LoadingScreenConfig.Random);
				Wooroutine<TownMainRoot> town = new TownLoadingFlowWithTransition().Start(input);
				yield return town;
				town.ReturnValue.StartView(islandData.tryStartNewContent, shouldShowNewBanner);
			}
			yield break;
		}

		// Token: 0x04004DC9 RID: 19913
		[WaitForService(true, true)]
		private GameStateService gameState;

		// Token: 0x04004DCA RID: 19914
		[WaitForService(true, true)]
		private QuestService questService;

		// Token: 0x04004DCB RID: 19915
		[WaitForService(true, true)]
		private ProgressionDataService.Service progressionService;

		// Token: 0x020004CC RID: 1228
		public class SwitchIslandFlowData
		{
			// Token: 0x06002262 RID: 8802 RVA: 0x000979BE File Offset: 0x00095DBE
			public SwitchIslandFlowData(int islandId, bool tryStartNewContent = false)
			{
				this.islandId = islandId;
				this.tryStartNewContent = tryStartNewContent;
			}

			// Token: 0x04004DCC RID: 19916
			public int islandId;

			// Token: 0x04004DCD RID: 19917
			public bool tryStartNewContent;
		}
	}
}
