using UnityEngine;

namespace Match3.Scripts1.Helpshift
{
	// Token: 0x020001C5 RID: 453
	public class HelpshiftAndroidCampaignsDelegate : AndroidJavaProxy
	{
		// Token: 0x06000CF1 RID: 3313 RVA: 0x0001EA03 File Offset: 0x0001CE03
		public HelpshiftAndroidCampaignsDelegate(IHelpshiftCampaignsDelegate externalDelegate) : base("com.helpshift.campaigns.HelpshiftCampaignsDelegate")
		{
			this.externalCampaignsDelegate = externalDelegate;
		}

		// Token: 0x06000CF2 RID: 3314 RVA: 0x0001EA17 File Offset: 0x0001CE17
		public void didReceiveUnreadMessagesCount(int count)
		{
			this.externalCampaignsDelegate.didReceiveUnreadMessagesCount(count);
		}

		// Token: 0x06000CF3 RID: 3315 RVA: 0x0001EA25 File Offset: 0x0001CE25
		public void sessionBegan()
		{
			this.externalCampaignsDelegate.sessionBegan();
		}

		// Token: 0x06000CF4 RID: 3316 RVA: 0x0001EA32 File Offset: 0x0001CE32
		public void sessionEnded()
		{
			this.externalCampaignsDelegate.sessionEnded();
		}

		// Token: 0x04003F5B RID: 16219
		private IHelpshiftCampaignsDelegate externalCampaignsDelegate;
	}
}
