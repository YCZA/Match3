using System;
using System.Collections.Generic;
using AndroidTools.Tools;
using Match3.Scripts1.Puzzletown.UI;
using Match3.Scripts2.Android.DataStatistics;
using Match3.Scripts2.Shop;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI; // using Firebase.Analytics;

// Token: 0x020009C1 RID: 2497
namespace Match3.Scripts1
{
	public class ShopPanelBundle : BaseShopPanelItem
	{
		// Token: 0x06003C77 RID: 15479 RVA: 0x0012E486 File Offset: 0x0012C886
		private void OnEnable()
		{
			this.purchase.onClick.AddListener(new UnityAction(this.TryPurchase));
		}

		// Token: 0x06003C78 RID: 15480 RVA: 0x0012E4A4 File Offset: 0x0012C8A4
		private void OnDisable()
		{
			this.purchase.onClick.RemoveListener(new UnityAction(this.TryPurchase));
		}

		// Token: 0x06003C79 RID: 15481 RVA: 0x0012E4C2 File Offset: 0x0012C8C2
		private void TryPurchase()
		{
			this._tryPurchase(this._iapData);
		}

		// Token: 0x06003C7A RID: 15482 RVA: 0x0012E4D5 File Offset: 0x0012C8D5
		public override void Init(ILocalizationService localization)
		{
			this.localizationService = localization;
		}

		// Token: 0x06003C7B RID: 15483 RVA: 0x0012E4E0 File Offset: 0x0012C8E0
		public override void SetData(IAPService service, IAPData data, Action<IAPData> tryPurchase)
		{
			this._tryPurchase = tryPurchase;
			this._iapData = data;
			// buried point: 查看特惠礼包
			DataStatistics.Instance.TriggerViewItem(_iapData.iap_name);
		
			int num = 0;
			List<MaterialAmount> list = new List<MaterialAmount>(10);
			IAPContent[] contents = service.GetContents(data);
			int i = 0;
			while (i < contents.Length)
			{
				IAPContent iapcontent = contents[i];
				string item_resource = iapcontent.item_resource;
				if (item_resource == null)
				{
					goto IL_AD;
				}
				if (!(item_resource == "coins"))
				{
					if (!(item_resource == "diamonds"))
					{
						goto IL_AD;
					}
					this.diamondsAmount.text = iapcontent.item_amount.ToString();
					num |= 2;
				}
				else
				{
					this.coinsAmount.text = iapcontent.item_amount.ToString();
					num |= 1;
				}
				IL_BE:
				i++;
				continue;
				IL_AD:
				list.Add(iapcontent.materialAmount);
				goto IL_BE;
			}
			if (num < 3)
			{
				WoogaDebug.LogError(new object[]
				{
					"Improperly configured IAP: " + data.iap_name
				});
				return;
			}
			this.boosters.Show(this.SplitBoosters(list));
			// if (data.storeProduct != null)
			// {
			// 	this.cost.text = data.storeProduct.metadata.localizedPriceString;
			// }
			var productInfo = AndroidPay.GetProductByNumId(data.id);
			this.cost.text = productInfo != null ? productInfo.price : "???";
			if (this.title != null)
			{
				this.title.text = this.localizationService.GetText("ui.diamondshop.newoffers.title." + data.iap_name, new LocaParam[0]);
			}
			if (this.description != null)
			{
				this.description.text = this.localizationService.GetText("shop.diamonds.description." + data.iap_name, new LocaParam[0]);
			}
		}

		// Token: 0x06003C7C RID: 15484 RVA: 0x0012E694 File Offset: 0x0012CA94
		private IEnumerable<IEnumerable<MaterialAmount>> SplitBoosters(List<MaterialAmount> boosters)
		{
			int i = 0;
			int j = 0;
			while (i < boosters.Count)
			{
				yield return boosters.GetRange(i, Mathf.Min(this.GetRowSize(j), boosters.Count - i));
				i += this.GetRowSize(j);
				j++;
			}
			yield break;
		}

		// Token: 0x06003C7D RID: 15485 RVA: 0x0012E6BE File Offset: 0x0012CABE
		private int GetRowSize(int i)
		{
			if (this.boostersRowSizes.IsNullOrEmptyCollection())
			{
				return 1;
			}
			return this.boostersRowSizes[Mathf.Min(i, this.boostersRowSizes.Length - 1)];
		}

		// Token: 0x0400651A RID: 25882
		public TextMeshProUGUI title;

		// Token: 0x0400651B RID: 25883
		public TextMeshProUGUI description;

		// Token: 0x0400651C RID: 25884
		public TextMeshProUGUI coinsAmount;

		// Token: 0x0400651D RID: 25885
		public TextMeshProUGUI diamondsAmount;

		// Token: 0x0400651E RID: 25886
		public MaterialGroupsDataSource boosters;

		// Token: 0x0400651F RID: 25887
		public TextMeshProUGUI cost;

		// Token: 0x04006520 RID: 25888
		public Button purchase;

		// Token: 0x04006521 RID: 25889
		private IAPData _iapData;

		// Token: 0x04006522 RID: 25890
		private Action<IAPData> _tryPurchase;

		// Token: 0x04006523 RID: 25891
		private ILocalizationService localizationService;

		// Token: 0x04006524 RID: 25892
		public int[] boostersRowSizes;
	}
}
