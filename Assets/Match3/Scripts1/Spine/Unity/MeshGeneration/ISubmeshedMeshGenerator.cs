using System.Collections.Generic;

namespace Match3.Scripts1.Spine.Unity.MeshGeneration
{
	// Token: 0x02000237 RID: 567
	public interface ISubmeshedMeshGenerator
	{
		// Token: 0x060011B2 RID: 4530
		SubmeshedMeshInstruction GenerateInstruction(Skeleton skeleton);

		// Token: 0x060011B3 RID: 4531
		MeshAndMaterials GenerateMesh(SubmeshedMeshInstruction wholeMeshInstruction);

		// Token: 0x17000286 RID: 646
		// (get) Token: 0x060011B4 RID: 4532
		List<Slot> Separators { get; }
	}
}
