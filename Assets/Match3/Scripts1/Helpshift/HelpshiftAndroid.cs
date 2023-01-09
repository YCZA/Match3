using System;
using System.Collections.Generic;
using Match3.Scripts1.HSMiniJSON;
using UnityEngine;

namespace Match3.Scripts1.Helpshift
{
	// Token: 0x020001C3 RID: 451
	public class HelpshiftAndroid : IWorkerMethodDispatcher, IDexLoaderListener
	{
		// Token: 0x06000CB8 RID: 3256 RVA: 0x0001E02C File Offset: 0x0001C42C
		public HelpshiftAndroid()
		{
			// this.jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			// this.currentActivity = this.jc.GetStatic<AndroidJavaObject>("currentActivity");
			// this.application = this.currentActivity.Call<AndroidJavaObject>("getApplication", new object[0]);
			// this.hsUnityAPIDelegate = new AndroidJavaClass("com.helpshift.supportCampaigns.UnityAPIDelegate");
			// HelpshiftWorker.getInstance().registerClient("support", this);
			// HelpshiftDexLoader.getInstance().loadDex(this, this.application);
			// this.hsInternalLogger = HelpshiftInternalLogger.getInstance();
		}

		// Token: 0x06000CB9 RID: 3257 RVA: 0x0001E0BD File Offset: 0x0001C4BD
		private void unityHSApiCall(string api, params object[] args)
		{
			this.addHSApiCallToQueue("unityHSApiCallWithArgs", api, args);
		}

		// Token: 0x06000CBA RID: 3258 RVA: 0x0001E0CC File Offset: 0x0001C4CC
		private void hsApiCall(string api, params object[] args)
		{
			this.addHSApiCallToQueue("hsApiCallWithArgs", api, args);
		}

		// Token: 0x06000CBB RID: 3259 RVA: 0x0001E0DB File Offset: 0x0001C4DB
		private void hsApiCall(string api)
		{
			this.addHSApiCallToQueue("hsApiCall", api, null);
		}

		// Token: 0x06000CBC RID: 3260 RVA: 0x0001E0EA File Offset: 0x0001C4EA
		private void hsSupportApiCall(string api, params object[] args)
		{
			this.addHSApiCallToQueue("hsSupportApiCallWithArgs", api, args);
		}

		// Token: 0x06000CBD RID: 3261 RVA: 0x0001E0F9 File Offset: 0x0001C4F9
		private void hsSupportApiCall(string api)
		{
			this.addHSApiCallToQueue("hsSupportApiCall", api, null);
		}

		// Token: 0x06000CBE RID: 3262 RVA: 0x0001E108 File Offset: 0x0001C508
		private void addHSApiCallToQueue(string methodIdentifier, string api, object[] args)
		{
			HelpshiftWorker.getInstance().enqueueApiCall("support", methodIdentifier, api, args);
		}

		// Token: 0x06000CBF RID: 3263 RVA: 0x0001E11C File Offset: 0x0001C51C
		public void resolveAndCallApi(string methodIdentifier, string api, object[] args)
		{
			if (methodIdentifier.Equals("hsApiCallWithArgs"))
			{
				this.hsHelpshiftClass.CallStatic(api, args);
			}
			else if (methodIdentifier.Equals("hsApiCall"))
			{
				this.hsHelpshiftClass.CallStatic(api, new object[0]);
			}
			else if (methodIdentifier.Equals("hsSupportApiCallWithArgs"))
			{
				this.hsSupportClass.CallStatic(api, args);
			}
			else if (methodIdentifier.Equals("hsSupportApiCall"))
			{
				this.hsSupportClass.CallStatic(api, new object[0]);
			}
			else if (methodIdentifier.Equals("unityHSApiCallWithArgs"))
			{
				this.hsUnityAPIDelegate.CallStatic(api, args);
			}
		}

		// Token: 0x06000CC0 RID: 3264 RVA: 0x0001E1D8 File Offset: 0x0001C5D8
		public void onDexLoaded()
		{
			//this.hsHelpshiftClass = HelpshiftDexLoader.getInstance().getHSDexLoaderJavaClass().CallStatic<AndroidJavaObject>("getHelpshiftUnityAPIInstance", new object[0]);
			//this.hsSupportClass = HelpshiftDexLoader.getInstance().getHSDexLoaderJavaClass().CallStatic<AndroidJavaObject>("getHelpshiftSupportInstance", new object[0]);
		}

