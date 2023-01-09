using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020008A6 RID: 2214
namespace Match3.Scripts1
{
	[ExecuteInEditMode]
	[RequireComponent(typeof(CanvasRenderer), typeof(RectTransform))]
	[DisallowMultipleComponent]
	public class MeshGraphic : MaskableGraphic
	{
		// Token: 0x06003626 RID: 13862 RVA: 0x00106476 File Offset: 0x00104876
		protected override void Awake()
		{
			base.Awake();
			this.Rebuild(CanvasUpdate.PreRender);
		}

		// Token: 0x06003627 RID: 13863 RVA: 0x00106485 File Offset: 0x00104885
		public override void Rebuild(CanvasUpdate update)
		{
			base.Rebuild(update);
			if (update == CanvasUpdate.PreRender)
			{
				this.UpdateMesh();
			}
		}

		// Token: 0x06003628 RID: 13864 RVA: 0x001064A8 File Offset: 0x001048A8
		private void UpdateMesh()
		{
			Canvas canvas = base.GetComponentInParent<Canvas>();
			if (!this.mesh || !canvas)
			{
				return;
			}
			if (!this._dynamicMesh)
			{
				this._dynamicMesh = new Mesh();
			}
			this._dynamicMesh.vertices = Array.ConvertAll<Vector3, Vector3>(this.mesh.vertices, (Vector3 v) => v * canvas.referencePixelsPerUnit);
			this._dynamicMesh.uv = this.mesh.uv;
			this._dynamicMesh.triangles = this.mesh.triangles;
			base.canvasRenderer.SetMesh(this._dynamicMesh);
		}

		// Token: 0x04005E27 RID: 24103
		public Mesh mesh;

		// Token: 0x04005E28 RID: 24104
		private Mesh _dynamicMesh;
	}
}
