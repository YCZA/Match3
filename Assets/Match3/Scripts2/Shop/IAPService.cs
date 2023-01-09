using System;
using System.Collections;
using System.Collections.Generic;
using AndroidTools;
using AndroidTools.Tools;
using Match3.Scripts1;
using Match3.Scripts1.Puzzletown.Config;
using Match3.Scripts1.Puzzletown.Services;
using Wooga.Coroutines;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;

//using Facebook.MiniJSON;
// using UnityEngine.Purchasing.Extension;

// eli mark 购买

// Token: 0x020007C8 RID: 1992
namespace Match3.Scripts2.Shop
{
	public class IAPService : AService
	{
		// Token: 0x0600310C RID: 12556 RVA: 0x000E60FF File Offset: 0x000E44FF
		public IAPService()
		{
			WooroutineRunner.StartCoroutine(this.LoadRoutine(), null);
		}

		// Token: 0x170007B9 RID: 1977
		// (get) Token: 0x0600310D RID: 12557 RVA: 0x000E6114 File Offset: 0x000E4514
		// (set) Token: 0x0600310E RID: 12558 RVA: 0x000E611C File Offset: 0x000E451C
		public bool initalialized { get; private set; }

		// Token: 0x0600310F RID: 12559 RVA: 0x000E6128 File Offset: 0x000E4528
		// private void InitializeIAPs()
		// {
		// 	this.purchaseInfo = new PurchaseResult();
		// 	ConfigurationBuilder configurationBuilder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance(), new IPurchasingModule[0]);
		// 	foreach (IAPData iapdata in this.configService.iapConfigDataList.iaps)
		// 	{
		// 		IDs ds = new IDs();
		// 		if (!string.IsNullOrEmpty(iapdata.iap_name))
		// 		{
		// 			if (!string.IsNullOrEmpty(iapdata.google_play_id))
		// 			{
		// 				ds.Add(iapdata.google_play_id, new string[]
		// 				{
		// 					"GooglePlay"
		// 				});
		// 			}
		// 			if (!string.IsNullOrEmpty(iapdata.ios_id))
		// 			{
		// 				ds.Add(iapdata.ios_id, new string[]
		// 				{
		// 					"AppleAppStore"
		// 				});
		// 			}
		// 			configurationBuilder.AddProduct(iapdata.iap_name, ProductType.Consumable, ds);
		// 		}
		// 	}
		// 	this.proxyListener = new StoreListenerProxy(new Action<IStoreController, IExtensionProvider>(this.OnIAPInitialized), new Action<InitializationFailureReason>(this.OnIAPInitializeFailed), new Func<PurchaseEventArgs, PurchaseProcessingResult>(this.ProcessIAPPurchase), new Action<Product, PurchaseFailureReason>(this.OnIAPPurchaseFailed));
		// 	this.waitingForInit = true;
		// 	UnityPurchasing.Initialize(this.proxyListener, configurationBuilder);
		// }
	
		// 初始化商品
		public void InitStoreItem(Action<IEnumerator> stopCoroutineAction, Action<IEnumerator> startCoroutineAction)
		{
			if (AndroidPay.productInfoList.Count != 0)
			{
				return;
			}
		
			Dictionary<string, string> itemDic = new Dictionary<string, string>();
			foreach (var item in IAPs)
			{
				if (!itemDic.ContainsKey(item.id.ToString()))
				{
					itemDic.Add(item.id.ToString(), item.iap_name);
				}
			}
			AndroidPay.InitStoreItem(itemDic, stopCoroutineAction, startCoroutineAction);
		}

