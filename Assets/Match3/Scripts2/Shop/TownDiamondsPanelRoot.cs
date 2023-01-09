using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AndroidTools;
using Match3.Scripts1;
using Match3.Scripts1.Audio;
using Match3.Scripts1.Puzzletown.Config;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Puzzletown.Town;
using Match3.Scripts1.Puzzletown.UI;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.Signals;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x020009C4 RID: 2500
namespace Match3.Scripts2.Shop
{
	public class TownDiamondsPanelRoot : APtSceneRoot<TownDiamondsPanelRoot.TownDiamondsPanelRootParameters>, IPersistentDialog
	{
		// Token: 0x17000910 RID: 2320
		// (get) Token: 0x06003C8A RID: 15498 RVA: 0x0012EC31 File Offset: 0x0012D031
		protected override bool IsSetup
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000911 RID: 2321
		// (get) Token: 0x06003C8B RID: 15499 RVA: 0x0012EC34 File Offset: 0x0012D034
		// (set) Token: 0x06003C8C RID: 15500 RVA: 0x0012EC3C File Offset: 0x0012D03C
		public TownDiamondsPanelRoot.TownDiamondsPanelRootParameters context
		{
			get
			{
				return this.parameters;
			}
			set
			{
				this.parameters = value;
			}
		}

		// Token: 0x17000912 RID: 2322
		// (get) Token: 0x06003C8D RID: 15501 RVA: 0x0012EC45 File Offset: 0x0012D045
		public int NumberOfWindowOpens
		{
			get
			{
				return TownDiamondsPanelRoot.numberOfWindowOpens;
			}
		}

		// Token: 0x06003C8E RID: 15502 RVA: 0x0012EC4C File Offset: 0x0012D04C
		protected override void Awake()
		{
			base.Awake();
			if (this.OldOfferPrototype != null)
			{
				this.OldOfferPrototype.gameObject.SetActive(false);
			}
			if (this.OfferPrototype != null)
			{
				this.OfferPrototype.gameObject.SetActive(false);
			}
			if (this.ItemPrototype != null)
			{
				this.ItemPrototype.gameObject.SetActive(false);
			}
			if (this.errorPanel != null)
			{
				this.errorPanel.gameObject.SetActive(false);
			}
			if (base.gameObject != null)
			{
				base.gameObject.SetActive(false);
			}
			if (this.cancelButton != null)
			{
				this.cancelButton.onClick.AddListener(new UnityAction(this.CancelClicked));
			}
			if (this.retryButton != null)
			{
				this.retryButton.onClick.AddListener(new UnityAction(this.PopulatePurchaseList));
			}
			if (this.sessionService != null && this.sessionService.onRestart != null)
			{
				this.sessionService.onRestart.AddListenerOnce(new Action(this.HandleRestart));
			}
			// 补单查询事件
			PayHandler.onGrantReplenishment = OnGrantReplenishment;
		}

		// Token: 0x06003C8F RID: 15503 RVA: 0x0012ED98 File Offset: 0x0012D198
		protected override void OnEnable()
		{
			base.OnEnable();
			this.dailyDealsService.RefreshDailyDeal();
			this.dialog.Show();
			BackButtonManager.Instance.AddAction(new Action(this.CancelClicked));
			this.audioService.PlaySFX(AudioId.PopupShowDefault, false, false, false);
			TownDiamondsPanelRoot.numberOfWindowOpens++;
		
			// 从服务器获取商品列表
			// Debug.Log("获取商品列表");
			// iapService.InitStoreItem(StopCoroutine, (s)=>StartCoroutine(s));
		
			this.trackingService.TrackDiamondsPurchaseAction("shop_open", null, null, this.context, string.Empty);
		}

		// Token: 0x06003C90 RID: 15504 RVA: 0x0012EE14 File Offset: 0x0012D214
		private void CloseWithCompletedSignal(bool dispatchPurchaseComplete = true)
		{
			if (dispatchPurchaseComplete)
			{
				this.onPurchaseComplete.Dispatch();
			}
			this.Close();
		}

		// Token: 0x06003C91 RID: 15505 RVA: 0x0012EE2D File Offset: 0x0012D22D
		private void Close()
		{
			this.dialog.Hide();
			BackButtonManager.Instance.RemoveAction(new Action(this.CancelClicked));
			this.audioService.PlaySFX(AudioId.PopupHideDefault, false, false, false);
		}

		// Token: 0x06003C92 RID: 15506 RVA: 0x0012EE64 File Offset: 0x0012D264
		private void CancelClicked()
		{
			this.CloseWithCompletedSignal(true);
		}

