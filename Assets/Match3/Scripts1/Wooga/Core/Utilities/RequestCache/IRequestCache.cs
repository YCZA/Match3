using System;
using Match3.Scripts1.Wooga.Core.Network;

namespace Wooga.Core.Utilities.RequestCache
{
	// Token: 0x020003AC RID: 940
	public interface IRequestCache
	{
		// Token: 0x06001C4E RID: 7246
		void Clear();

		// Token: 0x06001C4F RID: 7247
		void Cache(string path, string body, string etag);

		// Token: 0x06001C50 RID: 7248
		[Obsolete("This method should not be used anymore to avoid side effects. Instead use IRequestCache.Cache (string path, string body, string etag)")]
		void Cache(SbsRequest request, SbsResponse response);

		// Token: 0x06001C51 RID: 7249
		string GetResource(string path);

		// Token: 0x06001C52 RID: 7250
		string GetEtagForPath(string path);

		// Token: 0x06001C53 RID: 7251
		void SetEtag(SbsRequest sbsRequest, SbsResponse sbsResponse);

		// Token: 0x06001C54 RID: 7252
		void Remove(string path);
	}
}