		// Token: 0x06000CC1 RID: 3265 RVA: 0x0001E228 File Offset: 0x0001C628
		public void install(string apiKey, string domain, string appId, Dictionary<string, object> configMap)
		{
			configMap.Add("sdkType", "unity");
			configMap.Add("pluginVersion", "4.1.0");
			configMap.Add("runtimeVersion", Application.unityVersion);
			string text = Json.Serialize(configMap);
			this.hsApiCall("install", new object[]
			{
				this.application,
				apiKey,
				domain,
				appId,
				text
			});
			this.hsInternalLogger.d(string.Concat(new string[]
			{
				"Install called  : ApiKey : ",
				apiKey,
				", Domain :",
				domain,
				", AppId: ",
				appId,
				", Config : ",
				text
			}));
		}

		// Token: 0x06000CC2 RID: 3266 RVA: 0x0001E2E0 File Offset: 0x0001C6E0
		public void install()
		{
			// eli key point: push通知
			// this.hsApiCall("install", new object[]
			// {
				// this.application
			// });
			// this.hsInternalLogger.d("Install called without config");
		}

		// Token: 0x06000CC3 RID: 3267 RVA: 0x0001E30C File Offset: 0x0001C70C
		public int getNotificationCount(bool isAsync)
		{
			// HelpshiftWorker.getInstance().synchronousWaitForApiCallQueue();
			// this.hsInternalLogger.d("Call getNotificationCount: isAsync : " + isAsync);
			// return this.hsHelpshiftClass.CallStatic<int>("getNotificationCount", new object[]
			// {
				// isAsync
			// });
			return 0;
		}

		// Token: 0x06000CC4 RID: 3268 RVA: 0x0001E35D File Offset: 0x0001C75D
		public void requestUnreadMessagesCount(bool isAsync)
		{
			this.hsInternalLogger.d("Call requestUnreadMessagesCount: isAsync : " + isAsync);
			this.hsApiCall("requestUnreadMessagesCount", new object[]
			{
				isAsync
			});
		}

		// Token: 0x06000CC5 RID: 3269 RVA: 0x0001E394 File Offset: 0x0001C794
		[Obsolete]
		public void setNameAndEmail(string userName, string email)
		{
			this.hsApiCall("setNameAndEmail", new object[]
			{
				userName,
				email
			});
		}

		// Token: 0x06000CC6 RID: 3270 RVA: 0x0001E3AF File Offset: 0x0001C7AF
		[Obsolete]
		public void setUserIdentifier(string identifier)
		{
			this.hsApiCall("setUserIdentifier", new object[]
			{
				identifier
			});
		}

		// Token: 0x06000CC7 RID: 3271 RVA: 0x0001E3C6 File Offset: 0x0001C7C6
		public void registerDeviceToken(string deviceToken)
		{
			this.hsInternalLogger.d("Register device token :" + deviceToken);
			this.hsApiCall("registerDeviceToken", new object[]
			{
				this.currentActivity,
				deviceToken
			});
		}

		// Token: 0x06000CC8 RID: 3272 RVA: 0x0001E3FC File Offset: 0x0001C7FC
		public void leaveBreadCrumb(string breadCrumb)
		{
			this.hsApiCall("leaveBreadCrumb", new object[]
			{
				breadCrumb
			});
		}

		// Token: 0x06000CC9 RID: 3273 RVA: 0x0001E413 File Offset: 0x0001C813
		public void clearBreadCrumbs()
		{
			this.hsApiCall("clearBreadCrumbs");
		}

		// Token: 0x06000CCA RID: 3274 RVA: 0x0001E420 File Offset: 0x0001C820
		[Obsolete("Use the login(HelpshiftUser user) api instead.")]
		public void login(string identifier, string userName, string email)
		{
			this.hsInternalLogger.d("Login called : " + userName);
			this.hsApiCall("login", new object[]
			{
				identifier,
				userName,
				email
			});
		}

		// Token: 0x06000CCB RID: 3275 RVA: 0x0001E455 File Offset: 0x0001C855
		public void login(HelpshiftUser helpshiftUser)
		{
			this.hsInternalLogger.d("Login called : " + helpshiftUser.name);
			this.hsApiCall("loginHelpshiftUser", new object[]
			{
				this.jsonifyHelpshiftUser(helpshiftUser)
			});
		}

		// Token: 0x06000CCC RID: 3276 RVA: 0x0001E48D File Offset: 0x0001C88D
		public void clearAnonymousUser()
		{
			this.hsApiCall("clearAnonymousUser");
		}

		// Token: 0x06000CCD RID: 3277 RVA: 0x0001E49A File Offset: 0x0001C89A
		public void logout()
		{
			this.hsApiCall("logout");
		}

		// Token: 0x06000CCE RID: 3278 RVA: 0x0001E4A7 File Offset: 0x0001C8A7
		public void showConversation(Dictionary<string, object> configMap)
		{
			this.hsApiCall("showConversationUnity", new object[]
			{
				this.currentActivity,
				Json.Serialize(this.cleanConfig(configMap))
			});
		}

