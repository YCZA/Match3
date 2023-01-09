using System;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x020007AE RID: 1966
	[Serializable]
	public class SeasonalData
	{
		// Token: 0x04005936 RID: 22838
		public string currentName;

		// Token: 0x04005937 RID: 22839
		public int lastSeenSeasonalPromo;

		// Token: 0x04005938 RID: 22840
		public int grandPrizeProgress;

		// Token: 0x04005939 RID: 22841
		public SeasonPrizeInfo current;

		// Token: 0x0400593A RID: 22842
		public SeasonPrizeInfo previous;
	}
}
