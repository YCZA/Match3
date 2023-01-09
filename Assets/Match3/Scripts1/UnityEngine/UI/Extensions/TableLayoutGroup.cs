using UnityEngine;
using UnityEngine.UI;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000BFB RID: 3067
	[AddComponentMenu("Layout/Extensions/Table Layout Group")]
	public class TableLayoutGroup : LayoutGroup
	{
		// Token: 0x17000A6D RID: 2669
		// (get) Token: 0x06004803 RID: 18435 RVA: 0x0016FA6B File Offset: 0x0016DE6B
		// (set) Token: 0x06004804 RID: 18436 RVA: 0x0016FA73 File Offset: 0x0016DE73
		public TableLayoutGroup.Corner StartCorner
		{
			get
			{
				return this.startCorner;
			}
			set
			{
				base.SetProperty<TableLayoutGroup.Corner>(ref this.startCorner, value);
			}
		}

		// Token: 0x17000A6E RID: 2670
		// (get) Token: 0x06004805 RID: 18437 RVA: 0x0016FA82 File Offset: 0x0016DE82
		// (set) Token: 0x06004806 RID: 18438 RVA: 0x0016FA8A File Offset: 0x0016DE8A
		public float[] ColumnWidths
		{
			get
			{
				return this.columnWidths;
			}
			set
			{
				base.SetProperty<float[]>(ref this.columnWidths, value);
			}
		}

		// Token: 0x17000A6F RID: 2671
		// (get) Token: 0x06004807 RID: 18439 RVA: 0x0016FA99 File Offset: 0x0016DE99
		// (set) Token: 0x06004808 RID: 18440 RVA: 0x0016FAA1 File Offset: 0x0016DEA1
		public float MinimumRowHeight
		{
			get
			{
				return this.minimumRowHeight;
			}
			set
			{
				base.SetProperty<float>(ref this.minimumRowHeight, value);
			}
		}

		// Token: 0x17000A70 RID: 2672
		// (get) Token: 0x06004809 RID: 18441 RVA: 0x0016FAB0 File Offset: 0x0016DEB0
		// (set) Token: 0x0600480A RID: 18442 RVA: 0x0016FAB8 File Offset: 0x0016DEB8
		public bool FlexibleRowHeight
		{
			get
			{
				return this.flexibleRowHeight;
			}
			set
			{
				base.SetProperty<bool>(ref this.flexibleRowHeight, value);
			}
		}

		// Token: 0x17000A71 RID: 2673
		// (get) Token: 0x0600480B RID: 18443 RVA: 0x0016FAC7 File Offset: 0x0016DEC7
		// (set) Token: 0x0600480C RID: 18444 RVA: 0x0016FACF File Offset: 0x0016DECF
		public float ColumnSpacing
		{
			get
			{
				return this.columnSpacing;
			}
			set
			{
				base.SetProperty<float>(ref this.columnSpacing, value);
			}
		}

		// Token: 0x17000A72 RID: 2674
		// (get) Token: 0x0600480D RID: 18445 RVA: 0x0016FADE File Offset: 0x0016DEDE
		// (set) Token: 0x0600480E RID: 18446 RVA: 0x0016FAE6 File Offset: 0x0016DEE6
		public float RowSpacing
		{
			get
			{
				return this.rowSpacing;
			}
			set
			{
				base.SetProperty<float>(ref this.rowSpacing, value);
			}
		}

		// Token: 0x0600480F RID: 18447 RVA: 0x0016FAF8 File Offset: 0x0016DEF8
		public override void CalculateLayoutInputHorizontal()
		{
			base.CalculateLayoutInputHorizontal();
			float num = (float)base.padding.horizontal;
			int num2 = Mathf.Min(base.rectChildren.Count, this.columnWidths.Length);
			for (int i = 0; i < num2; i++)
			{
				num += this.columnWidths[i];
				num += this.columnSpacing;
			}
			num -= this.columnSpacing;
			base.SetLayoutInputForAxis(num, num, 0f, 0);
		}

		// Token: 0x06004810 RID: 18448 RVA: 0x0016FB70 File Offset: 0x0016DF70
		public override void CalculateLayoutInputVertical()
		{
			int num = this.columnWidths.Length;
			int num2 = Mathf.CeilToInt((float)base.rectChildren.Count / (float)num);
			this.preferredRowHeights = new float[num2];
			float num3 = (float)base.padding.vertical;
			float num4 = (float)base.padding.vertical;
			if (num2 > 1)
			{
				float num5 = (float)(num2 - 1) * this.rowSpacing;
				num3 += num5;
				num4 += num5;
			}
			if (this.flexibleRowHeight)
			{
				for (int i = 0; i < num2; i++)
				{
					float num6 = this.minimumRowHeight;
					float num7 = this.minimumRowHeight;
					for (int j = 0; j < num; j++)
					{
						int num8 = i * num + j;
						if (num8 == base.rectChildren.Count)
						{
							break;
						}
						num7 = Mathf.Max(LayoutUtility.GetPreferredHeight(base.rectChildren[num8]), num7);
						num6 = Mathf.Max(LayoutUtility.GetMinHeight(base.rectChildren[num8]), num6);
					}
					num3 += num6;
					num4 += num7;
					this.preferredRowHeights[i] = num7;
				}
			}
			else
			{
				for (int k = 0; k < num2; k++)
				{
					this.preferredRowHeights[k] = this.minimumRowHeight;
				}
				num3 += (float)num2 * this.minimumRowHeight;
				num4 = num3;
			}
			num4 = Mathf.Max(num3, num4);
			base.SetLayoutInputForAxis(num3, num4, 1f, 1);
		}

		// Token: 0x06004811 RID: 18449 RVA: 0x0016FCF0 File Offset: 0x0016E0F0
		public override void SetLayoutHorizontal()
		{
			if (this.columnWidths.Length == 0)
			{
				this.columnWidths = new float[1];
			}
			int num = this.columnWidths.Length;
			int num2 = ((int)this.startCorner % (int)TableLayoutGroup.Corner.LowerLeft);
			float num3 = 0f;
			int num4 = Mathf.Min(base.rectChildren.Count, this.columnWidths.Length);
			for (int i = 0; i < num4; i++)
			{
				num3 += this.columnWidths[i];
				num3 += this.columnSpacing;
			}
			num3 -= this.columnSpacing;
			float num5 = base.GetStartOffset(0, num3);
			if (num2 == 1)
			{
				num5 += num3;
			}
			float num6 = num5;
			for (int j = 0; j < base.rectChildren.Count; j++)
			{
				int num7 = j % num;
				if (num7 == 0)
				{
					num6 = num5;
				}
				if (num2 == 1)
				{
					num6 -= this.columnWidths[num7];
				}
				base.SetChildAlongAxis(base.rectChildren[j], 0, num6, this.columnWidths[num7]);
				if (num2 == 1)
				{
					num6 -= this.columnSpacing;
				}
				else
				{
					num6 += this.columnWidths[num7] + this.columnSpacing;
				}
			}
		}

		// Token: 0x06004812 RID: 18450 RVA: 0x0016FE2C File Offset: 0x0016E22C
		public override void SetLayoutVertical()
		{
			int num = this.columnWidths.Length;
			int num2 = this.preferredRowHeights.Length;
			int num3 = ((int)this.startCorner / (int)TableLayoutGroup.Corner.LowerLeft);
			float num4 = 0f;
			for (int i = 0; i < num2; i++)
			{
				num4 += this.preferredRowHeights[i];
			}
			if (num2 > 1)
			{
				num4 += (float)(num2 - 1) * this.rowSpacing;
			}
			float num5 = base.GetStartOffset(1, num4);
			if (num3 == 1)
			{
				num5 += num4;
			}
			float num6 = num5;
			for (int j = 0; j < num2; j++)
			{
				if (num3 == 1)
				{
					num6 -= this.preferredRowHeights[j];
				}
				for (int k = 0; k < num; k++)
				{
					int num7 = j * num + k;
					if (num7 == base.rectChildren.Count)
					{
						break;
					}
					base.SetChildAlongAxis(base.rectChildren[num7], 1, num6, this.preferredRowHeights[j]);
				}
				if (num3 == 1)
				{
					num6 -= this.rowSpacing;
				}
				else
				{
					num6 += this.preferredRowHeights[j] + this.rowSpacing;
				}
			}
			this.preferredRowHeights = null;
		}

		// Token: 0x04006EF6 RID: 28406
		[SerializeField]
		protected TableLayoutGroup.Corner startCorner;

		// Token: 0x04006EF7 RID: 28407
		[SerializeField]
		protected float[] columnWidths = new float[]
		{
			96f
		};

		// Token: 0x04006EF8 RID: 28408
		[SerializeField]
		protected float minimumRowHeight = 32f;

		// Token: 0x04006EF9 RID: 28409
		[SerializeField]
		protected bool flexibleRowHeight = true;

		// Token: 0x04006EFA RID: 28410
		[SerializeField]
		protected float columnSpacing;

		// Token: 0x04006EFB RID: 28411
		[SerializeField]
		protected float rowSpacing;

		// Token: 0x04006EFC RID: 28412
		private float[] preferredRowHeights;

		// Token: 0x02000BFC RID: 3068
		public enum Corner
		{
			// Token: 0x04006EFE RID: 28414
			UpperLeft,
			// Token: 0x04006EFF RID: 28415
			UpperRight,
			// Token: 0x04006F00 RID: 28416
			LowerLeft,
			// Token: 0x04006F01 RID: 28417
			LowerRight
		}
	}
}
