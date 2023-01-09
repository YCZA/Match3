using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Wooga.Core.Utilities;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.Services.AssetBundleManager.Internal;
using UnityEngine;

namespace Match3.Scripts1.Wooga.Services.AssetBundleManager
{
	// Token: 0x020002FF RID: 767
	public class AssetBundleManager : IAssetBundleManager, IDisposable
	{
		// Token: 0x060017EB RID: 6123 RVA: 0x0006DB67 File Offset: 0x0006BF67
		public AssetBundleManager(IAssetBundleResolver bundleResolver)
		{
			this._bundleResolver = bundleResolver;
			this._downloadService = new DownloadService(bundleResolver);
		}

		// Token: 0x060017EC RID: 6124 RVA: 0x0006DB98 File Offset: 0x0006BF98
		public void Dispose()
		{
			this._bundleRefs.Clear();
			AssetBundleCache.Clear();
		}

		// Token: 0x060017ED RID: 6125 RVA: 0x0006DBAA File Offset: 0x0006BFAA
		public List<string> GetLoadedBundleUrls()
		{
			return (from r in this._bundleRefs
			select this._bundleResolver.GetUrlForAsset(r.Value.BundleInfo, true)).ToList<string>();
		}

		// Token: 0x060017EE RID: 6126 RVA: 0x0006DBC8 File Offset: 0x0006BFC8
		public void UnloadAssetBundle(string bundleName, bool unloadAssets)
		{
			IAssetBundleRef bundleRef;
			if (this._bundleRefs.TryGetValue(bundleName, out bundleRef))
			{
				this.UnloadBundleRef(bundleRef, unloadAssets);
			}
		}

		// Token: 0x060017EF RID: 6127 RVA: 0x0006DBF0 File Offset: 0x0006BFF0
		public IEnumerator<AssetBundle> LoadAssetBundle(string bundleName, DownloadMonitor downloadMonitor = null)
		{
			return this.BundleRefForBundle(bundleName).ContinueWith((IAssetBundleRef bundleRef) => this.GetAssetBundle(bundleRef, downloadMonitor)).ContinueWith((IAssetBundleRef bundleRef) => bundleRef.AssetBundle);
		}

		// Token: 0x060017F0 RID: 6128 RVA: 0x0006DC4C File Offset: 0x0006C04C
		public IEnumerator<T> LoadAsset<T>(string bundleName, string assetPath, DownloadMonitor downloadMonitor = null) where T : global::UnityEngine.Object
		{
			return this.BundleRefForBundle(bundleName).ContinueWith((IAssetBundleRef bundleRef) => (!this.IsFullyLoaded(bundleRef)) ? this.GetAssetBundle(bundleRef, downloadMonitor).ContinueWith(() => AssetBundleLoader.LoadAssetAsync<T>(bundleRef.AssetBundle, assetPath)) : AssetBundleLoader.LoadAssetSync<T>(bundleRef.AssetBundle, assetPath).Yield<T>());
		}

		// Token: 0x060017F1 RID: 6129 RVA: 0x0006DC8C File Offset: 0x0006C08C
		public IEnumerator<global::UnityEngine.Object> LoadAsset(Type assetType, string bundleName, string assetPath, DownloadMonitor downloadMonitor = null)
		{
			return this.BundleRefForBundle(bundleName).ContinueWith((IAssetBundleRef bundleRef) => (!this.IsFullyLoaded(bundleRef)) ? this.GetAssetBundle(bundleRef, downloadMonitor).ContinueWith(() => AssetBundleLoader.LoadAssetAsync(assetType, bundleRef.AssetBundle, assetPath)) : AssetBundleLoader.LoadAssetSync(assetType, bundleRef.AssetBundle, assetPath).Yield<global::UnityEngine.Object>());
		}

