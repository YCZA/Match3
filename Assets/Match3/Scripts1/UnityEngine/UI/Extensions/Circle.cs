using UnityEngine;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000C19 RID: 3097
	public class Circle
	{
		// Token: 0x0600491B RID: 18715 RVA: 0x00176512 File Offset: 0x00174912
		public Circle(float radius)
		{
			this.xAxis = radius;
			this.yAxis = radius;
			this.steps = 1;
		}

		// Token: 0x0600491C RID: 18716 RVA: 0x0017652F File Offset: 0x0017492F
		public Circle(float radius, int steps)
		{
			this.xAxis = radius;
			this.yAxis = radius;
			this.steps = steps;
		}

		// Token: 0x0600491D RID: 18717 RVA: 0x0017654C File Offset: 0x0017494C
		public Circle(float xAxis, float yAxis)
		{
			this.xAxis = xAxis;
			this.yAxis = yAxis;
			this.steps = 10;
		}

		// Token: 0x0600491E RID: 18718 RVA: 0x0017656A File Offset: 0x0017496A
		public Circle(float xAxis, float yAxis, int steps)
		{
			this.xAxis = xAxis;
			this.yAxis = yAxis;
			this.steps = steps;
		}

		// Token: 0x17000AAE RID: 2734
		// (get) Token: 0x0600491F RID: 18719 RVA: 0x00176587 File Offset: 0x00174987
		// (set) Token: 0x06004920 RID: 18720 RVA: 0x0017658F File Offset: 0x0017498F
		public float X
		{
			get
			{
				return this.xAxis;
			}
			set
			{
				this.xAxis = value;
			}
		}

		// Token: 0x17000AAF RID: 2735
		// (get) Token: 0x06004921 RID: 18721 RVA: 0x00176598 File Offset: 0x00174998
		// (set) Token: 0x06004922 RID: 18722 RVA: 0x001765A0 File Offset: 0x001749A0
		public float Y
		{
			get
			{
				return this.yAxis;
			}
			set
			{
				this.yAxis = value;
			}
		}

		// Token: 0x17000AB0 RID: 2736
		// (get) Token: 0x06004923 RID: 18723 RVA: 0x001765A9 File Offset: 0x001749A9
		// (set) Token: 0x06004924 RID: 18724 RVA: 0x001765B1 File Offset: 0x001749B1
		public int Steps
		{
			get
			{
				return this.steps;
			}
			set
			{
				this.steps = value;
			}
		}

		// Token: 0x06004925 RID: 18725 RVA: 0x001765BC File Offset: 0x001749BC
		public Vector2 Evaluate(float t)
		{
			float num = 360f / (float)this.steps;
			float f = 0.017453292f * num * t;
			float x = Mathf.Sin(f) * this.xAxis;
			float y = Mathf.Cos(f) * this.yAxis;
			return new Vector2(x, y);
		}

		// Token: 0x06004926 RID: 18726 RVA: 0x00176604 File Offset: 0x00174A04
		public void Evaluate(float t, out Vector2 eval)
		{
			float num = 360f / (float)this.steps;
			float f = 0.017453292f * num * t;
			eval.x = Mathf.Sin(f) * this.xAxis;
			eval.y = Mathf.Cos(f) * this.yAxis;
		}

		// Token: 0x04006FA8 RID: 28584
		[SerializeField]
		private float xAxis;

		// Token: 0x04006FA9 RID: 28585
		[SerializeField]
		private float yAxis;

		// Token: 0x04006FAA RID: 28586
		[SerializeField]
		private int steps;
	}
}
