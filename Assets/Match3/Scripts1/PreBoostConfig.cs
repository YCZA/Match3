namespace Match3.Scripts1
{
	// Token: 0x02000583 RID: 1411
	public struct PreBoostConfig
	{
		// Token: 0x060024E9 RID: 9449 RVA: 0x000A5110 File Offset: 0x000A3510
		public PreBoostConfig(bool useBombAndLinGem, bool useDoubleFish, bool useRainbow)
		{
			this.useBombAndLinGem = useBombAndLinGem;
			this.useDoubleFish = useDoubleFish;
			this.useRainbow = useRainbow;
		}

		// Token: 0x17000585 RID: 1413
		// (get) Token: 0x060024EA RID: 9450 RVA: 0x000A5127 File Offset: 0x000A3527
		public bool UseBombAndLinGem
		{
			get
			{
				return this.useBombAndLinGem;
			}
		}

		// Token: 0x17000586 RID: 1414
		// (get) Token: 0x060024EB RID: 9451 RVA: 0x000A512F File Offset: 0x000A352F
		public bool UseDoubleFish
		{
			get
			{
				return this.useDoubleFish;
			}
		}

		// Token: 0x17000587 RID: 1415
		// (get) Token: 0x060024EC RID: 9452 RVA: 0x000A5137 File Offset: 0x000A3537
		public bool UseRainbow
		{
			get
			{
				return this.useRainbow;
			}
		}

		// Token: 0x060024ED RID: 9453 RVA: 0x000A513F File Offset: 0x000A353F
		public void ResetBoosts()
		{
			this.useBombAndLinGem = false;
			this.useDoubleFish = false;
			this.useRainbow = false;
		}

		// Token: 0x0400506C RID: 20588
		private bool useBombAndLinGem;

		// Token: 0x0400506D RID: 20589
		private bool useDoubleFish;

		// Token: 0x0400506E RID: 20590
		private bool useRainbow;
	}
}
