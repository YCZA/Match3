using System.Collections.Generic;

namespace Match3.Scripts1.Wooga.Services.AssetBundleManager
{
	// Token: 0x0200030C RID: 780
	public interface IAssetBundleResolver
	{
		// Token: 0x06001874 RID: 6260
		IEnumerator<bool> Contains(string bundleName);

		// Token: 0x06001875 RID: 6261
		IEnumerator<IList<string>> GetDependencies(string bundleName);

		// Token: 0x06001876 RID: 6262
		IEnumerator<IBundleInfo> Resolve(string bundleName);

		// Token: 0x06001877 RID: 6263
		IEnumerator<OptionalResult<IBundleInfo>> TryResolve(string bundleName);

		// Token: 0x06001878 RID: 6264
		string GetUrlForAsset(IBundleInfo bundleInfo, bool cached);

		// Token: 0x06001879 RID: 6265
		string[] GetUrlsForAsset(IBundleInfo bundleInfo, bool cached);
	}
}
