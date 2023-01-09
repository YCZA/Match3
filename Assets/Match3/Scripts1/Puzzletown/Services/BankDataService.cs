using System;
using Match3.Scripts2.PlayerData;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x02000750 RID: 1872
	public class BankDataService : AWeeklyEventDataService
	{
		// Token: 0x06002E5A RID: 11866 RVA: 0x000D8C09 File Offset: 0x000D7009
		public BankDataService(Func<GameState> getState, ConfigService configService) : base(getState)
		{
			this.configService = configService;
		}

		// Token: 0x17000723 RID: 1827
		// (get) Token: 0x06002E5B RID: 11867 RVA: 0x000D8C19 File Offset: 0x000D7019
		// (set) Token: 0x06002E5C RID: 11868 RVA: 0x000D8C31 File Offset: 0x000D7031
		public DateTime NextAutoShowTime
		{
			get
			{
				return Scripts1.DateTimeExtensions.FromUnixTimeStamp(base.state.bankData.nextAutoShowTime, DateTimeKind.Utc);
			}
			set
			{
				base.state.bankData.nextAutoShowTime = value.ToUniversalTime().ToUnixTimeStamp();
			}
		}

		// Token: 0x17000724 RID: 1828
		// (get) Token: 0x06002E5D RID: 11869 RVA: 0x000D8C4F File Offset: 0x000D704F
		// (set) Token: 0x06002E5E RID: 11870 RVA: 0x000D8C67 File Offset: 0x000D7067
		public int NumberOfBankedDiamonds
		{
			get
			{
				this.TryResetIfEventChanged();
				return base.state.bankData.numberOfBankedDiamonds;
			}
			set
			{
				base.state.bankData.numberOfBankedDiamonds = value;
			}
		}

		// Token: 0x17000725 RID: 1829
		// (get) Token: 0x06002E5F RID: 11871 RVA: 0x000D8C7A File Offset: 0x000D707A
		// (set) Token: 0x06002E60 RID: 11872 RVA: 0x000D8C92 File Offset: 0x000D7092
		public int NumberOfDiamondsLastSeen
		{
			get
			{
				this.TryResetIfEventChanged();
				return base.state.bankData.numberOfDiamondsLastSeen;
			}
			set
			{
				base.state.bankData.numberOfDiamondsLastSeen = value;
			}
		}

		// Token: 0x17000726 RID: 1830
		// (get) Token: 0x06002E61 RID: 11873 RVA: 0x000D8CA5 File Offset: 0x000D70A5
		// (set) Token: 0x06002E62 RID: 11874 RVA: 0x000D8CB7 File Offset: 0x000D70B7
		public int TotalNumberOfPigyBanksLifeTime
		{
			get
			{
				return base.state.bankData.totalNumberOfPigyBanksLifeTime;
			}
			set
			{
				base.state.bankData.totalNumberOfPigyBanksLifeTime = value;
			}
		}

		// Token: 0x17000727 RID: 1831
		// (get) Token: 0x06002E63 RID: 11875 RVA: 0x000D8CCA File Offset: 0x000D70CA
		public bool IsCurrentEventBought
		{
			get
			{
				return base.EventId != null && base.EventId.Equals(base.state.bankData.lastBoughtEventId);
			}
		}

		// Token: 0x06002E64 RID: 11876 RVA: 0x000D8CF5 File Offset: 0x000D70F5
		public void OnBuy()
		{
			this.ResetBankDiamonds();
			base.state.bankData.lastBoughtEventId = base.EventId;
			this.TotalNumberOfPigyBanksLifeTime++;
		}

		// Token: 0x06002E65 RID: 11877 RVA: 0x000D8D21 File Offset: 0x000D7121
		public void DebugReset()
		{
			this.ResetBankDiamonds();
			base.state.bankData.lastBoughtEventId = string.Empty;
		}

		// Token: 0x06002E66 RID: 11878 RVA: 0x000D8D3E File Offset: 0x000D713E
		private void ResetBankDiamonds()
		{
			base.state.bankData.numberOfBankedDiamonds = 0;
			base.state.bankData.numberOfDiamondsLastSeen = 0;
			this.NextAutoShowTime = DateTime.UtcNow;
		}

		// Token: 0x06002E67 RID: 11879 RVA: 0x000D8D70 File Offset: 0x000D7170
		private void TryResetIfEventChanged()
		{
			if (!this.configService.SbsConfig.feature_switches.piggy_bank_as_event)
			{
				return;
			}
			if (!string.IsNullOrEmpty(base.EventId) && base.EventId != base.state.bankData.currentCountingEventId)
			{
				if (!string.IsNullOrEmpty(base.state.bankData.currentCountingEventId))
				{
					this.ResetBankDiamonds();
				}
				base.state.bankData.currentCountingEventId = base.EventId;
			}
		}

		// Token: 0x17000728 RID: 1832
		// (get) Token: 0x06002E68 RID: 11880 RVA: 0x000D8DFE File Offset: 0x000D71FE
		public override string ConfigKey
		{
			get
			{
				return "BankEventConfig";
			}
		}

		// Token: 0x17000729 RID: 1833
		// (get) Token: 0x06002E69 RID: 11881 RVA: 0x000D8E05 File Offset: 0x000D7205
		public override WeeklyEventData EventData
		{
			get
			{
				return base.state.bankData;
			}
		}

		// Token: 0x040057B6 RID: 22454
		private const string CONFIG_KEY = "BankEventConfig";

		// Token: 0x040057B7 RID: 22455
		private readonly ConfigService configService;
	}
}
