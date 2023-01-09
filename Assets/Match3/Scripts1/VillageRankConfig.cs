using System;

// Token: 0x020004AA RID: 1194
namespace Match3.Scripts1
{
	[Serializable]
	public class VillageRankConfig
	{
		// Token: 0x060021A2 RID: 8610 RVA: 0x0008CEB8 File Offset: 0x0008B2B8
		public void Init()
		{
			foreach (VillageRank villageRank in this.ranks)
			{
				villageRank.reward_coins = this.reward.reward_coins;
				villageRank.reward_diamonds = this.reward.reward_diamonds;
				villageRank.reward_booster_amount = this.reward.reward_booster_amount;
			}
		}

		// Token: 0x04004CC8 RID: 19656
		public VillageRewards reward;

		// Token: 0x04004CC9 RID: 19657
		public VillageRank[] ranks;
	}
}