		// Token: 0x060017F2 RID: 6130 RVA: 0x0006DCD4 File Offset: 0x0006C0D4
		public IEnumerator<T> LoadAssetAsync<T>(string bundleName, string assetPath, DownloadMonitor downloadMonitor = null) where T : global::UnityEngine.Object
		{
			return AssetBundleLoader.LoadAssetAsync<T>(null, assetPath);
			
			// Debug.LogWarning("LoadAssetAsync");
			var brb = BundleRefForBundle(bundleName);
			var cw = brb.ContinueWith((IAssetBundleRef bundleRef) => 
					// eli todo 暂时用resource.load
					// new code
					AssetBundleLoader.LoadAssetAsync<T>(bundleRef.AssetBundle, assetPath)
					// old code
					// IsFullyLoaded(bundleRef) ?
					// 	AssetBundleLoader.LoadAssetAsync<T>(bundleRef.AssetBundle, assetPath):
					// 	GetAssetBundle(bundleRef, downloadMonitor).ContinueWith(() => AssetBundleLoader.LoadAssetAsync<T>(bundleRef.AssetBundle, assetPath))
					);
			return cw;
		}

		// Token: 0x060017F3 RID: 6131 RVA: 0x0006DD14 File Offset: 0x0006C114
		public IEnumerator<global::UnityEngine.Object> LoadAssetAsync(Type assetType, string bundleName, string assetPath, DownloadMonitor downloadMonitor = null)
		{
			return this.BundleRefForBundle(bundleName).ContinueWith((IAssetBundleRef bundleRef) => (!this.IsFullyLoaded(bundleRef)) ? this.GetAssetBundle(bundleRef, downloadMonitor).ContinueWith(() => AssetBundleLoader.LoadAssetAsync(assetType, bundleRef.AssetBundle, assetPath)) : AssetBundleLoader.LoadAssetAsync(assetType, bundleRef.AssetBundle, assetPath));
		}

		// Token: 0x060017F4 RID: 6132 RVA: 0x0006DD5C File Offset: 0x0006C15C
		public T LoadAssetSync<T>(string bundleName, string assetPath) where T : global::UnityEngine.Object
		{
			IAssetBundleRef assetBundleRef;
			if (!this._bundleRefs.TryGetValue(bundleName, out assetBundleRef) || !this.IsFullyLoaded(assetBundleRef))
			{
				throw new AssetBundleNotLoadedException(bundleName, assetPath);
			}
			return AssetBundleLoader.LoadAssetSync<T>(assetBundleRef.AssetBundle, assetPath);
		}

		// Token: 0x060017F5 RID: 6133 RVA: 0x0006DD9C File Offset: 0x0006C19C
		public global::UnityEngine.Object LoadAssetSync(Type assetType, string bundleName, string assetPath)
		{
			IAssetBundleRef assetBundleRef;
			if (!this._bundleRefs.TryGetValue(bundleName, out assetBundleRef) || !this.IsFullyLoaded(assetBundleRef))
			{
				throw new AssetBundleNotLoadedException(bundleName, assetPath);
			}
			return AssetBundleLoader.LoadAssetSync(assetType, assetBundleRef.AssetBundle, assetPath);
		}

		// Token: 0x060017F6 RID: 6134 RVA: 0x0006DDE0 File Offset: 0x0006C1E0
		public IEnumerator<BundledScene> LoadScene(string bundleName, string levelName, bool isAdditive, DownloadMonitor downloadMonitor = null)
		{
			// eli todo 暂时将场景添加到build中
			Debug.Log("LoadScene: levelName:" + levelName + " bundleName:" + bundleName);
			return BundleRefForBundle(bundleName).ContinueWith((IAssetBundleRef bundleRef) =>
				IsFullyLoaded(bundleRef) ? LoadSceneAsync(bundleRef, levelName, isAdditive):
					GetAssetBundle(bundleRef, downloadMonitor).ContinueWith(() => LoadSceneAsync(bundleRef, levelName, isAdditive))
					);
		}

