using UnityEngine;

namespace Match3.Scripts1.Spine.Unity
{
	// Token: 0x02000235 RID: 565
	public class DoubleBufferedMesh
	{
		// Token: 0x060011AE RID: 4526 RVA: 0x00031498 File Offset: 0x0002F898
		public Mesh GetNextMesh()
		{
			this.usingMesh1 = !this.usingMesh1;
			return (!this.usingMesh1) ? this.mesh2 : this.mesh1;
		}

		// Token: 0x040041E5 RID: 16869
		private readonly Mesh mesh1 = SpineMesh.NewMesh();

		// Token: 0x040041E6 RID: 16870
		private readonly Mesh mesh2 = SpineMesh.NewMesh();

		// Token: 0x040041E7 RID: 16871
		private bool usingMesh1;
	}
}
