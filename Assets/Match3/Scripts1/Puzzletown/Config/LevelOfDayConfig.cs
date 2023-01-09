using System;
using System.Collections.Generic;

namespace Match3.Scripts1.Puzzletown.Config
{
	// Token: 0x020007CB RID: 1995
	[Serializable]
	public class LevelOfDayConfig
	{
		// Token: 0x0600312B RID: 12587 RVA: 0x000E72D0 File Offset: 0x000E56D0
		public List<MaterialAmount> GetRewardsForDay(int day)
		{
			List<MaterialAmount> list = new List<MaterialAmount>();
			if (this.rewards != null)
			{
				foreach (LevelOfDayConfig.Reward reward in this.rewards)
				{
					if (reward.day == day)
					{
						list.Add(new MaterialAmount(reward.reward_1_type, reward.reward_1_amount, MaterialAmountUsage.Undefined, 0));
						list.Add(new MaterialAmount(reward.reward_2_type, reward.reward_2_amount, MaterialAmountUsage.Undefined, 0));
					}
				}
			}
			return list;
		}

		// Token: 0x040059D6 RID: 22998
		public LevelOfDayConfig.General general;

		// Token: 0x040059D7 RID: 22999
		public List<LevelOfDayConfig.Reward> rewards;

		// Token: 0x020007CC RID: 1996
		[Serializable]
		public class General
		{
			// Token: 0x040059D8 RID: 23000
			public int unlock_level;

			// Token: 0x040059D9 RID: 23001
			public int start_hour;

			// Token: 0x040059DA RID: 23002
			public string control_reward_1_type;

			// Token: 0x040059DB RID: 23003
			public int control_reward_1_amount;

			// Token: 0x040059DC RID: 23004
			public string control_reward_2_type;

			// Token: 0x040059DD RID: 23005
			public int control_reward_2_amount;
		}

		// Token: 0x020007CD RID: 1997
		[Serializable]
		public class Reward
		{
			// Token: 0x040059DE RID: 23006
			public int day;

			// Token: 0x040059DF RID: 23007
			public string reward_1_type;

			// Token: 0x040059E0 RID: 23008
			public int reward_1_amount;

			// Token: 0x040059E1 RID: 23009
			public string reward_2_type;

			// Token: 0x040059E2 RID: 23010
			public int reward_2_amount;
		}
	}
}
