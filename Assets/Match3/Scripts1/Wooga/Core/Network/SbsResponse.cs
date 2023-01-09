using System;
using System.Collections.Generic;
using System.Net;
using Wooga.Foundation.Json;
using Wooga.Core.Extensions;

namespace Match3.Scripts1.Wooga.Core.Network
{
	// Token: 0x02000366 RID: 870
	public class SbsResponse
	{
		// Token: 0x06001A43 RID: 6723 RVA: 0x00075E0C File Offset: 0x0007420C
		public SbsResponse(Exception ex)
		{
			if (ex is WebException && ((WebException)ex).Response != null)
			{
				HttpWebResponse httpWebResponse = (HttpWebResponse)((WebException)ex).Response;
				this.StatusCode = httpWebResponse.StatusCode;
			}
			else
			{
				this.StatusCode = HttpStatusCode.BadRequest;
			}
			this.Exceptions = new List<Exception>();
			this.Exceptions.Add(ex);
			this.BodyString = null;
		}

		// Token: 0x06001A44 RID: 6724 RVA: 0x00075E85 File Offset: 0x00074285
		public SbsResponse(string bodyJson, HttpStatusCode statusCode)
		{
			this.BodyString = bodyJson;
			this.StatusCode = statusCode;
			this.Exceptions = new List<Exception>();
		}

		// Token: 0x17000427 RID: 1063
		// (get) Token: 0x06001A45 RID: 6725 RVA: 0x00075EA6 File Offset: 0x000742A6
		// (set) Token: 0x06001A46 RID: 6726 RVA: 0x00075EAE File Offset: 0x000742AE
		public HttpStatusCode StatusCode { get; set; }

		// Token: 0x17000428 RID: 1064
		// (get) Token: 0x06001A47 RID: 6727 RVA: 0x00075EB7 File Offset: 0x000742B7
		// (set) Token: 0x06001A48 RID: 6728 RVA: 0x00075EBF File Offset: 0x000742BF
		public string BodyString { get; set; }

		// Token: 0x17000429 RID: 1065
		// (get) Token: 0x06001A49 RID: 6729 RVA: 0x00075EC8 File Offset: 0x000742C8
		public string ETag
		{
			get
			{
				return this.Headers.GetHeaderValue("ETag");
			}
		}

		// Token: 0x1700042A RID: 1066
		// (get) Token: 0x06001A4A RID: 6730 RVA: 0x00075EDA File Offset: 0x000742DA
		// (set) Token: 0x06001A4B RID: 6731 RVA: 0x00075EE2 File Offset: 0x000742E2
		public string ContentType { get; set; }

		// Token: 0x1700042B RID: 1067
		// (get) Token: 0x06001A4C RID: 6732 RVA: 0x00075EEB File Offset: 0x000742EB
		// (set) Token: 0x06001A4D RID: 6733 RVA: 0x00075EF3 File Offset: 0x000742F3
		public string Signature { get; set; }

		// Token: 0x1700042C RID: 1068
		// (get) Token: 0x06001A4E RID: 6734 RVA: 0x00075EFC File Offset: 0x000742FC
		// (set) Token: 0x06001A4F RID: 6735 RVA: 0x00075F04 File Offset: 0x00074304
		public string SignatureConfig { get; set; }

		// Token: 0x1700042D RID: 1069
		// (get) Token: 0x06001A50 RID: 6736 RVA: 0x00075F0D File Offset: 0x0007430D
		// (set) Token: 0x06001A51 RID: 6737 RVA: 0x00075F15 File Offset: 0x00074315
		public string SignatureAlgo { get; set; }

		// Token: 0x1700042E RID: 1070
		// (get) Token: 0x06001A52 RID: 6738 RVA: 0x00075F1E File Offset: 0x0007431E
		// (set) Token: 0x06001A53 RID: 6739 RVA: 0x00075F26 File Offset: 0x00074326
		public string Timestamp { get; set; }

		// Token: 0x1700042F RID: 1071
		// (get) Token: 0x06001A54 RID: 6740 RVA: 0x00075F2F File Offset: 0x0007432F
		// (set) Token: 0x06001A55 RID: 6741 RVA: 0x00075F37 File Offset: 0x00074337
		public Dictionary<string, string> Headers { get; set; }

		// Token: 0x17000430 RID: 1072
		// (get) Token: 0x06001A56 RID: 6742 RVA: 0x00075F40 File Offset: 0x00074340
		// (set) Token: 0x06001A57 RID: 6743 RVA: 0x00075F48 File Offset: 0x00074348
		public IList<Exception> Exceptions { get; set; }

		// Token: 0x06001A58 RID: 6744 RVA: 0x00075F54 File Offset: 0x00074354
		public IDictionary<string, JSONNode> ParseBody()
		{
			IDictionary<string, JSONNode> result;
			try
			{
				result = (JSONObject)JSON.Deserialize(this.BodyString);
			}
			catch (Exception)
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06001A59 RID: 6745 RVA: 0x00075F90 File Offset: 0x00074390
		public T ParseBody<T>()
		{
			T result;
			try
			{
				result = JSON.Deserialize<T>(this.BodyString);
			}
			catch (Exception)
			{
				result = default(T);
			}
			return result;
		}

		// Token: 0x06001A5A RID: 6746 RVA: 0x00075FD0 File Offset: 0x000743D0
		public bool HasError()
		{
			int statusCode = (int)this.StatusCode;
			return statusCode >= 400 || this.Exceptions.Count > 0;
		}

		// Token: 0x0400488C RID: 18572
		public const string HEADER_ETAG = "ETag";
	}
}
