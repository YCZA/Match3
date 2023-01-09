using System;
using Match3.Scripts1.Audio;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Wooga.Signals;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;
using Match3.Scripts2.Shop;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.UI
{
	// Token: 0x02000884 RID: 2180
	[LoadOptions(true, true, false)]
	public class StarterPackRoot : APtSceneRoot, IHandler<PopupOperation>, IDisposableDialog
	{
		// Token: 0x06003586 RID: 13702 RVA: 0x00100EE8 File Offset: 0x000FF2E8
		protected override void Go()
		{
			this.dialog.Show();
			BackButtonManager.Instance.AddAction(new Action(this.Close));
			this.audioService.PlaySFX(AudioId.PopupShowDefault, false, false, false);
			if (this.offersService.CurrentOffer != null)
			{
				this.SetupPanels();
			}
			else
			{
				bool showCheatMenus = this.gameStateService.Debug.ShowCheatMenus;
				this.offer.gameObject.SetActive(showCheatMenus);
				this.errorPanel.SetActive(!showCheatMenus);
			}
		}

		// Token: 0x06003587 RID: 13703 RVA: 0x00100F78 File Offset: 0x000FF378
		private void SetupPanels()
		{
			this.TrackUI("open", this.offersService.CurrentOffer.iap_name);
			BaseShopPanelItem baseShopPanelItem = (!configService.FeatureSwitchesConfig.new_shop_layout) ? (BaseShopPanelItem)offer : bundle;
			this.bundle.gameObject.SetActive(false);
			this.offer.gameObject.SetActive(false);
			if (this.iapService.initalialized)
			{
				baseShopPanelItem.Init(this.localizationService);
				IAPData currentOffer = this.offersService.CurrentOffer;
				currentOffer.context = "starter_window";
				baseShopPanelItem.SetData(this.iapService, currentOffer, new Action<IAPData>(this.townDiamondsPanelRoot.TryPurchase));
				TownDiamondsPanelRoot.TownDiamondsPanelRootParameters townDiamondsPanelRootParameters = new TownDiamondsPanelRoot.TownDiamondsPanelRootParameters();
				townDiamondsPanelRootParameters.source1 = "starter_window";
				townDiamondsPanelRootParameters.source2 = "island";
				this.townDiamondsPanelRoot.context = townDiamondsPanelRootParameters;
				this.errorPanel.SetActive(false);
				baseShopPanelItem.gameObject.SetActive(true);
			}
			else
			{
				this.errorPanel.SetActive(true);
			}
		}

		// Token: 0x06003588 RID: 13704 RVA: 0x0010108C File Offset: 0x000FF48C
		public void Close()
		{
			if (this.offersService.CurrentOffer != null)
			{
				this.UpdateAndSaveAndTrack();
			}
			this.dialog.Hide();
			BackButtonManager.Instance.RemoveAction(new Action(this.Close));
			this.audioService.PlaySFX(AudioId.PopupHideDefault, false, false, false);
			this.onClose.Dispatch();
		}

		// Token: 0x06003589 RID: 13705 RVA: 0x001010F0 File Offset: 0x000FF4F0
		private void UpdateAndSaveAndTrack()
		{
			string iap_name = this.offersService.CurrentOffer.iap_name;
			if (this.iapService.initalialized)
			{
				this.gameStateService.SetSeenFlag(this.offersService.CurrentOffer.iap_name);
				this.offersService.UpdateCurrentOffer(false);
			}
			this.TrackUI("close", iap_name);
		}

		// Token: 0x0600358A RID: 13706 RVA: 0x00101154 File Offset: 0x000FF554
		public void Handle(PopupOperation evt)
		{
			if (evt != PopupOperation.Close)
			{
				if (evt == PopupOperation.OK)
				{
					if (this.offersService.CurrentOffer != null)
					{
						this.TrackUI("buy", this.offersService.CurrentOffer.iap_name);
					}
					else
					{
						WoogaDebug.LogWarning(new object[]
						{
							"CURRENT OFFER NULL: Executed ok button"
						});
					}
					this.Close();
				}
			}
			else
			{
				this.Close();
			}
		}

		// Token: 0x0600358B RID: 13707 RVA: 0x001011CD File Offset: 0x000FF5CD
		private void TrackUI(string action, string offerId)
		{
			this.trackingService.TrackUi("starter_window", offerId, action, string.Empty, new object[0]);
		}

		// Token: 0x04005D6A RID: 23914
		[WaitForService(true, true)]
		private GameStateService gameStateService;

		// Token: 0x04005D6B RID: 23915
		[WaitForService(true, true)]
		private TrackingService trackingService;

		// Token: 0x04005D6C RID: 23916
		[WaitForService(true, true)]
		private AudioService audioService;

		// Token: 0x04005D6D RID: 23917
		[WaitForService(true, true)]
		private ILocalizationService localizationService;

		// Token: 0x04005D6E RID: 23918
		[WaitForService(true, true)]
		private ConfigService configService;

		// Token: 0x04005D6F RID: 23919
		[WaitForService(true, true)]
		private IAPService iapService;

		// Token: 0x04005D70 RID: 23920
		[WaitForService(true, true)]
		private OffersService offersService;

		// Token: 0x04005D71 RID: 23921
		[WaitForRoot(false, false)]
		private TownDiamondsPanelRoot townDiamondsPanelRoot;

		// Token: 0x04005D72 RID: 23922
		public const string STARTER_PACK_SEEN_FLAG = "starter_pack";

		// Token: 0x04005D73 RID: 23923
		public const string STARTER_PACK_WINDOW = "starter_window";

		// Token: 0x04005D74 RID: 23924
		public readonly AwaitSignal onClose = new AwaitSignal();

		// Token: 0x04005D75 RID: 23925
		public AnimatedUi dialog;

		// Token: 0x04005D76 RID: 23926
		public ShopPanelOffer offer;

		// Token: 0x04005D77 RID: 23927
		public ShopPanelBundle bundle;

		// Token: 0x04005D78 RID: 23928
		public GameObject errorPanel;
	}
}