		// Token: 0x06003110 RID: 12560 RVA: 0x000E624C File Offset: 0x000E464C
		private IEnumerator LoadRoutine()
		{
			yield return ServiceLocator.Instance.Inject(this);
			DateTime start = DateTime.UtcNow;
			// 初始化iap
			// this.InitializeIAPs();
			initalialized = true;	// 直接设置初始化成功
			while (this.waitingForInit)
			{
				DateTime now = DateTime.UtcNow;
				if ((now - start).TotalSeconds > 5.0)
				{
					WoogaDebug.LogWarning(new object[]
					{
						"init failed"
					});
					// this.proxyListener.OnInitializeFailed(InitializationFailureReason.PurchasingUnavailable);
					break;
				}
				yield return null;
			}
			base.OnInitialized.Dispatch();
			yield break;
		}

		// Token: 0x06003111 RID: 12561 RVA: 0x000E6268 File Offset: 0x000E4668
		private IEnumerator DoPurchase(IAPData data)
		{
			// if (!this.purchaseInfo.waiting && this.controller != null)
			{
				purchaseInfo = new PurchaseResult();
		
				this.purchaseInfo.waiting = true;
				this.purchaseInfo.iapToBuy = data;
				// this.adjustService.TrackPurchaseScreenOpen();
				// this.controller.InitiatePurchase(data.iap_name);
				// while (this.purchaseInfo.waiting)
				// {
				// yield return null;
				// }
				// if (this.purchaseInfo.success)
				// {
				this.gameStateService.Transactions.AddTransactionData(this.purchaseInfo.iapToBuy.iap_name, this.purchaseInfo.iapToBuy.price);
				if (this.gameStateService.Transactions.GetNumberOfTransactions() == 3)
				{
					// this.adjustService.TrackThirdPurchase();
				}
				this.gameStateService.Save(true);
				// }
				// eli todo 购买直接成功
				purchaseInfo.success = true;
				yield return this.purchaseInfo;
			}
			// else
			// {
			// 购买失败时:
			// yield return new PurchaseResult(false, null, PurchaseFailureReason.ExistingPurchasePending);
			// }
		}

		private IEnumerator DoPurchaseByAndroid(IAPData data)
		{
			purchaseInfo = new PurchaseResult();
		
			this.purchaseInfo.iapToBuy = data;

#if UNITY_EDITOR
			purchaseInfo.success = true;
#else
		AndroidPay.PayItem(data.id);
		
		while (!PayHandler.isEnd)
		{
			yield return null;
		}
		
		purchaseInfo.success = PayHandler.isSuccess;
#endif
		
			if (this.purchaseInfo.success)
			{
				// 添加购买记录
				this.gameStateService.Transactions.AddTransactionData(this.purchaseInfo.iapToBuy.iap_name, this.purchaseInfo.iapToBuy.price);
				if (this.gameStateService.Transactions.GetNumberOfTransactions() == 3)
				{
					// this.adjustService.TrackThirdPurchase();
				}
				this.gameStateService.Save(true);
			}
			else
			{
				// yield return new PurchaseResult(false, null, PurchaseFailureReason.ExistingPurchasePending);
			}
			// 返回购买结果
			yield return this.purchaseInfo;
		}

		// Token: 0x06003112 RID: 12562 RVA: 0x000E628A File Offset: 0x000E468A
		public Wooroutine<PurchaseResult> TryPurchase(IAPData data)
		{
			// return WooroutineRunner.StartWooroutine<PurchaseResult>(this.DoPurchase(data));
			return WooroutineRunner.StartWooroutine<PurchaseResult>(this.DoPurchaseByAndroid(data));
		}

		// Token: 0x170007BA RID: 1978
		// (get) Token: 0x06003113 RID: 12563 RVA: 0x000E6298 File Offset: 0x000E4698
		public IAPData[] IAPs
		{
			get
			{
				return this.configService.iapConfigDataList.iaps;
			}
		}

		// Token: 0x06003114 RID: 12564 RVA: 0x000E62AA File Offset: 0x000E46AA
		public bool IsOffer(IAPData data)
		{
			return this.configService.iapConfigDataList.content.CountIf(new Func<IAPContent, bool>(data.IsValidForContent)) > 1;
		}

