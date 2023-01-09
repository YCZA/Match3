namespace Match3.Scripts1.Wooga.Notifications
{
	// Token: 0x02000429 RID: 1065
	public class NullNotificationService : INotificationService
	{
		// Token: 0x06001F4C RID: 8012 RVA: 0x000834D6 File Offset: 0x000818D6
		public void Schedule(NotificationSettings settings)
		{
			NotificationLogger.Log(LogLevel.Debug, string.Format("Should fire notification at {0} with body {1}", settings.FireDate, settings.Body));
		}

		// Token: 0x06001F4D RID: 8013 RVA: 0x000834FB File Offset: 0x000818FB
		public void CancelAll()
		{
		}

		// Token: 0x06001F4E RID: 8014 RVA: 0x000834FD File Offset: 0x000818FD
		public void ClearAll()
		{
		}

		// Token: 0x06001F4F RID: 8015 RVA: 0x000834FF File Offset: 0x000818FF
		public void Cancel(int id)
		{
		}

		// Token: 0x06001F50 RID: 8016 RVA: 0x00083501 File Offset: 0x00081901
		public bool CheckForLaunchFromNotification(out string userInfo)
		{
			userInfo = null;
			return false;
		}
	}
}
