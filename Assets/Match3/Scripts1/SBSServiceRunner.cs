using System.Collections;
using System.Collections.Generic;
using System.Text;
using Match3.Scripts1.Wooga.Core.Exceptions.Network;
using Match3.Scripts1.Wooga.Core.Network;
using Wooga.Coroutines;

// Token: 0x020007FE RID: 2046
namespace Match3.Scripts1
{
	public class SBSServiceRunner
	{
		// Token: 0x0600327E RID: 12926 RVA: 0x000EDC80 File Offset: 0x000EC080
		public SBSServiceRunner(ISbsNetworking sbsNetwork)
		{
			this.sbsNetwork = sbsNetwork;
		}

		// Token: 0x0600327F RID: 12927 RVA: 0x000EDC8F File Offset: 0x000EC08F
		public Wooroutine<SbsResponse> WaitForSBSRequest(SbsRequest request)
		{
			return WooroutineRunner.StartWooroutine<SbsResponse>(this.WaitForSBSRequestRoutine(request));
		}

		// Token: 0x06003280 RID: 12928 RVA: 0x000EDCA0 File Offset: 0x000EC0A0
		private IEnumerator WaitForSBSRequestRoutine(SbsRequest request)
		{
			Wooroutine<SbsResponse> response = this.sbsNetwork.Send(request).StartWooroutine<SbsResponse>();
			yield return response;
			SbsResponse result = null;
			try
			{
				result = response.ReturnValue;
			}
			catch (SbsNetworkingRequestFailedException ex)
			{
				WoogaDebug.LogWarning(new object[]
				{
					"failed sbs request: " + ex.Message
				});
			}
			yield return result;
			yield break;
		}

		// Token: 0x06003281 RID: 12929 RVA: 0x000EDCC4 File Offset: 0x000EC0C4
		private void LogResponse(SbsResponse sbsResponse)
		{
			if (sbsResponse == null)
			{
				WoogaDebug.Log(new object[]
				{
					"NULL Response"
				});
				return;
			}
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
			WoogaDebug.Log(new object[]
			{
				stringBuilder.ToString()
			});
		}

		// Token: 0x06003282 RID: 12930 RVA: 0x000EDDB4 File Offset: 0x000EC1B4
		private void LogRequest(SbsRequest sbsRequest)
		{
			if (sbsRequest == null)
			{
				WoogaDebug.Log(new object[]
				{
					"NULL Request"
				});
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(string.Concat(new object[]
			{
				"SBS Request: ",
				sbsRequest.Method,
				" ",
				// string.Format("{0}://{1}{2}", sbsRequest.Protocol, sbsRequest.Host, sbsRequest.Path)
				string.Format("{0}://{1}{2}", "23334", "23334", "23334")
			}));
			stringBuilder.AppendLine("Headers: ");
			foreach (KeyValuePair<string, string> keyValuePair in sbsRequest.Headers)
			{
				stringBuilder.AppendLine(string.Format("{0}: {1}", keyValuePair.Key, keyValuePair.Value));
			}
			stringBuilder.Append("ETag: ");
			stringBuilder.AppendLine((!string.IsNullOrEmpty(sbsRequest.ETag)) ? sbsRequest.ETag : "<NO ETAG>");
			stringBuilder.Append("Body: ");
			stringBuilder.AppendLine((!string.IsNullOrEmpty(sbsRequest.Body)) ? ("\n" + sbsRequest.Body) : "<EMPTY BODY>");
			WoogaDebug.Log(new object[]
			{
				stringBuilder.ToString()
			});
		}

		// Token: 0x04005B03 RID: 23299
		private ISbsNetworking sbsNetwork;
	}
}
