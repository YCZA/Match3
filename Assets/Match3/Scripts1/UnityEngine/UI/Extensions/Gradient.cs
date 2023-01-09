using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000BCC RID: 3020
	[AddComponentMenu("UI/Effects/Extensions/Gradient")]
	public class Gradient : BaseMeshEffect
	{
		// Token: 0x17000A4A RID: 2634
		// (get) Token: 0x060046E3 RID: 18147 RVA: 0x001686C6 File Offset: 0x00166AC6
		// (set) Token: 0x060046E4 RID: 18148 RVA: 0x001686CE File Offset: 0x00166ACE
		public GradientMode GradientMode
		{
			get
			{
				return this._gradientMode;
			}
			set
			{
				this._gradientMode = value;
				base.graphic.SetVerticesDirty();
			}
		}

		// Token: 0x17000A4B RID: 2635
		// (get) Token: 0x060046E5 RID: 18149 RVA: 0x001686E2 File Offset: 0x00166AE2
		// (set) Token: 0x060046E6 RID: 18150 RVA: 0x001686EA File Offset: 0x00166AEA
		public GradientDir GradientDir
		{
			get
			{
				return this._gradientDir;
			}
			set
			{
				this._gradientDir = value;
				base.graphic.SetVerticesDirty();
			}
		}

		// Token: 0x17000A4C RID: 2636
		// (get) Token: 0x060046E7 RID: 18151 RVA: 0x001686FE File Offset: 0x00166AFE
		// (set) Token: 0x060046E8 RID: 18152 RVA: 0x00168706 File Offset: 0x00166B06
		public bool OverwriteAllColor
		{
			get
			{
				return this._overwriteAllColor;
			}
			set
			{
				this._overwriteAllColor = value;
				base.graphic.SetVerticesDirty();
			}
		}

		// Token: 0x17000A4D RID: 2637
		// (get) Token: 0x060046E9 RID: 18153 RVA: 0x0016871A File Offset: 0x00166B1A
		// (set) Token: 0x060046EA RID: 18154 RVA: 0x00168722 File Offset: 0x00166B22
		public Color Vertex1
		{
			get
			{
				return this._vertex1;
			}
			set
			{
				this._vertex1 = value;
				base.graphic.SetAllDirty();
			}
		}

		// Token: 0x17000A4E RID: 2638
		// (get) Token: 0x060046EB RID: 18155 RVA: 0x00168736 File Offset: 0x00166B36
		// (set) Token: 0x060046EC RID: 18156 RVA: 0x0016873E File Offset: 0x00166B3E
		public Color Vertex2
		{
			get
			{
				return this._vertex2;
			}
			set
			{
				this._vertex2 = value;
				base.graphic.SetAllDirty();
			}
		}

		// Token: 0x060046ED RID: 18157 RVA: 0x00168752 File Offset: 0x00166B52
		protected override void Awake()
		{
			this.targetGraphic = base.GetComponent<Graphic>();
		}

		// Token: 0x060046EE RID: 18158 RVA: 0x00168760 File Offset: 0x00166B60
		public override void ModifyMesh(VertexHelper vh)
		{
			int currentVertCount = vh.currentVertCount;
			if (!this.IsActive() || currentVertCount == 0)
			{
				return;
			}
			List<UIVertex> list = new List<UIVertex>();
			vh.GetUIVertexStream(list);
			UIVertex vertex = default(UIVertex);
			if (this._gradientMode == GradientMode.Global)
			{
				if (this._gradientDir == GradientDir.DiagonalLeftToRight || this._gradientDir == GradientDir.DiagonalRightToLeft)
				{
					this._gradientDir = GradientDir.Vertical;
				}
				float num = (this._gradientDir != GradientDir.Vertical) ? list[list.Count - 1].position.x : list[list.Count - 1].position.y;
				float num2 = (this._gradientDir != GradientDir.Vertical) ? list[0].position.x : list[0].position.y;
				float num3 = num2 - num;
				for (int i = 0; i < currentVertCount; i++)
				{
					vh.PopulateUIVertex(ref vertex, i);
					if (this._overwriteAllColor || !(vertex.color != this.targetGraphic.color))
					{
						vertex.color *= Color.Lerp(this._vertex2, this._vertex1, (((this._gradientDir != GradientDir.Vertical) ? vertex.position.x : vertex.position.y) - num) / num3);
						vh.SetUIVertex(vertex, i);
					}
				}
			}
			else
			{
				for (int j = 0; j < currentVertCount; j++)
				{
					vh.PopulateUIVertex(ref vertex, j);
					if (this._overwriteAllColor || this.CompareCarefully(vertex.color, this.targetGraphic.color))
					{
						switch (this._gradientDir)
						{
						case GradientDir.Vertical:
							vertex.color *= ((j % 4 != 0 && (j - 1) % 4 != 0) ? this._vertex2 : this._vertex1);
							break;
						case GradientDir.Horizontal:
							vertex.color *= ((j % 4 != 0 && (j - 3) % 4 != 0) ? this._vertex2 : this._vertex1);
							break;
						case GradientDir.DiagonalLeftToRight:
							vertex.color *= ((j % 4 != 0) ? (((j - 2) % 4 != 0) ? Color.Lerp(this._vertex2, this._vertex1, 0.5f) : this._vertex2) : this._vertex1);
							break;
						case GradientDir.DiagonalRightToLeft:
							vertex.color *= (((j - 1) % 4 != 0) ? (((j - 3) % 4 != 0) ? Color.Lerp(this._vertex2, this._vertex1, 0.5f) : this._vertex2) : this._vertex1);
							break;
						}
						vh.SetUIVertex(vertex, j);
					}
				}
			}
		}

		// Token: 0x060046EF RID: 18159 RVA: 0x00168AD8 File Offset: 0x00166ED8
		private bool CompareCarefully(Color col1, Color col2)
		{
			return Mathf.Abs(col1.r - col2.r) < 0.003f && Mathf.Abs(col1.g - col2.g) < 0.003f && Mathf.Abs(col1.b - col2.b) < 0.003f && Mathf.Abs(col1.a - col2.a) < 0.003f;
		}

		// Token: 0x04006E14 RID: 28180
		[SerializeField]
		private GradientMode _gradientMode;

		// Token: 0x04006E15 RID: 28181
		[SerializeField]
		private GradientDir _gradientDir;

		// Token: 0x04006E16 RID: 28182
		[SerializeField]
		private bool _overwriteAllColor;

		// Token: 0x04006E17 RID: 28183
		[SerializeField]
		private Color _vertex1 = Color.white;

		// Token: 0x04006E18 RID: 28184
		[SerializeField]
		private Color _vertex2 = Color.black;

		// Token: 0x04006E19 RID: 28185
		private Graphic targetGraphic;
	}
}
