using System;
using System.Collections.Generic;
using System.Linq;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Shared.DataStructures;
using Match3.Scripts1.Town;
using Match3.Scripts1.Wooga.Signals;
using UnityEngine;

// Token: 0x020008F9 RID: 2297
namespace Match3.Scripts1
{
	public class BuildingsController
	{
		// Token: 0x060037ED RID: 14317 RVA: 0x00111A44 File Offset: 0x0010FE44
		public BuildingsController(BuildingDataService iDataService)
		{
			this.dataService = iDataService;
			this.map = new Map<BuildingInstance>(128);
		}

		// Token: 0x17000897 RID: 2199
		// (get) Token: 0x060037EE RID: 14318 RVA: 0x00111ADC File Offset: 0x0010FEDC
		public IEnumerable<BuildingInstance> Buildings
		{
			get
			{
				return this._buildingInstances;
			}
		}

		// Token: 0x060037EF RID: 14319 RVA: 0x00111AE4 File Offset: 0x0010FEE4
		public IEnumerable<BuildingInstance> BuildingsByDecoTag(string deco_tag)
		{
			return from b in this._buildingInstances
				where b.sv.DecoSet == deco_tag
				select b;
		}

		// Token: 0x060037F0 RID: 14320 RVA: 0x00111B18 File Offset: 0x0010FF18
		public BuildingInstance CreateBuildingFromBlueprint(BuildingConfig blueprint, BuildingInstance.PersistentData data, bool isForeshadowing = false, bool createUsingTimeSlicing = true)
		{
			BuildingInstance buildingInstance = (data != null) ? new BuildingInstance(blueprint, data) : new BuildingInstance(blueprint);
			buildingInstance.controller = this;
			buildingInstance.isForeshadowing = isForeshadowing;
			buildingInstance.instantiateWithTimeSlicing = createUsingTimeSlicing;
			buildingInstance.onHarvest.AddListener(new Action<BuildingInstance, MaterialAmount>(this.onBuildingHarvest.Dispatch));
			// Debug.LogError("AddBuilding:" + buildingInstance.blueprint.name);
			this._buildingInstances.Add(buildingInstance);
			this.onBuildingCreated.Dispatch(buildingInstance);
			return buildingInstance;
		}

		// Token: 0x060037F1 RID: 14321 RVA: 0x00111B8A File Offset: 0x0010FF8A
		public void DestroyBuilding(BuildingInstance oldBuildingInstance)
		{
			this.DestroyBuildingInstance(oldBuildingInstance);
			this._buildingInstances.Remove(oldBuildingInstance);
		}

		// Token: 0x060037F2 RID: 14322 RVA: 0x00111BA0 File Offset: 0x0010FFA0
		public void AddBuildingToGamestate(BuildingInstance buildingInstance)
		{
			this.dataService.RegisterBuilding(buildingInstance);
		}

		// Token: 0x060037F3 RID: 14323 RVA: 0x00111BB0 File Offset: 0x0010FFB0
		public void RefreshMap()
		{
			// Debug.LogError("RefreshMap!!!!!!!!!!!!!!" + Buildings.Count());
			this.map.Clear();
			foreach (BuildingInstance buildingInstance in this.Buildings)
			{
				BuildingConfig blueprint = buildingInstance.blueprint;
				int x = buildingInstance.position.x;
				int y = buildingInstance.position.y;
				// rubble不添加
				// Debug.LogError("RefreshMap!!!!!!!!!!!!!!@@" + buildingInstance.blueprint.name);
				if (!blueprint.IsRubble())
				{
					for (int i = 0; i < blueprint.size; i++)
					{
						for (int j = 0; j < blueprint.size; j++)
						{
							this.map[TownPathfinding.ConvertCoordinates(i + x, j + y)] = buildingInstance;
						}
					}
				}
			}
		}

		// Token: 0x060037F4 RID: 14324 RVA: 0x00111C98 File Offset: 0x00110098
		public int GetStoredBuildingsCount(BuildingConfig blueprint)
		{
			return this.dataService.GetStoredBuildingsCount(blueprint);
		}

		// Token: 0x060037F5 RID: 14325 RVA: 0x00111CA6 File Offset: 0x001100A6
		public void StoreBuilding(BuildingConfig blueprint, int amount = 1)
		{
			this.dataService.StoreBuilding(blueprint, amount);
		}

		// Token: 0x060037F6 RID: 14326 RVA: 0x00111CB5 File Offset: 0x001100B5
		public BuildingInstance BuildingAt(IntVector2 position)
		{
			return this.BuildingAt(position.x, position.y);
		}

		// Token: 0x060037F7 RID: 14327 RVA: 0x00111CCB File Offset: 0x001100CB
		private BuildingInstance BuildingAt(int x, int y)
		{
			return this.map[TownPathfinding.ConvertCoordinates(x, y)];
		}

		// Token: 0x060037F8 RID: 14328 RVA: 0x00111CDF File Offset: 0x001100DF
		private void DestroyBuildingInstance(BuildingInstance oldBuildingInstance)
		{
			if (oldBuildingInstance == null)
			{
				throw new InvalidOperationException("Cannot destroy a null BuildingInstance");
			}
			this.onBuildingDestroyed.Dispatch(oldBuildingInstance);
		}

		// Token: 0x04006015 RID: 24597
		public readonly Signal<BuildingInstance> onBuildingRepaired = new Signal<BuildingInstance>();

		// Token: 0x04006016 RID: 24598
		public readonly Signal<BuildingInstance> onBuildingCreated = new Signal<BuildingInstance>();

		// Token: 0x04006017 RID: 24599
		public readonly Signal<BuildingInstance> onBuildingDestroyed = new Signal<BuildingInstance>();

		// Token: 0x04006018 RID: 24600
		public readonly Signal<BuildingConfig, Vector3> onBuildingStored = new Signal<BuildingConfig, Vector3>();

		// Token: 0x04006019 RID: 24601
		public readonly Signal onPurchaseCancelled = new Signal();

		// Token: 0x0400601A RID: 24602
		public readonly Signal<BuildingInstance> onBuildingComplete = new Signal<BuildingInstance>();

		// Token: 0x0400601B RID: 24603
		public readonly Signal<BuildingInstance, MaterialAmount> onBuildingHarvest = new Signal<BuildingInstance, MaterialAmount>();

		// Token: 0x0400601C RID: 24604
		public readonly Signal<BuildingInstance, bool> onBuildingSelected = new Signal<BuildingInstance, bool>();

		// Token: 0x0400601D RID: 24605
		public readonly Signal onBuildingsRefreshed = new Signal();

		// Token: 0x0400601E RID: 24606
		private readonly BuildingDataService dataService;

		// Token: 0x0400601F RID: 24607
		private readonly Map<BuildingInstance> map;

		// Token: 0x04006020 RID: 24608
		private readonly HashSet<BuildingInstance> _buildingInstances = new HashSet<BuildingInstance>();
	}
}
