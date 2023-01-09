using UnityEngine;

namespace Match3.Scripts1.Spine.Unity.MeshGeneration
{
	// Token: 0x02000236 RID: 566
	public interface ISimpleMeshGenerator
	{
		// Token: 0x17000284 RID: 644
		// (set) Token: 0x060011AF RID: 4527
		float Scale { set; }

		// Token: 0x060011B0 RID: 4528
		Mesh GenerateMesh(Skeleton skeleton);

		// Token: 0x17000285 RID: 645
		// (get) Token: 0x060011B1 RID: 4529
		Mesh LastGeneratedMesh { get; }
	}
}
