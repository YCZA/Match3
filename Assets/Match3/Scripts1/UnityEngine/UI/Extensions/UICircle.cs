using UnityEngine;
using UnityEngine.UI;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000C05 RID: 3077
	[AddComponentMenu("UI/Extensions/Primitives/UI Circle")]
	public class UICircle : UIPrimitiveBase
	{
		// Token: 0x17000A7C RID: 2684
		// (get) Token: 0x06004864 RID: 18532 RVA: 0x00171D8E File Offset: 0x0017018E
		// (set) Token: 0x06004865 RID: 18533 RVA: 0x00171D96 File Offset: 0x00170196
		public int FillPercent
		{
			get
			{
				return this.m_fillPercent;
			}
			set
			{
				this.m_fillPercent = value;
				this.SetAllDirty();
			}
		}

		// Token: 0x17000A7D RID: 2685
		// (get) Token: 0x06004866 RID: 18534 RVA: 0x00171DA5 File Offset: 0x001701A5
		// (set) Token: 0x06004867 RID: 18535 RVA: 0x00171DAD File Offset: 0x001701AD
		public bool Fill
		{
			get
			{
				return this.m_fill;
			}
			set
			{
				this.m_fill = value;
				this.SetAllDirty();
			}
		}

		// Token: 0x17000A7E RID: 2686
		// (get) Token: 0x06004868 RID: 18536 RVA: 0x00171DBC File Offset: 0x001701BC
		// (set) Token: 0x06004869 RID: 18537 RVA: 0x00171DC4 File Offset: 0x001701C4
		public float Thickness
		{
			get
			{
				return this.m_thickness;
			}
			set
			{
				this.m_thickness = value;
				this.SetAllDirty();
			}
		}

		// Token: 0x0600486A RID: 18538 RVA: 0x00171DD4 File Offset: 0x001701D4
		private void Update()
		{
			this.m_thickness = Mathf.Clamp(this.m_thickness, 0f, base.rectTransform.rect.width / 2f);
		}

		// Token: 0x17000A7F RID: 2687
		// (get) Token: 0x0600486B RID: 18539 RVA: 0x00171E11 File Offset: 0x00170211
		// (set) Token: 0x0600486C RID: 18540 RVA: 0x00171E19 File Offset: 0x00170219
		public int Segments
		{
			get
			{
				return this.m_segments;
			}
			set
			{
				this.m_segments = value;
				this.SetAllDirty();
			}
		}

		// Token: 0x0600486D RID: 18541 RVA: 0x00171E28 File Offset: 0x00170228
		protected override void OnPopulateMesh(VertexHelper vh)
		{
			float outer = -base.rectTransform.pivot.x * base.rectTransform.rect.width;
			float inner = -base.rectTransform.pivot.x * base.rectTransform.rect.width + this.m_thickness;
			vh.Clear();
			Vector2 zero = Vector2.zero;
			Vector2 zero2 = Vector2.zero;
			Vector2 vector = new Vector2(0f, 0f);
			Vector2 vector2 = new Vector2(0f, 1f);
			Vector2 vector3 = new Vector2(1f, 1f);
			Vector2 vector4 = new Vector2(1f, 0f);
			if (this.FixedToSegments)
			{
				float num = (float)this.m_fillPercent / 100f;
				float num2 = 360f / (float)this.m_segments;
				int num3 = (int)((float)(this.m_segments + 1) * num);
				for (int i = 0; i < num3; i++)
				{
					float f = 0.017453292f * ((float)i * num2);
					float c = Mathf.Cos(f);
					float s = Mathf.Sin(f);
					vector = new Vector2(0f, 1f);
					vector2 = new Vector2(1f, 1f);
					vector3 = new Vector2(1f, 0f);
					vector4 = new Vector2(0f, 0f);
					Vector2 vector5;
					Vector2 vector6;
					Vector2 vector7;
					Vector2 vector8;
					this.StepThroughPointsAndFill(outer, inner, ref zero, ref zero2, out vector5, out vector6, out vector7, out vector8, c, s);
					vh.AddUIVertexQuad(base.SetVbo(new Vector2[]
					{
						vector5,
						vector6,
						vector7,
						vector8
					}, new Vector2[]
					{
						vector,
						vector2,
						vector3,
						vector4
					}));
				}
			}
			else
			{
				float width = base.rectTransform.rect.width;
				float height = base.rectTransform.rect.height;
				float num4 = (float)this.m_fillPercent / 100f * 6.2831855f / (float)this.m_segments;
				float num5 = 0f;
				for (int j = 0; j < this.m_segments + 1; j++)
				{
					float c2 = Mathf.Cos(num5);
					float s2 = Mathf.Sin(num5);
					Vector2 vector5;
					Vector2 vector6;
					Vector2 vector7;
					Vector2 vector8;
					this.StepThroughPointsAndFill(outer, inner, ref zero, ref zero2, out vector5, out vector6, out vector7, out vector8, c2, s2);
					vector = new Vector2(vector5.x / width + 0.5f, vector5.y / height + 0.5f);
					vector2 = new Vector2(vector6.x / width + 0.5f, vector6.y / height + 0.5f);
					vector3 = new Vector2(vector7.x / width + 0.5f, vector7.y / height + 0.5f);
					vector4 = new Vector2(vector8.x / width + 0.5f, vector8.y / height + 0.5f);
					vh.AddUIVertexQuad(base.SetVbo(new Vector2[]
					{
						vector5,
						vector6,
						vector7,
						vector8
					}, new Vector2[]
					{
						vector,
						vector2,
						vector3,
						vector4
					}));
					num5 += num4;
				}
			}
		}

		// Token: 0x0600486E RID: 18542 RVA: 0x00172204 File Offset: 0x00170604
		private void StepThroughPointsAndFill(float outer, float inner, ref Vector2 prevX, ref Vector2 prevY, out Vector2 pos0, out Vector2 pos1, out Vector2 pos2, out Vector2 pos3, float c, float s)
		{
			pos0 = prevX;
			pos1 = new Vector2(outer * c, outer * s);
			if (this.m_fill)
			{
				pos2 = Vector2.zero;
				pos3 = Vector2.zero;
			}
			else
			{
				pos2 = new Vector2(inner * c, inner * s);
				pos3 = prevY;
			}
			prevX = pos1;
			prevY = pos2;
		}

		// Token: 0x04006F1E RID: 28446
		[Tooltip("The circular fill percentage of the primitive, affected by FixedToSegments")]
		[Range(0f, 100f)]
		[SerializeField]
		private int m_fillPercent = 100;

		// Token: 0x04006F1F RID: 28447
		[Tooltip("Should the primitive fill draw by segments or absolute percentage")]
		public bool FixedToSegments;

		// Token: 0x04006F20 RID: 28448
		[Tooltip("Draw the primitive filled or as a line")]
		[SerializeField]
		private bool m_fill = true;

		// Token: 0x04006F21 RID: 28449
		[Tooltip("If not filled, the thickness of the primitive line")]
		[SerializeField]
		private float m_thickness = 5f;

		// Token: 0x04006F22 RID: 28450
		[Tooltip("The number of segments to draw the primitive, more segments = smoother primitive")]
		[Range(0f, 360f)]
		[SerializeField]
		private int m_segments = 360;
	}
}
