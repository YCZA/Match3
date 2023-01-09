using System.Collections.Generic;

namespace Match3.Scripts1.Wooga.Core.DeviceInfo
{
	// Token: 0x02000351 RID: 849
	public static class Platform
	{
		// Token: 0x060019E8 RID: 6632 RVA: 0x00074C59 File Offset: 0x00073059
		public static bool isJailbroken()
		{
			return false;
		}

		// Token: 0x060019E9 RID: 6633 RVA: 0x00074C5C File Offset: 0x0007305C
		public static List<string> InstalledWoogaBundleIds()
		{
			List<string> list = new List<string>();
			// AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			// AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
			// AndroidJavaObject androidJavaObject = @static.Call<AndroidJavaObject>("getPackageManager", new object[0]);
			object[] args = new object[]
			{
				0
			};
			// AndroidJavaObject androidJavaObject2 = androidJavaObject.Call<AndroidJavaObject>("getInstalledApplications", args);
			// Regex regex = new Regex("^(com|net)\\.wooga\\.|^net\\.mantisshrimp\\.", RegexOptions.IgnoreCase);
			// int num = androidJavaObject2.Call<int>("size", new object[0]);
			// for (int i = 0; i < num; i++)
			// {
				// AndroidJavaObject androidJavaObject3 = androidJavaObject2.Call<AndroidJavaObject>("get", new object[]
				// {
					// i
				// });
				// string text = androidJavaObject3.Get<string>("packageName");
				// if (regex.Match(text).Success)
				// {
					// list.Add(text);
				// }
			// }
			return list;
		}
	}
}
