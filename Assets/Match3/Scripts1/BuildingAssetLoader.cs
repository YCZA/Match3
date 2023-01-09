using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Puzzletown.Town;
using Match3.Scripts1.Spine;
using Match3.Scripts1.Spine.Unity;
using Match3.Scripts1.Town;
using Match3.Scripts1.UnityEngine;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.Services.ErrorAnalytics;
using Match3.Scripts1.Wooga.Signals;
using UnityEngine;

// Token: 0x0200095A RID: 2394
namespace Match3.Scripts1
{
	public class BuildingAssetLoader : MonoBehaviour
	{
		// Token: 0x170008E7 RID: 2279
		// (get) Token: 0x06003A62 RID: 14946 RVA: 0x00120DEF File Offset: 0x0011F1EF
		// (set) Token: 0x06003A63 RID: 14947 RVA: 0x00120DF7 File Offset: 0x0011F1F7
		public GameObject BuildingAsset { get; private set; }

		// Token: 0x06003A64 RID: 14948 RVA: 0x00120E00 File Offset: 0x0011F200
		public void SetupServices(BuildingResourceServiceRoot buildingResService)
		{
			if (BuildingAssetLoader.timeSlicer == null)
			{
				this.SetupTimeSlicer();
			}
			this.buildingResourceService = buildingResService;
		}

		// Token: 0x06003A65 RID: 14949 RVA: 0x00120E1C File Offset: 0x0011F21C
		private void SetupTimeSlicer()
		{
			BuildingAssetLoader.timeSlice = PlayerPrefs.GetFloat(BuildingAssetLoader.SLICE_LENGTH_PP_KEY, 0.01f);
			BuildingAssetLoader.useTimeSlicer = (PlayerPrefs.GetInt(BuildingAssetLoader.USE_SLICER_PP_KEY, 1) == 1);
			BuildingAssetLoader.mockOnDemandFetch = (PlayerPrefs.GetInt(BuildingAssetLoader.MOCK_OD_PP_KEY, 0) == 1);
			BuildingAssetLoader.mockOffline = (PlayerPrefs.GetInt(BuildingAssetLoader.MOCK_OFFLINE_PP_KEY, 0) == 1);
			BuildingAssetLoader.timeSlicer = new TimeSlicer(BuildingAssetLoader.timeSlice);
		}

		// Token: 0x06003A66 RID: 14950 RVA: 0x00120E85 File Offset: 0x0011F285
		public void Show(BuildingInstance building)
		{
			this.data = building;
			this.Refresh(building);
			this.AddListeners();
		}

		// Token: 0x06003A67 RID: 14951 RVA: 0x00120E9B File Offset: 0x0011F29B
		private void OnDestroy()
		{
			this.RemoveListeners();
		}

		// Token: 0x06003A68 RID: 14952 RVA: 0x00120EA4 File Offset: 0x0011F2A4
		private void AddListeners()
		{
			if (this.data != null)
			{
				this.data.onTimer.AddListener(new Action<BuildingInstance>(this.RefreshOnTimer));
				this.data.onSelected.AddListener(new Action<BuildingInstance, bool>(this.HandleSelected));
				this.data.onPositionChanged.AddListener(new Action<BuildingInstance>(this.HandlePositionChanged));
			}
		}

		// Token: 0x06003A69 RID: 14953 RVA: 0x00120F10 File Offset: 0x0011F310
		private void RemoveListeners()
		{
			if (this.data != null)
			{
				this.data.onTimer.RemoveListener(new Action<BuildingInstance>(this.RefreshOnTimer));
				this.data.onSelected.RemoveListener(new Action<BuildingInstance, bool>(this.HandleSelected));
				this.data.onPositionChanged.RemoveListener(new Action<BuildingInstance>(this.HandlePositionChanged));
			}
		}

		// Token: 0x06003A6A RID: 14954 RVA: 0x00120F7C File Offset: 0x0011F37C
		private void HandlePositionChanged(BuildingInstance building)
		{
			this.ExecuteOnChildren(delegate(IBuildingOnPositionChanged h)
			{
				h.HandlePositionChanged(building);
			}, true);
		}

		// Token: 0x06003A6B RID: 14955 RVA: 0x00120FAC File Offset: 0x0011F3AC
		public static void ToggleBuildingLayer(BuildingInstance b, Renderer r, bool selected)
		{
			BuildingMainView buildingMainView = r.GetComponentsInParent<BuildingMainView>(true)[0];
			bool flag = b.isForeshadowing && !TownCheatsRoot.EditMode;
			r.gameObject.layer = ((!selected) ? ((!flag) ? b.objectLayer : 0) : 15);
			buildingMainView.gameObject.layer = ((!selected) ? 16 : 0);
		}

