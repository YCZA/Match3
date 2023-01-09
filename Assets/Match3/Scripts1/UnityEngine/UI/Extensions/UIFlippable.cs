using UnityEngine;
using UnityEngine.UI;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000BDF RID: 3039
	[RequireComponent(typeof(RectTransform), typeof(Graphic))]
	[DisallowMultipleComponent]
	[AddComponentMenu("UI/Effects/Extensions/Flippable")]
	public class UIFlippable : BaseMeshEffect
	{
		// Token: 0x17000A5B RID: 2651
		// (get) Token: 0x0600473F RID: 18239 RVA: 0x0016B1CD File Offset: 0x001695CD
		// (set) Token: 0x06004740 RID: 18240 RVA: 0x0016B1D5 File Offset: 0x001695D5
		public bool horizontal
		{
			get
			{
				return this.m_Horizontal;
			}
			set
			{
				this.m_Horizontal = value;
			}
		}

		// Token: 0x17000A5C RID: 2652
		// (get) Token: 0x06004741 RID: 18241 RVA: 0x0016B1DE File Offset: 0x001695DE
		// (set) Token: 0x06004742 RID: 18242 RVA: 0x0016B1E6 File Offset: 0x001695E6
		public bool vertical
		{
			get
			{
				return this.m_Veritical;
			}
			set
			{
				this.m_Veritical = value;
			}
		}

		// Token: 0x06004743 RID: 18243 RVA: 0x0016B1F0 File Offset: 0x001695F0
		public override void ModifyMesh(VertexHelper verts)
		{
			RectTransform rectTransform = base.transform as RectTransform;
			for (int i = 0; i < verts.currentVertCount; i++)
			{
				UIVertex vertex = default(UIVertex);
				verts.PopulateUIVertex(ref vertex, i);
				vertex.position = new Vector3((!this.m_Horizontal) ? vertex.position.x : (vertex.position.x + (rectTransform.rect.center.x - vertex.position.x) * 2f), (!this.m_Veritical) ? vertex.position.y : (vertex.position.y + (rectTransform.rect.center.y - vertex.position.y) * 2f), vertex.position.z);
				verts.SetUIVertex(vertex, i);
			}
		}

		// Token: 0x04006E59 RID: 28249
		[SerializeField]
		private bool m_Horizontal;

		// Token: 0x04006E5A RID: 28250
		[SerializeField]
		private bool m_Veritical;
	}
}
