using UnityEngine;

namespace Match3.Scripts1.Puzzletown
{
	// Token: 0x02000735 RID: 1845
	public static class PTDeviceInfo
	{
		// Token: 0x17000713 RID: 1811
		// (get) Token: 0x06002DAF RID: 11695 RVA: 0x000D49D2 File Offset: 0x000D2DD2
		public static DeviceType Type
		{
			get
			{
				if (PTDeviceInfo.type == DeviceType.None)
				{
					PTDeviceInfo.type = PTDeviceInfo.FindDeviceType();
				}
				return PTDeviceInfo.type;
			}
		}

		// Token: 0x06002DB0 RID: 11696 RVA: 0x000D49F0 File Offset: 0x000D2DF0
		public static DeviceType FindDeviceType()
		{
			DeviceType result = DeviceType.None;
			try
			{
				PTDeviceInfo.DisplayMetricsAndroid.FetchMetricsAndroid();
				result = ((PTDeviceInfo.DisplayMetricsAndroid.ScreenSizeInch <= 7f) ? DeviceType.Phone : DeviceType.Tablet);
			}
			catch
			{
				WoogaDebug.LogWarning(new object[]
				{
					"PTDeviceInfo: Failed to determine screen size from java code -> assuming it's a phone:"
				});
				result = DeviceType.Phone;
			}
			return result;
		}

		// Token: 0x0400574A RID: 22346
		public const float TABLET_MIN_SCREEN_SIZE_INCH = 7f;

		// Token: 0x0400574B RID: 22347
		private static DeviceType type;

		// Token: 0x02000736 RID: 1846
		public class DisplayMetricsAndroid
		{
			// Token: 0x17000714 RID: 1812
			// (get) Token: 0x06002DB3 RID: 11699 RVA: 0x000D4A56 File Offset: 0x000D2E56
			// (set) Token: 0x06002DB4 RID: 11700 RVA: 0x000D4A5D File Offset: 0x000D2E5D
			public static float Density { get; protected set; }

			// Token: 0x17000715 RID: 1813
			// (get) Token: 0x06002DB5 RID: 11701 RVA: 0x000D4A65 File Offset: 0x000D2E65
			// (set) Token: 0x06002DB6 RID: 11702 RVA: 0x000D4A6C File Offset: 0x000D2E6C
			public static int DensityDPI { get; protected set; }

			// Token: 0x17000716 RID: 1814
			// (get) Token: 0x06002DB7 RID: 11703 RVA: 0x000D4A74 File Offset: 0x000D2E74
			// (set) Token: 0x06002DB8 RID: 11704 RVA: 0x000D4A7B File Offset: 0x000D2E7B
			public static int HeightPixels { get; protected set; }

			// Token: 0x17000717 RID: 1815
			// (get) Token: 0x06002DB9 RID: 11705 RVA: 0x000D4A83 File Offset: 0x000D2E83
			// (set) Token: 0x06002DBA RID: 11706 RVA: 0x000D4A8A File Offset: 0x000D2E8A
			public static int WidthPixels { get; protected set; }

			// Token: 0x17000718 RID: 1816
			// (get) Token: 0x06002DBB RID: 11707 RVA: 0x000D4A92 File Offset: 0x000D2E92
			// (set) Token: 0x06002DBC RID: 11708 RVA: 0x000D4A99 File Offset: 0x000D2E99
			public static float ScaledDensity { get; protected set; }

			// Token: 0x17000719 RID: 1817
			// (get) Token: 0x06002DBD RID: 11709 RVA: 0x000D4AA1 File Offset: 0x000D2EA1
			// (set) Token: 0x06002DBE RID: 11710 RVA: 0x000D4AA8 File Offset: 0x000D2EA8
			public static float XDPI { get; protected set; }

			// Token: 0x1700071A RID: 1818
			// (get) Token: 0x06002DBF RID: 11711 RVA: 0x000D4AB0 File Offset: 0x000D2EB0
			// (set) Token: 0x06002DC0 RID: 11712 RVA: 0x000D4AB7 File Offset: 0x000D2EB7
			public static float YDPI { get; protected set; }

			// Token: 0x1700071B RID: 1819
			// (get) Token: 0x06002DC1 RID: 11713 RVA: 0x000D4ABF File Offset: 0x000D2EBF
			// (set) Token: 0x06002DC2 RID: 11714 RVA: 0x000D4AC6 File Offset: 0x000D2EC6
			public static float ScreenSizeInch { get; protected set; }

			// Token: 0x06002DC3 RID: 11715 RVA: 0x000D4AD0 File Offset: 0x000D2ED0
			public static void FetchMetricsAndroid()
			{
				if (Application.platform != RuntimePlatform.Android)
				{
					return;
				}
				//using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
				{
					//using (new AndroidJavaClass("android.util.DisplayMetrics"))
					{
						//using (AndroidJavaObject androidJavaObject = new AndroidJavaObject("android.util.DisplayMetrics", new object[0]))
						//{
						//	using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
						//	{
						//		using (AndroidJavaObject androidJavaObject2 = @static.Call<AndroidJavaObject>("getWindowManager", new object[0]))
						//		{
						//			using (AndroidJavaObject androidJavaObject3 = androidJavaObject2.Call<AndroidJavaObject>("getDefaultDisplay", new object[0]))
						//			{
						//				androidJavaObject3.Call("getMetrics", new object[]
						//				{
						//					androidJavaObject
						//				});
						//				PTDeviceInfo.DisplayMetricsAndroid.Density = androidJavaObject.Get<float>("density");
						//				PTDeviceInfo.DisplayMetricsAndroid.DensityDPI = androidJavaObject.Get<int>("densityDpi");
						//				PTDeviceInfo.DisplayMetricsAndroid.HeightPixels = androidJavaObject.Get<int>("heightPixels");
						//				PTDeviceInfo.DisplayMetricsAndroid.WidthPixels = androidJavaObject.Get<int>("widthPixels");
						//				PTDeviceInfo.DisplayMetricsAndroid.ScaledDensity = androidJavaObject.Get<float>("scaledDensity");
						//				PTDeviceInfo.DisplayMetricsAndroid.XDPI = androidJavaObject.Get<float>("xdpi");
						//				PTDeviceInfo.DisplayMetricsAndroid.YDPI = androidJavaObject.Get<float>("ydpi");
						//				if (PTDeviceInfo.DisplayMetricsAndroid.XDPI > 0f && PTDeviceInfo.DisplayMetricsAndroid.YDPI > 0f && (float)PTDeviceInfo.DisplayMetricsAndroid.HeightPixels > 0f && (float)PTDeviceInfo.DisplayMetricsAndroid.WidthPixels > 0f)
						//				{
						//					float num = (float)PTDeviceInfo.DisplayMetricsAndroid.WidthPixels / PTDeviceInfo.DisplayMetricsAndroid.XDPI;
						//					float num2 = (float)PTDeviceInfo.DisplayMetricsAndroid.HeightPixels / PTDeviceInfo.DisplayMetricsAndroid.YDPI;
						//					PTDeviceInfo.DisplayMetricsAndroid.ScreenSizeInch = Mathf.Sqrt(num * num + num2 * num2);
						//				}
						//			}
						//		}
						//	}
						//}
					}
				}
			}
		}
	}
}
