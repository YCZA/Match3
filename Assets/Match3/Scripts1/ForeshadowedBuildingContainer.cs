using System;
using System.Collections.Generic;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Puzzletown.Town;
using Match3.Scripts1.Town;
using UnityEngine;

// Token: 0x0200091C RID: 2332
namespace Match3.Scripts1
{
	public class ForeshadowedBuildingContainer : MonoBehaviour
	{
		// Token: 0x170008C0 RID: 2240
		// (get) Token: 0x060038DA RID: 14554 RVA: 0x00117EF7 File Offset: 0x001162F7
		public bool IsInitialized
		{
			get
			{
				return this.configService != null && this.buildingServices != null;
			}
		}

		// Token: 0x170008C1 RID: 2241
		// (get) Token: 0x060038DB RID: 14555 RVA: 0x00117F13 File Offset: 0x00116313
		private IEnumerable<StartBuilding> StartBuildings
		{
			get
			{
				return this.configService.startBuilding.GetAllOnIsland(this.buildingServices.GameStateService.Buildings.CurrentIsland);
			}
		}

		// Token: 0x060038DC RID: 14556 RVA: 0x00117F3A File Offset: 0x0011633A
		private void OnDestroy()
		{
			this.configService = null;
			this.buildingServices = null;
		}

		// Token: 0x060038DD RID: 14557 RVA: 0x00117F4A File Offset: 0x0011634A
		public void Init(ConfigService configService, BuildingServices buildingServices)
		{
			this.configService = configService;
			this.buildingServices = buildingServices;
		}

		// Token: 0x060038DE RID: 14558 RVA: 0x00117F5C File Offset: 0x0011635C
		public void RefreshForeshadowing(int lastUnlockedArea, int buildingsToLoadWithoutSlicingCount = 0)
		{
			if (!this.IsInitialized)
			{
				return;
			}
			IEnumerable<StartBuilding> startBuildings = this.StartBuildings;
			foreach (StartBuilding building in startBuildings)
			{
				bool createUsingSlicing = buildingsToLoadWithoutSlicingCount <= 0;
				if (this.TryCreateForeshadowedBuilding(building, lastUnlockedArea, createUsingSlicing))
				{
					buildingsToLoadWithoutSlicingCount--;
				}
			}
			this.buildingServices.BuildingsController.RefreshMap();
		}

		// Token: 0x060038DF RID: 14559 RVA: 0x00117FE8 File Offset: 0x001163E8
		public int CountForeshadowedBuildingsToCreate(int lastUnlockedArea)
		{
			return this.StartBuildings.CountIf((StartBuilding building) => building.area > lastUnlockedArea);
		}

		// Token: 0x060038E0 RID: 14560 RVA: 0x0011801C File Offset: 0x0011641C
		private bool TryCreateForeshadowedBuilding(StartBuilding building, int lastUnlockedArea, bool createUsingSlicing = true)
		{
			BuildingMainView buildingMainView = this.FindForeshadowedBuilding(building);
			if (!TownCheatsRoot.EditMode && building.area <= lastUnlockedArea)
			{
				if (buildingMainView)
				{
					this.buildingServices.BuildingsController.DestroyBuilding(buildingMainView.Data);
					global::UnityEngine.Object.Destroy(buildingMainView.gameObject);
				}
				return false;
			}
			if (buildingMainView)
			{
				return false;
			}
			BuildingConfig config = this.buildingServices.BuildingConfigs.GetConfig(building.building_id);
			if (config == null)
			{
				WoogaDebug.LogWarning(new object[]
				{
					"No Config found!",
					building.building_id
				});
				return false;
			}
			BuildingInstance.PersistentData persistentData = new BuildingInstance.PersistentData
			{
				blueprintName = building.building_id,
				position = new IntVector2(building.pos_x, building.pos_y),
				ownerArea = building.area,
				DecoSet = building.deco_set
			};
			if (!building.destroyed)
			{
				persistentData.SetTimer(BuildingTimer.Repair, BuildingInstance.UnixStamp);
			}
			if (TownCheatsRoot.EditMode)
			{
				persistentData.SetTimer(BuildingTimer.Repair, BuildingInstance.UnixStamp);
				persistentData.SetTimer(BuildingTimer.Placed, BuildingInstance.UnixStamp);
			}
			BuildingInstance buildingInstance = this.buildingServices.BuildingsController.CreateBuildingFromBlueprint(config, persistentData, true, createUsingSlicing);
			BuildingMainView view = buildingInstance.view;
			view.transform.SetParent(base.transform, false);
			if (TownCheatsRoot.EditMode)
			{
				this.buildingServices.BuildingsController.AddBuildingToGamestate(buildingInstance);
				return false;
			}
			view.GetComponent<Collider>().enabled = false;
			view.GetComponent<BuildingAreaDisplay>().display.gameObject.SetActive(false);
			return true;
		}

		// Token: 0x060038E1 RID: 14561 RVA: 0x001181B0 File Offset: 0x001165B0
		private BuildingMainView FindForeshadowedBuilding(StartBuilding building)
		{
			return Array.Find<BuildingMainView>(base.GetComponentsInChildren<BuildingMainView>(), (BuildingMainView view) => building.IsMatching(view.Data.sv));
		}

		// Token: 0x04006134 RID: 24884
		private ConfigService configService;

		// Token: 0x04006135 RID: 24885
		private BuildingServices buildingServices;
	}
}
