using System;
using System.Collections;
using System.Linq;
using Match3.Scripts1.Puzzletown.Services;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.Signals;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;
using Match3.Scripts2.Shop;
using UnityEngine;

// Token: 0x020007C9 RID: 1993
namespace Match3.Scripts1
{
	public class OffersService : AService
	{
		// Token: 0x0600311D RID: 12573 RVA: 0x000E6EF4 File Offset: 0x000E52F4
		public OffersService(ConfigService configs, GameStateService gameState, IAPService iapService)
		{
			this.configs = configs;
			this.gameState = gameState;
			this.iapService = iapService;
			this.iapData = configs.iapConfigDataList;
			this.UpdateCurrentOffer(false);
			base.OnInitialized.Dispatch();
		}

		// Token: 0x170007BB RID: 1979
		// (get) Token: 0x0600311E RID: 12574 RVA: 0x000E6F45 File Offset: 0x000E5345
		// (set) Token: 0x0600311F RID: 12575 RVA: 0x000E6F4D File Offset: 0x000E534D
		public IAPData CurrentOffer { get; private set; }

		// Token: 0x06003120 RID: 12576 RVA: 0x000E6F56 File Offset: 0x000E5356
		public void SkipCooldown()
		{
			this.UpdateCurrentOffer(true);
		}

		// Token: 0x06003121 RID: 12577 RVA: 0x000E6F5F File Offset: 0x000E535F
		public void UpdateCurrentOffer(bool skipCooldown = false)
		{
			if (this.updateRoutine == null)
			{
				this.updateRoutine = WooroutineRunner.StartCoroutine(this.UpdateCurrentOfferRoutine(skipCooldown), null);
			}
		}

		// Token: 0x06003122 RID: 12578 RVA: 0x000E6F80 File Offset: 0x000E5380
		private IEnumerator UpdateCurrentOfferRoutine(bool skipCooldown)
		{
			Debug.Log("start set offer");
			while (!this.iapService.initalialized)
			{
				yield return null;
			}
			int latest = this.gameState.Transactions.GetLatestOfferTransactionTime();
			int timeSince = DateTime.UtcNow.ToUnixTimeStamp() - latest; // 这个用的是本地时间
			int cooldown = this.configs.general.balance.offer_cooldown_hours * 60 * 60;
			if (skipCooldown)
			{
				cooldown = 0;
			}
			IAPData offer = null;
			if (timeSince >= cooldown)
			{
				Transaction highestTransaction = this.GetHighestTransaction();
				int price = (highestTransaction == null) ? 0 : this.GetPrice(highestTransaction);
				offer = this.iapData.iaps.FirstOrDefault((IAPData iap) => iap.IsAvailable && iap.price > price && iap.IsOffer);
				// offer循环
				// if (offer == null)
				// {
				// 	offer = iapData.iaps.First();
				// }
			}
			Debug.Log("set offer: " + offer);
			this.CurrentOffer = offer;
			this.onCurrentOfferChanged.Dispatch();
			this.updateRoutine = null;
			yield break;
		}

		// Token: 0x06003123 RID: 12579 RVA: 0x000E6FA4 File Offset: 0x000E53A4
		private Transaction GetHighestTransaction()
		{
			int num = 0;
			int num2 = 0;
			TransactionDataService transactions = this.gameState.Transactions;
			for (int i = 0; i < transactions.Data.Count; i++)
			{
				int price = this.GetPrice(transactions.Data[i]);
				if (price > num)
				{
					num = price;
					num2 = i;
				}
			}
			return (num2 >= transactions.Data.Count) ? null : transactions.Data[num2];
		}

		// Token: 0x06003124 RID: 12580 RVA: 0x000E7024 File Offset: 0x000E5424
		private int GetPrice(Transaction transaction)
		{
			IAPData iapdata = this.iapData.iaps.FirstOrDefault((IAPData d) => d.iap_name == transaction.itemId);
			return (iapdata == null) ? 0 : iapdata.price;
		}

		// Token: 0x040059CA RID: 22986
		public const string OFFER = "offer";

		// Token: 0x040059CB RID: 22987
		public readonly Signal onCurrentOfferChanged = new Signal();

		// Token: 0x040059CC RID: 22988
		private GameStateService gameState;

		// Token: 0x040059CD RID: 22989
		private ConfigService configs;

		// Token: 0x040059CE RID: 22990
		private IAPService iapService;

		// Token: 0x040059CF RID: 22991
		private IAPConfigDataList iapData;

		// Token: 0x040059D1 RID: 22993
		private Coroutine updateRoutine;
	}
}
