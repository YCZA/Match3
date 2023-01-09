using System;
using System.Collections.Generic;
using Match3.Scripts2.PlayerData;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x02000758 RID: 1880
	public class ChallengeDataService : ADataService
	{
		// Token: 0x06002E9E RID: 11934 RVA: 0x000D9FC8 File Offset: 0x000D83C8
		public ChallengeDataService(Func<GameState> getState) : base(getState)
		{
		}

		// Token: 0x17000737 RID: 1847
		// (get) Token: 0x06002E9F RID: 11935 RVA: 0x000D9FD1 File Offset: 0x000D83D1
		// (set) Token: 0x06002EA0 RID: 11936 RVA: 0x000D9FE9 File Offset: 0x000D83E9
		public DateTime DecoSetExpireTime
		{
			get
			{
				return Scripts1.DateTimeExtensions.FromUnixTimeStamp(base.state.challengeData.decoExpireTime, DateTimeKind.Utc);
			}
			set
			{
				base.state.challengeData.decoExpireTime = value.ToUniversalTime().ToUnixTimeStamp();
			}
		}

		// Token: 0x17000738 RID: 1848
		// (get) Token: 0x06002EA1 RID: 11937 RVA: 0x000DA007 File Offset: 0x000D8407
		// (set) Token: 0x06002EA2 RID: 11938 RVA: 0x000DA01F File Offset: 0x000D841F
		public DateTime ChallengeExpireTime
		{
			get
			{
				return Scripts1.DateTimeExtensions.FromUnixTimeStamp(base.state.challengeData.challengeExpireTime, DateTimeKind.Utc);
			}
			set
			{
				base.state.challengeData.challengeExpireTime = value.ToUniversalTime().ToUnixTimeStamp();
			}
		}

		// Token: 0x17000739 RID: 1849
		// (get) Token: 0x06002EA3 RID: 11939 RVA: 0x000DA03D File Offset: 0x000D843D
		// (set) Token: 0x06002EA4 RID: 11940 RVA: 0x000DA055 File Offset: 0x000D8455
		public DateTime ChallengeStartTime
		{
			get
			{
				return Scripts1.DateTimeExtensions.FromUnixTimeStamp(base.state.challengeData.challengeStartTime, DateTimeKind.Utc);
			}
			set
			{
				base.state.challengeData.challengeStartTime = value.ToUniversalTime().ToUnixTimeStamp();
			}
		}

		// Token: 0x1700073A RID: 1850
		// (get) Token: 0x06002EA5 RID: 11941 RVA: 0x000DA073 File Offset: 0x000D8473
		// (set) Token: 0x06002EA6 RID: 11942 RVA: 0x000DA085 File Offset: 0x000D8485
		public int CurrentAdBonus
		{
			get
			{
				return base.state.challengeData.currentAdBonus;
			}
			set
			{
				base.state.challengeData.currentAdBonus = value;
			}
		}

		// Token: 0x1700073B RID: 1851
		// (get) Token: 0x06002EA7 RID: 11943 RVA: 0x000DA098 File Offset: 0x000D8498
		// (set) Token: 0x06002EA8 RID: 11944 RVA: 0x000DA0AA File Offset: 0x000D84AA
		public int NumberOfBoughtExtentions
		{
			get
			{
				return base.state.challengeData.numberOfBoughtExtentions;
			}
			set
			{
				base.state.challengeData.numberOfBoughtExtentions = value;
			}
		}

		// Token: 0x1700073C RID: 1852
		// (get) Token: 0x06002EA9 RID: 11945 RVA: 0x000DA0BD File Offset: 0x000D84BD
		// (set) Token: 0x06002EAA RID: 11946 RVA: 0x000DA0CF File Offset: 0x000D84CF
		public int CurrentDecoSet
		{
			get
			{
				return base.state.challengeData.currentDecoSet;
			}
			set
			{
				base.state.challengeData.currentDecoSet = value;
			}
		}

		// Token: 0x1700073D RID: 1853
		// (get) Token: 0x06002EAB RID: 11947 RVA: 0x000DA0E2 File Offset: 0x000D84E2
		// (set) Token: 0x06002EAC RID: 11948 RVA: 0x000DA0F4 File Offset: 0x000D84F4
		public int NumberOfEarnedRewards
		{
			get
			{
				return base.state.challengeData.numberOfRewardsEarned;
			}
			set
			{
				base.state.challengeData.numberOfRewardsEarned = value;
			}
		}

		// Token: 0x1700073E RID: 1854
		// (get) Token: 0x06002EAD RID: 11949 RVA: 0x000DA107 File Offset: 0x000D8507
		// (set) Token: 0x06002EAE RID: 11950 RVA: 0x000DA119 File Offset: 0x000D8519
		public List<ChallengeGoal> CurrentChallenges
		{
			get
			{
				return base.state.challengeData.currentChallenges;
			}
			set
			{
				base.state.challengeData.currentChallenges = value;
			}
		}

		// Token: 0x1700073F RID: 1855
		// (get) Token: 0x06002EAF RID: 11951 RVA: 0x000DA12C File Offset: 0x000D852C
		// (set) Token: 0x06002EB0 RID: 11952 RVA: 0x000DA13E File Offset: 0x000D853E
		public List<ChallengeBuildingProgress> BuildingProgress
		{
			get
			{
				return base.state.challengeData.buildingProgress;
			}
			set
			{
				base.state.challengeData.buildingProgress = value;
			}
		}
	}
}
