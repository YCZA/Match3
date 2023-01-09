using System;
using UnityEngine;

namespace Match3.Scripts1.com.adjust.sdk
{
	// Token: 0x02000009 RID: 9
	public class Adjust : MonoBehaviour
	{
		// Token: 0x06000071 RID: 113 RVA: 0x00004104 File Offset: 0x00002504
		private void Awake()
		{
			if (!Application.isEditor)
			{
				//UnityEngine.Object.DontDestroyOnLoad(base.transform.gameObject);
				//if (!this.startManually)
				//{
				//	AdjustConfig adjustConfig = new AdjustConfig(this.appToken, this.environment, this.logLevel == AdjustLogLevel.Suppress);
				//	adjustConfig.setLogLevel(this.logLevel);
				//	adjustConfig.setSendInBackground(this.sendInBackground);
				//	adjustConfig.setEventBufferingEnabled(this.eventBuffering);
				//	adjustConfig.setLaunchDeferredDeeplink(this.launchDeferredDeeplink);
				//	Adjust.start(adjustConfig);
				//}
			}
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00004187 File Offset: 0x00002587
		private void OnApplicationPause(bool pauseStatus)
		{
			if (!Application.isEditor)
			{
				//if (pauseStatus)
				//{
				//	AdjustAndroid.OnPause();
				//}
				//else
				//{
				//	AdjustAndroid.OnResume();
				//}
			}
		}

		// Token: 0x06000073 RID: 115 RVA: 0x000041A8 File Offset: 0x000025A8
		public static void start(AdjustConfig adjustConfig)
		{
			if (!Application.isEditor)
			{
				//if (adjustConfig == null)
				//{
				//	UnityEngine.Debug.Log("Adjust: Missing config to start.");
				//	return;
				//}
				//AdjustAndroid.Start(adjustConfig);
			}
		}

		// Token: 0x06000074 RID: 116 RVA: 0x000041CB File Offset: 0x000025CB
		public static void trackEvent(AdjustEvent adjustEvent)
		{
			if (!Application.isEditor)
			{
				//if (adjustEvent == null)
				//{
				//	UnityEngine.Debug.Log("Adjust: Missing event to track.");
				//	return;
				//}
				//AdjustAndroid.TrackEvent(adjustEvent);
			}
		}

		// Token: 0x06000075 RID: 117 RVA: 0x000041EE File Offset: 0x000025EE
		public static void setEnabled(bool enabled)
		{
			if (!Application.isEditor)
			{
				//AdjustAndroid.SetEnabled(enabled);
			}
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00004200 File Offset: 0x00002600
		public static bool isEnabled()
		{
			//return !Application.isEditor && AdjustAndroid.IsEnabled();
			return false;
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00004213 File Offset: 0x00002613
		public static void setOfflineMode(bool enabled)
		{
			if (!Application.isEditor)
			{
				//AdjustAndroid.SetOfflineMode(enabled);
			}
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00004225 File Offset: 0x00002625
		public static void setDeviceToken(string deviceToken)
		{
			if (!Application.isEditor)
			{
				//AdjustAndroid.SetDeviceToken(deviceToken);
			}
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00004237 File Offset: 0x00002637
		public static void appWillOpenUrl(string url)
		{
			if (!Application.isEditor)
			{
				//AdjustAndroid.AppWillOpenUrl(url);
			}
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00004249 File Offset: 0x00002649
		public static void sendFirstPackages()
		{
			if (!Application.isEditor)
			{
				//AdjustAndroid.SendFirstPackages();
			}
		}

		// Token: 0x0600007B RID: 123 RVA: 0x0000425A File Offset: 0x0000265A
		public static void addSessionPartnerParameter(string key, string value)
		{
			if (!Application.isEditor)
			{
				//AdjustAndroid.AddSessionPartnerParameter(key, value);
			}
		}

		// Token: 0x0600007C RID: 124 RVA: 0x0000426D File Offset: 0x0000266D
		public static void addSessionCallbackParameter(string key, string value)
		{
			if (!Application.isEditor)
			{
				//AdjustAndroid.AddSessionCallbackParameter(key, value);
			}
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00004280 File Offset: 0x00002680
		public static void removeSessionPartnerParameter(string key)
		{
			if (!Application.isEditor)
			{
				//AdjustAndroid.RemoveSessionPartnerParameter(key);
			}
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00004292 File Offset: 0x00002692
		public static void removeSessionCallbackParameter(string key)
		{
			if (!Application.isEditor)
			{
				//AdjustAndroid.RemoveSessionCallbackParameter(key);
			}
		}

		// Token: 0x0600007F RID: 127 RVA: 0x000042A4 File Offset: 0x000026A4
		public static void resetSessionPartnerParameters()
		{
			if (!Application.isEditor)
			{
				//AdjustAndroid.ResetSessionPartnerParameters();
			}
		}

		// Token: 0x06000080 RID: 128 RVA: 0x000042B5 File Offset: 0x000026B5
		public static void resetSessionCallbackParameters()
		{
			if (!Application.isEditor)
			{
				//AdjustAndroid.ResetSessionCallbackParameters();
			}
		}

		// Token: 0x06000081 RID: 129 RVA: 0x000042C6 File Offset: 0x000026C6
		public static string getAdid()
		{
			if (!Application.isEditor)
			{
				//return AdjustAndroid.GetAdid();
			}
			return string.Empty;
		}

		// Token: 0x06000082 RID: 130 RVA: 0x000042DD File Offset: 0x000026DD
		public static AdjustAttribution getAttribution()
		{
			if (!Application.isEditor)
			{
				//return AdjustAndroid.GetAttribution();
			}
			return null;
		}

		// Token: 0x06000083 RID: 131 RVA: 0x000042F0 File Offset: 0x000026F0
		public static string getWinAdid()
		{
			if (!Application.isEditor)
			{
				//UnityEngine.Debug.Log("Adjust: Error! Windows Advertising ID is not available on Android platform.");
				return string.Empty;
			}
			return string.Empty;
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00004311 File Offset: 0x00002711
		public static string getIdfa()
		{
			if (!Application.isEditor)
			{
				//UnityEngine.Debug.Log("Adjust: Error! IDFA is not available on Android platform.");
				return string.Empty;
			}
			return string.Empty;
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00004332 File Offset: 0x00002732
		[Obsolete("This method is intended for testing purposes only. Do not use it.")]
		public static void setReferrer(string referrer)
		{
			if (!Application.isEditor)
			{
				//AdjustAndroid.SetReferrer(referrer);
			}
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00004344 File Offset: 0x00002744
		public static void getGoogleAdId(Action<string> onDeviceIdsRead)
		{
			if (!Application.isEditor)
			{
				//AdjustAndroid.GetGoogleAdId(onDeviceIdsRead);
			}
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00004356 File Offset: 0x00002756
		public static string getAmazonAdId()
		{
			if (!Application.isEditor)
			{
				//return AdjustAndroid.GetAmazonAdId();
			}
			return string.Empty;
		}

		// Token: 0x0400000E RID: 14
		private const string errorMsgStart = "Adjust: SDK not started. Start it manually using the 'start' method.";

		// Token: 0x0400000F RID: 15
		private const string errorMsgPlatform = "Adjust: SDK can only be used in Android, iOS, Windows Phone 8.1, Windows Store or Universal Windows apps.";

		// Token: 0x04000010 RID: 16
		public bool startManually = true;

		// Token: 0x04000011 RID: 17
		public bool eventBuffering;

		// Token: 0x04000012 RID: 18
		public bool sendInBackground;

		// Token: 0x04000013 RID: 19
		public bool launchDeferredDeeplink = true;

		// Token: 0x04000014 RID: 20
		public string appToken = "{Your App Token}";

		// Token: 0x04000015 RID: 21
		public AdjustLogLevel logLevel = AdjustLogLevel.Info;

		// Token: 0x04000016 RID: 22
		public AdjustEnvironment environment;
	}
}
