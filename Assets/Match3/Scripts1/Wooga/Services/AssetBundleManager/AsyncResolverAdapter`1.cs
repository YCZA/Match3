using System.Collections.Generic;
using Wooga.Coroutines;

namespace Match3.Scripts1.Wooga.Services.AssetBundleManager
{
	// Token: 0x0200030D RID: 781
	public abstract class AsyncResolverAdapter<T> : IAssetBundleResolver where T : AsyncResolverAdapter<T>, ISyncAssetBundleResolver
	{
		// Token: 0x0600187B RID: 6267 RVA: 0x0006F754 File Offset: 0x0006DB54
		public IEnumerator<bool> Contains(string bundleName)
		{
			yield return ((ISyncAssetBundleResolver)this).Contains(bundleName);
			yield break;
		}

		// Token: 0x0600187C RID: 6268 RVA: 0x0006F778 File Offset: 0x0006DB78
		public IEnumerator<IList<string>> GetDependencies(string bundleName)
		{
			yield return ((ISyncAssetBundleResolver)this).GetDependencies(bundleName);
			yield break;
		}

		// Token: 0x0600187D RID: 6269 RVA: 0x0006F79C File Offset: 0x0006DB9C
		public IEnumerator<IBundleInfo> Resolve(string bundleName)
		{
			yield return ((ISyncAssetBundleResolver)this).Resolve(bundleName);
			yield break;
		}

		// Token: 0x0600187E RID: 6270 RVA: 0x0006F7C0 File Offset: 0x0006DBC0
		public IEnumerator<OptionalResult<IBundleInfo>> TryResolve(string bundleName)
		{
			IBundleInfo bundleInfo;
			if (((ISyncAssetBundleResolver)this).TryResolve(bundleName, out bundleInfo))
			{
				yield return OptionalResult.Some<IBundleInfo>(bundleInfo);
			}
			else
			{
				yield return OptionalResult.None;
			}
			yield break;
		}

		// Token: 0x0600187F RID: 6271
		public abstract string GetUrlForAsset(IBundleInfo bundleInfo, bool cached);

		// Token: 0x06001880 RID: 6272
		public abstract string[] GetUrlsForAsset(IBundleInfo bundleInfo, bool cached);
	}
}
