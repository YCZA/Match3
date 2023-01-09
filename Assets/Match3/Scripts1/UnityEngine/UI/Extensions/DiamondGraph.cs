using System;
using UnityEngine;
using UnityEngine.UI;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000C04 RID: 3076
	[AddComponentMenu("UI/Extensions/Primitives/Diamond Graph")]
	public class DiamondGraph : UIPrimitiveBase
	{
		// Token: 0x17000A78 RID: 2680
		// (get) Token: 0x0600485A RID: 18522 RVA: 0x00171BAD File Offset: 0x0016FFAD
		// (set) Token: 0x0600485B RID: 18523 RVA: 0x00171BB5 File Offset: 0x0016FFB5
		public float A
		{
			get
			{
				return this.m_a;
			}
			set
			{
				this.m_a = value;
			}
		}

		// Token: 0x17000A79 RID: 2681
		// (get) Token: 0x0600485C RID: 18524 RVA: 0x00171BBE File Offset: 0x0016FFBE
		// (set) Token: 0x0600485D RID: 18525 RVA: 0x00171BC6 File Offset: 0x0016FFC6
		public float B
		{
			get
			{
				return this.m_b;
			}
			set
			{
				this.m_b = value;
			}
		}

		// Token: 0x17000A7A RID: 2682
		// (get) Token: 0x0600485E RID: 18526 RVA: 0x00171BCF File Offset: 0x0016FFCF
		// (set) Token: 0x0600485F RID: 18527 RVA: 0x00171BD7 File Offset: 0x0016FFD7
		public float C
		{
			get
			{
				return this.m_c;
			}
			set
			{
				this.m_c = value;
			}
		}

		// Token: 0x17000A7B RID: 2683
		// (get) Token: 0x06004860 RID: 18528 RVA: 0x00171BE0 File Offset: 0x0016FFE0
		// (set) Token: 0x06004861 RID: 18529 RVA: 0x00171BE8 File Offset: 0x0016FFE8
		public float D
		{
			get
			{
				return this.m_d;
			}
			set
			{
				this.m_d = value;
			}
		}

		// Token: 0x06004862 RID: 18530 RVA: 0x00171BF4 File Offset: 0x0016FFF4
		protected override void OnPopulateMesh(VertexHelper vh)
		{
			vh.Clear();
			float num = base.rectTransform.rect.width / 2f;
			this.m_a = Math.Min(1f, Math.Max(0f, this.m_a));
			this.m_b = Math.Min(1f, Math.Max(0f, this.m_b));
			this.m_c = Math.Min(1f, Math.Max(0f, this.m_c));
			this.m_d = Math.Min(1f, Math.Max(0f, this.m_d));
			Color32 color = this.color;
			vh.AddVert(new Vector3(-num * this.m_a, 0f), color, new Vector2(0f, 0f));
			vh.AddVert(new Vector3(0f, num * this.m_b), color, new Vector2(0f, 1f));
			vh.AddVert(new Vector3(num * this.m_c, 0f), color, new Vector2(1f, 1f));
			vh.AddVert(new Vector3(0f, -num * this.m_d), color, new Vector2(1f, 0f));
			vh.AddTriangle(0, 1, 2);
			vh.AddTriangle(2, 3, 0);
		}

		// Token: 0x04006F1A RID: 28442
		[SerializeField]
		private float m_a = 1f;

		// Token: 0x04006F1B RID: 28443
		[SerializeField]
		private float m_b = 1f;

		// Token: 0x04006F1C RID: 28444
		[SerializeField]
		private float m_c = 1f;

		// Token: 0x04006F1D RID: 28445
		[SerializeField]
		private float m_d = 1f;
	}
}
