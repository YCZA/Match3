using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Wooga.Core.Utilities;
using Match3.Scripts1.Wooga.Services.Tracking.Tools;

namespace Match3.Scripts1.Wooga.Services.Tracking.Calls
{
	// Token: 0x02000435 RID: 1077
	public class AppLinks
	{
		// Token: 0x14000022 RID: 34
		// (add) Token: 0x06001F94 RID: 8084 RVA: 0x00084E38 File Offset: 0x00083238
		// (remove) Token: 0x06001F95 RID: 8085 RVA: 0x00084E6C File Offset: 0x0008326C
		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event AppLinks.LinkReceivedHandler OnAppLink;

		// Token: 0x06001F96 RID: 8086 RVA: 0x00084EA0 File Offset: 0x000832A0
		public static void Track(string applink)
		{
			try
			{
				Uri uri = new Uri(applink);
				string eventType = AppLinks.GetEventType(uri);
				NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(uri.Query);
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				foreach (string text in nameValueCollection.AllKeys)
				{
					dictionary.Add(text, nameValueCollection[text]);
				}
				dictionary.Add("event_type", eventType);
				Tracking.Track("ige", dictionary);
				if (AppLinks.OnAppLink != null)
				{
					AppLinks.OnAppLink(applink);
				}
			}
			catch (Exception)
			{
				Log.Error(new object[]
				{
					"Error processing applink " + applink
				});
			}
		}

		// Token: 0x06001F97 RID: 8087 RVA: 0x00084F68 File Offset: 0x00083368
		public static string GetEventType(Uri uri)
		{
			if (uri.Scheme.Equals("http") || uri.Scheme.Equals("https"))
			{
				return uri.Segments.Last<string>();
			}
			return uri.Host;
		}

		// Token: 0x04004AEB RID: 19179
		private const string CALL_NAME = "ige";

		// Token: 0x02000436 RID: 1078
		// (Invoke) Token: 0x06001F99 RID: 8089
		public delegate void LinkReceivedHandler(string applink);
	}
}
