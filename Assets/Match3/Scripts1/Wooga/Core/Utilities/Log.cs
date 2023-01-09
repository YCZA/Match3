using System;
using System.Collections;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Match3.Scripts1;
using UnityEngine;

namespace Wooga.Core.Utilities
{
	// Token: 0x020003A8 RID: 936
	public static class Log
	{
		// Token: 0x06001C35 RID: 7221 RVA: 0x0007C226 File Offset: 0x0007A626
		static Log()
		{
			if (Log._003C_003Ef__mg_0024cache0 == null)
			{
				Log._003C_003Ef__mg_0024cache0 = new LogHandler(UnityLogReceiver.Log);
			}
			Log.OnLog += Log._003C_003Ef__mg_0024cache0;
		}

		// Token: 0x14000020 RID: 32
		// (add) Token: 0x06001C36 RID: 7222 RVA: 0x0007C250 File Offset: 0x0007A650
		// (remove) Token: 0x06001C37 RID: 7223 RVA: 0x0007C284 File Offset: 0x0007A684
		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event LogHandler OnLog;

		// Token: 0x06001C38 RID: 7224 RVA: 0x0007C2B8 File Offset: 0x0007A6B8
		public static void Error(params object[] logObjects)
		{
			Log.LogString(logObjects, SeverityId.Error);
		}

		// Token: 0x06001C39 RID: 7225 RVA: 0x0007C2C1 File Offset: 0x0007A6C1
		public static void Warning(params object[] logObjects)
		{
			Log.LogString(logObjects, SeverityId.Warning);
		}

		// Token: 0x06001C3A RID: 7226 RVA: 0x0007C2CA File Offset: 0x0007A6CA
		public static void Info(params object[] logObjects)
		{
			Log.LogString(logObjects, SeverityId.Info);
		}

		// Token: 0x06001C3B RID: 7227 RVA: 0x0007C2D3 File Offset: 0x0007A6D3
		public static void Debug(params object[] logObjects)
		{
			Log.LogString(logObjects, SeverityId.Debug);
		}

		// Token: 0x06001C3C RID: 7228 RVA: 0x0007C2DC File Offset: 0x0007A6DC
		public static void ErrorFormatted(string format, params object[] logObjects)
		{
			Log.LogStringFormatted(format, logObjects, SeverityId.Error);
		}

		// Token: 0x06001C3D RID: 7229 RVA: 0x0007C2E6 File Offset: 0x0007A6E6
		public static void WarningFormatted(string format, params object[] logObjects)
		{
			Log.LogStringFormatted(format, logObjects, SeverityId.Warning);
		}

		// Token: 0x06001C3E RID: 7230 RVA: 0x0007C2F0 File Offset: 0x0007A6F0
		public static void InfoFormatted(string format, params object[] logObjects)
		{
			Log.LogStringFormatted(format, logObjects, SeverityId.Info);
		}

		// Token: 0x06001C3F RID: 7231 RVA: 0x0007C2FA File Offset: 0x0007A6FA
		public static void DebugFormatted(string format, params object[] logObjects)
		{
			Log.LogStringFormatted(format, logObjects, SeverityId.Debug);
		}

		// Token: 0x06001C40 RID: 7232 RVA: 0x0007C304 File Offset: 0x0007A704
		public static void NativeLogListener()
		{
		}

		// Token: 0x06001C41 RID: 7233 RVA: 0x0007C306 File Offset: 0x0007A706
		private static bool ShouldLog(SeverityId severity)
		{
			return Log.MinSeverity <= severity;
		}

		// Token: 0x06001C42 RID: 7234 RVA: 0x0007C313 File Offset: 0x0007A713
		private static void HandleLog(string message, SeverityId severity)
		{
			if (Log.OnLog != null)
			{
				Log.OnLog(message, severity);
			}
		}

