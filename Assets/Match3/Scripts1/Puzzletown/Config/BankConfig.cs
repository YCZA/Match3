using System;

namespace Match3.Scripts1.Puzzletown.Config
{
	// Token: 0x020008E3 RID: 2275
	[Serializable]
	public class BankConfig
	{
		// Token: 0x04005F70 RID: 24432
		public BankConfig.Balancing balancing;

		// Token: 0x020008E4 RID: 2276
		[Serializable]
		public class Balancing
		{
			// Token: 0x04005F71 RID: 24433
			public int easy_diamond_reward;

			// Token: 0x04005F72 RID: 24434
			public int medium_diamond_reward;

			// Token: 0x04005F73 RID: 24435
			public int hard_diamond_reward;

			// Token: 0x04005F74 RID: 24436
			public int open_threshold;

			// Token: 0x04005F75 RID: 24437
			public int full_amount;

			// Token: 0x04005F76 RID: 24438
			public int unlock_level;

			// Token: 0x04005F77 RID: 24439
			public int hours_between_auto_show;
		}
	}
}