		// Token: 0x06000CCF RID: 3279 RVA: 0x0001E4D2 File Offset: 0x0001C8D2
		public void showFAQSection(string sectionPublishId, Dictionary<string, object> configMap)
		{
			this.hsApiCall("showFAQSectionUnity", new object[]
			{
				this.currentActivity,
				sectionPublishId,
				Json.Serialize(this.cleanConfig(configMap))
			});
		}

		// Token: 0x06000CD0 RID: 3280 RVA: 0x0001E501 File Offset: 0x0001C901
		public void showSingleFAQ(string questionPublishId, Dictionary<string, object> configMap)
		{
			this.hsApiCall("showSingleFAQUnity", new object[]
			{
				this.currentActivity,
				questionPublishId,
				Json.Serialize(this.cleanConfig(configMap))
			});
		}

		// Token: 0x06000CD1 RID: 3281 RVA: 0x0001E530 File Offset: 0x0001C930
		public void showFAQs(Dictionary<string, object> configMap)
		{
			this.hsApiCall("showFAQsUnity", new object[]
			{
				this.currentActivity,
				Json.Serialize(this.cleanConfig(configMap))
			});
		}

		// Token: 0x06000CD2 RID: 3282 RVA: 0x0001E55B File Offset: 0x0001C95B
		public void showConversation()
		{
			string api = "showConversationUnity";
			object[] array = new object[2];
			array[0] = this.currentActivity;
			this.hsApiCall(api, array);
		}

		// Token: 0x06000CD3 RID: 3283 RVA: 0x0001E577 File Offset: 0x0001C977
		public void showFAQSection(string sectionPublishId)
		{
			string api = "showFAQSectionUnity";
			object[] array = new object[3];
			array[0] = this.currentActivity;
			array[1] = sectionPublishId;
			this.hsApiCall(api, array);
		}

		// Token: 0x06000CD4 RID: 3284 RVA: 0x0001E597 File Offset: 0x0001C997
		public void showSingleFAQ(string questionPublishId)
		{
			string api = "showSingleFAQUnity";
			object[] array = new object[3];
			array[0] = this.currentActivity;
			array[1] = questionPublishId;
			this.hsApiCall(api, array);
		}

		// Token: 0x06000CD5 RID: 3285 RVA: 0x0001E5B7 File Offset: 0x0001C9B7
		public void showFAQs()
		{
			string api = "showFAQsUnity";
			object[] array = new object[2];
			array[0] = this.currentActivity;
			this.hsApiCall(api, array);
		}

		// Token: 0x06000CD6 RID: 3286 RVA: 0x0001E5D3 File Offset: 0x0001C9D3
		public void showConversationWithMeta(Dictionary<string, object> configMap)
		{
			this.hsApiCall("showConversationWithMetaUnity", new object[]
			{
				this.currentActivity,
				Json.Serialize(this.cleanConfig(configMap))
			});
		}

		// Token: 0x06000CD7 RID: 3287 RVA: 0x0001E5FE File Offset: 0x0001C9FE
		public void showFAQSectionWithMeta(string sectionPublishId, Dictionary<string, object> configMap)
		{
			this.hsApiCall("showFAQSectionWithMetaUnity", new object[]
			{
				this.currentActivity,
				sectionPublishId,
				Json.Serialize(this.cleanConfig(configMap))
			});
		}

		// Token: 0x06000CD8 RID: 3288 RVA: 0x0001E62D File Offset: 0x0001CA2D
		public void showSingleFAQWithMeta(string questionPublishId, Dictionary<string, object> configMap)
		{
			this.hsApiCall("showSingleFAQWithMetaUnity", new object[]
			{
				this.currentActivity,
				questionPublishId,
				Json.Serialize(this.cleanConfig(configMap))
			});
		}

		// Token: 0x06000CD9 RID: 3289 RVA: 0x0001E65C File Offset: 0x0001CA5C
		public void showFAQsWithMeta(Dictionary<string, object> configMap)
		{
			this.hsApiCall("showFAQsWithMetaUnity", new object[]
			{
				this.currentActivity,
				Json.Serialize(this.cleanConfig(configMap))
			});
		}

		// Token: 0x06000CDA RID: 3290 RVA: 0x0001E687 File Offset: 0x0001CA87
		public void updateMetaData(Dictionary<string, object> metaData)
		{
			this.hsApiCall("setMetaData", new object[]
			{
				Json.Serialize(metaData)
			});
		}

		// Token: 0x06000CDB RID: 3291 RVA: 0x0001E6A3 File Offset: 0x0001CAA3
		private Dictionary<string, object> cleanConfig(Dictionary<string, object> configMap)
		{
			if (configMap.ContainsKey("customIssueFields"))
			{
				configMap["hs-custom-issue-field"] = configMap["customIssueFields"];
				configMap.Remove("customIssueFields");
			}
			return configMap;
		}

