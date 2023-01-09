using System.Collections;
using System.Linq;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Puzzletown.Town;
using Match3.Scripts1.Town;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Flows
{
	// Token: 0x020004C0 RID: 1216
	public class PanCameraFlow : IBlocker
	{
		// Token: 0x0600221E RID: 8734 RVA: 0x00093CCC File Offset: 0x000920CC
		public PanCameraFlow(Vector3 target, float zoomRatio = -100f, float time = 1f, bool enableZoom = false)
		{
			this.m_target = target;
			this.m_time = time;
			this.m_enableZoom = enableZoom;
			this.m_zoomRatio = ((zoomRatio >= -99f || !enableZoom) ? zoomRatio : CameraInputController.current.PreferredZoomRatio);
		}

		// Token: 0x0600221F RID: 8735 RVA: 0x00093D24 File Offset: 0x00092124
		public PanCameraFlow(PanCameraTarget targetType, float zoomRatio = -100f, float time = 1f, bool enableZoom = false)
		{
			this.m_targetType = targetType;
			this.m_time = time;
			this.m_enableZoom = enableZoom;
			this.m_zoomRatio = ((!enableZoom || zoomRatio >= -99f) ? zoomRatio : CameraInputController.current.PreferredZoomRatio);
		}

		// Token: 0x1700053C RID: 1340
		// (get) Token: 0x06002220 RID: 8736 RVA: 0x00093D7C File Offset: 0x0009217C
		public bool BlockInput
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06002221 RID: 8737 RVA: 0x00093D80 File Offset: 0x00092180
		public static PanCameraFlow CreateZoomOutFlow()
		{
			float num = Mathf.Clamp01(CameraInputController.current.ZoomRatioSetByUser);
			float num2 = Mathf.Clamp01(CameraInputController.current.CurrentZoomRatio);
			float num3 = Mathf.Abs(num - num2);
			num3 = Mathf.Clamp01(num3 * PanCameraFlow.zoomOutSmoothnessFactor);
			return new PanCameraFlow(PanCameraTarget.OnlyZoom, num, num3, true);
		}

		// Token: 0x1700053D RID: 1341
		// (get) Token: 0x06002222 RID: 8738 RVA: 0x00093DCC File Offset: 0x000921CC
		private Vector3 CurrentFocusPoint
		{
			get
			{
				BuildingInstance buildingInstance;
				if (this.TryFindRubbleWaitingForRepair(out buildingInstance))
				{
					return buildingInstance.view.FocusPosition;
				}
				int unlockedAreaWithQuestAndEndOfContent = this.quests.UnlockedAreaWithQuestAndEndOfContent;
				int currentIsland = this.gameStateService.Buildings.CurrentIsland;
				int areaId = this.config.SbsConfig.islandareaconfig.ClampGlobalToLocalArea(currentIsland, unlockedAreaWithQuestAndEndOfContent);
				return this.env.map.FindAreaFocusPoint(areaId);
			}
		}

		// Token: 0x06002223 RID: 8739 RVA: 0x00093E3C File Offset: 0x0009223C
		private bool TryFindRubbleWaitingForRepair(out BuildingInstance rubbleToRepair)
		{
			if (this.env.map.Buildings.Buildings != null)
			{
				rubbleToRepair = this.env.map.Buildings.Buildings.FirstOrDefault((BuildingInstance b) => b.blueprint.rubble_id == 0 && b.IsWaitingForRepairs(false));
				return rubbleToRepair != null;
			}
			rubbleToRepair = null;
			return false;
		}

		// Token: 0x06002224 RID: 8740 RVA: 0x00093EAC File Offset: 0x000922AC
		private Vector3 FindPreviousAreaFocusPoint()
		{
			int lastUnlockedArea = this.progressionService.LastUnlockedArea;
			int num = lastUnlockedArea - 1;
			int num2 = this.config.SbsConfig.islandareaconfig.IslandForArea(num);
			int num3 = this.config.SbsConfig.islandareaconfig.IslandForArea(lastUnlockedArea);
			int globalArea = (num2 != num3) ? lastUnlockedArea : num;
			int areaId = this.config.SbsConfig.islandareaconfig.GlobalAreaToLocalArea(globalArea);
			return this.env.map.FindAreaFocusPoint(areaId);
		}

		// Token: 0x06002225 RID: 8741 RVA: 0x00093F34 File Offset: 0x00092334
		public IEnumerator ExecuteRoutine()
		{
			yield return SceneManager.Instance.Inject(this);
			yield return ServiceLocator.Instance.Inject(this);
			while (!this.env.map || this.env.map.Buildings == null || (this.waitForMainCamera && !Camera.main))
			{
				yield return null;
			}
			Vector3 target = this.m_target;
			switch (this.m_targetType)
			{
			case PanCameraTarget.LatestUnlockedArea:
			{
				int areaId = this.config.SbsConfig.islandareaconfig.GlobalAreaToLocalArea(this.progressionService.LastUnlockedArea);
				target = this.env.map.FindAreaFocusPoint(areaId);
				break;
			}
			case PanCameraTarget.PreviousUnlockedArea:
				target = this.FindPreviousAreaFocusPoint();
				break;
			case PanCameraTarget.CurrentFocusPoint:
				target = this.CurrentFocusPoint;
				break;
			case PanCameraTarget.MostBuildings:
			{
				Vector3 a = Vector3.zero;
				int num = 0;
				if (this.env.map.Buildings.Buildings != null)
				{
					foreach (BuildingInstance buildingInstance in this.env.map.Buildings.Buildings)
					{
						int num2 = buildingInstance.blueprint.size * buildingInstance.blueprint.size;
						a += (buildingInstance.view.transform.position + buildingInstance.worldCenter) * (float)num2;
						num += num2;
					}
				}
				target = ((num >= 1) ? (a / (float)num) : this.CurrentFocusPoint);
				break;
			}
			case PanCameraTarget.OnlyZoom:
				target = CameraInputController.CameraPosition;
				break;
			}
			yield return this.env.cameraPan.PanTo(target, this.m_time, this.m_enableZoom, this.m_zoomRatio);
			yield break;
		}

		// Token: 0x04004D79 RID: 19833
		[WaitForRoot(false, false)]
		private TownEnvironmentRoot env;

		// Token: 0x04004D7A RID: 19834
		[WaitForService(true, true)]
		private QuestService quests;

		// Token: 0x04004D7B RID: 19835
		[WaitForService(true, true)]
		private ProgressionDataService.Service progressionService;

		// Token: 0x04004D7C RID: 19836
		[WaitForService(true, true)]
		private ConfigService config;

		// Token: 0x04004D7D RID: 19837
		[WaitForService(true, true)]
		private GameStateService gameStateService;

		// Token: 0x04004D7E RID: 19838
		private const float PAN_TIME = 1f;

		// Token: 0x04004D7F RID: 19839
		private readonly Vector3 m_target;

		// Token: 0x04004D80 RID: 19840
		private readonly float m_time;

		// Token: 0x04004D81 RID: 19841
		private readonly PanCameraTarget m_targetType;

		// Token: 0x04004D82 RID: 19842
		private readonly bool m_enableZoom;

		// Token: 0x04004D83 RID: 19843
		private readonly float m_zoomRatio;

		// Token: 0x04004D84 RID: 19844
		public bool waitForMainCamera = true;

		// Token: 0x04004D85 RID: 19845
		public static float zoomOutSmoothnessFactor = 1.72f;
	}
}
