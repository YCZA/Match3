using System.Collections.Generic;
using System.Linq;

namespace Match3.Scripts1.Wooga.Services.AssetBundleManager
{
	// Token: 0x02000314 RID: 788
	public abstract class ABundleManifestResolver : AsyncResolverAdapter<ABundleManifestResolver>, ISyncAssetBundleResolver
	{
		// Token: 0x06001893 RID: 6291 RVA: 0x0006FE10 File Offset: 0x0006E210
		protected ABundleManifestResolver(BundleManifest manifest, IEnumerable<string> activeVariants = null)
		{
			foreach (BundleInfo bundleInfo in manifest.BundleInfos)
			{
				AssetBundleName key = new AssetBundleName(bundleInfo.Name, bundleInfo.Variant);
				this._bundles[key] = bundleInfo;
			}
			this._variants = ABundleManifestResolver.InitializeVariants(manifest, activeVariants ?? Enumerable.Empty<string>());
		}

		// Token: 0x06001894 RID: 6292 RVA: 0x0006FEB0 File Offset: 0x0006E2B0
		bool ISyncAssetBundleResolver.Contains(string bundleName)
		{
			return this._variants.ContainsKey(this.BaseName(bundleName));
		}

		// Token: 0x06001895 RID: 6293 RVA: 0x0006FEC4 File Offset: 0x0006E2C4
		IList<string> ISyncAssetBundleResolver.GetDependencies(string baseName)
		{
			AssetBundleName key = this.ResolveBundleName(baseName);
			return this._bundles[key].Dependencies;
		}

		// Token: 0x06001896 RID: 6294 RVA: 0x0006FEEC File Offset: 0x0006E2EC
		IBundleInfo ISyncAssetBundleResolver.Resolve(string baseName)
		{
			AssetBundleName key = this.ResolveBundleName(baseName);
			return this._bundles[key];
		}

		// Token: 0x06001897 RID: 6295 RVA: 0x0006FF10 File Offset: 0x0006E310
		bool ISyncAssetBundleResolver.TryResolve(string baseName, out IBundleInfo bundleInfo)
		{
			AssetBundleName key;
			if (this.TryResolveBundleName(baseName, out key))
			{
				bundleInfo = this._bundles[key];
				return true;
			}
			bundleInfo = null;
			return false;
		}

		// Token: 0x06001898 RID: 6296 RVA: 0x0006FF3F File Offset: 0x0006E33F
		protected bool TryResolveBundleName(string bundleName, out AssetBundleName assetBundleName)
		{
			return this._variants.TryGetValue(this.BaseName(bundleName), out assetBundleName);
		}

		// Token: 0x06001899 RID: 6297 RVA: 0x0006FF54 File Offset: 0x0006E354
		protected AssetBundleName ResolveBundleName(string bundleName)
		{
			AssetBundleName result;
			if (!this._variants.TryGetValue(this.BaseName(bundleName), out result))
			{
				throw new AssetBundleNotFoundException(bundleName);
			}
			return result;
		}

		// Token: 0x0600189A RID: 6298 RVA: 0x0006FF84 File Offset: 0x0006E384
		protected string BaseName(string bundleName)
		{
			int num = bundleName.IndexOf('.');
			return (num < 0) ? bundleName : bundleName.Substring(0, num);
		}

		// Token: 0x0600189B RID: 6299 RVA: 0x0006FFB0 File Offset: 0x0006E3B0
		private static Dictionary<string, AssetBundleName> InitializeVariants(BundleManifest manifest, IEnumerable<string> activeVariants)
		{
			return (from bundle in manifest
			select new AssetBundleName(bundle.Name, bundle.Variant) into n
			group n by n.BaseName).ToDictionary((IGrouping<string, AssetBundleName> g) => g.Key, (IGrouping<string, AssetBundleName> g) => ABundleManifestResolver.BestMatch(g, activeVariants));
		}

		// Token: 0x0600189C RID: 6300 RVA: 0x00070040 File Offset: 0x0006E440
		private static AssetBundleName BestMatch(IEnumerable<AssetBundleName> names, IEnumerable<string> activeVariants)
		{
			return (from variant in activeVariants
			join name in names on variant equals name.Variant
			select name).Concat(names).First<AssetBundleName>();
		}

		// Token: 0x0600189D RID: 6301 RVA: 0x000700B6 File Offset: 0x0006E4B6
		public override string ToString()
		{
			return string.Format("[{0}({1})]", base.GetType(), string.Join(",", this._variants.Keys.ToArray<string>()));
		}

		// Token: 0x040047C2 RID: 18370
		protected readonly Dictionary<AssetBundleName, BundleInfo> _bundles = new Dictionary<AssetBundleName, BundleInfo>();

		// Token: 0x040047C3 RID: 18371
		protected readonly Dictionary<string, AssetBundleName> _variants;
	}
}
