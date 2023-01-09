using System;
using System.Collections.Generic;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Config
{
	// Token: 0x0200047E RID: 1150
	[Serializable]
	public class ContentUnlockConfig
	{
		// Token: 0x1700051F RID: 1311
		// (get) Token: 0x06002127 RID: 8487 RVA: 0x0008BA5C File Offset: 0x00089E5C
		private Dictionary<string, ContentUnlockConfig.Entry> map
		{
			get
			{
				if (this._map == null)
				{
					this._map = new Dictionary<string, ContentUnlockConfig.Entry>();
					foreach (ContentUnlockConfig.Entry entry in this.unlocks)
					{
						this._map[entry.id] = entry;
					}
				}
				return this._map;
			}
		}

		// Token: 0x06002128 RID: 8488 RVA: 0x0008BAE0 File Offset: 0x00089EE0
		public DateTime? UnlockDateFor(string contentId)
		{
			if (this.map.ContainsKey(contentId))
			{
				return new DateTime?(this.map[contentId].EndDate);
			}
			return null;
		}

		// Token: 0x04004BD1 RID: 19409
		[SerializeField]
		private List<ContentUnlockConfig.Entry> unlocks = new List<ContentUnlockConfig.Entry>();

		// Token: 0x04004BD2 RID: 19410
		private Dictionary<string, ContentUnlockConfig.Entry> _map;

		// Token: 0x0200047F RID: 1151
		[Serializable]
		public class Entry
		{
			// Token: 0x17000520 RID: 1312
			// (get) Token: 0x0600212A RID: 8490 RVA: 0x0008BB28 File Offset: 0x00089F28
			public DateTime EndDate
			{
				get
				{
					if (this.unlockDate == default(DateTime))
					{
						this.unlockDate = new DateTime(Scripts1.DateTimeExtensions.SortableDateStringToDateTime(this.unlock_date).Ticks, DateTimeKind.Local);
					}
					return this.unlockDate;
				}
			}

			// Token: 0x04004BD3 RID: 19411
			public string id;

			// Token: 0x04004BD4 RID: 19412
			[SerializeField]
			private string unlock_date;

			// Token: 0x04004BD5 RID: 19413
			private DateTime unlockDate;
		}
	}
}
