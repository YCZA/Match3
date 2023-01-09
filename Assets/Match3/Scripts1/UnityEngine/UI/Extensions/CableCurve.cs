using System;
using UnityEngine;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000C18 RID: 3096
	[Serializable]
	public class CableCurve
	{
		// Token: 0x0600490B RID: 18699 RVA: 0x00175F80 File Offset: 0x00174380
		public CableCurve()
		{
			this.points = CableCurve.emptyCurve;
			this.m_start = Vector2.up;
			this.m_end = Vector2.up + Vector2.right;
			this.m_slack = 0.5f;
			this.m_steps = 20;
			this.m_regen = true;
		}

		// Token: 0x0600490C RID: 18700 RVA: 0x00175FD8 File Offset: 0x001743D8
		public CableCurve(Vector2[] inputPoints)
		{
			this.points = inputPoints;
			this.m_start = inputPoints[0];
			this.m_end = inputPoints[1];
			this.m_slack = 0.5f;
			this.m_steps = 20;
			this.m_regen = true;
		}

		// Token: 0x0600490D RID: 18701 RVA: 0x00176030 File Offset: 0x00174430
		public CableCurve(CableCurve v)
		{
			this.points = v.Points();
			this.m_start = v.start;
			this.m_end = v.end;
			this.m_slack = v.slack;
			this.m_steps = v.steps;
			this.m_regen = v.regenPoints;
		}

		// Token: 0x17000AA8 RID: 2728
		// (get) Token: 0x0600490E RID: 18702 RVA: 0x0017608B File Offset: 0x0017448B
		// (set) Token: 0x0600490F RID: 18703 RVA: 0x00176093 File Offset: 0x00174493
		public bool regenPoints
		{
			get
			{
				return this.m_regen;
			}
			set
			{
				this.m_regen = value;
			}
		}

		// Token: 0x17000AA9 RID: 2729
		// (get) Token: 0x06004910 RID: 18704 RVA: 0x0017609C File Offset: 0x0017449C
		// (set) Token: 0x06004911 RID: 18705 RVA: 0x001760A4 File Offset: 0x001744A4
		public Vector2 start
		{
			get
			{
				return this.m_start;
			}
			set
			{
				if (value != this.m_start)
				{
					this.m_regen = true;
				}
				this.m_start = value;
			}
		}

		// Token: 0x17000AAA RID: 2730
		// (get) Token: 0x06004912 RID: 18706 RVA: 0x001760C5 File Offset: 0x001744C5
		// (set) Token: 0x06004913 RID: 18707 RVA: 0x001760CD File Offset: 0x001744CD
		public Vector2 end
		{
			get
			{
				return this.m_end;
			}
			set
			{
				if (value != this.m_end)
				{
					this.m_regen = true;
				}
				this.m_end = value;
			}
		}

		// Token: 0x17000AAB RID: 2731
		// (get) Token: 0x06004914 RID: 18708 RVA: 0x001760EE File Offset: 0x001744EE
		// (set) Token: 0x06004915 RID: 18709 RVA: 0x001760F6 File Offset: 0x001744F6
		public float slack
		{
			get
			{
				return this.m_slack;
			}
			set
			{
				if (value != this.m_slack)
				{
					this.m_regen = true;
				}
				this.m_slack = Mathf.Max(0f, value);
			}
		}

		// Token: 0x17000AAC RID: 2732
		// (get) Token: 0x06004916 RID: 18710 RVA: 0x0017611C File Offset: 0x0017451C
		// (set) Token: 0x06004917 RID: 18711 RVA: 0x00176124 File Offset: 0x00174524
		public int steps
		{
			get
			{
				return this.m_steps;
			}
			set
			{
				if (value != this.m_steps)
				{
					this.m_regen = true;
				}
				this.m_steps = Mathf.Max(2, value);
			}
		}

		// Token: 0x17000AAD RID: 2733
		// (get) Token: 0x06004918 RID: 18712 RVA: 0x00176148 File Offset: 0x00174548
		public Vector2 midPoint
		{
			get
			{
				Vector2 result = Vector2.zero;
				if (this.m_steps == 2)
				{
					return (this.points[0] + this.points[1]) * 0.5f;
				}
				if (this.m_steps > 2)
				{
					int num = this.m_steps / 2;
					if (this.m_steps % 2 == 0)
					{
						result = (this.points[num] + this.points[num + 1]) * 0.5f;
					}
					else
					{
						result = this.points[num];
					}
				}
				return result;
			}
		}

		// Token: 0x06004919 RID: 18713 RVA: 0x00176208 File Offset: 0x00174608
		public Vector2[] Points()
		{
			if (!this.m_regen)
			{
				return this.points;
			}
			if (this.m_steps < 2)
			{
				return CableCurve.emptyCurve;
			}
			float num = Vector2.Distance(this.m_end, this.m_start);
			float num2 = Vector2.Distance(new Vector2(this.m_end.x, this.m_start.y), this.m_start);
			float num3 = num + Mathf.Max(0.0001f, this.m_slack);
			float num4 = 0f;
			float y = this.m_start.y;
			float num5 = num2;
			float y2 = this.end.y;
			if (num5 - num4 == 0f)
			{
				return CableCurve.emptyCurve;
			}
			float num6 = Mathf.Sqrt(Mathf.Pow(num3, 2f) - Mathf.Pow(y2 - y, 2f)) / (num5 - num4);
			int num7 = 30;
			int num8 = 0;
			int num9 = num7 * 10;
			bool flag = false;
			float num10 = 0f;
			float num11 = 100f;
			for (int i = 0; i < num7; i++)
			{
				for (int j = 0; j < 10; j++)
				{
					num8++;
					float num12 = num10 + num11;
					float num13 = (float)Math.Sinh((double)num12) / num12;
					if (!float.IsInfinity(num13))
					{
						if (num13 == num6)
						{
							flag = true;
							num10 = num12;
							break;
						}
						if (num13 > num6)
						{
							break;
						}
						num10 = num12;
						if (num8 > num9)
						{
							flag = true;
							break;
						}
					}
				}
				if (flag)
				{
					break;
				}
				num11 *= 0.1f;
			}
			float num14 = (num5 - num4) / 2f / num10;
			float num15 = (num4 + num5 - num14 * Mathf.Log((num3 + y2 - y) / (num3 - y2 + y))) / 2f;
			float num16 = (y2 + y - num3 * (float)Math.Cosh((double)num10) / (float)Math.Sinh((double)num10)) / 2f;
			this.points = new Vector2[this.m_steps];
			float num17 = (float)(this.m_steps - 1);
			for (int k = 0; k < this.m_steps; k++)
			{
				float num18 = (float)k / num17;
				Vector2 zero = Vector2.zero;
				zero.x = Mathf.Lerp(this.start.x, this.end.x, num18);
				zero.y = num14 * (float)Math.Cosh((double)((num18 * num2 - num15) / num14)) + num16;
				this.points[k] = zero;
			}
			this.m_regen = false;
			return this.points;
		}

		// Token: 0x04006FA1 RID: 28577
		[SerializeField]
		private Vector2 m_start;

		// Token: 0x04006FA2 RID: 28578
		[SerializeField]
		private Vector2 m_end;

		// Token: 0x04006FA3 RID: 28579
		[SerializeField]
		private float m_slack;

		// Token: 0x04006FA4 RID: 28580
		[SerializeField]
		private int m_steps;

		// Token: 0x04006FA5 RID: 28581
		[SerializeField]
		private bool m_regen;

		// Token: 0x04006FA6 RID: 28582
		private static Vector2[] emptyCurve = new Vector2[]
		{
			new Vector2(0f, 0f),
			new Vector2(0f, 0f)
		};

		// Token: 0x04006FA7 RID: 28583
		[SerializeField]
		private Vector2[] points;
	}
}