		// Token: 0x06003A6C RID: 14956 RVA: 0x0012101D File Offset: 0x0011F41D
		private void HandleSelected(BuildingInstance building, bool selected)
		{
			if (base.isActiveAndEnabled)
			{
				base.StartCoroutine(this.HandleSelectedRoutine(building, selected));
			}
		}

		// Token: 0x06003A6D RID: 14957 RVA: 0x0012103C File Offset: 0x0011F43C
		private IEnumerator HandleSelectedRoutine(BuildingInstance building, bool selected)
		{
			while (!this.BuildingAsset)
			{
				yield return null;
			}
			foreach (Renderer r in this.BuildingAsset.GetComponentsInChildren<Renderer>(true))
			{
				BuildingAssetLoader.ToggleBuildingLayer(building, r, selected);
			}
			yield break;
		}

		// Token: 0x06003A6E RID: 14958 RVA: 0x00121065 File Offset: 0x0011F465
		private void RefreshOnTimer(BuildingInstance building)
		{
			if (base.isActiveAndEnabled)
			{
				base.StartCoroutine(this.RefreshOnTimerRoutine(building));
			}
		}

		// Token: 0x06003A6F RID: 14959 RVA: 0x00121080 File Offset: 0x0011F480
		private IEnumerator RefreshOnTimerRoutine(BuildingInstance building)
		{
			yield return this.RefreshRoutine(building);
			yield break;
		}

		// Token: 0x06003A70 RID: 14960 RVA: 0x001210A4 File Offset: 0x0011F4A4
		private static void ScaleDown(BuildingInstance building, GameObject asset)
		{
			AssetDestroyTrigger @object = asset.AddComponent<AssetDestroyTrigger>();
			Tweener t = asset.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InSine);
			asset.transform.DOLocalMove(building.worldCenter, 0.5f, false).SetEase(Ease.InSine);
			asset.layer = 0;
			t.OnComplete(new TweenCallback(@object.OnTweenEnd));
		}

		// Token: 0x06003A71 RID: 14961 RVA: 0x0012110C File Offset: 0x0011F50C
		private static void ScaleUp(BuildingInstance building, GameObject asset)
		{
			Vector3 endValue = Vector3.one * (float)building.blueprint.size;
			Vector3 zero = Vector3.zero;
			asset.transform.localPosition = building.worldCenter;
			asset.transform.localScale = Vector3.zero;
			asset.transform.DOScale(endValue, 0.5f).SetEase(Ease.OutBack);
			asset.transform.DOLocalMove(zero, 0.5f, false).SetEase(Ease.OutBack);
		}

		// Token: 0x06003A72 RID: 14962 RVA: 0x0012118A File Offset: 0x0011F58A
		private void Refresh(BuildingInstance building)
		{
			if (base.isActiveAndEnabled)
			{
				base.StartCoroutine(this.RefreshRoutine(building));
			}
		}

		// Token: 0x06003A73 RID: 14963 RVA: 0x001211A8 File Offset: 0x0011F5A8
		private IEnumerator RefreshRoutine(BuildingInstance building)
		{
			while (this.isRefreshing)
			{
				yield return null;
			}
			this.isRefreshing = true;
			yield return this.buildingResourceService.onReady;
			BuildingAssetWrapper<GameObject> buildingAssetContainer = this.TryGetCachedAssetSync(building, building.sv.IsRepaired);
			if (!buildingAssetContainer.isPlaceHolder || this.NeedsToDownloadBundleFor(building, building.sv.IsRepaired))
			{
				yield return this.TryShowNewBuildingAsset(building, buildingAssetContainer.asset);
				if (!buildingAssetContainer.isPlaceHolder || this.buildingResourceService.IsFriendsGameState())
				{
					this.HandleRefreshFinished(building.State);
					this.isRefreshing = false;
					yield break;
				}
			}
			Wooroutine<GameObject> reference = this.StartWooroutine<GameObject>(this.GetActualAsset(building));
			yield return reference;
			GameObject assetToShow = reference.ReturnValue;
			if (assetToShow != null && !BuildingAssetLoader.mockOffline)
			{
				yield return this.TryShowNewBuildingAsset(building, assetToShow);
			}
			this.HandleRefreshFinished(building.State);
			this.isRefreshing = false;
			yield break;
		}

