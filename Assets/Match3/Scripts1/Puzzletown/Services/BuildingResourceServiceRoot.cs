using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Match3.Scripts1.Puzzletown.Config;
using Match3.Scripts1.Town;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.Services.ErrorAnalytics;
using Match3.Scripts1.Wooga.Signals;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x020008F2 RID: 2290
	public class BuildingResourceServiceRoot : APtSceneRoot
	{
		// Token: 0x17000896 RID: 2198
		// (get) Token: 0x060037BE RID: 14270 RVA: 0x0010FEF0 File Offset: 0x0010E2F0
		// (set) Token: 0x060037BF RID: 14271 RVA: 0x0010FF14 File Offset: 0x0010E314
		private BuildingResourceCache Cache
		{
			get
			{
				if (this.cache == null)
				{
					this.cache = BuildingResourceCache.Instance;
				}
				return this.cache;
			}
			set
			{
				this.cache = value;
			}
		}

		// Token: 0x060037C0 RID: 14272 RVA: 0x0010FF1D File Offset: 0x0010E31D
		protected override void OnDestroy()
		{
			if (this.cache != null)
			{
				this.cache.CleanUpAssetReferences();
				this.cache.MarkNothingBeingLoaded();
			}
			base.OnDestroy();
		}

		// Token: 0x060037C1 RID: 14273 RVA: 0x0010FF4C File Offset: 0x0010E34C
		protected override IEnumerator GoRoutine()
		{
			yield return this.LoadBuildingToBundleMapRoutine();
			yield return this.CacheAvailableBundleNamesRoutine();
			yield return this.LoadGenericPrefabsRoutine();
			this.onReady.Dispatch();
			base.StartCoroutine(this.LoadAllSpritesRoutine());
			BuildingResourceServiceRoot.startGreedyLoadingAssets = (PlayerPrefs.GetInt(BuildingResourceServiceRoot.START_PRELOAD_PP_KEY, 1) == 1);
			if (BuildingResourceServiceRoot.startGreedyLoadingAssets && this.gameState.IsMyOwnState)
			{
				base.StartCoroutine(this.LoadRequiredPrefabsRoutine());
			}
			this.preloadService.TryStartPreloadNewAreaBundles(BuildingResourceServiceRoot.buildingToBundleMap);
			yield break;
		}

		// Token: 0x060037C2 RID: 14274 RVA: 0x0010FF68 File Offset: 0x0010E368
		public bool DoesAnyBundleNeedDownload(bool includeStartSetBuildings)
		{
			List<BuildingInstance.PersistentData> buildings = this.gameState.Buildings.BuildingsData.Buildings;
			IEnumerable<StartBuilding> allOnIsland = this.configService.startBuilding.GetAllOnIsland(this.gameState.Buildings.CurrentIsland);
			Dictionary<string, BuildingConfig> map = this.configService.buildingConfigList.Map;
			foreach (BuildingInstance.PersistentData persistentData in buildings)
			{
				BuildingConfig buildingConfig;
				if (map.TryGetValue(persistentData.blueprintName, out buildingConfig) && this.NeedsToDownloadBundleFor(buildingConfig.Asset, !persistentData.IsRepaired, false))
				{
					return true;
				}
			}
			if (includeStartSetBuildings)
			{
				foreach (StartBuilding startBuilding in allOnIsland)
				{
					if (this.NeedsToDownloadBundleFor(startBuilding.building_id, startBuilding.destroyed, false))
					{
						return true;
					}
				}
			}
			foreach (BuildingConfig buildingConfig2 in this.configService.buildingConfigList.buildings)
			{
				if (buildingConfig2.island_id == this.gameState.Buildings.CurrentIsland && !buildingConfig2.IsSeasonal() && !buildingConfig2.IsTrophy() && !buildingConfig2.IsAlphabet() && !buildingConfig2.IsFriendDeco() && this.NeedsToDownloadBundleFor(buildingConfig2.Asset, false, true))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060037C3 RID: 14275 RVA: 0x0011013C File Offset: 0x0010E53C
		protected IEnumerator CacheAvailableBundleNamesRoutine()
		{
			this.bundlesAvailable.Clear();
			string[] bundlesToCheck = new string[]
			{
				string.Empty,
				string.Empty,
				string.Empty
			};
			HashSet<string> bundlesChecked = new HashSet<string>();
			foreach (BuildingToBundleMap.Entry entry in BuildingResourceServiceRoot.buildingToBundleMap.entries)
			{
				bundlesToCheck[0] = entry.prefabBundle;
				bundlesToCheck[1] = entry.destroyedBundle;
				bundlesToCheck[2] = entry.iconBundle;
				foreach (string bundleName in bundlesToCheck)
				{
					if (!bundleName.IsNullOrEmpty() && !bundlesChecked.Contains(bundleName))
					{
						bundlesChecked.Add(bundleName);
						Wooroutine<bool> checkRoutine = this.assetBundleService.IsBundleAvailable(bundleName);
						yield return checkRoutine;
						bool isAvailable = false;
						try
						{
							isAvailable = checkRoutine.ReturnValue;
						}
						catch (Exception ex)
						{
							WoogaDebug.Log(new object[]
							{
								ex.Message
							});
						}
						if (isAvailable)
						{
							this.bundlesAvailable.Add(bundleName);
						}
					}
				}
			}
			yield break;
		}

		// Token: 0x060037C4 RID: 14276 RVA: 0x00110158 File Offset: 0x0010E558
		protected IEnumerator LoadBuildingToBundleMapRoutine()
		{
			if (BuildingResourceCache.IsValid())
			{
				this.Cache = BuildingResourceCache.Instance;
				BuildingResourceServiceRoot.buildingToBundleMap = this.Cache.BuildingToBundleMap;
				yield break;
			}
			Wooroutine<TextAsset> mapTextAsset = this.assetBundleService.LoadAsset<TextAsset>(BuildingResourceServiceRoot.MAP_ASSETBUNDLE_NAME, BuildingResourceServiceRoot.BUILDING_TO_BUNDLE_MAP_FILE_NAME_WITH_PATH);
			yield return mapTextAsset;
			TextAsset asJson = mapTextAsset.ReturnValue;
			BuildingResourceServiceRoot.buildingToBundleMap = JsonUtility.FromJson<BuildingToBundleMap>(asJson.text);
			this.Cache.Init(this.sessions, this.assetBundleService, BuildingResourceServiceRoot.buildingToBundleMap);
			yield break;
		}

		// Token: 0x060037C5 RID: 14277 RVA: 0x00110174 File Offset: 0x0010E574
		public BuildingAssetWrapper<GameObject> TryGetCachedAssetSync(BuildingConfig building, bool repaired)
		{
			GameObject asset;
			if (this.Cache.TryGetPrefab(building.Asset, repaired, out asset))
			{
				return new BuildingAssetWrapper<GameObject>(asset, false);
			}
			if (!repaired && this.UsesGenericDestroyedPrefab(building))
			{
				return new BuildingAssetWrapper<GameObject>(this.Cache.GetGenericDestroyedPrefab(building), false);
			}
			return new BuildingAssetWrapper<GameObject>(this.GetPlaceholderBuildingSync(building, repaired), true);
		}

		// Token: 0x060037C6 RID: 14278 RVA: 0x001101D8 File Offset: 0x0010E5D8
		public GameObject GetPlaceholderBuildingSync(BuildingConfig building, bool repaired)
		{
			if (!repaired)
			{
				return this.Cache.GetGenericDestroyedPrefab(building);
			}
			GameObject result;
			if (this.Cache.TryGetPrefab(building.GetPlaceholderName(), true, out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x060037C7 RID: 14279 RVA: 0x00110214 File Offset: 0x0010E614
		public Wooroutine<GameObject> GetPrefabAsync(BuildingConfig building, bool repaired)
		{
			return WooroutineRunner.StartWooroutine<GameObject>(this.GetPrefabAsyncRoutine(building, repaired));
		}

		// Token: 0x060037C8 RID: 14280 RVA: 0x00110224 File Offset: 0x0010E624
		public bool NeedsToDownloadBundleFor(string assetName, bool asDestroyed, bool asIcon = false)
		{
			BuildingToBundleMap.Entry entry;
			if (BuildingResourceServiceRoot.buildingToBundleMap != null && BuildingResourceServiceRoot.buildingToBundleMap.Map.TryGetValue(assetName, out entry))
			{
				string text = (!asIcon) ? ((!asDestroyed) ? entry.prefabBundle : entry.destroyedBundle) : entry.iconBundle;
				return !text.IsNullOrEmpty() && !this.bundlesAvailable.Contains(text);
			}
			return false;
		}

		// Token: 0x060037C9 RID: 14281 RVA: 0x0011029C File Offset: 0x0010E69C
		protected bool UsesGenericDestroyedPrefab(BuildingConfig building)
		{
			BuildingToBundleMap.Entry entry;
			return !BuildingResourceServiceRoot.buildingToBundleMap.Map.TryGetValue(building.Asset, out entry) || !entry.HasCompleteDestroyedInfo;
		}

		// Token: 0x060037CA RID: 14282 RVA: 0x001102D0 File Offset: 0x0010E6D0
		private IEnumerator GetPrefabAsyncRoutine(BuildingConfig buildingConfig, bool repaired)
		{
			if (buildingConfig == null)
			{
				yield break;
			}
			GameObject buildingAsset;
			if (this.Cache.TryGetPrefab(buildingConfig.Asset, repaired, out buildingAsset))
			{
				yield return buildingAsset;
			}
			else
			{
				string assetID = buildingConfig.Asset;
				if (this.Cache.IsAssetBeingLoaded(assetID, !repaired, false))
				{
					while (this.Cache.IsAssetBeingLoaded(assetID, !repaired, false))
					{
						yield return this.waitForAssetUpdateInterval;
					}
				}
				else
				{
					yield return this.Cache.LoadBuildingComponentsFromBundleRoutine(new List<string>
					{
						buildingConfig.Asset
					}, !repaired, false);
				}
				GameObject asset = null;
				if (!this.Cache.TryGetPrefab(buildingConfig.Asset, repaired, out asset) && repaired)
				{
					WoogaDebug.Log(new object[]
					{
						"Couldn't load: ",
						buildingConfig.Asset,
						"destroyed? ",
						!repaired
					});
				}
				yield return asset;
			}
			yield break;
		}

		// Token: 0x060037CB RID: 14283 RVA: 0x001102FC File Offset: 0x0010E6FC
		public IEnumerator CacheBuildingIconAsync(BuildingConfig buildingConfig)
		{
			if (buildingConfig == null || this.Cache.IsCached(buildingConfig.name, false, true))
			{
				yield break;
			}
			yield return this.Cache.LoadBuildingComponentsFromBundleRoutine(new List<string>
			{
				buildingConfig.Asset
			}, false, true);
			yield break;
		}

		// Token: 0x060037CC RID: 14284 RVA: 0x00110320 File Offset: 0x0010E720
		public BuildingAssetWrapper<Sprite> GetWrappedSpriteOrPlaceholder(BuildingConfig building)
		{
			Sprite asset;
			if (this.Cache.TryGetIcon(building.name, out asset))
			{
				return new BuildingAssetWrapper<Sprite>(asset, false);
			}
			return new BuildingAssetWrapper<Sprite>(this.GetPlaceholderSprite(building), true);
		}

		// Token: 0x060037CD RID: 14285 RVA: 0x0011035C File Offset: 0x0010E75C
		private Sprite GetPlaceholderSprite(BuildingConfig building)
		{
			Sprite result;
			if (this.Cache.TryGetIcon("iso_under_construction1", out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x060037CE RID: 14286 RVA: 0x00110384 File Offset: 0x0010E784
		private IEnumerator LoadRequiredPrefabsRoutine()
		{
			List<string> requiredNormalBuildings;
			List<string> requiredDestroyedBuildings;
			this.FindRequiredBuildings(out requiredNormalBuildings, out requiredDestroyedBuildings);
			yield return this.Cache.LoadBuildingComponentsFromBundleRoutine(requiredNormalBuildings, false, false);
			yield return this.Cache.LoadBuildingComponentsFromBundleRoutine(requiredDestroyedBuildings, true, false);
			yield break;
		}

		// Token: 0x060037CF RID: 14287 RVA: 0x001103A0 File Offset: 0x0010E7A0
		private void FindRequiredBuildings(out List<string> requiredNormalBuildings, out List<string> requiredDestroyedBuildings)
		{
			int num = this.gameState.Progression.LastUnlockedArea + 1;
			List<BuildingInstance.PersistentData> buildings = this.gameState.Buildings.BuildingsData.Buildings;
			IEnumerable<StartBuilding> allOnIsland = this.configService.startBuilding.GetAllOnIsland(this.gameState.Buildings.CurrentIsland);
			Dictionary<string, BuildingConfig> map = this.configService.buildingConfigList.Map;
			HashSet<string> hashSet = new HashSet<string>();
			HashSet<string> hashSet2 = new HashSet<string>();
			foreach (BuildingInstance.PersistentData persistentData in buildings)
			{
				BuildingConfig buildingConfig;
				if (map.TryGetValue(persistentData.blueprintName, out buildingConfig))
				{
					if (persistentData.IsRepaired)
					{
						hashSet.Add(buildingConfig.Asset);
					}
					else
					{
						hashSet2.Add(buildingConfig.Asset);
					}
				}
				else
				{
					Log.Warning("ConfigError", "Couldn't find config for " + persistentData.blueprintName, null);
				}
			}
			foreach (StartBuilding startBuilding in allOnIsland)
			{
				BuildingConfig buildingConfig2;
				if (map.TryGetValue(startBuilding.building_id, out buildingConfig2))
				{
					if (startBuilding.area >= num)
					{
						if (startBuilding.destroyed)
						{
							hashSet2.Add(buildingConfig2.Asset);
						}
						else
						{
							hashSet.Add(buildingConfig2.Asset);
						}
					}
				}
				else
				{
					Log.Warning("ConfigError", "Couldn't find config for " + startBuilding.building_id, null);
				}
			}
			requiredNormalBuildings = hashSet.ToList<string>();
			requiredDestroyedBuildings = hashSet2.ToList<string>();
		}

		// Token: 0x060037D0 RID: 14288 RVA: 0x0011058C File Offset: 0x0010E98C
		private IEnumerator LoadGenericPrefabsRoutine()
		{
			List<string> genericPrefabs = new List<string>();
			for (int i = 1; i <= BuildingResourceServiceRoot.MAX_PLACEHOLDER_SIZE; i++)
			{
				genericPrefabs.Add(string.Format("iso_deco_destroyed_{0}x{0}", i));
				genericPrefabs.Add(string.Format("iso_nature_destroyed_{0}x{0}", i));
				genericPrefabs.Add(string.Format("iso_under_construction{0}", i));
			}
			genericPrefabs.Add("iso_connectable_destroyed");
			genericPrefabs.Add("iso_road_stone");
			yield return this.Cache.LoadBuildingComponentsFromBundleRoutine(genericPrefabs, false, false);
			genericPrefabs.Clear();
			genericPrefabs.Add("iso_under_construction1");
			yield return this.Cache.LoadBuildingComponentsFromBundleRoutine(genericPrefabs, false, true);
			yield break;
		}

		// Token: 0x060037D1 RID: 14289 RVA: 0x001105A7 File Offset: 0x0010E9A7
		public Coroutine LoadSprites()
		{
			return base.StartCoroutine(this.LoadAllSpritesRoutine());
		}

		// Token: 0x060037D2 RID: 14290 RVA: 0x001105B8 File Offset: 0x0010E9B8
		private IEnumerator LoadAllSpritesRoutine()
		{
			Dictionary<string, int> storedShared = this.gameState.Buildings.SharedStoredBuildings;
			int currentIsland = this.gameState.Buildings.CurrentIsland;
			List<string> activeSeasonSetNames = new List<string>();
			SeasonConfig activeSeason = this.seasonService.GetActiveSeason();
			if (activeSeason != null)
			{
				activeSeasonSetNames.AddRange(activeSeason.SetNames);
			}
			SeasonPrizeInfo previousSeason = this.seasonService.GetPreviousSeasonPrizeInfo();
			if (previousSeason != null)
			{
				activeSeasonSetNames.Add(previousSeason.name);
			}
			Func<BuildingConfig, bool>[] array = new Func<BuildingConfig, bool>[4];
			array[0] = BuildingResourceServiceRoot.CreateActiveSeasonSpritesFilter(activeSeasonSetNames);
			array[1] = ((BuildingConfig building) => building.shared_storage == 0 && building.island_id == currentIsland);
			array[2] = ((BuildingConfig building) => building.shared_storage == 1 && !building.IsSeasonal());
			array[3] = ((BuildingConfig building) => storedShared.ContainsKey(building.name));
			Func<BuildingConfig, bool> filters = BuildingResourceServiceRoot.Compose(array);
			yield return this.LoadFilteredSpritesRoutine(filters);
			this.onSpritesLoaded.Dispatch();
			yield break;
		}

		// Token: 0x060037D3 RID: 14291 RVA: 0x001105D4 File Offset: 0x0010E9D4
		public IEnumerator LoadFilteredSpritesRoutine(Func<BuildingConfig, bool> pred)
		{
			List<string> spritesToLoad = (from building in this.configService.buildingConfigList.buildings.Where(pred)
			select building.name).ToList<string>();
			yield return this.Cache.LoadBuildingComponentsFromBundleRoutine(spritesToLoad, false, true);
			yield break;
		}

		// Token: 0x060037D4 RID: 14292 RVA: 0x001105F8 File Offset: 0x0010E9F8
		public static Func<BuildingConfig, bool> CreateSeasonSpritesFilter(string seasonName)
		{
			return (BuildingConfig building) => building.IsSeasonal() && building.season == seasonName;
		}

		// Token: 0x060037D5 RID: 14293 RVA: 0x00110620 File Offset: 0x0010EA20
		public static Func<BuildingConfig, bool> CreateActiveSeasonSpritesFilter(List<string> activeSeasonSetNames)
		{
			return (BuildingConfig building) => building.IsSeasonal() && activeSeasonSetNames.Contains(building.season);
		}

		// Token: 0x060037D6 RID: 14294 RVA: 0x00110648 File Offset: 0x0010EA48
		public static Func<BuildingConfig, bool> Compose(IEnumerable<Func<BuildingConfig, bool>> filters)
		{
			return (BuildingConfig building) => filters.Any((Func<BuildingConfig, bool> e) => e(building));
		}

		// Token: 0x060037D7 RID: 14295 RVA: 0x0011066E File Offset: 0x0010EA6E
		public bool IsFriendsGameState()
		{
			return !this.gameState.IsMyOwnState;
		}

		// Token: 0x04005FE9 RID: 24553
		public static string JSON_PATH = "Assets/Puzzletown/Config/";

		// Token: 0x04005FEA RID: 24554
		public static string BUILDING_TO_BUNDLE_MAP_FILE_NAME_WITH_PATH = BuildingResourceServiceRoot.JSON_PATH + "buildingtobundlemap.json.txt";

		// Token: 0x04005FEB RID: 24555
		public static string MAP_ASSETBUNDLE_NAME = "bldg_to_bundle_map";

		// Token: 0x04005FEC RID: 24556
		public static int MAX_PLACEHOLDER_SIZE = 3;

		// Token: 0x04005FED RID: 24557
		public static string START_PRELOAD_PP_KEY = "STARTLOADASSETS";

		// Token: 0x04005FEE RID: 24558
		public static bool startGreedyLoadingAssets = true;

		// Token: 0x04005FEF RID: 24559
		private static BuildingToBundleMap buildingToBundleMap;

		// Token: 0x04005FF0 RID: 24560
		[WaitForService(true, true)]
		public AssetBundleService assetBundleService;

		// Token: 0x04005FF1 RID: 24561
		[WaitForService(true, true)]
		public ConfigService configService;

		// Token: 0x04005FF2 RID: 24562
		[WaitForService(true, true)]
		public SeasonService seasonService;

		// Token: 0x04005FF3 RID: 24563
		[WaitForService(true, true)]
		public SessionService sessions;

		// Token: 0x04005FF4 RID: 24564
		[WaitForService(true, true)]
		private GameStateService gameState;

		// Token: 0x04005FF5 RID: 24565
		[WaitForService(true, true)]
		private AssetBundlePreloadService preloadService;

		// Token: 0x04005FF6 RID: 24566
		public const string PLACEHOLDER_ICON = "iso_under_construction1";

		// Token: 0x04005FF7 RID: 24567
		public const string CONNECTABLE_DESTROYED = "iso_connectable_destroyed";

		// Token: 0x04005FF8 RID: 24568
		public const string PLACEHOLDER_ROAD = "iso_road_stone";

		// Token: 0x04005FF9 RID: 24569
		public const string DECO_DESTROYED = "iso_deco_destroyed_{0}x{0}";

		// Token: 0x04005FFA RID: 24570
		public const string NATURE_DESTROYED = "iso_nature_destroyed_{0}x{0}";

		// Token: 0x04005FFB RID: 24571
		public const string PLACEHOLDER_GENERIC = "iso_under_construction{0}";

		// Token: 0x04005FFC RID: 24572
		private BuildingResourceCache cache;

		// Token: 0x04005FFD RID: 24573
		private HashSet<string> bundlesAvailable = new HashSet<string>();

		// Token: 0x04005FFE RID: 24574
		public readonly AwaitSignal onReady = new AwaitSignal();

		// Token: 0x04005FFF RID: 24575
		public readonly AwaitSignal onSpritesLoaded = new AwaitSignal();

		// Token: 0x04006000 RID: 24576
		private WaitForSeconds waitForAssetUpdateInterval = new WaitForSeconds(0.05f);
	}
}
