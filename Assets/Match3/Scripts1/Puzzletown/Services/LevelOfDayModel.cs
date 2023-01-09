using System;
using System.Collections.Generic;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x020007CF RID: 1999
	[Serializable]
	public class LevelOfDayModel
	{
		// Token: 0x06003132 RID: 12594 RVA: 0x000E73D8 File Offset: 0x000E57D8
		public int CompareProgress(LevelOfDayModel other)
		{
			if (this.endUTCTime != other.endUTCTime)
			{
				return (this.endUTCTime <= other.endUTCTime) ? -1 : 1;
			}
			if (this.currentDay != other.currentDay)
			{
				return (this.currentDay <= other.currentDay) ? -1 : 1;
			}
			return 0;
		}

		// Token: 0x040059E3 RID: 23011
		public int level;

		// Token: 0x040059E4 RID: 23012
		public int triesSoFar;

		// Token: 0x040059E5 RID: 23013
		public bool isCompleted;

		// Token: 0x040059E6 RID: 23014
		public int endUTCTime;

		// Token: 0x040059E7 RID: 23015
		public bool notificationSeen;

		// Token: 0x040059E8 RID: 23016
		public int currentDay;

		// Token: 0x040059E9 RID: 23017
		public List<int> lodHistory;
	}
}
