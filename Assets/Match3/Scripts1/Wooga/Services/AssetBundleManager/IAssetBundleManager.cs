using System;
using System.Collections.Generic;
using UnityEngine;

namespace Match3.Scripts1.Wooga.Services.AssetBundleManager
{
	// Token: 0x02000309 RID: 777
	public interface IAssetBundleManager : IDisposable
	{
		// Token: 0x06001855 RID: 6229
		List<string> GetLoadedBundleUrls();

		// Token: 0x06001856 RID: 6230
		void UnloadAssetBundle(string bundleName, bool unloadAssets);

		// Token: 0x06001857 RID: 6231
		IEnumerator<AssetBundle> LoadAssetBundle(string bundleName, DownloadMonitor downloadMonitor = null);

		// Token: 0x06001858 RID: 6232
		IEnumerator<T> LoadAsset<T>(string bundleName, string assetPath, DownloadMonitor downloadMonitor = null) where T : global::UnityEngine.Object;

		// Token: 0x06001859 RID: 6233
		IEnumerator<T> LoadAssetAsync<T>(string bundleName, string assetPath, DownloadMonitor downloadMonitor = null) where T : global::UnityEngine.Object;

		// Token: 0x0600185A RID: 6234
		T LoadAssetSync<T>(string bundleName, string assetPath) where T : global::UnityEngine.Object;

		// Token: 0x0600185B RID: 6235
		IEnumerator<global::UnityEngine.Object> LoadAsset(Type assetType, string bundleName, string assetPath, DownloadMonitor downloadMonitor = null);

		// Token: 0x0600185C RID: 6236
		IEnumerator<global::UnityEngine.Object> LoadAssetAsync(Type assetType, string bundleName, string assetPath, DownloadMonitor downloadMonitor = null);

		// Token: 0x0600185D RID: 6237
		global::UnityEngine.Object LoadAssetSync(Type assetType, string bundleName, string assetPath);

		// Token: 0x0600185E RID: 6238
		IEnumerator<BundledScene> LoadScene(string bundleName, string levelName, bool isAdditive, DownloadMonitor downloadMonitor = null);

		// Token: 0x0600185F RID: 6239
		IEnumerator<bool> PreloadAssetBundle(string bundleName, DownloadMonitor downloadMonitor = null);

		// Token: 0x06001860 RID: 6240
		IEnumerator<bool> IsBundleCached(string bundleName);

		// Token: 0x06001861 RID: 6241
		IEnumerator<string[]> GetAssetBundleNames(string bundleName);

		// Token: 0x06001862 RID: 6242
		IEnumerator<bool> IsBundleAvailable(string bundleName);

		// Token: 0x06001863 RID: 6243
		void PrepareToRetryFailedDownloads();
	}
}
