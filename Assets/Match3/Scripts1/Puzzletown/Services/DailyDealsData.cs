using System;
using Match3.Scripts1.Puzzletown.Config;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x02000764 RID: 1892
	[Serializable]
	public class DailyDealsData
	{
		// Token: 0x04005820 RID: 22560
		public DailyDealsConfig.Deal currentDeal;

		// Token: 0x04005821 RID: 22561
		public int currentDealExpirationDate;

		// Token: 0x04005822 RID: 22562
		public int dealDayNumber;

		// Token: 0x04005823 RID: 22563
		public bool currentDealPurchased;
	}
}
