using System.Collections;
using Match3.Scripts1.Puzzletown.Config;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Puzzletown.UI;
using Match3.Scripts1.Shared.Flows;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.Signals;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;
using Match3.Scripts2.Shop;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Flows
{
	// Token: 0x020004C4 RID: 1220
	public class PurchaseFlow : AFlow<MaterialAmount>
	{
		// Token: 0x06002230 RID: 8752 RVA: 0x00094AE4 File Offset: 0x00092EE4
		public PurchaseFlow(TrackingService.PurchaseFlowContext purchaseFlowContext, TownDiamondsPanelRoot.TownDiamondsPanelRootParameters purchaseDiamondsContext)
		{
			this._purchaseFlowContext = purchaseFlowContext;
			this._purchaseDiamondsContext = purchaseDiamondsContext;
		}

		// Token: 0x06002231 RID: 8753 RVA: 0x00094AFC File Offset: 0x00092EFC
		protected override IEnumerator FlowRoutine(MaterialAmount purchase)
		{
			yield return ServiceLocator.Instance.Inject(this);
			if (purchase.type == "diamonds")
			{
				yield return new PurchaseDiamondsJourney(this._purchaseDiamondsContext, true).Start();
			}
			else
			{
				AwaitSignal<bool> onClose = new AwaitSignal<bool>();
				MaterialAmount fallback = this.configService.general.GetConversionPrice(purchase);
				Wooroutine<PopupDialogRoot> popupShowRoutine;
				if (purchase.type == "earned_season_currency")
				{
					SeasonConfig season = this.seasonService.GetActiveSeason();
					if (season == null)
					{
						yield break;
					}
					fallback = this.seasonService.GetConversionPrice(purchase);
					string title = this.loc.GetText("ui.specialdecos.shared.purchase.tokens.title", new LocaParam[0]);
					LocaParam[] locaParams = new LocaParam[]
					{
						new LocaParam("{seasonCurrencyIcon}", season.TMProIconName),
						new LocaParam("{diamondsIcon}", fallback.SpriteName)
					};
					string content = this.loc.GetText("ui.specialdecos.shared.purchase.tokens.content", locaParams);
					Wooroutine<SpriteManager> spriteManagerRoutine = new SeasonSpriteManagerFlow().Start<SpriteManager>();
					yield return spriteManagerRoutine;
					SpriteManager seasonSpriteManager = spriteManagerRoutine.ReturnValue;
					Sprite currencySprite = (!(seasonSpriteManager != null)) ? null : seasonSpriteManager.GetSimilar("season_currency");
					PopupSeasonalCurrencyCell.Data currencyCell = new PopupSeasonalCurrencyCell.Data
					{
						sprite = currencySprite,
						label = purchase.amount.ToString()
					};
					popupShowRoutine = PopupDialogRoot.Show(new object[]
					{
						TextData.Title(title),
						TextData.Content(content),
						currencyCell,
						new CloseButton(delegate()
						{
							onClose.Dispatch(false);
						}),
						new PricedButtonWithCallback(fallback, delegate()
						{
							onClose.Dispatch(true);
						})
					});
				}
				else
				{
					string text = "boost_";
					string title;
					string content;
					if (purchase.type.StartsWith(text))
					{
						string arg = purchase.type.Substring(text.Length);
						title = this.loc.GetText(string.Format("ui.boosts.ingame.add.{0}.title", arg), new LocaParam[0]);
						content = this.loc.GetText(string.Format("ui.boosts.ingame.add.{0}.body", arg), new LocaParam[0]);
					}
					else
					{
						title = string.Format(this.loc.GetText("ui.shared.purchase.title", new LocaParam[0]), purchase.FormatName(this.loc));
						content = string.Format(this.loc.GetText("ui.shared.purchase.content", new LocaParam[0]), purchase.Format(this.loc), fallback.Format(this.loc));
					}
					popupShowRoutine = PopupDialogRoot.Show(new object[]
					{
						TextData.Title(title),
						TextData.Content(content),
						purchase,
						new CloseButton(delegate()
						{
							onClose.Dispatch(false);
						}),
						new PricedButtonWithCallback(fallback, delegate()
						{
							onClose.Dispatch(true);
						})
					});
				}
				yield return popupShowRoutine;
				yield return popupShowRoutine.ReturnValue.onDisabled.Await();
				if (onClose.Dispatched)
				{
					if (this.state.Resources.Pay(new MaterialAmount[]
					{
						fallback
					}))
					{
						this.state.Resources.AddMaterials(new MaterialAmount[]
						{
							purchase
						}, true);
						this.HandleTracking(this._purchaseFlowContext, fallback, purchase);
					}
					else
					{
						Wooroutine<bool> routine = new PaymentFlow(this._purchaseFlowContext, this._purchaseDiamondsContext).Start(new MaterialAmount[]
						{
							fallback
						});
						yield return routine;
						if (routine.ReturnValue)
						{
							this.state.Resources.AddMaterials(new MaterialAmount[]
							{
								purchase
							}, true);
							this.HandleTracking(this._purchaseFlowContext, fallback, purchase);
						}
					}
				}
			}
			yield break;
		}

		// Token: 0x06002232 RID: 8754 RVA: 0x00094B20 File Offset: 0x00092F20
		private void HandleDiamondTrackingCall(MaterialAmount purchase, int diamonds)
		{
			TrackingService.PurchaseFlowContext purchaseFlowContext = new TrackingService.PurchaseFlowContext();
			purchaseFlowContext.det1 = "not_enough_coins";
			purchaseFlowContext.det2 = "building";
			purchaseFlowContext.det3 = this._purchaseFlowContext.det2;
			purchaseFlowContext.det4 = purchase.amount.ToString();
			purchaseFlowContext.action = "spent";
			this.trackingService.TrackPurchase(purchaseFlowContext, 0, diamonds);
		}

		// Token: 0x06002233 RID: 8755 RVA: 0x00094B8C File Offset: 0x00092F8C
		private void HandleCoinsTrackingCall(MaterialAmount purchase, int diamonds)
		{
			TrackingService.PurchaseFlowContext purchaseFlowContext = new TrackingService.PurchaseFlowContext();
			purchaseFlowContext.det1 = "coins_for_diamonds";
			purchaseFlowContext.det2 = "building";
			purchaseFlowContext.det3 = this._purchaseFlowContext.det2;
			purchaseFlowContext.det4 = diamonds.ToString();
			purchaseFlowContext.action = "gained";
			this.trackingService.TrackPurchase(purchaseFlowContext, purchase.amount, 0);
		}

		// Token: 0x06002234 RID: 8756 RVA: 0x00094BF8 File Offset: 0x00092FF8
		private void HandleTracking(TrackingService.PurchaseFlowContext context, MaterialAmount fallback, MaterialAmount purchase)
		{
			if (context == null)
			{
				return;
			}
			int coins = (!(fallback.type == "coins")) ? 0 : fallback.amount;
			int diamonds = (!(fallback.type == "diamonds")) ? 0 : fallback.amount;
			if (fallback.type == "diamonds")
			{
				this.HandleDiamondTrackingCall(purchase, diamonds);
				this.HandleCoinsTrackingCall(purchase, diamonds);
			}
			else
			{
				this.trackingService.TrackPurchase(context, coins, diamonds);
			}
		}

		// Token: 0x04004D93 RID: 19859
		[WaitForService(true, true)]
		private GameStateService state;

		// Token: 0x04004D94 RID: 19860
		[WaitForService(true, true)]
		private ILocalizationService loc;

		// Token: 0x04004D95 RID: 19861
		[WaitForService(true, true)]
		private ConfigService configService;

		// Token: 0x04004D96 RID: 19862
		[WaitForService(true, true)]
		private TrackingService trackingService;

		// Token: 0x04004D97 RID: 19863
		[WaitForService(true, true)]
		private SeasonService seasonService;

		// Token: 0x04004D98 RID: 19864
		private TrackingService.PurchaseFlowContext _purchaseFlowContext;

		// Token: 0x04004D99 RID: 19865
		private TownDiamondsPanelRoot.TownDiamondsPanelRootParameters _purchaseDiamondsContext;
	}
}
