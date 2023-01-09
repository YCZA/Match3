using System;
using System.Reflection;
using Match3.Scripts1.Wooga.Services.ErrorAnalytics;
using UnityEngine;

// Token: 0x02000B4B RID: 2891
namespace Match3.Scripts1
{
	public static class EAHelper
	{
		// Token: 0x060043B4 RID: 17332 RVA: 0x00159BFC File Offset: 0x00157FFC
		public static void AddBreadcrumb(string text)
		{
			ErrorAnalytics.AddBreadcrumb(string.Format("[{0:0.0000}]{1}", Time.realtimeSinceStartup, text));
		}

		// Token: 0x060043B5 RID: 17333 RVA: 0x00159C18 File Offset: 0x00158018
		public static void LogBreadcrumbs()
		{
			Type typeFromHandle = typeof(ErrorAnalytics);
			object value = typeFromHandle.GetField("_breadrumbs", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
			MethodInfo method = value.GetType().GetMethod("ToArray", BindingFlags.Instance | BindingFlags.Public);
			string[] array = (string[])method.Invoke(value, null);
			string text = string.Empty;
			foreach (string str in array)
			{
				text = text + str + "\n";
			}
			WoogaDebug.Log(new object[]
			{
				text
			});
		}
	}
}
