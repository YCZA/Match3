using System;
using Match3.Scripts1.Puzzletown.Services;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Config
{
	// Token: 0x020004A5 RID: 1189
	[Serializable]
	public class TournamentRewardConfig
	{
		// Token: 0x06002191 RID: 8593 RVA: 0x0008CAEC File Offset: 0x0008AEEC
		public TournamentRewardConfig Copy()
		{
			TournamentRewardConfig tournamentRewardConfig = new TournamentRewardConfig();
			if (this.gold != null)
			{
				tournamentRewardConfig.gold = new MaterialAmount[this.gold.Length];
				this.gold.CopyTo(tournamentRewardConfig.gold, 0);
			}
			if (this.silver != null)
			{
				tournamentRewardConfig.silver = new MaterialAmount[this.silver.Length];
				this.silver.CopyTo(tournamentRewardConfig.silver, 0);
			}
			if (this.bronze != null)
			{
				tournamentRewardConfig.bronze = new MaterialAmount[this.bronze.Length];
				this.bronze.CopyTo(tournamentRewardConfig.bronze, 0);
			}
			if (this.wood != null)
			{
				tournamentRewardConfig.wood = new MaterialAmount[this.wood.Length];
				this.wood.CopyTo(tournamentRewardConfig.wood, 0);
			}
			return tournamentRewardConfig;
		}

		// Token: 0x06002192 RID: 8594 RVA: 0x0008CBC0 File Offset: 0x0008AFC0
		public Materials GetRewards(int playerPosition)
		{
			TournamentRewardCategory rewardCategoryFor = this.GetRewardCategoryFor(playerPosition);
			MaterialAmount[] array = this.gold;
			switch (rewardCategoryFor)
			{
			case TournamentRewardCategory.None:
				array = null;
				break;
			case TournamentRewardCategory.Silver:
				array = this.silver;
				break;
			case TournamentRewardCategory.Bronze:
				array = this.bronze;
				break;
			case TournamentRewardCategory.Wood:
				array = this.wood;
				break;
			}
			if (array == null)
			{
				return null;
			}
			return new Materials(array);
		}

		// Token: 0x06002193 RID: 8595 RVA: 0x0008CC34 File Offset: 0x0008B034
		public TournamentRewardCategory GetRewardCategoryFor(int playerPosition)
		{
			playerPosition = Mathf.Max(0, playerPosition);
			return (TournamentRewardCategory)((playerPosition <= 10) ? Mathf.Min(4, playerPosition) : 0);
		}

		// Token: 0x04004CB5 RID: 19637
		public MaterialAmount[] gold;

		// Token: 0x04004CB6 RID: 19638
		public MaterialAmount[] silver;

		// Token: 0x04004CB7 RID: 19639
		public MaterialAmount[] bronze;

		// Token: 0x04004CB8 RID: 19640
		public MaterialAmount[] wood;
	}
}