		// Token: 0x06001C43 RID: 7235 RVA: 0x0007C32C File Offset: 0x0007A72C
		private static void LogString(object[] logObjects, SeverityId severityId)
		{
			if (!Log.ShouldLog(severityId))
			{
				return;
			}
			string message;
			if (logObjects.Length > 0)
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (object obj in logObjects)
				{
					stringBuilder.Append(Log.ObjectToString(obj) + " | ");
				}
				stringBuilder.Remove(stringBuilder.Length - 3, 3);
				message = stringBuilder.ToString();
			}
			else
			{
				message = "<NOTHING_TO_LOG>";
			}
			Log.HandleLog(message, severityId);
		}

		// Token: 0x06001C44 RID: 7236 RVA: 0x0007C3B4 File Offset: 0x0007A7B4
		private static void LogStringFormatted(string format, object[] logObjects, SeverityId severityId)
		{
			if (Log.ShouldLog(severityId))
			{
				if (Log._003C_003Ef__mg_0024cache1 == null)
				{
					Log._003C_003Ef__mg_0024cache1 = new Func<object, string>(Log.ObjectToString);
				}
				Log.HandleLog(string.Format(format, logObjects.Select(Log._003C_003Ef__mg_0024cache1).ToArray<string>()), severityId);
			}
		}

		// Token: 0x06001C45 RID: 7237 RVA: 0x0007C400 File Offset: 0x0007A800
		public static string EnumerableToString(IEnumerable list)
		{
			if (!list.IsNullOrEmptyEnumerable())
			{
				StringBuilder stringBuilder = new StringBuilder();
				IEnumerator enumerator = list.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						stringBuilder.Append(Log.ObjectToString(obj) + ", ");
					}
				}
				finally
				{
					IDisposable disposable;
					if ((disposable = (enumerator as IDisposable)) != null)
					{
						disposable.Dispose();
					}
				}
				stringBuilder.Remove(stringBuilder.Length - 2, 2);
				return stringBuilder.ToString();
			}
			return (list != null) ? "<EMPTY>" : "<NULL>";
		}

		// Token: 0x06001C46 RID: 7238 RVA: 0x0007C4A8 File Offset: 0x0007A8A8
		public static string DictToString(IDictionary dict)
		{
			if (!dict.IsNullOrEmptyCollection())
			{
				StringBuilder stringBuilder = new StringBuilder();
				IEnumerator enumerator = dict.Keys.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						stringBuilder.AppendFormat("{0}: {1}, ", Log.ObjectToString(obj), Log.ObjectToString(dict[obj]));
					}
				}
				finally
				{
					IDisposable disposable;
					if ((disposable = (enumerator as IDisposable)) != null)
					{
						disposable.Dispose();
					}
				}
				stringBuilder.Remove(stringBuilder.Length - 2, 2);
				return stringBuilder.ToString();
			}
			return (dict != null) ? "<EMPTY>" : "<NULL>";
		}

		// Token: 0x06001C47 RID: 7239 RVA: 0x0007C55C File Offset: 0x0007A95C
		public static string ObjectToString(object obj)
		{
			if (obj == null)
			{
				return "<NULL>";
			}
			Type type = obj.GetType();
			if (typeof(IDictionary).IsAssignableFrom(type))
			{
				return "[ " + Log.DictToString(obj as IDictionary) + " ]";
			}
			if (typeof(IEnumerable).IsAssignableFrom(type) && type != typeof(string) && type != typeof(Transform))
			{
				return "[ " + Log.EnumerableToString(obj as IEnumerable) + " ]";
			}
			return obj.ToString();
		}

		// Token: 0x04004987 RID: 18823
		private const string NULL = "<NULL>";

		// Token: 0x04004988 RID: 18824
		private const string EMPTY = "<EMPTY>";

		// Token: 0x04004989 RID: 18825
		private const string NOTHING = "<NOTHING_TO_LOG>";

		// Token: 0x0400498A RID: 18826
		public static bool LogSbsRequest;

		// Token: 0x0400498B RID: 18827
		public static SeverityId MinSeverity = SeverityId.Warning;

		// Token: 0x0400498D RID: 18829
		[CompilerGenerated]
		private static LogHandler _003C_003Ef__mg_0024cache0;

		// Token: 0x0400498E RID: 18830
		[CompilerGenerated]
		private static Func<object, string> _003C_003Ef__mg_0024cache1;
	}
}
