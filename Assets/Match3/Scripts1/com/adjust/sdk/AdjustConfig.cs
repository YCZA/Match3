using System;

namespace Match3.Scripts1.com.adjust.sdk
{
	// Token: 0x02000014 RID: 20
	public class AdjustConfig
	{
		// Token: 0x060000CE RID: 206 RVA: 0x00005E12 File Offset: 0x00004212
		public AdjustConfig(string appToken, AdjustEnvironment environment)
		{
			this.sceneName = string.Empty;
			this.processName = string.Empty;
			this.appToken = appToken;
			this.environment = environment;
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00005E3E File Offset: 0x0000423E
		public AdjustConfig(string appToken, AdjustEnvironment environment, bool allowSuppressLogLevel)
		{
			this.sceneName = string.Empty;
			this.processName = string.Empty;
			this.appToken = appToken;
			this.environment = environment;
			this.allowSuppressLogLevel = new bool?(allowSuppressLogLevel);
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00005E76 File Offset: 0x00004276
		public void setLogLevel(AdjustLogLevel logLevel)
		{
			this.logLevel = new AdjustLogLevel?(logLevel);
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00005E84 File Offset: 0x00004284
		public void setDefaultTracker(string defaultTracker)
		{
			this.defaultTracker = defaultTracker;
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00005E8D File Offset: 0x0000428D
		public void setLaunchDeferredDeeplink(bool launchDeferredDeeplink)
		{
			this.launchDeferredDeeplink = launchDeferredDeeplink;
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00005E96 File Offset: 0x00004296
		public void setSendInBackground(bool sendInBackground)
		{
			this.sendInBackground = new bool?(sendInBackground);
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00005EA4 File Offset: 0x000042A4
		public void setEventBufferingEnabled(bool eventBufferingEnabled)
		{
			this.eventBufferingEnabled = new bool?(eventBufferingEnabled);
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00005EB2 File Offset: 0x000042B2
		public void setDelayStart(double delayStart)
		{
			this.delayStart = new double?(delayStart);
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00005EC0 File Offset: 0x000042C0
		public void setUserAgent(string userAgent)
		{
			this.userAgent = userAgent;
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00005EC9 File Offset: 0x000042C9
		public void setIsDeviceKnown(bool isDeviceKnown)
		{
			this.isDeviceKnown = new bool?(isDeviceKnown);
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00005ED7 File Offset: 0x000042D7
		public void setDeferredDeeplinkDelegate(Action<string> deferredDeeplinkDelegate, string sceneName = "Adjust")
		{
			this.deferredDeeplinkDelegate = deferredDeeplinkDelegate;
			this.sceneName = sceneName;
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00005EE7 File Offset: 0x000042E7
		public Action<string> getDeferredDeeplinkDelegate()
		{
			return this.deferredDeeplinkDelegate;
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00005EEF File Offset: 0x000042EF
		public void setAttributionChangedDelegate(Action<AdjustAttribution> attributionChangedDelegate, string sceneName = "Adjust")
		{
			this.attributionChangedDelegate = attributionChangedDelegate;
			this.sceneName = sceneName;
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00005EFF File Offset: 0x000042FF
		public Action<AdjustAttribution> getAttributionChangedDelegate()
		{
			return this.attributionChangedDelegate;
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00005F07 File Offset: 0x00004307
		public void setEventSuccessDelegate(Action<AdjustEventSuccess> eventSuccessDelegate, string sceneName = "Adjust")
		{
			this.eventSuccessDelegate = eventSuccessDelegate;
			this.sceneName = sceneName;
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00005F17 File Offset: 0x00004317
		public Action<AdjustEventSuccess> getEventSuccessDelegate()
		{
			return this.eventSuccessDelegate;
		}

		// Token: 0x060000DE RID: 222 RVA: 0x00005F1F File Offset: 0x0000431F
		public void setEventFailureDelegate(Action<AdjustEventFailure> eventFailureDelegate, string sceneName = "Adjust")
		{
			this.eventFailureDelegate = eventFailureDelegate;
			this.sceneName = sceneName;
		}

		// Token: 0x060000DF RID: 223 RVA: 0x00005F2F File Offset: 0x0000432F
		public Action<AdjustEventFailure> getEventFailureDelegate()
		{
			return this.eventFailureDelegate;
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x00005F37 File Offset: 0x00004337
		public void setSessionSuccessDelegate(Action<AdjustSessionSuccess> sessionSuccessDelegate, string sceneName = "Adjust")
		{
			this.sessionSuccessDelegate = sessionSuccessDelegate;
			this.sceneName = sceneName;
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00005F47 File Offset: 0x00004347
		public Action<AdjustSessionSuccess> getSessionSuccessDelegate()
		{
			return this.sessionSuccessDelegate;
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x00005F4F File Offset: 0x0000434F
		public void setSessionFailureDelegate(Action<AdjustSessionFailure> sessionFailureDelegate, string sceneName = "Adjust")
		{
			this.sessionFailureDelegate = sessionFailureDelegate;
			this.sceneName = sceneName;
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x00005F5F File Offset: 0x0000435F
		public Action<AdjustSessionFailure> getSessionFailureDelegate()
		{
			return this.sessionFailureDelegate;
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00005F67 File Offset: 0x00004367
		public void setAppSecret(long secretId, long info1, long info2, long info3, long info4)
		{
			this.secretId = new long?(secretId);
			this.info1 = new long?(info1);
			this.info2 = new long?(info2);
			this.info3 = new long?(info3);
			this.info4 = new long?(info4);
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00005FA7 File Offset: 0x000043A7
		public void setProcessName(string processName)
		{
			this.processName = processName;
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x00005FB0 File Offset: 0x000043B0
		public void setReadMobileEquipmentIdentity(bool readMobileEquipmentIdentity)
		{
			this.readImei = new bool?(readMobileEquipmentIdentity);
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00005FBE File Offset: 0x000043BE
		public void setLogDelegate(Action<string> logDelegate)
		{
			this.logDelegate = logDelegate;
		}

		// Token: 0x04000037 RID: 55
		internal string appToken;

		// Token: 0x04000038 RID: 56
		internal string sceneName;

		// Token: 0x04000039 RID: 57
		internal string userAgent;

		// Token: 0x0400003A RID: 58
		internal string defaultTracker;

		// Token: 0x0400003B RID: 59
		internal bool? isDeviceKnown;

		// Token: 0x0400003C RID: 60
		internal bool? sendInBackground;

		// Token: 0x0400003D RID: 61
		internal bool? eventBufferingEnabled;

		// Token: 0x0400003E RID: 62
		internal bool? allowSuppressLogLevel;

		// Token: 0x0400003F RID: 63
		internal bool launchDeferredDeeplink;

		// Token: 0x04000040 RID: 64
		internal AdjustLogLevel? logLevel;

		// Token: 0x04000041 RID: 65
		internal AdjustEnvironment environment;

		// Token: 0x04000042 RID: 66
		internal Action<string> deferredDeeplinkDelegate;

		// Token: 0x04000043 RID: 67
		internal Action<AdjustEventSuccess> eventSuccessDelegate;

		// Token: 0x04000044 RID: 68
		internal Action<AdjustEventFailure> eventFailureDelegate;

		// Token: 0x04000045 RID: 69
		internal Action<AdjustSessionSuccess> sessionSuccessDelegate;

		// Token: 0x04000046 RID: 70
		internal Action<AdjustSessionFailure> sessionFailureDelegate;

		// Token: 0x04000047 RID: 71
		internal Action<AdjustAttribution> attributionChangedDelegate;

		// Token: 0x04000048 RID: 72
		internal long? info1;

		// Token: 0x04000049 RID: 73
		internal long? info2;

		// Token: 0x0400004A RID: 74
		internal long? info3;

		// Token: 0x0400004B RID: 75
		internal long? info4;

		// Token: 0x0400004C RID: 76
		internal long? secretId;

		// Token: 0x0400004D RID: 77
		internal double? delayStart;

		// Token: 0x0400004E RID: 78
		internal string processName;

		// Token: 0x0400004F RID: 79
		internal bool? readImei;

		// Token: 0x04000050 RID: 80
		internal Action<string> logDelegate;
	}
}