		// Token: 0x06003115 RID: 12565 RVA: 0x000E62D0 File Offset: 0x000E46D0
		public IAPContent[] GetContents(IAPData data)
		{
			IAPContent[] array = Array.FindAll<IAPContent>(this.configService.iapConfigDataList.content, new Predicate<IAPContent>(data.IsValidForContent));
			foreach (IAPContent iapcontent in array)
			{
				foreach (string a in iapcontent.item_tag)
				{
					if (a == "piggy_open_items")
					{
						iapcontent.item_amount = this.gameStateService.Bank.NumberOfBankedDiamonds;
					}
					else if (a == "daily_deal_open_items")
					{
						List<IAPContent> list = new List<IAPContent>();
						DailyDealsConfig.Deal currentDeal = this.gameStateService.DailyDeals.CurrentDeal;
						list.Add(new IAPContent
						{
							item_resource = currentDeal.currency_type,
							item_amount = currentDeal.currency_amount
						});
						list.Add(new IAPContent
						{
							item_resource = currentDeal.bonus_1_type,
							item_amount = currentDeal.bonus_1_amount
						});
						if (!string.IsNullOrEmpty(currentDeal.bonus_2_type))
						{
							list.Add(new IAPContent
							{
								item_resource = currentDeal.bonus_2_type,
								item_amount = currentDeal.bonus_2_amount
							});
						}
						if (!string.IsNullOrEmpty(currentDeal.bonus_3_type))
						{
							list.Add(new IAPContent
							{
								item_resource = currentDeal.bonus_3_type,
								item_amount = currentDeal.bonus_3_amount
							});
						}
						array = list.ToArray();
						array = this.MergeIdenticalContents(array);
					}
					else if (a == "sale_open_items")
					{
						array = this.GetCurrentSaleIapContent();
						array = this.MergeIdenticalContents(array);
					}
				}
			}
			return array;
		}

		// Token: 0x06003116 RID: 12566 RVA: 0x000E64A0 File Offset: 0x000E48A0
		private IAPContent[] GetCurrentSaleIapContent()
		{
			string currentSaleConfigName = this.gameStateService.Sale.CurrentSaleConfigName;
			SaleConfig config = this.configService.SbsConfig.salesconfig.GetConfig(currentSaleConfigName);
			return config.AsIapContent;
		}

		// Token: 0x06003117 RID: 12567 RVA: 0x000E64DC File Offset: 0x000E48DC
		private IAPContent[] MergeIdenticalContents(IAPContent[] contents)
		{
			IAPContent[] array = contents;
			if (contents != null)
			{
				Dictionary<string, int> dictionary = new Dictionary<string, int>();
				foreach (IAPContent iapcontent in contents)
				{
					if (dictionary.ContainsKey(iapcontent.item_resource))
					{
						Dictionary<string, int> dictionary2;
						string item_resource;
						(dictionary2 = dictionary)[item_resource = iapcontent.item_resource] = dictionary2[item_resource] + iapcontent.item_amount;
					}
					else
					{
						dictionary[iapcontent.item_resource] = iapcontent.item_amount;
					}
				}
				array = new IAPContent[dictionary.Count];
				int num = 0;
				foreach (KeyValuePair<string, int> keyValuePair in dictionary)
				{
					array[num] = new IAPContent
					{
						item_resource = keyValuePair.Key,
						item_amount = keyValuePair.Value
					};
					num++;
				}
			}
			return array;
		}

