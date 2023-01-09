using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Match3.Scripts1.Wooga.Core.DeviceInfo;
using Match3.Scripts1.Wooga.Core.Exceptions;
using Match3.Scripts1.Wooga.Core.Exceptions.Network;
using Wooga.Core.Utilities;
using Wooga.Coroutines;
using UnityEngine.Networking;
using Wooga.Core.Extensions;

namespace Match3.Scripts1.Wooga.Core.Network
{
	// Token: 0x02000364 RID: 868
	public class SbsNetworkingUnityWebRequest : ISbsNetworking
	{
		// Token: 0x06001A18 RID: 6680 RVA: 0x00075350 File Offset: 0x00073750
		public SbsNetworkingUnityWebRequest(string sbsId)
		{
			this._sbsId = sbsId;
			this._numErrorsIPV6 = 0;
		}

		// Token: 0x06001A19 RID: 6681 RVA: 0x00075368 File Offset: 0x00073768
		public IEnumerator<SbsResponse> Send(SbsRequest sbsRequest)
		{
			this.NormalizeRequest(sbsRequest);
			this.LogRequest(sbsRequest);
			return this.SendImpl(sbsRequest).Catch(delegate(Exception exception)
			{
				// if (sbsRequest.Host == "api.sbs.wooga.com2333")
				if (sbsRequest.Host == "2333")
				{
					this._numErrorsIPV6++;
					// sbsRequest.Host = "api-fallback.sbs.wooga.com";
					sbsRequest.Host = "2333";
					return this.SendImpl(sbsRequest);
				}
				return ExceptionUtils.RethrowException<IEnumerator<SbsResponse>>(exception);
			}).ContinueWith(delegate(SbsResponse sbsResponse)
			{
				this.LogResponse(sbsResponse);
				this.ValidateResponseSignature(sbsRequest, sbsResponse);
				return sbsResponse;
			});
		}

		// Token: 0x06001A1A RID: 6682 RVA: 0x000753D0 File Offset: 0x000737D0
		protected virtual UnityWebRequest BuildRequest(SbsRequest sbsRequest)
		{
			string text = string.Format("{0}://{1}{2}", sbsRequest.Protocol, sbsRequest.Host, sbsRequest.Path);
			UnityWebRequest unityWebRequest;
			switch (sbsRequest.Method)
			{
			case HttpMethod.GET:
				unityWebRequest = UnityWebRequest.Get(text);
				goto IL_EE;
			case HttpMethod.POST:
				if (sbsRequest.SendAsBytes)
				{
					byte[] bytes = Encoding.UTF8.GetBytes(sbsRequest.Body);
					unityWebRequest = new UnityWebRequest(text, "POST");
					unityWebRequest.uploadHandler = new UploadHandlerRaw(bytes);
					unityWebRequest.downloadHandler = new DownloadHandlerBuffer();
					unityWebRequest.SetRequestHeader("Content-Type", "application/json");
				}
				else
				{
					unityWebRequest = UnityWebRequest.Post(text, sbsRequest.Body);
				}
				goto IL_EE;
			case HttpMethod.PUT:
				unityWebRequest = UnityWebRequest.Put(text, sbsRequest.Body);
				goto IL_EE;
			case HttpMethod.DELETE:
				unityWebRequest = UnityWebRequest.Delete(text);
				goto IL_EE;
			}
			throw new NotSupportedException(string.Format("Http Method {0} is not supported.", sbsRequest.Method));
			IL_EE:
			foreach (KeyValuePair<string, string> keyValuePair in sbsRequest.Headers)
			{
				unityWebRequest.SetRequestHeader(keyValuePair.Key, keyValuePair.Value);
			}
			return unityWebRequest;
		}

