using UnityEngine;
using UnityEngine.UI;

namespace Match3.Scripts1.ATV.UI
{
	// Token: 0x0200001F RID: 31
	public class ContainerShapeLayoutGroup : LayoutGroup
	{
		// Token: 0x06000139 RID: 313 RVA: 0x00006D9F File Offset: 0x0000519F
		public override void CalculateLayoutInputHorizontal()
		{
			base.CalculateLayoutInputHorizontal();
		}

		// Token: 0x0600013A RID: 314 RVA: 0x00006DA7 File Offset: 0x000051A7
		public override void CalculateLayoutInputVertical()
		{
		}

		// Token: 0x0600013B RID: 315 RVA: 0x00006DAC File Offset: 0x000051AC
		public override void SetLayoutHorizontal()
		{
			this.CalculateItemDistribution();
			this.CalculateItemSize();
			int axis = (!this.verticalFlow) ? 0 : 1;
			int num = base.rectChildren.Count;
			float emptySpaceInLine = this.GetEmptySpaceInLine(this.maxItemsPerLine);
			float num2 = base.GetAlignmentOnAxis(axis);
			num2 = ((!this.reverseFlow) ? num2 : Mathf.Abs(1f - num2));
			int i = 0;
			int num3 = 0;
			while (i < base.rectChildren.Count)
			{
				float pos;
				if (!this.reverseFlow)
				{
					float num4 = (float)((!this.verticalFlow) ? this.m_Padding.left : this.m_Padding.top);
					pos = num4 + (float)num3 * this.itemSize + emptySpaceInLine * num2 + (float)num3 * this.itemSpacing;
				}
				else
				{
					float num5 = (!this.verticalFlow) ? (base.rectTransform.rect.size.x - (float)this.m_Padding.right) : (base.rectTransform.rect.size.y - (float)this.m_Padding.bottom);
					pos = num5 - (float)num3 * this.itemSize - emptySpaceInLine * num2 - (float)num3 * this.itemSpacing - this.itemSize;
				}
				base.SetChildAlongAxis(base.rectChildren[i], axis, pos, this.itemSize);
				num3++;
				if (num3 > this.maxItemsPerLine - 1)
				{
					num3 = 0;
					num -= this.maxItemsPerLine;
					int numItemsInLine = Mathf.Min(num, this.maxItemsPerLine);
					emptySpaceInLine = this.GetEmptySpaceInLine(numItemsInLine);
				}
				i++;
			}
		}

		// Token: 0x0600013C RID: 316 RVA: 0x00006F70 File Offset: 0x00005370
		private float GetEmptySpaceInLine(int numItemsInLine)
		{
			float num;
			if (!this.verticalFlow)
			{
				num = base.rectTransform.rect.size.x - (float)(this.m_Padding.left + this.m_Padding.right);
			}
			else
			{
				num = base.rectTransform.rect.size.y - (float)(this.m_Padding.top + this.m_Padding.bottom);
			}
			return num - (float)numItemsInLine * this.itemSize - (float)(numItemsInLine - 1) * this.itemSpacing;
		}

		// Token: 0x0600013D RID: 317 RVA: 0x00007014 File Offset: 0x00005414
		public override void SetLayoutVertical()
		{
			int axis = (!this.verticalFlow) ? 1 : 0;
			float emptySpaceAboveBelowLines = this.GetEmptySpaceAboveBelowLines();
			float num = base.GetAlignmentOnAxis(axis);
			num = ((!this.reverseFlow) ? num : Mathf.Abs(1f - num));
			int i = 0;
			int num2 = 0;
			int num3 = 0;
			while (i < base.rectChildren.Count)
			{
				float pos;
				if (!this.reverseFlow)
				{
					float num4 = (float)((!this.verticalFlow) ? this.m_Padding.top : this.m_Padding.left);
					pos = num4 + (float)num3 * this.itemSize + emptySpaceAboveBelowLines * num + (float)num3 * this.itemSpacing;
				}
				else
				{
					float num5 = (!this.verticalFlow) ? (base.rectTransform.rect.size.y - (float)this.m_Padding.bottom) : (base.rectTransform.rect.size.x - (float)this.m_Padding.right);
					pos = num5 - (float)num3 * this.itemSize - emptySpaceAboveBelowLines * num - (float)num3 * this.itemSpacing - this.itemSize;
				}
				base.SetChildAlongAxis(base.rectChildren[i], axis, pos, this.itemSize);
				num2++;
				if (num2 > this.maxItemsPerLine - 1)
				{
					num2 = 0;
					num3++;
				}
				i++;
			}
		}

		// Token: 0x0600013E RID: 318 RVA: 0x000071A0 File Offset: 0x000055A0
		private float GetEmptySpaceAboveBelowLines()
		{
			float num;
			if (!this.verticalFlow)
			{
				num = base.rectTransform.rect.size.y - (float)(this.m_Padding.top + this.m_Padding.bottom);
			}
			else
			{
				num = base.rectTransform.rect.size.x - (float)(this.m_Padding.left + this.m_Padding.right);
			}
			return num - (float)this.numLines * this.itemSize - (float)(this.numLines - 1) * this.itemSpacing;
		}

