using System;
using System.Collections;
using Wooga.UnityFramework;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x02000829 RID: 2089
	public interface IVideoAdService : IService, IInitializable
	{
		// Token: 0x17000825 RID: 2085
		// (get) Token: 0x060033E2 RID: 13282
		bool HasUnlockedVideoAd { get; }

		// Token: 0x17000826 RID: 2086
		// (get) Token: 0x060033E3 RID: 13283
		DateTime LastAdTime { get; }

		// Token: 0x17000827 RID: 2087
		// (get) Token: 0x060033E4 RID: 13284
		// (set) Token: 0x060033E5 RID: 13285
		bool FreeSpinAvailable { get; set; }

		// Token: 0x17000828 RID: 2088
		// (get) Token: 0x060033E6 RID: 13286
		string FreeSpinVideoAdId { get; }

		// Token: 0x060033E7 RID: 13287
		bool IsAllowedToWatchAd(AdPlacement placement);

		// Token: 0x060033E8 RID: 13288
		bool IsVideoAvailable(bool tryRefresh = true);

		// Token: 0x060033E9 RID: 13289
		IEnumerator ShowAd(AdPlacement placement);

		// Token: 0x060033EA RID: 13290
		void TrackClaim();

		// Token: 0x060033EB RID: 13291
		void ChangeSetting(ToggleSetting setting, bool value);

		// Token: 0x060033EC RID: 13292
		void DebugShowTestSuite();

		// Token: 0x060033ED RID: 13293
		void EditorPretendYouJustWatchedAnAd(AdPlacement placement);
	}
}
