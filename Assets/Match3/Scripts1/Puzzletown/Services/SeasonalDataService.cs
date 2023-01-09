using System;
using Match3.Scripts1.Puzzletown.Config;
using Match3.Scripts2.PlayerData;

namespace Match3.Scripts1.Puzzletown.Services
{
	// eli key point seasonal数据服务
	public class SeasonalDataService : ADataService
	{
		// Token: 0x060030A2 RID: 12450 RVA: 0x000E4187 File Offset: 0x000E2587
		public SeasonalDataService(Func<GameState> i_getState) : base(i_getState)
		{
			this.Migrate();
		}

		// Token: 0x060030A3 RID: 12451 RVA: 0x000E4198 File Offset: 0x000E2598
		private void Migrate()
		{
			SeasonalData seasonalPromoData = base.state.seasonalPromoData;
			if (!string.IsNullOrEmpty(seasonalPromoData.currentName))
			{
				seasonalPromoData.current = new SeasonPrizeInfo(seasonalPromoData.currentName, seasonalPromoData.grandPrizeProgress, SeasonPricingInfo.CreateDefault(), 0);
				seasonalPromoData.currentName = null;
				seasonalPromoData.grandPrizeProgress = 0;
			}
		}

		// Token: 0x060030A4 RID: 12452 RVA: 0x000E41EC File Offset: 0x000E25EC
		private int GetGrandPrizeProgress()
		{
			SeasonalData seasonalPromoData = base.state.seasonalPromoData;
			if (seasonalPromoData.current != null)
			{
				return seasonalPromoData.current.grandPrizeProgress;
			}
			return 0;
		}

		// Token: 0x060030A5 RID: 12453 RVA: 0x000E4220 File Offset: 0x000E2620
		public void AddGrandPrizeProgress(int progress)
		{
			SeasonalData seasonalPromoData = base.state.seasonalPromoData;
			if (seasonalPromoData.current != null)
			{
				SeasonPrizeInfo current = seasonalPromoData.current;
				current.grandPrizeProgress = this.GetGrandPrizeProgress() + progress;
			}
		}

		// Token: 0x170007A7 RID: 1959
		// (get) Token: 0x060030A6 RID: 12454 RVA: 0x000E4259 File Offset: 0x000E2659
		// (set) Token: 0x060030A7 RID: 12455 RVA: 0x000E426B File Offset: 0x000E266B
		public SeasonPrizeInfo Current
		{
			get
			{
				return base.state.seasonalPromoData.current;
			}
			set
			{
				base.state.seasonalPromoData.current = value;
			}
		}

		// Token: 0x170007A8 RID: 1960
		// (get) Token: 0x060030A8 RID: 12456 RVA: 0x000E427E File Offset: 0x000E267E
		// (set) Token: 0x060030A9 RID: 12457 RVA: 0x000E4290 File Offset: 0x000E2690
		public SeasonPrizeInfo Previous
		{
			get
			{
				return base.state.seasonalPromoData.previous;
			}
			set
			{
				base.state.seasonalPromoData.previous = value;
			}
		}

		// Token: 0x170007A9 RID: 1961
		// (get) Token: 0x060030AA RID: 12458 RVA: 0x000E42A3 File Offset: 0x000E26A3
		// (set) Token: 0x060030AB RID: 12459 RVA: 0x000E42B5 File Offset: 0x000E26B5
		public int LastSeenSeasonalPromoTimeStamp
		{
			get
			{
				return base.state.seasonalPromoData.lastSeenSeasonalPromo;
			}
			set
			{
				base.state.seasonalPromoData.lastSeenSeasonalPromo = value;
			}
		}
	}
}
