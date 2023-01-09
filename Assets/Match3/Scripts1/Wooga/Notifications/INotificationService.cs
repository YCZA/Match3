namespace Match3.Scripts1.Wooga.Notifications
{
	// Token: 0x02000421 RID: 1057
	public interface INotificationService
	{
		// Token: 0x06001F11 RID: 7953
		void Schedule(NotificationSettings settings);

		// Token: 0x06001F12 RID: 7954
		void CancelAll();

		// Token: 0x06001F13 RID: 7955
		void ClearAll();

		// Token: 0x06001F14 RID: 7956
		void Cancel(int id);

		// Token: 0x06001F15 RID: 7957
		bool CheckForLaunchFromNotification(out string userInfo);
	}
}