		// Token: 0x060017F7 RID: 6135 RVA: 0x0006DE28 File Offset: 0x0006C228
		public IEnumerator<bool> PreloadAssetBundle(string bundleName, DownloadMonitor downloadMonitor = null)
		{
			return this.BundleRefForBundle(bundleName).ContinueWith((IAssetBundleRef bundleRef) => this.GetAssetBundle(bundleRef, downloadMonitor)).ContinueWith((IAssetBundleRef bundleRef) => this._downloadService.IsAssetBundleCached(bundleRef.BundleInfo));
		}

		// Token: 0x060017F8 RID: 6136 RVA: 0x0006DE72 File Offset: 0x0006C272
		public IEnumerator<bool> IsBundleCached(string bundleName)
		{
			return this._bundleResolver.Resolve(bundleName).ContinueWith((IBundleInfo resolvedBundle) => this._downloadService.IsAssetBundleCached(resolvedBundle));
		}

		// Token: 0x060017F9 RID: 6137 RVA: 0x0006DE91 File Offset: 0x0006C291
		public IEnumerator<string[]> GetAssetBundleNames(string bundleName)
		{
			return this.LoadAssetBundle(bundleName, null).ContinueWith((AssetBundle bundle) => bundle.GetAllAssetNames());
		}

		// Token: 0x060017FA RID: 6138 RVA: 0x0006DEBD File Offset: 0x0006C2BD
		public IEnumerator<bool> IsBundleAvailable(string bundleName)
		{
			return this.ResolveWithAllDependencies(bundleName).ContinueWith((List<IBundleInfo> bundleInfos) => bundleInfos.All((IBundleInfo info) => !this._bundleResolver.GetUrlForAsset(info, true).StartsWith("http") || this._downloadService.IsAssetBundleCached(info)));
		}

		// Token: 0x060017FB RID: 6139 RVA: 0x0006DED8 File Offset: 0x0006C2D8
		public void PrepareToRetryFailedDownloads()
		{
			foreach (IAssetBundleRef assetBundleRef in this._bundleRefs.Values)
			{
				assetBundleRef.PrepareToRetryFailedDownload();
			}
		}

		// Token: 0x060017FC RID: 6140 RVA: 0x0006DF38 File Offset: 0x0006C338
		private bool IsFullyLoaded(IAssetBundleRef bundleRef)
		{
			IList<string> source;
			return bundleRef.IsLoaded && (!this._dependencies.TryGetValue(bundleRef.BundleInfo.Name, out source) || !source.Any((string bundle) => !this.IsFullyLoaded(bundle)));
		}

		// Token: 0x060017FD RID: 6141 RVA: 0x0006DF88 File Offset: 0x0006C388
		private bool IsFullyLoaded(string assetBundleName)
		{
			IAssetBundleRef bundleRef;
			return this.TryGetBundleRef(assetBundleName, out bundleRef) && this.IsFullyLoaded(bundleRef);
		}

		private static IEnumerator<BundledScene> LoadSceneAsync(IAssetBundleRef assetRef, string levelName, bool isAdditive)
		{
			Debug.Log("LoadSceneAsync " + levelName);
			return AssetBundleLoader.LoadSceneAsync(levelName, isAdditive);
		}

		// Token: 0x060017FF RID: 6143 RVA: 0x0006DFB6 File Offset: 0x0006C3B6
		private static BundledScene LoadSceneSync(IAssetBundleRef assetRef, string levelName, bool isAdditive)
		{
			Debug.Log("LoadSceneSync " + levelName);
			return AssetBundleLoader.LoadSceneSync(levelName, isAdditive);
		}

		// Token: 0x06001800 RID: 6144 RVA: 0x0006DFBF File Offset: 0x0006C3BF
		private IEnumerator<IAssetBundleRef> GetAssetBundle(IAssetBundleRef bundleRef, DownloadMonitor downloadMonitor)
		{
			return this.StartDownloadIfMissing(bundleRef, downloadMonitor);
		}

