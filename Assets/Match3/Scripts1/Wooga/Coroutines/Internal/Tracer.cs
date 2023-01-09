using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Wooga.Coroutines.YieldOperations;

namespace Wooga.Coroutines.Internal
{
	// Token: 0x020003D0 RID: 976
	public static class Tracer
	{
		// Token: 0x06001D9C RID: 7580 RVA: 0x0007EE50 File Offset: 0x0007D250
		[Conditional("COROUTINE_TRACE")]
		public static void Trace(this IYieldOperation yieldOperation, string message, params object[] objects)
		{
			if (!Tracer.Enabled)
			{
				return;
			}
			string text = Tracer.FormatMessage(message, objects);
			global::UnityEngine.Debug.Log(string.Concat(new object[]
			{
				"Frame(",
				Time.frameCount,
				") ",
				yieldOperation.GetCallInfo(),
				": ",
				text
			}));
		}

		// Token: 0x06001D9D RID: 7581 RVA: 0x0007EEB4 File Offset: 0x0007D2B4
		[Conditional("COROUTINE_TRACE")]
		public static void Trace(string message, params object[] objects)
		{
			if (!Tracer.Enabled)
			{
				return;
			}
			string text = Tracer.FormatMessage(message, objects);
			global::UnityEngine.Debug.Log(string.Concat(new object[]
			{
				"Frame(",
				Time.frameCount,
				") ",
				text
			}));
		}

		// Token: 0x06001D9E RID: 7582 RVA: 0x0007EF08 File Offset: 0x0007D308
		private static string FormatMessage(string message, object[] objects)
		{
			if (objects != null && objects.Length > 0)
			{
				try
				{
					if (Tracer._003C_003Ef__mg_0024cache0 == null)
					{
						Tracer._003C_003Ef__mg_0024cache0 = new Func<object, object>(Tracer.ObjectToString);
					}
					return string.Format(message, objects.Select(Tracer._003C_003Ef__mg_0024cache0).ToArray<object>());
				}
				catch
				{
					return message;
				}
				return message;
			}
			return message;
		}

		// Token: 0x06001D9F RID: 7583 RVA: 0x0007EF74 File Offset: 0x0007D374
		private static object ObjectToString(object logObj)
		{
			Exception ex = logObj as Exception;
			return (ex == null) ? logObj : string.Format("{0}({1})", ex.GetType(), ex);
		}

		// Token: 0x040049D8 RID: 18904
		public static bool Enabled = true;

		// Token: 0x040049D9 RID: 18905
		[CompilerGenerated]
		private static Func<object, object> _003C_003Ef__mg_0024cache0;
	}
}
