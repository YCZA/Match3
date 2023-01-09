namespace Match3.Scripts1
{
	// Token: 0x02000B95 RID: 2965
	public struct HsvColor
	{
		// Token: 0x06004568 RID: 17768 RVA: 0x0015FB70 File Offset: 0x0015DF70
		public HsvColor(double h, double s, double v)
		{
			this.H = h;
			this.S = s;
			this.V = v;
		}

		// Token: 0x170009F8 RID: 2552
		// (get) Token: 0x06004569 RID: 17769 RVA: 0x0015FB87 File Offset: 0x0015DF87
		// (set) Token: 0x0600456A RID: 17770 RVA: 0x0015FB96 File Offset: 0x0015DF96
		public float NormalizedH
		{
			get
			{
				return (float)this.H / 360f;
			}
			set
			{
				this.H = (double)value * 360.0;
			}
		}

		// Token: 0x170009F9 RID: 2553
		// (get) Token: 0x0600456B RID: 17771 RVA: 0x0015FBAA File Offset: 0x0015DFAA
		// (set) Token: 0x0600456C RID: 17772 RVA: 0x0015FBB3 File Offset: 0x0015DFB3
		public float NormalizedS
		{
			get
			{
				return (float)this.S;
			}
			set
			{
				this.S = (double)value;
			}
		}

		// Token: 0x170009FA RID: 2554
		// (get) Token: 0x0600456D RID: 17773 RVA: 0x0015FBBD File Offset: 0x0015DFBD
		// (set) Token: 0x0600456E RID: 17774 RVA: 0x0015FBC6 File Offset: 0x0015DFC6
		public float NormalizedV
		{
			get
			{
				return (float)this.V;
			}
			set
			{
				this.V = (double)value;
			}
		}

		// Token: 0x0600456F RID: 17775 RVA: 0x0015FBD0 File Offset: 0x0015DFD0
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"{",
				this.H.ToString("f2"),
				",",
				this.S.ToString("f2"),
				",",
				this.V.ToString("f2"),
				"}"
			});
		}

		// Token: 0x04006CE6 RID: 27878
		public double H;

		// Token: 0x04006CE7 RID: 27879
		public double S;

		// Token: 0x04006CE8 RID: 27880
		public double V;
	}
}
