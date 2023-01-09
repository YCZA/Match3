using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000BD2 RID: 3026
	[AddComponentMenu("UI/Effects/Extensions/Letter Spacing")]
	public class LetterSpacing : BaseMeshEffect
	{
		// Token: 0x060046FB RID: 18171 RVA: 0x00169458 File Offset: 0x00167858
		protected LetterSpacing()
		{
		}

		// Token: 0x17000A53 RID: 2643
		// (get) Token: 0x060046FC RID: 18172 RVA: 0x00169460 File Offset: 0x00167860
		// (set) Token: 0x060046FD RID: 18173 RVA: 0x00169468 File Offset: 0x00167868
		public float spacing
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

		// Token: 0x060046FE RID: 18174 RVA: 0x0016949C File Offset: 0x0016789C
		public override void ModifyMesh(VertexHelper vh)
		{
			if (!this.IsActive())
			{
				return;
			}
			List<UIVertex> list = new List<UIVertex>();
			vh.GetUIVertexStream(list);
			Text component = base.GetComponent<Text>();
			if (component == null)
			{
				global::UnityEngine.Debug.LogWarning("LetterSpacing: Missing Text component");
				return;
			}
			string[] array = component.text.Split(new char[]
			{
				'\n'
			});
			float num = this.spacing * (float)component.fontSize / 100f;
			float num2 = 0f;
			int num3 = 0;
			switch (component.alignment)
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
				float num4 = (float)(text.Length - 1) * num * num2;
				for (int j = 0; j < text.Length; j++)
				{
					int index = num3 * 6;
					int index2 = num3 * 6 + 1;
					int index3 = num3 * 6 + 2;
					int index4 = num3 * 6 + 3;
					int index5 = num3 * 6 + 4;
					int num5 = num3 * 6 + 5;
					if (num5 > list.Count - 1)
					{
						return;
					}
					UIVertex value = list[index];
					UIVertex value2 = list[index2];
					UIVertex value3 = list[index3];
					UIVertex value4 = list[index4];
					UIVertex value5 = list[index5];
					UIVertex value6 = list[num5];
					Vector3 b = Vector3.right * (num * (float)j - num4);
					value.position += b;
					value2.position += b;
					value3.position += b;
					value4.position += b;
					value5.position += b;
					value6.position += b;
					list[index] = value;
					list[index2] = value2;
					list[index3] = value3;
					list[index4] = value4;
					list[index5] = value5;
					list[num5] = value6;
					num3++;
				}
				num3++;
			}
			vh.Clear();
			vh.AddUIVertexTriangleStream(list);
		}

		// Token: 0x04006E2F RID: 28207
		[SerializeField]
		private float m_spacing;
	}
}
