using Match3.Scripts1.Wooga.Services.ErrorAnalytics.Payload;
using UnityEngine;

namespace Match3.Scripts1.Wooga.Services.ErrorAnalytics.Internal
{
	// Token: 0x020003FB RID: 1019
	internal class SbsNativeErrorAnalytics
	{
		// Token: 0x170004B3 RID: 1203
		// (get) Token: 0x06001E6C RID: 7788 RVA: 0x00081019 File Offset: 0x0007F419
		private static AndroidJavaClass errorAnalyticsAndroid
		{
			get
			{
				// if (SbsNativeErrorAnalytics._errorAnalyticsAndroid == null)
				// {
				// 	SbsNativeErrorAnalytics._errorAnalyticsAndroid = new AndroidJavaClass("com.wooga.services.erroranalytics.facade.EAFacade");
				// }
				return SbsNativeErrorAnalytics._errorAnalyticsAndroid;
			}
		}

		// Token: 0x06001E6D RID: 7789 RVA: 0x0008103C File Offset: 0x0007F43C
		public static void InitErrorAnalytics(string gameId, string userId, Information.App appInfo)
		{
			// AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			// SbsNativeErrorAnalytics.errorAnalyticsAndroid.CallStatic("initErrorAnalytics", new object[]
			// {
			// 	androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"),
			// 	gameId,
			// 	userId,
			// 	appInfo.Version,
			// 	appInfo.TechnicalVersion,
			// 	appInfo.BundleIdentifier
			// });
		}

		// Token: 0x06001E6E RID: 7790 RVA: 0x0008109D File Offset: 0x0007F49D
		public static void SetSBSDeviceId(string deviceId)
		{
			SbsNativeErrorAnalytics.errorAnalyticsAndroid.CallStatic("setSBSDeviceId", new object[]
			{
				deviceId
			});
		}

		// Token: 0x06001E6F RID: 7791 RVA: 0x000810B8 File Offset: 0x0007F4B8
		public static void SetSBSUserId(string sbsUserId)
		{
			SbsNativeErrorAnalytics.errorAnalyticsAndroid.CallStatic("setSBSUserId", new object[]
			{
				sbsUserId
			});
		}

		// Token: 0x06001E70 RID: 7792 RVA: 0x000810D3 File Offset: 0x0007F4D3
		public static void NotifyError(string errorClass, string errorMessage, string stackTrace, string metaData, bool inBackground, int severity)
		{
		}

		// Token: 0x06001E71 RID: 7793 RVA: 0x000810D5 File Offset: 0x0007F4D5
		public static void SetBreadcrumbBufferSize(int size)
		{
			SbsNativeErrorAnalytics.errorAnalyticsAndroid.CallStatic("setBreadcrumbBufferSize", new object[]
			{
				size
			});
		}

		// Token: 0x06001E72 RID: 7794 RVA: 0x000810F5 File Offset: 0x0007F4F5
		public static void AddBreadcrumb(string text)
		{
			
			// SbsNativeErrorAnalytics.errorAnalyticsAndroid.CallStatic("addBreadcrumb", new object[]
			// {
			// 	text
			// });
		}

		// Token: 0x06001E73 RID: 7795 RVA: 0x00081110 File Offset: 0x0007F510
		public static void Print(string text)
		{
		}

		// Token: 0x04004A49 RID: 19017
		private static AndroidJavaClass _errorAnalyticsAndroid;
	}
}