		// Token: 0x06001A1B RID: 6683 RVA: 0x0007552C File Offset: 0x0007392C
		private void NormalizeRequest(SbsRequest sbsRequest)
		{
			if (!sbsRequest.Headers.ContainsKey("X-SBS-ID") && sbsRequest.UserContext != null)
			{
				sbsRequest.Headers.Add("X-SBS-ID", this._sbsId);
			}
			if (!string.IsNullOrEmpty(sbsRequest.Body))
			{
				sbsRequest.Headers.Add("Content-Type", "application/json");
			}
			if (sbsRequest.UseSignature)
			{
				SbsSigningHandler.SignRequest(sbsRequest, this._sbsId);
			}
			string etag = sbsRequest.ETag;
			if (etag != null && sbsRequest.Method == HttpMethod.GET)
			{
				sbsRequest.Headers.Add("If-None-Match", etag);
			}
			else if (etag != null && sbsRequest.Method == HttpMethod.PUT)
			{
				sbsRequest.Headers.Add("If-Match", etag);
			}
			sbsRequest.Headers.Add("X-Client-Version", Bundle.version);
			// if (sbsRequest.Host == "api.sbs.wooga.com" && this.ShouldFallbackToIPv4())
			if (sbsRequest.Host == "xxxx2333" && this.ShouldFallbackToIPv4())
			{
				// sbsRequest.Host = "api-fallback.sbs.wooga.com";
				sbsRequest.Host = "xxxx2333";
			}
		}

		// Token: 0x06001A1C RID: 6684 RVA: 0x0007563C File Offset: 0x00073A3C
		protected virtual IEnumerator<SbsResponse> SendImpl(SbsRequest sbsRequest)
		{
			using (UnityWebRequest request = this.BuildRequest(sbsRequest))
			{
				int timeout = (sbsRequest.TimeoutInSeconds <= 0) ? int.MaxValue : sbsRequest.TimeoutInSeconds;
				ConnectionMonitor monitor = new ConnectionMonitor(request, timeout, timeout);
				request.SendWebRequest();
				while (!request.isDone)
				{
					monitor.AssertLiveliness();
					yield return null;
				}
				if (request.responseCode <= 0L && (request.isNetworkError || !string.IsNullOrEmpty(request.error)))
				{
					throw new SbsUnknownNetworkException(request.error);
				}
				if (request.responseCode <= 0L || request.responseCode >= 500L)
				{
					throw new SbsServerException((HttpStatusCode)request.responseCode);
				}
				yield return this.BuildResponse(request);
			}
			yield break;
		}

		// Token: 0x06001A1D RID: 6685 RVA: 0x00075660 File Offset: 0x00073A60
		protected virtual SbsResponse BuildResponse(UnityWebRequest unityWebRequest)
		{
			long responseCode = unityWebRequest.responseCode;
			string bodyJson = (unityWebRequest.downloadHandler != null) ? unityWebRequest.downloadHandler.text : string.Empty;
			Dictionary<string, string> responseHeaders = unityWebRequest.GetResponseHeaders();
			HttpStatusCode statusCode = (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), responseCode.ToString());
			return new SbsResponse(bodyJson, statusCode)
			{
				Signature = responseHeaders.GetHeaderValue("X-SBS-SIGNATURE"),
				SignatureConfig = responseHeaders.GetHeaderValue("X-SBS-Config-Signature"),
				SignatureAlgo = responseHeaders.GetHeaderValue("X-SBS-Config-Signature-Algo"),
				Timestamp = responseHeaders.GetHeaderValue("X-SBS-DATE"),
				Headers = responseHeaders,
				ContentType = responseHeaders.GetHeaderValue("Content-Type")
			};
		}

		// Token: 0x06001A1E RID: 6686 RVA: 0x0007572C File Offset: 0x00073B2C
		private void ValidateResponseSignature(SbsRequest sbsRequest, SbsResponse sbsResponse)
		{
			if (sbsRequest.UseSignature && !string.IsNullOrEmpty(sbsRequest.Signature))
			{
				string stringToSign = SbsSigningHandler.BuildSignedResponseString(sbsResponse, sbsRequest.Signature);
				string b = SbsSigningHandler.BuildSignature(sbsRequest.UserContext.password, stringToSign);
				if (string.IsNullOrEmpty(sbsResponse.Signature) || sbsResponse.Signature != b)
				{
					throw new SbsInvalidSignatureException(sbsRequest.Path, string.Empty, sbsResponse.Signature);
				}
			}
		}