		// Token: 0x06003118 RID: 12568 RVA: 0x000E65E4 File Offset: 0x000E49E4
		// public void OnIAPInitialized(IStoreController controller, IExtensionProvider extensions)
		// {
		// 	this.controller = controller;
		// 	this.extensions = extensions;
		// 	List<Product> list = controller.products.all.ToList<Product>();
		// 	bool flag = SbsEnvironment.CurrentEnvironment != SbsEnvironment.Environment.PRODUCTION;
		// 	IAPData[] iaps = this.IAPs;
		// 	for (int i = 0; i < iaps.Length; i++)
		// 	{
		// 		IAPData config = iaps[i];
		// 		config.storeProduct = list.Find((Product product) => product.definition.id == config.iap_name);
		// 		if (!config.storeProduct.availableToPurchase && flag && config.iap_name != "starter_m")
		// 		{
		// 			throw new Exception(string.Format("UNDEFINED BUNDLE ID: {0}", config.iap_name));
		// 		}
		// 	}
		// 	this.waitingForInit = false;
		// 	this.initalialized = true;
		// 	UnityEngine.Debug.Log("IAP Service Succeed");
		// 	WoogaDebug.Log(new object[]
		// 	{
		// 		"IAP Service init succeeded",
		// 		this.controller,
		// 		this.extensions
		// 	});
		// }

		// Token: 0x06003119 RID: 12569 RVA: 0x000E66F7 File Offset: 0x000E4AF7
		// public void OnIAPInitializeFailed(InitializationFailureReason error)
		// {
		// 	this.waitingForInit = false;
		// 	UnityEngine.Debug.Log("IAP Init Failed");
		// }

		// Token: 0x0600311A RID: 12570 RVA: 0x000E670C File Offset: 0x000E4B0C
		// public PurchaseProcessingResult ProcessIAPPurchase(PurchaseEventArgs e)
		// {
		// 	int diamonds = 0;
		// 	int coins = 0;
		// 	Dictionary<string, object> dictionary = new Dictionary<string, object>();
		// 	if (this.purchaseInfo.iapToBuy != null)
		// 	{
		// 		IAPContent[] contents = this.GetContents(this.purchaseInfo.iapToBuy);
		// 		int i = 0;
		// 		while (i < contents.Length)
		// 		{
		// 			IAPContent iapcontent = contents[i];
		// 			string item_resource = iapcontent.item_resource;
		// 			if (item_resource == null)
		// 			{
		// 				goto IL_B8;
		// 			}
		// 			if (!(item_resource == "diamonds"))
		// 			{
		// 				if (!(item_resource == "coins"))
		// 				{
		// 					goto IL_B8;
		// 				}
		// 				coins = iapcontent.item_amount;
		// 			}
		// 			else
		// 			{
		// 				diamonds = iapcontent.item_amount;
		// 				dictionary.Add("quantity", iapcontent.item_amount);
		// 			}
		// 			IL_DB:
		// 			i++;
		// 			continue;
		// 			IL_B8:
		// 			dictionary.Add(TrackingService.GetPurchaseResourceKey(iapcontent.item_resource), iapcontent.item_amount);
		// 			goto IL_DB;
		// 		}
		// 	}
		// 	CultureInfo provider = CultureInfo.CreateSpecificCulture("en-US");
		// 	dictionary.Add("price", e.purchasedProduct.metadata.localizedPrice.ToString(provider));
		// 	dictionary.Add("currency", e.purchasedProduct.metadata.isoCurrencyCode);
		// 	dictionary.Add("price_currency_code", e.purchasedProduct.metadata.isoCurrencyCode);
		// 	dictionary.Add("price_amount_micros", ((long)(e.purchasedProduct.metadata.localizedPrice * 1000000m)).ToString());
		// 	this.trackingService.AddPurchaseTracking(dictionary);
		// 	if (this.purchaseInfo != null && this.purchaseInfo.iapToBuy != null)
		// 	{
		// 		this.trackingService.TrackBuyFromShop(this.purchaseInfo.iapToBuy.iap_name, coins, diamonds, this.purchaseInfo.iapToBuy.context, this.GetContents(this.purchaseInfo.iapToBuy));
		// 	}
		// 	if (e.purchasedProduct != null)
		// 	{
		// 		if (e.purchasedProduct.hasReceipt)
		// 		{
		// 			Dictionary<string, object> dictionary2 = (Dictionary<string, object>)Json.Deserialize(e.purchasedProduct.receipt);
		// 			if (dictionary2 != null)
		// 			{
		// 				string json = (string)dictionary2["Payload"];
		// 				Dictionary<string, object> dictionary3 = (Dictionary<string, object>)Json.Deserialize(json);
		// 				string signature = (string)dictionary3["signature"];
		// 				string text = (string)dictionary3["json"];
		// 				Dictionary<string, object> dictionary4 = (Dictionary<string, object>)Json.Deserialize(text);
		// 				if (dictionary4.ContainsKey("packageName"))
		// 				{
		// 					dictionary.Add("package", dictionary4["packageName"]);
		// 				}
		// 				if (dictionary4.ContainsKey("productId"))
		// 				{
		// 					dictionary.Add("item", dictionary4["productId"]);
		// 				}
		// 				if (dictionary4.ContainsKey("orderId"))
		// 				{
		// 					string text2 = (string)dictionary4["orderId"];
		// 					text2 = text2.Replace("GPA.", string.Empty);
		// 					dictionary.Add("order_id", text2);
		// 				}
		// 				SBS.PaymentValidation.ValidateGooglePlayReceipt(text, signature, dictionary).ContinueWith(delegate(PaymentValidationResult result)
		// 				{
		// 					if (result == PaymentValidationResult.Success)
		// 					{
		// 						this.adjustService.TrackPurchase(e.purchasedProduct);
		// 					}
		// 				}).Catch(delegate(Exception exception)
		// 				{
		// 					WoogaDebug.Log(new object[]
		// 					{
		// 						"exception while validating"
		// 					});
		// 				}).Start();
		// 			}
		// 			else
		// 			{
		// 				WoogaDebug.LogWarning(new object[]
		// 				{
		// 					"UNABLE TO DESERIALIZE RECIEPT DATA"
		// 				});
		// 			}
		// 		}
		// 		else
		// 		{
		// 			WoogaDebug.LogWarning(new object[]
		// 			{
		// 				"NO RECIEPT ON PUCHASE"
		// 			});
		// 		}
		// 	}
		// 	this.purchaseInfo.result = e.purchasedProduct;
		// 	this.purchaseInfo.waiting = false;
		// 	this.purchaseInfo.success = true;
		// 	return PurchaseProcessingResult.Complete;
		// }

