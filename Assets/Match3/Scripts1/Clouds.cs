using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Match3.Scripts1.Puzzletown.Config;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Puzzletown.Town;
using Match3.Scripts1.Wooga.UI;
using UnityEngine;

// Token: 0x02000912 RID: 2322
namespace Match3.Scripts1
{
	public class Clouds : MonoBehaviour, IEditorDescription
	{
		// Token: 0x0600389C RID: 14492 RVA: 0x001166E4 File Offset: 0x00114AE4
		public void Init(ProgressionDataService.Service progression, ConfigService configService, TownPathfinding map, QuestService quests, TownOverheadUiRoot overheadUIroot)
		{
			this.progression = progression;
			this.map = map;
			this.quests = quests;
			this.config = configService.SbsConfig.islandareaconfig;
			this.islandId = map.islandId;
			int unlockedAreaWithQuestAndEndOfContent = quests.UnlockedAreaWithQuestAndEndOfContent;
			int localAreaUnlocked = this.config.ClampGlobalToLocalArea(this.islandId, unlockedAreaWithQuestAndEndOfContent);
			this.cloudsLimit = this.config.CloudLimitOnIsland(this.islandId, localAreaUnlocked);
			this.CreateAreas();
			if (this.borderMaterial)
			{
				map.onAreaClick.AddListener(new Action<int>(this.OnAreaClick));
			}
			progression.onCleanupChanged.AddListener(new Action<int>(this.OnCleanupChanged));
			base.StartCoroutine(this.ShowAfterOverheadUISetup(overheadUIroot));
		}

		// Token: 0x0600389D RID: 14493 RVA: 0x001167AA File Offset: 0x00114BAA
		public string GetEditorDescription()
		{
			return this.mode.ToString();
		}

		// eli key point 创建area 
		public void CreateAreas()
		{
			int num = this.cloudsLimit;
			this.areaClouds = new AreaClouds[num];
			for (int i = 0; i < num; i++)
			{
				int num2 = i + 1;
				GameObject gameObject = new GameObject(string.Format("Area {0}", num2));
				MeshRenderer cloudsRenderer = gameObject.AddComponent<MeshRenderer>();
				AreaClouds areaClouds = gameObject.AddComponent<AreaClouds>();
				gameObject.AddComponent<MeshFilter>();
				gameObject.transform.SetParent(base.transform, false);
				gameObject.layer = base.gameObject.layer;
				areaClouds.cloudsRenderer = cloudsRenderer;
				areaClouds.area = num2;
				if (areaClouds.area <= num)
				{
					areaClouds.defaultMaterial = this.material;
					areaClouds.selectedMaterial = this.selectedMaterial;
				}

				// 设置前面关系
				if (mode == CloudsMode.Unlocked)
				{
					cloudsRenderer.sortingOrder = 2;
				}
				else
				{
					cloudsRenderer.sortingOrder = 0;
				}
			
				this.areaClouds[i] = areaClouds;
			}
			if (this.areaLabel)
			{
				this.areaLabel.gameObject.SetActive(false);
				this.areaLabel.transform.parent.gameObject.SetActive(false);
			}
		}

		// Token: 0x0600389F RID: 14495 RVA: 0x001168C4 File Offset: 0x00114CC4
		private IEnumerator ShowAfterOverheadUISetup(TownOverheadUiRoot overheadUIroot)
		{
			while (!overheadUIroot.IsSetup)
			{
				yield return null;
			}
			this.Show();
			yield break;
		}

		// Token: 0x060038A0 RID: 14496 RVA: 0x001168E6 File Offset: 0x00114CE6
		private void OnDestroy()
		{
			if (this.progression != null)
			{
				this.progression.onCleanupChanged.RemoveListener(new Action<int>(this.OnCleanupChanged));
			}
		}

		// Token: 0x060038A1 RID: 14497 RVA: 0x00116910 File Offset: 0x00114D10
		public void OnAreaClick(int area)
		{
			if (this.map.townUi.BottomPanel.State == TownBottomPanelRoot.UIState.None)
			{
				return;
			}
			int num = area - 1;
			for (int i = 0; i < this.areaClouds.Length; i++)
			{
				if (this.areaClouds[i])
				{
					if (num == i)
					{
						this.areaClouds[i].Toggle();
					}
					else
					{
						this.areaClouds[i].isSelected = false;
					}
				}
			}
			AreaClouds areaClouds = Array.Find<AreaClouds>(this.areaClouds, (AreaClouds c) => c.IsVisible && c.isSelected);
			GameObject gameObject = this.areaLabel.transform.parent.gameObject;
			gameObject.SetActive(areaClouds);
			if (areaClouds)
			{
				this.map.townUi.BottomPanel.controlButtons.Show(areaClouds);
			}
			else
			{
				this.map.townUi.BottomPanel.State = TownBottomPanelRoot.UIState.InGameUI;
			}
		}

