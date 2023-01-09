using Match3.Scripts1.Wooga.Core.Network;

namespace Wooga.Core.Utilities.RequestCache
{
	// Token: 0x020003AD RID: 941
	public class NullRequestCache : IRequestCache
	{
		// Token: 0x06001C56 RID: 7254 RVA: 0x0007C637 File Offset: 0x0007AA37
		public void Clear()
		{
		}

		// Token: 0x06001C57 RID: 7255 RVA: 0x0007C639 File Offset: 0x0007AA39
		public void Cache(string path, string body, string etag)
		{
		}

		// Token: 0x06001C58 RID: 7256 RVA: 0x0007C63B File Offset: 0x0007AA3B
		public void Cache(SbsRequest request, SbsResponse response)
		{
		}

		// Token: 0x06001C59 RID: 7257 RVA: 0x0007C63D File Offset: 0x0007AA3D
		public string GetResource(string path)
		{
			return string.Empty;
		}

		// Token: 0x06001C5A RID: 7258 RVA: 0x0007C644 File Offset: 0x0007AA44
		public string GetEtagForPath(string path)
		{
			return string.Empty;
		}

		// Token: 0x06001C5B RID: 7259 RVA: 0x0007C64B File Offset: 0x0007AA4B
		public void SetEtag(SbsRequest sbsRequest, SbsResponse sbsResponse)
		{
		}

		// Token: 0x06001C5C RID: 7260 RVA: 0x0007C64D File Offset: 0x0007AA4D
		public void Remove(string path)
		{
		}
	}
}