		public void TryPurchase(int id)
		{
			foreach (var iapData in iapService.IAPs)
			{
				if (iapData.id == id)
				{
					TryPurchase(iapData);
					break;
				}
			}
		}
		// Token: 0x06003C93 RID: 15507 RVA: 0x0012EE6D File Offset: 0x0012D26D
		public void TryPurchase(IAPData data)
		{
			// eli key point 购买直接给奖励
			if (this.CurrentlyPurchasing)
			{
				return;
			}
			this.CurrentlyPurchasing = true;
			WooroutineRunner.StartCoroutine(this.TryPurchaseRoutine(data));
		}

		// Token: 0x06003C94 RID: 15508 RVA: 0x0012EE90 File Offset: 0x0012D290
		public Wooroutine<PurchaseResult> TryPurchaseWithResult(IAPData data)
		{
			if (!this.CurrentlyPurchasing)
			{
				this.CurrentlyPurchasing = true;
				return WooroutineRunner.StartWooroutine<PurchaseResult>(this.TryPurchaseRoutine(data));
			}
			return null;
		}

		// Token: 0x06003C95 RID: 15509 RVA: 0x0012EEB8 File Offset: 0x0012D2B8
		private IEnumerator TryPurchaseRoutine(IAPData data)
		{
			this.StorePanel.SetActive(false);
			// 显示loading界面
			Wooroutine<LoadingSpinnerRoot> loadingSpinner = SceneManager.Instance.LoadScene<LoadingSpinnerRoot>(null);
			yield return loadingSpinner;
			yield return new WaitForSeconds(loadingSpinner.ReturnValue.dialog.open.length + 0.33f);
			// 进行购买
			Wooroutine<PurchaseResult> tryIap = this.iapService.TryPurchase(data);
			yield return tryIap;
			// 购买结束
			this.CurrentlyPurchasing = false;
			loadingSpinner.ReturnValue.Close();
			if (tryIap.ReturnValue.success)
			{
				Debug.Log("purchase success: " + data.price);
// #if AAK
// 			AntiAddictionCtrl.Instance.OnPaySuccess((int)(data.price*100));
// #endif
				yield return this.PurchaseSucceeded(data);
			}
			else
			{
				Debug.Log("purchase failure: " + data.price);
				// yield return this.PurchaseFailed(data, Enum.GetName(typeof(PurchaseFailureReason), tryIap.ReturnValue.failureReason));
				yield return this.PurchaseFailed(data, "购买已取消或失败");
			}
			yield return tryIap.ReturnValue;
		}

		public void OnGrantReplenishment(int id)
		{
			IAPData data = null;
			foreach (var item in iapService.IAPs)
			{
				if (item.id == id)
				{
					data = item;
					break;
				}
			}

			if (data == null)
			{
				Debug.LogError("failed to get iapdata, id:" + id);
				return;
			}

			Debug.Log("查询到补单");
			WooroutineRunner.StartCoroutine(PurchaseSucceeded(data));
		}

		// Token: 0x06003C96 RID: 15510 RVA: 0x0012EEDC File Offset: 0x0012D2DC
		private IEnumerator PurchaseFailed(IAPData data, string failureReason)
		{
			// this.trackingService.TrackDiamondsPurchaseAction("purchase_failed", data, this.iapService.GetContents(data), this.context, failureReason);
			PopupDialogRoot.Show(new object[]
			{
				TextData.Title(this.localizationService.GetText("ui.common.failed_title", new LocaParam[0])),
				// IllustrationType.SadCharacter,
				TextData.Content(this.localizationService.GetText("ui.shop.purchase_failed", new LocaParam[0])),
				new LabeledButtonWithCallback(this.localizationService.GetText("ui.shop.purchase_retry", new LocaParam[0]), null)
			});
			yield return PopupDialogRoot.ShowOkDialog(this.localizationService.GetText("ui.common.failed_title", new LocaParam[0]), string.Format(this.localizationService.GetText("ui.shop.purchase_failed", new LocaParam[0]), data.iap_name), null, null);
			this.StorePanel.SetActive(true);
			yield break;
		}

