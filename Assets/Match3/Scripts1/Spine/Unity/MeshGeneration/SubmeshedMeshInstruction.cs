using UnityEngine;

namespace Match3.Scripts1.Spine.Unity.MeshGeneration
{
	// Token: 0x02000239 RID: 569
	public class SubmeshedMeshInstruction
	{
		// Token: 0x060011B9 RID: 4537 RVA: 0x000314EA File Offset: 0x0002F8EA
		public Material[] GetUpdatedMaterialArray(Material[] materials)
		{
			return this.submeshInstructions.GetUpdatedMaterialArray(materials);
		}

		// Token: 0x040041E8 RID: 16872
		public readonly ExposedList<SubmeshInstruction> submeshInstructions = new ExposedList<SubmeshInstruction>();

		// Token: 0x040041E9 RID: 16873
		public readonly ExposedList<Attachment> attachmentList = new ExposedList<Attachment>();

		// Token: 0x040041EA RID: 16874
		public int vertexCount = -1;
	}
}
