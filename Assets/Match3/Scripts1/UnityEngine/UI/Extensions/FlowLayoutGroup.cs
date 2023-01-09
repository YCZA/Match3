using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000BE9 RID: 3049
	[AddComponentMenu("Layout/Extensions/Flow Layout Group")]
	public class FlowLayoutGroup : LayoutGroup
	{
		// Token: 0x06004780 RID: 18304 RVA: 0x0016CAAC File Offset: 0x0016AEAC
		public override void CalculateLayoutInputHorizontal()
		{
			base.CalculateLayoutInputHorizontal();
			float totalMin = this.GetGreatestMinimumChildWidth() + (float)base.padding.left + (float)base.padding.right;
			base.SetLayoutInputForAxis(totalMin, -1f, -1f, 0);
		}

		// Token: 0x06004781 RID: 18305 RVA: 0x0016CAF4 File Offset: 0x0016AEF4
		public override void SetLayoutHorizontal()
		{
			this.SetLayout(base.rectTransform.rect.width, 0, false);
		}

		// Token: 0x06004782 RID: 18306 RVA: 0x0016CB20 File Offset: 0x0016AF20
		public override void SetLayoutVertical()
		{
			this.SetLayout(base.rectTransform.rect.width, 1, false);
		}

		// Token: 0x06004783 RID: 18307 RVA: 0x0016CB4C File Offset: 0x0016AF4C
		public override void CalculateLayoutInputVertical()
		{
			this._layoutHeight = this.SetLayout(base.rectTransform.rect.width, 1, true);
		}

		// Token: 0x17000A65 RID: 2661
		// (get) Token: 0x06004784 RID: 18308 RVA: 0x0016CB7A File Offset: 0x0016AF7A
		protected bool IsCenterAlign
		{
			get
			{
				return base.childAlignment == TextAnchor.LowerCenter || base.childAlignment == TextAnchor.MiddleCenter || base.childAlignment == TextAnchor.UpperCenter;
			}
		}

		// Token: 0x17000A66 RID: 2662
		// (get) Token: 0x06004785 RID: 18309 RVA: 0x0016CBA0 File Offset: 0x0016AFA0
		protected bool IsRightAlign
		{
			get
			{
				return base.childAlignment == TextAnchor.LowerRight || base.childAlignment == TextAnchor.MiddleRight || base.childAlignment == TextAnchor.UpperRight;
			}
		}

		// Token: 0x17000A67 RID: 2663
		// (get) Token: 0x06004786 RID: 18310 RVA: 0x0016CBC6 File Offset: 0x0016AFC6
		protected bool IsMiddleAlign
		{
			get
			{
				return base.childAlignment == TextAnchor.MiddleLeft || base.childAlignment == TextAnchor.MiddleRight || base.childAlignment == TextAnchor.MiddleCenter;
			}
		}

		// Token: 0x17000A68 RID: 2664
		// (get) Token: 0x06004787 RID: 18311 RVA: 0x0016CBEC File Offset: 0x0016AFEC
		protected bool IsLowerAlign
		{
			get
			{
				return base.childAlignment == TextAnchor.LowerLeft || base.childAlignment == TextAnchor.LowerRight || base.childAlignment == TextAnchor.LowerCenter;
			}
		}

		// Token: 0x06004788 RID: 18312 RVA: 0x0016CC14 File Offset: 0x0016B014
		public float SetLayout(float width, int axis, bool layoutInput)
		{
			float height = base.rectTransform.rect.height;
			float num = base.rectTransform.rect.width - (float)base.padding.left - (float)base.padding.right;
			float num2 = (!this.IsLowerAlign) ? ((float)base.padding.top) : ((float)base.padding.bottom);
			float num3 = 0f;
			float num4 = 0f;
			for (int i = 0; i < base.rectChildren.Count; i++)
			{
				int index = (!this.IsLowerAlign) ? i : (base.rectChildren.Count - 1 - i);
				RectTransform rectTransform = base.rectChildren[index];
				float num5 = LayoutUtility.GetPreferredSize(rectTransform, 0);
				float preferredSize = LayoutUtility.GetPreferredSize(rectTransform, 1);
				num5 = Mathf.Min(num5, num);
				if (num3 + num5 > num)
				{
					num3 -= this.SpacingX;
					if (!layoutInput)
					{
						float yOffset = this.CalculateRowVerticalOffset(height, num2, num4);
						this.LayoutRow(this._rowList, num3, num4, num, (float)base.padding.left, yOffset, axis);
					}
					this._rowList.Clear();
					num2 += num4;
					num2 += this.SpacingY;
					num4 = 0f;
					num3 = 0f;
				}
				num3 += num5;
				this._rowList.Add(rectTransform);
				if (preferredSize > num4)
				{
					num4 = preferredSize;
				}
				if (i < base.rectChildren.Count - 1)
				{
					num3 += this.SpacingX;
				}
			}
			if (!layoutInput)
			{
				float yOffset2 = this.CalculateRowVerticalOffset(height, num2, num4);
				num3 -= this.SpacingX;
				this.LayoutRow(this._rowList, num3, num4, num - ((this._rowList.Count <= 1) ? 0f : this.SpacingX), (float)base.padding.left, yOffset2, axis);
			}
			this._rowList.Clear();
			num2 += num4;
			num2 += (float)((!this.IsLowerAlign) ? base.padding.bottom : base.padding.top);
			if (layoutInput && axis == 1)
			{
				base.SetLayoutInputForAxis(num2, num2, -1f, axis);
			}
			return num2;
		}

		// Token: 0x06004789 RID: 18313 RVA: 0x0016CE80 File Offset: 0x0016B280
		private float CalculateRowVerticalOffset(float groupHeight, float yOffset, float currentRowHeight)
		{
			float result;
			if (this.IsLowerAlign)
			{
				result = groupHeight - yOffset - currentRowHeight;
			}
			else if (this.IsMiddleAlign)
			{
				result = groupHeight * 0.5f - this._layoutHeight * 0.5f + yOffset;
			}
			else
			{
				result = yOffset;
			}
			return result;
		}

		// Token: 0x0600478A RID: 18314 RVA: 0x0016CED0 File Offset: 0x0016B2D0
		protected void LayoutRow(IList<RectTransform> contents, float rowWidth, float rowHeight, float maxWidth, float xOffset, float yOffset, int axis)
		{
			float num = xOffset;
			if (!this.ChildForceExpandWidth && this.IsCenterAlign)
			{
				num += (maxWidth - rowWidth) * 0.5f;
			}
			else if (!this.ChildForceExpandWidth && this.IsRightAlign)
			{
				num += maxWidth - rowWidth;
			}
			float num2 = 0f;
			float num3 = 0f;
			if (this.ChildForceExpandWidth)
			{
				num2 = (maxWidth - rowWidth) / (float)this._rowList.Count;
			}
			else if (this.ExpandHorizontalSpacing)
			{
				num3 = (maxWidth - rowWidth) / (float)(this._rowList.Count - 1);
				if (this._rowList.Count > 1)
				{
					if (this.IsCenterAlign)
					{
						num -= num3 * 0.5f * (float)(this._rowList.Count - 1);
					}
					else if (this.IsRightAlign)
					{
						num -= num3 * (float)(this._rowList.Count - 1);
					}
				}
			}
			for (int i = 0; i < this._rowList.Count; i++)
			{
				int index = (!this.IsLowerAlign) ? i : (this._rowList.Count - 1 - i);
				RectTransform rect = this._rowList[index];
				float num4 = LayoutUtility.GetPreferredSize(rect, 0) + num2;
				float num5 = LayoutUtility.GetPreferredSize(rect, 1);
				if (this.ChildForceExpandHeight)
				{
					num5 = rowHeight;
				}
				num4 = Mathf.Min(num4, maxWidth);
				float num6 = yOffset;
				if (this.IsMiddleAlign)
				{
					num6 += (rowHeight - num5) * 0.5f;
				}
				else if (this.IsLowerAlign)
				{
					num6 += rowHeight - num5;
				}
				if (this.ExpandHorizontalSpacing && i > 0)
				{
					num += num3;
				}
				if (axis == 0)
				{
					base.SetChildAlongAxis(rect, 0, num, num4);
				}
				else
				{
					base.SetChildAlongAxis(rect, 1, num6, num5);
				}
				if (i < this._rowList.Count - 1)
				{
					num += num4 + this.SpacingX;
				}
			}
		}

		// Token: 0x0600478B RID: 18315 RVA: 0x0016D0D8 File Offset: 0x0016B4D8
		public float GetGreatestMinimumChildWidth()
		{
			float num = 0f;
			for (int i = 0; i < base.rectChildren.Count; i++)
			{
				float minWidth = LayoutUtility.GetMinWidth(base.rectChildren[i]);
				num = Mathf.Max(minWidth, num);
			}
			return num;
		}

		// Token: 0x04006E81 RID: 28289
		public float SpacingX;

		// Token: 0x04006E82 RID: 28290
		public float SpacingY;

		// Token: 0x04006E83 RID: 28291
		public bool ExpandHorizontalSpacing;

		// Token: 0x04006E84 RID: 28292
		public bool ChildForceExpandWidth;

		// Token: 0x04006E85 RID: 28293
		public bool ChildForceExpandHeight;

		// Token: 0x04006E86 RID: 28294
		private float _layoutHeight;

		// Token: 0x04006E87 RID: 28295
		private readonly IList<RectTransform> _rowList = new List<RectTransform>();
	}
}
