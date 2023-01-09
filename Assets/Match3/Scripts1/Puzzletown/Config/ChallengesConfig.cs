using System;
using System.Collections.Generic;

namespace Match3.Scripts1.Puzzletown.Config
{
	// Token: 0x02000909 RID: 2313
	[Serializable]
	public class ChallengesConfig
	{
		// Token: 0x04006092 RID: 24722
		public ChallengesConfig.Balancing balancing_v2;

		// Token: 0x04006093 RID: 24723
		public List<ChallengeConfig> challenge_definitions_v2;

		// Token: 0x0200090A RID: 2314
		[Serializable]
		public class Balancing
		{
			// Token: 0x04006094 RID: 24724
			public int mystery_box_paw_price;

			// Token: 0x04006095 RID: 24725
			public int paw_reward_easy;

			// Token: 0x04006096 RID: 24726
			public int paw_reward_medium;

			// Token: 0x04006097 RID: 24727
			public int paw_reward_hard;

			// Token: 0x04006098 RID: 24728
			public int challenge_extend_minutes;

			// Token: 0x04006099 RID: 24729
			public int icon_enabled_level;

			// Token: 0x0400609A RID: 24730
			public int play_minimum_level;

			// Token: 0x0400609B RID: 24731
			public string easy_additional_reward;

			// Token: 0x0400609C RID: 24732
			public int easy_additional_amount;

			// Token: 0x0400609D RID: 24733
			public string medium_additional_reward;

			// Token: 0x0400609E RID: 24734
			public int medium_additional_amount;

			// Token: 0x0400609F RID: 24735
			public string hard_additional_reward;

			// Token: 0x040060A0 RID: 24736
			public int hard_additional_amount;

			// Token: 0x040060A1 RID: 24737
			public int cool_down_timer_minutes;

			// Token: 0x040060A2 RID: 24738
			public int diamond_skip_price;

			// Token: 0x040060A3 RID: 24739
			public int easy_diamond_skip_price;

			// Token: 0x040060A4 RID: 24740
			public int medium_diamond_skip_price;

			// Token: 0x040060A5 RID: 24741
			public int hard_diamond_skip_price;

			// Token: 0x040060A6 RID: 24742
			public DayOfWeek reset_time_day;

			// Token: 0x040060A7 RID: 24743
			public int reset_time_minutes;
		}
	}
}
