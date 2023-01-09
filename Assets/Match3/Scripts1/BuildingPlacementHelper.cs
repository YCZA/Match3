using Match3.Scripts1.Town;
using UnityEngine;

// Token: 0x020008F0 RID: 2288
namespace Match3.Scripts1
{
	public class BuildingPlacementHelper
	{
		// Token: 0x060037A4 RID: 14244 RVA: 0x0010F47C File Offset: 0x0010D87C
		public static IntVector2 FindEmptySpotFor(BuildingInstance building, IntVector2 wp, int turns = 100, int steps = 90)
		{
			IntVector2 b = IntVector2.Zero;
			for (int i = 0; i < turns * steps; i++)
			{
				float num = (float)i / (float)steps;
				float num2 = 6.2831855f * num - 1.5707964f;
				IntVector2 intVector = wp + new IntVector2(Mathf.FloorToInt(Mathf.Cos(num2) * num), Mathf.FloorToInt(Mathf.Sin(num2) * num));
				if (!(intVector == b))
				{
					if (BuildingLocation.IsValidPlacement(intVector, building))
					{
						global::UnityEngine.Debug.LogFormat("Found at {0} step, rads {1}, progress {2}", new object[]
						{
							i,
							num2,
							num
						});
						return intVector;
					}
					b = intVector;
				}
			}
			global::UnityEngine.Debug.LogWarningFormat("Could not find a spot for {0}", new object[]
			{
				building.blueprint.name
			});
			return wp;
		}

		// Token: 0x060037A5 RID: 14245 RVA: 0x0010F550 File Offset: 0x0010D950
		public static bool TryFindPlaceForBuilding(BuildingShopView.BuildingBuildRequest request, TownPathfinding map, out IntVector2 buildingLocation)
		{
			buildingLocation = new IntVector2(-1, -1);
			IntVector2 intVector = IntVector2.ProjectToGridXZ(CameraInputController.CameraPosition);
			return map.FindBuildLocation(new IntVector2(intVector.x - request.Config.size, intVector.y - request.Config.size), request.Config, out buildingLocation);
		}
	}
}
