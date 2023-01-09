using System;
using System.Text;
using UnityEngine;

namespace Match3.Scripts1.Wooga.Notifications
{
	// Token: 0x02000420 RID: 1056
	public class AndroidNotificationService : INotificationService
	{
		// Token: 0x06001F07 RID: 7943 RVA: 0x00082FA4 File Offset: 0x000813A4
		public void Schedule(NotificationSettings settings)
		{
			//AndroidJavaObject androidJavaObject = AndroidNotificationService.CreateAndroidNotificationFromSettings(settings);
			//AndroidNotificationService.Plugin.Call("schedule", new object[]
			//{
			//	androidJavaObject
			//});
		}

		// Token: 0x06001F08 RID: 7944 RVA: 0x00082FD1 File Offset: 0x000813D1
		public void Cancel(int id)
		{
			//AndroidNotificationService.Plugin.Call("cancel", new object[]
			//{
				//id
			//});
		}

		// Token: 0x06001F09 RID: 7945 RVA: 0x00082FF1 File Offset: 0x000813F1
		public void CancelAll()
		{
			//AndroidNotificationService.Plugin.Call("cancelAll", new object[0]);
		}

		// Token: 0x06001F0A RID: 7946 RVA: 0x00083008 File Offset: 0x00081408
		public void Clear(int id)
		{
			//AndroidNotificationService.Plugin.Call("clear", new object[]
			//{
			//	id
			//});
		}

		// Token: 0x06001F0B RID: 7947 RVA: 0x00083028 File Offset: 0x00081428
		public void ClearAll()
		{
			//AndroidNotificationService.Plugin.Call("clearAll", new object[0]);
		}

		// Token: 0x06001F0C RID: 7948 RVA: 0x00083040 File Offset: 0x00081440
		public bool CheckForLaunchFromNotification(out string userInfo)
		{
			//bool flag = AndroidNotificationService.Plugin.Call<bool>("hasStoredUserInfo", new object[0]);
			//userInfo = ((!flag) ? null : AndroidNotificationService.Plugin.Call<string>("consumeStoredUserInfo", new object[0]));
			//return flag;
			userInfo = null;
			return false;
		}

		// Token: 0x06001F0D RID: 7949 RVA: 0x00083088 File Offset: 0x00081488
		private static AndroidJavaObject CreateAndroidNotificationFromSettings(NotificationSettings settings)
		{
			AndroidNotificationService.GroupAssertNotNull(new object[]
			{
				settings.FireDate,
				settings.AndroidSettings.Title,
				settings.Body,
				settings.AndroidSettings.SmallIcon
			});
			byte[] bytes = Encoding.UTF8.GetBytes(settings.AndroidSettings.Title);
			byte[] bytes2 = Encoding.UTF8.GetBytes(settings.Body);
			long num = (long)(settings.FireDate.ToUniversalTime() - AndroidNotificationService.UnixEpoch).TotalMilliseconds;
			//AndroidJavaObject androidJavaObject = new AndroidJavaObject("com.wooga.notifications.Notification", new object[]
			//{
			//	settings.Id,
			//	num,
			//	bytes,
			//	bytes2,
			//	settings.AndroidSettings.SmallIcon
			//});
			//AndroidNotificationService.SetIfNotNull(androidJavaObject, "sound", settings.Sound);
			//AndroidNotificationService.SetIfNotNull(androidJavaObject, "userInfo", settings.UserInfo);
			//AndroidNotificationService.SetIfNotNull(androidJavaObject, "bigIcon", settings.AndroidSettings.BigIcon);
			//AndroidNotificationService.SetIfNotNull(androidJavaObject, "vibratePattern", settings.AndroidSettings.VibratePattern);
			//AndroidNotificationService.SetIfNotNull(androidJavaObject, "tickerText", settings.AndroidSettings.TickerText);
			//if (settings.AndroidSettings.Number > 0)
			//{
			//	androidJavaObject.Set<int>("number", settings.AndroidSettings.Number);
			//}
			//return androidJavaObject;
			return null;
		}

		// Token: 0x06001F0E RID: 7950 RVA: 0x00083200 File Offset: 0x00081600
		private static void GroupAssertNotNull(params object[] values)
		{
			foreach (object obj in values)
			{
				if (obj is string)
				{
				}
			}
		}

		// Token: 0x06001F0F RID: 7951 RVA: 0x00083237 File Offset: 0x00081637
		private static void SetIfNotNull(AndroidJavaObject notification, string setter, string value)
		{
			if (!string.IsNullOrEmpty(value))
			{
				notification.Set<string>(setter, value);
			}
		}

		// Token: 0x04004AB8 RID: 19128
		private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		// Token: 0x04004AB9 RID: 19129
		//private static readonly AndroidJavaObject Plugin = new AndroidJavaObject("com.wooga.notifications.NotificationController", new object[0]);
	}
}
