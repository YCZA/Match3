using UnityEngine;
using UnityEngine.UI;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000C06 RID: 3078
	[AddComponentMenu("UI/Extensions/Primitives/Cut Corners")]
	public class UICornerCut : UIPrimitiveBase
	{
		// Token: 0x17000A80 RID: 2688
		// (get) Token: 0x06004870 RID: 18544 RVA: 0x001722B1 File Offset: 0x001706B1
		// (set) Token: 0x06004871 RID: 18545 RVA: 0x001722B9 File Offset: 0x001706B9
		public bool CutUL
		{
			get
			{
				return this.m_cutUL;
			}
			set
			{
				this.m_cutUL = value;
				this.SetAllDirty();
			}
		}

		// Token: 0x17000A81 RID: 2689
		// (get) Token: 0x06004872 RID: 18546 RVA: 0x001722C8 File Offset: 0x001706C8
		// (set) Token: 0x06004873 RID: 18547 RVA: 0x001722D0 File Offset: 0x001706D0
		public bool CutUR
		{
			get
			{
				return this.m_cutUR;
			}
			set
			{
				this.m_cutUR = value;
				this.SetAllDirty();
			}
		}

		// Token: 0x17000A82 RID: 2690
		// (get) Token: 0x06004874 RID: 18548 RVA: 0x001722DF File Offset: 0x001706DF
		// (set) Token: 0x06004875 RID: 18549 RVA: 0x001722E7 File Offset: 0x001706E7
		public bool CutLL
		{
			get
			{
				return this.m_cutLL;
			}
			set
			{
				this.m_cutLL = value;
				this.SetAllDirty();
			}
		}

		// Token: 0x17000A83 RID: 2691
		// (get) Token: 0x06004876 RID: 18550 RVA: 0x001722F6 File Offset: 0x001706F6
		// (set) Token: 0x06004877 RID: 18551 RVA: 0x001722FE File Offset: 0x001706FE
		public bool CutLR
		{
			get
			{
				return this.m_cutLR;
			}
			set
			{
				this.m_cutLR = value;
				this.SetAllDirty();
			}
		}

		// Token: 0x17000A84 RID: 2692
		// (get) Token: 0x06004878 RID: 18552 RVA: 0x0017230D File Offset: 0x0017070D
		// (set) Token: 0x06004879 RID: 18553 RVA: 0x00172315 File Offset: 0x00170715
		public bool MakeColumns
		{
			get
			{
				return this.m_makeColumns;
			}
			set
			{
				this.m_makeColumns = value;
				this.SetAllDirty();
			}
		}

		// Token: 0x17000A85 RID: 2693
		// (get) Token: 0x0600487A RID: 18554 RVA: 0x00172324 File Offset: 0x00170724
		// (set) Token: 0x0600487B RID: 18555 RVA: 0x0017232C File Offset: 0x0017072C
		public bool UseColorUp
		{
			get
			{
				return this.m_useColorUp;
			}
			set
			{
				this.m_useColorUp = value;
			}
		}

		// Token: 0x17000A86 RID: 2694
		// (get) Token: 0x0600487C RID: 18556 RVA: 0x00172335 File Offset: 0x00170735
		// (set) Token: 0x0600487D RID: 18557 RVA: 0x0017233D File Offset: 0x0017073D
		public Color32 ColorUp
		{
			get
			{
				return this.m_colorUp;
			}
			set
			{
				this.m_colorUp = value;
			}
		}

		// Token: 0x17000A87 RID: 2695
		// (get) Token: 0x0600487E RID: 18558 RVA: 0x00172346 File Offset: 0x00170746
		// (set) Token: 0x0600487F RID: 18559 RVA: 0x0017234E File Offset: 0x0017074E
		public bool UseColorDown
		{
			get
			{
				return this.m_useColorDown;
			}
			set
			{
				this.m_useColorDown = value;
			}
		}

		// Token: 0x17000A88 RID: 2696
		// (get) Token: 0x06004880 RID: 18560 RVA: 0x00172357 File Offset: 0x00170757
		// (set) Token: 0x06004881 RID: 18561 RVA: 0x0017235F File Offset: 0x0017075F
		public Color32 ColorDown
		{
			get
			{
				return this.m_colorDown;
			}
			set
			{
				this.m_colorDown = value;
			}
		}

		// Token: 0x06004882 RID: 18562 RVA: 0x00172368 File Offset: 0x00170768
		protected override void OnPopulateMesh(VertexHelper vh)
		{
			Rect rect = base.rectTransform.rect;
			Rect rect2 = rect;
			Color32 color = this.color;
			bool flag = this.m_cutUL | this.m_cutUR;
			bool flag2 = this.m_cutLL | this.m_cutLR;
			bool flag3 = this.m_cutLL | this.m_cutUL;
			bool flag4 = this.m_cutLR | this.m_cutUR;
			bool flag5 = flag || flag2;
			if (flag5 && this.cornerSize.sqrMagnitude > 0f)
			{
				vh.Clear();
				if (flag3)
				{
					rect2.xMin += this.cornerSize.x;
				}
				if (flag2)
				{
					rect2.yMin += this.cornerSize.y;
				}
				if (flag)
				{
					rect2.yMax -= this.cornerSize.y;
				}
				if (flag4)
				{
					rect2.xMax -= this.cornerSize.x;
				}
				if (this.m_makeColumns)
				{
					Vector2 vector = new Vector2(rect.xMin, (!this.m_cutUL) ? rect.yMax : rect2.yMax);
					Vector2 vector2 = new Vector2(rect.xMax, (!this.m_cutUR) ? rect.yMax : rect2.yMax);
					Vector2 vector3 = new Vector2(rect.xMin, (!this.m_cutLL) ? rect.yMin : rect2.yMin);
					Vector2 vector4 = new Vector2(rect.xMax, (!this.m_cutLR) ? rect.yMin : rect2.yMin);
					if (flag3)
					{
						UICornerCut.AddSquare(vector3, vector, new Vector2(rect2.xMin, rect.yMax), new Vector2(rect2.xMin, rect.yMin), rect, (!this.m_useColorUp) ? color : this.m_colorUp, vh);
					}
					if (flag4)
					{
						UICornerCut.AddSquare(vector2, vector4, new Vector2(rect2.xMax, rect.yMin), new Vector2(rect2.xMax, rect.yMax), rect, (!this.m_useColorDown) ? color : this.m_colorDown, vh);
					}
				}
				else
				{
					Vector2 vector = new Vector2((!this.m_cutUL) ? rect.xMin : rect2.xMin, rect.yMax);
					Vector2 vector2 = new Vector2((!this.m_cutUR) ? rect.xMax : rect2.xMax, rect.yMax);
					Vector2 vector3 = new Vector2((!this.m_cutLL) ? rect.xMin : rect2.xMin, rect.yMin);
					Vector2 vector4 = new Vector2((!this.m_cutLR) ? rect.xMax : rect2.xMax, rect.yMin);
					if (flag2)
					{
						UICornerCut.AddSquare(vector4, vector3, new Vector2(rect.xMin, rect2.yMin), new Vector2(rect.xMax, rect2.yMin), rect, (!this.m_useColorDown) ? color : this.m_colorDown, vh);
					}
					if (flag)
					{
						UICornerCut.AddSquare(vector, vector2, new Vector2(rect.xMax, rect2.yMax), new Vector2(rect.xMin, rect2.yMax), rect, (!this.m_useColorUp) ? color : this.m_colorUp, vh);
					}
				}
				if (this.m_makeColumns)
				{
					UICornerCut.AddSquare(new Rect(rect2.xMin, rect.yMin, rect2.width, rect.height), rect, color, vh);
				}
				else
				{
					UICornerCut.AddSquare(new Rect(rect.xMin, rect2.yMin, rect.width, rect2.height), rect, color, vh);
				}
			}
		}

		// Token: 0x06004883 RID: 18563 RVA: 0x00172780 File Offset: 0x00170B80
		private static void AddSquare(Rect rect, Rect rectUV, Color32 color32, VertexHelper vh)
		{
			int num = UICornerCut.AddVert(rect.xMin, rect.yMin, rectUV, color32, vh);
			int idx = UICornerCut.AddVert(rect.xMin, rect.yMax, rectUV, color32, vh);
			int num2 = UICornerCut.AddVert(rect.xMax, rect.yMax, rectUV, color32, vh);
			int idx2 = UICornerCut.AddVert(rect.xMax, rect.yMin, rectUV, color32, vh);
			vh.AddTriangle(num, idx, num2);
			vh.AddTriangle(num2, idx2, num);
		}

		// Token: 0x06004884 RID: 18564 RVA: 0x001727FC File Offset: 0x00170BFC
		private static void AddSquare(Vector2 a, Vector2 b, Vector2 c, Vector2 d, Rect rectUV, Color32 color32, VertexHelper vh)
		{
			int num = UICornerCut.AddVert(a.x, a.y, rectUV, color32, vh);
			int idx = UICornerCut.AddVert(b.x, b.y, rectUV, color32, vh);
			int num2 = UICornerCut.AddVert(c.x, c.y, rectUV, color32, vh);
			int idx2 = UICornerCut.AddVert(d.x, d.y, rectUV, color32, vh);
			vh.AddTriangle(num, idx, num2);
			vh.AddTriangle(num2, idx2, num);
		}

		// Token: 0x06004885 RID: 18565 RVA: 0x00172888 File Offset: 0x00170C88
		private static int AddVert(float x, float y, Rect area, Color32 color32, VertexHelper vh)
		{
			Vector2 uv = new Vector2(Mathf.InverseLerp(area.xMin, area.xMax, x), Mathf.InverseLerp(area.yMin, area.yMax, y));
			vh.AddVert(new Vector3(x, y), color32, uv);
			return vh.currentVertCount - 1;
		}

		// Token: 0x04006F23 RID: 28451
		public Vector2 cornerSize = new Vector2(16f, 16f);

		// Token: 0x04006F24 RID: 28452
		[Header("Corners to cut")]
		[SerializeField]
		private bool m_cutUL = true;

		// Token: 0x04006F25 RID: 28453
		[SerializeField]
		private bool m_cutUR;

		// Token: 0x04006F26 RID: 28454
		[SerializeField]
		private bool m_cutLL;

		// Token: 0x04006F27 RID: 28455
		[SerializeField]
		private bool m_cutLR;

		// Token: 0x04006F28 RID: 28456
		[Tooltip("Up-Down colors become Left-Right colors")]
		[SerializeField]
		private bool m_makeColumns;

		// Token: 0x04006F29 RID: 28457
		[Header("Color the cut bars differently")]
		[SerializeField]
		private bool m_useColorUp;

		// Token: 0x04006F2A RID: 28458
		[SerializeField]
		private Color32 m_colorUp;

		// Token: 0x04006F2B RID: 28459
		[SerializeField]
		private bool m_useColorDown;

		// Token: 0x04006F2C RID: 28460
		[SerializeField]
		private Color32 m_colorDown;
	}
}
