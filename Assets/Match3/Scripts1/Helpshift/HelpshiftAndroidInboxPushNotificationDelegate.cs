using UnityEngine;

namespace Match3.Scripts1.Helpshift
{
	// Token: 0x020001C8 RID: 456
	public class HelpshiftAndroidInboxPushNotificationDelegate : AndroidJavaProxy
	{
		// Token: 0x06000D13 RID: 3347 RVA: 0x0001F011 File Offset: 0x0001D411
		public HelpshiftAndroidInboxPushNotificationDelegate(IHelpshiftInboxPushNotificationDelegate externalDelegate) : base("com.helpshift.campaigns.delegates.InboxPushNotificationDelegate")
		{
			this.externalDelegate = externalDelegate;
		}

		// Token: 0x06000D14 RID: 3348 RVA: 0x0001F025 File Offset: 0x0001D425
		public void onInboxMessagePushNotificationClicked(string messageIdentifier)
		{
			this.externalDelegate.OnInboxMessagePushNotificationClicked(messageIdentifier);
		}

		// Token: 0x04003F6A RID: 16234
		private IHelpshiftInboxPushNotificationDelegate externalDelegate;
	}
}
