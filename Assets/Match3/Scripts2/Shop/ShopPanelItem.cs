using System;
using AndroidTools.Tools;
using Match3.Scripts1;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020009C2 RID: 2498
namespace Match3.Scripts2.Shop
{
	public class ShopPanelItem : BaseShopPanelItem
	{
		// Token: 0x06003C7F RID: 15487 RVA: 0x0012E84C File Offset: 0x0012CC4C
		private void Start()
		{
			this.purchase.onClick.AddListener(delegate()
			{
				// eli key point 点击购买按钮
				Debug.Log("onclick_purchase_btn, price: " + _iapData.price);
// #if AAK
// 			AntiAddictionCtrl.Instance.CheckPayLimit((int)(_iapData.price * 100), (is_success, msg) =>
// 			{
// 				if (is_success)
// 				{
// #endif
				this._tryPurchase(this._iapData);
// #if AAK
// 				}
// 			});
// #endif
			});
		}

		// Token: 0x06003C80 RID: 15488 RVA: 0x0012E86A File Offset: 0x0012CC6A
		public override void Init(ILocalizationService localization)
		{
		}

		// Token: 0x06003C81 RID: 15489 RVA: 0x0012E86C File Offset: 0x0012CC6C
		public override void SetData(IAPService service, IAPData data, Action<IAPData> tryPurchase)
		{
			this._tryPurchase = tryPurchase;
			this._iapData = data;
			// eli key point 设置商品价格(购买)
			// this.cost.text = data.storeProduct.metadata.localizedPriceString;
			var productInfo = AndroidPay.GetProductByNumId(data.id);
			this.cost.text = productInfo != null ? productInfo.price : "???";
		
			IAPContent[] contents = service.GetContents(data);
			if (contents.Length == 0)
			{
				WoogaDebug.LogError(new object[]
				{
					"Missing IAP item resource value"
				});
				return;
			}
			if (this.image)
			{
				string path = "icons/" + contents[0].item_image;
				Sprite sprite = Resources.Load<Sprite>(path);
				this.image.sprite = sprite;
				this.image.SetNativeSize();
			}
			if (!string.IsNullOrEmpty(data.item_icon))
			{
				// 不显示商品角标："最畅销"”最划算“
				// 如ui_macaron_mostpopular
				// string spriteName = "ui_macaron_" + this._iapData.item_icon;
				// bool flag = spriteName.Contains("mostpopular");
				// // Debug.Log("查找图片：" + spriteName);
				// Sprite sprite2 = this.itemIcons.FirstOrDefault((Sprite icon) => icon.name == spriteName);
				// this.item_icon.sprite = sprite2;
				// this.item_icon.SetNativeSize();
				// this.item_icon.gameObject.SetActive(true);
				// this.mostPopular.SetActive(flag);
				// this.bestValue.SetActive(!flag);
				// // 审核版隐藏
				// #if REVIEW_VERSION
				// item_icon.gameObject.SetActive(false);
				// #endif
			}
			else
			{
				this.item_icon.gameObject.SetActive(false);
			}
			this.amount.text = contents[0].item_amount.ToString();
		}

		// Token: 0x04006525 RID: 25893
		private const string MOST_POPULAR = "mostpopular";

		// Token: 0x04006526 RID: 25894
		public TextMeshProUGUI amount;

		// Token: 0x04006527 RID: 25895
		public TextMeshProUGUI cost;

		// Token: 0x04006528 RID: 25896
		public Image image;

		// Token: 0x04006529 RID: 25897
		public Image item_icon;

		// Token: 0x0400652A RID: 25898
		public GameObject bestValue;

		// Token: 0x0400652B RID: 25899
		public GameObject mostPopular;

		// Token: 0x0400652C RID: 25900
		public Button purchase;

		// Token: 0x0400652D RID: 25901
		public Sprite[] itemIcons;

		// Token: 0x0400652E RID: 25902
		private IAPData _iapData;

		// Token: 0x0400652F RID: 25903
		private Action<IAPData> _tryPurchase;
	}
}
