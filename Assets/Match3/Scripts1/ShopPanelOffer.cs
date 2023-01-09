using System;
using Match3.Scripts2.Shop;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x020009C3 RID: 2499
namespace Match3.Scripts1
{
	public class ShopPanelOffer : BaseShopPanelItem
	{
		// Token: 0x06003C84 RID: 15492 RVA: 0x0012EA13 File Offset: 0x0012CE13
		private void OnEnable()
		{
			this.purchase.onClick.AddListener(new UnityAction(this.TryPurchase));
		}

		// Token: 0x06003C85 RID: 15493 RVA: 0x0012EA31 File Offset: 0x0012CE31
		private void OnDisable()
		{
			this.purchase.onClick.RemoveListener(new UnityAction(this.TryPurchase));
		}

		// Token: 0x06003C86 RID: 15494 RVA: 0x0012EA4F File Offset: 0x0012CE4F
		private void TryPurchase()
		{
			this._tryPurchase(this._iapData);
		}

		// Token: 0x06003C87 RID: 15495 RVA: 0x0012EA62 File Offset: 0x0012CE62
		public override void Init(ILocalizationService localization)
		{
			this.localizationService = localization;
		}

		// Token: 0x06003C88 RID: 15496 RVA: 0x0012EA6C File Offset: 0x0012CE6C
		public override void SetData(IAPService service, IAPData data, Action<IAPData> tryPurchase)
		{
			this._tryPurchase = tryPurchase;
			this._iapData = data;
			IAPContent[] contents = service.GetContents(data);
			if (contents.Length < 2)
			{
				WoogaDebug.LogError(new object[]
				{
					"Improperly configured IAP: " + data.iap_name
				});
				return;
			}
			string path = "icons/" + contents[0].item_image;
			if (this.leftSprite != null)
			{
				this.leftSprite.sprite = Resources.Load<Sprite>(path);
				this.leftSprite.SetNativeSize();
			}
			this.leftAmount.text = contents[0].item_amount.ToString();
			string path2 = "icons/" + contents[1].item_image;
			if (this.rightSprite != null)
			{
				this.rightSprite.sprite = Resources.Load<Sprite>(path2);
				this.rightSprite.SetNativeSize();
			}
			this.rightAmount.text = contents[1].item_amount.ToString();
			// if (data.storeProduct != null)
			// {
			// 	this.cost.text = data.storeProduct.metadata.localizedPriceString;
			// }
			if (this.title != null)
			{
				this.title.text = this.localizationService.GetText("shop.diamonds.title." + data.iap_name, new LocaParam[0]);
			}
			if (this.description != null)
			{
				this.description.text = this.localizationService.GetText("shop.diamonds.description." + data.iap_name, new LocaParam[0]);
			}
		}

		// Token: 0x04006530 RID: 25904
		public TextMeshProUGUI title;

		// Token: 0x04006531 RID: 25905
		public TextMeshProUGUI description;

		// Token: 0x04006532 RID: 25906
		public TextMeshProUGUI leftAmount;

		// Token: 0x04006533 RID: 25907
		public TextMeshProUGUI rightAmount;

		// Token: 0x04006534 RID: 25908
		public Image leftSprite;

		// Token: 0x04006535 RID: 25909
		public Image rightSprite;

		// Token: 0x04006536 RID: 25910
		public TextMeshProUGUI cost;

		// Token: 0x04006537 RID: 25911
		public Button purchase;

		// Token: 0x04006538 RID: 25912
		private IAPData _iapData;

		// Token: 0x04006539 RID: 25913
		private Action<IAPData> _tryPurchase;

		// Token: 0x0400653A RID: 25914
		private ILocalizationService localizationService;
	}
}
