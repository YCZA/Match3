using System.Collections;
using Match3.Scripts1.Puzzletown.Flows;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Shared.Flows;
using Wooga.Coroutines;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;
using Match3.Scripts2.Shop;

// Token: 0x02000A3B RID: 2619
namespace Match3.Scripts1
{
	public class GrandPrizePurchaseFlow : AFlow
	{
		// Token: 0x06003EC7 RID: 16071 RVA: 0x0013F1C0 File Offset: 0x0013D5C0
		public static GrandPrizePurchaseFlow CreateWithMockData(SeasonPrizeInfo prizeInfo)
		{
			return new GrandPrizePurchaseFlow
			{
				_seasonData = prizeInfo
			};
		}

		// Token: 0x06003EC8 RID: 16072 RVA: 0x0013F1DC File Offset: 0x0013D5DC
		protected override IEnumerator FlowRoutine()
		{
			yield return ServiceLocator.Instance.Inject(this);
			yield return SceneManager.Instance.Inject(this);
			if (this.sbsService == null || !this.sbsService.SbsConfig.feature_switches.show_seasonal_grand_prize_offer)
			{
				yield break;
			}
			SeasonPrizeInfo previousSeason = (this._seasonData == null) ? this.seasonService.GetPreviousSeasonPrizeInfo() : this._seasonData;
			Wooroutine<bool> bundlesAvailable = this.seasonService.AreAllSeasonBundlesAvailable(previousSeason.BundleNames);
			yield return bundlesAvailable;
			if (!bundlesAvailable.ReturnValue)
			{
				yield break;
			}
			yield return this.buildingResources.LoadFilteredSpritesRoutine(BuildingResourceServiceRoot.CreateSeasonSpritesFilter(previousSeason.name));
			int currentProgress = previousSeason.grandPrizeProgress;
			SeasonService.PriceInfo grandPrizeCost = this.seasonService.CalculatePriceForGrandPrize(previousSeason, currentProgress);
			BuildingConfig buildingConfig = this.seasonService.GrandPrizeConfigForSeason(previousSeason.name);
			string[] uiTackingSubtypes = new string[]
			{
				previousSeason.name,
				buildingConfig.name,
				((int)grandPrizeCost.discountPercent).ToString()
			};
			bool grandPrizePurchased;
			do
			{
				Wooroutine<PopupGrandPrizeLastChanceRoot> popup = SceneManager.Instance.LoadSceneWithParams<PopupGrandPrizeLastChanceRoot, SeasonPrizeInfo>(previousSeason, null);
				yield return popup;
				this.trackingService.TrackUi("grand_prize_offer", string.Empty, "open", string.Empty, uiTackingSubtypes);
				popup.ReturnValue.Show();
				yield return popup.ReturnValue.onDestroyed;
				bool purchaseBuilding = popup.ReturnValue.Response == PopupOperation.OK;
				if (!purchaseBuilding)
				{
					Wooroutine<PopupGrandPrizeLastChancePurchaseRoot> aversion = SceneManager.Instance.LoadSceneWithParams<PopupGrandPrizeLastChancePurchaseRoot, SeasonPrizeInfo>(previousSeason, null);
					yield return aversion;
					this.trackingService.TrackUi("grand_prize_offer", "are_you_sure", "open", string.Empty, uiTackingSubtypes);
					aversion.ReturnValue.Show();
					yield return aversion.ReturnValue.onDestroyed;
					purchaseBuilding = (aversion.ReturnValue.Response == PopupOperation.OK);
					if (purchaseBuilding)
					{
						this.trackingService.TrackUi("grand_prize_offer", "are_you_sure", "buy", string.Empty, uiTackingSubtypes);
					}
					else
					{
						this.trackingService.TrackUi("grand_prize_offer", "are_you_sure", "close", string.Empty, uiTackingSubtypes);
					}
				}
				else
				{
					this.trackingService.TrackUi("grand_prize_offer", string.Empty, "buy", string.Empty, uiTackingSubtypes);
				}
				if (!purchaseBuilding)
				{
					break;
				}
				bool cannotAfford = !this.gameStateService.Resources.HasEnoughMaterial(grandPrizeCost.MaterialCost);
				if (cannotAfford)
				{
					TownDiamondsPanelRoot.TownDiamondsPanelRootParameters townDiamondsPanelRootParameters = new TownDiamondsPanelRoot.TownDiamondsPanelRootParameters
					{
						source1 = "grand_prize_offer"
					};
					yield return new PurchaseDiamondsJourney(townDiamondsPanelRootParameters, true).Start();
				}
				grandPrizePurchased = false;
				if (this.gameStateService.Resources.Pay(grandPrizeCost.MaterialCost))
				{
					grandPrizePurchased = true;
					this.gameStateService.Buildings.StoreBuilding(buildingConfig, 1);
					this.gameStateService.Save(false);
					this.trackingService.TrackGrandPrizePurchase(previousSeason, grandPrizeCost, buildingConfig.name);
					yield return new ForceUserPlaceDecoFlow(buildingConfig).Start();
				}
			}
			while (!grandPrizePurchased);
			this.seasonService.MarkPreviousGrandPrizeOfferSeen();
			yield break;
		}

		// Token: 0x040067FE RID: 26622
		[WaitForService(true, true)]
		private SeasonService seasonService;

		// Token: 0x040067FF RID: 26623
		[WaitForService(true, true)]
		private GameStateService gameStateService;

		// Token: 0x04006800 RID: 26624
		[WaitForService(true, true)]
		private TrackingService trackingService;

		// Token: 0x04006801 RID: 26625
		[WaitForService(true, true)]
		private SBSService sbsService;

		// Token: 0x04006802 RID: 26626
		[WaitForRoot(false, false)]
		private BuildingResourceServiceRoot buildingResources;

		// Token: 0x04006803 RID: 26627
		private SeasonPrizeInfo _seasonData;

		// Token: 0x02000A3C RID: 2620
		public class Trigger : PopupManager.Trigger
		{
			// Token: 0x06003EC9 RID: 16073 RVA: 0x0013F1F7 File Offset: 0x0013D5F7
			public Trigger(GameStateService gameStateService, SeasonService seasonService)
			{
				this.gameStateService = gameStateService;
				this.seasonService = seasonService;
			}

			// Token: 0x06003ECA RID: 16074 RVA: 0x0013F210 File Offset: 0x0013D610
			public override bool ShouldTrigger()
			{
				return this.seasonService.PreviousGrandPrizeOfferAvailable();
			}

			// Token: 0x06003ECB RID: 16075 RVA: 0x0013F22C File Offset: 0x0013D62C
			public override IEnumerator Run()
			{
				yield return new GrandPrizePurchaseFlow().Start();
				this.gameStateService.Save(false);
				yield break;
			}

			// Token: 0x04006804 RID: 26628
			private readonly GameStateService gameStateService;

			// Token: 0x04006805 RID: 26629
			private readonly SeasonService seasonService;
		}
	}
}
