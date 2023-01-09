using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000BD4 RID: 3028
	[AddComponentMenu("UI/Effects/Extensions/Nicer Outline")]
	public class NicerOutline : BaseMeshEffect
	{
		// Token: 0x17000A55 RID: 2645
		// (get) Token: 0x06004705 RID: 18181 RVA: 0x00169BDE File Offset: 0x00167FDE
		// (set) Token: 0x06004706 RID: 18182 RVA: 0x00169BE6 File Offset: 0x00167FE6
		public Color effectColor
		{
			get
			{
				return this.m_EffectColor;
			}
			set
			{
				this.m_EffectColor = value;
				if (base.graphic != null)
				{
					base.graphic.SetVerticesDirty();
				}
			}
		}

		// Token: 0x17000A56 RID: 2646
		// (get) Token: 0x06004707 RID: 18183 RVA: 0x00169C0B File Offset: 0x0016800B
		// (set) Token: 0x06004708 RID: 18184 RVA: 0x00169C14 File Offset: 0x00168014
		public Vector2 effectDistance
		{
			get
			{
				return this.m_EffectDistance;
			}
			set
			{
				if (value.x > 600f)
				{
					value.x = 600f;
				}
				if (value.x < -600f)
				{
					value.x = -600f;
				}
				if (value.y > 600f)
				{
					value.y = 600f;
				}
				if (value.y < -600f)
				{
					value.y = -600f;
				}
				if (this.m_EffectDistance == value)
				{
					return;
				}
				this.m_EffectDistance = value;
				if (base.graphic != null)
				{
					base.graphic.SetVerticesDirty();
				}
			}
		}

		// Token: 0x17000A57 RID: 2647
		// (get) Token: 0x06004709 RID: 18185 RVA: 0x00169CCA File Offset: 0x001680CA
		// (set) Token: 0x0600470A RID: 18186 RVA: 0x00169CD2 File Offset: 0x001680D2
		public bool useGraphicAlpha
		{
			get
			{
				return this.m_UseGraphicAlpha;
			}
			set
			{
				this.m_UseGraphicAlpha = value;
				if (base.graphic != null)
				{
					base.graphic.SetVerticesDirty();
				}
			}
		}

		// Token: 0x0600470B RID: 18187 RVA: 0x00169CF8 File Offset: 0x001680F8
		protected void ApplyShadowZeroAlloc(List<UIVertex> verts, Color32 color, int start, int end, float x, float y)
		{
			int num = verts.Count * 2;
			if (verts.Capacity < num)
			{
				verts.Capacity = num;
			}
			for (int i = start; i < end; i++)
			{
				UIVertex uivertex = verts[i];
				verts.Add(uivertex);
				Vector3 position = uivertex.position;
				position.x += x;
				position.y += y;
				uivertex.position = position;
				Color32 color2 = color;
				if (this.m_UseGraphicAlpha)
				{
					color2.a = (byte)(color2.a * verts[i].color.a / byte.MaxValue);
				}
				uivertex.color = color2;
				verts[i] = uivertex;
			}
		}

		// Token: 0x0600470C RID: 18188 RVA: 0x00169DC0 File Offset: 0x001681C0
		protected void ApplyShadow(List<UIVertex> verts, Color32 color, int start, int end, float x, float y)
		{
			int num = verts.Count * 2;
			if (verts.Capacity < num)
			{
				verts.Capacity = num;
			}
			this.ApplyShadowZeroAlloc(verts, color, start, end, x, y);
		}

		// Token: 0x0600470D RID: 18189 RVA: 0x00169DF8 File Offset: 0x001681F8
		public override void ModifyMesh(VertexHelper vh)
		{
			if (!this.IsActive())
			{
				return;
			}
			List<UIVertex> list = new List<UIVertex>();
			vh.GetUIVertexStream(list);
			Text component = base.GetComponent<Text>();
			float num = 1f;
			if (component && component.resizeTextForBestFit)
			{
				num = (float)component.cachedTextGenerator.fontSizeUsedForBestFit / (float)(component.resizeTextMaxSize - 1);
			}
			float num2 = this.effectDistance.x * num;
			float num3 = this.effectDistance.y * num;
			int start = 0;
			int count = list.Count;
			this.ApplyShadow(list, this.effectColor, start, list.Count, num2, num3);
			start = count;
			count = list.Count;
			this.ApplyShadow(list, this.effectColor, start, list.Count, num2, -num3);
			start = count;
			count = list.Count;
			this.ApplyShadow(list, this.effectColor, start, list.Count, -num2, num3);
			start = count;
			count = list.Count;
			this.ApplyShadow(list, this.effectColor, start, list.Count, -num2, -num3);
			start = count;
			count = list.Count;
			this.ApplyShadow(list, this.effectColor, start, list.Count, num2, 0f);
			start = count;
			count = list.Count;
			this.ApplyShadow(list, this.effectColor, start, list.Count, -num2, 0f);
			start = count;
			count = list.Count;
			this.ApplyShadow(list, this.effectColor, start, list.Count, 0f, num3);
			start = count;
			count = list.Count;
			this.ApplyShadow(list, this.effectColor, start, list.Count, 0f, -num3);
			vh.Clear();
			vh.AddUIVertexTriangleStream(list);
		}

		// Token: 0x04006E35 RID: 28213
		[SerializeField]
		private Color m_EffectColor = new Color(0f, 0f, 0f, 0.5f);

		// Token: 0x04006E36 RID: 28214
		[SerializeField]
		private Vector2 m_EffectDistance = new Vector2(1f, -1f);

		// Token: 0x04006E37 RID: 28215
		[SerializeField]
		private bool m_UseGraphicAlpha = true;
	}
}
