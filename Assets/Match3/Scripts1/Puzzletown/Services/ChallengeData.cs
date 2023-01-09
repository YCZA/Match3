using System;
using System.Collections.Generic;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x02000759 RID: 1881
	[Serializable]
	public class ChallengeData
	{
		// Token: 0x040057D5 RID: 22485
		public int currentAdBonus;

		// Token: 0x040057D6 RID: 22486
		public int numberOfBoughtExtentions;

		// Token: 0x040057D7 RID: 22487
		public int currentDecoSet;

		// Token: 0x040057D8 RID: 22488
		public int challengeStartTime;

		// Token: 0x040057D9 RID: 22489
		public int challengeExpireTime;

		// Token: 0x040057DA RID: 22490
		public int decoExpireTime;

		// Token: 0x040057DB RID: 22491
		public int numberOfRewardsEarned;

		// Token: 0x040057DC RID: 22492
		public List<ChallengeGoal> currentChallenges;

		// Token: 0x040057DD RID: 22493
		public List<ChallengeBuildingProgress> buildingProgress;
	}
}