		// Token: 0x06001801 RID: 6145 RVA: 0x0006DFCC File Offset: 0x0006C3CC
		private IEnumerator<IAssetBundleRef> StartDownloadIfMissing(string bundleName, DownloadMonitor downloadMonitor)
		{
			return this.BundleRefForBundle(bundleName).ContinueWith((IAssetBundleRef bundleRef) => this.StartDownloadIfMissing(bundleRef, downloadMonitor));
		}

		// Token: 0x06001802 RID: 6146 RVA: 0x0006E008 File Offset: 0x0006C408
		private IEnumerator<IAssetBundleRef> StartDownloadIfMissing(IAssetBundleRef bundleRef, DownloadMonitor downloadMonitor)
		{
			IBundleInfo bundleInfo = bundleRef.BundleInfo;
			
			if (bundleRef.NeedsLoading)
			{
				bundleRef.MarkLoading();
				return this._downloadService.StartDownload(bundleInfo, downloadMonitor, true).ContinueWith(() => this.LoadDependencies(bundleInfo, downloadMonitor)).ContinueWith(() => bundleRef).StartAndAwait(delegate(Exception e)
				{
					bundleRef.MarkLoadingFailed(e);
					ExceptionUtils.RethrowException(e);
				});
			}
			this._downloadService.TryMonitorProgress(bundleInfo, downloadMonitor);
			return this.WaitForLoadedAsset(bundleRef, downloadMonitor);
		}

		// Token: 0x06001803 RID: 6147 RVA: 0x0006E0D4 File Offset: 0x0006C4D4
		private IEnumerator<List<IAssetBundleRef>> LoadDependencies(IBundleInfo bundle, DownloadMonitor downloadMonitor)
		{
			return this._bundleResolver.GetDependencies(bundle.Name).ContinueWith(delegate(IList<string> dependencies)
			{
				if (dependencies.Count > 0)
				{
					this._dependencies[bundle.Name] = dependencies;
					return dependencies.StartAllAndAwait((string dependency) => this.StartDownloadIfMissing(dependency, downloadMonitor));
				}
				return new List<IAssetBundleRef>().Yield<List<IAssetBundleRef>>();
			});
		}

		// Token: 0x06001804 RID: 6148 RVA: 0x0006E123 File Offset: 0x0006C523
		private IEnumerator<List<IBundleInfo>> ResolveWithAllDependencies(string bundleName)
		{
			return this.DoResolveWithAllDependencies(bundleName).Cast<List<IBundleInfo>>();
		}

		// Token: 0x06001805 RID: 6149 RVA: 0x0006E134 File Offset: 0x0006C534
		private IEnumerator DoResolveWithAllDependencies(string bundleName)
		{
			List<string> toResolve = new List<string>
			{
				bundleName
			};
			List<string> dependencies = new List<string>();
			List<IBundleInfo> infos = new List<IBundleInfo>();
			while (toResolve.Count > 0)
			{
				string name = toResolve[toResolve.Count - 1];
				toResolve.RemoveAt(toResolve.Count - 1);
				// string dependency;
				yield return this._bundleResolver.GetDependencies(name).ContinueWith(delegate(IList<string> deps)
				{
					dependencies.Add(name);
					toResolve.AddRange(from dependency in deps
					where !dependencies.Contains(dependency)
					select dependency);
				});
			}
			foreach (string dependency in dependencies)
			{
				// string dependency;
				yield return this._bundleResolver.Resolve(dependency).ContinueWith(delegate(IBundleInfo info)
				{
					infos.Add(info);
				});
			}
			yield return infos;
			yield break;
		}

		// Token: 0x06001806 RID: 6150 RVA: 0x0006E158 File Offset: 0x0006C558
		private IEnumerator<IAssetBundleRef> WaitForLoadedAsset(IAssetBundleRef assetBundleRef, DownloadMonitor downloadMonitor)
		{
			while (!this.IsFullyLoaded(assetBundleRef))
			{
				if (assetBundleRef.HasLoadingFailed)
				{
					throw assetBundleRef.FailedReason ?? new Exception("Could not load asset bundle: " + assetBundleRef.BundleInfo);
				}
				yield return null;
			}
			if (downloadMonitor.HasProgressMonitor())
			{
				downloadMonitor.OnProgress(assetBundleRef.BundleInfo.Name, 1f);
			}
			yield return assetBundleRef;
			yield break;
		}

