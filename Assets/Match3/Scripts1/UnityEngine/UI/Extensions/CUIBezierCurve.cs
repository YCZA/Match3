using System;
using UnityEngine;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000BC5 RID: 3013
	public class CUIBezierCurve : MonoBehaviour
	{
		// Token: 0x17000A3D RID: 2621
		// (get) Token: 0x060046A9 RID: 18089 RVA: 0x001669BC File Offset: 0x00164DBC
		public Vector3[] ControlPoints
		{
			get
			{
				return this.controlPoints;
			}
		}

		// Token: 0x060046AA RID: 18090 RVA: 0x001669C4 File Offset: 0x00164DC4
		public void Refresh()
		{
			if (this.OnRefresh != null)
			{
				this.OnRefresh();
			}
		}

		// Token: 0x060046AB RID: 18091 RVA: 0x001669DC File Offset: 0x00164DDC
		public Vector3 GetPoint(float _time)
		{
			float num = 1f - _time;
			return num * num * num * this.controlPoints[0] + 3f * num * num * _time * this.controlPoints[1] + 3f * num * _time * _time * this.controlPoints[2] + _time * _time * _time * this.controlPoints[3];
		}

		// Token: 0x060046AC RID: 18092 RVA: 0x00166A78 File Offset: 0x00164E78
		public Vector3 GetTangent(float _time)
		{
			float num = 1f - _time;
			return 3f * num * num * (this.controlPoints[1] - this.controlPoints[0]) + 6f * num * _time * (this.controlPoints[2] - this.controlPoints[1]) + 3f * _time * _time * (this.controlPoints[3] - this.controlPoints[2]);
		}

		// Token: 0x060046AD RID: 18093 RVA: 0x00166B38 File Offset: 0x00164F38
		public void ReportSet()
		{
			if (this.controlPoints == null)
			{
				this.controlPoints = new Vector3[CUIBezierCurve.CubicBezierCurvePtNum];
				this.controlPoints[0] = new Vector3(0f, 0f, 0f);
				this.controlPoints[1] = new Vector3(0f, 1f, 0f);
				this.controlPoints[2] = new Vector3(1f, 1f, 0f);
				this.controlPoints[3] = new Vector3(1f, 0f, 0f);
			}
			bool flag = true;
			flag &= (this.controlPoints.Length == CUIBezierCurve.CubicBezierCurvePtNum);
		}

		// Token: 0x04006DFC RID: 28156
		public static readonly int CubicBezierCurvePtNum = 4;

		// Token: 0x04006DFD RID: 28157
		[SerializeField]
		protected Vector3[] controlPoints;

		// Token: 0x04006DFE RID: 28158
		public Action OnRefresh;
	}
}
