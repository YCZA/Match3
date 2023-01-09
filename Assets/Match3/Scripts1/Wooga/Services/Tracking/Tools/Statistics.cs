using System;

namespace Match3.Scripts1.Wooga.Services.Tracking.Tools
{
	// Token: 0x02000468 RID: 1128
	public class Statistics
	{
		// Token: 0x060020B2 RID: 8370 RVA: 0x0008A404 File Offset: 0x00088804
		public static TimeSpan SinceAppInstall()
		{
			if (Statistics.__installTime == null)
			{
				Statistics.SetInstallTime();
			}
			DateTime value = Statistics.__installTime.Value;
			return global::Wooga.Core.Utilities.Time.UtcNow() - value;
		}

		// Token: 0x060020B3 RID: 8371 RVA: 0x0008A43C File Offset: 0x0008883C
		private static void SetInstallTime()
		{
			double value = Statistics.SecondsSinceEpochAndroid();
			if (Math.Abs(value) > 0.0)
			{
				DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
				DateTime value2 = dateTime.AddSeconds(value);
				Statistics.__installTime = new DateTime?(value2);
			}
			else
			{
				Statistics.__installTime = new DateTime?(global::Wooga.Core.Utilities.Time.UtcNow());
			}
		}

		// Token: 0x060020B4 RID: 8372 RVA: 0x0008A4A8 File Offset: 0x000888A8
		private static double SecondsSinceEpochAndroid()
		{
			// AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			// AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
			// AndroidJavaObject androidJavaObject = @static.Call<AndroidJavaObject>("getPackageManager", new object[0]);
			// AndroidJavaObject androidJavaObject2 = androidJavaObject.Call<AndroidJavaObject>("getPackageInfo", new object[]
			// {
				// Bundle.identifier,
				// 0
			// });
			// long num = androidJavaObject2.Get<long>("firstInstallTime");
			// return (double)(num / 1000L);
			return 0;
		}

		// Token: 0x04004B92 RID: 19346
		private static DateTime? __installTime;
	}
}
