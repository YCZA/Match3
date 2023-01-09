using System;
using Match3.Scripts1.com.adjust.sdk;
using UnityEngine;

// Token: 0x02000012 RID: 18
namespace Match3.Scripts1
{
	public class ExampleGUI : MonoBehaviour
	{
		// Token: 0x060000B0 RID: 176 RVA: 0x000052DC File Offset: 0x000036DC
		private void OnGUI()
		{
			if (this.showPopUp)
			{
				GUI.Window(0, new Rect((float)(Screen.width / 2 - 150), (float)(Screen.height / 2 - 65), 300f, 130f), new GUI.WindowFunction(this.showGUI), "Is SDK enabled?");
			}
			if (GUI.Button(new Rect(0f, (float)(Screen.height * 0 / this.numberOfButtons), (float)Screen.width, (float)(Screen.height / this.numberOfButtons)), this.txtManualLaunch) && !string.Equals(this.txtManualLaunch, "SDK Launched", StringComparison.OrdinalIgnoreCase))
			{
				AdjustConfig adjustConfig = new AdjustConfig("2fm9gkqubvpc", AdjustEnvironment.Sandbox);
				adjustConfig.setLogLevel(AdjustLogLevel.Verbose);
				adjustConfig.setLogDelegate(delegate(string msg)
				{
					global::UnityEngine.Debug.Log(msg);
				});
				adjustConfig.setSendInBackground(true);
				adjustConfig.setLaunchDeferredDeeplink(true);
				adjustConfig.setEventSuccessDelegate(new Action<AdjustEventSuccess>(this.EventSuccessCallback), "Adjust");
				adjustConfig.setEventFailureDelegate(new Action<AdjustEventFailure>(this.EventFailureCallback), "Adjust");
				adjustConfig.setSessionSuccessDelegate(new Action<AdjustSessionSuccess>(this.SessionSuccessCallback), "Adjust");
				adjustConfig.setSessionFailureDelegate(new Action<AdjustSessionFailure>(this.SessionFailureCallback), "Adjust");
				adjustConfig.setDeferredDeeplinkDelegate(new Action<string>(this.DeferredDeeplinkCallback), "Adjust");
				adjustConfig.setAttributionChangedDelegate(new Action<AdjustAttribution>(this.AttributionChangedCallback), "Adjust");
				Adjust.start(adjustConfig);
				this.isEnabled = true;
				this.txtManualLaunch = "SDK Launched";
			}
			if (GUI.Button(new Rect(0f, (float)(Screen.height / this.numberOfButtons), (float)Screen.width, (float)(Screen.height / this.numberOfButtons)), "Track Simple Event"))
			{
				AdjustEvent adjustEvent = new AdjustEvent("g3mfiw");
				Adjust.trackEvent(adjustEvent);
			}
			if (GUI.Button(new Rect(0f, (float)(Screen.height * 2 / this.numberOfButtons), (float)Screen.width, (float)(Screen.height / this.numberOfButtons)), "Track Revenue Event"))
			{
				AdjustEvent adjustEvent2 = new AdjustEvent("a4fd35");
				adjustEvent2.setRevenue(0.25, "EUR");
				Adjust.trackEvent(adjustEvent2);
			}
			if (GUI.Button(new Rect(0f, (float)(Screen.height * 3 / this.numberOfButtons), (float)Screen.width, (float)(Screen.height / this.numberOfButtons)), "Track Callback Event"))
			{
				AdjustEvent adjustEvent3 = new AdjustEvent("34vgg9");
				adjustEvent3.addCallbackParameter("key", "value");
				adjustEvent3.addCallbackParameter("foo", "bar");
				Adjust.trackEvent(adjustEvent3);
			}
			if (GUI.Button(new Rect(0f, (float)(Screen.height * 4 / this.numberOfButtons), (float)Screen.width, (float)(Screen.height / this.numberOfButtons)), "Track Partner Event"))
			{
				AdjustEvent adjustEvent4 = new AdjustEvent("w788qs");
				adjustEvent4.addPartnerParameter("key", "value");
				adjustEvent4.addPartnerParameter("foo", "bar");
				Adjust.trackEvent(adjustEvent4);
			}
			if (GUI.Button(new Rect(0f, (float)(Screen.height * 5 / this.numberOfButtons), (float)Screen.width, (float)(Screen.height / this.numberOfButtons)), this.txtSetOfflineMode))
			{
				if (string.Equals(this.txtSetOfflineMode, "Turn Offline Mode ON", StringComparison.OrdinalIgnoreCase))
				{
					Adjust.setOfflineMode(true);
					this.txtSetOfflineMode = "Turn Offline Mode OFF";
				}
				else
				{
					Adjust.setOfflineMode(false);
					this.txtSetOfflineMode = "Turn Offline Mode ON";
				}
			}
			if (GUI.Button(new Rect(0f, (float)(Screen.height * 6 / this.numberOfButtons), (float)Screen.width, (float)(Screen.height / this.numberOfButtons)), this.txtSetEnabled))
			{
				if (string.Equals(this.txtSetEnabled, "Disable SDK", StringComparison.OrdinalIgnoreCase))
				{
					Adjust.setEnabled(false);
					this.txtSetEnabled = "Enable SDK";
				}
				else
				{
					Adjust.setEnabled(true);
					this.txtSetEnabled = "Disable SDK";
				}
			}
			if (GUI.Button(new Rect(0f, (float)(Screen.height * 7 / this.numberOfButtons), (float)Screen.width, (float)(Screen.height / this.numberOfButtons)), "Is SDK Enabled?"))
			{
				this.isEnabled = Adjust.isEnabled();
				this.showPopUp = true;
			}
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00005734 File Offset: 0x00003B34
		private void showGUI(int windowID)
		{
			if (this.isEnabled)
			{
				GUI.Label(new Rect(65f, 40f, 200f, 30f), "Adjust SDK is ENABLED!");
			}
			else
			{
				GUI.Label(new Rect(65f, 40f, 200f, 30f), "Adjust SDK is DISABLED!");
			}
			if (GUI.Button(new Rect(90f, 75f, 120f, 40f), "OK"))
			{
				this.showPopUp = false;
			}
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x000057C6 File Offset: 0x00003BC6
		public void handleGooglePlayId(string adId)
		{
			global::UnityEngine.Debug.Log("Google Play Ad ID = " + adId);
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x000057D8 File Offset: 0x00003BD8
		public void AttributionChangedCallback(AdjustAttribution attributionData)
		{
			global::UnityEngine.Debug.Log("Attribution changed!");
			if (attributionData.trackerName != null)
			{
				global::UnityEngine.Debug.Log("Tracker name: " + attributionData.trackerName);
			}
			if (attributionData.trackerToken != null)
			{
				global::UnityEngine.Debug.Log("Tracker token: " + attributionData.trackerToken);
			}
			if (attributionData.network != null)
			{
				global::UnityEngine.Debug.Log("Network: " + attributionData.network);
			}
			if (attributionData.campaign != null)
			{
				global::UnityEngine.Debug.Log("Campaign: " + attributionData.campaign);
			}
			if (attributionData.adgroup != null)
			{
				global::UnityEngine.Debug.Log("Adgroup: " + attributionData.adgroup);
			}
			if (attributionData.creative != null)
			{
				global::UnityEngine.Debug.Log("Creative: " + attributionData.creative);
			}
			if (attributionData.clickLabel != null)
			{
				global::UnityEngine.Debug.Log("Click label: " + attributionData.clickLabel);
			}
			if (attributionData.adid != null)
			{
				global::UnityEngine.Debug.Log("ADID: " + attributionData.adid);
			}
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x000058F0 File Offset: 0x00003CF0
		public void EventSuccessCallback(AdjustEventSuccess eventSuccessData)
		{
			global::UnityEngine.Debug.Log("Event tracked successfully!");
			if (eventSuccessData.Message != null)
			{
				global::UnityEngine.Debug.Log("Message: " + eventSuccessData.Message);
			}
			if (eventSuccessData.Timestamp != null)
			{
				global::UnityEngine.Debug.Log("Timestamp: " + eventSuccessData.Timestamp);
			}
			if (eventSuccessData.Adid != null)
			{
				global::UnityEngine.Debug.Log("Adid: " + eventSuccessData.Adid);
			}
			if (eventSuccessData.EventToken != null)
			{
				global::UnityEngine.Debug.Log("EventToken: " + eventSuccessData.EventToken);
			}
			if (eventSuccessData.JsonResponse != null)
			{
				global::UnityEngine.Debug.Log("JsonResponse: " + eventSuccessData.GetJsonResponse());
			}
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x000059A8 File Offset: 0x00003DA8
		public void EventFailureCallback(AdjustEventFailure eventFailureData)
		{
			global::UnityEngine.Debug.Log("Event tracking failed!");
			if (eventFailureData.Message != null)
			{
				global::UnityEngine.Debug.Log("Message: " + eventFailureData.Message);
			}
			if (eventFailureData.Timestamp != null)
			{
				global::UnityEngine.Debug.Log("Timestamp: " + eventFailureData.Timestamp);
			}
			if (eventFailureData.Adid != null)
			{
				global::UnityEngine.Debug.Log("Adid: " + eventFailureData.Adid);
			}
			if (eventFailureData.EventToken != null)
			{
				global::UnityEngine.Debug.Log("EventToken: " + eventFailureData.EventToken);
			}
			global::UnityEngine.Debug.Log("WillRetry: " + eventFailureData.WillRetry.ToString());
			if (eventFailureData.JsonResponse != null)
			{
				global::UnityEngine.Debug.Log("JsonResponse: " + eventFailureData.GetJsonResponse());
			}
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00005A84 File Offset: 0x00003E84
		public void SessionSuccessCallback(AdjustSessionSuccess sessionSuccessData)
		{
			global::UnityEngine.Debug.Log("Session tracked successfully!");
			if (sessionSuccessData.Message != null)
			{
				global::UnityEngine.Debug.Log("Message: " + sessionSuccessData.Message);
			}
			if (sessionSuccessData.Timestamp != null)
			{
				global::UnityEngine.Debug.Log("Timestamp: " + sessionSuccessData.Timestamp);
			}
			if (sessionSuccessData.Adid != null)
			{
				global::UnityEngine.Debug.Log("Adid: " + sessionSuccessData.Adid);
			}
			if (sessionSuccessData.JsonResponse != null)
			{
				global::UnityEngine.Debug.Log("JsonResponse: " + sessionSuccessData.GetJsonResponse());
			}
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00005B1C File Offset: 0x00003F1C
		public void SessionFailureCallback(AdjustSessionFailure sessionFailureData)
		{
			global::UnityEngine.Debug.Log("Session tracking failed!");
			if (sessionFailureData.Message != null)
			{
				global::UnityEngine.Debug.Log("Message: " + sessionFailureData.Message);
			}
			if (sessionFailureData.Timestamp != null)
			{
				global::UnityEngine.Debug.Log("Timestamp: " + sessionFailureData.Timestamp);
			}
			if (sessionFailureData.Adid != null)
			{
				global::UnityEngine.Debug.Log("Adid: " + sessionFailureData.Adid);
			}
			global::UnityEngine.Debug.Log("WillRetry: " + sessionFailureData.WillRetry.ToString());
			if (sessionFailureData.JsonResponse != null)
			{
				global::UnityEngine.Debug.Log("JsonResponse: " + sessionFailureData.GetJsonResponse());
			}
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00005BD6 File Offset: 0x00003FD6
		private void DeferredDeeplinkCallback(string deeplinkURL)
		{
			global::UnityEngine.Debug.Log("Deferred deeplink reported!");
			if (deeplinkURL != null)
			{
				global::UnityEngine.Debug.Log("Deeplink URL: " + deeplinkURL);
			}
			else
			{
				global::UnityEngine.Debug.Log("Deeplink URL is null!");
			}
		}

		// Token: 0x04000028 RID: 40
		private int numberOfButtons = 8;

		// Token: 0x04000029 RID: 41
		private bool isEnabled;

		// Token: 0x0400002A RID: 42
		private bool showPopUp;

		// Token: 0x0400002B RID: 43
		private string txtSetEnabled = "Disable SDK";

		// Token: 0x0400002C RID: 44
		private string txtManualLaunch = "Manual Launch";

		// Token: 0x0400002D RID: 45
		private string txtSetOfflineMode = "Turn Offline Mode ON";
	}
}
