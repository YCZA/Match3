using System;
using System.Collections;
using Wooga.Coroutines;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x02000765 RID: 1893
	public class DailyDealsService : AService
	{
		// Token: 0x06002EFC RID: 12028 RVA: 0x000DB6F7 File Offset: 0x000D9AF7
		public DailyDealsService()
		{
			WooroutineRunner.StartCoroutine(this.InitRoutine(), null);
		}

		// Token: 0x17000755 RID: 1877
		// (get) Token: 0x06002EFD RID: 12029 RVA: 0x000DB70C File Offset: 0x000D9B0C
		public Sprite BuildingSprite
		{
			get
			{
				return this.buildingSprite;
			}
		}

		// Token: 0x06002EFE RID: 12030 RVA: 0x000DB714 File Offset: 0x000D9B14
		private IEnumerator InitRoutine()
		{
			yield return ServiceLocator.Instance.Inject(this);
			this.RefreshDailyDeal();
			base.OnInitialized.Dispatch();
			yield break;
		}

		// Token: 0x06002EFF RID: 12031 RVA: 0x000DB730 File Offset: 0x000D9B30
		public void RefreshDailyDeal()
		{
			if (this.timeService.LocalNow > this.gameStateService.DailyDeals.CurrentDealExpirationDate && !this.IsFeatureAvailable())
			{
				this.ResetDailyDeals();
			}
			else if (this.timeService.LocalNow > this.gameStateService.DailyDeals.CurrentDealExpirationDate && this.IsFeatureAvailable() && this.timeService.IsTimeValid && this.HasBoughtATriggerPackage() && !this.HasBoughtAnExcludePackage())
			{
				this.CreateNewDailyDeal();
			}
			if (this.buildingSprite == null)
			{
				this.UpdateShopSprite();
			}
		}

		// Token: 0x06002F00 RID: 12032 RVA: 0x000DB7EC File Offset: 0x000D9BEC
		public bool ShouldShowDailyDealPopout()
		{
			// eli key point 限时礼包
			// eli todo 限时礼包
			return false;
			// return this.IsEnabled() && this.gameStateService.DailyDeals.CurrentDeal != null && this.gameStateService.DailyDeals.CurrentDealExpirationDate > DateTime.Now && !this.gameStateService.DailyDeals.CurrentDealPurchased;
		}

		// Token: 0x06002F01 RID: 12033 RVA: 0x000DB84E File Offset: 0x000D9C4E
		public bool IsFeatureAvailable()
		{
			return this.IsEnabled() && this.IsUnlocked();
		}

		// Token: 0x06002F02 RID: 12034 RVA: 0x000DB864 File Offset: 0x000D9C64
		public void CheatMenuNextDailyDeal()
		{
			this.CreateNewDailyDeal();
			this.UpdateShopSprite();
		}

		// Token: 0x06002F03 RID: 12035 RVA: 0x000DB874 File Offset: 0x000D9C74
		public void ResetDailyDeals()
		{
			this.gameStateService.DailyDeals.CurrentDeal = null;
			this.gameStateService.DailyDeals.CurrentDealPurchased = false;
			this.gameStateService.DailyDeals.CurrentDealDayNumber = 0;
			this.gameStateService.DailyDeals.CurrentDealExpirationDate = DateTime.MinValue;
			this.buildingSprite = null;
		}

		// Token: 0x06002F04 RID: 12036 RVA: 0x000DB8D0 File Offset: 0x000D9CD0
		private void UpdateShopSprite()
		{
			string text = string.Empty;
			if (this.gameStateService.DailyDeals.CurrentDeal == null)
			{
				return;
			}
			if (!string.IsNullOrEmpty(this.gameStateService.DailyDeals.CurrentDeal.bonus_1_type) && this.gameStateService.DailyDeals.CurrentDeal.bonus_1_type.StartsWith("iso_"))
			{
				text = this.gameStateService.DailyDeals.CurrentDeal.bonus_1_type;
			}
			else if (!string.IsNullOrEmpty(this.gameStateService.DailyDeals.CurrentDeal.bonus_2_type) && this.gameStateService.DailyDeals.CurrentDeal.bonus_2_type.StartsWith("iso_"))
			{
				text = this.gameStateService.DailyDeals.CurrentDeal.bonus_2_type;
			}
			else if (!string.IsNullOrEmpty(this.gameStateService.DailyDeals.CurrentDeal.bonus_3_type) && this.gameStateService.DailyDeals.CurrentDeal.bonus_3_type.StartsWith("iso_"))
			{
				text = this.gameStateService.DailyDeals.CurrentDeal.bonus_3_type;
			}
			if (!string.IsNullOrEmpty(text))
			{
				WooroutineRunner.StartCoroutine(this.LoadBuildingSpriteRoutine(text), null);
			}
		}

		// Token: 0x06002F05 RID: 12037 RVA: 0x000DBA28 File Offset: 0x000D9E28
		private IEnumerator LoadBuildingSpriteRoutine(string buildingName)
		{
			if (SceneManager.IsPlayingMatch3)
			{
				yield break;
			}
			BuildingConfig buildingConfig = this.configService.buildingConfigList.GetConfig(buildingName);
			Wooroutine<BuildingResourceServiceRoot> brsLoader = SceneManager.Instance.Await<BuildingResourceServiceRoot>(true);
			yield return brsLoader;
			BuildingResourceServiceRoot resourceService = brsLoader.ReturnValue;
			yield return resourceService.onSpritesLoaded;
			Debug.LogError("test:" + buildingConfig);
			Debug.LogError("test:" + buildingName);
			this.buildingSprite = resourceService.GetWrappedSpriteOrPlaceholder(buildingConfig).asset;
			yield break;
		}

		// Token: 0x06002F06 RID: 12038 RVA: 0x000DBA4C File Offset: 0x000D9E4C
		private bool HasBoughtATriggerPackage()
		{
			bool result = false;
			foreach (string itemId in this.sbsService.SbsConfig.dailydealsconfig.balancing.TriggerPackages)
			{
				if (this.gameStateService.Transactions.TransactionDataContainsId(itemId))
				{
					result = true;
					break;
				}
			}
			return result;
		}

		// Token: 0x06002F07 RID: 12039 RVA: 0x000DBAD8 File Offset: 0x000D9ED8
		private bool HasBoughtAnExcludePackage()
		{
			bool result = false;
			foreach (string itemId in this.sbsService.SbsConfig.dailydealsconfig.balancing.ExcludePackages)
			{
				if (this.gameStateService.Transactions.TransactionDataContainsId(itemId))
				{
					result = true;
					break;
				}
			}
			return result;
		}

		// Token: 0x06002F08 RID: 12040 RVA: 0x000DBB64 File Offset: 0x000D9F64
		private bool IsEnabled()
		{
			return this.sbsService != null && this.sbsService.SbsConfig.feature_switches.daily_deals;
		}

		// Token: 0x06002F09 RID: 12041 RVA: 0x000DBB8C File Offset: 0x000D9F8C
		private bool IsUnlocked()
		{
			return this.sbsService != null && this.progressionService != null && this.progressionService.UnlockedLevel >= this.sbsService.SbsConfig.dailydealsconfig.balancing.unlock_level;
		}

		// Token: 0x06002F0A RID: 12042 RVA: 0x000DBBDC File Offset: 0x000D9FDC
		private void CreateNewDailyDeal()
		{
			if (this.gameStateService.DailyDeals.CurrentDealDayNumber >= this.sbsService.SbsConfig.dailydealsconfig.deals.Count)
			{
				this.gameStateService.DailyDeals.CurrentDealDayNumber = 0;
			}
			int currentDealDayNumber = this.gameStateService.DailyDeals.CurrentDealDayNumber;
			int offer_id = this.sbsService.SbsConfig.dailydealsconfig.order[currentDealDayNumber].offer_id;
			this.gameStateService.DailyDeals.CurrentDeal = this.sbsService.SbsConfig.dailydealsconfig.deals[offer_id - 1];
			this.gameStateService.DailyDeals.CurrentDealPurchased = false;
			this.gameStateService.DailyDeals.CurrentDealDayNumber++;
			this.gameStateService.DailyDeals.CurrentDealExpirationDate = DateTime.Now.NextMidnight().AddHours((double)this.sbsService.SbsConfig.dailydealsconfig.balancing.unlock_time);
			this.buildingSprite = null;
		}

		// Token: 0x04005824 RID: 22564
		public const string DAILY_DEAL_TAG = "daily_deal_open_items";

		// Token: 0x04005825 RID: 22565
		public const string DAILY_DEAL_IAP_TYPE = "daily_deal";

		// Token: 0x04005826 RID: 22566
		[WaitForService(true, true)]
		private GameStateService gameStateService;

		// Token: 0x04005827 RID: 22567
		[WaitForService(true, true)]
		private ProgressionDataService.Service progressionService;

		// Token: 0x04005828 RID: 22568
		[WaitForService(true, true)]
		private SBSService sbsService;

		// Token: 0x04005829 RID: 22569
		[WaitForService(true, true)]
		private ConfigService configService;

		// Token: 0x0400582A RID: 22570
		[WaitForService(true, true)]
		private TimeService timeService;

		// Token: 0x0400582B RID: 22571
		private Sprite buildingSprite;
	}
}
