using System;
using System.Collections;
using System.Collections.Generic;
using Match3.Scripts1.Audio;
using Match3.Scripts1.Puzzletown.Services;
using Wooga.Coroutines;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;
using Match3.Scripts2.Shop;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000A1C RID: 2588
namespace Match3.Scripts1
{
	[LoadOptions(true, false, false)]
	public class SalePopupRoot : ASceneRoot<string>, IDisposableDialog
	{
		// Token: 0x06003E37 RID: 15927 RVA: 0x0013B8C0 File Offset: 0x00139CC0
		protected override void Go()
		{
			this.sale = this.saleService.CurrentSale;
			if (this.sale != null)
			{
				DateTime lowDateTime = this.sale.GetLowDateTime(this.configService.general.notifications.low_time_event);
				this.countdownTimer.SetTargetTime(this.sale.endDateLocal, lowDateTime, false, new Action(this.Close));
				this.title.text = this.localService.GetText(this.sale.titleLocaKey, new LocaParam[0]);
				this.realPrice.text = this.sale.realPriceString;
				this.discountedPrice.text = this.sale.discountedPriceString;
				if (this.sale.value > 0f)
				{
					this.realPriceContainer.SetActive(false);
					this.discount.text = string.Format(this.localService.GetText("ui.monthly.sale.popup.extra_value.badge", new LocaParam[0]), this.sale.value);
				}
				else
				{
					this.discount.text = string.Format(this.localService.GetText("ui.monthly.sale.popup.discount.badge", new LocaParam[0]), this.sale.discount);
				}
				int num = 0;
				while (num < this.saleItemViews.Count && num < this.sale.content.Count)
				{
					this.saleItemViews[num].Show(this.sale.content[num], this.localService);
					num++;
				}
				this.dialog.Show();
				BackButtonManager.Instance.AddAction(new Action(this.Close));
				this.audioService.PlaySFX(AudioId.PopupShowDefault, false, false, false);
				this.closeButton.onClick.AddListener(new UnityAction(this.Close));
				this.buyButton.onClick.AddListener(new UnityAction(this.Buy));
				this.TrackUI("open");
				this.gameStateService.Sale.MarkSeenToday();
				bool flag = lowDateTime < DateTime.Now;
				if (this.showLimitedTime != null && flag)
				{
					this.showLimitedTime.Play();
				}
			}
			else
			{
				this.dialog.Show();
				this.dialog.Hide();
			}
		}

		// Token: 0x06003E38 RID: 15928 RVA: 0x0013BB3F File Offset: 0x00139F3F
		private void Buy()
		{
			this.buyButton.interactable = false;
			this.closeButton.interactable = false;
			base.StartCoroutine(this.BuyRoutine());
		}

		// Token: 0x06003E39 RID: 15929 RVA: 0x0013BB68 File Offset: 0x00139F68
		private IEnumerator BuyRoutine()
		{
			this.TrackUI("buy");
			TownDiamondsPanelRoot.TownDiamondsPanelRootParameters context = new TownDiamondsPanelRoot.TownDiamondsPanelRootParameters();
			context.source1 = "sale";
			context.source2 = this.parameters;
			this.townDiamondsPanelRoot.context = context;
			Wooroutine<PurchaseResult> purchaseRoutine = this.townDiamondsPanelRoot.TryPurchaseWithResult(this.sale.iap);
			yield return purchaseRoutine;
			PurchaseResult purchaseResult = purchaseRoutine.ReturnValue;
			if (purchaseResult.success)
			{
				this.saleService.MarkBoughtSale(this.sale);
				this.Close();
			}
			else
			{
				this.buyButton.interactable = true;
			}
			this.closeButton.interactable = true;
			yield break;
		}

		// Token: 0x06003E3A RID: 15930 RVA: 0x0013BB84 File Offset: 0x00139F84
		private void Close()
		{
			this.TrackUI("close");
			this.audioService.PlaySFX(AudioId.PopupHideDefault, false, false, false);
			BackButtonManager.Instance.RemoveAction(new Action(this.Close));
			this.dialog.Hide();
		}

		// Token: 0x06003E3B RID: 15931 RVA: 0x0013BBD4 File Offset: 0x00139FD4
		private void TrackUI(string action)
		{
			this.trackingService.TrackUi("sale", this.parameters, action, string.Empty, new object[]
			{
				this.sale.id
			});
		}

