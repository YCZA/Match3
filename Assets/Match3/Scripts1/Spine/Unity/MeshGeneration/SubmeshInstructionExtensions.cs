using UnityEngine;

namespace Match3.Scripts1.Spine.Unity.MeshGeneration
{
	// Token: 0x0200023B RID: 571
	public static class SubmeshInstructionExtensions
	{
		// Token: 0x060011BB RID: 4539 RVA: 0x00031508 File Offset: 0x0002F908
		public static Material[] GetUpdatedMaterialArray(this ExposedList<SubmeshInstruction> instructions, Material[] materials)
		{
			int count = instructions.Count;
			if (count != materials.Length)
			{
				materials = new Material[count];
			}
			int i = 0;
			int num = materials.Length;
			while (i < num)
			{
				materials[i] = instructions.Items[i].material;
				i++;
			}
			return materials;
		}
	}
}