		// Token: 0x06003C97 RID: 15511 RVA: 0x0012EF08 File Offset: 0x0012D308
		private IEnumerator PurchaseSucceeded(IAPData data)
		{
			List<MaterialAmount> materialAmounts = (from c in this.iapService.GetContents(data)
				select c.materialAmount).ToList<MaterialAmount>();
			// 发放补单调用这里时，这句会报空指针错误
			// this.trackingService.TrackDiamondsPurchaseAction("purchase_done", data, this.iapService.GetContents(data), this.context, string.Empty);
			// string iapLocalization = string.Format(this.localizationService.GetText("ui.shared.purchase.diamonds.report", new LocaParam[0]), this.localizationService.GetText("shop.diamonds.title." + data.iap_name, new LocaParam[0]), data.storeProduct.metadata.localizedPriceString);
			string iapLocalization = "@@@@@@@@@@@@@lalalala@@@@@@@@@@@@@";
			if (!string.IsNullOrEmpty(data.localization_key))
			{
				iapLocalization = string.Format(this.localizationService.GetText(data.localization_key, new LocaParam[0]), (from ma in materialAmounts
					select ma.amount.ToString()).ToArray<string>());
			}
			string title = this.localizationService.GetText("ui.shared.purchase.diamonds.success", new LocaParam[0]);
			string content = iapLocalization;
			string action = this.localizationService.GetText("ui.shared.purchase.diamonds.proceed", new LocaParam[0]);
			if (data.IsDailyDeal)
			{
				this.gameStateService.DailyDeals.CurrentDealPurchased = data.IsDailyDeal;
			}
			this.CloseWithCompletedSignal(false);
			this.gameStateService.SetSeenFlag(data.iap_name);
			if (!this.configService.FeatureSwitchesConfig.new_shop_layout)
			{
				materialAmounts = materialAmounts.Take(2).ToList<MaterialAmount>();
			}
			for (int i = 0; i < materialAmounts.Count; i++)
			{
				MaterialAmount materialAmount = materialAmounts[i];
				if (materialAmount.type == "lives_unlimited")
				{
					this.livesService.StartUnlimitedLives(materialAmount.amount);
				}
			}
			List<object> itemsToShow = new List<object>();
			itemsToShow.Add(TextData.Title(title));
			itemsToShow.Add(TextData.Content(content));
			if (materialAmounts.Count > 1)
			{
				if (this.configService.FeatureSwitchesConfig.new_shop_layout)
				{
					itemsToShow.Add(new Bundle
					{
						materials = materialAmounts
					});
				}
				else
				{
					itemsToShow.Add(materialAmounts);
				}
			}
			else
			{
				itemsToShow.Add(materialAmounts[0]);
			}
			itemsToShow.Add(new LabeledButtonWithCallback(action, null));
			Wooroutine<PopupDialogRoot> popupDialog = PopupDialogRoot.Show(itemsToShow.ToArray());
			yield return popupDialog;
			yield return new WaitForSeconds(popupDialog.ReturnValue.dialog.open.length);
			string buildingName = string.Empty;
			foreach (MaterialAmount material in materialAmounts)
			{
				if (!material.type.StartsWith("iso_"))
				{
					this.gameStateService.Resources.AddMaterial(material, true, "内购"+data.iap_name);
					this.audioService.PlaySFX(this.audioService.GetSimilar(material.type + "Purchased"), false, false, false);
				}
				else
				{
					buildingName = material.type;
				}
			}
			// 宝石飞的动画
			// MaterialAmountView.CollectMaterials(this.resourcePanel, popupDialog.ReturnValue.targetLayout.gameObject);
			yield return popupDialog.ReturnValue.onDisabled.Await();
			// 关闭对话框后才执行到这里
			// while (Doober.ActiveDoobers > 0)
			// {
			// 	yield return null;
			// }
			this.offersService.UpdateCurrentOffer(false);
			this.onPurchaseComplete.Dispatch();
			if (!buildingName.IsNullOrEmpty())
			{
				if (SceneManager.IsPlayingMatch3)
				{
					this.buildingConfig = this.configService.buildingConfigList.GetConfig(buildingName);
					this.gameStateService.Buildings.StoreBuilding(this.buildingConfig, 1);
				}
				else
				{
					yield return this.WaitForPlacementRoutine(buildingName);
				}
			}
		}

