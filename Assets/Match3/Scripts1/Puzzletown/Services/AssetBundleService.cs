using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Match3.Scripts1.Shared.Build;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.Services.AssetBundleManager;
using Match3.Scripts1.Wooga.Services.ErrorAnalytics;
using Wooga.UnityFramework;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x02000747 RID: 1863
	public class AssetBundleService : AService
	{
		// Token: 0x06002E18 RID: 11800 RVA: 0x000D711F File Offset: 0x000D551F
		public AssetBundleService()
		{
			if (Application.isPlaying)
			{
				WooroutineRunner.StartCoroutine(this.InitAssetManagerRoutine(), null);
			}
		}

		// Token: 0x06002E19 RID: 11801 RVA: 0x000D713E File Offset: 0x000D553E
		public void PrepareToRetryFailedDownloads()
		{
			this.manager.PrepareToRetryFailedDownloads();
		}

		// Token: 0x06002E1A RID: 11802 RVA: 0x000D714C File Offset: 0x000D554C
		public T DefaultClassExceptionHandler<T>(Exception exception)
		{
			WoogaDebug.LogWarning(new object[]
			{
				exception
			});
			return default(T);
		}

		// Token: 0x06002E1B RID: 11803 RVA: 0x000D7171 File Offset: 0x000D5571
		public bool DefaultBoolExceptionHandler(Exception exception)
		{
			return false;
		}

		// Token: 0x06002E1C RID: 11804 RVA: 0x000D7174 File Offset: 0x000D5574
		public string[] DefaultStringArrayExceptionHandler(Exception exception)
		{
			WoogaDebug.LogWarning(new object[]
			{
				exception
			});
			return null;
		}

		// Token: 0x06002E1D RID: 11805 RVA: 0x000D7186 File Offset: 0x000D5586
		public Wooroutine<T> LoadAsset<T>(string bundleName, string assetName) where T : global::UnityEngine.Object
		{
			return WooroutineRunner.StartWooroutine<T>(this.LoadRoutine<T>(bundleName, assetName).Catch((Exception e) => this.DefaultClassExceptionHandler<T>(e)).StartAndAwait<T>());
		}

		// Token: 0x06002E1E RID: 11806 RVA: 0x000D71AB File Offset: 0x000D55AB
		public Wooroutine<List<T>> LoadAssets<T>(IEnumerable<KeyValuePair<string, string>> assetDescriptors) where T : global::UnityEngine.Object
		{
			return WooroutineRunner.StartWooroutine<List<T>>(
				assetDescriptors.StartAllAndAwait(d =>
						LoadRoutine<T>(d.Key, d.Value)
							.Catch(e => this.DefaultClassExceptionHandler<T>(e))
						)
				);
		}

		// Token: 0x06002E1F RID: 11807 RVA: 0x000D71C4 File Offset: 0x000D55C4
		public Wooroutine<List<T>> LoadAssets<T>(string bundleName, List<string> assetNames) where T : global::UnityEngine.Object
		{
			return WooroutineRunner.StartWooroutine<List<T>>(assetNames.StartAllAndAwait((string assetName) => this.LoadRoutine<T>(bundleName, assetName).Catch((Exception e) => this.DefaultClassExceptionHandler<T>(e))));
		}

		// Token: 0x06002E20 RID: 11808 RVA: 0x000D71FC File Offset: 0x000D55FC
		public Wooroutine<bool> PreLoadBundle(string bundleName, DownloadMonitor dlMonitor = null)
		{
			return WooroutineRunner.StartWooroutine<bool>(this.BundlePreLoadRoutine(bundleName, dlMonitor).Catch((Exception e) => this.DefaultBoolExceptionHandler(e)).StartAndAwait<bool>());
		}

		// Token: 0x06002E21 RID: 11809 RVA: 0x000D7221 File Offset: 0x000D5621
		public Wooroutine<AssetBundle> LoadAssetBundle(string bundleName)
		{
			return WooroutineRunner.StartWooroutine<AssetBundle>(this.manager.LoadAssetBundle(bundleName, null).Catch((Exception e) => this.DefaultClassExceptionHandler<AssetBundle>(e)).StartAndAwait<AssetBundle>());
		}

		// Token: 0x06002E22 RID: 11810 RVA: 0x000D724B File Offset: 0x000D564B
		public Wooroutine<BundledScene> LoadScene(string bundleName, string sceneName, LoadOptions options)
		{
			return WooroutineRunner.StartWooroutine<BundledScene>(this.manager.LoadScene(bundleName, sceneName, options.loadAdditively, null).StartAndAwait<BundledScene>());
		}

		// Token: 0x06002E23 RID: 11811 RVA: 0x000D726B File Offset: 0x000D566B
		public Wooroutine<bool> IsBundleCached(string bundleName)
		{
			return WooroutineRunner.StartWooroutine<bool>(this.manager.IsBundleCached(bundleName).Catch((Exception e) => this.DefaultBoolExceptionHandler(e)).StartAndAwait<bool>());
		}

		// Token: 0x06002E24 RID: 11812 RVA: 0x000D7294 File Offset: 0x000D5694
		public Wooroutine<string[]> GetAssetNames(string bundle)
		{
			return WooroutineRunner.StartWooroutine<string[]>(this.manager.GetAssetBundleNames(bundle).Catch((Exception e) => this.DefaultStringArrayExceptionHandler(e)).StartAndAwait<string[]>());
		}

		// Token: 0x06002E25 RID: 11813 RVA: 0x000D72BD File Offset: 0x000D56BD
		public Wooroutine<bool> AreAllBundlesAvailable(IEnumerable<string> bundles)
		{
			return WooroutineRunner.StartWooroutine<bool>(this.AreAllBundlesAvailableRoutine(bundles));
		}

		// Token: 0x06002E26 RID: 11814 RVA: 0x000D72CC File Offset: 0x000D56CC
		private IEnumerator AreAllBundlesAvailableRoutine(IEnumerable<string> bundles)
		{
			bool allBundlesAreAvailable = true;
			if (bundles != null)
			{
				foreach (string bundle in bundles)
				{
					if (!allBundlesAreAvailable)
					{
						break;
					}
					Wooroutine<bool> isAvailableRoutine = this.IsBundleAvailable(bundle);
					yield return isAvailableRoutine;
					allBundlesAreAvailable &= isAvailableRoutine.ReturnValue;
				}
			}
			yield return allBundlesAreAvailable;
			yield break;
		}

		// Token: 0x06002E27 RID: 11815 RVA: 0x000D72EE File Offset: 0x000D56EE
		public Wooroutine<bool> IsBundleAvailable(string bundleName)
		{
			return this.manager.IsBundleAvailable(bundleName).Catch((Exception e) => this.DefaultBoolExceptionHandler(e)).StartWooroutine<bool>();
		}

		// Token: 0x06002E28 RID: 11816 RVA: 0x000D7314 File Offset: 0x000D5714
		private IEnumerator MockIslandBundleAvailabilityRoutine()
		{
			bool isAvailable = global::UnityEngine.Input.GetKey(KeyCode.RightShift);
			yield return isAvailable;
			yield break;
		}

		// Token: 0x06002E29 RID: 11817 RVA: 0x000D7328 File Offset: 0x000D5728
		private IEnumerator InitAssetManagerRoutine()
		{
			yield return ServiceLocator.Instance.Inject(this);
			// string res = DeviceInfo.Load().resolution.ToString();
			// BundleManagerCDNConfig cdnConfig = BundleManagerCDNConfig.TryGet().Value;
			BundleManagerCDNConfig cdnConfig = new BundleManagerCDNConfig();
			// string originBaseUrl = URLHelper.GetOriginBaseUrl(cdnConfig.SBSAccount, cdnConfig.Environment) + "/" + res + "/";
			// string cdnBaseUrl = URLHelper.GetCdnBaseUrl(cdnConfig.SBSAccount, cdnConfig.Environment) + "/" + res + "/";
			string originBaseUrl = "xxx";
			string cdnBaseUrl = "xxxyy";
			CDNAssetsResolver[] resolvers = (from m in this.Manifests.all
			select new CDNAssetsResolver(originBaseUrl, new string[]
			{
				cdnBaseUrl
			}, m.manifest, null)).ToArray<CDNAssetsResolver>();
			yield return new AssetBundleManagerBuilder().WithResolvers(resolvers).WithBundleManagerConfig(cdnConfig).Build().ContinueWith(delegate(IAssetBundleManager manager)
			{
				this.manager = manager;
				return manager;
			}).OnComplete(delegate(IAssetBundleManager manager)
			{
				ErrorAnalytics.RegisterObjectToSerialize("bundleInfo", new AssetBundleServiceInfo(manager));
				this.OnInitialized.Dispatch();
			});
			yield break;
		}

		// Token: 0x17000721 RID: 1825
		// (get) Token: 0x06002E2A RID: 11818 RVA: 0x000D7344 File Offset: 0x000D5744
		private BundleManifestList Manifests
		{
			get
			{
				// DeviceInfo deviceInfo = DeviceInfo.Load();
				DeviceInfo deviceInfo = new DeviceInfo();
				deviceInfo.resolution = DeviceInfo.Resolution.hd;
				DeviceInfo.Resolution resolution = deviceInfo.resolution;
				BundleManifestList result;
				try
				{
					result = ((resolution != DeviceInfo.Resolution.hd) ? this.sbs.SbsConfig.assetbundles_sd : this.sbs.SbsConfig.assetbundles_hd);
				}
				catch (Exception ex)
				{
					throw ex;
				}
				return result;
			}
		}

		// Token: 0x06002E2B RID: 11819 RVA: 0x000D73A4 File Offset: 0x000D57A4
		private IEnumerator<bool> BundlePreLoadRoutine(string bundleName, DownloadMonitor dlMonitor = null)
		{
			return this.manager.PreloadAssetBundle(bundleName, dlMonitor);
		}

		// Token: 0x06002E2C RID: 11820 RVA: 0x000D73B3 File Offset: 0x000D57B3
		private IEnumerator<T> LoadRoutine<T>(string bundleName, string assetName) where T : global::UnityEngine.Object
		{
			return this.manager.LoadAssetAsync<T>(bundleName, assetName, null);
		}

		// Token: 0x0400578D RID: 22413
		[WaitForService(true, true)]
		private SBSService sbs;

		// Token: 0x0400578E RID: 22414
		protected IAssetBundleManager manager;
	}
}
