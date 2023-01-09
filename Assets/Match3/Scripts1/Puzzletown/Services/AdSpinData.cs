using System;
using System.Collections.Generic;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x020007AB RID: 1963
	[Serializable]
	public class AdSpinData
	{
		// Token: 0x04005927 RID: 22823
		public int jackpotProgress;

		// Token: 0x04005928 RID: 22824
		public int unlimitedLivesEnd;

		// Token: 0x04005929 RID: 22825
		public int unlimitedLivesStart;

		// Token: 0x0400592A RID: 22826
		public int nextSpinAvailable;

		// Token: 0x0400592B RID: 22827
		public int lastWatchedAdData;

		// Token: 0x0400592C RID: 22828
		public int numberOfVideosWatchToday;

		// Token: 0x0400592D RID: 22829
		public List<WatchedAd> adsWatchedToday;
	}
}
