using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Match3.Scripts1.Puzzletown.Config
{
	// Token: 0x020004A1 RID: 1185
	[Serializable]
	public class SeasonConfig : EventConfig
	{
		// Token: 0x06002180 RID: 8576 RVA: 0x0008C952 File Offset: 0x0008AD52
		public SeasonConfig.SegmentsConfig GetSegment(int segment)
		{
			return (segment < this.segment_configs.Count) ? this.segment_configs[segment] : this.segment_configs[1];
		}

		// Token: 0x1700052C RID: 1324
		// (get) Token: 0x06002181 RID: 8577 RVA: 0x0008C984 File Offset: 0x0008AD84
		public bool IsValid
		{
			get
			{
				return this.sets != null && this.sets.Length > 0 && this.segment_configs != null && this.segment_configs.Count > 1 && this.pricing != null && this.pricing.IsValid;
			}
		}

		// Token: 0x06002182 RID: 8578 RVA: 0x0008C9DF File Offset: 0x0008ADDF
		public SeasonConfig Fixup(EventConfigContainer container)
		{
			this.start = container.start;
			this.end = container.end;
			this.id = container.id;
			return this;
		}

		// Token: 0x1700052D RID: 1325
		// (get) Token: 0x06002183 RID: 8579 RVA: 0x0008CA06 File Offset: 0x0008AE06
		public DateTime EndDate
		{
			get
			{
				return Scripts1.DateTimeExtensions.FromUnixTimeStamp(this.end, DateTimeKind.Utc);
			}
		}

		// Token: 0x1700052E RID: 1326
		// (get) Token: 0x06002184 RID: 8580 RVA: 0x0008CA14 File Offset: 0x0008AE14
		public DateTime StartDate
		{
			get
			{
				return Scripts1.DateTimeExtensions.FromUnixTimeStamp(this.start, DateTimeKind.Utc);
			}
		}

		// Token: 0x06002185 RID: 8581 RVA: 0x0008CA22 File Offset: 0x0008AE22
		public string GetPrimary()
		{
			return (this.sets == null || this.sets.Length <= 0) ? string.Empty : this.sets[0];
		}

		// Token: 0x1700052F RID: 1327
		// (get) Token: 0x06002186 RID: 8582 RVA: 0x0008CA4F File Offset: 0x0008AE4F
		public bool IsSeasonActive
		{
			get
			{
				return base.IsOngoing(DateTime.UtcNow.ToUnixTimeStamp());
			}
		}

		// Token: 0x17000530 RID: 1328
		// (get) Token: 0x06002187 RID: 8583 RVA: 0x0008CA61 File Offset: 0x0008AE61
		public string Primary
		{
			get
			{
				return this.GetPrimary();
			}
		}

		// Token: 0x17000531 RID: 1329
		// (get) Token: 0x06002188 RID: 8584 RVA: 0x0008CA69 File Offset: 0x0008AE69
		public string[] SetNames
		{
			get
			{
				return this.sets;
			}
		}

		// Token: 0x17000532 RID: 1330
		// (get) Token: 0x06002189 RID: 8585 RVA: 0x0008CA71 File Offset: 0x0008AE71
		public string PrimaryBundleName
		{
			get
			{
				return SeasonConfigHelpers.BundleNameForSeason(this.Primary);
			}
		}

		// Token: 0x17000533 RID: 1331
		// (get) Token: 0x0600218A RID: 8586 RVA: 0x0008CA7E File Offset: 0x0008AE7E
		public IEnumerable<string> BundleNames
		{
			get
			{
				IEnumerable<string> setNames = this.SetNames;
				if (SeasonConfig._003C_003Ef__mg_0024cache0 == null)
				{
					SeasonConfig._003C_003Ef__mg_0024cache0 = new Func<string, string>(SeasonConfigHelpers.BundleNameForSeason);
				}
				return setNames.Select(SeasonConfig._003C_003Ef__mg_0024cache0);
			}
		}

		// Token: 0x17000534 RID: 1332
		// (get) Token: 0x0600218B RID: 8587 RVA: 0x0008CAA8 File Offset: 0x0008AEA8
		public string FxTexturePath
		{
			get
			{
				return SeasonConfigHelpers.FxTextureForSeason(this.Primary);
			}
		}

		// Token: 0x17000535 RID: 1333
		// (get) Token: 0x0600218C RID: 8588 RVA: 0x0008CAB5 File Offset: 0x0008AEB5
		public string CurrentIconName
		{
			get
			{
				return SeasonConfigHelpers.IconNameForSeason(this.Primary);
			}
		}

		// Token: 0x17000536 RID: 1334
		// (get) Token: 0x0600218D RID: 8589 RVA: 0x0008CAC2 File Offset: 0x0008AEC2
		public string TMProIconName
		{
			get
			{
				return SeasonConfigHelpers.TMProIconName(this.Primary);
			}
		}

		// Token: 0x17000537 RID: 1335
		// (get) Token: 0x0600218E RID: 8590 RVA: 0x0008CACF File Offset: 0x0008AECF
		public string CurrentPromoIllustrationName
		{
			get
			{
				return SeasonConfigHelpers.PromoIllustrationForSeason(this.Primary);
			}
		}

		// Token: 0x04004C9B RID: 19611
		public string[] sets;

		// Token: 0x04004C9C RID: 19612
		public int promo_cooldown_hours;

		// Token: 0x04004C9D RID: 19613
		public string title_loca_key;

		// Token: 0x04004C9E RID: 19614
		public SeasonPricingInfo pricing;

		// Token: 0x04004C9F RID: 19615
		public SeasonPricingInfo pricing_v3;

		// Token: 0x04004CA0 RID: 19616
		public float seasonal_diamond_conversion;

		// Token: 0x04004CA1 RID: 19617
		public List<SeasonConfig.SegmentsConfig> segment_configs;

		// Token: 0x04004CA2 RID: 19618
		[CompilerGenerated]
		private static Func<string, string> _003C_003Ef__mg_0024cache0;

		// Token: 0x020004A2 RID: 1186
		[Serializable]
		public class SegmentsConfig
		{
			// Token: 0x04004CA3 RID: 19619
			public List<int> tier_rewards;
		}
	}
}
