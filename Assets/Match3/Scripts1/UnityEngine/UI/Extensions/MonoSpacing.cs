using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000BD3 RID: 3027
	[AddComponentMenu("UI/Effects/Extensions/Mono Spacing")]
	[RequireComponent(typeof(Text))]
	[RequireComponent(typeof(RectTransform))]
	public class MonoSpacing : BaseMeshEffect
	{
		// Token: 0x060046FF RID: 18175 RVA: 0x00169714 File Offset: 0x00167B14
		protected MonoSpacing()
		{
		}

		// Token: 0x06004700 RID: 18176 RVA: 0x00169727 File Offset: 0x00167B27
		protected override void Awake()
		{
			this.text = base.GetComponent<Text>();
			if (this.text == null)
			{
				global::UnityEngine.Debug.LogWarning("MonoSpacing: Missing Text component");
				return;
			}
			this.rectTransform = this.text.GetComponent<RectTransform>();
		}

		// Token: 0x17000A54 RID: 2644
		// (get) Token: 0x06004701 RID: 18177 RVA: 0x00169762 File Offset: 0x00167B62
		// (set) Token: 0x06004702 RID: 18178 RVA: 0x0016976A File Offset: 0x00167B6A
		public float Spacing
		{
			get
			{
				return this.m_spacing;
			}
			set
			{
				if (this.m_spacing == value)
				{
					return;
				}
				this.m_spacing = value;
				if (base.graphic != null)
				{
					base.graphic.SetVerticesDirty();
				}
			}
		}

		// Token: 0x06004703 RID: 18179 RVA: 0x0016979C File Offset: 0x00167B9C
		public override void ModifyMesh(VertexHelper vh)
		{
			if (!this.IsActive())
			{
				return;
			}
			List<UIVertex> list = new List<UIVertex>();
			vh.GetUIVertexStream(list);
			string[] array = this.text.text.Split(new char[]
			{
				'\n'
			});
			float num = this.Spacing * (float)this.text.fontSize / 100f;
			float num2 = 0f;
			int num3 = 0;
			switch (this.text.alignment)
			{
			case TextAnchor.UpperLeft:
			case TextAnchor.MiddleLeft:
			case TextAnchor.LowerLeft:
				num2 = 0f;
				break;
			case TextAnchor.UpperCenter:
			case TextAnchor.MiddleCenter:
			case TextAnchor.LowerCenter:
				num2 = 0.5f;
				break;
			case TextAnchor.UpperRight:
			case TextAnchor.MiddleRight:
			case TextAnchor.LowerRight:
				num2 = 1f;
				break;
			}
			foreach (string text in array)
			{
				float num4 = (float)(text.Length - 1) * num * num2 - (num2 - 0.5f) * this.rectTransform.rect.width;
				float num5 = -num4 + num / 2f * (1f - num2 * 2f);
				for (int j = 0; j < text.Length; j++)
				{
					int index = num3 * 6;
					int index2 = num3 * 6 + 1;
					int index3 = num3 * 6 + 2;
					int index4 = num3 * 6 + 3;
					int index5 = num3 * 6 + 4;
					int num6 = num3 * 6 + 5;
					if (num6 > list.Count - 1)
					{
						return;
					}
					UIVertex value = list[index];
					UIVertex value2 = list[index2];
					UIVertex value3 = list[index3];
					UIVertex value4 = list[index4];
					UIVertex value5 = list[index5];
					UIVertex value6 = list[num6];
					float x = (value2.position - value.position).x;
					bool flag = this.UseHalfCharWidth && x < this.HalfCharWidth;
					float num7 = (!flag) ? 0f : (-num / 4f);
					value.position += new Vector3(-value.position.x + num5 + -0.5f * x + num7, 0f, 0f);
					value2.position += new Vector3(-value2.position.x + num5 + 0.5f * x + num7, 0f, 0f);
					value3.position += new Vector3(-value3.position.x + num5 + 0.5f * x + num7, 0f, 0f);
					value4.position += new Vector3(-value4.position.x + num5 + 0.5f * x + num7, 0f, 0f);
					value5.position += new Vector3(-value5.position.x + num5 + -0.5f * x + num7, 0f, 0f);
					value6.position += new Vector3(-value6.position.x + num5 + -0.5f * x + num7, 0f, 0f);
					if (flag)
					{
						num5 += num / 2f;
					}
					else
					{
						num5 += num;
					}
					list[index] = value;
					list[index2] = value2;
					list[index3] = value3;
					list[index4] = value4;
					list[index5] = value5;
					list[num6] = value6;
					num3++;
				}
				num3++;
			}
			vh.Clear();
			vh.AddUIVertexTriangleStream(list);
		}

		// Token: 0x04006E30 RID: 28208
		[SerializeField]
		private float m_spacing;

		// Token: 0x04006E31 RID: 28209
		public float HalfCharWidth = 1f;

		// Token: 0x04006E32 RID: 28210
		public bool UseHalfCharWidth;

		// Token: 0x04006E33 RID: 28211
		private RectTransform rectTransform;

		// Token: 0x04006E34 RID: 28212
		private Text text;
	}
}
