namespace Match3.Scripts1.Spine.Unity.MeshGeneration
{
	// Token: 0x02000238 RID: 568
	public interface ISubmeshSetMeshGenerator
	{
		// Token: 0x060011B5 RID: 4533
		MeshAndMaterials GenerateMesh(ExposedList<SubmeshInstruction> instructions, int startSubmesh, int endSubmesh);

		// Token: 0x17000287 RID: 647
		// (get) Token: 0x060011B6 RID: 4534
		// (set) Token: 0x060011B7 RID: 4535
		bool GenerateNormals { get; set; }
	}
}
