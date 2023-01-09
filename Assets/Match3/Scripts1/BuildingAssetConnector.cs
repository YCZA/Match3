using System.Collections;
using Match3.Scripts1.Town;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.UI;
using UnityEngine;

// Token: 0x02000959 RID: 2393
namespace Match3.Scripts1
{
	public class BuildingAssetConnector : ABuildingAssetSwitcher, IDataView<BuildingInstance>, IBuildingOnPositionChanged
	{
		// Token: 0x06003A5A RID: 14938 RVA: 0x00120BA2 File Offset: 0x0011EFA2
		public void Show(BuildingInstance building)
		{
			this.building = building;
			WooroutineRunner.StartCoroutine(this.RefreshCoroutine(building), null);
		}

		// Token: 0x06003A5B RID: 14939 RVA: 0x00120BBC File Offset: 0x0011EFBC
		private IEnumerator RefreshCoroutine(BuildingInstance building)
		{
			// Debug.LogError("RefreshCoroutine");
			yield return new WaitForEndOfFrame();
			base.SwitchAsset(this.GetConnectionConfiguration(building), building);
			yield break;
		}

		// Token: 0x06003A5C RID: 14940 RVA: 0x00120BDE File Offset: 0x0011EFDE
		public void HandlePositionChanged(BuildingInstance building)
		{
			BuildingAssetConnector.UpdateAllConfigurations();
		}

		// Token: 0x06003A5D RID: 14941 RVA: 0x00120BE8 File Offset: 0x0011EFE8
		public static void UpdateAllConfigurations()
		{
			Debug.Log("UpdateAllConfigurations");
			foreach (BuildingAssetConnector buildingAssetConnector in global::UnityEngine.Object.FindObjectsOfType<BuildingAssetConnector>())
			{
				buildingAssetConnector.UpdateConfiguration();
			}
		}

		// Token: 0x06003A5E RID: 14942 RVA: 0x00120C19 File Offset: 0x0011F019
		private void UpdateConfiguration()
		{
			base.SwitchAsset(this.GetConnectionConfiguration(this.building), this.building);
		}

		// Token: 0x06003A5F RID: 14943 RVA: 0x00120C34 File Offset: 0x0011F034
		private int GetConnectionConfiguration(BuildingInstance building)
		{
			int num = 0;
			int size = building.blueprint.size;
			IntVector2 location = BuildingLocation.GetLocation(building);
			// 检查上下左右4个位置
			for (int i = 0; i < BuildingAssetConnector.s_connections.Length; i++)
			{
				BuildingInstance buildingInstance = building.controller.BuildingAt(location + BuildingAssetConnector.s_connections[i] * size);
				if (buildingInstance != null && buildingInstance != building)
				{
					if (buildingInstance.blueprint.name == building.blueprint.name)
					{
						num |= 1 << i;
					}
				}
			}
			// Debug.LogError("GetConnectionConfiguration: " + num + " " + building.blueprint.name);
			return num;
		}

		// Token: 0x04006255 RID: 25173
		private static IntVector2[] s_connections = new IntVector2[]
		{
			IntVector2.Up,
			IntVector2.Right,
			IntVector2.Down,
			IntVector2.Left
		};

		// Token: 0x04006256 RID: 25174
		private BuildingInstance building;
	}
}
