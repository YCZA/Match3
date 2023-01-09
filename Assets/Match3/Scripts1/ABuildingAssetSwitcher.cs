using Match3.Scripts1.Town;
using Match3.Scripts1.UnityEngine;
using Match3.Scripts1.Wooga.UI;
using UnityEngine;

// Token: 0x02000958 RID: 2392
namespace Match3.Scripts1
{
	public abstract class ABuildingAssetSwitcher : MonoBehaviour
	{
		// Token: 0x06003A58 RID: 14936 RVA: 0x001209DC File Offset: 0x0011EDDC
		protected void SwitchAsset(int id, BuildingInstance building)
		{
			if (id == this.lastSelectedId)
			{
				return;
			}
			GameObject gameObject = (!this.variants[id]) ? this.variants[0] : this.variants[id];
			if (!gameObject)
			{
				WoogaDebug.LogWarningFormatted("Could not load variant {0} on {1}", new object[]
				{
					id,
					building.blueprint.name
				});
				return;
			}
			if (this.selectedConnection)
			{
				global::UnityEngine.Object.Destroy(this.selectedConnection);
			}
			this.selectedConnection = global::UnityEngine.Object.Instantiate<GameObject>(gameObject, Vector3.zero, Quaternion.identity);
			if (!this.selectedConnection)
			{
				WoogaDebug.LogWarning(new object[]
				{
					"Could not find asset for",
					building.blueprint.name
				});
				return;
			}
			this.selectedConnection.transform.SetParent(base.transform, false);
			this.lastSelectedId = id;
			IDataView<BuildingInstance> component = this.selectedConnection.GetComponent<IDataView<BuildingInstance>>();
			if (component != null)
			{
				component.Show(building);
			}
			bool isSelected = building == BuildingLocation.Selected;
			this.ExecuteOnChildren(delegate(Renderer r)
			{
				BuildingAssetLoader.ToggleBuildingLayer(building, r, isSelected);
			}, true);
			BuildingAssetLoader.RealignAllMeshFilters(base.GetComponentsInChildren<MeshFilter>());
			if (!building.isForeshadowing)
			{
				MeshRenderer[] componentsInChildren = base.GetComponentsInChildren<MeshRenderer>();
				foreach (MeshRenderer meshRenderer in componentsInChildren)
				{
					meshRenderer.sortingLayerName = CameraExtensions.ABOVE_FORSHADOWING_LAYER;
				}
			}
		}

		// Token: 0x04006252 RID: 25170
		protected GameObject selectedConnection;

		// Token: 0x04006253 RID: 25171
		protected int lastSelectedId = -1;

		// Token: 0x04006254 RID: 25172
		public GameObject[] variants;
	}
}
