using System;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020004F7 RID: 1271
	[Serializable]
	public class PirateBreakoutSetConfig : ALevelCollectionConfig
	{
		// Token: 0x06002302 RID: 8962 RVA: 0x0009B0A4 File Offset: 0x000994A4
		public PirateBreakoutSetConfig(PirateBreakoutLevelConfigExcel config)
		{
			this.level = config.level;
			this.file_name = config.file_name;
			this.objective = config.objective;
			this.tiers = new TierConfig[1];
			this.tiers[0] = new TierConfig(config.tier, 0, this.level <= 3);
			this.set = config.set;
		}

		// Token: 0x1700056A RID: 1386
		// (get) Token: 0x06002303 RID: 8963 RVA: 0x0009B113 File Offset: 0x00099513
		public bool IsEasy
		{
			get
			{
				return this.level <= 3;
			}
		}

		// Token: 0x1700056B RID: 1387
		// (get) Token: 0x06002304 RID: 8964 RVA: 0x0009B121 File Offset: 0x00099521
		public string Tier
		{
			get
			{
				return (this.level > 3) ? "c" : "a";
			}
		}

		// Token: 0x04004EC9 RID: 20169
		public string file_name;

		// Token: 0x04004ECA RID: 20170
		public int set;

		// Token: 0x04004ECB RID: 20171
		private const int LAST_EASY_LEVEL = 3;
	}
}
