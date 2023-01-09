using System;
using System.Collections.Generic;
using Match3.Scripts2.PlayerData;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x020007A0 RID: 1952
	public class AdViewDataService : ADataService
	{
		// Token: 0x06002FD2 RID: 12242 RVA: 0x000E0FA3 File Offset: 0x000DF3A3
		public AdViewDataService(Func<GameState> i_getState) : base(i_getState)
		{
		}

		// Token: 0x17000775 RID: 1909
		// (get) Token: 0x06002FD3 RID: 12243 RVA: 0x000E0FAC File Offset: 0x000DF3AC
		public int TotalAdsWatchedToday
		{
			get
			{
				return this.GetNumberOfAdsWatchedToday(AdPlacement.Unspecified);
			}
		}

		// Token: 0x06002FD4 RID: 12244 RVA: 0x000E0FB8 File Offset: 0x000DF3B8
		public int GetNumberOfAdsWatchedToday(AdPlacement placement)
		{
			if (base.state == null || base.state.adSpinData == null)
			{
				return 0;
			}
			if (placement == AdPlacement.Unspecified)
			{
				return base.state.adSpinData.numberOfVideosWatchToday;
			}
			string adPlacementAsString = VideoAdHelper.GetAdPlacementAsString(placement);
			if (base.state.adSpinData.adsWatchedToday != null)
			{
				foreach (WatchedAd watchedAd in base.state.adSpinData.adsWatchedToday)
				{
					if (watchedAd.placement == adPlacementAsString)
					{
						return watchedAd.count;
					}
				}
				return 0;
			}
			return 0;
		}

		// Token: 0x06002FD5 RID: 12245 RVA: 0x000E1088 File Offset: 0x000DF488
		public void SetNumberOfAdsWatchedToday(AdPlacement placement, int count)
		{
			if (base.state == null || base.state.adSpinData == null)
			{
				return;
			}
			if (placement == AdPlacement.Unspecified)
			{
				base.state.adSpinData.numberOfVideosWatchToday = count;
				return;
			}
			if (base.state.adSpinData.adsWatchedToday == null)
			{
				base.state.adSpinData.adsWatchedToday = new List<WatchedAd>();
			}
			WatchedAd watchedAd = new WatchedAd(VideoAdHelper.GetAdPlacementAsString(placement), count);
			if (!this.TryUpdateExistingAdCountEntry(watchedAd))
			{
				base.state.adSpinData.adsWatchedToday.Add(watchedAd);
			}
			this.UpdateTotalAdsWatchedCount();
		}

		// Token: 0x06002FD6 RID: 12246 RVA: 0x000E1128 File Offset: 0x000DF528
		private bool TryUpdateExistingAdCountEntry(WatchedAd currentAd)
		{
			foreach (WatchedAd watchedAd in base.state.adSpinData.adsWatchedToday)
			{
				if (watchedAd.placement == currentAd.placement)
				{
					watchedAd.count = currentAd.count;
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002FD7 RID: 12247 RVA: 0x000E11B4 File Offset: 0x000DF5B4
		private void UpdateTotalAdsWatchedCount()
		{
			string adPlacementAsString = VideoAdHelper.GetAdPlacementAsString(AdPlacement.Unspecified);
			int num = 0;
			foreach (WatchedAd watchedAd in base.state.adSpinData.adsWatchedToday)
			{
				if (watchedAd.placement != adPlacementAsString)
				{
					num += watchedAd.count;
				}
			}
			base.state.adSpinData.numberOfVideosWatchToday = num;
		}

		// Token: 0x06002FD8 RID: 12248 RVA: 0x000E1248 File Offset: 0x000DF648
		public void ClearDailyAdCounters()
		{
			if (base.state.adSpinData.adsWatchedToday == null)
			{
				base.state.adSpinData.adsWatchedToday = new List<WatchedAd>();
			}
			else
			{
				base.state.adSpinData.adsWatchedToday.Clear();
			}
			this.UpdateTotalAdsWatchedCount();
		}

		// Token: 0x06002FD9 RID: 12249 RVA: 0x000E12A0 File Offset: 0x000DF6A0
		public void IncreaseViewCount(AdPlacement placement)
		{
			int numberOfAdsWatchedToday = this.GetNumberOfAdsWatchedToday(placement);
			this.SetNumberOfAdsWatchedToday(placement, numberOfAdsWatchedToday + 1);
		}

		// Token: 0x06002FDA RID: 12250 RVA: 0x000E12BF File Offset: 0x000DF6BF
		public bool CanWatchAd(int globalLimit, AdPlacement placement, int specificLimit)
		{
			return base.state != null && base.state.adSpinData != null && this.TotalAdsWatchedToday < globalLimit && this.GetNumberOfAdsWatchedToday(placement) < specificLimit;
		}
	}
}
