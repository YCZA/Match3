using System.Collections.Generic;
using Wooga.Coroutines;

namespace Match3.Scripts1.Wooga.Services.AssetBundleManager
{
	// Token: 0x02000315 RID: 789
	public class AssetBundleResolver : IAssetBundleResolver
	{
		// Token: 0x060018A4 RID: 6308 RVA: 0x00070129 File Offset: 0x0006E529
		public AssetBundleResolver(IAssetBundleResolver mainResolver, StreamingAssetsResolver streamingAssetsResolver = null)
		{
			this._mainResolver = mainResolver;
			this._streamingAssetsResolver = streamingAssetsResolver;
		}

		// Token: 0x060018A5 RID: 6309 RVA: 0x0007014A File Offset: 0x0006E54A
		public IEnumerator<bool> Contains(string bundleName)
		{
			return (!this.IsBundledInApp(bundleName)) ? this._mainResolver.Contains(bundleName) : true.Yield<bool>();
		}

		// Token: 0x060018A6 RID: 6310 RVA: 0x00070170 File Offset: 0x0006E570
		public IEnumerator<IList<string>> GetDependencies(string bundleName)
		{
			IBundleInfo iaBundleInfo;
			if (!this.TryResolveInApp(bundleName, out iaBundleInfo))
			{
				return this._mainResolver.GetDependencies(bundleName);
			}
			ISyncAssetBundleResolver syncAssetBundleResolver = this._mainResolver as ISyncAssetBundleResolver;
			if (syncAssetBundleResolver == null)
			{
				return this._mainResolver.TryResolve(bundleName).ContinueWith(delegate(OptionalResult<IBundleInfo> maybeBundle)
				{
					if (maybeBundle.HasValue)
					{
						IBundleInfo value = maybeBundle.Value;
						return (!(value.Hash128 != iaBundleInfo.Hash128)) ? this._streamingAssetsResolver.GetDependencies(bundleName).Yield<IList<string>>() : this._mainResolver.GetDependencies(bundleName);
					}
					return this._streamingAssetsResolver.GetDependencies(bundleName).Yield<IList<string>>();
				});
			}
			if (syncAssetBundleResolver.Contains(bundleName))
			{
				IBundleInfo bundleInfo = syncAssetBundleResolver.Resolve(bundleName);
				return (!(bundleInfo.Hash128 != iaBundleInfo.Hash128)) ? this._streamingAssetsResolver.GetDependencies(bundleName).Yield<IList<string>>() : this._mainResolver.GetDependencies(bundleName);
			}
			return this._streamingAssetsResolver.GetDependencies(bundleName).Yield<IList<string>>();
		}

		// Token: 0x060018A7 RID: 6311 RVA: 0x0007026C File Offset: 0x0006E66C
		public IEnumerator<IBundleInfo> Resolve(string bundleName)
		{
			IBundleInfo iaBundleInfo;
			if (this.TryResolveInApp(bundleName, out iaBundleInfo))
			{
				// Debug.LogError("3+"+bundleName);
				return this._mainResolver.TryResolve(bundleName).ContinueWith(delegate(OptionalResult<IBundleInfo> maybeBundle)
				{
					if (maybeBundle.HasValue)
					{
						IBundleInfo value = maybeBundle.Value;
						if (value.Hash128 != iaBundleInfo.Hash128)
						{
							return value;
						}
					}
					this._inAppBundleCache.Add(iaBundleInfo.Name);
					return iaBundleInfo;
				});
			}
			// Debug.LogError("4+" + bundleName);
			return this._mainResolver.Resolve(bundleName);
		}

		// Token: 0x060018A8 RID: 6312 RVA: 0x000702C4 File Offset: 0x0006E6C4
		public IEnumerator<OptionalResult<IBundleInfo>> TryResolve(string bundleName)
		{
			IBundleInfo iaBundleInfo;
			if (this.TryResolveInApp(bundleName, out iaBundleInfo))
			{
				return this._mainResolver.TryResolve(bundleName).ContinueWith(delegate(OptionalResult<IBundleInfo> maybeBundle)
				{
					IBundleInfo value = iaBundleInfo;
					if (maybeBundle.HasValue)
					{
						IBundleInfo value2 = maybeBundle.Value;
						if (value2.Hash128 != iaBundleInfo.Hash128)
						{
							value = value2;
						}
					}
					return OptionalResult.Some<IBundleInfo>(value);
				});
			}
			return this._mainResolver.TryResolve(bundleName);
		}

		// Token: 0x060018A9 RID: 6313 RVA: 0x00070313 File Offset: 0x0006E713
		public string GetUrlForAsset(IBundleInfo bundleInfo, bool cached)
		{
			return (!this._inAppBundleCache.Contains(bundleInfo.Name)) ? this._mainResolver.GetUrlForAsset(bundleInfo, cached) : this._streamingAssetsResolver.GetUrlForAsset(bundleInfo, cached);
		}

		// Token: 0x060018AA RID: 6314 RVA: 0x0007034A File Offset: 0x0006E74A
		public string[] GetUrlsForAsset(IBundleInfo bundleInfo, bool cached)
		{
			string[] result;
			if (this._inAppBundleCache.Contains(bundleInfo.Name))
			{
				(result = new string[1])[0] = this._streamingAssetsResolver.GetUrlForAsset(bundleInfo, cached);
			}
			else
			{
				result = this._mainResolver.GetUrlsForAsset(bundleInfo, cached);
			}
			return result;
		}

		// Token: 0x060018AB RID: 6315 RVA: 0x0007038A File Offset: 0x0006E78A
		private bool IsBundledInApp(string bundleName)
		{
			return this._streamingAssetsResolver != null && this._streamingAssetsResolver.Contains(bundleName);
		}

		// Token: 0x060018AC RID: 6316 RVA: 0x000703A6 File Offset: 0x0006E7A6
		private bool TryResolveInApp(string bundleName, out IBundleInfo bundleInfo)
		{
			if (this._streamingAssetsResolver != null && this._streamingAssetsResolver.Contains(bundleName))
			{
				bundleInfo = this._streamingAssetsResolver.Resolve(bundleName);
				return true;
			}
			bundleInfo = null;
			return false;
		}

		// Token: 0x040047CA RID: 18378
		private readonly HashSet<string> _inAppBundleCache = new HashSet<string>();

		// Token: 0x040047CB RID: 18379
		private readonly ISyncAssetBundleResolver _streamingAssetsResolver;

		// Token: 0x040047CC RID: 18380
		private readonly IAssetBundleResolver _mainResolver;
	}
}
