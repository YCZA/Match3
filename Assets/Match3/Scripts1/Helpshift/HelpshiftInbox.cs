using System.Collections.Generic;

namespace Match3.Scripts1.Helpshift
{
	// Token: 0x020001D6 RID: 470
	public class HelpshiftInbox
	{
		// Token: 0x06000DD7 RID: 3543 RVA: 0x00020B6D File Offset: 0x0001EF6D
		private HelpshiftInbox()
		{
		}

		// Token: 0x06000DD8 RID: 3544 RVA: 0x00020B75 File Offset: 0x0001EF75
		public static HelpshiftInbox getInstance()
		{
			if (HelpshiftInbox.instance == null)
			{
				HelpshiftInbox.instance = new HelpshiftInbox();
				HelpshiftInbox.nativeSdk = new HelpshiftInboxAndroid();
			}
			return HelpshiftInbox.instance;
		}

		// Token: 0x06000DD9 RID: 3545 RVA: 0x00020B9A File Offset: 0x0001EF9A
		public List<HelpshiftInboxMessage> GetAllInboxMessages()
		{
			return HelpshiftInbox.nativeSdk.GetAllInboxMessages();
		}

		// Token: 0x06000DDA RID: 3546 RVA: 0x00020BA6 File Offset: 0x0001EFA6
		public HelpshiftInboxMessage GetInboxMessage(string messageIdentifier)
		{
			return HelpshiftInbox.nativeSdk.GetInboxMessage(messageIdentifier);
		}

		// Token: 0x06000DDB RID: 3547 RVA: 0x00020BB3 File Offset: 0x0001EFB3
		public void MarkInboxMessageAsRead(string messageIdentifier)
		{
			HelpshiftInbox.nativeSdk.MarkInboxMessageAsRead(messageIdentifier);
		}

		// Token: 0x06000DDC RID: 3548 RVA: 0x00020BC0 File Offset: 0x0001EFC0
		public void MarkInboxMessageAsSeen(string messageIdentifier)
		{
			HelpshiftInbox.nativeSdk.MarkInboxMessageAsSeen(messageIdentifier);
		}

		// Token: 0x06000DDD RID: 3549 RVA: 0x00020BCD File Offset: 0x0001EFCD
		public void DeleteInboxMessage(string messageIdentifier)
		{
			HelpshiftInbox.nativeSdk.DeleteInboxMessage(messageIdentifier);
		}

		// Token: 0x06000DDE RID: 3550 RVA: 0x00020BDA File Offset: 0x0001EFDA
		public void SetInboxMessageDelegate(IHelpshiftInboxDelegate externalDelegate)
		{
			HelpshiftInbox.nativeSdk.SetInboxMessageDelegate(externalDelegate);
		}

		// Token: 0x06000DDF RID: 3551 RVA: 0x00020BE7 File Offset: 0x0001EFE7
		public void SetInboxPushNotificationDelegate(IHelpshiftInboxPushNotificationDelegate externalDelegate)
		{
			HelpshiftInbox.nativeSdk.SetInboxPushNotificationDelegate(externalDelegate);
		}

		// Token: 0x04003FCA RID: 16330
		private static HelpshiftInbox instance;

		// Token: 0x04003FCB RID: 16331
		private static HelpshiftInboxAndroid nativeSdk;
	}
}
