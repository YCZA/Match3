using System;
using System.Collections;
using System.Collections.Generic;
using AndroidTools.Tools;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.Services.ErrorAnalytics;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;
using UnityEngine; // using Heyzap;

// eli mark ads

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x02000825 RID: 2085
	public class FairbidVideoAdService : AService, IVideoAdService, IService, IInitializable
	{
		// Token: 0x060033C5 RID: 13253 RVA: 0x000F796C File Offset: 0x000F5D6C
		public FairbidVideoAdService()
		{
			WooroutineRunner.StartCoroutine(this.LoadRoutine(), null);
		}

		// Token: 0x17000821 RID: 2081
		// (get) Token: 0x060033C6 RID: 13254 RVA: 0x000F798C File Offset: 0x000F5D8C
		public bool HasUnlockedVideoAd
		{
			get
			{
				// 审核版不开放视频广告
				// #if REVIEW_VERSION
					// return false;
				// #endif
				return this.progression.UnlockedLevel >= this.configService.general.wheel_settings.minimum_level;
			}
		}

		// Token: 0x17000822 RID: 2082
		// (get) Token: 0x060033C7 RID: 13255 RVA: 0x000F79B3 File Offset: 0x000F5DB3
		// (set) Token: 0x060033C8 RID: 13256 RVA: 0x000F79BB File Offset: 0x000F5DBB
		public DateTime LastAdTime { get; private set; }

		// Token: 0x17000823 RID: 2083
		// (get) Token: 0x060033C9 RID: 13257 RVA: 0x000F79C4 File Offset: 0x000F5DC4
		// (set) Token: 0x060033CA RID: 13258 RVA: 0x000F79CC File Offset: 0x000F5DCC
		public bool FreeSpinAvailable { get; set; }

		// Token: 0x17000824 RID: 2084
		// (get) Token: 0x060033CB RID: 13259 RVA: 0x000F79D5 File Offset: 0x000F5DD5
		// (set) Token: 0x060033CC RID: 13260 RVA: 0x000F79DD File Offset: 0x000F5DDD
		public string FreeSpinVideoAdId { get; set; }

		// Token: 0x060033CD RID: 13261 RVA: 0x000F79E8 File Offset: 0x000F5DE8
		private IEnumerator LoadRoutine()
		{
			yield return ServiceLocator.Instance.Inject(this);
			if (this.gameStateService.lastVideoWatchedDate != DateTime.Today)
			{
				this.gameStateService.AdViews.ClearDailyAdCounters();
			}
			if (!Application.isEditor)
			{
				//if (!this.gameSettingsService.HasToggle(ToggleSetting.AdPersonalisation))
				//{
				//	this.gameSettingsService.SetToggle(ToggleSetting.AdPersonalisation, true);
				//}
				// HeyzapAds.SetGdprConsent(this.gameSettingsService.GetToggle(ToggleSetting.AdPersonalisation));
				// if (!FairbidVideoAdService.debugSetup && SbsEnvironment.CurrentEnvironment != SbsEnvironment.Environment.PRODUCTION)
				// {
				// 	FairbidVideoAdService.debugSetup = true;
				// 	HeyzapAds.ShowDebugLogs();
				// 	HeyzapAds.ShowThirdPartyDebugLogs();
				// 	HeyzapAds.SetBundleIdentifier("net.wooga.tropicats_tropical_cats_puzzle_paradise");
				// }
				// HeyzapAds.SetNetworkCallbackListener(new HeyzapAds.NetworkCallbackListener(this.NetworkDelegate));
				// HeyzapAds.Start("ff3785ca7d603931149871b9bc17a645", 0);
				// HZIncentivizedAd.SetDisplayListener(new HZIncentivizedAd.AdDisplayListener(this.VideoAdDelegate));
				// HZIncentivizedAd.SetAdDidShowListener(new HZIncentivizedAd.AdDidShowCallbackListener(this.EcpmDelegate));
				// this.progression.onLevelUnlocked.AddListenerOnce(new Action<int>(this.HandleLevelUnlocked));
			}
			base.OnInitialized.Dispatch();
			if (!Application.isEditor)
			{
				//WaitForSeconds waitTime = new WaitForSeconds(3f);
				//// while (!HeyzapAds.HasStarted())
				//// {
				//// 	yield return waitTime;
				//// }
				//this.Track("sdk_initialised", 0, null);
				//this.Fetch();
			}
			yield break;
		}

		// Token: 0x060033CE RID: 13262 RVA: 0x000F7A03 File Offset: 0x000F5E03
		public override void DeInit()
		{
			// HZIncentivizedAd.SetDisplayListener(null);
			// HZIncentivizedAd.SetAdDidShowListener(null);
			// HeyzapAds.SetNetworkCallbackListener(null);
		}

		// Token: 0x060033CF RID: 13263 RVA: 0x000F7A18 File Offset: 0x000F5E18
		public bool IsAllowedToWatchAd(AdPlacement placement)
		{
			int dailyAdLimit = this.GetDailyAdLimit(AdPlacement.Unspecified);
			int dailyAdLimit2 = this.GetDailyAdLimit(placement);
			return this.gameStateService.AdViews.CanWatchAd(dailyAdLimit, placement, dailyAdLimit2);
		}

		// Token: 0x060033D0 RID: 13264 RVA: 0x000F7A48 File Offset: 0x000F5E48
		public bool IsVideoAvailable(bool tryRefresh = true)
		{
			// eli key point 视频广告是否可用
			// Debug.LogWarning("!!!!!!!!!!!!!!!!!!!!: IsVideoAvailable");
			// return false;
			// return true;
			return AndroidAds.IsLoadedVideoAd();
			// return Application.isEditor || HZIncentivizedAd.IsAvailable();
		}

		// Token: 0x060033D1 RID: 13265 RVA: 0x000F7A5C File Offset: 0x000F5E5C
		public IEnumerator ShowAd(AdPlacement placement)
		{
			//eli key point 视频广告
			Resources.UnloadUnusedAssets();
			this.currentlyRequestedAdPlacement = placement;
			this.videoShowResult = VideoShowResult.None;
			this.adMetaData = new VideoAdMetaData();
			if (placement == AdPlacement.AdWheel)
			{
				this.FreeSpinVideoAdId = this.adMetaData.id;
				this.trackingService.TrackAdWheel("watch", this.FreeSpinTrackingData());
			}
			this.Track("start", 0, this.adMetaData);
			Application.targetFrameRate = 10;
			if (!Application.isEditor)
			{
				// HZIncentivizedAd.Show();
				AndroidAds.ShowRewardAd(()=>
				{
					videoShowResult = VideoShowResult.Success;
				}, () =>
				{
					videoShowResult = VideoShowResult.Aborted;
				});
			}
			else
			{
				this.videoShowResult = VideoShowResult.Success;
			}
			while (this.videoShowResult == VideoShowResult.None)
			{
				yield return null;
			}
			Application.targetFrameRate = FPSService.TargetFrameRate;
			this.UpdateViewCounts(this.videoShowResult, placement);
			yield return this.videoShowResult;
			yield break;
		}

		// Token: 0x060033D2 RID: 13266 RVA: 0x000F7A7E File Offset: 0x000F5E7E
		public void ChangeSetting(ToggleSetting setting, bool value)
		{
			// HeyzapAds.SetGdprConsent(value);
		}

		// Token: 0x060033D3 RID: 13267 RVA: 0x000F7A86 File Offset: 0x000F5E86
		public void DebugShowTestSuite()
		{
			// HeyzapAds.ShowMediationTestSuite();
		}

		// Token: 0x060033D4 RID: 13268 RVA: 0x000F7A8D File Offset: 0x000F5E8D
		public void EditorPretendYouJustWatchedAnAd(AdPlacement placement)
		{
			if (!Application.isEditor)
			{
				return;
			}
			this.gameStateService.AdViews.IncreaseViewCount(placement);
			this.gameStateService.lastVideoWatchedDate = DateTime.Today;
		}

		// Token: 0x060033D5 RID: 13269 RVA: 0x000F7ABB File Offset: 0x000F5EBB
		private void Fetch()
		{
			if (this.HasUnlockedVideoAd)
			{
				// HZIncentivizedAd.Fetch();
				this.Track("request", 0, null);
			}
		}

		// Token: 0x060033D6 RID: 13270 RVA: 0x000F7ADA File Offset: 0x000F5EDA
		private void HandleLevelUnlocked(int level)
		{
			if (level == this.configService.general.wheel_settings.minimum_level)
			{
				this.Fetch();
			}
		}

		// Token: 0x060033D7 RID: 13271 RVA: 0x000F7B00 File Offset: 0x000F5F00
		private void VideoAdDelegate(string adState, string adTag)
		{
			WoogaDebug.Log(new object[]
			{
				"VideoAd: [Fairbid]: " + adState + " Tag : " + adTag
			});
			if (adState.Equals("available"))
			{
				this.Track("fill", 0, null);
			}
			else if (adState.Equals("fetch_failed"))
			{
				this.Track("no_fill", 0, null);
			}
			else if (adState.Equals("show"))
			{
				this.LastAdTime = DateTime.UtcNow;
				this.lastStartTime = Time.realtimeSinceStartup;
				this.Track("show", 0, this.adMetaData);
				EAHelper.AddBreadcrumb(string.Format("VideoAd: [Fairbid]: SHOW [{0}]", this.adMetaData.network));
			}
			else if (adState.Equals("hide"))
			{
				this.Track("hide", this.AdDuration(), this.adMetaData);
			}
			else if (adState.Equals("click"))
			{
				this.Track("click", this.AdDuration(), this.adMetaData);
			}
			else if (adState.Equals("failed"))
			{
				this.videoShowResult = VideoShowResult.Success;
				this.Track("fail", 0, this.adMetaData);
				Log.Warning(string.Format("VideoAd: [Fairbid] [{0}]: ERROR PLAYING VIDEO", this.adMetaData.network), null, null);
			}
			else if (adState.Equals("incentivized_result_complete"))
			{
				this.videoShowResult = VideoShowResult.Success;
				this.Track("end", this.AdDuration(), this.adMetaData);
				EAHelper.AddBreadcrumb("VideoAd: [Fairbid]: AD FINISHED WITH RESULT END");
			}
			else if (adState.Equals("incentivized_result_incomplete"))
			{
				this.videoShowResult = VideoShowResult.Aborted;
				this.Track("cancel", this.AdDuration(), this.adMetaData);
				EAHelper.AddBreadcrumb("VideoAd: [Fairbid]: AD FINISHED WITH RESULT CANCEL");
			}
		}

		// Token: 0x060033D8 RID: 13272 RVA: 0x000F7CE0 File Offset: 0x000F60E0
		private void NetworkDelegate(string network, string adState)
		{
			if (!adState.Equals("fetch_failed"))
			{
				if (adState.Equals("show"))
				{
					this.adMetaData.network = network;
				}
				else if (adState.Equals("failed"))
				{
					this.adMetaData.network = network;
				}
			}
		}

		// Token: 0x060033D9 RID: 13273 RVA: 0x000F7D40 File Offset: 0x000F6140
		// private void EcpmDelegate(string tag, HZAdImpressionData adImpressionData)
		// {
		// 	this.adMetaData.ecpmPricing = adImpressionData.eCPM.ToString();
		// 	this.adMetaData.ecpmPricingType = adImpressionData.pricingType;
		// }

		// Token: 0x060033DA RID: 13274 RVA: 0x000F7D7D File Offset: 0x000F617D
		private int AdDuration()
		{
			return Mathf.RoundToInt(Time.realtimeSinceStartup - this.lastStartTime);
		}

		// Token: 0x060033DB RID: 13275 RVA: 0x000F7D90 File Offset: 0x000F6190
		private int GetDailyAdLimit(AdPlacement placement)
		{
			return this.sbsService.SbsConfig.ad_limits.GetDailyLimitFor(placement);
		}

		// Token: 0x060033DC RID: 13276 RVA: 0x000F7DA8 File Offset: 0x000F61A8
		private bool UserReachedDailyLimit()
		{
			return !this.IsAllowedToWatchAd(AdPlacement.Unspecified);
		}

		// Token: 0x060033DD RID: 13277 RVA: 0x000F7DB4 File Offset: 0x000F61B4
		private void UpdateViewCounts(VideoShowResult adResult, AdPlacement placement)
		{
			if (adResult == VideoShowResult.Success)
			{
				this.gameStateService.AdViews.IncreaseViewCount(placement);
				this.gameStateService.lastVideoWatchedDate = DateTime.Today;
				if (this.UserReachedDailyLimit())
				{
					this.Track("viewcapreached", 0, this.adMetaData);
				}
			}
		}

		// Token: 0x060033DE RID: 13278 RVA: 0x000F7E06 File Offset: 0x000F6206
		public void TrackClaim()
		{
			this.Track("claim", 0, this.adMetaData);
		}

		// Token: 0x060033DF RID: 13279 RVA: 0x000F7E1C File Offset: 0x000F621C
		private Dictionary<string, object> FreeSpinTrackingData()
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["video_adid"] = this.FreeSpinVideoAdId;
			return dictionary;
		}

		// Token: 0x060033E0 RID: 13280 RVA: 0x000F7E41 File Offset: 0x000F6241
		private void Track(string user_action, int adDuration, VideoAdMetaData metaData = null)
		{
			this.trackingService.TrackWoogaVideoAds(adDuration, VideoAdHelper.GetAdPlacementAsString(this.currentlyRequestedAdPlacement), user_action, "fairbid", metaData);
		}

		// Token: 0x04005BC8 RID: 23496
		private const string PUBLISHER_ID = "ff3785ca7d603931149871b9bc17a645";

		// Token: 0x04005BC9 RID: 23497
		// private const string BUNDLE_ID_OVERRIDE = "net.wooga.tropicats_tropical_cats_puzzle_paradise";

		// Token: 0x04005BCA RID: 23498
		private const bool AD_CONSENT_DEFAULT = true;

		// Token: 0x04005BCE RID: 23502
		[WaitForService(true, true)]
		private GameStateService gameStateService;

		// Token: 0x04005BCF RID: 23503
		[WaitForService(true, true)]
		private ConfigService configService;

		// Token: 0x04005BD0 RID: 23504
		[WaitForService(true, true)]
		private TrackingService trackingService;

		// Token: 0x04005BD1 RID: 23505
		[WaitForService(true, true)]
		private ProgressionDataService.Service progression;

		// Token: 0x04005BD2 RID: 23506
		[WaitForService(true, true)]
		private GameSettingsService gameSettingsService;

		// Token: 0x04005BD3 RID: 23507
		[WaitForService(true, true)]
		private SBSService sbsService;

		// Token: 0x04005BD4 RID: 23508
		private AdPlacement currentlyRequestedAdPlacement;

		// Token: 0x04005BD5 RID: 23509
		private VideoShowResult videoShowResult;

		// Token: 0x04005BD6 RID: 23510
		private float lastStartTime;

		// Token: 0x04005BD7 RID: 23511
		private VideoAdMetaData adMetaData = new VideoAdMetaData();

		// Token: 0x04005BD8 RID: 23512
		private static bool debugSetup;
		public void Init()
		{
			throw new NotImplementedException();
		}
	}
}
