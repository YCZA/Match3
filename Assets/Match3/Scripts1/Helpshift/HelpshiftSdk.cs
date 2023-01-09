using System;
using System.Collections.Generic;

namespace Match3.Scripts1.Helpshift
{
	// Token: 0x020001D1 RID: 465
	public class HelpshiftSdk
	{
		// Token: 0x06000D59 RID: 3417 RVA: 0x0001FF78 File Offset: 0x0001E378
		private HelpshiftSdk()
		{
		}

		// Token: 0x06000D5A RID: 3418 RVA: 0x0001FF80 File Offset: 0x0001E380
		public static HelpshiftSdk getInstance()
		{
			if (HelpshiftSdk.instance == null)
			{
				HelpshiftSdk.instance = new HelpshiftSdk();
				HelpshiftSdk.nativeSdk = new HelpshiftAndroid();
			}
			return HelpshiftSdk.instance;
		}

		// Token: 0x06000D5B RID: 3419 RVA: 0x0001FFA5 File Offset: 0x0001E3A5
		public void install(string apiKey, string domainName, string appId, Dictionary<string, object> config)
		{
			HelpshiftSdk.nativeSdk.install(apiKey, domainName, appId, config);
		}

		// Token: 0x06000D5C RID: 3420 RVA: 0x0001FFB6 File Offset: 0x0001E3B6
		public void install(string apiKey, string domainName, string appId)
		{
			this.install(apiKey, domainName, appId, new Dictionary<string, object>());
		}

		// Token: 0x06000D5D RID: 3421 RVA: 0x0001FFC6 File Offset: 0x0001E3C6
		public void install()
		{
			HelpshiftSdk.nativeSdk.install();
		}

		// Token: 0x06000D5E RID: 3422 RVA: 0x0001FFD2 File Offset: 0x0001E3D2
		[Obsolete("[Helpshift Warning]: THIS API IS DEPRECATED AND USING IT COULD CAUSE UNCERTAIN BEHAVIOUR. PLEASE USE THE VARIANT 'requestUnreadMessagesCount:' API instead. https://developers.helpshift.com/unity/notifications-android/#showing-notification-count", false)]
		public int getNotificationCount(bool isAsync)
		{
			return HelpshiftSdk.nativeSdk.getNotificationCount(isAsync);
		}

		// Token: 0x06000D5F RID: 3423 RVA: 0x0001FFDF File Offset: 0x0001E3DF
		public void requestUnreadMessagesCount(bool isAsync)
		{
			HelpshiftSdk.nativeSdk.requestUnreadMessagesCount(isAsync);
		}

		// Token: 0x06000D60 RID: 3424 RVA: 0x0001FFEC File Offset: 0x0001E3EC
		[Obsolete]
		public void setNameAndEmail(string userName, string email)
		{
			HelpshiftSdk.nativeSdk.setNameAndEmail(userName, email);
		}

		// Token: 0x06000D61 RID: 3425 RVA: 0x0001FFFA File Offset: 0x0001E3FA
		[Obsolete]
		public void setUserIdentifier(string identifier)
		{
			HelpshiftSdk.nativeSdk.setUserIdentifier(identifier);
		}

		// Token: 0x06000D62 RID: 3426 RVA: 0x00020007 File Offset: 0x0001E407
		[Obsolete("Use the login(HelpshiftUser user) api instead.")]
		public void login(string identifier, string name, string email)
		{
			HelpshiftSdk.nativeSdk.login(identifier, name, email);
		}

		// Token: 0x06000D63 RID: 3427 RVA: 0x00020016 File Offset: 0x0001E416
		public void login(HelpshiftUser helpshiftUser)
		{
			HelpshiftSdk.nativeSdk.login(helpshiftUser);
		}

		// Token: 0x06000D64 RID: 3428 RVA: 0x00020023 File Offset: 0x0001E423
		public void clearAnonymousUser()
		{
			HelpshiftSdk.nativeSdk.clearAnonymousUser();
		}

