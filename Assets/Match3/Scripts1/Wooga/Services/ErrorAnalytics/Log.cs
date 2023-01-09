using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Match3.Scripts1.Wooga.Services.ErrorAnalytics.Internal;

namespace Match3.Scripts1.Wooga.Services.ErrorAnalytics
{
	// Token: 0x020003FD RID: 1021
	public static class Log
	{
		// Token: 0x06001E76 RID: 7798 RVA: 0x00081114 File Offset: 0x0007F514
		private static void NotifyError(ErrorAnalytics.LogSeverity severity, Exception exception, string message, Dictionary<string, object> metaData)
		{
			string stackTrace;
			List<ParsingUtility.StackTraceElement> stackTrace2;
			if (string.IsNullOrEmpty(exception.StackTrace))
			{
				stackTrace = Environment.StackTrace;
				stackTrace2 = ParsingUtility.ParseStackTraceElements(stackTrace, ParsingUtility.ParseType.EnvironmentStackTrace);
			}
			else
			{
				stackTrace = exception.StackTrace;
				stackTrace2 = ParsingUtility.ParseStackTraceElements(stackTrace, ParsingUtility.ParseType.UnhandledExceptionStackTrace);
			}
			message = (message ?? string.Empty);
			ErrorAnalytics.StoreAndForwardError(exception.GetType().ToString(), message, stackTrace, stackTrace2, severity, metaData);
		}

		// Token: 0x06001E77 RID: 7799 RVA: 0x00081178 File Offset: 0x0007F578
		private static void NotifyError(ErrorAnalytics.LogSeverity severity, string errorType, string message, Dictionary<string, object> metaData)
		{
			string stackTrace = Environment.StackTrace;
			List<ParsingUtility.StackTraceElement> stackTrace2 = ParsingUtility.ParseStackTraceElements(stackTrace, ParsingUtility.ParseType.EnvironmentStackTrace);
			message = (message ?? string.Empty);
			ErrorAnalytics.StoreAndForwardError(errorType, message, stackTrace, stackTrace2, severity, metaData);
		}

		// Token: 0x06001E78 RID: 7800 RVA: 0x000811B0 File Offset: 0x0007F5B0
		public static void InfoBreadcrumb(string message)
		{
			string str = global::Wooga.Core.Utilities.Time.Now() + ": ";
			ErrorAnalytics.AddBreadcrumb(str + message);
		}

		// Token: 0x06001E79 RID: 7801 RVA: 0x000811DE File Offset: 0x0007F5DE
		public static void Debug(string message)
		{
		}

		// Token: 0x06001E7A RID: 7802 RVA: 0x000811E0 File Offset: 0x0007F5E0
		private static void Notify(ErrorAnalytics.LogSeverity severity, Exception ex, string message, Dictionary<string, object> metaData, Action<string> logHandler)
		{
			if (Log.LogToErrorAnalytics)
			{
				Log.NotifyError(severity, ex, message, metaData);
				return;
			}
			logHandler(ex.Message + ((ex.StackTrace == null) ? string.Empty : ("\n" + ex.StackTrace)));
		}

		// Token: 0x06001E7B RID: 7803 RVA: 0x00081238 File Offset: 0x0007F638
		private static void Notify(ErrorAnalytics.LogSeverity severity, string errorType, string message, Dictionary<string, object> metaData, Action<string> logHandler)
		{
			if (Log.LogToErrorAnalytics)
			{
				Log.NotifyError(severity, errorType, message, metaData);
				return;
			}
			logHandler(message);
		}

		// Token: 0x06001E7C RID: 7804 RVA: 0x00081256 File Offset: 0x0007F656
		public static void Warning(Exception exception, string message = null, Dictionary<string, object> metaData = null)
		{
			ErrorAnalytics.LogSeverity severity = ErrorAnalytics.LogSeverity.Warning;
			if (Log._003C_003Ef__mg_0024cache0 == null)
			{
				Log._003C_003Ef__mg_0024cache0 = new Action<string>(global::UnityEngine.Debug.LogWarning);
			}
			Log.Notify(severity, exception, message, metaData, Log._003C_003Ef__mg_0024cache0);
		}

		// Token: 0x06001E7D RID: 7805 RVA: 0x0008127E File Offset: 0x0007F67E
		public static void Error(Exception exception, string message = null, Dictionary<string, object> metaData = null)
		{
			ErrorAnalytics.LogSeverity severity = ErrorAnalytics.LogSeverity.Error;
			if (Log._003C_003Ef__mg_0024cache1 == null)
			{
				Log._003C_003Ef__mg_0024cache1 = new Action<string>(global::UnityEngine.Debug.LogError);
			}
			Log.Notify(severity, exception, message, metaData, Log._003C_003Ef__mg_0024cache1);
		}

