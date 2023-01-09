using System;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020004EE RID: 1262
	[Serializable]
	public class DiveForTreasureSetConfig : ALevelCollectionConfig
	{
		// Token: 0x060022D3 RID: 8915 RVA: 0x0009A4D8 File Offset: 0x000988D8
		public DiveForTreasureSetConfig(DiveForTreasureLevelConfigExcel config)
		{
			this.level = config.level;
			this.file_name = config.file_name;
			this.objective = config.objective;
			this.tiers = new TierConfig[1];
			this.tiers[0] = new TierConfig(config.tier, 0, this.level <= 3);
			this.set = config.set;
		}

		// Token: 0x1700054E RID: 1358
		// (get) Token: 0x060022D4 RID: 8916 RVA: 0x0009A547 File Offset: 0x00098947
		public bool IsEasy
		{
			get
			{
				return this.level <= 3;
			}
		}

		// Token: 0x1700054F RID: 1359
		// (get) Token: 0x060022D5 RID: 8917 RVA: 0x0009A555 File Offset: 0x00098955
		public string Tier
		{
			get
			{
				return (this.level > 3) ? "c" : "a";
			}
		}

		// Token: 0x04004E8D RID: 20109
		public string file_name;

		// Token: 0x04004E8E RID: 20110
		public int set;

		// Token: 0x04004E8F RID: 20111
		private const int LAST_EASY_LEVEL = 3;
	}
}
