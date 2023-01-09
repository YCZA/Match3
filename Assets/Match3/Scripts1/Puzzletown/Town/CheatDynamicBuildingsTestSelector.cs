using System;
using System.Collections;
using System.Text;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Shared.Util;
using UnityEngine;
using UnityEngine.UI;

namespace Match3.Scripts1.Puzzletown.Town
{
	// Token: 0x020009AF RID: 2479
	public class CheatDynamicBuildingsTestSelector : MonoBehaviour, IHandler<DynamicBuildingsTest>
	{
		// Token: 0x06003C06 RID: 15366 RVA: 0x0012A530 File Offset: 0x00128930
		public IEnumerator SelectTestRoutine()
		{
			this.isRunning = true;
			this.ShowSelectorButtons();
			this.UpdateStatus();
			while (this.isRunning)
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x06003C07 RID: 15367 RVA: 0x0012A54B File Offset: 0x0012894B
		private void ShowSelectorButtons()
		{
			this.dataSource.Show((DynamicBuildingsTest[])Enum.GetValues(typeof(DynamicBuildingsTest)));
		}

		// Token: 0x06003C08 RID: 15368 RVA: 0x0012A56C File Offset: 0x0012896C
		public void HandleCancelTap()
		{
			this.isRunning = false;
		}

		// Token: 0x06003C09 RID: 15369 RVA: 0x0012A578 File Offset: 0x00128978
		public void Handle(DynamicBuildingsTest test)
		{
			switch (test)
			{
			case DynamicBuildingsTest.ToggleSlicing:
				this.ToggleSlicing();
				break;
			case DynamicBuildingsTest.SliceBitShorter:
				this.AdjustSlice(-0.001f);
				break;
			case DynamicBuildingsTest.SliceBitLonger:
				this.AdjustSlice(0.001f);
				break;
			case DynamicBuildingsTest.SliceShorter:
				this.AdjustSlice(-0.01f);
				break;
			case DynamicBuildingsTest.SliceLonger:
				this.AdjustSlice(0.01f);
				break;
			case DynamicBuildingsTest.MockOnDemand:
				this.ToggleMockOnDemand();
				break;
			case DynamicBuildingsTest.MockOffline:
				this.ToggleMockOffline();
				break;
			case DynamicBuildingsTest.LoadMore:
				this.AdjustRequiredBuildingRatio(0.05f);
				break;
			case DynamicBuildingsTest.LoadFewer:
				this.AdjustRequiredBuildingRatio(-0.05f);
				break;
			case DynamicBuildingsTest.TogglePreload:
				this.TogglePreload();
				break;
			}
		}

		// Token: 0x06003C0A RID: 15370 RVA: 0x0012A64C File Offset: 0x00128A4C
		private void AdjustRequiredBuildingRatio(float delta)
		{
			float requiredBuildingRatio = TownSceneLoader.requiredBuildingRatio;
			float num = Mathf.Clamp01(requiredBuildingRatio + delta);
			if (!Mathf.Approximately(requiredBuildingRatio, num))
			{
				PlayerPrefs.SetFloat(TownSceneLoader.REQUIRED_BUILDING_RATIO_PP_KEY, num);
				PlayerPrefs.Save();
				TownSceneLoader.requiredBuildingRatio = num;
				this.UpdateStatus();
			}
		}

		// Token: 0x06003C0B RID: 15371 RVA: 0x0012A690 File Offset: 0x00128A90
		private void AdjustSlice(float delta)
		{
			float timeSlice = BuildingAssetLoader.timeSlice;
			float num = Mathf.Clamp(timeSlice + delta, 0.001f, 2f);
			if (!Mathf.Approximately(timeSlice, num))
			{
				PlayerPrefs.SetFloat(BuildingAssetLoader.SLICE_LENGTH_PP_KEY, num);
				PlayerPrefs.Save();
				BuildingAssetLoader.timeSlice = num;
				BuildingAssetLoader.timeSlicer.TimeSlice = num;
				this.UpdateStatus();
			}
		}

		// Token: 0x06003C0C RID: 15372 RVA: 0x0012A6EC File Offset: 0x00128AEC
		private void TogglePreload()
		{
			bool flag = !BuildingResourceServiceRoot.startGreedyLoadingAssets;
			BuildingResourceServiceRoot.startGreedyLoadingAssets = flag;
			PlayerPrefs.SetInt(BuildingResourceServiceRoot.START_PRELOAD_PP_KEY, (!flag) ? 0 : 1);
			PlayerPrefs.Save();
			this.UpdateStatus();
		}

		// Token: 0x06003C0D RID: 15373 RVA: 0x0012A72C File Offset: 0x00128B2C
		private void ToggleMockOffline()
		{
			bool flag = !BuildingAssetLoader.mockOffline;
			BuildingAssetLoader.mockOffline = flag;
			PlayerPrefs.SetInt(BuildingAssetLoader.MOCK_OFFLINE_PP_KEY, (!flag) ? 0 : 1);
			PlayerPrefs.Save();
			this.UpdateStatus();
		}

		// Token: 0x06003C0E RID: 15374 RVA: 0x0012A76C File Offset: 0x00128B6C
		private void ToggleMockOnDemand()
		{
			bool flag = !BuildingAssetLoader.mockOnDemandFetch;
			BuildingAssetLoader.mockOnDemandFetch = flag;
			PlayerPrefs.SetInt(BuildingAssetLoader.MOCK_OD_PP_KEY, (!flag) ? 0 : 1);
			PlayerPrefs.Save();
			this.UpdateStatus();
		}

		// Token: 0x06003C0F RID: 15375 RVA: 0x0012A7AC File Offset: 0x00128BAC
		private void ToggleSlicing()
		{
			bool flag = !BuildingAssetLoader.useTimeSlicer;
			BuildingAssetLoader.useTimeSlicer = flag;
			PlayerPrefs.SetInt(BuildingAssetLoader.USE_SLICER_PP_KEY, (!flag) ? 0 : 1);
			PlayerPrefs.Save();
			this.UpdateStatus();
		}

		// Token: 0x06003C10 RID: 15376 RVA: 0x0012A7EC File Offset: 0x00128BEC
		private void UpdateStatus()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("Be as lazy as possible? {0}\n", this.GetState(!BuildingResourceServiceRoot.startGreedyLoadingAssets));
			stringBuilder.AppendFormat("Use time slicing? {0}\n", this.GetState(BuildingAssetLoader.useTimeSlicer));
			stringBuilder.AppendFormat("Time slice: {0:F3} seconds\n", BuildingAssetLoader.timeSlice);
			stringBuilder.AppendFormat("Mock on demand load: {0}\n", this.GetState(BuildingAssetLoader.mockOnDemandFetch));
			stringBuilder.AppendFormat("Mock being offline: {0}\n", this.GetState(BuildingAssetLoader.mockOffline));
			stringBuilder.AppendFormat("Loaded behind loading screen: {0}%\n", (int)(100f * TownSceneLoader.requiredBuildingRatio));
			if (Profiler.TryGetDuration("ISLAND_LOAD", out CheatDynamicBuildingsTestSelector.islandLoadDuration))
			{
				stringBuilder.AppendFormat("Last island load duration (sec): {0:F3}\n", CheatDynamicBuildingsTestSelector.islandLoadDuration);
			}
			this.statusLabel.text = stringBuilder.ToString();
		}

		// Token: 0x06003C11 RID: 15377 RVA: 0x0012A8CC File Offset: 0x00128CCC
		private string GetState(bool value)
		{
			return (!value) ? "false" : "true";
		}

		// Token: 0x04006421 RID: 25633
		public static float islandLoadDuration = -1f;

		// Token: 0x04006422 RID: 25634
		public DynamicBuildingsTestDataSource dataSource;

		// Token: 0x04006423 RID: 25635
		private bool isRunning;

		// Token: 0x04006424 RID: 25636
		public Text statusLabel;
	}
}
