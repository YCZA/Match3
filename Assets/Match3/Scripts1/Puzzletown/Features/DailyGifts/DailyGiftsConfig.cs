using System;
using System.Collections.Generic;

namespace Match3.Scripts1.Puzzletown.Features.DailyGifts
{
	// Token: 0x020008D3 RID: 2259
	[Serializable]
	public class DailyGiftsConfig
	{
		// Token: 0x04005F2B RID: 24363
		public List<DailyGiftsConfig.Day> days;

		// Token: 0x04005F2C RID: 24364
		public General general;

		// Token: 0x020008D4 RID: 2260
		[Serializable]
		public class Day
		{
			// Token: 0x1700086B RID: 2155
			// (get) Token: 0x0600370F RID: 14095 RVA: 0x0010CC40 File Offset: 0x0010B040
			public MaterialAmount[] Rewards
			{
				get
				{
					if (this.rewards == null)
					{
						this.rewards = new MaterialAmount[]
						{
							new MaterialAmount(this.reward_type, this.reward_amount, MaterialAmountUsage.Undefined, 0),
							new MaterialAmount(this.bonus_type, this.bonus_amount, MaterialAmountUsage.Undefined, 0)
						};
					}
					return this.rewards;
				}
			}

			// Token: 0x1700086C RID: 2156
			// (get) Token: 0x06003710 RID: 14096 RVA: 0x0010CCA8 File Offset: 0x0010B0A8
			// (set) Token: 0x06003711 RID: 14097 RVA: 0x0010CCB0 File Offset: 0x0010B0B0
			public int adjustedDay { get; set; }

			// Token: 0x04005F2D RID: 24365
			public int day;

			// Token: 0x04005F2E RID: 24366
			public int reward_amount;

			// Token: 0x04005F2F RID: 24367
			public string reward_type;

			// Token: 0x04005F30 RID: 24368
			public int bonus_amount;

			// Token: 0x04005F31 RID: 24369
			public string bonus_type;

			// Token: 0x04005F32 RID: 24370
			public bool is_jackpot;

			// Token: 0x04005F33 RID: 24371
			private MaterialAmount[] rewards;
		}
	}
}
