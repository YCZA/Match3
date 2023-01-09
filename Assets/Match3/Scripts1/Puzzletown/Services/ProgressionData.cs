using System;
using System.Collections.Generic;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x020007E4 RID: 2020
	[Serializable]
	public class ProgressionData
	{
		// Token: 0x04005A72 RID: 23154
		public string currentTutorial;

		// Token: 0x04005A73 RID: 23155
		public List<int> tiers = new List<int>();

		// Token: 0x04005A74 RID: 23156
		public int cleanupIndex;

		// Token: 0x04005A75 RID: 23157
		public int lastUnlockedArea;

		// Token: 0x04005A76 RID: 23158
		public List<string> completedTutorials = new List<string>();

		// Token: 0x04005A77 RID: 23159
		public bool hasOpenedTropicam;

		// Token: 0x04005A78 RID: 23160
		public int endOfContentReachedAtArea;
	}
}
