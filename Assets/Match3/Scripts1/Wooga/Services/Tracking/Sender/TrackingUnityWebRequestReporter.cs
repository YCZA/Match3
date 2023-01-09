using System.Collections.Generic;
using Wooga.Core.Utilities;
using UnityEngine.Networking;

namespace Match3.Scripts1.Wooga.Services.Tracking.Sender
{
	// eli key point tracking unitywebrequest
	public class TrackingUnityWebRequestReporter
	{
		// Token: 0x06002024 RID: 8228 RVA: 0x0008678C File Offset: 0x00084B8C
		public IEnumerator<long> Send(string uri)
		{
			Log.Info(new object[]
			{
				"send " + uri
			});
			UnityWebRequest request = UnityWebRequest.Get(uri);
			// 不发送
			// request.SendWebRequest();
			// while (!request.isDone)
			// {
				yield return 0L;
			// }
			// Log.Info(new object[]
			// {
				// request.responseCode.ToString()
			// });
			// yield return request.responseCode;
		}
	}
}