		// Token: 0x06000D65 RID: 3429 RVA: 0x0002002F File Offset: 0x0001E42F
		public void logout()
		{
			HelpshiftSdk.nativeSdk.logout();
		}

		// Token: 0x06000D66 RID: 3430 RVA: 0x0002003B File Offset: 0x0001E43B
		public void registerDeviceToken(string deviceToken)
		{
			HelpshiftSdk.nativeSdk.registerDeviceToken(deviceToken);
		}

		// Token: 0x06000D67 RID: 3431 RVA: 0x00020048 File Offset: 0x0001E448
		public void leaveBreadCrumb(string breadCrumb)
		{
			HelpshiftSdk.nativeSdk.leaveBreadCrumb(breadCrumb);
		}

		// Token: 0x06000D68 RID: 3432 RVA: 0x00020055 File Offset: 0x0001E455
		public void clearBreadCrumbs()
		{
			HelpshiftSdk.nativeSdk.clearBreadCrumbs();
		}

		// Token: 0x06000D69 RID: 3433 RVA: 0x00020061 File Offset: 0x0001E461
		public void showConversation(Dictionary<string, object> configMap)
		{
			HelpshiftSdk.nativeSdk.showConversation(configMap);
		}

		// Token: 0x06000D6A RID: 3434 RVA: 0x0002006E File Offset: 0x0001E46E
		public void showConversation()
		{
			HelpshiftSdk.nativeSdk.showConversation();
		}

		// Token: 0x06000D6B RID: 3435 RVA: 0x0002007A File Offset: 0x0001E47A
		public void showConversationWithMeta(Dictionary<string, object> configMap)
		{
			HelpshiftSdk.nativeSdk.showConversationWithMeta(configMap);
		}

		// Token: 0x06000D6C RID: 3436 RVA: 0x00020087 File Offset: 0x0001E487
		public void showFAQSection(string sectionPublishId, Dictionary<string, object> configMap)
		{
			HelpshiftSdk.nativeSdk.showFAQSection(sectionPublishId, configMap);
		}

		// Token: 0x06000D6D RID: 3437 RVA: 0x00020095 File Offset: 0x0001E495
		public void showFAQSection(string sectionPublishId)
		{
			HelpshiftSdk.nativeSdk.showFAQSection(sectionPublishId);
		}

		// Token: 0x06000D6E RID: 3438 RVA: 0x000200A2 File Offset: 0x0001E4A2
		public void showFAQSectionWithMeta(string sectionPublishId, Dictionary<string, object> configMap)
		{
			HelpshiftSdk.nativeSdk.showFAQSectionWithMeta(sectionPublishId, configMap);
		}

		// Token: 0x06000D6F RID: 3439 RVA: 0x000200B0 File Offset: 0x0001E4B0
		public void showSingleFAQ(string questionPublishId, Dictionary<string, object> configMap)
		{
			HelpshiftSdk.nativeSdk.showSingleFAQ(questionPublishId, configMap);
		}

		// Token: 0x06000D70 RID: 3440 RVA: 0x000200BE File Offset: 0x0001E4BE
		public void showSingleFAQ(string questionPublishId)
		{
			HelpshiftSdk.nativeSdk.showSingleFAQ(questionPublishId);
		}

		// Token: 0x06000D71 RID: 3441 RVA: 0x000200CB File Offset: 0x0001E4CB
		public void showSingleFAQWithMeta(string questionPublishId, Dictionary<string, object> configMap)
		{
			HelpshiftSdk.nativeSdk.showSingleFAQWithMeta(questionPublishId, configMap);
		}

		// Token: 0x06000D72 RID: 3442 RVA: 0x000200D9 File Offset: 0x0001E4D9
		public void showFAQs(Dictionary<string, object> configMap)
		{
			HelpshiftSdk.nativeSdk.showFAQs(configMap);
		}

