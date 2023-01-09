using System;
using UnityEngine;

namespace Match3.Scripts1.Spine.Unity.Modules
{
	// Token: 0x0200026D RID: 621
	[ExecuteInEditMode]
	public class SkeletonUtilitySubmeshRenderer : MonoBehaviour
	{
		// Token: 0x060012FC RID: 4860 RVA: 0x0003C18A File Offset: 0x0003A58A
		private void Awake()
		{
			this.cachedRenderer = base.GetComponent<Renderer>();
			this.filter = base.GetComponent<MeshFilter>();
			this.sharedMaterials = new Material[0];
		}

		// Token: 0x060012FD RID: 4861 RVA: 0x0003C1B0 File Offset: 0x0003A5B0
		public void SetMesh(Renderer parentRenderer, Mesh mesh, Material mat)
		{
			if (this.cachedRenderer == null)
			{
				return;
			}
			this.cachedRenderer.enabled = true;
			this.filter.sharedMesh = mesh;
			if (this.cachedRenderer.sharedMaterials.Length != parentRenderer.sharedMaterials.Length)
			{
				this.sharedMaterials = parentRenderer.sharedMaterials;
			}
			for (int i = 0; i < this.sharedMaterials.Length; i++)
			{
				if (i == this.submeshIndex)
				{
					this.sharedMaterials[i] = mat;
				}
				else
				{
					this.sharedMaterials[i] = this.hiddenPassMaterial;
				}
			}
			this.cachedRenderer.sharedMaterials = this.sharedMaterials;
		}

		// Token: 0x0400433E RID: 17214
		[NonSerialized]
		public Mesh mesh;

		// Token: 0x0400433F RID: 17215
		public int submeshIndex;

		// Token: 0x04004340 RID: 17216
		public Material hiddenPassMaterial;

		// Token: 0x04004341 RID: 17217
		private Renderer cachedRenderer;

		// Token: 0x04004342 RID: 17218
		private MeshFilter filter;

		// Token: 0x04004343 RID: 17219
		private Material[] sharedMaterials;
	}
}
