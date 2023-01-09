using System;
using System.Linq;

// Token: 0x020007C4 RID: 1988
namespace Match3.Scripts1
{
	[Serializable]
	public class IAPData
	{
		// Token: 0x170007B4 RID: 1972
		// (get) Token: 0x060030FE RID: 12542 RVA: 0x000E6003 File Offset: 0x000E4403
		public bool IsAvailable
		{
			get
			{
				// eli key point iap is available
				// return this.is_visible > 0 && this.storeProduct != null && this.storeProduct.availableToPurchase;
				return this.is_visible > 0;
			}
		}

		// Token: 0x060030FF RID: 12543 RVA: 0x000E602A File Offset: 0x000E442A
		public static bool IsOfferName(string iapName)
		{
			return iapName.StartsWith("offer_");
		}

		// Token: 0x170007B5 RID: 1973
		// (get) Token: 0x06003100 RID: 12544 RVA: 0x000E6037 File Offset: 0x000E4437
		public bool IsSale
		{
			get
			{
				return this.item_tag.Contains("sale_open_items");
			}
		}

		// Token: 0x170007B6 RID: 1974
		// (get) Token: 0x06003101 RID: 12545 RVA: 0x000E6049 File Offset: 0x000E4449
		public bool IsOffer
		{
			get
			{
				return IAPData.IsOfferName(this.iap_name);
			}
		}

		// Token: 0x06003102 RID: 12546 RVA: 0x000E6056 File Offset: 0x000E4456
		public static bool IsDailyDealName(string iapName)
		{
			return iapName.StartsWith("tc_dd");
		}

		// Token: 0x06003103 RID: 12547 RVA: 0x000E6063 File Offset: 0x000E4463
		public static bool IsMoreMovesPackName(string iapName)
		{
			return iapName.StartsWith("offer_post_moves");
		}

		// Token: 0x170007B7 RID: 1975
		// (get) Token: 0x06003104 RID: 12548 RVA: 0x000E6070 File Offset: 0x000E4470
		public bool IsDailyDeal
		{
			get
			{
				return IAPData.IsDailyDealName(this.iap_name);
			}
		}

		// Token: 0x06003105 RID: 12549 RVA: 0x000E607D File Offset: 0x000E447D
		public bool IsValidForContent(IAPContent content)
		{
			return !this.item_tag.Intersect(content.item_tag).IsNullOrEmptyEnumerable();
		}

		// Token: 0x06003106 RID: 12550 RVA: 0x000E6098 File Offset: 0x000E4498
		public override string ToString()
		{
			return string.Format("IapName: {0}, Price: {1}", this.iap_name, this.price);
		}

		// Token: 0x040059A7 RID: 22951
		public int id;
		public string iap_name;

		// Token: 0x040059A8 RID: 22952
		public string ios_id;

		// Token: 0x040059A9 RID: 22953
		public string google_play_id;

		// Token: 0x040059AA RID: 22954
		public int price;

		// Token: 0x040059AB RID: 22955
		public string item_icon;

		// Token: 0x040059AC RID: 22956
		public int purchase_cool_down;

		// Token: 0x040059AD RID: 22957
		public int is_visible;

		// Token: 0x040059AE RID: 22958
		public string context = "shop";

		// Token: 0x040059AF RID: 22959
		public string[] item_tag;

		// Token: 0x040059B0 RID: 22960
		public string localization_key;

		// Token: 0x040059B1 RID: 22961
		public string iap_type;

		// Token: 0x040059B2 RID: 22962
		// public Product storeProduct;
	}
}
