using System;
using Match3.Scripts2.PlayerData;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x020007F0 RID: 2032
	public class SaleDataService : AWeeklyEventDataService
	{
		// Token: 0x06003250 RID: 12880 RVA: 0x000ECC57 File Offset: 0x000EB057
		public SaleDataService(Func<GameState> getState) : base(getState)
		{
		}

		// Token: 0x06003251 RID: 12881 RVA: 0x000ECC60 File Offset: 0x000EB060
		public void MarkSeenToday()
		{
			base.state.saleData.lastSeen = DateTime.Today.ToUnixTimeStamp();
		}

		// Token: 0x06003252 RID: 12882 RVA: 0x000ECC7C File Offset: 0x000EB07C
		public bool NotSeenToday()
		{
			int num = DateTime.Today.ToUnixTimeStamp();
			return base.state.saleData.lastSeen < num;
		}

		// Token: 0x170007E9 RID: 2025
		// (get) Token: 0x06003253 RID: 12883 RVA: 0x000ECCA7 File Offset: 0x000EB0A7
		// (set) Token: 0x06003254 RID: 12884 RVA: 0x000ECCB9 File Offset: 0x000EB0B9
		public string BoughtSaleId
		{
			get
			{
				return base.state.saleData.lastBoughtSaleId;
			}
			set
			{
				base.state.saleData.lastBoughtSaleId = value;
			}
		}

		// Token: 0x170007EA RID: 2026
		// (get) Token: 0x06003255 RID: 12885 RVA: 0x000ECCCC File Offset: 0x000EB0CC
		public string CurrentSaleConfigName
		{
			get
			{
				return base.state.saleData.currentConfigName;
			}
		}

		// Token: 0x170007EB RID: 2027
		// (get) Token: 0x06003256 RID: 12886 RVA: 0x000ECCDE File Offset: 0x000EB0DE
		public override string ConfigKey
		{
			get
			{
				return "SaleConfigV2";
			}
		}

		// Token: 0x170007EC RID: 2028
		// (get) Token: 0x06003257 RID: 12887 RVA: 0x000ECCE5 File Offset: 0x000EB0E5
		public override WeeklyEventData EventData
		{
			get
			{
				return base.state.saleData;
			}
		}

		// Token: 0x04005ACD RID: 23245
		private const string CONFIG_KEY = "SaleConfigV2";
	}
}
