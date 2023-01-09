using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Match3.Scripts1.Puzzletown.Config;
using Wooga.Coroutines;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x02000800 RID: 2048
	public class SeasonService : AService
	{
		// Token: 0x06003288 RID: 12936 RVA: 0x000EE128 File Offset: 0x000EC528
		public SeasonService()
		{
			WooroutineRunner.StartCoroutine(this.LoadRoutine(), null);
		}

		// Token: 0x06003289 RID: 12937 RVA: 0x000EE140 File Offset: 0x000EC540
		public bool PreviousGrandPrizeOfferAvailable()
		{
			this.TryEndAndStartSeason();
			return this.configService.SbsConfig.IsRemote() && this.stateService.SeasonalData.Previous != null && this.stateService.SeasonalData.Previous.IsValid && !this.stateService.SeasonalData.Previous.offerSeen;
		}

		// Token: 0x0600328A RID: 12938 RVA: 0x000EE1B4 File Offset: 0x000EC5B4
		public SeasonPrizeInfo GetPreviousSeasonPrizeInfo()
		{
			this.TryEndAndStartSeason();
			SeasonPrizeInfo previous = this.stateService.SeasonalData.Previous;
			if (previous != null && previous.IsValid)
			{
				return previous;
			}
			return null;
		}

		// Token: 0x0600328B RID: 12939 RVA: 0x000EE1EC File Offset: 0x000EC5EC
		public SeasonConfig GetActiveSeason()
		{
			return this.seasonDataService.GetActiveSeason();
		}

		// Token: 0x0600328C RID: 12940 RVA: 0x000EE1F9 File Offset: 0x000EC5F9
		public SeasonConfig GetLoadingScreenSeason(string loadingScreenSeason)
		{
			return this.seasonDataService.GetLoadingScreenSeason(loadingScreenSeason);
		}

		// Token: 0x170007F3 RID: 2035
		// (get) Token: 0x0600328D RID: 12941 RVA: 0x000EE207 File Offset: 0x000EC607
		private bool IsSeasonFeatureAvailable
		{
			get
			{
				return this.progression.UnlockedLevel >= 23;
			}
		}

		// Token: 0x170007F4 RID: 2036
		// (get) Token: 0x0600328E RID: 12942 RVA: 0x000EE21C File Offset: 0x000EC61C
		public bool IsActive
		{
			get
			{
				SeasonConfig activeSeason = this.GetActiveSeason();
				return this.IsSeasonFeatureAvailable && activeSeason != null;
			}
		}

		// Token: 0x170007F5 RID: 2037
		// (get) Token: 0x0600328F RID: 12943 RVA: 0x000EE245 File Offset: 0x000EC645
		public bool IsSeasonalsV3
		{
			get
			{
				return this.sbsService.SbsConfig.feature_switches.seasonals_v3;
			}
		}

		// Token: 0x06003290 RID: 12944 RVA: 0x000EE25C File Offset: 0x000EC65C
		private IEnumerator LoadRoutine()
		{
			yield return ServiceLocator.Instance.Inject(this);
			this.seasonDataService = new SeasonServiceEventsDataSource(this.configService);
			this.TryEndAndStartSeason();
			this.stateService.Resources.onChanged.AddListener(new Action<MaterialChange>(this.OnSeasonMaterialChanged));
			base.OnInitialized.Dispatch();
			yield break;
		}

		// Token: 0x06003291 RID: 12945 RVA: 0x000EE278 File Offset: 0x000EC678
		public void TryEndAndStartSeason()
		{
			if (!this.IsSeasonFeatureAvailable || !this.configService.SbsConfig.IsRemote())
			{
				return;
			}
			SeasonConfig activeSeason = this.GetActiveSeason();
			SeasonalDataService seasonalData = this.stateService.SeasonalData;
			if (activeSeason != null && seasonalData.Current != null && seasonalData.Current.IsValid && activeSeason.Primary == seasonalData.Current.name)
			{
				return;
			}
			if (seasonalData.Current != null && (seasonalData.Previous == null || seasonalData.Previous.name != seasonalData.Current.name))
			{
				seasonalData.Previous = seasonalData.Current;
			}
			if (activeSeason != null)
			{
				SeasonPricingInfo pricing = activeSeason.pricing;
				if (this.IsSeasonalsV3 && activeSeason.pricing_v3 != null && activeSeason.pricing_v3.IsValid)
				{
					pricing = activeSeason.pricing_v3;
				}
				seasonalData.Current = new SeasonPrizeInfo(activeSeason.Primary, 0, pricing, this.sbsService.SbsConfig.segmentationconfig.seasonSegment);
				this.stateService.Resources.RemoveMaterial("earned_season_currency");
			}
		}

		// Token: 0x06003292 RID: 12946 RVA: 0x000EE3B0 File Offset: 0x000EC7B0
		public BuildingConfig GetGrandPrizeBuildingConfig()
		{
			SeasonConfig activeSeason = this.GetActiveSeason();
			if (activeSeason == null)
			{
				return null;
			}
			return this.GrandPrizeConfigForSeason(activeSeason.Primary);
		}

		// Token: 0x170007F6 RID: 2038
		// (get) Token: 0x06003293 RID: 12947 RVA: 0x000EE3D8 File Offset: 0x000EC7D8
		public int GrandPrizeProgress
		{
			get
			{
				return (this.stateService.SeasonalData.Current == null) ? 0 : this.stateService.SeasonalData.Current.grandPrizeProgress;
			}
		}

		// Token: 0x170007F7 RID: 2039
		// (get) Token: 0x06003294 RID: 12948 RVA: 0x000EE40C File Offset: 0x000EC80C
		public bool GrandPrizeReady
		{
			get
			{
				BuildingConfig grandPrizeBuildingConfig = this.GetGrandPrizeBuildingConfig();
				if (grandPrizeBuildingConfig == null)
				{
					return false;
				}
				int num = SeasonService.TargetValueForGrandPrizeBuilding(grandPrizeBuildingConfig);
				return num <= this.GrandPrizeProgress;
			}
		}

		// Token: 0x06003295 RID: 12949 RVA: 0x000EE43C File Offset: 0x000EC83C
		public void MarkPreviousGrandPrizeOfferSeen()
		{
			if (this.stateService.SeasonalData.Previous != null && this.stateService.SeasonalData.Previous.IsValid)
			{
				this.stateService.SeasonalData.Previous.offerSeen = true;
			}
			this.stateService.Save(false);
		}

		// Token: 0x06003296 RID: 12950 RVA: 0x000EE49A File Offset: 0x000EC89A
		public void DebugReset()
		{
			this.stateService.SeasonalData.Previous = this.stateService.SeasonalData.Current;
			this.stateService.SeasonalData.Current = null;
			this.TryEndAndStartSeason();
		}

		// Token: 0x06003297 RID: 12951 RVA: 0x000EE4D4 File Offset: 0x000EC8D4
		public MaterialAmount GetConversionPrice(MaterialAmount originalPrice)
		{
			SeasonConfig activeSeason = this.GetActiveSeason();
			float num = activeSeason.seasonal_diamond_conversion;
			num = ((num <= 0f) ? 1f : num);
			int amount = Mathf.CeilToInt((float)originalPrice.amount / num);
			return new MaterialAmount("diamonds", amount, MaterialAmountUsage.Undefined, 0);
		}

		// Token: 0x06003298 RID: 12952 RVA: 0x000EE524 File Offset: 0x000EC924
		public SeasonService.PriceInfo CalculatePriceForGrandPrize(SeasonPrizeInfo seasonInfo, int currentProgress)
		{
			int num = this.TargetValueForGrandPrize(seasonInfo.name);
			float[] priceFactors = seasonInfo.Pricing.priceFactors;
			int grandPrizePrice = seasonInfo.Pricing.grandPrizePrice;
			int minPrice = seasonInfo.Pricing.minPrice;
			int priceRounding = seasonInfo.Pricing.priceRounding;
			float num2 = Mathf.Clamp01((float)currentProgress / (float)num);
			int num3 = (int)Mathf.Clamp((float)priceFactors.Length * num2, 0f, (float)(priceFactors.Length - 1));
			float num4 = priceFactors[num3];
			int num5 = (int)((float)grandPrizePrice * num4);
			int a = num5 - num5 % priceRounding;
			int num6 = Mathf.Max(a, minPrice);
			float value = (float)num6 / (float)grandPrizePrice;
			float num7 = Mathf.Clamp01(value);
			int num8 = (int)(num7 * 100f);
			int num9 = 100 - num8;
			return new SeasonService.PriceInfo(grandPrizePrice, num6, (float)num9, "diamonds");
		}

		// Token: 0x06003299 RID: 12953 RVA: 0x000EE5EC File Offset: 0x000EC9EC
		public int TargetValueForGrandPrize(string seasonName)
		{
			BuildingConfig buildingConfig = this.GrandPrizeConfigForSeason(seasonName);
			if (buildingConfig == null)
			{
				return 0;
			}
			return SeasonService.TargetValueForGrandPrizeBuilding(buildingConfig);
		}

		// Token: 0x0600329A RID: 12954 RVA: 0x000EE610 File Offset: 0x000ECA10
		public BuildingConfig GrandPrizeConfigForSeason(string seasonName)
		{
			return this.configService.buildingConfigList.buildings.FirstOrDefault((BuildingConfig b) => b.IsSeasonGrandPrice() && b.season == seasonName);
		}

		// Token: 0x0600329B RID: 12955 RVA: 0x000EE64B File Offset: 0x000ECA4B
		private static int TargetValueForGrandPrizeBuilding(BuildingConfig grandPrizeBuilding)
		{
			return grandPrizeBuilding.costs[0].amount;
		}

		// Token: 0x0600329C RID: 12956 RVA: 0x000EE660 File Offset: 0x000ECA60
		public MaterialAmount GetEarnedMaterialForTier(int tier)
		{
			int amount = 0;
			if (this.IsSeasonalsV3 && this.IsActive)
			{
				SeasonConfig activeSeason = this.GetActiveSeason();
				SeasonPrizeInfo seasonPrizeInfo = this.stateService.SeasonalData.Current;
				List<int> tier_rewards = activeSeason.GetSegment(seasonPrizeInfo.segment).tier_rewards;
				amount = tier_rewards[tier];
			}
			return new MaterialAmount("earned_season_currency", amount, MaterialAmountUsage.Undefined, 0);
		}

		// Token: 0x0600329D RID: 12957 RVA: 0x000EE6C4 File Offset: 0x000ECAC4
		private void OnSeasonMaterialChanged(MaterialChange change)
		{
			if (change.name == "earned_season_currency" && change.Delta < 0)
			{
				int amount = Math.Abs(change.Delta);
				this.stateService.Resources.AddMaterial(new MaterialAmount("season_currency", amount, MaterialAmountUsage.Undefined, 0), true);
				this.stateService.Save(false);
			}
			else if (change.name == "season_currency" && this.IsActive)
			{
				this.TryEndAndStartSeason();
				this.stateService.SeasonalData.AddGrandPrizeProgress(change.Delta);
				this.stateService.Save(false);
			}
		}

		// Token: 0x0600329E RID: 12958 RVA: 0x000EE77C File Offset: 0x000ECB7C
		public Wooroutine<bool> AreAllActiveSeasonBundlesAvailable()
		{
			SeasonConfig activeSeason = this.GetActiveSeason();
			if (activeSeason == null)
			{
				return Wooroutine<bool>.CreateCompleted(false);
			}
			return WooroutineRunner.StartWooroutine<bool>(this.CheckAllSeasonBundleSetsAvailableRoutine(activeSeason.BundleNames));
		}

		// Token: 0x0600329F RID: 12959 RVA: 0x000EE7AE File Offset: 0x000ECBAE
		public Wooroutine<bool> AreAllSeasonBundlesAvailable(IEnumerable<string> bundleNames)
		{
			return WooroutineRunner.StartWooroutine<bool>(this.CheckAllSeasonBundleSetsAvailableRoutine(bundleNames));
		}

		// Token: 0x060032A0 RID: 12960 RVA: 0x000EE7BC File Offset: 0x000ECBBC
		private IEnumerator CheckAllSeasonBundleSetsAvailableRoutine(IEnumerable<string> bundleNames)
		{
			Wooroutine<bool> checkBundlesAvailableRoutine = this.assetBundleService.AreAllBundlesAvailable(bundleNames);
			yield return checkBundlesAvailableRoutine;
			bool allBundlesAvailable = true;
			try
			{
				allBundlesAvailable = checkBundlesAvailableRoutine.ReturnValue;
			}
			catch (Exception ex)
			{
				WoogaDebug.Log(new object[]
				{
					ex.Message
				});
			}
			yield return allBundlesAvailable;
			yield break;
		}

		// Token: 0x04005B06 RID: 23302
		[WaitForService(true, true)]
		private GameStateService stateService;

		// Token: 0x04005B07 RID: 23303
		[WaitForService(true, true)]
		private ConfigService configService;

		// Token: 0x04005B08 RID: 23304
		[WaitForService(true, true)]
		private AssetBundleService assetBundleService;

		// Token: 0x04005B09 RID: 23305
		[WaitForService(true, true)]
		private ProgressionDataService.Service progression;

		// Token: 0x04005B0A RID: 23306
		[WaitForService(true, true)]
		private SBSService sbsService;

		// Token: 0x04005B0B RID: 23307
		private SeasonServiceEventsDataSource seasonDataService;

		// Token: 0x04005B0C RID: 23308
		public const string SEASONALS_V3_SHOP_TUTORIAL = "seasonalsV3ShopTutorial";

		// Token: 0x04005B0D RID: 23309
		private const int UNLOCK_SEASONAL_CONTENT = 23;

		// Token: 0x02000801 RID: 2049
		public struct PriceInfo
		{
			// Token: 0x060032A1 RID: 12961 RVA: 0x000EE7DE File Offset: 0x000ECBDE
			public PriceInfo(int fullPrice, int discountPrice, float discountPercent, string material)
			{
				this.fullPrice = fullPrice;
				this.discountPrice = discountPrice;
				this.discountPercent = discountPercent;
				this.material = material;
			}

			// Token: 0x170007F8 RID: 2040
			// (get) Token: 0x060032A2 RID: 12962 RVA: 0x000EE7FD File Offset: 0x000ECBFD
			public MaterialAmount MaterialCost
			{
				get
				{
					return new MaterialAmount(this.material, this.discountPrice, MaterialAmountUsage.Undefined, 0);
				}
			}

			// Token: 0x04005B0E RID: 23310
			public int fullPrice;

			// Token: 0x04005B0F RID: 23311
			public int discountPrice;

			// Token: 0x04005B10 RID: 23312
			public float discountPercent;

			// Token: 0x04005B11 RID: 23313
			public string material;
		}
	}
}
