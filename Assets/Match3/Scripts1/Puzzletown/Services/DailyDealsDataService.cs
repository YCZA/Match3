using System;
using Match3.Scripts1.Puzzletown.Config;
using Match3.Scripts2.PlayerData;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x02000763 RID: 1891
	public class DailyDealsDataService : ADataService
	{
		// Token: 0x06002EF1 RID: 12017 RVA: 0x000DB621 File Offset: 0x000D9A21
		public DailyDealsDataService(Func<GameState> getState) : base(getState)
		{
		}

		// Token: 0x17000750 RID: 1872
		// (get) Token: 0x06002EF2 RID: 12018 RVA: 0x000DB62A File Offset: 0x000D9A2A
		// (set) Token: 0x06002EF3 RID: 12019 RVA: 0x000DB63C File Offset: 0x000D9A3C
		public DailyDealsConfig.Deal CurrentDeal
		{
			get
			{
				return base.state.dailyDealsData.currentDeal;
			}
			set
			{
				base.state.dailyDealsData.currentDeal = value;
			}
		}

		// Token: 0x17000751 RID: 1873
		// (get) Token: 0x06002EF4 RID: 12020 RVA: 0x000DB64F File Offset: 0x000D9A4F
		// (set) Token: 0x06002EF5 RID: 12021 RVA: 0x000DB667 File Offset: 0x000D9A67
		public DateTime CurrentDealExpirationDate
		{
			get
			{
				return Scripts1.DateTimeExtensions.FromUnixTimeStamp(base.state.dailyDealsData.currentDealExpirationDate, DateTimeKind.Local);
			}
			set
			{
				base.state.dailyDealsData.currentDealExpirationDate = value.ToUnixTimeStamp();
			}
		}

		// Token: 0x17000752 RID: 1874
		// (get) Token: 0x06002EF6 RID: 12022 RVA: 0x000DB67F File Offset: 0x000D9A7F
		// (set) Token: 0x06002EF7 RID: 12023 RVA: 0x000DB691 File Offset: 0x000D9A91
		public bool CurrentDealPurchased
		{
			get
			{
				return base.state.dailyDealsData.currentDealPurchased;
			}
			set
			{
				base.state.dailyDealsData.currentDealPurchased = value;
			}
		}

		// Token: 0x17000753 RID: 1875
		// (get) Token: 0x06002EF8 RID: 12024 RVA: 0x000DB6A4 File Offset: 0x000D9AA4
		// (set) Token: 0x06002EF9 RID: 12025 RVA: 0x000DB6B6 File Offset: 0x000D9AB6
		public int CurrentDealDayNumber
		{
			get
			{
				return base.state.dailyDealsData.dealDayNumber;
			}
			set
			{
				base.state.dailyDealsData.dealDayNumber = value;
			}
		}

		// Token: 0x17000754 RID: 1876
		// (get) Token: 0x06002EFA RID: 12026 RVA: 0x000DB6C9 File Offset: 0x000D9AC9
		public bool CurrentDealActive
		{
			get
			{
				// eli key point 限时礼包
				// eli todo 暂时没有限时礼包
				return false;
				// return this.CurrentDealExpirationDate > DateTime.Now && this.CurrentDealDayNumber != 0;
			}
		}
	}
}
