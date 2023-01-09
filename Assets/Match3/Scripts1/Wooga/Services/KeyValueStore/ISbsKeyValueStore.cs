using System.Collections.Generic;

namespace Match3.Scripts1.Wooga.Services.KeyValueStore
{
	// Token: 0x0200040C RID: 1036
	public interface ISbsKeyValueStore
	{
		// Token: 0x06001EAE RID: 7854
		IEnumerator<SbsWriteResult> WriteJsonToBucket(string bucket, string json, int formatVersion, SbsMergeHandler mergeHandler, string etag = null);

		// Token: 0x06001EAF RID: 7855
		IEnumerator<SbsWriteResult> WriteToBucket(string bucket, string data, int formatVersion, SbsMergeHandler mergeHandler, string etag = null);

		// Token: 0x06001EB0 RID: 7856
		IEnumerator<SbsReadResult> ReadFromBucket(string bucket, string sbsUserId = null, int timeoutInSeconds = -1);

		// Token: 0x06001EB1 RID: 7857
		void ClearRequestCache();
	}
}
