using UnityEngine;

namespace Match3.Scripts1.Wooga.Notifications
{
	// Token: 0x02000425 RID: 1061
	public static class NotificationService
	{
		// Token: 0x06001F26 RID: 7974 RVA: 0x00083373 File Offset: 0x00081773
		public static void Schedule(NotificationSettings settings)
		{
			NotificationService.DefaultNotificationSettings.ApplyTo(ref settings);
			NotificationService.InternalNotificationService.Schedule(settings);
		}

		// Token: 0x06001F27 RID: 7975 RVA: 0x0008338C File Offset: 0x0008178C
		public static void CancelAll()
		{
			NotificationService.InternalNotificationService.CancelAll();
		}

		// Token: 0x06001F28 RID: 7976 RVA: 0x00083398 File Offset: 0x00081798
		public static void ClearAll()
		{
			NotificationService.InternalNotificationService.ClearAll();
		}

		// Token: 0x06001F29 RID: 7977 RVA: 0x000833A4 File Offset: 0x000817A4
		public static void Cancel(int id)
		{
			NotificationService.InternalNotificationService.Cancel(id);
		}

		// Token: 0x06001F2A RID: 7978 RVA: 0x000833B1 File Offset: 0x000817B1
		public static bool CheckForLaunchFromNotification(out string userInfo)
		{
			return NotificationService.InternalNotificationService.CheckForLaunchFromNotification(out userInfo);
		}

		// Token: 0x04004AC1 RID: 19137
		private const string SettingsResourcePath = "DefaultNotificationSettings";

		// Token: 0x04004AC2 RID: 19138
		private static readonly INotificationService InternalNotificationService = new AndroidNotificationService();

		// Token: 0x04004AC3 RID: 19139
		private static readonly DefaultNotificationSettings DefaultNotificationSettings = Resources.Load<DefaultNotificationSettings>("DefaultNotificationSettings");
	}
}
