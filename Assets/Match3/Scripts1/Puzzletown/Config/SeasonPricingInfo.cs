using System;

namespace Match3.Scripts1.Puzzletown.Config
{
	// Token: 0x020004A0 RID: 1184
	[Serializable]
	public class SeasonPricingInfo
	{
		// Token: 0x1700052B RID: 1323
		// (get) Token: 0x0600217C RID: 8572 RVA: 0x0008C8DA File Offset: 0x0008ACDA
		public bool IsValid
		{
			get
			{
				return this.priceFactors != null && this.priceFactors.Length > 0;
			}
		}

		// Token: 0x0600217D RID: 8573 RVA: 0x0008C8F8 File Offset: 0x0008ACF8
		public static SeasonPricingInfo CreateDefault()
		{
			return new SeasonPricingInfo
			{
				minPrice = 10,
				priceRounding = 10,
				grandPrizePrice = 1000,
				priceFactors = SeasonPricingInfo.defaultPriceFactors
			};
		}

		// Token: 0x04004C93 RID: 19603
		public int minPrice;

		// Token: 0x04004C94 RID: 19604
		public int priceRounding;

		// Token: 0x04004C95 RID: 19605
		public int grandPrizePrice;

		// Token: 0x04004C96 RID: 19606
		public float[] priceFactors;

		// Token: 0x04004C97 RID: 19607
		private const int defaultMinPrice = 10;

		// Token: 0x04004C98 RID: 19608
		private const int defaultPriceRounding = 10;

		// Token: 0x04004C99 RID: 19609
		private const int defaultGrandPrizePrice = 1000;

		// Token: 0x04004C9A RID: 19610
		private static readonly float[] defaultPriceFactors = new float[]
		{
			1f,
			0.8f,
			0.6f,
			0.4f,
			0.2f
		};
	}
}