		// Token: 0x04006727 RID: 26407
		public const string ORIGIN_LEVEL_START_BANNER = "level_start_banner";

		// Token: 0x04006728 RID: 26408
		public const string ORIGIN_PLAYER_TRIGGERED = "player_triggered";

		// Token: 0x04006729 RID: 26409
		public const string ORIGIN_ISLAND = "island";

		// Token: 0x0400672A RID: 26410
		public const string TRACKING_SALE = "sale";

		// Token: 0x0400672B RID: 26411
		[SerializeField]
		private AnimatedUi dialog;

		// Token: 0x0400672C RID: 26412
		[SerializeField]
		private Button closeButton;

		// Token: 0x0400672D RID: 26413
		[SerializeField]
		private Button buyButton;

		// Token: 0x0400672E RID: 26414
		[SerializeField]
		private TextMeshProUGUI title;

		// Token: 0x0400672F RID: 26415
		[SerializeField]
		private CountdownTimer countdownTimer;

		// Token: 0x04006730 RID: 26416
		[SerializeField]
		private TextMeshProUGUI realPrice;

		// Token: 0x04006731 RID: 26417
		[SerializeField]
		private TextMeshProUGUI discountedPrice;

		// Token: 0x04006732 RID: 26418
		[SerializeField]
		private GameObject realPriceContainer;

		// Token: 0x04006733 RID: 26419
		[SerializeField]
		private TextMeshProUGUI discount;

		// Token: 0x04006734 RID: 26420
		[SerializeField]
		private List<SaleItemView> saleItemViews;

		// Token: 0x04006735 RID: 26421
		[SerializeField]
		private Animation showLimitedTime;

		// Token: 0x04006736 RID: 26422
		[WaitForService(true, true)]
		private SaleService saleService;

		// Token: 0x04006737 RID: 26423
		[WaitForService(true, true)]
		private AudioService audioService;

		// Token: 0x04006738 RID: 26424
		[WaitForService(true, true)]
		private IAPService iapService;

		// Token: 0x04006739 RID: 26425
		[WaitForService(true, true)]
		private GameStateService gameStateService;

		// Token: 0x0400673A RID: 26426
		[WaitForService(true, true)]
		private ILocalizationService localService;

		// Token: 0x0400673B RID: 26427
		[WaitForService(true, true)]
		private TrackingService trackingService;

		// Token: 0x0400673C RID: 26428
		[WaitForService(true, true)]
		protected ConfigService configService;

		// Token: 0x0400673D RID: 26429
		[WaitForRoot(false, false)]
		private TownDiamondsPanelRoot townDiamondsPanelRoot;

		// Token: 0x0400673E RID: 26430
		private Sale sale;

		// Token: 0x02000A1D RID: 2589
		public class Trigger : PopupManager.Trigger
		{
			// Token: 0x06003E3C RID: 15932 RVA: 0x0013BC11 File Offset: 0x0013A011
			public Trigger(SaleService salesService, SessionService sessionService, GameStateService gameStateService)
			{
				this.salesService = salesService;
				this.sessionService = sessionService;
				this.gameStateService = gameStateService;
			}

			// Token: 0x06003E3D RID: 15933 RVA: 0x0013BC2E File Offset: 0x0013A02E
			public override bool ShouldTrigger()
			{
				return this.salesService.CurrentSale != null && this.sessionService.NumberOfIslandLoads >= 2 && this.gameStateService.Sale.NotSeenToday();
			}

			// Token: 0x06003E3E RID: 15934 RVA: 0x0013BC64 File Offset: 0x0013A064
			public override IEnumerator Run()
			{
				Wooroutine<SalePopupRoot> scene = SceneManager.Instance.LoadSceneWithParams<SalePopupRoot, string>("island", null);
				yield return scene;
				yield return scene.ReturnValue.onDestroyed;
				yield break;
			}

			// Token: 0x0400673F RID: 26431
			private SaleService salesService;

			// Token: 0x04006740 RID: 26432
			private SessionService sessionService;

			// Token: 0x04006741 RID: 26433
			private GameStateService gameStateService;
		}
	}
}
