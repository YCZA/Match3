using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Wooga.Coroutines;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x020008F1 RID: 2289
	public class BuildingResourceCache : MonoBehaviour
	{
		// Token: 0x17000891 RID: 2193
		// (get) Token: 0x060037A7 RID: 14247 RVA: 0x0010F604 File Offset: 0x0010DA04
		public static BuildingResourceCache Instance
		{
			get
			{
				if (BuildingResourceCache.instance != null)
				{
					return BuildingResourceCache.instance;
				}
				BuildingResourceCache.instance = new GameObject("BuildingResourceCache").AddComponent<BuildingResourceCache>();
				global::UnityEngine.Object.DontDestroyOnLoad(BuildingResourceCache.instance);
				BuildingResourceCache.instance.hideFlags = HideFlags.DontSaveInEditor;
				BuildingResourceCache.EnsureCleanupAfterGameStoppedInEditor();
				return BuildingResourceCache.instance;
			}
		}

		// Token: 0x060037A8 RID: 14248 RVA: 0x0010F65A File Offset: 0x0010DA5A
		private static void EnsureCleanupAfterGameStoppedInEditor()
		{
		}

		// Token: 0x17000892 RID: 2194
		// (get) Token: 0x060037A9 RID: 14249 RVA: 0x0010F65C File Offset: 0x0010DA5C
		// (set) Token: 0x060037AA RID: 14250 RVA: 0x0010F664 File Offset: 0x0010DA64
		public BuildingToBundleMap BuildingToBundleMap { get; protected set; }

		// Token: 0x17000893 RID: 2195
		// (get) Token: 0x060037AB RID: 14251 RVA: 0x0010F66D File Offset: 0x0010DA6D
		public Dictionary<string, Sprite> SpriteCache
		{
			get
			{
				return this.spriteCache;
			}
		}

		// Token: 0x17000894 RID: 2196
		// (get) Token: 0x060037AC RID: 14252 RVA: 0x0010F675 File Offset: 0x0010DA75
		public Dictionary<string, GameObject> BuildingCache
		{
			get
			{
				return this.buildingCache;
			}
		}

		// Token: 0x17000895 RID: 2197
		// (get) Token: 0x060037AD RID: 14253 RVA: 0x0010F67D File Offset: 0x0010DA7D
		public Dictionary<string, GameObject> DestroyedCache
		{
			get
			{
				return this.destroyedCache;
			}
		}

		// Token: 0x060037AE RID: 14254 RVA: 0x0010F685 File Offset: 0x0010DA85
		public static bool IsValid()
		{
			return !(BuildingResourceCache.instance == null) && BuildingResourceCache.Instance.BuildingToBundleMap != null && BuildingResourceCache.Instance.assetBundleService != null;
		}

		// Token: 0x060037AF RID: 14255 RVA: 0x0010F6BB File Offset: 0x0010DABB
		public void Init(SessionService sessions, AssetBundleService assetBundleService, BuildingToBundleMap buildingToBundleMap)
		{
			this.assetBundleService = assetBundleService;
			sessions.onRestart.AddListenerOnce(new Action(this.CleanupAndInvalidate));
			this.BuildingToBundleMap = buildingToBundleMap;
		}

		// Token: 0x060037B0 RID: 14256 RVA: 0x0010F6E2 File Offset: 0x0010DAE2
		public void CleanUpAssetReferences()
		{
			this.spriteCache.Clear();
			this.buildingCache.Clear();
			this.destroyedCache.Clear();
		}

		// Token: 0x060037B1 RID: 14257 RVA: 0x0010F705 File Offset: 0x0010DB05
		public void CleanupAndInvalidate()
		{
			this.BuildingToBundleMap = null;
			this.MarkNothingBeingLoaded();
			this.CleanUpAssetReferences();
		}

		// Token: 0x060037B2 RID: 14258 RVA: 0x0010F71A File Offset: 0x0010DB1A
		public bool IsAssetBeingLoaded(string buildingID, bool asDestroyed, bool asIcon)
		{
			if (asIcon)
			{
				return this.iconsBeingLoaded.Contains(buildingID);
			}
			if (asDestroyed)
			{
				return this.destroyedBeingLoaded.Contains(buildingID);
			}
			return this.prefabsBeingLoaded.Contains(buildingID);
		}

		// Token: 0x060037B3 RID: 14259 RVA: 0x0010F750 File Offset: 0x0010DB50
		public void MarkAssetBeingLoaded(bool isBeingLoaded, string buildingID, bool asDestroyed, bool asIcon)
		{
			if (isBeingLoaded)
			{
				if (asIcon)
				{
					this.iconsBeingLoaded.Add(buildingID);
				}
				else if (asDestroyed)
				{
					this.destroyedBeingLoaded.Add(buildingID);
				}
				else
				{
					this.prefabsBeingLoaded.Add(buildingID);
				}
			}
			else if (asIcon)
			{
				this.iconsBeingLoaded.Remove(buildingID);
			}
			else if (asDestroyed)
			{
				this.destroyedBeingLoaded.Remove(buildingID);
			}
			else
			{
				this.prefabsBeingLoaded.Remove(buildingID);
			}
		}

		// Token: 0x060037B4 RID: 14260 RVA: 0x0010F7E4 File Offset: 0x0010DBE4
		public void MarkNothingBeingLoaded()
		{
			this.iconsBeingLoaded.Clear();
			this.prefabsBeingLoaded.Clear();
			this.destroyedBeingLoaded.Clear();
		}

		// Token: 0x060037B5 RID: 14261 RVA: 0x0010F807 File Offset: 0x0010DC07
		public bool IsCached(string buildingID, bool asDestroyed, bool asIcon)
		{
			return (!asDestroyed) ? ((!asIcon) ? this.buildingCache.ContainsKey(buildingID) : this.spriteCache.ContainsKey(buildingID)) : this.destroyedCache.ContainsKey(buildingID);
		}

		// Token: 0x060037B6 RID: 14262 RVA: 0x0010F844 File Offset: 0x0010DC44
		public bool TryGetPrefab(string buildingID, bool repaired, out GameObject buildingAsset)
		{
			Dictionary<string, GameObject> dictionary = (!repaired) ? this.destroyedCache : this.buildingCache;
			return dictionary.TryGetValue(buildingID, out buildingAsset);
		}

		// Token: 0x060037B7 RID: 14263 RVA: 0x0010F871 File Offset: 0x0010DC71
		public bool TryGetIcon(string buildingID, out Sprite buildingAsset)
		{
			return this.spriteCache.TryGetValue(buildingID, out buildingAsset);
		}

		// Token: 0x060037B8 RID: 14264 RVA: 0x0010F880 File Offset: 0x0010DC80
		public GameObject GetGenericDestroyedPrefab(BuildingConfig building)
		{
			string genericDestroyedPrefabName = building.GetGenericDestroyedPrefabName();
			GameObject result;
			if (!string.IsNullOrEmpty(genericDestroyedPrefabName) && this.buildingCache.TryGetValue(genericDestroyedPrefabName, out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x060037B9 RID: 14265 RVA: 0x0010F8B8 File Offset: 0x0010DCB8
		public IEnumerator LoadBuildingComponentsFromBundleRoutine(List<string> buildingIDs, bool asDestroyed, bool asIcon)
		{
			if (buildingIDs == null || buildingIDs.Count < 1)
			{
				yield break;
			}
			Dictionary<string, List<AssetToPathWrapper>> assetsToLoadByBundleName = this.GetAssetsToLoadByBundleName(buildingIDs, asDestroyed, asIcon);
			foreach (KeyValuePair<string, List<AssetToPathWrapper>> kvPair in assetsToLoadByBundleName)
			{
				string bundleName = kvPair.Key;
				List<AssetToPathWrapper> paths = kvPair.Value;
				if (asIcon)
				{
					yield return this.LoadAndCacheAssetsRoutine<Sprite>(bundleName, paths, this.spriteCache, asDestroyed, asIcon);
				}
				else
				{
					Dictionary<string, GameObject> cache = (!asDestroyed) ? this.buildingCache : this.destroyedCache;
					yield return this.LoadAndCacheAssetsRoutine<GameObject>(bundleName, paths, cache, asDestroyed, asIcon);
				}
			}
			yield break;
		}

		// Token: 0x060037BA RID: 14266 RVA: 0x0010F8E8 File Offset: 0x0010DCE8
		private Dictionary<string, List<AssetToPathWrapper>> GetAssetsToLoadByBundleName(List<string> buildingIDs, bool asDestroyed, bool asIcon)
		{
			Dictionary<string, List<AssetToPathWrapper>> dictionary = new Dictionary<string, List<AssetToPathWrapper>>();
			foreach (string buildingID in buildingIDs)
			{
				if (!this.IsCached(buildingID, asDestroyed, asIcon))
				{
					BuildingToBundleMap.Entry entry = this.BuildingToBundleMap.FindEntry(buildingID, asDestroyed, asIcon);
					if (entry != null)
					{
						this.MarkAssetBeingLoaded(true, buildingID, asDestroyed, asIcon);
						string key = (!asIcon) ? ((!asDestroyed) ? entry.prefabBundle : entry.destroyedBundle) : entry.iconBundle;
						List<AssetToPathWrapper> list;
						if (!dictionary.TryGetValue(key, out list))
						{
							list = new List<AssetToPathWrapper>();
							dictionary[key] = list;
						}
						list.Add(new AssetToPathWrapper(entry.id, entry.GetAssetPath(asDestroyed, asIcon)));
					}
				}
			}
			return dictionary;
		}

		// Token: 0x060037BB RID: 14267 RVA: 0x0010F9D0 File Offset: 0x0010DDD0
		private IEnumerator LoadAndCacheAssetsRoutine<T>(string bundleName, List<AssetToPathWrapper> pathsByAssetID, Dictionary<string, T> cache, bool asDestroyed, bool asIcon) where T : global::UnityEngine.Object
		{
			Wooroutine<List<T>> assetsToLoad = this.assetBundleService.LoadAssets<T>(bundleName, (from assetToPathWrapper in pathsByAssetID
			select assetToPathWrapper.path).ToList<string>());
			yield return assetsToLoad;
			List<T> loadedAssets = null;
			try
			{
				loadedAssets = assetsToLoad.ReturnValue;
			}
			catch (Exception ex)
			{
				WoogaDebug.Log(new object[]
				{
					ex.Message
				});
			}
			if (loadedAssets != null)
			{
				this.AddToCache<T>(cache, pathsByAssetID, loadedAssets);
			}
			foreach (AssetToPathWrapper assetToPathWrapper2 in pathsByAssetID)
			{
				this.MarkAssetBeingLoaded(false, assetToPathWrapper2.id, asDestroyed, asIcon);
			}
			yield break;
		}

		// Token: 0x060037BC RID: 14268 RVA: 0x0010FA10 File Offset: 0x0010DE10
		private void AddToCache<T>(Dictionary<string, T> cache, List<AssetToPathWrapper> pathsByAssetName, List<T> loadedAssets) where T : global::UnityEngine.Object
		{
			for (int i = 0; i < loadedAssets.Count; i++)
			{
				if (loadedAssets[i] == null)
				{
					WoogaDebug.LogWarning(new object[]
					{
						"Bundled asset null: ",
						pathsByAssetName[i].path
					});
				}
				else
				{
					cache[pathsByAssetName[i].id] = loadedAssets[i];
				}
			}
		}

		// Token: 0x04005FE0 RID: 24544
		private static BuildingResourceCache instance;

		// Token: 0x04005FE2 RID: 24546
		private readonly HashSet<string> prefabsBeingLoaded = new HashSet<string>();

		// Token: 0x04005FE3 RID: 24547
		private readonly HashSet<string> destroyedBeingLoaded = new HashSet<string>();

		// Token: 0x04005FE4 RID: 24548
		private readonly HashSet<string> iconsBeingLoaded = new HashSet<string>();

		// Token: 0x04005FE5 RID: 24549
		private readonly Dictionary<string, Sprite> spriteCache = new Dictionary<string, Sprite>();

		// Token: 0x04005FE6 RID: 24550
		private readonly Dictionary<string, GameObject> buildingCache = new Dictionary<string, GameObject>();

		// Token: 0x04005FE7 RID: 24551
		private readonly Dictionary<string, GameObject> destroyedCache = new Dictionary<string, GameObject>();

		// Token: 0x04005FE8 RID: 24552
		private AssetBundleService assetBundleService;
	}
}