		// Token: 0x06001807 RID: 6151 RVA: 0x0006E181 File Offset: 0x0006C581
		private IAssetBundleRef CreateBundleRef(IBundleInfo bundle)
		{
			return new AssetBundleManager.AssetBundleRef(this, bundle);
		}

		// Token: 0x06001808 RID: 6152 RVA: 0x0006E18A File Offset: 0x0006C58A
		private IEnumerator<IAssetBundleRef> BundleRefForBundle(string bundleName)
		{
			return this._bundleResolver.Resolve(bundleName).ContinueWith((IBundleInfo resolvedBundle) => this.BundleRefForBundle(resolvedBundle));
		}

		// Token: 0x06001809 RID: 6153 RVA: 0x0006E1AC File Offset: 0x0006C5AC
		private IAssetBundleRef BundleRefForBundle(IBundleInfo bundle)
		{
			IAssetBundleRef assetBundleRef = this.GetBundleRef(bundle);
			if (assetBundleRef == null)
			{
				assetBundleRef = this.CreateBundleRef(bundle);
				this.AddBundleRef(bundle.Name, assetBundleRef);
			}
			return assetBundleRef;
		}

		// Token: 0x0600180A RID: 6154 RVA: 0x0006E1DD File Offset: 0x0006C5DD
		private bool ContainsBundleRef(IBundleInfo bundle)
		{
			return this._bundleRefs.ContainsKey(bundle.Name);
		}

		// Token: 0x0600180B RID: 6155 RVA: 0x0006E1F0 File Offset: 0x0006C5F0
		private IAssetBundleRef GetBundleRef(IBundleInfo bundle)
		{
			return this._bundleRefs.Get(bundle.Name, null);
		}

		// Token: 0x0600180C RID: 6156 RVA: 0x0006E204 File Offset: 0x0006C604
		private bool TryGetBundleRef(string bundleName, out IAssetBundleRef bundleRef)
		{
			return this._bundleRefs.TryGetValue(bundleName, out bundleRef);
		}

		// Token: 0x0600180D RID: 6157 RVA: 0x0006E213 File Offset: 0x0006C613
		private void AddBundleRef(string bundleName, IAssetBundleRef bundleRef)
		{
			this._bundleRefs[bundleName] = bundleRef;
		}

		// Token: 0x0600180E RID: 6158 RVA: 0x0006E222 File Offset: 0x0006C622
		private void UnloadBundleRef(IAssetBundleRef bundleRef, bool unloadAssets = false)
		{
			this._bundleRefs.Remove(bundleRef.BundleInfo.Name);
			AssetBundleCache.GetInstance().Unload(bundleRef.BundleInfo.Name, unloadAssets);
		}

		// Token: 0x0400479D RID: 18333
		private readonly Dictionary<string, IList<string>> _dependencies = new Dictionary<string, IList<string>>();

		// Token: 0x0400479E RID: 18334
		private readonly Dictionary<string, IAssetBundleRef> _bundleRefs = new Dictionary<string, IAssetBundleRef>();

		// Token: 0x0400479F RID: 18335
		private readonly DownloadService _downloadService;

		// Token: 0x040047A0 RID: 18336
		private readonly IAssetBundleResolver _bundleResolver;

		// Token: 0x02000300 RID: 768
		public class AssetBundleRef : IAssetBundleRef, IDisposable
		{
			// Token: 0x06001817 RID: 6167 RVA: 0x0006E2DF File Offset: 0x0006C6DF
			public AssetBundleRef(AssetBundleManager manager, IBundleInfo bundleInfo)
			{
				this._manager = manager;
				this.BundleInfo = bundleInfo;
			}

