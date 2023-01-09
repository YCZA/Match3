using System;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x02000751 RID: 1873
	[Serializable]
	public class BankData : WeeklyEventData
	{
		// Token: 0x040057B8 RID: 22456
		public int numberOfBankedDiamonds;

		// Token: 0x040057B9 RID: 22457
		public int numberOfDiamondsLastSeen;

		// Token: 0x040057BA RID: 22458
		public int nextAutoShowTime;

		// Token: 0x040057BB RID: 22459
		public int totalNumberOfPigyBanksLifeTime = 1;

		// Token: 0x040057BC RID: 22460
		public string currentCountingEventId;

		// Token: 0x040057BD RID: 22461
		public string lastBoughtEventId;
	}
}