		// Token: 0x06000D73 RID: 3443 RVA: 0x000200E6 File Offset: 0x0001E4E6
		public void showFAQs()
		{
			HelpshiftSdk.nativeSdk.showFAQs();
		}

		// Token: 0x06000D74 RID: 3444 RVA: 0x000200F2 File Offset: 0x0001E4F2
		public void showFAQsWithMeta(Dictionary<string, object> configMap)
		{
			HelpshiftSdk.nativeSdk.showFAQsWithMeta(configMap);
		}

		// Token: 0x06000D75 RID: 3445 RVA: 0x000200FF File Offset: 0x0001E4FF
		public void updateMetaData(Dictionary<string, object> metaData)
		{
			HelpshiftSdk.nativeSdk.updateMetaData(metaData);
		}

		// Token: 0x06000D76 RID: 3446 RVA: 0x0002010C File Offset: 0x0001E50C
		[Obsolete("handlePushNotification(string id) is deprecated, please use handlePushNotification(Dictionary<string, string> data)")]
		public void handlePushNotification(string issueId)
		{
			HelpshiftSdk.nativeSdk.handlePushNotification(issueId);
		}

		// Token: 0x06000D77 RID: 3447 RVA: 0x0002011C File Offset: 0x0001E51C
		public void handlePushNotification(Dictionary<string, object> pushNotificationData)
		{
			List<string> list = new List<string>();
			foreach (KeyValuePair<string, object> keyValuePair in pushNotificationData)
			{
				if (keyValuePair.Value == null)
				{
					list.Add(keyValuePair.Key);
				}
			}
			foreach (string key in list)
			{
				pushNotificationData.Remove(key);
			}
			HelpshiftSdk.nativeSdk.handlePushNotification(pushNotificationData);
		}

		// Token: 0x06000D78 RID: 3448 RVA: 0x000201E0 File Offset: 0x0001E5E0
		public void showAlertToRateAppWithURL(string url)
		{
			HelpshiftSdk.nativeSdk.showAlertToRateAppWithURL(url);
		}

		// Token: 0x06000D79 RID: 3449 RVA: 0x000201ED File Offset: 0x0001E5ED
		public void setSDKLanguage(string locale)
		{
			HelpshiftSdk.nativeSdk.setSDKLanguage(locale);
		}

		// Token: 0x06000D7A RID: 3450 RVA: 0x000201FA File Offset: 0x0001E5FA
		public void registerDelegates()
		{
			HelpshiftSdk.nativeSdk.registerDelegates();
		}

		// Token: 0x06000D7B RID: 3451 RVA: 0x00020206 File Offset: 0x0001E606
		[Obsolete("[Helpshift Warning]: This API is deprecated. Please use FCM push notifications for Unity instead.", false)]
		public void registerForPush(string gcmId)
		{
			HelpshiftSdk.nativeSdk.registerForPushWithGcmId(gcmId);
		}

		// Token: 0x06000D7C RID: 3452 RVA: 0x00020213 File Offset: 0x0001E613
		public void showDynamicForm(string title, Dictionary<string, object>[] flows)
		{
			HelpshiftSdk.nativeSdk.showDynamicForm(title, flows);
		}

		// Token: 0x06000D7D RID: 3453 RVA: 0x00020221 File Offset: 0x0001E621
		public void onApplicationQuit()
		{
			HelpshiftSdk.nativeSdk.onApplicationQuit();
		}

		// Token: 0x06000D7E RID: 3454 RVA: 0x0002022D File Offset: 0x0001E62D
		[Obsolete("[Helpshift Warning]: THIS API IS DEPRECATED AND USING IT COULD CAUSE UNCERTAIN BEHAVIOUR. PLEASE USE THE VARIANT 'checkIfConversationActive:' API instead. https://developers.helpshift.com/unity/tracking-android/#isConversationActive", false)]
		public bool isConversationActive()
		{
			return HelpshiftSdk.nativeSdk.isConversationActive();
		}