		// Token: 0x06003A74 RID: 14964 RVA: 0x001211CC File Offset: 0x0011F5CC
		private bool NeedsToDownloadBundleFor(BuildingInstance building, bool isRepaired)
		{
			return BuildingAssetLoader.mockOffline || BuildingAssetLoader.mockOnDemandFetch || (this.buildingResourceService != null && this.buildingResourceService.NeedsToDownloadBundleFor(building.blueprint.Asset, !isRepaired, false));
		}

		// Token: 0x06003A75 RID: 14965 RVA: 0x00121220 File Offset: 0x0011F620
		private IEnumerator GetActualAsset(BuildingInstance building)
		{
			Wooroutine<GameObject> prefabLoadingRoutine = this.buildingResourceService.GetPrefabAsync(building.blueprint, building.sv.IsRepaired);
			yield return prefabLoadingRoutine;
			GameObject prefab;
			try
			{
				prefab = prefabLoadingRoutine.ReturnValue;
			}
			catch (Exception)
			{
				prefab = null;
			}
			if (BuildingAssetLoader.mockOnDemandFetch)
			{
				yield return new WaitForSeconds(global::UnityEngine.Random.Range(1f, 10f));
			}
			yield return prefab;
			yield break;
		}

		// Token: 0x06003A76 RID: 14966 RVA: 0x00121244 File Offset: 0x0011F644
		private BuildingAssetWrapper<GameObject> TryGetCachedAssetSync(BuildingInstance building, bool repaired)
		{
			if (BuildingAssetLoader.mockOnDemandFetch || BuildingAssetLoader.mockOffline)
			{
				GameObject placeholderBuildingSync = this.buildingResourceService.GetPlaceholderBuildingSync(building.blueprint, repaired);
				return new BuildingAssetWrapper<GameObject>(placeholderBuildingSync, true);
			}
			return this.buildingResourceService.TryGetCachedAssetSync(building.blueprint, repaired);
		}

		// Token: 0x06003A77 RID: 14967 RVA: 0x00121294 File Offset: 0x0011F694
		private IEnumerator TryShowNewBuildingAsset(BuildingInstance building, GameObject assetReference)
		{
			GameObject previousAsset = this.BuildingAsset;
			if (this._previousState == assetReference)
			{
				yield break;
			}
			this._previousState = assetReference;
			BuildingAssetLoader.HidePreviousAsset(building, previousAsset);
			if (!this)
			{
				this.LogSelfNullError(building);
				yield break;
			}
			if (BuildingAssetLoader.useTimeSlicer && building.instantiateWithTimeSlicing)
			{
				YieldInstruction op = BuildingAssetLoader.timeSlicer.AddAndAwait(delegate
				{
					this.InstantiateAsset(building, assetReference);
				});
				yield return op;
			}
			else
			{
				this.InstantiateAsset(building, assetReference);
			}
			this.HandleSelected(building, BuildingLocation.Selected == building);
			BuildingAssetLoader.RealignAllMeshFilters(base.GetComponentsInChildren<MeshFilter>());
			this.ShowNewAsset(building, previousAsset);
			building.onAssetUpdated.Dispatch(building);
			yield break;
		}