		// Token: 0x060038A2 RID: 14498 RVA: 0x00116A1F File Offset: 0x00114E1F
		private void OnCleanupChanged(int index)
		{
			// Debug.Log("清理云");
			if (index >= this.areaClouds.Length)
			{
				return;
			}
			if (this.mode == CloudsMode.Cleared)
			{
				this.areaClouds[index - 1].Hide();
			}
		}

		// Token: 0x060038A3 RID: 14499 RVA: 0x00116A4B File Offset: 0x00114E4B
		public void FadeOut(int index)
		{
			base.StartCoroutine(this.FadeOutRoutine(index - 1));
		}

		// Token: 0x060038A4 RID: 14500 RVA: 0x00116A60 File Offset: 0x00114E60
		private IEnumerator FadeOutRoutine(int index)
		{
			MaterialPropertyBlock props = new MaterialPropertyBlock();
			AreaClouds clouds = this.areaClouds[index];
			WaitForEndOfFrame wait = new WaitForEndOfFrame();
			if (clouds.areaLabel)
			{
				clouds.areaLabel.Hide();
			}
			if (clouds.indicators != null)
			{
				clouds.indicators.ForEach(delegate(ChapterIndicator ind)
				{
					ind.Hide();
				});
				clouds.indicators = null;
			}
			for (float t = 1f; t > 0f; t -= Time.deltaTime / this.fadeOutTime)
			{
				yield return wait;
				Color color = clouds.cloudsRenderer.material.color;
				color.a = t;
				props.SetColor("_Color", color);
				clouds.cloudsRenderer.SetPropertyBlock(props);
			}
			clouds.Hide();
			yield break;
		}

		// Token: 0x060038A5 RID: 14501 RVA: 0x00116A84 File Offset: 0x00114E84
		private void ShowClouds(TownPathfinding map, int area, AreaClouds clouds)
		{
			if (this.areaLabel || !this.centerAtTile)
			{
				clouds.cloudsRenderer.GetComponent<MeshFilter>().sharedMesh = map.GetAreaInfo(area).cloudMesh;
				if (map.GetAreaInfo(area).cloudTexture)
				{
					MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
					materialPropertyBlock.SetTexture("_MapTexture", map.GetAreaInfo(area).cloudTexture);
					clouds.cloudsRenderer.SetPropertyBlock(materialPropertyBlock);
				}
			}
			else
			{
				clouds.cloudsRenderer.GetComponent<MeshFilter>().sharedMesh = map.GetAreaInfo(area).areaMesh;
			}
			clouds.cloudsRenderer.gameObject.layer = base.gameObject.layer;
			if (area <= this.areaClouds.Length)
			{
				clouds.cloudsRenderer.sharedMaterial = this.material;
				int num = this.config.LocalAreaToGlobalArea(area, this.islandId);
				if (this.areaLabel && map.IsAreaValid(area) && this.quests.UnlockedAreaWithQuestAndEndOfContent < num)
				{
					this.AddLabel(clouds);
					this.AddChapterIndicators(clouds, num);
				}
			}
		}

		// Token: 0x060038A6 RID: 14502 RVA: 0x00116BC4 File Offset: 0x00114FC4
		private void AddLabel(AreaClouds clouds)
		{
			TownAreaLabel townAreaLabel = global::UnityEngine.Object.Instantiate<TownAreaLabel>(this.areaLabel, this.areaLabel.transform.parent, false);
			int area = this.config.LocalAreaToGlobalArea(clouds.area, this.islandId);
			townAreaLabel.SetArea(area, this.quests.localizationService);
			townAreaLabel.transform.position = this.map.FindAreaFocusPoint(clouds.area);
			townAreaLabel.gameObject.SetActive(true);
			clouds.areaLabel = townAreaLabel;
		}

		// Token: 0x060038A7 RID: 14503 RVA: 0x00116C48 File Offset: 0x00115048
		private Vector3 ConvertPosition(IntVector2 position)
		{
			IntVector2 intVector = TownPathfinding.ConvertCoordinates(position);
			return new Vector3((float)intVector.x, 0f, (float)intVector.y);
		}

