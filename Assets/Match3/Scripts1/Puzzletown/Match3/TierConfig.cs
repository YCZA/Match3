using System;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020004EB RID: 1259
	[Serializable]
	public class TierConfig
	{
		// Token: 0x060022CE RID: 8910 RVA: 0x0009A44B File Offset: 0x0009884B
		public TierConfig(AreaConfig.Tier tier, int diamonds)
		{
			this.name = tier.ToString();
			this.tier = tier;
			this.diamonds = diamonds;
		}

		// Token: 0x060022CF RID: 8911 RVA: 0x0009A474 File Offset: 0x00098874
		public TierConfig(string tierName, int diamonds, bool isEasy)
		{
			this.name = tierName;
			this.tier = ((!isEasy) ? AreaConfig.Tier.c : AreaConfig.Tier.a);
			this.diamonds = diamonds;
		}

		// Token: 0x04004E85 RID: 20101
		public AreaConfig.Tier tier;

		// Token: 0x04004E86 RID: 20102
		public string name;

		// Token: 0x04004E87 RID: 20103
		public int diamonds;
	}
}