			// Token: 0x170003BB RID: 955
			// (get) Token: 0x06001818 RID: 6168 RVA: 0x0006E2F5 File Offset: 0x0006C6F5
			public AssetBundle AssetBundle
			{
				get
				{
					return AssetBundleCache.GetInstance().GetBundle(this.BundleInfo.Name);
				}
			}

			// Token: 0x170003BC RID: 956
			// (get) Token: 0x06001819 RID: 6169 RVA: 0x0006E30C File Offset: 0x0006C70C
			public bool IsLoaded
			{
				get
				{
					return this.AssetBundle != null;
				}
			}

			// Token: 0x170003BD RID: 957
			// (get) Token: 0x0600181A RID: 6170 RVA: 0x0006E31A File Offset: 0x0006C71A
			public bool IsLoading
			{
				get
				{
					return this._loadingWasStarted && !this._loadingFailed;
				}
			}

			// Token: 0x170003BE RID: 958
			// (get) Token: 0x0600181B RID: 6171 RVA: 0x0006E333 File Offset: 0x0006C733
			public bool HasLoadingFailed
			{
				get
				{
					return this._loadingFailed;
				}
			}

			// Token: 0x170003BF RID: 959
			// (get) Token: 0x0600181C RID: 6172 RVA: 0x0006E33B File Offset: 0x0006C73B
			public bool NeedsLoading
			{
				get
				{
					return !this.IsLoaded && !this._loadingWasStarted;
				}
			}

			// Token: 0x170003C0 RID: 960
			// (get) Token: 0x0600181D RID: 6173 RVA: 0x0006E354 File Offset: 0x0006C754
			// (set) Token: 0x0600181E RID: 6174 RVA: 0x0006E35C File Offset: 0x0006C75C
			public IBundleInfo BundleInfo { get; private set; }

			// Token: 0x170003C1 RID: 961
			// (get) Token: 0x0600181F RID: 6175 RVA: 0x0006E365 File Offset: 0x0006C765
			// (set) Token: 0x06001820 RID: 6176 RVA: 0x0006E36D File Offset: 0x0006C76D
			public string BundleURI { get; set; }

			// Token: 0x170003C2 RID: 962
			// (get) Token: 0x06001821 RID: 6177 RVA: 0x0006E376 File Offset: 0x0006C776
			public Exception FailedReason
			{
				get
				{
					return this._exception;
				}
			}

			// Token: 0x06001822 RID: 6178 RVA: 0x0006E37E File Offset: 0x0006C77E
			public void MarkLoading()
			{
				this._loadingWasStarted = true;
			}

			// Token: 0x06001823 RID: 6179 RVA: 0x0006E387 File Offset: 0x0006C787
			public void MarkLoadingFailed(Exception e = null)
			{
				this._loadingFailed = true;
				this._exception = e;
			}

			// Token: 0x06001824 RID: 6180 RVA: 0x0006E397 File Offset: 0x0006C797
			public void PrepareToRetryFailedDownload()
			{
				if (!this.IsLoaded && !this.IsLoading)
				{
					this._loadingWasStarted = false;
					this._loadingFailed = false;
					this._exception = null;
				}
			}

			// Token: 0x06001825 RID: 6181 RVA: 0x0006E3C4 File Offset: 0x0006C7C4
			public void Dispose()
			{
				this._manager.UnloadBundleRef(this, false);
			}

			// Token: 0x06001826 RID: 6182 RVA: 0x0006E3D3 File Offset: 0x0006C7D3
			public override string ToString()
			{
				return string.Format("[AssetBundleRef:{0} AssetBundle={1}, Loaded={2}]", RuntimeHelpers.GetHashCode(this), this.BundleInfo.Name, this.IsLoaded);
			}

			// Token: 0x040047A3 RID: 18339
			private AssetBundleManager _manager;

			// Token: 0x040047A4 RID: 18340
			private bool _loadingFailed;

			// Token: 0x040047A5 RID: 18341
			private bool _loadingWasStarted;

			// Token: 0x040047A6 RID: 18342
			private Exception _exception;
		}
	}
}
