using UnityEngine;

namespace Match3.Scripts1.Wooga.SafeDK.Android
{
	// Token: 0x0200042F RID: 1071
	public class AndroidSafeDKService : ISafeDKService
	{
		// Token: 0x06001F5B RID: 8027 RVA: 0x0008378C File Offset: 0x00081B8C
		public AndroidSafeDKService()
		{
			//this.plugin = new AndroidJavaObject("com.wooga.safedk.SafeDKService", new object[0]);
		}

		// Token: 0x06001F5C RID: 8028 RVA: 0x000837AA File Offset: 0x00081BAA
		public string GetUserId()
		{
			//return this.plugin.Call<string>("GetUserId", new object[0]);
			return "";
		}

		// Token: 0x04004AE0 RID: 19168
		private readonly AndroidJavaObject plugin;
	}
}
