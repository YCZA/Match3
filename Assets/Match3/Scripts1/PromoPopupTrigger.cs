using System.Collections;
using Match3.Scripts1.Puzzletown.Services;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.Signals;
using Wooga.UnityFramework;
using Match3.Scripts2.Building.Shop;
using Match3.Scripts2.PlayerData;
using UnityEngine;

// Token: 0x02000A2C RID: 2604
namespace Match3.Scripts1
{
	public class PromoPopupTrigger : PopupManager.Trigger
	{
		// Token: 0x06003E99 RID: 16025 RVA: 0x0013E2DB File Offset: 0x0013C6DB
		public PromoPopupTrigger(GameStateService gameStateService, AssetBundleService assetBundleService, PTConfig sbsConfig, SessionService sessionService, TownShopRoot shop)
		{
			this.gameStateService = gameStateService;
			this.assetBundleService = assetBundleService;
			this.sbsConfig = sbsConfig;
			this.sessionService = sessionService;
			this.shop = shop;
		}

		// Token: 0x06003E9A RID: 16026 RVA: 0x0013E308 File Offset: 0x0013C708
		public override bool ShouldTrigger()
		{
			return this.sbsConfig.feature_switches.alphabet_decos_enabled && this.sbsConfig.promo_popup.Active() && this.sessionService.SessionsToday >= 2 && this.gameStateService.PromoPopupData.SeenToday() && this.IsShopAvailable();
		}

		// Token: 0x06003E9B RID: 16027 RVA: 0x0013E370 File Offset: 0x0013C770
		public override IEnumerator Run()
		{
			Wooroutine<bool> isAvailableRoutine = this.assetBundleService.IsBundleAvailable(this.sbsConfig.promo_popup.AlphabetAssetBundleName);
			yield return isAvailableRoutine;
			if (!isAvailableRoutine.ReturnValue)
			{
				yield break;
			}
			Wooroutine<PopupSeasonalPromoAdvancedRoot> scene = SceneManager.Instance.LoadSceneWithParams<PopupSeasonalPromoAdvancedRoot, PopupSeasonalPromoAdvancedRoot.Params>(new PopupSeasonalPromoAdvancedRoot.Params(this.sbsConfig.promo_popup.promo_name, this.sbsConfig.promo_popup.AlphabetAssetBundleName, this.sbsConfig.promo_popup.title_key, this.sbsConfig.promo_popup.speechbubble_key, ShopTag.RarityLevel1), null);
			yield return scene;
			AwaitSignal<bool> showingShopAfterPopup = scene.ReturnValue.onClose.Await<bool>();
			yield return showingShopAfterPopup;
			if (showingShopAfterPopup.Dispatched)
			{
				yield return this.shop.onShopReady;
				WaitForSeconds updateInterval = new WaitForSeconds(1f);
				while (this.IsTownShopVisible() || this.HasBuildingSelected() || this.HasBlocker())
				{
					yield return updateInterval;
				}
			}
			yield break;
		}

		// Token: 0x06003E9C RID: 16028 RVA: 0x0013E38B File Offset: 0x0013C78B
		private bool IsShopAvailable()
		{
			return this.gameStateService.Progression.UnlockedLevel >= PopupSeasonalPromoRoot.Trigger.SHOP_UNLOCK_LEVEL;
		}

		// Token: 0x06003E9D RID: 16029 RVA: 0x0013E3A7 File Offset: 0x0013C7A7
		private bool IsTownShopVisible()
		{
			return this.shop.isActiveAndEnabled;
		}

		// Token: 0x06003E9E RID: 16030 RVA: 0x0013E3B4 File Offset: 0x0013C7B4
		private bool HasBuildingSelected()
		{
			return BuildingLocation.Selected != null;
		}

		// Token: 0x06003E9F RID: 16031 RVA: 0x0013E3C1 File Offset: 0x0013C7C1
		private bool HasBlocker()
		{
			return BlockerManager.global.HasBlockers;
		}

		// Token: 0x040067BC RID: 26556
		private GameStateService gameStateService;

		// Token: 0x040067BD RID: 26557
		private TownShopRoot shop;

		// Token: 0x040067BE RID: 26558
		private PTConfig sbsConfig;

		// Token: 0x040067BF RID: 26559
		private SessionService sessionService;

		// Token: 0x040067C0 RID: 26560
		private AssetBundleService assetBundleService;
	}
}
