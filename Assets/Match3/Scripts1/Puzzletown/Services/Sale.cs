using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Match3.Scripts1.Puzzletown.Config;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x020007EF RID: 2031
	public class Sale
	{
		// Token: 0x0600324A RID: 12874 RVA: 0x000ECAA8 File Offset: 0x000EAEA8
		public Sale(EventConfigContainer eventConfig, SaleConfig saleConfig, IAPData iap)
		{
			this.id = eventConfig.id;
			this.endDateLocal = eventConfig.EndDateLocal;
			this.titleLocaKey = eventConfig.config.sale.titleLocaKey;
			this.iconName = saleConfig.icon_name;
			this.content = saleConfig.content;
			this.discount = saleConfig.discount;
			this.value = saleConfig.value;
			this.realPriceString = this.RealPriceStringFromIAP(iap);
			// this.discountedPriceString = ((iap == null) ? 1.23.ToString() : iap.storeProduct.metadata.localizedPriceString);
			this.discountedPriceString = 1.23.ToString();
			this.iap = iap;
		}

		// Token: 0x170007E8 RID: 2024
		// (get) Token: 0x0600324B RID: 12875 RVA: 0x000ECB65 File Offset: 0x000EAF65
		public bool HasPreBoosts
		{
			get
			{
				return this.content.Any((Materials c) => c.Any((MaterialAmount m) => m.type.StartsWith("boost_pre_")));
			}
		}

		// Token: 0x0600324C RID: 12876 RVA: 0x000ECB8F File Offset: 0x000EAF8F
		public DateTime GetLowDateTime(int seconds)
		{
			return this.endDateLocal - TimeSpan.FromSeconds((double)seconds);
		}

		// Token: 0x0600324D RID: 12877 RVA: 0x000ECBA4 File Offset: 0x000EAFA4
		private string RealPriceStringFromIAP(IAPData iap)
		{
			double num = 1.23;
			string currencyCode = "USD";
			if (iap != null)
			{
				// ProductMetadata metadata = iap.storeProduct.metadata;
				// num = (double)metadata.localizedPrice;
				// currencyCode = metadata.isoCurrencyCode;
			}
			double num2 = num / (1.0 - (double)this.discount / 100.0);
			CultureInfo cultureInfoFromCurrencyCode = WoogaSystemLanguageExtensions.GetCultureInfoFromCurrencyCode(currencyCode);
			return num2.ToString("C", cultureInfoFromCurrencyCode.NumberFormat);
		}

		// Token: 0x04005ABF RID: 23231
		private const double PLACEHOLER_PRICE = 1.23;

		// Token: 0x04005AC0 RID: 23232
		private const string PLACEHOLDER_CURRENCTY = "USD";

		// Token: 0x04005AC1 RID: 23233
		public readonly string id;

		// Token: 0x04005AC2 RID: 23234
		public readonly DateTime endDateLocal;

		// Token: 0x04005AC3 RID: 23235
		public readonly string titleLocaKey;

		// Token: 0x04005AC4 RID: 23236
		public readonly string iconName;

		// Token: 0x04005AC5 RID: 23237
		public readonly List<Materials> content;

		// Token: 0x04005AC6 RID: 23238
		public readonly float discount;

		// Token: 0x04005AC7 RID: 23239
		public readonly float value;

		// Token: 0x04005AC8 RID: 23240
		public readonly string realPriceString;

		// Token: 0x04005AC9 RID: 23241
		public readonly string discountedPriceString;

		// Token: 0x04005ACA RID: 23242
		public readonly IAPData iap;
	}
}