		// Token: 0x06001A1F RID: 6687 RVA: 0x000757AC File Offset: 0x00073BAC
		private void LogRequest(SbsRequest sbsRequest)
		{
			if (Log.LogSbsRequest)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine(string.Concat(new object[]
				{
					"SBS Request: ",
					sbsRequest.Method,
					" ",
					string.Format("{0}://{1}{2}", sbsRequest.Protocol, sbsRequest.Host, sbsRequest.Path)
				}));
				stringBuilder.AppendLine("Headers: ");
				foreach (KeyValuePair<string, string> keyValuePair in sbsRequest.Headers)
				{
					stringBuilder.AppendLine(string.Format("{0}: {1}", keyValuePair.Key, keyValuePair.Value));
				}
				stringBuilder.Append("Body: ");
				stringBuilder.AppendLine((!string.IsNullOrEmpty(sbsRequest.Body)) ? ("\n" + sbsRequest.Body) : "<EMPTY BODY>");
				Log.Info(new object[]
				{
					stringBuilder.ToString()
				});
			}
		}

		// Token: 0x06001A20 RID: 6688 RVA: 0x000758DC File Offset: 0x00073CDC
		protected void LogResponse(SbsResponse sbsResponse)
		{
			if (Log.LogSbsRequest)
			{
				StringBuilder stringBuilder = new StringBuilder();
				int statusCode = (int)sbsResponse.StatusCode;
				stringBuilder.AppendLine(string.Concat(new object[]
				{
					"SBS Response: ",
					statusCode,
					" ",
					sbsResponse.StatusCode
				}));
				stringBuilder.AppendLine("Etag: " + ((!string.IsNullOrEmpty(sbsResponse.ETag)) ? sbsResponse.ETag : "<none>"));
				stringBuilder.Append("Body: ");
				stringBuilder.AppendLine((!string.IsNullOrEmpty(sbsResponse.BodyString)) ? ("\n" + sbsResponse.BodyString) : "<EMPTY BODY>");
				Log.Info(new object[]
				{
					stringBuilder.ToString()
				});
			}
		}

		// Token: 0x06001A21 RID: 6689 RVA: 0x000759BB File Offset: 0x00073DBB
		private bool ShouldFallbackToIPv4()
		{
			return this._numErrorsIPV6 >= 2;
		}

		// Token: 0x0400486C RID: 18540
		private const string HEADER_SIGNATURE_CONFIG = "X-SBS-Config-Signature";

		// Token: 0x0400486D RID: 18541
		private const string HEADER_SIGNATURE_ALGO = "X-SBS-Config-Signature-Algo";

		// Token: 0x0400486E RID: 18542
		private const string HEADER_TIMESTAMP = "X-SBS-DATE";

		// Token: 0x0400486F RID: 18543
		private const string HEADER_SIGNATURE = "X-SBS-SIGNATURE";

		// Token: 0x04004870 RID: 18544
		private const string HEADER_SBS_ID = "X-SBS-ID";

		// Token: 0x04004871 RID: 18545
		private const string HEADER_IF_NONE_MATCH = "If-None-Match";

		// Token: 0x04004872 RID: 18546
		private const string HEADER_IF_MATCH = "If-Match";

		// Token: 0x04004873 RID: 18547
		private const string HEADER_CONTENT_TYPE = "Content-Type";

		// Token: 0x04004874 RID: 18548
		private const string HEADER_CLIENT_VERSION = "X-Client-Version";

		// Token: 0x04004875 RID: 18549
		private const int MAX_ATTEMPTS_IPV6 = 2;

		// Token: 0x04004876 RID: 18550
		private readonly string _sbsId;

		// Token: 0x04004877 RID: 18551
		private int _numErrorsIPV6;
	}
}
