using System;
using UnityEngine;

namespace Match3.Scripts1.Wooga.Notifications
{
	// Token: 0x02000424 RID: 1060
	public class NotificationLogger : MonoBehaviour
	{
		// Token: 0x06001F1B RID: 7963 RVA: 0x000832BC File Offset: 0x000816BC
		static NotificationLogger()
		{
			bool flag = global::UnityEngine.Object.FindObjectOfType<NotificationLogger>() != null;
			if (flag)
			{
				return;
			}
			GameObject gameObject = new GameObject("WoogaNotificationLogReceiver");
			gameObject.AddComponent<NotificationLogger>();
			global::UnityEngine.Object.DontDestroyOnLoad(gameObject);
		}

		// Token: 0x170004BB RID: 1211
		// (get) Token: 0x06001F1D RID: 7965 RVA: 0x000832FC File Offset: 0x000816FC
		// (set) Token: 0x06001F1E RID: 7966 RVA: 0x00083303 File Offset: 0x00081703
		public static Action<LogLevel, string> LogHandler { get; set; }

		// Token: 0x170004BC RID: 1212
		// (get) Token: 0x06001F1F RID: 7967 RVA: 0x0008330B File Offset: 0x0008170B
		// (set) Token: 0x06001F20 RID: 7968 RVA: 0x00083312 File Offset: 0x00081712
		public static LogLevel LogLevel { get; set; }

		// Token: 0x06001F21 RID: 7969 RVA: 0x0008331A File Offset: 0x0008171A
		public static void Log(LogLevel level, string message)
		{
			if (NotificationLogger.LogHandler != null && level >= NotificationLogger.LogLevel)
			{
				NotificationLogger.LogHandler(level, message);
			}
		}

		// Token: 0x06001F22 RID: 7970 RVA: 0x0008333D File Offset: 0x0008173D
		private void OnReceiveNativeDebugLog(string message)
		{
			NotificationLogger.Log(LogLevel.Debug, message);
		}

		// Token: 0x06001F23 RID: 7971 RVA: 0x00083346 File Offset: 0x00081746
		private void OnReceiveNativeWarningLog(string message)
		{
			NotificationLogger.Log(LogLevel.Warning, message);
		}

		// Token: 0x06001F24 RID: 7972 RVA: 0x0008334F File Offset: 0x0008174F
		private void OnReceiveNativeErrorLog(string message)
		{
			NotificationLogger.Log(LogLevel.Error, message);
		}
	}
}
