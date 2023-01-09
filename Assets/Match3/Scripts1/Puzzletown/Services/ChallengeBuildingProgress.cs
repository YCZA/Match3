using System;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x0200075C RID: 1884
	[Serializable]
	public class ChallengeBuildingProgress
	{
		// Token: 0x06002EB8 RID: 11960 RVA: 0x000DA1FE File Offset: 0x000D85FE
		public ChallengeBuildingProgress(string buildingName, int currentAmount, int lastSeenAmount, int targetAmount)
		{
			this.buildingName = buildingName;
			this.currentAmount = currentAmount;
			this.lastSeenAmount = lastSeenAmount;
			this.targetAmount = targetAmount;
		}

		// Token: 0x17000744 RID: 1860
		// (get) Token: 0x06002EB9 RID: 11961 RVA: 0x000DA223 File Offset: 0x000D8623
		public bool IsCompleted
		{
			get
			{
				return this.currentAmount >= this.targetAmount;
			}
		}

		// Token: 0x040057EA RID: 22506
		public string buildingName;

		// Token: 0x040057EB RID: 22507
		public int currentAmount;

		// Token: 0x040057EC RID: 22508
		public int lastSeenAmount;

		// Token: 0x040057ED RID: 22509
		public int targetAmount;
	}
}
