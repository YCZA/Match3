using System;
using System.Collections.Generic;
using Wooga.Coroutines;

namespace Match3.Scripts1.Wooga.Services.AssetBundleManager
{
	// Token: 0x0200031A RID: 794
	public class FallbackAssetBundleResolver : IAssetBundleResolver
	{
		// Token: 0x060018CE RID: 6350 RVA: 0x00070A88 File Offset: 0x0006EE88
		public FallbackAssetBundleResolver(List<IAssetBundleResolver> resolvers = null)
		{
			this._resolvers = (resolvers ?? new List<IAssetBundleResolver>());
		}

		// Token: 0x060018CF RID: 6351 RVA: 0x00070AAE File Offset: 0x0006EEAE
		public void AddResolver(IAssetBundleResolver resolver)
		{
			this._resolvers.Add(resolver);
		}

		// Token: 0x060018D0 RID: 6352 RVA: 0x00070ABC File Offset: 0x0006EEBC
		public IEnumerator<bool> Contains(string bundleName)
		{
			return this.GetResolver(bundleName).ContinueWith((OptionalResult<IAssetBundleResolver> resolver) => resolver.HasValue);
		}

		// Token: 0x060018D1 RID: 6353 RVA: 0x00070AE8 File Offset: 0x0006EEE8
		public IEnumerator<IList<string>> GetDependencies(string bundleName)
		{
			return this.GetResolver(bundleName).ContinueWith(delegate(OptionalResult<IAssetBundleResolver> resolver)
			{
				if (resolver.HasValue)
				{
					return resolver.Value.GetDependencies(bundleName);
				}
				throw new AssetBundleNotFoundException(bundleName);
			});
		}

		// Token: 0x060018D2 RID: 6354 RVA: 0x00070B20 File Offset: 0x0006EF20
		public IEnumerator<IBundleInfo> Resolve(string bundleName)
		{
			return this.GetResolver(bundleName).ContinueWith(delegate(OptionalResult<IAssetBundleResolver> resolver)
			{
				if (resolver.HasValue)
				{
					return resolver.Value.Resolve(bundleName);
				}
				throw new AssetBundleNotFoundException(bundleName);
			});
		}

		// Token: 0x060018D3 RID: 6355 RVA: 0x00070B58 File Offset: 0x0006EF58
		public IEnumerator<OptionalResult<IBundleInfo>> TryResolve(string bundleName)
		{
			return this.GetResolver(bundleName).ContinueWith((OptionalResult<IAssetBundleResolver> resolver) => (!resolver.HasValue) ? OptionalResult<IBundleInfo>.None.Yield<OptionalResult<IBundleInfo>>() : resolver.Value.TryResolve(bundleName));
		}

		// Token: 0x060018D4 RID: 6356 RVA: 0x00070B90 File Offset: 0x0006EF90
		public string GetUrlForAsset(IBundleInfo bundleInfo, bool cached)
		{
			IAssetBundleResolver assetBundleResolver;
			if (this._cachedResolvers.TryGetValue(bundleInfo.Name, out assetBundleResolver))
			{
				return assetBundleResolver.GetUrlForAsset(bundleInfo, cached);
			}
			throw new Exception("Bundle Info has not beed resolved through this resolver: " + bundleInfo.Name);
		}

		// Token: 0x060018D5 RID: 6357 RVA: 0x00070BD4 File Offset: 0x0006EFD4
		public string[] GetUrlsForAsset(IBundleInfo bundleInfo, bool cached)
		{
			IAssetBundleResolver assetBundleResolver;
			if (this._cachedResolvers.TryGetValue(bundleInfo.Name, out assetBundleResolver))
			{
				return assetBundleResolver.GetUrlsForAsset(bundleInfo, cached);
			}
			throw new Exception("Bundle Info has not beed resolved through this resolver: " + bundleInfo.Name);
		}

		// Token: 0x060018D6 RID: 6358 RVA: 0x00070C18 File Offset: 0x0006F018
		private IEnumerator<OptionalResult<IAssetBundleResolver>> GetResolver(string bundleName)
		{
			IAssetBundleResolver value;
			if (this._cachedResolvers.TryGetValue(bundleName, out value))
			{
				return OptionalResult.Some<IAssetBundleResolver>(value).Yield<OptionalResult<IAssetBundleResolver>>();
			}
			return this.GetResolver(0, bundleName).ContinueWith(delegate(OptionalResult<IAssetBundleResolver> resolver)
			{
				if (resolver.HasValue)
				{
					this._cachedResolvers[bundleName] = resolver.Value;
				}
				return resolver;
			});
		}

		// Token: 0x060018D7 RID: 6359 RVA: 0x00070C7C File Offset: 0x0006F07C
		private IEnumerator<OptionalResult<IAssetBundleResolver>> GetResolver(int currentResolver, string bundleName)
		{
			for (int i = currentResolver; i < this._resolvers.Count; i++)
			{
				ISyncAssetBundleResolver syncAssetBundleResolver = this._resolvers[i] as ISyncAssetBundleResolver;
				if (syncAssetBundleResolver == null)
				{
					return this.GetResolverAsync(i, bundleName);
				}
				if (syncAssetBundleResolver.Contains(bundleName))
				{
					return OptionalResult.Some<IAssetBundleResolver>(this._resolvers[i]).Yield<OptionalResult<IAssetBundleResolver>>();
				}
			}
			return OptionalResult<IAssetBundleResolver>.None.Yield<OptionalResult<IAssetBundleResolver>>();
		}

		// Token: 0x060018D8 RID: 6360 RVA: 0x00070CF8 File Offset: 0x0006F0F8
		private IEnumerator<OptionalResult<IAssetBundleResolver>> GetResolverAsync(int i, string bundleName)
		{
			return this._resolvers[i].Contains(bundleName).ContinueWith(delegate(bool result)
			{
				if (result)
				{
					return OptionalResult.Some<IAssetBundleResolver>(this._resolvers[i]).Yield<OptionalResult<IAssetBundleResolver>>();
				}
				if (i == this._resolvers.Count)
				{
					return OptionalResult<IAssetBundleResolver>.None.Yield<OptionalResult<IAssetBundleResolver>>();
				}
				return this.GetResolverAsync(i + 1, bundleName);
			});
		}

		// Token: 0x040047DE RID: 18398
		private readonly Dictionary<string, IAssetBundleResolver> _cachedResolvers = new Dictionary<string, IAssetBundleResolver>();

		// Token: 0x040047DF RID: 18399
		private readonly List<IAssetBundleResolver> _resolvers;
	}
}
