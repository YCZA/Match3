using UnityEngine;

// Token: 0x020004E2 RID: 1250
namespace Match3.Scripts1
{
	public class PrepareRubbleMesh : MonoBehaviour
	{
		// Token: 0x060022BB RID: 8891 RVA: 0x00099E0C File Offset: 0x0009820C
		public void Prepare(RenderTexture swipeRenderTexture)
		{
			this.meshFilter = base.GetComponent<MeshFilter>();
			this.meshRenderer = base.GetComponent<MeshRenderer>();
			this.material = this.meshRenderer.material;
			Bounds bounds = this.meshFilter.mesh.bounds;
			Vector4 value = new Vector4(bounds.center.x - bounds.extents.x, bounds.center.x + bounds.extents.x, bounds.center.z - bounds.extents.z, bounds.center.z + bounds.extents.z);
			this.material.SetVector("_Bounds", value);
			this.material.SetTexture("_Mask", swipeRenderTexture);
		}

		// Token: 0x04004E65 RID: 20069
		private MeshFilter meshFilter;

		// Token: 0x04004E66 RID: 20070
		private MeshRenderer meshRenderer;

		// Token: 0x04004E67 RID: 20071
		private Material material;
	}
}