		// Token: 0x06000D7F RID: 3455 RVA: 0x00020239 File Offset: 0x0001E639
		public void checkIfConversationActive()
		{
			HelpshiftSdk.nativeSdk.checkIfConversationActive();
		}

		// Token: 0x04003F86 RID: 16262
		public const string HS_RATE_ALERT_CLOSE = "HS_RATE_ALERT_CLOSE";

		// Token: 0x04003F87 RID: 16263
		public const string HS_RATE_ALERT_FEEDBACK = "HS_RATE_ALERT_FEEDBACK";

		// Token: 0x04003F88 RID: 16264
		public const string HS_RATE_ALERT_SUCCESS = "HS_RATE_ALERT_SUCCESS";

		// Token: 0x04003F89 RID: 16265
		public const string HS_RATE_ALERT_FAIL = "HS_RATE_ALERT_FAIL";

		// Token: 0x04003F8A RID: 16266
		public const string HSTAGSKEY = "hs-tags";

		// Token: 0x04003F8B RID: 16267
		public const string HSCUSTOMMETADATAKEY = "hs-custom-metadata";

		// Token: 0x04003F8C RID: 16268
		public const string HSCUSTOMISSUEFIELDKEY = "hs-custom-issue-field";

		// Token: 0x04003F8D RID: 16269
		public const string HSTAGSMATCHINGKEY = "withTagsMatching";

		// Token: 0x04003F8E RID: 16270
		public const string CONTACT_US_ALWAYS = "always";

		// Token: 0x04003F8F RID: 16271
		public const string CONTACT_US_NEVER = "never";

		// Token: 0x04003F90 RID: 16272
		public const string CONTACT_US_AFTER_VIEWING_FAQS = "after_viewing_faqs";

		// Token: 0x04003F91 RID: 16273
		public const string CONTACT_US_AFTER_MARKING_ANSWER_UNHELPFUL = "after_marking_answer_unhelpful";

		// Token: 0x04003F92 RID: 16274
		public const string HSUserAcceptedTheSolution = "User accepted the solution";

		// Token: 0x04003F93 RID: 16275
		public const string HSUserRejectedTheSolution = "User rejected the solution";

		// Token: 0x04003F94 RID: 16276
		public const string HSUserSentScreenShot = "User sent a screenshot";

		// Token: 0x04003F95 RID: 16277
		public const string HSUserReviewedTheApp = "User reviewed the app";

		// Token: 0x04003F96 RID: 16278
		public const string HsFlowTypeDefault = "defaultFlow";

		// Token: 0x04003F97 RID: 16279
		public const string HsFlowTypeConversation = "conversationFlow";

		// Token: 0x04003F98 RID: 16280
		public const string HsFlowTypeFaqs = "faqsFlow";

		// Token: 0x04003F99 RID: 16281
		public const string HsFlowTypeFaqSection = "faqSectionFlow";

		// Token: 0x04003F9A RID: 16282
		public const string HsFlowTypeSingleFaq = "singleFaqFlow";

		// Token: 0x04003F9B RID: 16283
		public const string HsFlowTypeNested = "dynamicFormFlow";

		// Token: 0x04003F9C RID: 16284
		public const string HsCustomContactUsFlows = "customContactUsFlows";

		// Token: 0x04003F9D RID: 16285
		public const string HsFlowType = "type";

		// Token: 0x04003F9E RID: 16286
		public const string HsFlowConfig = "config";

		// Token: 0x04003F9F RID: 16287
		public const string HsFlowData = "data";

		// Token: 0x04003FA0 RID: 16288
		public const string HsFlowTitle = "title";

		// Token: 0x04003FA1 RID: 16289
		private static HelpshiftSdk instance;

		// Token: 0x04003FA2 RID: 16290
		private static HelpshiftAndroid nativeSdk;
	}
}
