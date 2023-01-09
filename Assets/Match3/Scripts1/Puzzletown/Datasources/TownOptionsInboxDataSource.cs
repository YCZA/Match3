using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Datasources
{
	// Token: 0x020009E5 RID: 2533
	public class TownOptionsInboxDataSource : ArrayDataSource<TownOptionsInboxView, TownOptionsInboxData>
	{
		// Token: 0x06003D2A RID: 15658 RVA: 0x00134AB0 File Offset: 0x00132EB0
		public override void Show(IEnumerable<TownOptionsInboxData> feed)
		{
			base.Show(feed);
			this.ButtonAcceptAll.SetActive(this.dataArray.Length > 0);
			this.ButtonAcceptAllDisabled.SetActive(this.dataArray.Length == 0);
			if (this.dataArray.Length > 0)
			{
				this.unreadMessagesCounter.gameObject.SetActive(true);
				this.unreadMessagesCounter.text = string.Format("{0}", this.dataArray.Length);
			}
			else
			{
				this.unreadMessagesCounter.gameObject.SetActive(false);
			}
		}

		// Token: 0x040065E4 RID: 26084
		public TMP_Text unreadMessagesCounter;

		// Token: 0x040065E5 RID: 26085
		public GameObject ButtonAcceptAll;

		// Token: 0x040065E6 RID: 26086
		public GameObject ButtonAcceptAllDisabled;
	}
}
