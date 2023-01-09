using System;
using Match3.Scripts1.Puzzletown.Config;
using Match3.Scripts1.Puzzletown.Features.DailyGifts;
using Match3.Scripts1.Puzzletown.Match3;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x020007F7 RID: 2039
	[Serializable]
	public class PTConfig
	{
		// Token: 0x06003266 RID: 12902 RVA: 0x000ED01F File Offset: 0x000EB41F
		public void Init()
		{
			this.villagerankconfig.Init();
			this.questconfig.Init();
			this._events.Init();
		}

		// Token: 0x06003267 RID: 12903 RVA: 0x000ED042 File Offset: 0x000EB442
		public bool IsRemote()
		{
			return this._events != null && this._events.events != null;
		}

		// Token: 0x04005ADB RID: 23259
		public FeatureSwitchesConfig feature_switches;

		// Token: 0x04005ADC RID: 23260
		public ForceUpdateConfig force_update;

		// Token: 0x04005ADD RID: 23261
		public SbsTimeoutsConfig sbs_timeouts;

		// Token: 0x04005ADE RID: 23262
		public BundleManifestList assetbundles_hd;

		// Token: 0x04005ADF RID: 23263
		public BundleManifestList assetbundles_sd;

		// Token: 0x04005AE0 RID: 23264
		public PtEventsConfig _events;

		// Token: 0x04005AE1 RID: 23265
		public LevelForeshadowingConfig levelforeshadowingconfig;

		// Token: 0x04005AE2 RID: 23266
		public RewardsConfig level_rewards;

		// Token: 0x04005AE3 RID: 23267
		public ChallengesConfig challenges;

		// Token: 0x04005AE4 RID: 23268
		public BankConfig bank;

		// Token: 0x04005AE5 RID: 23269
		public DailyDealsConfig dailydealsconfig;

		// Token: 0x04005AE6 RID: 23270
		public DailyGiftsConfig dailygifts;

		// Token: 0x04005AE7 RID: 23271
		public AdLimitsConfig ad_limits;

		// Token: 0x04005AE8 RID: 23272
		public ExceptionsConfig exceptions;

		// Token: 0x04005AE9 RID: 23273
		public LevelOfDayConfig level_of_day;

		// Token: 0x04005AEA RID: 23274
		public DiveForTreasureSetsConfig divefortreasuresets;

		// Token: 0x04005AEB RID: 23275
		public PirateBreakoutSetsConfig piratebreakoutsets;

		// Token: 0x04005AEC RID: 23276
		public ContentUnlockConfig content_unlock;

		// Token: 0x04005AED RID: 23277
		public PromoPopupConfig promo_popup;

		// Token: 0x04005AEE RID: 23278
		public VillageRankConfig villagerankconfig;

		// Token: 0x04005AEF RID: 23279
		public IslandAreaConfigs islandareaconfig;

		// Token: 0x04005AF0 RID: 23280
		public QuestConfig questconfig;

		// Token: 0x04005AF1 RID: 23281
		public SalesConfig salesconfig;

		// Token: 0x04005AF2 RID: 23282
		public GiftConfig giftconfig;

		// Token: 0x04005AF3 RID: 23283
		public SegmentationConfig segmentationconfig;
	}
}
