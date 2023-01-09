namespace Match3.Scripts1.Helpshift
{
	// Token: 0x020001E1 RID: 481
	public interface IHelpshiftInboxDelegate
	{
		// Token: 0x06000E17 RID: 3607
		void InboxMessageAdded(HelpshiftInboxMessage message);

		// Token: 0x06000E18 RID: 3608
		void IconImageDownloaded(string messageIdentifier);

		// Token: 0x06000E19 RID: 3609
		void CoverImageDownloaded(string messageIdentifier);

		// Token: 0x06000E1A RID: 3610
		void InboxMessageDeleted(string messageIdentifier);

		// Token: 0x06000E1B RID: 3611
		void InboxMessageMarkedAsSeen(string messageIdentifier);

		// Token: 0x06000E1C RID: 3612
		void InboxMessageMarkedAsRead(string messageIdentifier);
	}
}
