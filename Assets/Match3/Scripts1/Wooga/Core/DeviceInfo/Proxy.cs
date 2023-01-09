using System;
using System.Net;

namespace Match3.Scripts1.Wooga.Core.DeviceInfo
{
	// Token: 0x02000352 RID: 850
	public static class Proxy
	{
		// Token: 0x060019EA RID: 6634 RVA: 0x00074D38 File Offset: 0x00073138
		private static Proxy.AndroidProxySettings getProxySettings()
		{
			// AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			// AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
			//AndroidJavaObject androidJavaObject = new AndroidJavaObject("com.wooga.deviceinfo.proxy.ProxySettings", new object[]
			//{
			//	@static
			//});
			Proxy.AndroidProxySettings androidProxySettings = new Proxy.AndroidProxySettings();
			//if (androidJavaObject != null)
			//{
			//	androidProxySettings.Host = androidJavaObject.Call<string>("getHost", new object[0]);
			//	androidProxySettings.Port = androidJavaObject.Call<int>("getPort", new object[0]);
			//	androidProxySettings.ExclusionList = androidJavaObject.Call<string>("getExclusionList", new object[0]);
			//}
			return androidProxySettings;
		}

		// Token: 0x060019EB RID: 6635 RVA: 0x00074DC3 File Offset: 0x000731C3
		private static void ensureProxySettings()
		{
			if (Proxy.proxySettings == null)
			{
				Proxy.proxySettings = Proxy.getProxySettings();
			}
		}

		// Token: 0x060019EC RID: 6636 RVA: 0x00074DD9 File Offset: 0x000731D9
		public static string GetHost()
		{
			Proxy.ensureProxySettings();
			return Proxy.proxySettings.Host;
		}

		// Token: 0x060019ED RID: 6637 RVA: 0x00074DEA File Offset: 0x000731EA
		public static int GetPort()
		{
			Proxy.ensureProxySettings();
			return Proxy.proxySettings.Port;
		}

		// Token: 0x060019EE RID: 6638 RVA: 0x00074DFB File Offset: 0x000731FB
		public static string GetExclusionList()
		{
			Proxy.ensureProxySettings();
			return Proxy.proxySettings.ExclusionList;
		}

		// Token: 0x060019EF RID: 6639 RVA: 0x00074E0C File Offset: 0x0007320C
		public static WebProxy GetWebProxy()
		{
			string host = Proxy.GetHost();
			int port = Proxy.GetPort();
			if (host == string.Empty || port == 0 || port == -1)
			{
				return null;
			}
			return new WebProxy
			{
				Address = new Uri(string.Concat(new object[]
				{
					"http://",
					host,
					":",
					port
				}))
			};
		}

		// Token: 0x060019F0 RID: 6640 RVA: 0x00074E80 File Offset: 0x00073280
		public static void SetGlobalDefaultProxy()
		{
			WebProxy webProxy = Proxy.GetWebProxy();
			if (webProxy != null)
			{
				WebRequest.DefaultWebProxy = webProxy;
			}
		}

		// Token: 0x04004854 RID: 18516
		private static Proxy.AndroidProxySettings proxySettings;

		// Token: 0x02000353 RID: 851
		internal class AndroidProxySettings
		{
			// Token: 0x04004855 RID: 18517
			public string Host;

			// Token: 0x04004856 RID: 18518
			public int Port;

			// Token: 0x04004857 RID: 18519
			public string ExclusionList;
		}
	}
}
