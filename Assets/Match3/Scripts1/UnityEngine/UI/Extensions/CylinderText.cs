using UnityEngine;
using UnityEngine.UI;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000BCB RID: 3019
	[RequireComponent(typeof(Text), typeof(RectTransform))]
	[AddComponentMenu("UI/Effects/Extensions/Cylinder Text")]
	public class CylinderText : BaseMeshEffect
	{
		// Token: 0x060046DF RID: 18143 RVA: 0x001685C2 File Offset: 0x001669C2
		protected override void Awake()
		{
			base.Awake();
			this.rectTrans = base.GetComponent<RectTransform>();
			this.OnRectTransformDimensionsChange();
		}

		// Token: 0x060046E0 RID: 18144 RVA: 0x001685DC File Offset: 0x001669DC
		protected override void OnEnable()
		{
			base.OnEnable();
			this.rectTrans = base.GetComponent<RectTransform>();
			this.OnRectTransformDimensionsChange();
		}

		// Token: 0x060046E1 RID: 18145 RVA: 0x001685F8 File Offset: 0x001669F8
		public override void ModifyMesh(VertexHelper vh)
		{
			if (!this.IsActive())
			{
				return;
			}
			int currentVertCount = vh.currentVertCount;
			if (!this.IsActive() || currentVertCount == 0)
			{
				return;
			}
			for (int i = 0; i < vh.currentVertCount; i++)
			{
				UIVertex vertex = default(UIVertex);
				vh.PopulateUIVertex(ref vertex, i);
				float x = vertex.position.x;
				vertex.position.z = -this.radius * Mathf.Cos(x / this.radius);
				vertex.position.x = this.radius * Mathf.Sin(x / this.radius);
				vh.SetUIVertex(vertex, i);
			}
		}

		// Token: 0x04006E12 RID: 28178
		public float radius;

		// Token: 0x04006E13 RID: 28179
		private RectTransform rectTrans;
	}
}
