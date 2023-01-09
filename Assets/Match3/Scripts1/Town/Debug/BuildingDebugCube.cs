using UnityEngine;

namespace Match3.Scripts1.Town.Debug
{
	// Token: 0x0200095F RID: 2399
	public class BuildingDebugCube : MonoBehaviour, IDebugDataView<BuildingInstance>
	{
		// Token: 0x06003A89 RID: 14985 RVA: 0x00121EC8 File Offset: 0x001202C8
		public void ShowDebug(BuildingInstance building)
		{
			this.cube.localScale = new Vector3((float)building.blueprint.size - 0.33f, (float)building.blueprint.size * 0.4f, (float)building.blueprint.size - 0.33f);
			this.cube.localPosition = new Vector3(0.5f, 0.2f, 0.5f) * (float)building.blueprint.size;
			this.cube.gameObject.SetActive(true);
		}

		// Token: 0x06003A8A RID: 14986 RVA: 0x00121F5C File Offset: 0x0012035C
		public void HideDebug()
		{
			this.cube.gameObject.SetActive(false);
		}

		// Token: 0x0400626A RID: 25194
		public Transform cube;
	}
}