		// Token: 0x060038A8 RID: 14504 RVA: 0x00116C78 File Offset: 0x00115078
		private List<IntVector2> GetChapterIndicatorPositions(AreaClouds clouds, int globalArea)
		{
			List<IntVector2> list = new List<IntVector2>();
			IEnumerable<IntVector2> areaTiles = this.map.GetAreaTiles(clouds.area);
			IntVector2 tilesCenter = TownPathfinding.GetTilesCenter(areaTiles);
			int centerSum = tilesCenter.x + tilesCenter.y;
			IntVector2 tilesCenter2 = TownPathfinding.GetTilesCenter(from t in areaTiles
				where t.x + t.y < centerSum
				select t);
			list.Add(tilesCenter2);
			if (this.config.HasOneChapterPerArea(globalArea))
			{
				return list;
			}
			IntVector2 tilesCenter3 = TownPathfinding.GetTilesCenter(from t in areaTiles
				where t.x + t.y > centerSum
				select t);
			list.Add(tilesCenter3);
			return list;
		}

		// Token: 0x060038A9 RID: 14505 RVA: 0x00116D18 File Offset: 0x00115118
		private void AddChapterIndicators(AreaClouds clouds, int globalArea)
		{
			List<ChapterIndicator> list = new List<ChapterIndicator>();
			int num = this.config.FirstChapterOfArea(globalArea);
			foreach (IntVector2 mapPos in this.GetChapterIndicatorPositions(clouds, globalArea))
			{
				ChapterIndicator item = this.InstantiateChapterIndicator(mapPos, num);
				list.Add(item);
				num++;
			}
			clouds.indicators = list;
		}

		// Token: 0x060038AA RID: 14506 RVA: 0x00116DA0 File Offset: 0x001151A0
		private ChapterIndicator InstantiateChapterIndicator(IntVector2 mapPos, int chapter)
		{
			ChapterIndicator chapterIndicator = global::UnityEngine.Object.Instantiate<ChapterIndicator>(this.map.townOverheadUi.chapterIndicator, this.map.townOverheadUi.chapterIndicator.transform.parent, false);
			chapterIndicator.transform.position = this.ConvertPosition(mapPos);
			chapterIndicator.Hide();
			chapterIndicator.SetChapter(chapter, this.quests.localizationService);
			return chapterIndicator;
		}

		// eli key point 显示area
		private void Show()
		{
			int unlockedAreaWithQuestAndEndOfContent = this.quests.UnlockedAreaWithQuestAndEndOfContent;
			int num = this.config.ClampGlobalToLocalArea(this.islandId, unlockedAreaWithQuestAndEndOfContent);
			for (int i = 0; i < this.areaClouds.Length; i++)
			{
				// 审核版本只保留前5个区域
// #if REVIEW_VERSION
// 			if (i > 4)
// 			{
// 				continue;
// 			}
// #endif
				int num2 = i + 1;
				this.ShowClouds(this.map, num2, this.areaClouds[i]);
				CloudsMode cloudsMode = this.mode;
				if (cloudsMode != CloudsMode.Unlocked)
				{
					if (cloudsMode == CloudsMode.Cleared)
					{
						// 拖动建筑时的红云
						this.areaClouds[i].SetVisibility(this.progression.LastRubbleAreaCleared <= i && this.quests.UnlockedAreaWithQuestAndEndOfContent > i);
// #if REVIEW_VERSION
// 						areaClouds[i].SetVisibility(false);
// #endif
					}
				}
				else
				{
					// area上面的黑云
					this.areaClouds[i].SetVisibility(num2 > num && num2 <= this.cloudsLimit);
				}
			}
		}

		// Token: 0x040060E5 RID: 24805
		public Material material;

		// Token: 0x040060E6 RID: 24806
		public Material selectedMaterial;

		// Token: 0x040060E7 RID: 24807
		public CloudsMode mode;

		// Token: 0x040060E8 RID: 24808
		public float fadeOutTime = 1f;

		// Token: 0x040060E9 RID: 24809
		public bool centerAtTile;

		// Token: 0x040060EA RID: 24810
		public TownAreaLabel areaLabel;

		// Token: 0x040060EB RID: 24811
		public Material borderMaterial;

		// Token: 0x040060EC RID: 24812
		private AreaClouds[] areaClouds;

		// Token: 0x040060ED RID: 24813
		private int islandId;

		// Token: 0x040060EE RID: 24814
		private int cloudsLimit;

		// Token: 0x040060EF RID: 24815
		private ProgressionDataService.Service progression;

		// Token: 0x040060F0 RID: 24816
		private TownPathfinding map;

		// Token: 0x040060F1 RID: 24817
		private QuestService quests;

		// Token: 0x040060F2 RID: 24818
		private IslandAreaConfigs config;
	}
}
