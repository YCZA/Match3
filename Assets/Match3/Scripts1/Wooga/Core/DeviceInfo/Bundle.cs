namespace Match3.Scripts1.Wooga.Core.DeviceInfo
{
	// Token: 0x0200034C RID: 844
	public static class Bundle
	{
		// Token: 0x17000411 RID: 1041
		// (get) Token: 0x060019CA RID: 6602 RVA: 0x0007476C File Offset: 0x00072B6C
		public static string version
		{
			get
			{
				if (Bundle._version == null)
				{
					Bundle.Init();
				}
				return Bundle._version;
			}
		}

		// Token: 0x17000412 RID: 1042
		// (get) Token: 0x060019CB RID: 6603 RVA: 0x00074782 File Offset: 0x00072B82
		public static string identifier
		{
			get
			{
				if (Bundle._identifier == null)
				{
					Bundle.Init();
				}
				return Bundle._identifier;
			}
		}

		// Token: 0x17000413 RID: 1043
		// (get) Token: 0x060019CC RID: 6604 RVA: 0x00074798 File Offset: 0x00072B98
		public static string build
		{
			get
			{
				if (Bundle._build == null)
				{
					Bundle.Init();
				}
				return Bundle._build;
			}
		}

		// Token: 0x060019CD RID: 6605 RVA: 0x000747B0 File Offset: 0x00072BB0
		public static void Init()
		{
			try
			{
				// AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
				// AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
				// AndroidJavaObject androidJavaObject = @static.Call<AndroidJavaObject>("getPackageManager", new object[0]);
				// string text = @static.Call<string>("getPackageName", new object[0]);
				// AndroidJavaObject androidJavaObject2 = androidJavaObject.Call<AndroidJavaObject>("getPackageInfo", new object[]
				// {
					// text,
					// 0
				// });
				// Bundle._version = androidJavaObject2.Get<string>("versionName");
				// Bundle._identifier = text;
				// Bundle._build = androidJavaObject2.Get<int>("versionCode").ToString();
			}
			catch
			{
				Bundle._version = string.Empty;
				Bundle._identifier = string.Empty;
				Bundle._build = string.Empty;
			}
		}

		// Token: 0x04004848 RID: 18504
		private static string _version;

		// Token: 0x04004849 RID: 18505
		private static string _identifier;

		// Token: 0x0400484A RID: 18506
		private static string _build;
	}
}
