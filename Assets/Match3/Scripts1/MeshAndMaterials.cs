using UnityEngine;

namespace Match3.Scripts1
{
	// Token: 0x0200023C RID: 572
	public struct MeshAndMaterials
	{
		// Token: 0x060011BC RID: 4540 RVA: 0x00031558 File Offset: 0x0002F958
		public MeshAndMaterials(Mesh mesh, Material[] materials)
		{
			this.mesh = mesh;
			this.materials = materials;
		}

		// Token: 0x040041F3 RID: 16883
		public readonly Mesh mesh;

		// Token: 0x040041F4 RID: 16884
		public readonly Material[] materials;
	}
}
