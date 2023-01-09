using System;
using System.Collections.Generic;
using Match3.Scripts1.Puzzletown.Config;
using Wooga.Newtonsoft.Json;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x020007AD RID: 1965
	[Serializable]
	public class SeasonPrizeInfo
	{
		// Token: 0x0600301B RID: 12315 RVA: 0x000E1CE2 File Offset: 0x000E00E2
		public SeasonPrizeInfo(string name, int grandPrizeProgress, SeasonPricingInfo pricing, int segment = 0)
		{
			this.name = name;
			this.grandPrizeProgress = grandPrizeProgress;
			this.pricing = ((pricing == null) ? SeasonPricingInfo.CreateDefault() : pricing);
			this.segment = segment;
		}

		// Token: 0x1700077E RID: 1918
		// (get) Token: 0x0600301C RID: 12316 RVA: 0x000E1D17 File Offset: 0x000E0117
		public SeasonPricingInfo Pricing
		{
			get
			{
				if (this.pricing == null || !this.pricing.IsValid)
				{
					this.pricing = SeasonPricingInfo.CreateDefault();
				}
				return this.pricing;
			}
		}

		// Token: 0x1700077F RID: 1919
		// (get) Token: 0x0600301D RID: 12317 RVA: 0x000E1D45 File Offset: 0x000E0145
		public bool IsValid
		{
			get
			{
				return !string.IsNullOrEmpty(this.name);
			}
		}

		// Token: 0x17000780 RID: 1920
		// (get) Token: 0x0600301E RID: 12318 RVA: 0x000E1D55 File Offset: 0x000E0155
		public string PrimaryBundleName
		{
			get
			{
				return SeasonConfigHelpers.BundleNameForSeason(this.name);
			}
		}

		// Token: 0x17000781 RID: 1921
		// (get) Token: 0x0600301F RID: 12319 RVA: 0x000E1D64 File Offset: 0x000E0164
		public IEnumerable<string> BundleNames
		{
			get
			{
				yield return SeasonConfigHelpers.BundleNameForSeason(this.name);
				yield break;
			}
		}

		// Token: 0x04005931 RID: 22833
		public string name;

		// Token: 0x04005932 RID: 22834
		public int grandPrizeProgress;

		// Token: 0x04005933 RID: 22835
		public int segment;

		// Token: 0x04005934 RID: 22836
		public bool offerSeen;

		// Token: 0x04005935 RID: 22837
		[JsonProperty]
		[SerializeField]
		private SeasonPricingInfo pricing;
	}
}