		// Token: 0x06000CDC RID: 3292 RVA: 0x0001E6D8 File Offset: 0x0001CAD8
		public void handlePushNotification(string issueId)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("issue_id", issueId);
			this.hsInternalLogger.d("Handle push notification : issueId " + issueId);
			this.handlePushNotification(dictionary);
		}

		// Token: 0x06000CDD RID: 3293 RVA: 0x0001E714 File Offset: 0x0001CB14
		public void handlePushNotification(Dictionary<string, object> pushNotificationData)
		{
			this.hsInternalLogger.d("Handle push notification : data :" + pushNotificationData.ToString());
			this.unityHSApiCall("handlePush", new object[]
			{
				this.currentActivity,
				Json.Serialize(pushNotificationData)
			});
		}

		// Token: 0x06000CDE RID: 3294 RVA: 0x0001E754 File Offset: 0x0001CB54
		public void showAlertToRateAppWithURL(string url)
		{
			this.hsApiCall("showAlertToRateApp", new object[]
			{
				url
			});
		}

		// Token: 0x06000CDF RID: 3295 RVA: 0x0001E76B File Offset: 0x0001CB6B
		public void registerDelegates()
		{
			this.hsInternalLogger.d("Registering delegates");
			this.hsApiCall("registerDelegates");
		}

		// Token: 0x06000CE0 RID: 3296 RVA: 0x0001E788 File Offset: 0x0001CB88
		[Obsolete("[Helpshift Warning]: This API is deprecated. Please use FCM push notifications for Unity instead.", false)]
		public void registerForPushWithGcmId(string gcmId)
		{
			this.hsInternalLogger.d("Registering for push notification : GCM ID : " + gcmId);
			this.hsApiCall("registerGcmKey", new object[]
			{
				gcmId,
				this.currentActivity
			});
		}

		// Token: 0x06000CE1 RID: 3297 RVA: 0x0001E7BE File Offset: 0x0001CBBE
		public void setSDKLanguage(string locale)
		{
			this.hsApiCall("setSDKLanguage", new object[]
			{
				locale
			});
		}

		// Token: 0x06000CE2 RID: 3298 RVA: 0x0001E7D5 File Offset: 0x0001CBD5
		public void showDynamicForm(string title, Dictionary<string, object>[] flows)
		{
			this.hsApiCall("showDynamicFormFromDataJson", new object[]
			{
				this.currentActivity,
				title,
				Json.Serialize(flows)
			});
		}

		// Token: 0x06000CE3 RID: 3299 RVA: 0x0001E7FE File Offset: 0x0001CBFE
		public bool isConversationActive()
		{
			HelpshiftWorker.getInstance().synchronousWaitForApiCallQueue();
			return this.hsSupportClass.CallStatic<bool>("isConversationActive", new object[0]);
		}

		// Token: 0x06000CE4 RID: 3300 RVA: 0x0001E820 File Offset: 0x0001CC20
		public void checkIfConversationActive()
		{
			this.hsHelpshiftClass.CallStatic("checkIfConversationActive", new object[0]);
		}

		// Token: 0x06000CE5 RID: 3301 RVA: 0x0001E838 File Offset: 0x0001CC38
		public void onApplicationQuit()
		{
			this.hsInternalLogger.d("onApplicationQuit");
			HelpshiftWorker.getInstance().onApplicationQuit();
		}

		// Token: 0x06000CE6 RID: 3302 RVA: 0x0001E854 File Offset: 0x0001CC54
		private string jsonifyHelpshiftUser(HelpshiftUser helpshiftUser)
		{
			return Json.Serialize(new Dictionary<string, string>
			{
				{
					"identifier",
					helpshiftUser.identifier
				},
				{
					"email",
					helpshiftUser.email
				},
				{
					"name",
					helpshiftUser.name
				},
				{
					"authToken",
					helpshiftUser.authToken
				}
			});
		}

		// Token: 0x04003F52 RID: 16210
		private AndroidJavaClass jc;

		// Token: 0x04003F53 RID: 16211
		private AndroidJavaObject currentActivity;

		// Token: 0x04003F54 RID: 16212
		private AndroidJavaObject application;

		// Token: 0x04003F55 RID: 16213
		private AndroidJavaObject hsHelpshiftClass;

		// Token: 0x04003F56 RID: 16214
		private AndroidJavaObject hsSupportClass;

		// Token: 0x04003F57 RID: 16215
		private AndroidJavaClass hsUnityAPIDelegate;

		// Token: 0x04003F58 RID: 16216
		private HelpshiftInternalLogger hsInternalLogger;
	}
}
