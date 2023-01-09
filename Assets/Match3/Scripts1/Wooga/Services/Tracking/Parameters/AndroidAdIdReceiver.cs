using Wooga.Core.Utilities;
using UnityEngine;

namespace Match3.Scripts1.Wooga.Services.Tracking.Parameters
{
	// Token: 0x02000443 RID: 1091
	public class AndroidAdIdReceiver : MonoBehaviour
	{
		// Token: 0x06001FCC RID: 8140 RVA: 0x000858C8 File Offset: 0x00083CC8
		private void Awake()
		{
			this.RetrieveAdId();
		}

		// Token: 0x06001FCD RID: 8141 RVA: 0x000858D0 File Offset: 0x00083CD0
		private void Start()
		{
		}

		// Token: 0x06001FCE RID: 8142 RVA: 0x000858D4 File Offset: 0x00083CD4
		private void RetrieveAdId()
		{
			Log.Info(new object[]
			{
				"TRACKING: RETRIEVING GOOGLE AD ID"
			});
			//using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.wooga.tracking.AndroidAdId"))
			//{
			//	if (androidJavaClass != null)
			//	{
			//		androidJavaClass.CallStatic("fetchAdId", new object[]
			//		{
			//			base.transform.name
			//		});
			//	}
			//}
		}

		// Token: 0x06001FCF RID: 8143 RVA: 0x00085948 File Offset: 0x00083D48
		public void OnReceivedAndroidAdId(string adId)
		{
			if (adId != null)
			{
				Log.Info(new object[]
				{
					"RETRIEVED GOOGLE AD ID " + adId
				});
				AndroidAdId.adId = adId;
			}
			else
			{
				Log.Error(new object[]
				{
					"FAILED TO RETRIEVE GOOGLE AD ID"
				});
			}
		}
	}
}
