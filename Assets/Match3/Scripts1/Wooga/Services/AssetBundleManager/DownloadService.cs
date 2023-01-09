using System;
using System.Collections.Generic;
using Wooga.Coroutines;
using UnityEngine;

namespace Match3.Scripts1.Wooga.Services.AssetBundleManager
{
	// Token: 0x02000307 RID: 775
	public class DownloadService
	{
		// Token: 0x0600184C RID: 6220 RVA: 0x0006F2D4 File Offset: 0x0006D6D4
		public DownloadService(IAssetBundleResolver bundleResolver)
		{
			this._progressMonitors = new Dictionary<string, List<Action<float>>>();
			this._bundleResolver = bundleResolver;
		}

		// Token: 0x0600184D RID: 6221 RVA: 0x0006F2F0 File Offset: 0x0006D6F0
		public bool TryMonitorProgress(IBundleInfo bundle, DownloadMonitor downloadMonitor)
		{
			return downloadMonitor.HasProgressMonitor() && this.TryMonitorProgress(bundle, delegate(float progress)
			{
				downloadMonitor.OnProgress(bundle.Name, progress);
			});
		}

		// Token: 0x0600184E RID: 6222 RVA: 0x0006F33C File Offset: 0x0006D73C
		public bool TryMonitorProgress(IBundleInfo bundleName, Action<float> onProgress)
		{
			List<Action<float>> list;
			if (this._progressMonitors.TryGetValue(bundleName.Name, out list))
			{
				list.Add(onProgress);
				return true;
			}
			return false;
		}

		// Token: 0x0600184F RID: 6223 RVA: 0x0006F36C File Offset: 0x0006D76C
		public IEnumerator<AssetBundle> StartDownload(IBundleInfo bundle, DownloadMonitor downloadMonitor, bool cached)
		{
			List<Action<float>> list = new List<Action<float>>();
			this._progressMonitors[bundle.Name] = list;
			if (downloadMonitor.HasProgressMonitor())
			{
				list.Add(delegate(float progress)
				{
					downloadMonitor.OnProgress(bundle.Name, progress);
				});
			}
			string[] urls = this._bundleResolver.GetUrlsForAsset(bundle, cached);
			if (cached)
			{
				IEnumerator<AssetBundle> enumerator = DownloadHelper.DownloadAssetBundleCached(bundle.Name, urls[0], bundle.Hash128, bundle.CRC, this.ProgressReporter(bundle.Name));
				for (int i = 1; i < urls.Length; i++)
				{
					int index = i;
					enumerator = enumerator.Catch((Exception exception) => DownloadHelper.DownloadAssetBundleCached(bundle.Name, urls[index], bundle.Hash128, bundle.CRC, this.ProgressReporter(bundle.Name)));
				}
				return enumerator;
			}
			IEnumerator<AssetBundle> enumerator2 = DownloadHelper.DownloadAssetBundleUncached(bundle.Name, urls[0], 0U, this.ProgressReporter(bundle.Name));
			for (int j = 1; j < urls.Length; j++)
			{
				int index = j;
				enumerator2 = enumerator2.Catch((Exception exception) => DownloadHelper.DownloadAssetBundleUncached(bundle.Name, urls[index], 0U, this.ProgressReporter(bundle.Name)));
			}
			return enumerator2;
		}

		// Token: 0x06001850 RID: 6224 RVA: 0x0006F4F0 File Offset: 0x0006D8F0
		public IEnumerator<bool> IsAssetBundleCached(string bundleName)
		{
			return this._bundleResolver.Resolve(bundleName).ContinueWith((IBundleInfo bundle) => this.IsAssetBundleCached(bundle));
		}

		// Token: 0x06001851 RID: 6225 RVA: 0x0006F510 File Offset: 0x0006D910
		public bool IsAssetBundleCached(IBundleInfo bundle)
		{
			string urlForAsset = this._bundleResolver.GetUrlForAsset(bundle, true);
			return Caching.IsVersionCached(urlForAsset, bundle.Hash128);
		}

		// Token: 0x06001852 RID: 6226 RVA: 0x0006F538 File Offset: 0x0006D938
		private Action<float> ProgressReporter(string bundleName)
		{
			return delegate(float progress)
			{
				List<Action<float>> list;
				if (this._progressMonitors.TryGetValue(bundleName, out list))
				{
					foreach (Action<float> action in list)
					{
						action(progress);
					}
				}
			};
		}

		// Token: 0x040047BA RID: 18362
		private readonly IAssetBundleResolver _bundleResolver;

		// Token: 0x040047BB RID: 18363
		private readonly Dictionary<string, List<Action<float>>> _progressMonitors;
	}
}
