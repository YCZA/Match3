using System.Collections.Generic;

namespace Match3.Scripts1.Wooga.Services.AssetBundleManager
{
	// Token: 0x0200030B RID: 779
	public interface ISyncAssetBundleResolver
	{
		// Token: 0x0600186F RID: 6255
		bool Contains(string bundleName);

		// Token: 0x06001870 RID: 6256
		IList<string> GetDependencies(string bundleName);

		// Token: 0x06001871 RID: 6257
		IBundleInfo Resolve(string bundleName);

		// Token: 0x06001872 RID: 6258
		bool TryResolve(string bundleName, out IBundleInfo bundleInfo);

		// Token: 0x06001873 RID: 6259
		string GetUrlForAsset(IBundleInfo bundleInfo, bool cached);
	}
}
