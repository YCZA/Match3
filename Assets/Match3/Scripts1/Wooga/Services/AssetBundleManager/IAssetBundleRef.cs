using System;
using UnityEngine;

namespace Match3.Scripts1.Wooga.Services.AssetBundleManager
{
	// Token: 0x0200030A RID: 778
	public interface IAssetBundleRef : IDisposable
	{
		// Token: 0x170003C7 RID: 967
		// (get) Token: 0x06001864 RID: 6244
		AssetBundle AssetBundle { get; }

		// Token: 0x170003C8 RID: 968
		// (get) Token: 0x06001865 RID: 6245
		bool NeedsLoading { get; }

		// Token: 0x170003C9 RID: 969
		// (get) Token: 0x06001866 RID: 6246
		bool IsLoaded { get; }

		// Token: 0x170003CA RID: 970
		// (get) Token: 0x06001867 RID: 6247
		bool IsLoading { get; }

		// Token: 0x170003CB RID: 971
		// (get) Token: 0x06001868 RID: 6248
		bool HasLoadingFailed { get; }

		// Token: 0x170003CC RID: 972
		// (get) Token: 0x06001869 RID: 6249
		Exception FailedReason { get; }

		// Token: 0x170003CD RID: 973
		// (get) Token: 0x0600186A RID: 6250
		IBundleInfo BundleInfo { get; }

		// Token: 0x170003CE RID: 974
		// (get) Token: 0x0600186B RID: 6251
		string BundleURI { get; }

		// Token: 0x0600186C RID: 6252
		void MarkLoadingFailed(Exception e = null);

		// Token: 0x0600186D RID: 6253
		void MarkLoading();

		// Token: 0x0600186E RID: 6254
		void PrepareToRetryFailedDownload();
	}
}
