using System;
using System.Collections.Generic;
using Match3.Scripts1.Puzzletown.Config;
using Match3.Scripts1.Puzzletown.Services;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Town
{
	// Token: 0x0200094A RID: 2378
	public class Rubble : MonoBehaviour
	{
		// Token: 0x060039C6 RID: 14790 RVA: 0x0011C09C File Offset: 0x0011A49C
		public void Init(ProgressionDataService.Service progression, ConfigService configService, TownPathfinding map)
		{
			this.progression = progression;
			this.map = map;
			int lastRubbleAreaCleared = progression.LastRubbleAreaCleared;
			this.islandAreaConfigs = configService.SbsConfig.islandareaconfig;
			this.localLastAreaCleared = this.islandAreaConfigs.ClampGlobalToLocalArea(map.islandId, lastRubbleAreaCleared);
			this.CreateAreas();
			progression.onCleanupChanged.AddListener(new Action<int>(this.OnCleanupChanged));
		}

		// Token: 0x060039C7 RID: 14791 RVA: 0x0011C104 File Offset: 0x0011A504
		private void OnDestroy()
		{
			if (this.progression != null)
			{
				this.progression.onCleanupChanged.RemoveListener(new Action<int>(this.OnCleanupChanged));
			}
		}

		// Token: 0x060039C8 RID: 14792 RVA: 0x0011C130 File Offset: 0x0011A530
		private void CreateAreas()
		{
			int num = this.islandAreaConfigs.NumRubbleCoveredAreasOnIsland(this.map.islandId);
			for (int i = this.localLastAreaCleared; i < num; i++)
			{
				// 审核版只有5个区域
				// #if REVIEW_VERSION
				// 	if(i >= 5)							
				// 		break;
				// #endif
				AreaClouds item = this.CreateRubble(i + 1);
				this.areaRubble.Add(item);
			}
		}

		// Token: 0x060039C9 RID: 14793 RVA: 0x0011C184 File Offset: 0x0011A584
		private AreaClouds CreateRubble(int area)
		{
			GameObject gameObject = new GameObject(string.Format("Rubble {0}", area));
			gameObject.transform.SetParent(base.transform, false);
			gameObject.layer = base.gameObject.layer;
			gameObject.AddComponent<PrepareRubbleMesh>();
			gameObject.AddComponent<MeshFilter>();
			MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
			AreaClouds areaClouds = gameObject.AddComponent<AreaClouds>();
			meshRenderer.GetComponent<MeshFilter>().sharedMesh = this.map.GetAreaInfo(area).cloudMesh;
			meshRenderer.gameObject.layer = base.gameObject.layer;
			Texture2D cloudTexture = this.map.GetAreaInfo(area).cloudTexture;
			if (cloudTexture)
			{
				MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
				materialPropertyBlock.SetTexture("_MapTexture", cloudTexture);
				meshRenderer.SetPropertyBlock(materialPropertyBlock);
			}
			meshRenderer.sharedMaterial = this.rubbleMaterial;
			areaClouds.cloudsRenderer = meshRenderer;
			areaClouds.area = area;
			return areaClouds;
		}

		// Token: 0x060039CA RID: 14794 RVA: 0x0011C278 File Offset: 0x0011A678
		private void OnCleanupChanged(int globalRubbleArea)
		{
			int localRubbleArea = this.islandAreaConfigs.GlobalAreaToLocalArea(globalRubbleArea);
			this.FadeOutTill(localRubbleArea);
		}

		// Token: 0x060039CB RID: 14795 RVA: 0x0011C29C File Offset: 0x0011A69C
		private void FadeOutTill(int localRubbleArea)
		{
			int num = Math.Min(this.areaRubble.Count, localRubbleArea - this.localLastAreaCleared);
			for (int i = 0; i < num; i++)
			{
				this.areaRubble[i].GetComponent<PrepareRubbleMesh>().Prepare(this.swipeRenderTex);
				ParticleSystem particleSystem = global::UnityEngine.Object.Instantiate<ParticleSystem>(this.dust, base.transform);
				ParticleSystem particleSystem2 = global::UnityEngine.Object.Instantiate<ParticleSystem>(this.starGlowEffect, base.transform);
				ParticleSystem.ShapeModule shape = particleSystem.shape;
				ParticleSystem.ShapeModule shape2 = particleSystem2.shape;
				shape.meshRenderer = this.areaRubble[i].gameObject.GetComponent<MeshRenderer>();
				shape2.mesh = this.areaRubble[i].gameObject.GetComponent<MeshFilter>().mesh;
				particleSystem2.Play();
				particleSystem.Play();
				// 地面的碎石/果皮的消失动画可以和shader有关，不会修复，所以直接隐藏
				areaRubble[i].gameObject.SetActive(false);
			}
			this.swipeAnimator.gameObject.SetActive(true);
			this.swipeAnimator.Play(0);
			// 动画播放完后隐藏对象
			Invoke("HideSwipeAnimator", swipeAnimator.GetCurrentAnimatorStateInfo(0).length + 0.5f);
		}

		private void HideSwipeAnimator()
		{
			swipeAnimator.gameObject.SetActive(false);
		}

		// Token: 0x040061CF RID: 25039
		public Material rubbleMaterial;

		// Token: 0x040061D0 RID: 25040
		public RenderTexture swipeRenderTex;

		// Token: 0x040061D1 RID: 25041
		public Animator swipeAnimator;

		// Token: 0x040061D2 RID: 25042
		public ParticleSystem starGlowEffect;

		// Token: 0x040061D3 RID: 25043
		public ParticleSystem dust;

		// Token: 0x040061D4 RID: 25044
		private readonly List<AreaClouds> areaRubble = new List<AreaClouds>();

		// Token: 0x040061D5 RID: 25045
		private ProgressionDataService.Service progression;

		// Token: 0x040061D6 RID: 25046
		private TownPathfinding map;

		// Token: 0x040061D7 RID: 25047
		private int localLastAreaCleared;

		// Token: 0x040061D8 RID: 25048
		private IslandAreaConfigs islandAreaConfigs;
	}
}
