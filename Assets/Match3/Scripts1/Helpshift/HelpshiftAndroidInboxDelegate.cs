using UnityEngine;

namespace Match3.Scripts1.Helpshift
{
	// Token: 0x020001C6 RID: 454
	public class HelpshiftAndroidInboxDelegate : AndroidJavaProxy
	{
		// Token: 0x06000CF5 RID: 3317 RVA: 0x0001EA3F File Offset: 0x0001CE3F
		public HelpshiftAndroidInboxDelegate(IHelpshiftInboxDelegate externalDelegate) : base("com.helpshift.campaigns.delegates.InboxMessageDelegate")
		{
			this.externalDelegate = externalDelegate;
		}

		// Token: 0x06000CF6 RID: 3318 RVA: 0x0001EA54 File Offset: 0x0001CE54
		public void inboxMessageAdded(AndroidJavaObject message)
		{
			HelpshiftAndroidInboxMessage message2 = HelpshiftAndroidInboxMessage.createInboxMessage(message);
			this.externalDelegate.InboxMessageAdded(message2);
		}

		// Token: 0x06000CF7 RID: 3319 RVA: 0x0001EA74 File Offset: 0x0001CE74
		public void iconImageDownloaded(string messageIdentifier)
		{
			this.externalDelegate.IconImageDownloaded(messageIdentifier);
		}

		// Token: 0x06000CF8 RID: 3320 RVA: 0x0001EA82 File Offset: 0x0001CE82
		public void coverImageDownloaded(string messageIdentifier)
		{
			this.externalDelegate.CoverImageDownloaded(messageIdentifier);
		}

		// Token: 0x06000CF9 RID: 3321 RVA: 0x0001EA90 File Offset: 0x0001CE90
		public void inboxMessageDeleted(string messageIdentifier)
		{
			this.externalDelegate.InboxMessageDeleted(messageIdentifier);
		}

		// Token: 0x06000CFA RID: 3322 RVA: 0x0001EA9E File Offset: 0x0001CE9E
		public void inboxMessageMarkedAsSeen(string messageIdentifier)
		{
			this.externalDelegate.InboxMessageMarkedAsSeen(messageIdentifier);
		}

		// Token: 0x06000CFB RID: 3323 RVA: 0x0001EAAC File Offset: 0x0001CEAC
		public void inboxMessageMarkedAsRead(string messageIdentifier)
		{
			this.externalDelegate.InboxMessageMarkedAsRead(messageIdentifier);
		}

		// Token: 0x04003F5C RID: 16220
		private IHelpshiftInboxDelegate externalDelegate;
	}
}