		// Token: 0x06003A78 RID: 14968 RVA: 0x001212C0 File Offset: 0x0011F6C0
		private void LogSelfNullError(BuildingInstance building)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["building.asset"] = building.blueprint.Asset;
			dictionary["persistentData"] = building.sv;
			Log.Error("InvalidGameObject", "BuildingAssetLoader", dictionary);
		}

		// Token: 0x06003A79 RID: 14969 RVA: 0x0012130C File Offset: 0x0011F70C
		private void InstantiateAsset(BuildingInstance building, GameObject reference)
		{
			if (reference == null)
			{
				return;
			}
			// eli key point 实例化建筑
			// Vector3 rotation = new Vector3(45, 45, 0);
			this.BuildingAsset = global::UnityEngine.Object.Instantiate<GameObject>(reference, transform);
			BuildingAsset.transform.localPosition = Vector3.zero;
			BuildingAsset.transform.localScale = Vector3.one * (float)building.blueprint.size;
			if (BuildingAsset.GetComponent<SpriteRenderer>())
			{
				BuildingAsset.GetComponent<SpriteRenderer>().sortingOrder = 1;
			}
		
			building.objectLayer = reference.layer;
			this.ShowOnChildren(building, false, false);
		}

		// Token: 0x06003A7A RID: 14970 RVA: 0x00121383 File Offset: 0x0011F783
		private void ShowNewAsset(BuildingInstance building, GameObject previousAsset)
		{
			if (!previousAsset)
			{
				return;
			}
			BuildingAssetLoader.ScaleUp(building, this.BuildingAsset);
		}

		// Token: 0x06003A7B RID: 14971 RVA: 0x001213A0 File Offset: 0x0011F7A0
		private static void HidePreviousAsset(BuildingInstance building, GameObject previousAsset)
		{
			if (!previousAsset)
			{
				return;
			}
			SkeletonAnimation componentInChildren = previousAsset.GetComponentInChildren<SkeletonAnimation>();
			if (componentInChildren)
			{
				Spine.Animation animation = componentInChildren.state.Data.skeletonData.FindAnimation("reveal");
				if (animation != null)
				{
					AssetDestroyTrigger @object = previousAsset.AddComponent<AssetDestroyTrigger>();
					TrackEntry trackEntry = componentInChildren.state.SetAnimation(0, animation, false);
					Renderer componentInChildren2 = previousAsset.GetComponentInChildren<Renderer>();
					trackEntry.End += @object.OnAnimationEnd;
					componentInChildren2.sortingOrder += 10;
					componentInChildren2.gameObject.layer = 0;
				}
				else
				{
					BuildingAssetLoader.ScaleDown(building, previousAsset);
				}
			}
			else
			{
				BuildingAssetLoader.ScaleDown(building, previousAsset);
			}
		}

		// Token: 0x06003A7C RID: 14972 RVA: 0x00121451 File Offset: 0x0011F851
		private static GameObject GetBySize(BuildingConfig blueprint, GameObject[] array)
		{
			return array[((blueprint.size <= array.Length) ? blueprint.size : array.Length) - 1];
		}

		// Token: 0x06003A7D RID: 14973 RVA: 0x00121474 File Offset: 0x0011F874
		public static void RealignAllMeshFilters(IEnumerable<MeshFilter> meshFilters)
		{
			foreach (MeshFilter meshFilter in meshFilters)
			{
				Mesh mesh = meshFilter.mesh;
				if (mesh)
				{
					mesh.bounds = BuildingLocation.RealignBounds(mesh.bounds);
				}
			}
		}

		// Token: 0x06003A7E RID: 14974 RVA: 0x001214EC File Offset: 0x0011F8EC
		private void HandleRefreshFinished(BuildingState buildingState)
		{
			if (buildingState == BuildingState.Active)
			{
				this.onRefreshFinished.Dispatch();
			}
			if (!this.data.isForeshadowing)
			{
				MeshRenderer[] componentsInChildren = base.GetComponentsInChildren<MeshRenderer>();
				foreach (MeshRenderer meshRenderer in componentsInChildren)
				{
					meshRenderer.sortingLayerName = CameraExtensions.ABOVE_FORSHADOWING_LAYER;
				}
			}
			this.data.controller.onBuildingsRefreshed.Dispatch();
		}

		// Token: 0x04006257 RID: 25175
		public readonly Signal onRefreshFinished = new Signal();

		// Token: 0x04006258 RID: 25176
		private const float SCALE_DURATION = 0.5f;

		// Token: 0x04006259 RID: 25177
		private const string REVEAL = "reveal";

		// Token: 0x0400625A RID: 25178
		private BuildingResourceServiceRoot buildingResourceService;

		// Token: 0x0400625B RID: 25179
		public static TimeSlicer timeSlicer;

		// Token: 0x0400625C RID: 25180
		public static string USE_SLICER_PP_KEY = "TIMESLICER";

		// Token: 0x0400625D RID: 25181
		public static string SLICE_LENGTH_PP_KEY = "SLICELENGTH";

		// Token: 0x0400625E RID: 25182
		public static string MOCK_OD_PP_KEY = "MOCKNET";

		// Token: 0x0400625F RID: 25183
		public static string MOCK_OFFLINE_PP_KEY = "MOCKOFFLINE";

		// Token: 0x04006260 RID: 25184
		public static bool useTimeSlicer;

		// Token: 0x04006261 RID: 25185
		public static float timeSlice = 0.01f;

		// Token: 0x04006262 RID: 25186
		public static bool mockOnDemandFetch;

		// Token: 0x04006263 RID: 25187
		public static bool mockOffline;

		// Token: 0x04006264 RID: 25188
		private BuildingInstance data;

		// Token: 0x04006266 RID: 25190
		private GameObject _previousState;

		// Token: 0x04006267 RID: 25191
		private bool isRefreshing;
	}
}
