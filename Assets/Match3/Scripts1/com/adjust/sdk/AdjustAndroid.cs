using System;
using UnityEngine;

namespace Match3.Scripts1.com.adjust.sdk
{
	// Token: 0x0200000A RID: 10
	public class AdjustAndroid
	{
		// Token: 0x06000089 RID: 137 RVA: 0x00004378 File Offset: 0x00002778
		public static void Start(AdjustConfig adjustConfig)
		{
			//AndroidJavaObject androidJavaObject = (adjustConfig.environment != AdjustEnvironment.Sandbox) ? new AndroidJavaClass("com.adjust.sdk.AdjustConfig").GetStatic<AndroidJavaObject>("ENVIRONMENT_PRODUCTION") : new AndroidJavaClass("com.adjust.sdk.AdjustConfig").GetStatic<AndroidJavaObject>("ENVIRONMENT_SANDBOX");
			//bool? allowSuppressLogLevel = adjustConfig.allowSuppressLogLevel;
			//AndroidJavaObject androidJavaObject2;
			//if (allowSuppressLogLevel != null)
			//{
			//	androidJavaObject2 = new AndroidJavaObject("com.adjust.sdk.AdjustConfig", new object[]
			//	{
			//		AdjustAndroid.ajoCurrentActivity,
			//		adjustConfig.appToken,
			//		androidJavaObject,
			//		adjustConfig.allowSuppressLogLevel
			//	});
			//}
			//else
			//{
			//	androidJavaObject2 = new AndroidJavaObject("com.adjust.sdk.AdjustConfig", new object[]
			//	{
			//		AdjustAndroid.ajoCurrentActivity,
			//		adjustConfig.appToken,
			//		androidJavaObject
			//	});
			//}
			//AdjustAndroid.launchDeferredDeeplink = adjustConfig.launchDeferredDeeplink;
			//AdjustLogLevel? logLevel = adjustConfig.logLevel;
			//if (logLevel != null)
			//{
			//	AndroidJavaObject @static;
			//	if (adjustConfig.logLevel.Value.ToUppercaseString().Equals("SUPPRESS"))
			//	{
			//		@static = new AndroidJavaClass("com.adjust.sdk.LogLevel").GetStatic<AndroidJavaObject>("SUPRESS");
			//	}
			//	else
			//	{
			//		@static = new AndroidJavaClass("com.adjust.sdk.LogLevel").GetStatic<AndroidJavaObject>(adjustConfig.logLevel.Value.ToUppercaseString());
			//	}
			//	if (@static != null)
			//	{
			//		androidJavaObject2.Call("setLogLevel", new object[]
			//		{
			//			@static
			//		});
			//	}
			//}
			//double? delayStart = adjustConfig.delayStart;
			//if (delayStart != null)
			//{
			//	androidJavaObject2.Call("setDelayStart", new object[]
			//	{
			//		adjustConfig.delayStart
			//	});
			//}
			//bool? eventBufferingEnabled = adjustConfig.eventBufferingEnabled;
			//if (eventBufferingEnabled != null)
			//{
			//	AndroidJavaObject androidJavaObject3 = new AndroidJavaObject("java.lang.Boolean", new object[]
			//	{
			//		adjustConfig.eventBufferingEnabled.Value
			//	});
			//	androidJavaObject2.Call("setEventBufferingEnabled", new object[]
			//	{
			//		androidJavaObject3
			//	});
			//}
			//bool? sendInBackground = adjustConfig.sendInBackground;
			//if (sendInBackground != null)
			//{
			//	androidJavaObject2.Call("setSendInBackground", new object[]
			//	{
			//		adjustConfig.sendInBackground.Value
			//	});
			//}
			//if (adjustConfig.userAgent != null)
			//{
			//	androidJavaObject2.Call("setUserAgent", new object[]
			//	{
			//		adjustConfig.userAgent
			//	});
			//}
			//if (!string.IsNullOrEmpty(adjustConfig.processName))
			//{
			//	androidJavaObject2.Call("setProcessName", new object[]
			//	{
			//		adjustConfig.processName
			//	});
			//}
			//if (adjustConfig.defaultTracker != null)
			//{
			//	androidJavaObject2.Call("setDefaultTracker", new object[]
			//	{
			//		adjustConfig.defaultTracker
			//	});
			//}
			//if (adjustConfig.attributionChangedDelegate != null)
			//{
			//	AdjustAndroid.onAttributionChangedListener = new AdjustAndroid.AttributionChangeListener(adjustConfig.attributionChangedDelegate);
			//	androidJavaObject2.Call("setOnAttributionChangedListener", new object[]
			//	{
			//		AdjustAndroid.onAttributionChangedListener
			//	});
			//}
			//if (adjustConfig.eventSuccessDelegate != null)
			//{
			//	AdjustAndroid.onEventTrackingSucceededListener = new AdjustAndroid.EventTrackingSucceededListener(adjustConfig.eventSuccessDelegate);
			//	androidJavaObject2.Call("setOnEventTrackingSucceededListener", new object[]
			//	{
			//		AdjustAndroid.onEventTrackingSucceededListener
			//	});
			//}
			//if (adjustConfig.eventFailureDelegate != null)
			//{
			//	AdjustAndroid.onEventTrackingFailedListener = new AdjustAndroid.EventTrackingFailedListener(adjustConfig.eventFailureDelegate);
			//	androidJavaObject2.Call("setOnEventTrackingFailedListener", new object[]
			//	{
			//		AdjustAndroid.onEventTrackingFailedListener
			//	});
			//}
			//if (adjustConfig.sessionSuccessDelegate != null)
			//{
			//	AdjustAndroid.onSessionTrackingSucceededListener = new AdjustAndroid.SessionTrackingSucceededListener(adjustConfig.sessionSuccessDelegate);
			//	androidJavaObject2.Call("setOnSessionTrackingSucceededListener", new object[]
			//	{
			//		AdjustAndroid.onSessionTrackingSucceededListener
			//	});
			//}
			//if (adjustConfig.sessionFailureDelegate != null)
			//{
			//	AdjustAndroid.onSessionTrackingFailedListener = new AdjustAndroid.SessionTrackingFailedListener(adjustConfig.sessionFailureDelegate);
			//	androidJavaObject2.Call("setOnSessionTrackingFailedListener", new object[]
			//	{
			//		AdjustAndroid.onSessionTrackingFailedListener
			//	});
			//}
			//if (adjustConfig.deferredDeeplinkDelegate != null)
			//{
			//	AdjustAndroid.onDeferredDeeplinkListener = new AdjustAndroid.DeferredDeeplinkListener(adjustConfig.deferredDeeplinkDelegate);
			//	androidJavaObject2.Call("setOnDeeplinkResponseListener", new object[]
			//	{
			//		AdjustAndroid.onDeferredDeeplinkListener
			//	});
			//}
			//androidJavaObject2.Call("setSdkPrefix", new object[]
			//{
			//	"unity4.12.3"
			//});
			//if (AdjustAndroid.IsAppSecretSet(adjustConfig))
			//{
			//	androidJavaObject2.Call("setAppSecret", new object[]
			//	{
			//		adjustConfig.secretId.Value,
			//		adjustConfig.info1.Value,
			//		adjustConfig.info2.Value,
			//		adjustConfig.info3.Value,
			//		adjustConfig.info4.Value
			//	});
			//}
			//if (adjustConfig.isDeviceKnown != null)
			//{
			//	androidJavaObject2.Call("setDeviceKnown", new object[]
			//	{
			//		adjustConfig.isDeviceKnown.Value
			//	});
			//}
			//if (adjustConfig.readImei != null)
			//{
			//	androidJavaObject2.Call("setReadMobileEquipmentIdentity", new object[]
			//	{
			//		adjustConfig.readImei.Value
			//	});
			//}
			//AdjustAndroid.ajcAdjust.CallStatic("onCreate", new object[]
			//{
			//	androidJavaObject2
			//});
			//AdjustAndroid.ajcAdjust.CallStatic("onResume", new object[0]);
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00004858 File Offset: 0x00002C58
		public static void TrackEvent(AdjustEvent adjustEvent)
		{
			//AndroidJavaObject androidJavaObject = new AndroidJavaObject("com.adjust.sdk.AdjustEvent", new object[]
			//{
			//	adjustEvent.eventToken
			//});
			double? revenue = adjustEvent.revenue;
			if (revenue != null)
			{
				//AndroidJavaObject androidJavaObject2 = androidJavaObject;
				string methodName = "setRevenue";
				object[] array = new object[2];
				int num = 0;
				double? revenue2 = adjustEvent.revenue;
				array[num] = revenue2.Value;
				array[1] = adjustEvent.currency;
				//androidJavaObject2.Call(methodName, array);
			}
			if (adjustEvent.callbackList != null)
			{
				for (int i = 0; i < adjustEvent.callbackList.Count; i += 2)
				{
					string text = adjustEvent.callbackList[i];
					string text2 = adjustEvent.callbackList[i + 1];
					//androidJavaObject.Call("addCallbackParameter", new object[]
					//{
					//	text,
					//	text2
					//});
				}
			}
			if (adjustEvent.partnerList != null)
			{
				for (int j = 0; j < adjustEvent.partnerList.Count; j += 2)
				{
					string text3 = adjustEvent.partnerList[j];
					string text4 = adjustEvent.partnerList[j + 1];
					//androidJavaObject.Call("addPartnerParameter", new object[]
					//{
					//	text3,
					//	text4
					//});
				}
			}
			if (adjustEvent.transactionId != null)
			{
				//androidJavaObject.Call("setOrderId", new object[]
				//{
				//	adjustEvent.transactionId
				//});
			}
			//AdjustAndroid.ajcAdjust.CallStatic("trackEvent", new object[]
			//{
			//	androidJavaObject
			//});
		}

		// Token: 0x0600008B RID: 139 RVA: 0x000049C6 File Offset: 0x00002DC6
		public static bool IsEnabled()
		{
			return AdjustAndroid.ajcAdjust.CallStatic<bool>("isEnabled", new object[0]);
		}

		// Token: 0x0600008C RID: 140 RVA: 0x000049DD File Offset: 0x00002DDD
		public static void SetEnabled(bool enabled)
		{
			AdjustAndroid.ajcAdjust.CallStatic("setEnabled", new object[]
			{
				enabled
			});
		}

		// Token: 0x0600008D RID: 141 RVA: 0x000049FD File Offset: 0x00002DFD
		public static void SetOfflineMode(bool enabled)
		{
			AdjustAndroid.ajcAdjust.CallStatic("setOfflineMode", new object[]
			{
				enabled
			});
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00004A1D File Offset: 0x00002E1D
		public static void SendFirstPackages()
		{
			AdjustAndroid.ajcAdjust.CallStatic("sendFirstPackages", new object[0]);
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00004A34 File Offset: 0x00002E34
		public static void SetDeviceToken(string deviceToken)
		{
			AdjustAndroid.ajcAdjust.CallStatic("setPushToken", new object[]
			{
				deviceToken
			});
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00004A4F File Offset: 0x00002E4F
		public static string GetAdid()
		{
			return AdjustAndroid.ajcAdjust.CallStatic<string>("getAdid", new object[0]);
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00004A68 File Offset: 0x00002E68
		public static AdjustAttribution GetAttribution()
		{
			try
			{
				//AndroidJavaObject androidJavaObject = AdjustAndroid.ajcAdjust.CallStatic<AndroidJavaObject>("getAttribution", new object[0]);
				//if (androidJavaObject == null)
				//{
				//	return null;
				//}
				//return new AdjustAttribution
				//{
				//	trackerName = androidJavaObject.Get<string>(AdjustUtils.KeyTrackerName),
				//	trackerToken = androidJavaObject.Get<string>(AdjustUtils.KeyTrackerToken),
				//	network = androidJavaObject.Get<string>(AdjustUtils.KeyNetwork),
				//	campaign = androidJavaObject.Get<string>(AdjustUtils.KeyCampaign),
				//	adgroup = androidJavaObject.Get<string>(AdjustUtils.KeyAdgroup),
				//	creative = androidJavaObject.Get<string>(AdjustUtils.KeyCreative),
				//	clickLabel = androidJavaObject.Get<string>(AdjustUtils.KeyClickLabel),
				//	adid = androidJavaObject.Get<string>(AdjustUtils.KeyAdid)
				//};
			}
			catch (Exception)
			{
			}
			return null;
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00004B48 File Offset: 0x00002F48
		public static void AddSessionPartnerParameter(string key, string value)
		{
			if (AdjustAndroid.ajcAdjust == null)
			{
				// AdjustAndroid.ajcAdjust = new AndroidJavaClass("com.adjust.sdk.Adjust");
			}
			AdjustAndroid.ajcAdjust.CallStatic("addSessionPartnerParameter", new object[]
			{
				key,
				value
			});
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00004B80 File Offset: 0x00002F80
		public static void AddSessionCallbackParameter(string key, string value)
		{
			if (AdjustAndroid.ajcAdjust == null)
			{
				// AdjustAndroid.ajcAdjust = new AndroidJavaClass("com.adjust.sdk.Adjust");
			}
			AdjustAndroid.ajcAdjust.CallStatic("addSessionCallbackParameter", new object[]
			{
				key,
				value
			});
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00004BB8 File Offset: 0x00002FB8
		public static void RemoveSessionPartnerParameter(string key)
		{
			if (AdjustAndroid.ajcAdjust == null)
			{
				// AdjustAndroid.ajcAdjust = new AndroidJavaClass("com.adjust.sdk.Adjust");
			}
			AdjustAndroid.ajcAdjust.CallStatic("removeSessionPartnerParameter", new object[]
			{
				key
			});
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00004BEC File Offset: 0x00002FEC
		public static void RemoveSessionCallbackParameter(string key)
		{
			if (AdjustAndroid.ajcAdjust == null)
			{
				// AdjustAndroid.ajcAdjust = new AndroidJavaClass("com.adjust.sdk.Adjust");
			}
			AdjustAndroid.ajcAdjust.CallStatic("removeSessionCallbackParameter", new object[]
			{
				key
			});
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00004C20 File Offset: 0x00003020
		public static void ResetSessionPartnerParameters()
		{
			if (AdjustAndroid.ajcAdjust == null)
			{
				// AdjustAndroid.ajcAdjust = new AndroidJavaClass("com.adjust.sdk.Adjust");
			}
			AdjustAndroid.ajcAdjust.CallStatic("resetSessionPartnerParameters", new object[0]);
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00004C50 File Offset: 0x00003050
		public static void ResetSessionCallbackParameters()
		{
			if (AdjustAndroid.ajcAdjust == null)
			{
				// AdjustAndroid.ajcAdjust = new AndroidJavaClass("com.adjust.sdk.Adjust");
			}
			AdjustAndroid.ajcAdjust.CallStatic("resetSessionCallbackParameters", new object[0]);
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00004C80 File Offset: 0x00003080
		public static void OnPause()
		{
			AdjustAndroid.ajcAdjust.CallStatic("onPause", new object[0]);
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00004C97 File Offset: 0x00003097
		public static void OnResume()
		{
			AdjustAndroid.ajcAdjust.CallStatic("onResume", new object[0]);
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00004CAE File Offset: 0x000030AE
		public static void SetReferrer(string referrer)
		{
			AdjustAndroid.ajcAdjust.CallStatic("setReferrer", new object[]
			{
				referrer
			});
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00004CCC File Offset: 0x000030CC
		public static void GetGoogleAdId(Action<string> onDeviceIdsRead)
		{
			AdjustAndroid.DeviceIdsReadListener deviceIdsReadListener = new AdjustAndroid.DeviceIdsReadListener(onDeviceIdsRead);
			AdjustAndroid.ajcAdjust.CallStatic("getGoogleAdId", new object[]
			{
				AdjustAndroid.ajoCurrentActivity,
				deviceIdsReadListener
			});
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00004D04 File Offset: 0x00003104
		public static void AppWillOpenUrl(string url)
		{
			// AndroidJavaClass androidJavaClass = new AndroidJavaClass("android.net.Uri");
			// AndroidJavaObject androidJavaObject = androidJavaClass.CallStatic<AndroidJavaObject>("parse", new object[]
			// {
				// url
			// });
			// AdjustAndroid.ajcAdjust.CallStatic("appWillOpenUrl", new object[]
			// {
				// androidJavaObject
			// });
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00004D4B File Offset: 0x0000314B
		public static string GetAmazonAdId()
		{
			return AdjustAndroid.ajcAdjust.CallStatic<string>("getAmazonAdId", new object[]
			{
				AdjustAndroid.ajoCurrentActivity
			});
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00004D6C File Offset: 0x0000316C
		private static bool IsAppSecretSet(AdjustConfig adjustConfig)
		{
			return adjustConfig.secretId != null && adjustConfig.info1 != null && adjustConfig.info2 != null && adjustConfig.info3 != null && adjustConfig.info4 != null;
		}

		// Token: 0x04000017 RID: 23
		private const string sdkPrefix = "unity4.12.3";

		// Token: 0x04000018 RID: 24
		private static bool launchDeferredDeeplink = true;

		// Token: 0x04000019 RID: 25
		// private static AndroidJavaClass ajcAdjust = new AndroidJavaClass("com.adjust.sdk.Adjust");
		private static AndroidJavaClass ajcAdjust = null;

		// Token: 0x0400001A RID: 26
		// private static AndroidJavaObject ajoCurrentActivity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
		private static AndroidJavaObject ajoCurrentActivity = null;

		// Token: 0x0400001B RID: 27
		private static AdjustAndroid.DeferredDeeplinkListener onDeferredDeeplinkListener;

		// Token: 0x0400001C RID: 28
		private static AdjustAndroid.AttributionChangeListener onAttributionChangedListener;

		// Token: 0x0400001D RID: 29
		private static AdjustAndroid.EventTrackingFailedListener onEventTrackingFailedListener;

		// Token: 0x0400001E RID: 30
		private static AdjustAndroid.EventTrackingSucceededListener onEventTrackingSucceededListener;

		// Token: 0x0400001F RID: 31
		private static AdjustAndroid.SessionTrackingFailedListener onSessionTrackingFailedListener;

		// Token: 0x04000020 RID: 32
		private static AdjustAndroid.SessionTrackingSucceededListener onSessionTrackingSucceededListener;

		// Token: 0x0200000B RID: 11
		private class AttributionChangeListener : AndroidJavaProxy
		{
			// Token: 0x060000A0 RID: 160 RVA: 0x00004DF7 File Offset: 0x000031F7
			public AttributionChangeListener(Action<AdjustAttribution> pCallback) : base("com.adjust.sdk.OnAttributionChangedListener")
			{
				this.callback = pCallback;
			}

			// Token: 0x060000A1 RID: 161 RVA: 0x00004E0C File Offset: 0x0000320C
			public void onAttributionChanged(AndroidJavaObject attribution)
			{
				if (this.callback == null)
				{
					return;
				}
				AdjustAttribution adjustAttribution = new AdjustAttribution();
				adjustAttribution.trackerName = attribution.Get<string>(AdjustUtils.KeyTrackerName);
				adjustAttribution.trackerToken = attribution.Get<string>(AdjustUtils.KeyTrackerToken);
				adjustAttribution.network = attribution.Get<string>(AdjustUtils.KeyNetwork);
				adjustAttribution.campaign = attribution.Get<string>(AdjustUtils.KeyCampaign);
				adjustAttribution.adgroup = attribution.Get<string>(AdjustUtils.KeyAdgroup);
				adjustAttribution.creative = attribution.Get<string>(AdjustUtils.KeyCreative);
				adjustAttribution.clickLabel = attribution.Get<string>(AdjustUtils.KeyClickLabel);
				adjustAttribution.adid = attribution.Get<string>(AdjustUtils.KeyAdid);
				this.callback(adjustAttribution);
			}

			// Token: 0x04000021 RID: 33
			private Action<AdjustAttribution> callback;
		}

		// Token: 0x0200000C RID: 12
		private class DeferredDeeplinkListener : AndroidJavaProxy
		{
			// Token: 0x060000A2 RID: 162 RVA: 0x00004EBF File Offset: 0x000032BF
			public DeferredDeeplinkListener(Action<string> pCallback) : base("com.adjust.sdk.OnDeeplinkResponseListener")
			{
				this.callback = pCallback;
			}

			// Token: 0x060000A3 RID: 163 RVA: 0x00004ED4 File Offset: 0x000032D4
			public bool launchReceivedDeeplink(AndroidJavaObject deeplink)
			{
				if (this.callback == null)
				{
					return AdjustAndroid.launchDeferredDeeplink;
				}
				string obj = deeplink.Call<string>("toString", new object[0]);
				this.callback(obj);
				return AdjustAndroid.launchDeferredDeeplink;
			}

			// Token: 0x04000022 RID: 34
			private Action<string> callback;
		}

		// Token: 0x0200000D RID: 13
		private class EventTrackingSucceededListener : AndroidJavaProxy
		{
			// Token: 0x060000A4 RID: 164 RVA: 0x00004F15 File Offset: 0x00003315
			public EventTrackingSucceededListener(Action<AdjustEventSuccess> pCallback) : base("com.adjust.sdk.OnEventTrackingSucceededListener")
			{
				this.callback = pCallback;
			}

			// Token: 0x060000A5 RID: 165 RVA: 0x00004F2C File Offset: 0x0000332C
			public void onFinishedEventTrackingSucceeded(AndroidJavaObject eventSuccessData)
			{
				if (this.callback == null)
				{
					return;
				}
				if (eventSuccessData == null)
				{
					return;
				}
				AdjustEventSuccess adjustEventSuccess = new AdjustEventSuccess();
				adjustEventSuccess.Adid = eventSuccessData.Get<string>(AdjustUtils.KeyAdid);
				adjustEventSuccess.Message = eventSuccessData.Get<string>(AdjustUtils.KeyMessage);
				adjustEventSuccess.Timestamp = eventSuccessData.Get<string>(AdjustUtils.KeyTimestamp);
				adjustEventSuccess.EventToken = eventSuccessData.Get<string>(AdjustUtils.KeyEventToken);
				try
				{
					//AndroidJavaObject androidJavaObject = eventSuccessData.Get<AndroidJavaObject>(AdjustUtils.KeyJsonResponse);
					//string jsonResponseString = androidJavaObject.Call<string>("toString", new object[0]);
					//adjustEventSuccess.BuildJsonResponseFromString(jsonResponseString);
				}
				catch (Exception)
				{
				}
				this.callback(adjustEventSuccess);
			}

			// Token: 0x04000023 RID: 35
			private Action<AdjustEventSuccess> callback;
		}

		// Token: 0x0200000E RID: 14
		private class EventTrackingFailedListener : AndroidJavaProxy
		{
			// Token: 0x060000A6 RID: 166 RVA: 0x00004FE4 File Offset: 0x000033E4
			public EventTrackingFailedListener(Action<AdjustEventFailure> pCallback) : base("com.adjust.sdk.OnEventTrackingFailedListener")
			{
				this.callback = pCallback;
			}

			// Token: 0x060000A7 RID: 167 RVA: 0x00004FF8 File Offset: 0x000033F8
			public void onFinishedEventTrackingFailed(AndroidJavaObject eventFailureData)
			{
				if (this.callback == null)
				{
					return;
				}
				if (eventFailureData == null)
				{
					return;
				}
				AdjustEventFailure adjustEventFailure = new AdjustEventFailure();
				adjustEventFailure.Adid = eventFailureData.Get<string>(AdjustUtils.KeyAdid);
				adjustEventFailure.Message = eventFailureData.Get<string>(AdjustUtils.KeyMessage);
				adjustEventFailure.WillRetry = eventFailureData.Get<bool>(AdjustUtils.KeyWillRetry);
				adjustEventFailure.Timestamp = eventFailureData.Get<string>(AdjustUtils.KeyTimestamp);
				adjustEventFailure.EventToken = eventFailureData.Get<string>(AdjustUtils.KeyEventToken);
				try
				{
					//AndroidJavaObject androidJavaObject = eventFailureData.Get<AndroidJavaObject>(AdjustUtils.KeyJsonResponse);
					//string jsonResponseString = androidJavaObject.Call<string>("toString", new object[0]);
					//adjustEventFailure.BuildJsonResponseFromString(jsonResponseString);
				}
				catch (Exception)
				{
				}
				this.callback(adjustEventFailure);
			}

			// Token: 0x04000024 RID: 36
			private Action<AdjustEventFailure> callback;
		}

		// Token: 0x0200000F RID: 15
		private class SessionTrackingSucceededListener : AndroidJavaProxy
		{
			// Token: 0x060000A8 RID: 168 RVA: 0x000050C0 File Offset: 0x000034C0
			public SessionTrackingSucceededListener(Action<AdjustSessionSuccess> pCallback) : base("com.adjust.sdk.OnSessionTrackingSucceededListener")
			{
				this.callback = pCallback;
			}

			// Token: 0x060000A9 RID: 169 RVA: 0x000050D4 File Offset: 0x000034D4
			public void onFinishedSessionTrackingSucceeded(AndroidJavaObject sessionSuccessData)
			{
				if (this.callback == null)
				{
					return;
				}
				if (sessionSuccessData == null)
				{
					return;
				}
				AdjustSessionSuccess adjustSessionSuccess = new AdjustSessionSuccess();
				adjustSessionSuccess.Adid = sessionSuccessData.Get<string>(AdjustUtils.KeyAdid);
				adjustSessionSuccess.Message = sessionSuccessData.Get<string>(AdjustUtils.KeyMessage);
				adjustSessionSuccess.Timestamp = sessionSuccessData.Get<string>(AdjustUtils.KeyTimestamp);
				try
				{
					//AndroidJavaObject androidJavaObject = sessionSuccessData.Get<AndroidJavaObject>(AdjustUtils.KeyJsonResponse);
					//string jsonResponseString = androidJavaObject.Call<string>("toString", new object[0]);
					//adjustSessionSuccess.BuildJsonResponseFromString(jsonResponseString);
				}
				catch (Exception)
				{
				}
				this.callback(adjustSessionSuccess);
			}

			// Token: 0x04000025 RID: 37
			private Action<AdjustSessionSuccess> callback;
		}

		// Token: 0x02000010 RID: 16
		private class SessionTrackingFailedListener : AndroidJavaProxy
		{
			// Token: 0x060000AA RID: 170 RVA: 0x0000517C File Offset: 0x0000357C
			public SessionTrackingFailedListener(Action<AdjustSessionFailure> pCallback) : base("com.adjust.sdk.OnSessionTrackingFailedListener")
			{
				this.callback = pCallback;
			}

			// Token: 0x060000AB RID: 171 RVA: 0x00005190 File Offset: 0x00003590
			public void onFinishedSessionTrackingFailed(AndroidJavaObject sessionFailureData)
			{
				if (this.callback == null)
				{
					return;
				}
				if (sessionFailureData == null)
				{
					return;
				}
				AdjustSessionFailure adjustSessionFailure = new AdjustSessionFailure();
				adjustSessionFailure.Adid = sessionFailureData.Get<string>(AdjustUtils.KeyAdid);
				adjustSessionFailure.Message = sessionFailureData.Get<string>(AdjustUtils.KeyMessage);
				adjustSessionFailure.WillRetry = sessionFailureData.Get<bool>(AdjustUtils.KeyWillRetry);
				adjustSessionFailure.Timestamp = sessionFailureData.Get<string>(AdjustUtils.KeyTimestamp);
				try
				{
					//AndroidJavaObject androidJavaObject = sessionFailureData.Get<AndroidJavaObject>(AdjustUtils.KeyJsonResponse);
					//string jsonResponseString = androidJavaObject.Call<string>("toString", new object[0]);
					//adjustSessionFailure.BuildJsonResponseFromString(jsonResponseString);
				}
				catch (Exception)
				{
				}
				this.callback(adjustSessionFailure);
			}

			// Token: 0x04000026 RID: 38
			private Action<AdjustSessionFailure> callback;
		}

		// Token: 0x02000011 RID: 17
		private class DeviceIdsReadListener : AndroidJavaProxy
		{
			// Token: 0x060000AC RID: 172 RVA: 0x00005248 File Offset: 0x00003648
			public DeviceIdsReadListener(Action<string> pCallback) : base("com.adjust.sdk.OnDeviceIdsRead")
			{
				this.onPlayAdIdReadCallback = pCallback;
			}

			// Token: 0x060000AD RID: 173 RVA: 0x0000525C File Offset: 0x0000365C
			public void onGoogleAdIdRead(string playAdId)
			{
				if (this.onPlayAdIdReadCallback == null)
				{
					return;
				}
				this.onPlayAdIdReadCallback(playAdId);
			}

			// Token: 0x060000AE RID: 174 RVA: 0x00005278 File Offset: 0x00003678
			public void onGoogleAdIdRead(AndroidJavaObject ajoAdId)
			{
				if (ajoAdId == null)
				{
					string playAdId = null;
					this.onGoogleAdIdRead(playAdId);
					return;
				}
				this.onGoogleAdIdRead(ajoAdId.Call<string>("toString", new object[0]));
			}

			// Token: 0x04000027 RID: 39
			private Action<string> onPlayAdIdReadCallback;
		}
	}
}