		// Token: 0x0600013F RID: 319 RVA: 0x00007250 File Offset: 0x00005650
		private void CalculateItemSize()
		{
			float num = (!this.verticalFlow) ? (base.rectTransform.rect.size.x - (float)(this.m_Padding.left + this.m_Padding.right)) : (base.rectTransform.rect.size.y - (float)(this.m_Padding.top + this.m_Padding.bottom));
			float num2 = num / (float)this.maxItemsPerLine;
			num2 = Mathf.Clamp(num2, this.itemSizeMin, this.itemSizeMax);
			float num3 = (!this.verticalFlow) ? (base.rectTransform.rect.size.y - (float)(this.m_Padding.top + this.m_Padding.bottom)) : (base.rectTransform.rect.size.x - (float)(this.m_Padding.left + this.m_Padding.right));
			float num4 = num3 / (float)this.numLines;
			num4 = Mathf.Clamp(num4, this.itemSizeMin, this.itemSizeMax);
			this.itemSize = Mathf.Min(num2, num4);
		}

		// Token: 0x06000140 RID: 320 RVA: 0x000073A8 File Offset: 0x000057A8
		private void CalculateItemDistribution()
		{
			float num;
			if (this.verticalFlow)
			{
				num = (base.rectTransform.rect.size.y - (float)(this.m_Padding.top + this.m_Padding.bottom)) / (base.rectTransform.rect.size.x - (float)(this.m_Padding.left + this.m_Padding.right));
			}
			else
			{
				num = (base.rectTransform.rect.size.x - (float)(this.m_Padding.left + this.m_Padding.right)) / (base.rectTransform.rect.size.y - (float)(this.m_Padding.top + this.m_Padding.bottom));
			}
			this.numLinesFloat = Mathf.Sqrt((float)base.rectChildren.Count / num);
			this.maxItemsPerLineFloat = (float)base.rectChildren.Count / this.numLinesFloat;
			float num2 = this.numLinesFloat * this.maxItemsPerLineFloat;
			float f = Mathf.Round(this.numLinesFloat) - this.numLinesFloat;
			float f2 = Mathf.Round(this.maxItemsPerLineFloat) - this.maxItemsPerLineFloat;
			bool flag = Mathf.Abs(f) < Mathf.Abs(f2);
			if (flag)
			{
				this.numLines = Mathf.RoundToInt(this.numLinesFloat);
				float num3 = num2 / (float)this.numLines - this.maxItemsPerLineFloat;
				this.maxItemsPerLine = Mathf.RoundToInt(this.maxItemsPerLineFloat + num3);
			}
			else
			{
				this.maxItemsPerLine = Mathf.RoundToInt(this.maxItemsPerLineFloat);
				float num4 = num2 / (float)this.maxItemsPerLine - this.numLinesFloat;
				this.numLines = Mathf.RoundToInt(this.numLinesFloat + num4);
			}
			if (this.numLines * this.maxItemsPerLine < base.rectChildren.Count)
			{
				if (num > 1f)
				{
					this.maxItemsPerLine++;
				}
				else
				{
					this.numLines++;
				}
			}
			this.numLines = Mathf.Clamp(this.numLines, 1, base.rectChildren.Count);
			this.maxItemsPerLine = Mathf.Clamp(this.maxItemsPerLine, 1, base.rectChildren.Count);
		}

		// Token: 0x06000141 RID: 321 RVA: 0x00007618 File Offset: 0x00005A18
		protected override void OnRectTransformDimensionsChange()
		{
			base.OnRectTransformDimensionsChange();
			LayoutRebuilder.MarkLayoutForRebuild((RectTransform)base.transform);
		}

		// Token: 0x04000085 RID: 133
		[Space]
		[Header("Item Inputs:")]
		[SerializeField]
		protected float itemSpacing;

		// Token: 0x04000086 RID: 134
		[Space]
		[SerializeField]
		protected float itemSizeMin;

		// Token: 0x04000087 RID: 135
		[SerializeField]
		protected float itemSizeMax;

		// Token: 0x04000088 RID: 136
		[Space]
		[Header("Flow Direction Options:")]
		[SerializeField]
		protected bool verticalFlow;

		// Token: 0x04000089 RID: 137
		[SerializeField]
		protected bool reverseFlow;

		// Token: 0x0400008A RID: 138
		private int maxItemsPerLine;

		// Token: 0x0400008B RID: 139
		private float maxItemsPerLineFloat;

		// Token: 0x0400008C RID: 140
		private int numLines;

		// Token: 0x0400008D RID: 141
		private float numLinesFloat;

		// Token: 0x0400008E RID: 142
		private float itemSize;
	}
}
