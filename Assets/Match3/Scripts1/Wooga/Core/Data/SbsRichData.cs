namespace Match3.Scripts1.Wooga.Core.Data
{
	// Token: 0x0200034A RID: 842
	public class SbsRichData : ISbsData
	{
		// Token: 0x060019BF RID: 6591 RVA: 0x0007466E File Offset: 0x00072A6E
		public SbsRichData(string data, SbsMetaData metaData)
		{
			this.Data = data;
			this.MetaData = metaData;
		}

		// Token: 0x060019C0 RID: 6592 RVA: 0x00074684 File Offset: 0x00072A84
		public SbsRichData(string data, int formatVersion = -1)
		{
			this.Data = data;
			this.MetaData = new SbsMetaData(formatVersion);
		}

		// Token: 0x1700040E RID: 1038
		// (get) Token: 0x060019C1 RID: 6593 RVA: 0x0007469F File Offset: 0x00072A9F
		// (set) Token: 0x060019C2 RID: 6594 RVA: 0x000746A7 File Offset: 0x00072AA7
		public string Data { get; set; }

		// Token: 0x1700040F RID: 1039
		// (get) Token: 0x060019C3 RID: 6595 RVA: 0x000746B0 File Offset: 0x00072AB0
		// (set) Token: 0x060019C4 RID: 6596 RVA: 0x000746BD File Offset: 0x00072ABD
		public int FormatVersion
		{
			get
			{
				return this.MetaData.FormatVersion;
			}
			set
			{
				this.MetaData.FormatVersion = value;
			}
		}

		// Token: 0x17000410 RID: 1040
		// (get) Token: 0x060019C5 RID: 6597 RVA: 0x000746CB File Offset: 0x00072ACB
		// (set) Token: 0x060019C6 RID: 6598 RVA: 0x000746D3 File Offset: 0x00072AD3
		public SbsMetaData MetaData { get; set; }

		// Token: 0x060019C7 RID: 6599 RVA: 0x000746DC File Offset: 0x00072ADC
		public string Serialize()
		{
			return this.MetaData.Serialize() + this.Data;
		}

		// Token: 0x060019C8 RID: 6600 RVA: 0x000746F4 File Offset: 0x00072AF4
		public override string ToString()
		{
			return string.Format("{{\n    \"format_version\":{0},\n    \"data\":{1}\n}}", this.FormatVersion, this.Data);
		}
	}
}
