using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Wooga.Coroutines;
using UnityEngine;

namespace Match3.Scripts1.Wooga.Services.AssetBundleManager
{
	// Token: 0x02000301 RID: 769
	public class AssetBundleManagerBuilder
	{
		// Token: 0x06001828 RID: 6184 RVA: 0x0006ED31 File Offset: 0x0006D131
		public AssetBundleManagerBuilder WithActiveVariants(IEnumerable<string> variants)
		{
			this._activeVariants = variants.ToList<string>();
			return this;
		}

		// Token: 0x06001829 RID: 6185 RVA: 0x0006ED40 File Offset: 0x0006D140
		public AssetBundleManagerBuilder WithSBSConfig(string sbsAccount, string sbsEnvironment)
		{
			this._sbsEnvironment = sbsEnvironment;
			this._sbsAccount = sbsAccount;
			return this;
		}

		// Token: 0x0600182A RID: 6186 RVA: 0x0006ED54 File Offset: 0x0006D154
		public AssetBundleManagerBuilder WithBundleManagerConfig(BundleManagerCDNConfig cdnConfig = null)
		{
			if (cdnConfig == null)
			{
				cdnConfig = BundleManagerCDNConfig.TryGet().ValueOrDefault;
			}
			if (cdnConfig != null)
			{
				this._sbsAccount = cdnConfig.SBSAccount;
				this._sbsEnvironment = cdnConfig.Environment;
			}
			return this;
		}

		// Token: 0x0600182B RID: 6187 RVA: 0x0006ED95 File Offset: 0x0006D195
		public AssetBundleManagerBuilder WithManifestResolver(IManifestResolver manifestResolver)
		{
			this._manifestResolver = manifestResolver;
			return this;
		}

		// Token: 0x0600182C RID: 6188 RVA: 0x0006ED9F File Offset: 0x0006D19F
		public AssetBundleManagerBuilder WithManifests(params string[] manifests)
		{
			this._manifestNames.AddRange(manifests);
			return this;
		}

		// Token: 0x0600182D RID: 6189 RVA: 0x0006EDAE File Offset: 0x0006D1AE
		public AssetBundleManagerBuilder WithResolvers(params IAssetBundleResolver[] resolvers)
		{
			this._resolvers.AddRange(resolvers);
			return this;
		}

		// Token: 0x0600182E RID: 6190 RVA: 0x0006EDBD File Offset: 0x0006D1BD
		public AssetBundleManagerBuilder WithEditorFallback(Func<IEnumerator<IAssetBundleManager>> editorFallbackFactory)
		{
			return this;
		}

		// Token: 0x0600182F RID: 6191 RVA: 0x0006EDC0 File Offset: 0x0006D1C0
		public IEnumerator<IAssetBundleManager> Build()
		{
			AssetBundleMode bundleMode = AssetBundleEnvironmentSettings.BundleMode;
			if (this._manifestNames.Count > 0)
			{
				return this.InitBundleResolvers(this._manifestResolver).ContinueWith(() => this.CreateManager());
			}
			return this.CreateManager();
		}

		// Token: 0x06001830 RID: 6192 RVA: 0x0006EDFD File Offset: 0x0006D1FD
		private IEnumerator<IAssetBundleManager> CreateManager()
		{
			return AssetBundleManagerBuilder.WaitForCachingSystem().ContinueWith(() => StreamingAssetsResolver.CreateResolver(this._activeVariants, null)).ContinueWith(delegate(StreamingAssetsResolver saResolver)
			{
				IAssetBundleResolver bundleResolver;
				if (this._resolvers.Count > 1)
				{
					bundleResolver = new AssetBundleResolver(new FallbackAssetBundleResolver(this._resolvers), saResolver);
				}
				else if (this._resolvers.Count == 1)
				{
					bundleResolver = new AssetBundleResolver(this._resolvers[0], saResolver);
				}
				else
				{
					bundleResolver = saResolver;
				}
				return new AssetBundleManager(bundleResolver);
			});
		}

		// Token: 0x06001831 RID: 6193 RVA: 0x0006EE28 File Offset: 0x0006D228
		private IEnumerator InitBundleResolvers(IManifestResolver manifestResolver)
		{
			if (manifestResolver == null)
			{
				manifestResolver = new CDNManifestResolver(this._sbsAccount, this._sbsEnvironment, null, null);
			}
			return (from manifestName in this._manifestNames
			select manifestResolver.Resolve(manifestName)).StartAllAndAwait((IManifestInfo manifestInfo) => ManifestLoader.LoadManifest(manifestInfo.URL, manifestInfo.MD5, null).ContinueWith((BundleManifest manifest) => new
			{
				Manifest = manifest,
				ManifestInfo = manifestInfo
			})).ContinueWith((resolverConfigs)=>
			{
				foreach (var current in resolverConfigs)
				{
					this._resolvers.Add(new CDNAssetsResolver(current.ManifestInfo.OriginBaseURL, current.ManifestInfo.CDNBaseURLs, current.Manifest, this._activeVariants));
				}
			});
		}

		// Token: 0x06001832 RID: 6194 RVA: 0x0006EEB8 File Offset: 0x0006D2B8
		private static IEnumerator WaitForCachingSystem()
		{
			while (!Caching.ready)
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x040047A9 RID: 18345
		private readonly List<IAssetBundleResolver> _resolvers = new List<IAssetBundleResolver>();

		// Token: 0x040047AA RID: 18346
		private readonly List<string> _manifestNames = new List<string>();

		// Token: 0x040047AB RID: 18347
		private List<string> _activeVariants;

		// Token: 0x040047AC RID: 18348
		private IManifestResolver _manifestResolver;

		// Token: 0x040047AD RID: 18349
		private string _sbsAccount;

		// Token: 0x040047AE RID: 18350
		private string _sbsEnvironment;
	}
}
