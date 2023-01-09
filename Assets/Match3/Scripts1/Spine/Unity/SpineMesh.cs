using UnityEngine;

namespace Match3.Scripts1.Spine.Unity
{
	// Token: 0x0200023D RID: 573
	public static class SpineMesh
	{
		// Token: 0x060011BD RID: 4541 RVA: 0x00031568 File Offset: 0x0002F968
		public static Mesh NewMesh()
		{
			Mesh mesh = new Mesh();
			mesh.MarkDynamic();
			mesh.name = "Skeleton Mesh";
			mesh.hideFlags = (HideFlags.DontSaveInEditor | HideFlags.DontSaveInBuild);
			return mesh;
		}

		// Token: 0x040041F5 RID: 16885
		internal const HideFlags MeshHideflags = HideFlags.DontSaveInEditor | HideFlags.DontSaveInBuild;
	}
}
