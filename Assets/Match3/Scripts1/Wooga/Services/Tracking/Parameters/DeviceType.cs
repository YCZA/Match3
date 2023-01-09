namespace Match3.Scripts1.Wooga.Services.Tracking.Parameters
{
	// Token: 0x02000445 RID: 1093
	public static class DeviceType
	{
		// Token: 0x170004E2 RID: 1250
		// (get) Token: 0x06001FD3 RID: 8147 RVA: 0x00085B97 File Offset: 0x00083F97
		public static string type
		{
			get
			{
				if (DeviceType._type == null)
				{
					DeviceType._type = DeviceType.GetNativeType();
				}
				return DeviceType._type;
			}
		}

		// Token: 0x06001FD4 RID: 8148 RVA: 0x00085BB4 File Offset: 0x00083FB4
		private static string GetNativeType()
		{
			// AndroidJavaClass androidJavaClass = new AndroidJavaClass("android.os.Build");
			// return androidJavaClass.GetStatic<string>("MODEL");
			return "$$$$";
		}

		// Token: 0x04004B40 RID: 19264
		private static string _type;
	}
}
