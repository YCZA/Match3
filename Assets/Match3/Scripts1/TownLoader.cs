using System.Collections.Generic;
using Match3.Scripts1.Town;
using UnityEngine;

// Token: 0x02000951 RID: 2385
namespace Match3.Scripts1
{
	internal static class TownLoader
	{
		// Token: 0x060039FE RID: 14846 RVA: 0x0011E118 File Offset: 0x0011C518
		public static void LoadTownData(BuildingServices services, IEnumerable<BuildingInstance.PersistentData> buildings, int countToLoadWithoutSlicing = -1)
		{
			int num = 0;
			foreach (BuildingInstance.PersistentData persistentData in buildings)
			{
				// Debug.LogError(persistentData.position + " " + persistentData.blueprintName);
				if (services.BuildingConfigs.GetConfig(persistentData.blueprintName) != null)
				{
					bool instantiateWithTimeSlicing = num++ > countToLoadWithoutSlicing;
					TownLoader.Instantiate(persistentData, services.BuildingsController, services.BuildingConfigs, instantiateWithTimeSlicing);
				}
			}
		}

		// Token: 0x060039FF RID: 14847 RVA: 0x0011E1A0 File Offset: 0x0011C5A0
		public static BuildingInstance Instantiate(BuildingInstance.PersistentData data, BuildingsController buildingsController, BuildingConfigList buildingConfigs, bool instantiateWithTimeSlicing = false)
		{
			BuildingConfig config = buildingConfigs.GetConfig(data.blueprintName);
			BuildingInstance buildingInstance = buildingsController.CreateBuildingFromBlueprint(config, data, false, instantiateWithTimeSlicing);
			TownLoader.DisableColliderOnRubble(buildingInstance);
			return buildingInstance;
		}

		// Token: 0x06003A00 RID: 14848 RVA: 0x0011E1D0 File Offset: 0x0011C5D0
		private static void DisableColliderOnRubble(BuildingInstance building)
		{
			if (building != null && building.blueprint != null && building.blueprint.IsRubble() && building.view != null)
			{
				Collider component = building.view.GetComponent<Collider>();
				if (component != null)
				{
					component.enabled = false;
				}
			}
		}
	}
}
