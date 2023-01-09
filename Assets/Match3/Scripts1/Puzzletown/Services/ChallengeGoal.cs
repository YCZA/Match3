using System;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x0200075A RID: 1882
	[Serializable]
	public class ChallengeGoal
	{
		// Token: 0x06002EB2 RID: 11954 RVA: 0x000DA15C File Offset: 0x000D855C
		public ChallengeGoal(string id, string type, int start, int goal, bool collected, DateTime collectedTime, ChallengeGoal.ChallengeDifficulty difficulty)
		{
			this.id = id;
			this.type = type;
			this.start = start;
			this.lastViewed = start;
			this.goal = goal;
			this.collected = collected;
			this.CollectedTime = collectedTime;
			this.difficulty = difficulty;
		}

		// Token: 0x17000740 RID: 1856
		// (get) Token: 0x06002EB3 RID: 11955 RVA: 0x000DA1AB File Offset: 0x000D85AB
		public int TargetAmount
		{
			get
			{
				return this.start + this.goal;
			}
		}

		// Token: 0x17000741 RID: 1857
		// (get) Token: 0x06002EB4 RID: 11956 RVA: 0x000DA1BA File Offset: 0x000D85BA
		public int AmountCollectedAndViewed
		{
			get
			{
				return this.lastViewed - this.start;
			}
		}

		// Token: 0x17000742 RID: 1858
		// (get) Token: 0x06002EB5 RID: 11957 RVA: 0x000DA1C9 File Offset: 0x000D85C9
		public bool DidSeeCompleted
		{
			get
			{
				return this.AmountCollectedAndViewed >= this.goal;
			}
		}

		// Token: 0x17000743 RID: 1859
		// (get) Token: 0x06002EB6 RID: 11958 RVA: 0x000DA1DC File Offset: 0x000D85DC
		// (set) Token: 0x06002EB7 RID: 11959 RVA: 0x000DA1EA File Offset: 0x000D85EA
		public DateTime CollectedTime
		{
			get
			{
				return Scripts1.DateTimeExtensions.FromUnixTimeStamp(this.collectedTime, DateTimeKind.Utc);
			}
			set
			{
				this.collectedTime = value.ToUniversalTime().ToUnixTimeStamp();
			}
		}

		// Token: 0x040057DE RID: 22494
		public string id;

		// Token: 0x040057DF RID: 22495
		public string type;

		// Token: 0x040057E0 RID: 22496
		public int start;

		// Token: 0x040057E1 RID: 22497
		public int lastViewed;

		// Token: 0x040057E2 RID: 22498
		public int goal;

		// Token: 0x040057E3 RID: 22499
		public bool collected;

		// Token: 0x040057E4 RID: 22500
		public int collectedTime;

		// Token: 0x040057E5 RID: 22501
		public ChallengeGoal.ChallengeDifficulty difficulty;

		// Token: 0x0200075B RID: 1883
		public enum ChallengeDifficulty
		{
			// Token: 0x040057E7 RID: 22503
			easy,
			// Token: 0x040057E8 RID: 22504
			medium,
			// Token: 0x040057E9 RID: 22505
			hard
		}
	}
}
