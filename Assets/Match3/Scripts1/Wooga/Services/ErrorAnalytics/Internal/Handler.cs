using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Wooga.Core.Extensions;
using UnityEngine;
// using Wooga.Lambda.Control.Concurrent;

namespace Match3.Scripts1.Wooga.Services.ErrorAnalytics.Internal
{
	// Token: 0x020003F4 RID: 1012
	internal static class Handler
	{
		// Token: 0x06001E4D RID: 7757 RVA: 0x000808A0 File Offset: 0x0007ECA0
		public static void Init(bool ignoreFollowupFatals = false)
		{
			if (Handler._003C_003Ef__mg_0024cache0 == null)
			{
				Handler._003C_003Ef__mg_0024cache0 = new Application.LogCallback(Handler.HandleLog);
			}
			Application.logMessageReceivedThreaded += Handler._003C_003Ef__mg_0024cache0;
			AppDomain currentDomain = AppDomain.CurrentDomain;
			if (Handler._003C_003Ef__mg_0024cache1 == null)
			{
				Handler._003C_003Ef__mg_0024cache1 = new UnhandledExceptionEventHandler(Handler.HandleUnhandledException);
			}
			currentDomain.UnhandledException += Handler._003C_003Ef__mg_0024cache1;
			// if (Handler._003C_003Ef__mg_0024cache2 == null)
			// {
				// Handler._003C_003Ef__mg_0024cache2 = new Async.AsyncComputationExceptionEventHandler(Handler.HandleException);
			// }
			// Async.AsyncComputationExceptionEvent += Handler._003C_003Ef__mg_0024cache2;
			Handler._ignoreFollowupFatals = ignoreFollowupFatals;
			Handler.hadFatal = false;
		}

		// Token: 0x06001E4E RID: 7758 RVA: 0x00080924 File Offset: 0x0007ED24
		private static void HandleException(Exception exc)
		{
			Handler.HandleException(exc, exc.StackTrace ?? new StackTrace(1).ToString());
		}

		// Token: 0x06001E4F RID: 7759 RVA: 0x00080944 File Offset: 0x0007ED44
		private static void HandleException(Exception exc, string stackTrace)
		{
			ParsedException ex = new ParsedException
			{
				exception = exc,
				errorClass = exc.GetType().Name,
				errorMessage = exc.Message,
				stackTrace = stackTrace,
				elements = ParsingUtility.ParseStackTraceElements(exc.StackTrace, ParsingUtility.ParseType.EnvironmentStackTrace),
				severity = ErrorAnalytics.LogSeverity.Fatal
			};
			Handler.Handle(ex);
		}

		// Token: 0x06001E50 RID: 7760 RVA: 0x000809A4 File Offset: 0x0007EDA4
		private static void HandleUnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			Exception ex = e.ExceptionObject as Exception;
			Handler.HandleException(ex, ex.StackTrace ?? new StackTrace(1).ToString());
		}

		// Token: 0x06001E51 RID: 7761 RVA: 0x000809DC File Offset: 0x0007EDDC
		private static void HandleLog(string logString, string stackTrace, LogType type)
		{
			if (Handler.hadFatal && Handler._ignoreFollowupFatals)
			{
				return;
			}
			if (type == LogType.Exception || type == LogType.Error)
			{
				Handler.hadFatal = true;
				ErrorAnalytics.LogSeverity severity = (type != LogType.Exception) ? ErrorAnalytics.LogSeverity.Error : ErrorAnalytics.LogSeverity.Fatal;
				ParsedException ex = ParsingUtility.ParseExceptionLog(logString, severity);
				ex.stackTrace = stackTrace;
				ex.elements = ParsingUtility.ParseStackTraceElements(stackTrace, ParsingUtility.ParseType.LogStackTrace);
				Handler.Handle(ex);
			}
		}

		// Token: 0x06001E52 RID: 7762 RVA: 0x00080A44 File Offset: 0x0007EE44
		private static void Handle(ParsedException ex)
		{
			if (Handler.parsedExceptionUpdater != null)
			{
				Handler.parsedExceptionUpdater.UpdateParsedException(ex);
			}
			if (ex.severity == ErrorAnalytics.LogSeverity.None)
			{
				return;
			}
			if (ex.severity == ErrorAnalytics.LogSeverity.Info)
			{
				string arg = (ex.elements.Count <= 0) ? string.Concat(ex.stackTrace.Take(100)) : ex.elements[0].method;
				string breadcrumb = string.Format("EXCEPTION at {0} >>> {1}: {2}", arg, ex.errorClass, ex.errorMessage);
				ErrorAnalytics.AddBreadcrumb(breadcrumb);
			}
			else
			{
				string text = Guid.NewGuid().ToString("N");
				ex.meta["exUUID"] = text;
				ex.uuid = text;
				if (!Handler.CancelStoreAndForward(ex))
				{
					ErrorAnalytics.StoreAndForwardError(ex.errorClass, ex.errorMessage, ex.stackTrace, ex.elements, ex.severity, ex.meta);
				}
			}
			if (Handler.exceptionCallback != null)
			{
				Handler.exceptionCallback(ex);
			}
		}

		// Token: 0x06001E53 RID: 7763 RVA: 0x00080B50 File Offset: 0x0007EF50
		private static bool CancelStoreAndForward(ParsedException ex)
		{
			int i = 0;
			int count = Handler.filterPredicates.Count;
			while (i < count)
			{
				if (Handler.filterPredicates[i](ex))
				{
					return true;
				}
				i++;
			}
			return false;
		}

		// Token: 0x06001E54 RID: 7764 RVA: 0x00080B93 File Offset: 0x0007EF93
		public static void RegisterExceptionFilter(Predicate<ParsedException> predicate)
		{
			Assert.That(predicate != null, "Cannot pass in NULL predicate");
			Handler.filterPredicates.Add(predicate);
		}

		// Token: 0x06001E55 RID: 7765 RVA: 0x00080BB1 File Offset: 0x0007EFB1
		public static void UnregisterExceptionFilter(Predicate<ParsedException> predicate)
		{
			Assert.That(predicate != null, "Cannot pass in NULL predicate");
			Handler.filterPredicates.Remove(predicate);
		}

		// Token: 0x04004A28 RID: 18984
		public static Action<ParsedException> exceptionCallback = delegate(ParsedException ex)
		{
		};

		// Token: 0x04004A29 RID: 18985
		public static IParsedExceptionUpdater parsedExceptionUpdater;

		// Token: 0x04004A2A RID: 18986
		public static bool hadFatal;

		// Token: 0x04004A2B RID: 18987
		private static readonly List<Predicate<ParsedException>> filterPredicates = new List<Predicate<ParsedException>>();

		// Token: 0x04004A2C RID: 18988
		private static bool _ignoreFollowupFatals;

		// Token: 0x04004A2D RID: 18989
		[CompilerGenerated]
		private static Application.LogCallback _003C_003Ef__mg_0024cache0;

		// Token: 0x04004A2E RID: 18990
		[CompilerGenerated]
		private static UnhandledExceptionEventHandler _003C_003Ef__mg_0024cache1;

		// Token: 0x04004A2F RID: 18991
		// [CompilerGenerated]
		// private static Async.AsyncComputationExceptionEventHandler _003C_003Ef__mg_0024cache2;
	}
}
