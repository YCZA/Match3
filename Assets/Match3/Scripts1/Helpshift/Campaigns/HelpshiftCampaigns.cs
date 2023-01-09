using System;
using System.Collections.Generic;

namespace Match3.Scripts1.Helpshift.Campaigns
{
	// Token: 0x020001D4 RID: 468
	public class HelpshiftCampaigns
	{
		// Token: 0x06000D87 RID: 3463 RVA: 0x0002027C File Offset: 0x0001E67C
		private HelpshiftCampaigns()
		{
		}

		// Token: 0x06000D88 RID: 3464 RVA: 0x00020284 File Offset: 0x0001E684
		public static HelpshiftCampaigns getInstance()
		{
			if (HelpshiftCampaigns.instance == null)
			{
				HelpshiftCampaigns.instance = new HelpshiftCampaigns();
				HelpshiftCampaigns.nativeSdk = new HelpshiftCampaignsAndroid();
			}
			return HelpshiftCampaigns.instance;
		}

		// Token: 0x06000D89 RID: 3465 RVA: 0x000202A9 File Offset: 0x0001E6A9
		public bool AddProperty(string key, int value)
		{
			return HelpshiftCampaigns.nativeSdk.AddProperty(key, value);
		}

		// Token: 0x06000D8A RID: 3466 RVA: 0x000202B7 File Offset: 0x0001E6B7
		public bool AddProperty(string key, long value)
		{
			return HelpshiftCampaigns.nativeSdk.AddProperty(key, value);
		}

		// Token: 0x06000D8B RID: 3467 RVA: 0x000202C5 File Offset: 0x0001E6C5
		public bool AddProperty(string key, string value)
		{
			return HelpshiftCampaigns.nativeSdk.AddProperty(key, value);
		}

		// Token: 0x06000D8C RID: 3468 RVA: 0x000202D3 File Offset: 0x0001E6D3
		public bool AddProperty(string key, bool value)
		{
			return HelpshiftCampaigns.nativeSdk.AddProperty(key, value);
		}

		// Token: 0x06000D8D RID: 3469 RVA: 0x000202E1 File Offset: 0x0001E6E1
		public bool AddProperty(string key, DateTime value)
		{
			return HelpshiftCampaigns.nativeSdk.AddProperty(key, value);
		}

		// Token: 0x06000D8E RID: 3470 RVA: 0x000202EF File Offset: 0x0001E6EF
		public string[] AddProperties(Dictionary<string, object> value)
		{
			return HelpshiftCampaigns.nativeSdk.AddProperties(value);
		}

		// Token: 0x06000D8F RID: 3471 RVA: 0x000202FC File Offset: 0x0001E6FC
		public void ShowInbox(Dictionary<string, object> configMap)
		{
			HelpshiftCampaigns.nativeSdk.ShowInbox(configMap);
		}

		// Token: 0x06000D90 RID: 3472 RVA: 0x00020309 File Offset: 0x0001E709
		public void ShowMessage(string messageIdentifier, Dictionary<string, object> configMap)
		{
			HelpshiftCampaigns.nativeSdk.ShowMessage(messageIdentifier, configMap);
		}

		// Token: 0x06000D91 RID: 3473 RVA: 0x00020317 File Offset: 0x0001E717
		[Obsolete("[Helpshift Warning]: THIS API IS DEPRECATED AND USING IT COULD CAUSE UNCERTAIN BEHAVIOUR. PLEASE USE THE VARIANT 'RequestUnreadMessagesCount' API instead.")]
		public int GetCountOfUnreadMessages()
		{
			return HelpshiftCampaigns.nativeSdk.GetCountOfUnreadMessages();
		}

		// Token: 0x06000D92 RID: 3474 RVA: 0x00020323 File Offset: 0x0001E723
		public void RequestUnreadMessagesCount()
		{
			HelpshiftCampaigns.nativeSdk.RequestUnreadMessagesCount();
		}

		// Token: 0x06000D93 RID: 3475 RVA: 0x0002032F File Offset: 0x0001E72F
		public void SetInboxMessagesDelegate(IHelpshiftInboxDelegate inboxDelegate)
		{
			HelpshiftCampaigns.nativeSdk.SetInboxMessagesDelegate(inboxDelegate);
		}

		// Token: 0x06000D94 RID: 3476 RVA: 0x0002033C File Offset: 0x0001E73C
		public void SetCampaignsDelegate(IHelpshiftCampaignsDelegate campaignsDelegate)
		{
			HelpshiftCampaigns.nativeSdk.SetCampaignsDelegate(campaignsDelegate);
		}

		// Token: 0x04003FA6 RID: 16294
		private static HelpshiftCampaigns instance;

		// Token: 0x04003FA7 RID: 16295
		private static HelpshiftCampaignsAndroid nativeSdk;
	}
}