		// Token: 0x06001E7E RID: 7806 RVA: 0x000812A6 File Offset: 0x0007F6A6
		public static void Fatal(Exception exception, string message = null, Dictionary<string, object> metaData = null)
		{
			ErrorAnalytics.LogSeverity severity = ErrorAnalytics.LogSeverity.Fatal;
			if (Log._003C_003Ef__mg_0024cache2 == null)
			{
				Log._003C_003Ef__mg_0024cache2 = new Action<string>(global::UnityEngine.Debug.LogError);
			}
			Log.Notify(severity, exception, message, metaData, Log._003C_003Ef__mg_0024cache2);
		}

		// Token: 0x06001E7F RID: 7807 RVA: 0x000812CE File Offset: 0x0007F6CE
		public static void Warning(string errorType, string message = null, Dictionary<string, object> metaData = null)
		{
			ErrorAnalytics.LogSeverity severity = ErrorAnalytics.LogSeverity.Warning;
			if (Log._003C_003Ef__mg_0024cache3 == null)
			{
				Log._003C_003Ef__mg_0024cache3 = new Action<string>(global::UnityEngine.Debug.LogWarning);
			}
			Log.Notify(severity, errorType, message, metaData, Log._003C_003Ef__mg_0024cache3);
		}

		// Token: 0x06001E80 RID: 7808 RVA: 0x000812F6 File Offset: 0x0007F6F6
		public static void Error(string errorType, string message = null, Dictionary<string, object> metaData = null)
		{
			ErrorAnalytics.LogSeverity severity = ErrorAnalytics.LogSeverity.Error;
			if (Log._003C_003Ef__mg_0024cache4 == null)
			{
				Log._003C_003Ef__mg_0024cache4 = new Action<string>(global::UnityEngine.Debug.LogError);
			}
			Log.Notify(severity, errorType, message, metaData, Log._003C_003Ef__mg_0024cache4);
		}

		// Token: 0x06001E81 RID: 7809 RVA: 0x0008131E File Offset: 0x0007F71E
		public static void Fatal(string errorType, string message = null, Dictionary<string, object> metaData = null)
		{
			ErrorAnalytics.LogSeverity severity = ErrorAnalytics.LogSeverity.Fatal;
			if (Log._003C_003Ef__mg_0024cache5 == null)
			{
				Log._003C_003Ef__mg_0024cache5 = new Action<string>(global::UnityEngine.Debug.LogError);
			}
			Log.Notify(severity, errorType, message, metaData, Log._003C_003Ef__mg_0024cache5);
		}

		// Token: 0x06001E82 RID: 7810 RVA: 0x00081346 File Offset: 0x0007F746
		// Note: this type is marked as 'beforefieldinit'.
		static Log()
		{
			if (Log._003C_003Ef__mg_0024cache6 == null)
			{
				Log._003C_003Ef__mg_0024cache6 = new Log.LogHandler(Console.WriteLine);
			}
			Log.logHandler = Log._003C_003Ef__mg_0024cache6;
			Log.LogToErrorAnalytics = false;
		}

		// Token: 0x04004A4A RID: 19018
		private const string _emptyMessage = "";

		// Token: 0x04004A4B RID: 19019
		public static Log.LogHandler logHandler;

		// Token: 0x04004A4C RID: 19020
		public static bool LogToErrorAnalytics;

		// Token: 0x04004A4D RID: 19021
		[CompilerGenerated]
		private static Action<string> _003C_003Ef__mg_0024cache0;

		// Token: 0x04004A4E RID: 19022
		[CompilerGenerated]
		private static Action<string> _003C_003Ef__mg_0024cache1;

		// Token: 0x04004A4F RID: 19023
		[CompilerGenerated]
		private static Action<string> _003C_003Ef__mg_0024cache2;

		// Token: 0x04004A50 RID: 19024
		[CompilerGenerated]
		private static Action<string> _003C_003Ef__mg_0024cache3;

		// Token: 0x04004A51 RID: 19025
		[CompilerGenerated]
		private static Action<string> _003C_003Ef__mg_0024cache4;

		// Token: 0x04004A52 RID: 19026
		[CompilerGenerated]
		private static Action<string> _003C_003Ef__mg_0024cache5;

		// Token: 0x04004A53 RID: 19027
		[CompilerGenerated]
		private static Log.LogHandler _003C_003Ef__mg_0024cache6;

		// Token: 0x020003FE RID: 1022
		// (Invoke) Token: 0x06001E84 RID: 7812
		public delegate void LogHandler(string message);
	}
}
