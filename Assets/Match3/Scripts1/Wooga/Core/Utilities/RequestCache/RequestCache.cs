using System;
using Match3.Scripts1.Wooga.Core.Network;

namespace Wooga.Core.Utilities.RequestCache
{
	// Token: 0x020003AE RID: 942
	public class RequestCache : IRequestCache
	{
		// Token: 0x06001C5D RID: 7261 RVA: 0x0007C64F File Offset: 0x0007AA4F
		public RequestCache(IStorageStrategy storage)
		{
			this.Storage = storage;
		}

		// Token: 0x06001C5E RID: 7262 RVA: 0x0007C65E File Offset: 0x0007AA5E
		public void Clear()
		{
			this.Storage.Clear();
		}

		// Token: 0x06001C5F RID: 7263 RVA: 0x0007C66B File Offset: 0x0007AA6B
		public void Cache(string path, string body, string etag)
		{
			this.Storage.Store(path, body, etag);
		}

		// Token: 0x06001C60 RID: 7264 RVA: 0x0007C67C File Offset: 0x0007AA7C
		[Obsolete("This method should not be used anymore to avoid side effects. Instead use IRequestCache.Cache (string path, string body, string etag)")]
		public void Cache(SbsRequest request, SbsResponse response)
		{
			int statusCode = (int)response.StatusCode;
			if (statusCode >= 200 && statusCode < 300)
			{
				HttpMethod method = request.Method;
				if (method != HttpMethod.GET)
				{
					if (method == HttpMethod.PUT || method == HttpMethod.POST)
					{
						this.Storage.Store(request.Path, request.Body, response.ETag);
					}
				}
				else
				{
					this.Storage.Store(request.Path, response.BodyString, response.ETag);
				}
			}
		}

		// Token: 0x06001C61 RID: 7265 RVA: 0x0007C70A File Offset: 0x0007AB0A
		public string GetResource(string path)
		{
			return this.Storage.GetValue(path);
		}

		// Token: 0x06001C62 RID: 7266 RVA: 0x0007C718 File Offset: 0x0007AB18
		public string GetEtagForPath(string path)
		{
			return this.Storage.GetEtagForPath(path);
		}

		// Token: 0x06001C63 RID: 7267 RVA: 0x0007C726 File Offset: 0x0007AB26
		public void SetEtag(SbsRequest sbsRequest, SbsResponse sbsResponse)
		{
			this.Storage.SetEtagForPath(sbsRequest.Path, sbsResponse.ETag);
		}

		// Token: 0x06001C64 RID: 7268 RVA: 0x0007C73F File Offset: 0x0007AB3F
		public void Remove(string path)
		{
			this.Storage.RemoveValue(path);
		}

		// Token: 0x04004996 RID: 18838
		public IStorageStrategy Storage;
	}
}
