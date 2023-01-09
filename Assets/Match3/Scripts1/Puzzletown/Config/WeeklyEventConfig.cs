using System;
using System.Linq;
using Match3.Scripts1.Puzzletown.Services;

namespace Match3.Scripts1.Puzzletown.Config
{
	// Token: 0x0200096D RID: 2413
	[Serializable]
	public class WeeklyEventConfig
	{
		// Token: 0x170008F8 RID: 2296
		// (get) Token: 0x06003ADE RID: 15070 RVA: 0x00123A69 File Offset: 0x00121E69
		public Materials FirstRewards
		{
			get
			{
				return new Materials(this.firstRewards);
			}
		}

		// Token: 0x170008F9 RID: 2297
		// (get) Token: 0x06003ADF RID: 15071 RVA: 0x00123A76 File Offset: 0x00121E76
		public Materials SecondRewards
		{
			get
			{
				return new Materials(this.secondRewards);
			}
		}

		// Token: 0x170008FA RID: 2298
		// (get) Token: 0x06003AE0 RID: 15072 RVA: 0x00123A83 File Offset: 0x00121E83
		public Materials ThirdRewards
		{
			get
			{
				return new Materials(this.thirdRewards);
			}
		}

		// Token: 0x040062BE RID: 25278
		public SeasonConfig season_config;

		// Token: 0x040062BF RID: 25279
		public WeeklyEventConfig.Sale sale;

		// Token: 0x040062C0 RID: 25280
		public static string TypeName = "weekly_event";

		// Token: 0x040062C1 RID: 25281
		public WeeklyEventType weeklyEventType;

		// Token: 0x040062C2 RID: 25282
		public int levelSet;

		// Token: 0x040062C3 RID: 25283
		public WeeklyEventConfig.Balancing balancing;

		// Token: 0x040062C4 RID: 25284
		public bool shouldOverwrite;

		// Token: 0x040062C5 RID: 25285
		public MaterialAmount[] firstRewards;

		// Token: 0x040062C6 RID: 25286
		public MaterialAmount[] secondRewards;

		// Token: 0x040062C7 RID: 25287
		public MaterialAmount[] thirdRewards;

		// Token: 0x0200096E RID: 2414
		[Serializable]
		public class Balancing
		{
			// Token: 0x06003AE3 RID: 15075 RVA: 0x00123AA4 File Offset: 0x00121EA4
			public bool IsRewardLevel(int level)
			{
				return this.rewardLevels.Contains(level);
			}

			// Token: 0x040062C8 RID: 25288
			public int unlock_level;

			// Token: 0x040062C9 RID: 25289
			public int[] rewardLevels;

			// Token: 0x040062CA RID: 25290
			public int check_point;
		}

		// Token: 0x0200096F RID: 2415
		[Serializable]
		public class Sale
		{
			// Token: 0x040062CB RID: 25291
			public string titleLocaKey;

			// Token: 0x040062CC RID: 25292
			public string configName;
		}
	}
}
