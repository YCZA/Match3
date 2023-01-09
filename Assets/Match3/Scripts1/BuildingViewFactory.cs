using System;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Puzzletown.Town;
using Match3.Scripts1.Town;
using UnityEngine;

// Token: 0x020008FB RID: 2299
namespace Match3.Scripts1
{
	public class BuildingViewFactory : MonoBehaviour
	{
		// Token: 0x06003800 RID: 14336 RVA: 0x00111D9A File Offset: 0x0011019A
		private void OnDestroy()
		{
			if (BuildingAssetLoader.timeSlicer != null)
			{
				BuildingAssetLoader.timeSlicer.Stop();
			}
		}

		// Token: 0x06003801 RID: 14337 RVA: 0x00111DB0 File Offset: 0x001101B0
		public void Init(BuildingServices services, ConfigService configService, TownOverheadUiRoot buildingsUi, TownBottomPanelRoot bottomPanelRoot)
		{
			this.buildingsParent = base.transform.CreateChild("Buildings");
			this.buildingResourceService = services.BuildingResourceService;
			services.BuildingsController.onBuildingCreated.AddListener(new Action<BuildingInstance>(this.CreateView));
			services.BuildingsController.onBuildingDestroyed.AddListener(new Action<BuildingInstance>(this.DestroyView));
			services.BuildingsController.Buildings.ForEach(new Action<BuildingInstance>(this.CreateView));
			this.config = configService;
			this.uiRoot = buildingsUi;
			this.bottomPanel = bottomPanelRoot;
		}

		// eli key point: 生成家园建筑
		public void CreateView(BuildingInstance newBuilding)
		{
			BuildingMainView buildingMainView = global::UnityEngine.Object.Instantiate<BuildingMainView>(this.buildingPrefab);
			buildingMainView.Init(this.config, this.uiRoot, this.bottomPanel);
			BuildingAssetLoader assetLoader = buildingMainView.assetLoader;
			if (assetLoader != null)
			{
				assetLoader.SetupServices(this.buildingResourceService);
			}
			buildingMainView.LoadAsset(newBuilding);
			buildingMainView.Show(newBuilding);
			buildingMainView.transform.parent = this.buildingsParent;
			newBuilding.view = buildingMainView;
		}

		// Token: 0x06003803 RID: 14339 RVA: 0x00111EC4 File Offset: 0x001102C4
		public void DestroyView(BuildingInstance oldBuilding)
		{
			BuildingMainView view = oldBuilding.view;
			if (view != null)
			{
				global::UnityEngine.Object.Destroy(view.gameObject);
			}
		}

		// Token: 0x04006025 RID: 24613
		public BuildingMainView buildingPrefab;

		// Token: 0x04006026 RID: 24614
		public BuildingResourceServiceRoot buildingResourceService;

		// Token: 0x04006027 RID: 24615
		private Transform buildingsParent;

		// Token: 0x04006028 RID: 24616
		private TownOverheadUiRoot uiRoot;

		// Token: 0x04006029 RID: 24617
		private TownBottomPanelRoot bottomPanel;

		// Token: 0x0400602A RID: 24618
		private ConfigService config;
	}
}
