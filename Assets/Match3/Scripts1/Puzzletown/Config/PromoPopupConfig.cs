using System;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Config
{
	// Token: 0x02000498 RID: 1176
	[Serializable]
	public class PromoPopupConfig
	{
		// Token: 0x17000524 RID: 1316
		// (get) Token: 0x06002159 RID: 8537 RVA: 0x0008C2D0 File Offset: 0x0008A6D0
		public string AlphabetAssetBundleName
		{
			get
			{
				return string.Format("buildings_{0}", this.promo_name);
			}
		}

		// Token: 0x0600215A RID: 8538 RVA: 0x0008C2E2 File Offset: 0x0008A6E2
		public bool Active()
		{
			return DateTime.UtcNow > this.StartDate && DateTime.UtcNow < this.EndDate;
		}

		// Token: 0x17000525 RID: 1317
		// (get) Token: 0x0600215B RID: 8539 RVA: 0x0008C30C File Offset: 0x0008A70C
		private DateTime StartDate
		{
			get
			{
				if (this.startDate == default(DateTime))
				{
					this.startDate = Scripts1.DateTimeExtensions.SortableDateStringToDateTime(this.start_date);
				}
				return this.startDate;
			}
		}

		// Token: 0x17000526 RID: 1318
		// (get) Token: 0x0600215C RID: 8540 RVA: 0x0008C34C File Offset: 0x0008A74C
		private DateTime EndDate
		{
			get
			{
				if (this.endDate == default(DateTime))
				{
					this.endDate = Scripts1.DateTimeExtensions.SortableDateStringToDateTime(this.end_date);
				}
				return this.endDate;
			}
		}

		// Token: 0x04004C70 RID: 19568
		private const string ASSET_BUNDLE_PREFIX = "buildings_{0}";

		// Token: 0x04004C71 RID: 19569
		public string promo_name;

		// Token: 0x04004C72 RID: 19570
		public string title_key;

		// Token: 0x04004C73 RID: 19571
		public string speechbubble_key;

		// Token: 0x04004C74 RID: 19572
		[SerializeField]
		private string start_date;

		// Token: 0x04004C75 RID: 19573
		[SerializeField]
		private string end_date;

		// Token: 0x04004C76 RID: 19574
		private DateTime startDate;

		// Token: 0x04004C77 RID: 19575
		private DateTime endDate;
	}
}
