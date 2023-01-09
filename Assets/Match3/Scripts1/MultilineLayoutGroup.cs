using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000020 RID: 32
namespace Match3.Scripts1
{
	public class MultilineLayoutGroup : LayoutGroup
	{
		// Token: 0x06000143 RID: 323 RVA: 0x00007638 File Offset: 0x00005A38
		public override void CalculateLayoutInputHorizontal()
		{
			base.CalculateLayoutInputHorizontal();
		}

		// Token: 0x06000144 RID: 324 RVA: 0x00007640 File Offset: 0x00005A40
		public override void CalculateLayoutInputVertical()
		{
		}

		// Token: 0x06000145 RID: 325 RVA: 0x00007644 File Offset: 0x00005A44
		public override void SetLayoutHorizontal()
		{
			this.CalculateDimentions();
			this.itemRowIds = new List<int>(base.rectChildren.Count);
			this.maxRowHeights = new List<float>();
			this.maxRowHeights.Add(0f);
			float num = base.rectTransform.rect.size[0] - (float)(this.m_Padding.left + this.m_Padding.right);
			float num2 = 0f;
			int i = 0;
			int num3 = 0;
			int num4 = 0;
			while (i < base.rectChildren.Count)
			{
				float num5 = base.rectChildren[i].sizeDelta[0];
				float num6 = num2 + (float)this.m_Padding.left;
				if (num6 + num5 > num + (float)this.m_Padding.left && num3 > 0)
				{
					num6 = (float)this.m_Padding.left;
					num2 = 0f;
					num3 = 1;
					num4++;
					this.maxRowHeights.Add(base.rectChildren[i].sizeDelta[1]);
				}
				else
				{
					num3++;
				}
				base.SetChildAlongAxis(base.rectChildren[i], 0, num6);
				num2 += num5;
				this.itemRowIds.Add(num4);
				if (base.rectChildren[i].sizeDelta[1] > this.maxRowHeights[num4])
				{
					this.maxRowHeights[num4] = base.rectChildren[i].sizeDelta[1];
				}
				i++;
			}
		}

		// Token: 0x06000146 RID: 326 RVA: 0x00007808 File Offset: 0x00005C08
		private void CalculateDimentions()
		{
			this.ratio = base.rectTransform.rect.size.x / base.rectTransform.rect.size.y;
			this.maxItemsPerLineFloat = Mathf.Sqrt((float)base.rectChildren.Count / this.ratio);
			this.numLinesFloat = (float)base.rectChildren.Count / this.maxItemsPerLineFloat;
			this.maxItemsPerLine = Mathf.CeilToInt(this.maxItemsPerLineFloat);
			this.factor = (float)this.maxItemsPerLine - this.maxItemsPerLineFloat;
			this.numLines = Mathf.CeilToInt(this.numLinesFloat);
		}

		// Token: 0x06000147 RID: 327 RVA: 0x000078C0 File Offset: 0x00005CC0
		public override void SetLayoutVertical()
		{
			for (int i = 0; i < base.rectChildren.Count; i++)
			{
				float pos = (float)this.m_Padding.top;
				if (this.itemRowIds[i] > 0)
				{
					pos = this.GetRowOffset(this.itemRowIds[i]) + (float)this.m_Padding.top;
				}
				base.SetChildAlongAxis(base.rectChildren[i], 1, pos);
			}
		}

		// Token: 0x06000148 RID: 328 RVA: 0x0000793C File Offset: 0x00005D3C
		private float GetRowOffset(int ItemRowId)
		{
			float num = 0f;
			for (int i = ItemRowId - 1; i >= 0; i--)
			{
				num += this.maxRowHeights[i];
			}
			return num;
		}

		// Token: 0x06000149 RID: 329 RVA: 0x00007973 File Offset: 0x00005D73
		protected override void OnRectTransformDimensionsChange()
		{
			base.OnRectTransformDimensionsChange();
			LayoutRebuilder.MarkLayoutForRebuild((RectTransform)base.transform);
		}

		// Token: 0x0400008F RID: 143
		public float ratio;

		// Token: 0x04000090 RID: 144
		public float numLinesFloat;

		// Token: 0x04000091 RID: 145
		public float maxItemsPerLineFloat;

		// Token: 0x04000092 RID: 146
		public int numLines;

		// Token: 0x04000093 RID: 147
		public int maxItemsPerLine;

		// Token: 0x04000094 RID: 148
		public float factor;

		// Token: 0x04000095 RID: 149
		private List<int> itemRowIds;

		// Token: 0x04000096 RID: 150
		private List<float> maxRowHeights;
	}
}
