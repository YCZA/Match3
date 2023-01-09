using System;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts2.PlayerData;

namespace Match3.Scripts1.Puzzletown.Features.DailyGifts
{
	// Token: 0x020008D7 RID: 2263
	public class DailyGiftsDataService : ADataService
	{
		// Token: 0x06003714 RID: 14100 RVA: 0x0010CCC9 File Offset: 0x0010B0C9
		public DailyGiftsDataService(Func<GameState> i_getState) : base(i_getState)
		{
		}

		// Token: 0x1700086D RID: 2157
		// (get) Token: 0x06003715 RID: 14101 RVA: 0x0010CCD2 File Offset: 0x0010B0D2
		// (set) Token: 0x06003716 RID: 14102 RVA: 0x0010CCE4 File Offset: 0x0010B0E4
		public int NumConsecutiveDays
		{
			get
			{
				return base.state.dailyGiftData.numConsecutiveDays;
			}
			set
			{
				base.state.dailyGiftData.numConsecutiveDays = value;
			}
		}

		// Token: 0x1700086E RID: 2158
		// (get) Token: 0x06003717 RID: 14103 RVA: 0x0010CCF7 File Offset: 0x0010B0F7
		// (set) Token: 0x06003718 RID: 14104 RVA: 0x0010CD09 File Offset: 0x0010B109
		public int LastReceived
		{
			get
			{
				return base.state.dailyGiftData.lastReceived;
			}
			set
			{
				base.state.dailyGiftData.lastReceived = value;
			}
		}
	}
}
