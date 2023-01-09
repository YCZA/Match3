using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Sprites;
using UnityEngine.UI;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000C08 RID: 3080
	[AddComponentMenu("UI/Extensions/Primitives/UILineRenderer")]
	[RequireComponent(typeof(RectTransform))]
	public class UILineRenderer : UIPrimitiveBase
	{
		// Token: 0x17000A8B RID: 2699
		// (get) Token: 0x0600488D RID: 18573 RVA: 0x001728F8 File Offset: 0x00170CF8
		// (set) Token: 0x0600488E RID: 18574 RVA: 0x00172900 File Offset: 0x00170D00
		public float LineThickness
		{
			get
			{
				return this.lineThickness;
			}
			set
			{
				this.lineThickness = value;
				this.SetAllDirty();
			}
		}

		// Token: 0x17000A8C RID: 2700
		// (get) Token: 0x0600488F RID: 18575 RVA: 0x0017290F File Offset: 0x00170D0F
		// (set) Token: 0x06004890 RID: 18576 RVA: 0x00172917 File Offset: 0x00170D17
		public bool RelativeSize
		{
			get
			{
				return this.relativeSize;
			}
			set
			{
				this.relativeSize = value;
				this.SetAllDirty();
			}
		}

		// Token: 0x17000A8D RID: 2701
		// (get) Token: 0x06004891 RID: 18577 RVA: 0x00172926 File Offset: 0x00170D26
		// (set) Token: 0x06004892 RID: 18578 RVA: 0x0017292E File Offset: 0x00170D2E
		public bool LineList
		{
			get
			{
				return this.lineList;
			}
			set
			{
				this.lineList = value;
				this.SetAllDirty();
			}
		}

		// Token: 0x17000A8E RID: 2702
		// (get) Token: 0x06004893 RID: 18579 RVA: 0x0017293D File Offset: 0x00170D3D
		// (set) Token: 0x06004894 RID: 18580 RVA: 0x00172945 File Offset: 0x00170D45
		public bool LineCaps
		{
			get
			{
				return this.lineCaps;
			}
			set
			{
				this.lineCaps = value;
				this.SetAllDirty();
			}
		}

		// Token: 0x17000A8F RID: 2703
		// (get) Token: 0x06004895 RID: 18581 RVA: 0x00172954 File Offset: 0x00170D54
		// (set) Token: 0x06004896 RID: 18582 RVA: 0x0017295C File Offset: 0x00170D5C
		public int BezierSegmentsPerCurve
		{
			get
			{
				return this.bezierSegmentsPerCurve;
			}
			set
			{
				this.bezierSegmentsPerCurve = value;
			}
		}

		// Token: 0x17000A90 RID: 2704
		// (get) Token: 0x06004897 RID: 18583 RVA: 0x00172965 File Offset: 0x00170D65
		// (set) Token: 0x06004898 RID: 18584 RVA: 0x0017296D File Offset: 0x00170D6D
		public Vector2[] Points
		{
			get
			{
				return this.m_points;
			}
			set
			{
				if (this.m_points == value)
				{
					return;
				}
				this.m_points = value;
				this.SetAllDirty();
			}
		}

		// Token: 0x06004899 RID: 18585 RVA: 0x0017298C File Offset: 0x00170D8C
		protected override void OnPopulateMesh(VertexHelper vh)
		{
			if (this.m_points == null)
			{
				return;
			}
			this.GeneratedUVs();
			Vector2[] array = this.m_points;
			if (this.BezierMode != UILineRenderer.BezierType.None && this.BezierMode != UILineRenderer.BezierType.Catenary && this.m_points.Length > 3)
			{
				BezierPath bezierPath = new BezierPath();
				bezierPath.SetControlPoints(array);
				bezierPath.SegmentsPerCurve = this.bezierSegmentsPerCurve;
				UILineRenderer.BezierType bezierMode = this.BezierMode;
				List<Vector2> list;
				if (bezierMode != UILineRenderer.BezierType.Basic)
				{
					if (bezierMode != UILineRenderer.BezierType.Improved)
					{
						list = bezierPath.GetDrawingPoints2();
					}
					else
					{
						list = bezierPath.GetDrawingPoints1();
					}
				}
				else
				{
					list = bezierPath.GetDrawingPoints0();
				}
				array = list.ToArray();
			}
			if (this.BezierMode == UILineRenderer.BezierType.Catenary && this.m_points.Length == 2)
			{
				array = new CableCurve(array)
				{
					slack = base.Resoloution,
					steps = this.BezierSegmentsPerCurve
				}.Points();
			}
			if (base.ImproveResolution != ResolutionMode.None)
			{
				array = base.IncreaseResolution(array);
			}
			float num = this.relativeSize ? base.rectTransform.rect.width : 1f;
			float num2 = this.relativeSize ? base.rectTransform.rect.height : 1f;
			float num3 = -base.rectTransform.pivot.x * num;
			float num4 = -base.rectTransform.pivot.y * num2;
			vh.Clear();
			List<UIVertex[]> list2 = new List<UIVertex[]>();
			if (this.lineList)
			{
				for (int i = 1; i < array.Length; i += 2)
				{
					Vector2 start = array[i - 1];
					Vector2 end = array[i];
					start = new Vector2(start.x * num + num3, start.y * num2 + num4);
					end = new Vector2(end.x * num + num3, end.y * num2 + num4);
					if (this.lineCaps)
					{
						list2.Add(this.CreateLineCap(start, end, UILineRenderer.SegmentType.Start));
					}
					list2.Add(this.CreateLineSegment(start, end, UILineRenderer.SegmentType.Middle));
					if (this.lineCaps)
					{
						list2.Add(this.CreateLineCap(start, end, UILineRenderer.SegmentType.End));
					}
				}
			}
			else
			{
				for (int j = 1; j < array.Length; j++)
				{
					Vector2 start2 = array[j - 1];
					Vector2 end2 = array[j];
					start2 = new Vector2(start2.x * num + num3, start2.y * num2 + num4);
					end2 = new Vector2(end2.x * num + num3, end2.y * num2 + num4);
					if (this.lineCaps && j == 1)
					{
						list2.Add(this.CreateLineCap(start2, end2, UILineRenderer.SegmentType.Start));
					}
					list2.Add(this.CreateLineSegment(start2, end2, UILineRenderer.SegmentType.Middle));
					if (this.lineCaps && j == array.Length - 1)
					{
						list2.Add(this.CreateLineCap(start2, end2, UILineRenderer.SegmentType.End));
					}
				}
			}
			for (int k = 0; k < list2.Count; k++)
			{
				if (!this.lineList && k < list2.Count - 1)
				{
					Vector3 v = list2[k][1].position - list2[k][2].position;
					Vector3 v2 = list2[k + 1][2].position - list2[k + 1][1].position;
					float num5 = Vector2.Angle(v, v2) * 0.017453292f;
					float num6 = Mathf.Sign(Vector3.Cross(v.normalized, v2.normalized).z);
					float num7 = this.lineThickness / (2f * Mathf.Tan(num5 / 2f));
					Vector3 position = list2[k][2].position - v.normalized * num7 * num6;
					Vector3 position2 = list2[k][3].position + v.normalized * num7 * num6;
					UILineRenderer.JoinType joinType = this.LineJoins;
					if (joinType == UILineRenderer.JoinType.Miter)
					{
						if (num7 < v.magnitude / 2f && num7 < v2.magnitude / 2f && num5 > 0.2617994f)
						{
							list2[k][2].position = position;
							list2[k][3].position = position2;
							list2[k + 1][0].position = position2;
							list2[k + 1][1].position = position;
						}
						else
						{
							joinType = UILineRenderer.JoinType.Bevel;
						}
					}
					if (joinType == UILineRenderer.JoinType.Bevel)
					{
						if (num7 < v.magnitude / 2f && num7 < v2.magnitude / 2f && num5 > 0.5235988f)
						{
							if (num6 < 0f)
							{
								list2[k][2].position = position;
								list2[k + 1][1].position = position;
							}
							else
							{
								list2[k][3].position = position2;
								list2[k + 1][0].position = position2;
							}
						}
						UIVertex[] verts = new UIVertex[]
						{
							list2[k][2],
							list2[k][3],
							list2[k + 1][0],
							list2[k + 1][1]
						};
						vh.AddUIVertexQuad(verts);
					}
				}
				vh.AddUIVertexQuad(list2[k]);
			}
			if (vh.currentVertCount > 64000)
			{
				global::UnityEngine.Debug.LogError("Max Verticies size is 64000, current mesh vertcies count is [" + vh.currentVertCount + "] - Cannot Draw");
				vh.Clear();
				return;
			}
		}

		// Token: 0x0600489A RID: 18586 RVA: 0x00173050 File Offset: 0x00171450
		private UIVertex[] CreateLineCap(Vector2 start, Vector2 end, UILineRenderer.SegmentType type)
		{
			if (type == UILineRenderer.SegmentType.Start)
			{
				Vector2 start2 = start - (end - start).normalized * this.lineThickness / 2f;
				return this.CreateLineSegment(start2, start, UILineRenderer.SegmentType.Start);
			}
			if (type == UILineRenderer.SegmentType.End)
			{
				Vector2 end2 = end + (end - start).normalized * this.lineThickness / 2f;
				return this.CreateLineSegment(end, end2, UILineRenderer.SegmentType.End);
			}
			global::UnityEngine.Debug.LogError("Bad SegmentType passed in to CreateLineCap. Must be SegmentType.Start or SegmentType.End");
			return null;
		}

		// Token: 0x0600489B RID: 18587 RVA: 0x001730E0 File Offset: 0x001714E0
		private UIVertex[] CreateLineSegment(Vector2 start, Vector2 end, UILineRenderer.SegmentType type)
		{
			Vector2 vector = new Vector2(start.y - end.y, end.x - start.x);
			Vector2 b = vector.normalized * this.lineThickness / 2f;
			Vector2 vector2 = start - b;
			Vector2 vector3 = start + b;
			Vector2 vector4 = end + b;
			Vector2 vector5 = end - b;
			switch (type)
			{
			case UILineRenderer.SegmentType.Start:
				return base.SetVbo(new Vector2[]
				{
					vector2,
					vector3,
					vector4,
					vector5
				}, UILineRenderer.startUvs);
			case UILineRenderer.SegmentType.End:
				return base.SetVbo(new Vector2[]
				{
					vector2,
					vector3,
					vector4,
					vector5
				}, UILineRenderer.endUvs);
			case UILineRenderer.SegmentType.Full:
				return base.SetVbo(new Vector2[]
				{
					vector2,
					vector3,
					vector4,
					vector5
				}, UILineRenderer.fullUvs);
			}
			return base.SetVbo(new Vector2[]
			{
				vector2,
				vector3,
				vector4,
				vector5
			}, UILineRenderer.middleUvs);
		}

		// Token: 0x0600489C RID: 18588 RVA: 0x0017328C File Offset: 0x0017168C
		protected override void GeneratedUVs()
		{
			if (base.activeSprite != null)
			{
				Vector4 outerUV = DataUtility.GetOuterUV(base.activeSprite);
				Vector4 innerUV = DataUtility.GetInnerUV(base.activeSprite);
				UILineRenderer.UV_TOP_LEFT = new Vector2(outerUV.x, outerUV.y);
				UILineRenderer.UV_BOTTOM_LEFT = new Vector2(outerUV.x, outerUV.w);
				UILineRenderer.UV_TOP_CENTER_LEFT = new Vector2(innerUV.x, innerUV.y);
				UILineRenderer.UV_TOP_CENTER_RIGHT = new Vector2(innerUV.z, innerUV.y);
				UILineRenderer.UV_BOTTOM_CENTER_LEFT = new Vector2(innerUV.x, innerUV.w);
				UILineRenderer.UV_BOTTOM_CENTER_RIGHT = new Vector2(innerUV.z, innerUV.w);
				UILineRenderer.UV_TOP_RIGHT = new Vector2(outerUV.z, outerUV.y);
				UILineRenderer.UV_BOTTOM_RIGHT = new Vector2(outerUV.z, outerUV.w);
			}
			else
			{
				UILineRenderer.UV_TOP_LEFT = Vector2.zero;
				UILineRenderer.UV_BOTTOM_LEFT = new Vector2(0f, 1f);
				UILineRenderer.UV_TOP_CENTER_LEFT = new Vector2(0.5f, 0f);
				UILineRenderer.UV_TOP_CENTER_RIGHT = new Vector2(0.5f, 0f);
				UILineRenderer.UV_BOTTOM_CENTER_LEFT = new Vector2(0.5f, 1f);
				UILineRenderer.UV_BOTTOM_CENTER_RIGHT = new Vector2(0.5f, 1f);
				UILineRenderer.UV_TOP_RIGHT = new Vector2(1f, 0f);
				UILineRenderer.UV_BOTTOM_RIGHT = Vector2.one;
			}
			UILineRenderer.startUvs = new Vector2[]
			{
				UILineRenderer.UV_TOP_LEFT,
				UILineRenderer.UV_BOTTOM_LEFT,
				UILineRenderer.UV_BOTTOM_CENTER_LEFT,
				UILineRenderer.UV_TOP_CENTER_LEFT
			};
			UILineRenderer.middleUvs = new Vector2[]
			{
				UILineRenderer.UV_TOP_CENTER_LEFT,
				UILineRenderer.UV_BOTTOM_CENTER_LEFT,
				UILineRenderer.UV_BOTTOM_CENTER_RIGHT,
				UILineRenderer.UV_TOP_CENTER_RIGHT
			};
			UILineRenderer.endUvs = new Vector2[]
			{
				UILineRenderer.UV_TOP_CENTER_RIGHT,
				UILineRenderer.UV_BOTTOM_CENTER_RIGHT,
				UILineRenderer.UV_BOTTOM_RIGHT,
				UILineRenderer.UV_TOP_RIGHT
			};
			UILineRenderer.fullUvs = new Vector2[]
			{
				UILineRenderer.UV_TOP_LEFT,
				UILineRenderer.UV_BOTTOM_LEFT,
				UILineRenderer.UV_BOTTOM_RIGHT,
				UILineRenderer.UV_TOP_RIGHT
			};
		}

		// Token: 0x0600489D RID: 18589 RVA: 0x00173550 File Offset: 0x00171950
		protected override void ResolutionToNativeSize(float distance)
		{
			if (base.UseNativeSize)
			{
				this.m_Resolution = distance / (base.activeSprite.rect.width / base.pixelsPerUnit);
				this.lineThickness = base.activeSprite.rect.height / base.pixelsPerUnit;
			}
		}

		// Token: 0x04006F2F RID: 28463
		private const float MIN_MITER_JOIN = 0.2617994f;

		// Token: 0x04006F30 RID: 28464
		private const float MIN_BEVEL_NICE_JOIN = 0.5235988f;

		// Token: 0x04006F31 RID: 28465
		private static Vector2 UV_TOP_LEFT;

		// Token: 0x04006F32 RID: 28466
		private static Vector2 UV_BOTTOM_LEFT;

		// Token: 0x04006F33 RID: 28467
		private static Vector2 UV_TOP_CENTER_LEFT;

		// Token: 0x04006F34 RID: 28468
		private static Vector2 UV_TOP_CENTER_RIGHT;

		// Token: 0x04006F35 RID: 28469
		private static Vector2 UV_BOTTOM_CENTER_LEFT;

		// Token: 0x04006F36 RID: 28470
		private static Vector2 UV_BOTTOM_CENTER_RIGHT;

		// Token: 0x04006F37 RID: 28471
		private static Vector2 UV_TOP_RIGHT;

		// Token: 0x04006F38 RID: 28472
		private static Vector2 UV_BOTTOM_RIGHT;

		// Token: 0x04006F39 RID: 28473
		private static Vector2[] startUvs;

		// Token: 0x04006F3A RID: 28474
		private static Vector2[] middleUvs;

		// Token: 0x04006F3B RID: 28475
		private static Vector2[] endUvs;

		// Token: 0x04006F3C RID: 28476
		private static Vector2[] fullUvs;

		// Token: 0x04006F3D RID: 28477
		[SerializeField]
		[Tooltip("Points to draw lines between\n Can be improved using the Resolution Option")]
		internal Vector2[] m_points;

		// Token: 0x04006F3E RID: 28478
		[SerializeField]
		[Tooltip("Thickness of the line")]
		internal float lineThickness = 2f;

		// Token: 0x04006F3F RID: 28479
		[SerializeField]
		[Tooltip("Use the relative bounds of the Rect Transform (0,0 -> 0,1) or screen space coordinates")]
		internal bool relativeSize;

		// Token: 0x04006F40 RID: 28480
		[SerializeField]
		[Tooltip("Do the points identify a single line or split pairs of lines")]
		internal bool lineList;

		// Token: 0x04006F41 RID: 28481
		[SerializeField]
		[Tooltip("Add end caps to each line\nMultiple caps when used with Line List")]
		internal bool lineCaps;

		// Token: 0x04006F42 RID: 28482
		[SerializeField]
		[Tooltip("Resolution of the Bezier curve, different to line Resolution")]
		internal int bezierSegmentsPerCurve = 10;

		// Token: 0x04006F43 RID: 28483
		[Tooltip("The type of Join used between lines, Square/Mitre or Curved/Bevel")]
		public UILineRenderer.JoinType LineJoins;

		// Token: 0x04006F44 RID: 28484
		[Tooltip("Bezier method to apply to line, see docs for options\nCan't be used in conjunction with Resolution as Bezier already changes the resolution")]
		public UILineRenderer.BezierType BezierMode;

		// Token: 0x04006F45 RID: 28485
		[HideInInspector]
		public bool drivenExternally;

		// Token: 0x02000C09 RID: 3081
		private enum SegmentType
		{
			// Token: 0x04006F47 RID: 28487
			Start,
			// Token: 0x04006F48 RID: 28488
			Middle,
			// Token: 0x04006F49 RID: 28489
			End,
			// Token: 0x04006F4A RID: 28490
			Full
		}

		// Token: 0x02000C0A RID: 3082
		public enum JoinType
		{
			// Token: 0x04006F4C RID: 28492
			Bevel,
			// Token: 0x04006F4D RID: 28493
			Miter
		}

		// Token: 0x02000C0B RID: 3083
		public enum BezierType
		{
			// Token: 0x04006F4F RID: 28495
			None,
			// Token: 0x04006F50 RID: 28496
			Quick,
			// Token: 0x04006F51 RID: 28497
			Basic,
			// Token: 0x04006F52 RID: 28498
			Improved,
			// Token: 0x04006F53 RID: 28499
			Catenary
		}
	}
}
