using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine.Networking;

namespace Match3.Scripts1.Wooga.Services.TrackingCore.Networking
{
	// Token: 0x02000479 RID: 1145
	public class UnityWebRequestClient : ISender
	{
		// Token: 0x0600211A RID: 8474 RVA: 0x0008B538 File Offset: 0x00089938
		public IEnumerator<HttpStatusCode> Post(string endpoint, byte[] body, Dictionary<string, string> headers)
		{
			using (UnityWebRequest request = UnityWebRequest.Post(endpoint, "POST"))
			{
				UploadHandlerRaw uploadHandler = new UploadHandlerRaw(body)
				{
					contentType = "application/json"
				};
				request.uploadHandler = uploadHandler;
				request.disposeUploadHandlerOnDispose = true;
				request.useHttpContinue = false;
				foreach (KeyValuePair<string, string> keyValuePair in headers)
				{
					request.SetRequestHeader(keyValuePair.Key, keyValuePair.Value);
				}
				request.SendWebRequest();
				while (!request.isDone)
				{
					yield return (HttpStatusCode)0;
				}
				yield return (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), request.responseCode.ToString());
			}
			yield break;
		}

		// Token: 0x04004BB8 RID: 19384
		public const string HEADER_SIGNATURE_CONFIG = "X-SBS-Config-Signature";

		// Token: 0x04004BB9 RID: 19385
		public const string HEADER_SIGNATURE_ALGO = "X-SBS-Config-Signature-Algo";

		// Token: 0x04004BBA RID: 19386
		public const string HEADER_TIMESTAMP = "X-SBS-DATE";

		// Token: 0x04004BBB RID: 19387
		public const string HEADER_SIGNATURE = "X-SBS-SIGNATURE";

		// Token: 0x04004BBC RID: 19388
		public const string HEADER_SBS_ID = "X-SBS-ID";

		// Token: 0x04004BBD RID: 19389
		public const string HEADER_ETAG = "ETag";

		// Token: 0x04004BBE RID: 19390
		public const string HEADER_IF_NONE_MATCH = "If-None-Match";

		// Token: 0x04004BBF RID: 19391
		public const string HEADER_IF_MATCH = "If-Match";

		// Token: 0x04004BC0 RID: 19392
		public const string HEADER_CONTENT_TYPE = "Content-Type";
	}
}