		// Token: 0x0600311B RID: 12571 RVA: 0x000E6AFC File Offset: 0x000E4EFC
		// public void OnIAPPurchaseFailed(Product i, PurchaseFailureReason p)
		// {
		// 	UnityEngine.Debug.Log("PURCHASE FAILED, REASON: " + p);
		// 	this.purchaseInfo.waiting = false;
		// 	this.purchaseInfo.success = false;
		// 	this.purchaseInfo.result = i;
		// 	this.purchaseInfo.failureReason = p;
		// }

		// Token: 0x040059BE RID: 22974
		[WaitForService(true, true)]
		private ConfigService configService;

		// Token: 0x040059BF RID: 22975
		// [WaitForService(true, true)]
		// private TrackingService trackingService;

		// Token: 0x040059C0 RID: 22976
		[WaitForService(true, true)]
		private ILocalizationService locaService;

		// Token: 0x040059C1 RID: 22977
		// [WaitForService(true, true)]
		// private IAdjustService adjustService;

		// Token: 0x040059C2 RID: 22978
		[WaitForService(true, true)]
		private GameStateService gameStateService;

		// // Token: 0x040059C3 RID: 22979
		// private IStoreController controller;
		//
		// // Token: 0x040059C4 RID: 22980
		// private IExtensionProvider extensions;

		// Token: 0x040059C5 RID: 22981
		private bool waitingForInit;

		// Token: 0x040059C6 RID: 22982
		private PurchaseResult purchaseInfo;

		// Token: 0x040059C7 RID: 22983
		private StoreListenerProxy proxyListener;
	}
}