		// Token: 0x06003C98 RID: 15512 RVA: 0x0012EF2C File Offset: 0x0012D32C
		private IEnumerator WaitForPlacementRoutine(string buildingName)
		{
			this.buildingConfig = this.configService.buildingConfigList.GetConfig(buildingName);
			Wooroutine<TownMainRoot> townMainLoader = SceneManager.Instance.Await<TownMainRoot>(true);
			yield return townMainLoader;
			TownMainRoot townMain = townMainLoader.ReturnValue;
			townMain.townLoader.TryPlaceBuilding(new BuildingShopView.BuildingBuildRequest
			{
				Config = this.buildingConfig,
				isFree = true,
				shouldZoomIn = true,
				shouldPanCamera = true,
				zoomInRatio = BuildingShopView.BuildingBuildRequest.CalculateZoomFactorBasedOnSize(this.buildingConfig)
			}, true);
			townMain.townLoader.buildingServices.BuildingsController.onPurchaseCancelled.RemoveAllListeners();
			townMain.townLoader.buildingServices.BuildingsController.onPurchaseCancelled.AddListener(new Action(this.OnPlacementCanceled));
			while (BuildingLocation.Selected != null)
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x06003C99 RID: 15513 RVA: 0x0012EF4E File Offset: 0x0012D34E
		private void OnPlacementCanceled()
		{
			if (this.buildingConfig != null)
			{
				this.gameStateService.Buildings.StoreBuilding(this.buildingConfig, 1);
			}
		}

		private void PopulatePurchaseList()
		{
			// eli key point iap initalialized
			if (this.iapService.initalialized || true)
			{
				this.errorPanel.gameObject.SetActive(false);
				this.ItemList.SetActive(true);
				foreach (GameObject obj in this.listItems)
				{
					UnityEngine.Object.Destroy(obj);
				}
				listItems.Clear();
			
				bool flag = false;
				foreach (IAPData iapdata in this.iapService.IAPs)
				{
					bool isOffer = iapdata.iap_type == "offer";
					if (iapdata.IsAvailable && (!isOffer || this.offersService.CurrentOffer == iapdata) && !iapdata.IsSale)
					{
						BaseShopPanelItem baseShopPanelItem = null;
						if (isOffer)
							// if (isOffer && parameters.storeType == StoreType.StarterPack)
						{
							// eli todo 暂时没有限时礼包
							if (flag || this.gameStateService.DailyDeals.CurrentDealActive)
							{
								goto IL_2A4;
							}

							if (parameters.storeType != StoreType.StarterPack)
							{
								goto IL_2A4;
							}
							// baseShopPanelItem = UnityEngine.Object.Instantiate<BaseShopPanelItem>((!this.configService.FeatureSwitchesConfig.new_shop_layout) ? this.OldOfferPrototype : this.OfferPrototype);
							// 改用OfferPrototype
							baseShopPanelItem = UnityEngine.Object.Instantiate<BaseShopPanelItem>(this.OfferPrototype);
						
							baseShopPanelItem.transform.SetParent(this.ItemList.transform.parent, false);
							baseShopPanelItem.transform.SetSiblingIndex(1);
							flag = true;
						}
						else if (iapdata.iap_type == "daily_deal")
						{
							if (!this.gameStateService.DailyDeals.CurrentDealActive || flag || this.gameStateService.DailyDeals.CurrentDeal.bundle_id != iapdata.iap_name)
							{
								goto IL_2A4;
							}
							// DailyDealsPanel dailyDealsPanel = UnityEngine.Object.Instantiate<DailyDealsPanel>(this.dailyDealsPrototype);
							// dailyDealsPanel.SetDeal(this.gameStateService.DailyDeals.CurrentDeal, this.gameStateService.DailyDeals.CurrentDealExpirationDate, this.gameStateService.DailyDeals.CurrentDealPurchased, this.configService, this.dailyDealsService.BuildingSprite);
							// baseShopPanelItem = dailyDealsPanel;
							// baseShopPanelItem.transform.SetParent(this.ItemList.transform.parent, false);
							// baseShopPanelItem.transform.SetSiblingIndex(1);
							// flag = true;
						}
						// else
						else if(parameters.storeType == StoreType.Diamond)
						{
							// other goods
							baseShopPanelItem = UnityEngine.Object.Instantiate<BaseShopPanelItem>(this.ItemPrototype);
							baseShopPanelItem.transform.SetParent(this.ItemList.transform, false);
						}

						if (baseShopPanelItem != null)
						{
							baseShopPanelItem.Init(this.localizationService);
							baseShopPanelItem.gameObject.SetActive(true);
							baseShopPanelItem.SetData(this.iapService, iapdata, new Action<IAPData>(this.TryPurchase));
							this.listItems.Add(baseShopPanelItem.gameObject);
						}
					}
					IL_2A4:;
				}

				// 经过一番修改，终于能用了
				// 不显示钻石列表
				if (parameters.storeType != StoreType.Diamond)
				{
					ItemList.SetActive(false);
				}
			}
			else
			{
				this.ItemList.SetActive(false);
				// 隐藏没有商品的弹窗
				// this.errorPanel.gameObject.SetActive(true);
			}
		}

		// Token: 0x06003C9B RID: 15515 RVA: 0x0012F268 File Offset: 0x0012D668
		private void HandleRestart()
		{
			TownDiamondsPanelRoot.numberOfWindowOpens = 0;
			this.sessionService.onRestart.RemoveListener(new Action(this.HandleRestart));
		}

		// Token: 0x06003C9C RID: 15516 RVA: 0x0012F28C File Offset: 0x0012D68C
		public Coroutine TryToBuy()
		{
			return WooroutineRunner.StartCoroutine(this.TryToBuyRoutine(), null);
		}

		// Token: 0x06003C9D RID: 15517 RVA: 0x0012F29C File Offset: 0x0012D69C
		private IEnumerator TryToBuyRoutine()
		{
			base.Enable();
			this.onPurchaseComplete.Clear();
			this.PopulatePurchaseList();
			this.StorePanel.SetActive(true);
			yield return this.onPurchaseComplete;
			yield break;
		}

		// Token: 0x0400653B RID: 25915
		[WaitForService(true, true)]
		private ILocalizationService localizationService;

		// Token: 0x0400653C RID: 25916
		[WaitForService(true, true)]
		private GameStateService gameStateService;

		// Token: 0x0400653D RID: 25917
		[WaitForService(true, true)]
		private IAPService iapService;

		// Token: 0x0400653E RID: 25918
		[WaitForService(true, true)]
		private LivesService livesService;

		// Token: 0x0400653F RID: 25919
		[WaitForService(true, true)]
		private AudioService audioService;

		// Token: 0x04006540 RID: 25920
		[WaitForService(true, true)]
		private TrackingService trackingService;

		// Token: 0x04006541 RID: 25921
		[WaitForService(true, true)]
		private OffersService offersService;

		// Token: 0x04006542 RID: 25922
		[WaitForService(true, true)]
		private ConfigService configService;

		// Token: 0x04006543 RID: 25923
		[WaitForService(true, true)]
		private DailyDealsService dailyDealsService;

		// Token: 0x04006544 RID: 25924
		[WaitForService(true, true)]
		private SessionService sessionService;

		// Token: 0x04006545 RID: 25925
		[WaitForRoot(false, false)]
		private TownResourcePanelRoot resourcePanel;

		// Token: 0x04006546 RID: 25926
		[SerializeField]
		private Button cancelButton;

		// Token: 0x04006547 RID: 25927
		[SerializeField]
		private GameObject StorePanel;

		// Token: 0x04006548 RID: 25928
		[SerializeField]
		private GameObject ItemList; // diamonds parent

		// Token: 0x04006549 RID: 25929
		[SerializeField]
		private BaseShopPanelItem ItemPrototype;

		// Token: 0x0400654A RID: 25930
		[SerializeField]
		private BaseShopPanelItem OfferPrototype;

		// Token: 0x0400654B RID: 25931
		[SerializeField]
		private BaseShopPanelItem OldOfferPrototype;

		// Token: 0x0400654C RID: 25932
		[SerializeField]
		private DailyDealsPanel dailyDealsPrototype;

		// Token: 0x0400654D RID: 25933
		[SerializeField]
		private GameObject errorPanel;

		// Token: 0x0400654E RID: 25934
		[SerializeField]
		private Button retryButton;

		// Token: 0x0400654F RID: 25935
		[SerializeField]
		private AnimatedUi dialog;

		// Token: 0x04006550 RID: 25936
		private readonly List<GameObject> listItems = new List<GameObject>();

		// Token: 0x04006551 RID: 25937
		private bool CurrentlyPurchasing;

		// Token: 0x04006552 RID: 25938
		private BuildingConfig buildingConfig;

		// Token: 0x04006553 RID: 25939
		private static int numberOfWindowOpens;

		// Token: 0x04006554 RID: 25940
		public AwaitSignal onPurchaseComplete = new AwaitSignal();

		// Token: 0x020009C5 RID: 2501
		public class TownDiamondsPanelRootParameters
		{
			// TrackingContent
			public string source1;
			public string source2;
			public WeeklyEventType eventType;
			public WeeklyEventData eventData;
		
			// store type options
			public StoreType storeType = StoreType.Diamond;
		}
	
		public enum StoreType
		{
			Diamond,
			StarterPack,
		}
	}
}
