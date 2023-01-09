using System;
using System.Collections;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Puzzletown.Town;
using Match3.Scripts1.Shared.Flows;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;

namespace Match3.Scripts1.Puzzletown.Flows
{
	// Token: 0x020009CF RID: 2511
	public class ForceUserPlaceDecoFlow : AFlow, IBlocker
	{
		// Token: 0x06003CCF RID: 15567 RVA: 0x0013137E File Offset: 0x0012F77E
		public ForceUserPlaceDecoFlow(BuildingConfig config)
		{
			this.buildingConfig = config;
		}

		// Token: 0x06003CD0 RID: 15568 RVA: 0x00131390 File Offset: 0x0012F790
		public IEnumerator ExecuteRoutine()
		{
			yield return base.Start();
			yield break;
		}

		// Token: 0x17000915 RID: 2325
		// (get) Token: 0x06003CD1 RID: 15569 RVA: 0x001313AB File Offset: 0x0012F7AB
		public bool BlockInput
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06003CD2 RID: 15570 RVA: 0x001313B0 File Offset: 0x0012F7B0
		protected override IEnumerator FlowRoutine()
		{
			if (this.buildingConfig == null)
			{
				yield break;
			}
			if (ForceUserPlaceDecoFlow.flowRunning)
			{
				yield break;
			}
			ForceUserPlaceDecoFlow.flowRunning = true;
			yield return ServiceLocator.Instance.Inject(this);
			yield return SceneManager.Instance.Inject(this);
			this.StartSessionLifeCycle();
			yield return this.ForceUserPlaceDecoRoutine(this.buildingConfig);
			this.EndSessionLifeCycle();
			yield break;
		}

		// Token: 0x06003CD3 RID: 15571 RVA: 0x001313CC File Offset: 0x0012F7CC
		private IEnumerator ForceUserPlaceDecoRoutine(BuildingConfig config)
		{
			TownSceneLoader townSceneLoader = this.townMainRoot.townLoader;
			if (townSceneLoader != null)
			{
				BuildingShopView.BuildingBuildRequest buildingRequest = this.CreateBuildRequest(config);
				if (buildingRequest.Config != null && townSceneLoader.TryPlaceBuilding(buildingRequest, false) && BuildingLocation.Selected != null)
				{
					BuildingLocation.Selected.mustBePlaced = true;
					BuildingLocation.Selected.cancelBatchMode = true;
					BuildingLocation.Selected.view.controlButtons.Show(BuildingLocation.Selected, BuildingOperation.Confirm);
					yield return this.WaitForBuildingPlacementRoutine();
				}
			}
			yield break;
		}

		// Token: 0x06003CD4 RID: 15572 RVA: 0x001313F0 File Offset: 0x0012F7F0
		private IEnumerator WaitForBuildingPlacementRoutine()
		{
			while (BuildingLocation.Selected != null)
			{
				yield return null;
			}
			if (this.buildingConfig != null)
			{
				this.gameStateService.Buildings.StoreBuilding(this.buildingConfig, -1);
			}
			yield break;
		}

		// Token: 0x06003CD5 RID: 15573 RVA: 0x0013140C File Offset: 0x0012F80C
		private BuildingShopView.BuildingBuildRequest CreateBuildRequest(BuildingConfig config)
		{
			BuildingShopView.BuildingBuildRequest result = default(BuildingShopView.BuildingBuildRequest);
			float zoomInRatio = BuildingShopView.BuildingBuildRequest.CalculateZoomFactorBasedOnSize(config);
			result.isFree = true;
			result.Config = config;
			result.shouldZoomIn = true;
			result.zoomInRatio = zoomInRatio;
			result.shouldPanCamera = true;
			return result;
		}

		// Token: 0x06003CD6 RID: 15574 RVA: 0x00131451 File Offset: 0x0012F851
		private void StartSessionLifeCycle()
		{
			this.sessionService.onRestart.AddListenerOnce(new Action(this.HandleSessionOver));
		}

		// Token: 0x06003CD7 RID: 15575 RVA: 0x0013146F File Offset: 0x0012F86F
		private void EndSessionLifeCycle()
		{
			ForceUserPlaceDecoFlow.flowRunning = false;
			this.sessionService.onRestart.RemoveListener(new Action(this.HandleSessionOver));
		}

		// Token: 0x06003CD8 RID: 15576 RVA: 0x00131493 File Offset: 0x0012F893
		private void HandleSessionOver()
		{
			ForceUserPlaceDecoFlow.flowRunning = false;
		}

		// Token: 0x0400658A RID: 25994
		private static bool flowRunning;

		// Token: 0x0400658B RID: 25995
		[WaitForService(true, true)]
		private ConfigService configService;

		// Token: 0x0400658C RID: 25996
		[WaitForService(true, true)]
		private SessionService sessionService;

		// Token: 0x0400658D RID: 25997
		[WaitForService(true, true)]
		private GameStateService gameStateService;

		// Token: 0x0400658E RID: 25998
		[WaitForService(true, true)]
		private TrackingService trackingService;

		// Token: 0x0400658F RID: 25999
		[WaitForRoot(false, false)]
		private TownMainRoot townMainRoot;

		// Token: 0x04006590 RID: 26000
		private BuildingConfig buildingConfig;
	}
}
