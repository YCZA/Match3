using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Match3.Scripts1.Audio;
using Match3.Scripts1.Puzzletown.Flows;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Puzzletown.Town;
using Match3.Scripts1.Puzzletown.UI;
using Match3.Scripts1.Town;
using Match3.Scripts1.UnityEngine;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.Services.ErrorAnalytics;
using Match3.Scripts1.Wooga.Signals;
using Match3.Scripts2.PlayerData;
using UnityEngine;

// Token: 0x02000956 RID: 2390
namespace Match3.Scripts1
{
	public class TownSceneLoader : MonoBehaviour
	{
		// Token: 0x170008E5 RID: 2277
		// (get) Token: 0x06003A2C RID: 14892 RVA: 0x0011F5FF File Offset: 0x0011D9FF
		// (set) Token: 0x06003A2D RID: 14893 RVA: 0x0011F607 File Offset: 0x0011DA07
		public BuildingServices buildingServices { get; protected set; }

		// Token: 0x170008E6 RID: 2278
		// (get) Token: 0x06003A2E RID: 14894 RVA: 0x0011F610 File Offset: 0x0011DA10
		public TownPathfinding map
		{
			get
			{
				return this.townEnv.map;
			}
		}

		// Token: 0x06003A2F RID: 14895 RVA: 0x0011F620 File Offset: 0x0011DA20
		private IEnumerable<BuildingInstance.PersistentData> PrePopulateArea(int area, bool attemptingRepair)
		{
			List<BuildingInstance.PersistentData> list = new List<BuildingInstance.PersistentData>();
			this.RefreshForeshadowing(area, 0);
			IEnumerable<StartBuilding> startBuildings = this.buildingServices.StartBuildings;
			foreach (StartBuilding startBuilding in startBuildings)
			{
				if (startBuilding.area == area)
				{
					BuildingConfig config = this.buildingServices.BuildingConfigs.GetConfig(startBuilding.building_id);
					if (config == null)
					{
						Log.Warning("Unable to find building config for building", string.Format("Unable to find building config for building {0}", startBuilding.building_id), null);
					}
					else
					{
						bool flag = config.type == 64 && this.gameStateService.Buildings.CountOfBuilding(startBuilding.building_id) > 0;
						bool flag2 = config.type != 64;
						if (!flag && (!flag2 || !attemptingRepair))
						{
							IntVector2 position = new IntVector2(startBuilding.pos_x, startBuilding.pos_y);
							int size = config.size;
							IEnumerable<BuildingInstance> buildings = this.buildingServices.BuildingsController.Buildings;
							bool flag3 = BuildingLocation.BlockedByAnotherBuilding(position, size, null, buildings);
							if (flag3)
							{
								Log.Warning("Unable to find a place for start building", string.Format("Unable to place start building {0}, area {1}. Overriding placement: true", startBuilding.building_id, startBuilding.area), null);
							}
							BuildingInstance.PersistentData persistentData = BuildingInstance.PersistentData.CreateFromConfig(startBuilding);
							if (this.IsDecoSetRepaired(persistentData.DecoSet))
							{
								persistentData.SetTimer(BuildingTimer.Repair, BuildingInstance.UnixStamp - 1);
							}
							this.gameStateService.Buildings.RegisterBuildingData(persistentData);
							list.Add(persistentData);
						}
					}
				}
			}
			if (attemptingRepair && list.Count > 0)
			{
				Log.Warning("Repairing buildings for area", string.Format("Repairing {0} buildings for area {1}", list.Count, area), null);
			}
			if (!attemptingRepair)
			{
				this.gameStateService.Buildings.SetAreaDeployed(area);
			}
			return list;
		}

		// Token: 0x06003A30 RID: 14896 RVA: 0x0011F834 File Offset: 0x0011DC34
		public void CreateAreaBuildings(int area, bool attemptingRepair)
		{
			IEnumerable<BuildingInstance.PersistentData> buildings = this.PrePopulateArea(area, attemptingRepair);
			TownLoader.LoadTownData(this.buildingServices, buildings, -1);
		}

		// Token: 0x06003A31 RID: 14897 RVA: 0x0011F858 File Offset: 0x0011DC58
		public void SaveEditMode()
		{
			IEnumerable<BuildingInstance.PersistentData> buildings = from b in this.buildingServices.BuildingsController.Buildings
				select b.sv;
			this.editorMode.Save(buildings);
		}

		// eli key point 生成家园
		public void Init(GameStateService stateService, ConfigService configService, BuildingResourceServiceRoot resourceService, TownOverheadUiRoot buildingsUi, QuestService questService, ILocalizationService loc, ProgressionDataService.Service progressionService, TownEnvironmentRoot townEnv, TrackingService tracking, TownUiRoot townUiRoot, AudioService audioService, SeasonService seasonService)
		{
			this.progressionService = progressionService;
			this.townEnv = townEnv;
			this.questService = questService;
			this.tracking = tracking;
			this.townUiRoot = townUiRoot;
			this.seasonService = seasonService;
			if (TownCheatsRoot.EditMode)
			{
				this.editorMode = new TownEditorMode();
			}
			this.buildingResourceService = resourceService;
			this.audioService = audioService;
			this.gameStateService = stateService;
			this.configService = configService;
			this.buildingsUi = buildingsUi;
			this.localizationService = loc;
			if (TownSceneLoader.requiredBuildingRatio < 0f)
			{
				TownSceneLoader.requiredBuildingRatio = PlayerPrefs.GetFloat(TownSceneLoader.REQUIRED_BUILDING_RATIO_PP_KEY, TownSceneLoader.DEFAULT_REQUIRED_BUILDING_RATIO);
			}
			if (this.ResetBuildingsOnLaunch)
			{
				this.gameStateService.Buildings.Reset();
			}

			this.InitBuildingServices();
			this.InitViewFactory();
			this.LoadMap();
			this.AddListeners();
			this.buildingServices.BuildingsController.Buildings.ForEach(new Action<BuildingInstance>(this.ProcessBuildingReferences));
			this.CountBuildingsToLoadBehindLoadingScreen(questService.UnlockedAreaWithQuestAndEndOfContent);
			this.LoadFromGameState();
			this.PopulateRubble();
			if (TownCheatsRoot.EditMode)
			{
				foreach (BuildingConfig buildingConfig in this.buildingServices.BuildingConfigs.buildings)
				{
					buildingConfig.costs = new MaterialAmount[]
					{
						new MaterialAmount("coins", 1, MaterialAmountUsage.Undefined, 0)
					};
					buildingConfig.unlock_level = 1;
					buildingConfig.chapter_id = 1;
					buildingConfig.harmony = 1;
					buildingConfig.max_number = 99999;
					buildingConfig.harvest_timer = 0;
				}
			}
			this.CheckForCheatModeCheapBuildings();
			this.SetupCameraPanFlow();
			this.buildingServices.BuildingsController.RefreshMap();
			if (this.gameStateService.IsMyOwnState)
			{
				this.CheckForCorruptedBuildings();
			}
			this.RefreshForeshadowing(questService.UnlockedAreaWithQuestAndEndOfContent, (int)((float)this.foreshadowedBuildingCount * TownSceneLoader.requiredBuildingRatio));
			this.StartLoadingTimeoutRoutine();
		}

		// Token: 0x06003A33 RID: 14899 RVA: 0x0011FA85 File Offset: 0x0011DE85
		private void StartLoadingTimeoutRoutine()
		{
			this.loadingTimeoutRoutine = WooroutineRunner.StartCoroutine(this.LoadingTimeoutRoutine(), null);
		}

		// Token: 0x06003A34 RID: 14900 RVA: 0x0011FA9C File Offset: 0x0011DE9C
		private IEnumerator LoadingTimeoutRoutine()
		{
			yield return new WaitForSeconds(12f);
			if (!this.onRequiredNumberOfBuildingsLoaded.WasDispatched)
			{
				this.onRequiredNumberOfBuildingsLoaded.Dispatch();
			}
			this.loadingTimeoutRoutine = null;
			yield break;
		}

		// Token: 0x06003A35 RID: 14901 RVA: 0x0011FAB8 File Offset: 0x0011DEB8
		private void SetupCameraPanFlow()
		{
			if (this.gameStateService.isInteractable)
			{
				if (TownOptionsRoot.s_cameraPosition != null)
				{
					PanCameraFlow panCameraFlow = new PanCameraFlow(TownOptionsRoot.s_cameraPosition.Value, -100f, 0f, false);
					panCameraFlow.waitForMainCamera = false;
					BlockerManager.global.Append(panCameraFlow);
					TownOptionsRoot.s_cameraPosition = null;
				}
				else
				{
					PanCameraFlow panCameraFlow2 = new PanCameraFlow(PanCameraTarget.CurrentFocusPoint, -100f, 0f, false);
					panCameraFlow2.waitForMainCamera = false;
					BlockerManager.global.Append(panCameraFlow2);
				}
			}
			else
			{
				PanCameraFlow panCameraFlow3 = new PanCameraFlow(PanCameraTarget.MostBuildings, -100f, 0f, false);
				panCameraFlow3.waitForMainCamera = false;
				BlockerManager.global.Append(panCameraFlow3);
			}
		}

		// Token: 0x06003A36 RID: 14902 RVA: 0x0011FB74 File Offset: 0x0011DF74
		private void CheckForCheatModeCheapBuildings()
		{
			if (TownCheatsRoot.EditMode)
			{
				foreach (BuildingConfig buildingConfig in this.buildingServices.BuildingConfigs.buildings)
				{
					buildingConfig.costs = new MaterialAmount[]
					{
						new MaterialAmount("coins", 1, MaterialAmountUsage.Undefined, 0)
					};
					buildingConfig.unlock_level = 1;
					buildingConfig.chapter_id = 1;
					buildingConfig.harmony = 1;
					buildingConfig.max_number = 99999;
					buildingConfig.harvest_timer = 0;
				}
			}
		}

		// Token: 0x06003A37 RID: 14903 RVA: 0x0011FC00 File Offset: 0x0011E000
		private void AddListeners()
		{
			this.buildingServices.BuildingsController.onBuildingSelected.AddListener(new Action<BuildingInstance, bool>(this.HandleBuildingSelected));
			this.buildingServices.BuildingsController.onBuildingsRefreshed.AddListener(new Action(this.HandleBuildingsRefreshed));
			this.buildingServices.BuildingsController.onBuildingCreated.AddListener(new Action<BuildingInstance>(this.ProcessBuildingReferences));
			this.buildingServices.BuildingsController.onBuildingRepaired.AddListener(new Action<BuildingInstance>(this.HandleBuildingRepaired));
			this.buildingServices.BuildingsController.onBuildingDestroyed.AddListener(new Action<BuildingInstance>(this.HandleBuildingDestroyed));
			this.buildingServices.BuildingsController.onBuildingComplete.AddListener(new Action<BuildingInstance>(this.HandleBuildingCompleted));
			this.progressionService.onCleanupChanged.AddListener(new Action<int>(this.OnCleanupChanged));
		}

		// Token: 0x06003A38 RID: 14904 RVA: 0x0011FCF0 File Offset: 0x0011E0F0
		private void RemoveListeners()
		{
			if (this.gameStateService != null)
			{
				this.progressionService.onCleanupChanged.RemoveListener(new Action<int>(this.OnCleanupChanged));
			}
			if (this.buildingServices != null && this.buildingServices.BuildingsController != null)
			{
				BuildingsController buildingsController = this.buildingServices.BuildingsController;
				buildingsController.onBuildingSelected.RemoveListener(new Action<BuildingInstance, bool>(this.HandleBuildingSelected));
				buildingsController.onBuildingsRefreshed.RemoveListener(new Action(this.HandleBuildingsRefreshed));
				buildingsController.onBuildingCreated.RemoveListener(new Action<BuildingInstance>(this.ProcessBuildingReferences));
				buildingsController.onBuildingRepaired.RemoveListener(new Action<BuildingInstance>(this.HandleBuildingRepaired));
				buildingsController.onBuildingDestroyed.RemoveListener(new Action<BuildingInstance>(this.HandleBuildingDestroyed));
				buildingsController.onBuildingComplete.RemoveListener(new Action<BuildingInstance>(this.HandleBuildingCompleted));
			}
		}

		// Token: 0x06003A39 RID: 14905 RVA: 0x0011FDD5 File Offset: 0x0011E1D5
		private void InitBuildingServices()
		{
			this.buildingServices = new BuildingServices();
			this.buildingServices.Init(this.configService, this.buildingResourceService, this.gameStateService);
		}

		// Token: 0x06003A3A RID: 14906 RVA: 0x0011FDFF File Offset: 0x0011E1FF
		private void InitViewFactory()
		{
			this.buildingViewFactory.Init(this.buildingServices, this.configService, this.buildingsUi, this.townUiRoot.BottomPanel);
		}

		// Token: 0x06003A3B RID: 14907 RVA: 0x0011FE2C File Offset: 0x0011E22C
		private void CheckForCorruptedBuildings()
		{
			List<string> list = new List<string>();
			foreach (BuildingInstance buildingInstance in this.buildingServices.BuildingsController.Buildings)
			{
				if (!buildingInstance.sv.IsRepaired && this.IsDecoSetRepaired(buildingInstance.sv.DecoSet))
				{
					WoogaDebug.Log(new object[]
					{
						"should have been repaired",
						buildingInstance.sv.blueprintName
					});
					buildingInstance.Repair(false);
					list.Add(buildingInstance.sv.blueprintName);
				}
			}
			if (list.Count > 0)
			{
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				dictionary["buildings"] = list;
				Log.Warning("CorruptedBuildings", "found some destroyed buildings which should have been repaired", dictionary);
			}
		}

		// Token: 0x06003A3C RID: 14908 RVA: 0x0011FF1C File Offset: 0x0011E31C
		private bool IsDecoSetRepaired(string set)
		{
			foreach (QuestData questData in this.configService.SbsConfig.questconfig.quests)
			{
				foreach (QuestTaskData questTaskData in questData.Tasks)
				{
					if (questTaskData.type == QuestTaskType.collect_and_repair && questTaskData.action_target == set)
					{
						return this.questService.IsCollected(questData.id) || (this.questService.questManager.CurrentQuestData != null && this.questService.questManager.CurrentQuestData.id == questData.id && this.questService.questManager.GetTaskCollected(this.questService.questManager.CurrentQuestProgress, questTaskData.index));
					}
				}
			}
			return false;
		}

		// Token: 0x06003A3D RID: 14909 RVA: 0x00120044 File Offset: 0x0011E444
		private void PopulateRubble()
		{
			foreach (BuildingInstance buildingInstance in this.buildingServices.BuildingsController.Buildings)
			{
				if (buildingInstance.blueprint.IsRubble())
				{
					int globalArea = this.configService.SbsConfig.islandareaconfig.FirstAreaCoveredByThisRubble(buildingInstance.blueprint.rubble_id);
					int areaId = this.configService.SbsConfig.islandareaconfig.GlobalAreaToLocalArea(globalArea);
					Vector3 vector = this.map.FindAreaFocusPoint(areaId);
					buildingInstance.position = IntVector2.ProjectToGridXZ(vector);
					buildingInstance.view.transform.position = vector;
				}
			}
		}

		// Token: 0x06003A3E RID: 14910 RVA: 0x0012011C File Offset: 0x0011E51C
		private void HandleBuildingRepaired(BuildingInstance building)
		{
			if (building.blueprint.IsRubble())
			{
				int val = this.configService.SbsConfig.islandareaconfig.LastAreaCoveredByThisRubble(building.blueprint.rubble_id);
				this.progressionService.LastRubbleAreaCleared = Math.Max(val, this.progressionService.LastRubbleAreaCleared);
			}
		}

		// Token: 0x06003A3F RID: 14911 RVA: 0x00120176 File Offset: 0x0011E576
		private void HandleBuildingDestroyed(BuildingInstance building)
		{
			this.buildingServices.DataService.RemoveBuilding(building);
		}

		// Token: 0x06003A40 RID: 14912 RVA: 0x00120189 File Offset: 0x0011E589
		private void HandleBuildingCompleted(BuildingInstance newBuilding)
		{
			this.questService.DecoLikeTask.OnBuildingBuilt(newBuilding.blueprint);
		}

		// Token: 0x06003A41 RID: 14913 RVA: 0x001201A1 File Offset: 0x0011E5A1
		private void OnDestroy()
		{
			this.RemoveListeners();
			if (this.loadingTimeoutRoutine != null)
			{
				WooroutineRunner.Stop(this.loadingTimeoutRoutine);
			}
		}

		// Token: 0x06003A42 RID: 14914 RVA: 0x001201BF File Offset: 0x0011E5BF
		private void HandleBuildingSelected(BuildingInstance building, bool selected)
		{
			this.allBuildingsToLoadBeforeShadowRenderCount = 0;
			this.HandleBuildingsRefreshed();
		}

		// Token: 0x06003A43 RID: 14915 RVA: 0x001201CE File Offset: 0x0011E5CE
		private void HandleBuildingsRefreshed()
		{
			this.CheckIfWaitingForBuildingsToLoad();
			if (this.allBuildingsToLoadBeforeShadowRenderCount <= 0)
			{
				WooroutineRunner.StartCoroutine(this.RenderShadows(), null);
			}
		}

		// Token: 0x06003A44 RID: 14916 RVA: 0x001201F0 File Offset: 0x0011E5F0
		private void CheckIfWaitingForBuildingsToLoad()
		{
			if (this.buildingsToLoadBehindLoadingScreenCount >= 1)
			{
				this.buildingsToLoadBehindLoadingScreenCount--;
			}
			if (this.allBuildingsToLoadBeforeShadowRenderCount >= 1)
			{
				this.allBuildingsToLoadBeforeShadowRenderCount--;
			}
			if (this.buildingsToLoadBehindLoadingScreenCount <= 0 && !this.onRequiredNumberOfBuildingsLoaded.WasDispatched)
			{
				this.onRequiredNumberOfBuildingsLoaded.Dispatch();
			}
		}

		// Token: 0x06003A45 RID: 14917 RVA: 0x00120258 File Offset: 0x0011E658
		private IEnumerator RenderShadows()
		{
			yield return null;
			if (this.townEnv != null && this.townEnv.shadowsCamera != null)
			{
				this.townEnv.shadowsCamera.Render();
			}
			yield break;
		}

		// Token: 0x06003A46 RID: 14918 RVA: 0x00120274 File Offset: 0x0011E674
		public void ProcessBuildingReferences(BuildingInstance building)
		{
			if (building.sv.IsSaved)
			{
				this.buildingServices.DataService.RegisterBuilding(building);
			}
			building.controller = this.buildingServices.BuildingsController;
			building.mapDataProvider = this.map;
			building.resources = this.gameStateService.Resources;
			building.localization = this.localizationService;
			building.view.positionHelper = this.positionHelper;
			building.quests = this.questService;
			building.tracking = this.tracking;
			building.seasonService = this.seasonService;
			building.view.InitCalloutController();
		}

		// Token: 0x06003A47 RID: 14919 RVA: 0x0012031C File Offset: 0x0011E71C
		private void LoadFromGameState()
		{
			if (TownCheatsRoot.EditMode)
			{
				return;
			}
			IntVector2 cameraFocusAsGridPosition = this.GetCameraFocusAsGridPosition();
			IOrderedEnumerable<BuildingInstance.PersistentData> buildings = from buildingPersistentData in this.gameStateService.Buildings.BuildingsData.Buildings
				orderby IntVector2.SimpleDistance(cameraFocusAsGridPosition, buildingPersistentData.position)
				select buildingPersistentData;
			int countToLoadWithoutSlicing = (int)((float)this.normalBuildingCount * TownSceneLoader.requiredBuildingRatio);
			TownLoader.LoadTownData(this.buildingServices, buildings, countToLoadWithoutSlicing);
		}

		// Token: 0x06003A48 RID: 14920 RVA: 0x0012038C File Offset: 0x0011E78C
		private void CountBuildingsToLoadBehindLoadingScreen(int lastUnlockedArea)
		{
			if (!this.foreshadowedBuildings.IsInitialized)
			{
				this.foreshadowedBuildings.Init(this.configService, this.buildingServices);
			}
			this.normalBuildingCount = this.gameStateService.Buildings.BuildingsData.Buildings.Count;
			this.foreshadowedBuildingCount = this.foreshadowedBuildings.CountForeshadowedBuildingsToCreate(lastUnlockedArea);
			this.allBuildingsToLoadBeforeShadowRenderCount = this.foreshadowedBuildingCount + this.normalBuildingCount;
			this.buildingsToLoadBehindLoadingScreenCount = (int)((float)this.allBuildingsToLoadBeforeShadowRenderCount * Mathf.Clamp01(TownSceneLoader.requiredBuildingRatio));
			this.CheckIfWaitingForBuildingsToLoad();
		}

		// Token: 0x06003A49 RID: 14921 RVA: 0x00120424 File Offset: 0x0011E824
		private IntVector2 GetCameraFocusAsGridPosition()
		{
			if (!this.gameStateService.isInteractable)
			{
				return this.GetCameraFocusForFriendVisit();
			}
			Vector3 worldPosition;
			if (TownOptionsRoot.s_cameraPosition != null)
			{
				worldPosition = TownOptionsRoot.s_cameraPosition.Value;
			}
			else
			{
				int unlockedAreaWithQuestAndEndOfContent = this.questService.UnlockedAreaWithQuestAndEndOfContent;
				worldPosition = this.map.FindAreaFocusPoint(unlockedAreaWithQuestAndEndOfContent);
			}
			return IntVector2.ProjectToGridXZ(worldPosition);
		}

		// Token: 0x06003A4A RID: 14922 RVA: 0x00120488 File Offset: 0x0011E888
		private IntVector2 GetCameraFocusForFriendVisit()
		{
			IntVector2 a = IntVector2.Zero;
			int num = 0;
			foreach (BuildingInstance.PersistentData persistentData in this.gameStateService.Buildings.BuildingsData.Buildings)
			{
				a += persistentData.position;
				num++;
			}
			return new IntVector2(a.x / num, a.y / num);
		}

		// Token: 0x06003A4B RID: 14923 RVA: 0x0012051C File Offset: 0x0011E91C
		private void OnCleanupChanged(int cleanup)
		{
			this.audioService.PlaySFX(AudioId.RubbleCleanup, false, false, false);
		}

		private void LoadMap()
		{
			// eli key point 载入家园地图
			this.townEnv.map.Init(this.questService, this.configService, this.progressionService, this.buildingServices.BuildingsController, this.townUiRoot, this.buildingsUi);
			foreach (Clouds clouds in base.GetComponentsInChildren<Clouds>())
			{
				clouds.Init(this.progressionService, this.configService, this.map, this.questService, this.buildingsUi);
			}
			this.ExecuteOnChild(delegate(Rubble rubble)
			{
				// eli key point ???地面上的碎石初始化？
				// #if !REVIEW_VERSION
				rubble.Init(this.progressionService, this.configService, this.map);
				// #endif
			});
		}

		// Token: 0x06003A4D RID: 14925 RVA: 0x001205CF File Offset: 0x0011E9CF
		private void RefreshForeshadowing(int lastUnlockedArea, int createWithoutSlicingCount = 0)
		{
			if (!this.foreshadowedBuildings.IsInitialized)
			{
				this.foreshadowedBuildings.Init(this.configService, this.buildingServices);
			}
			this.foreshadowedBuildings.RefreshForeshadowing(lastUnlockedArea, createWithoutSlicingCount);
		}

		// Token: 0x06003A4E RID: 14926 RVA: 0x00120605 File Offset: 0x0011EA05
		public void PlaceBuilding(BuildingShopView.BuildingBuildRequest request)
		{
			this.TryPlaceBuilding(request, true);
		}

		// Token: 0x06003A4F RID: 14927 RVA: 0x00120610 File Offset: 0x0011EA10
		public bool TryPlaceBuilding(BuildingShopView.BuildingBuildRequest request, bool showErrorPopupOnFailure = true)
		{
			IntVector2 position;
			if (!BuildingPlacementHelper.TryFindPlaceForBuilding(request, this.map, out position))
			{
				if (showErrorPopupOnFailure)
				{
					this.ShowNoPlaceForBuildingPopup();
				}
				return false;
			}
			if (request.shouldPanCamera)
			{
				Vector3 vector = new Vector3((float)(position.x + request.Config.size), 0f, (float)(position.y + request.Config.size));
				BlockerManager global = BlockerManager.global;
				Vector3 target = vector;
				bool shouldZoomIn = request.shouldZoomIn;
				float zoomInRatio = request.zoomInRatio;
				global.Append(new PanCameraFlow(target, zoomInRatio, 1f, shouldZoomIn));
			}
			BuildingInstance.PersistentData persistentData = new BuildingInstance.PersistentData
			{
				blueprintName = request.Config.name,
				position = position
			};
			persistentData.SetTimer(BuildingTimer.Repair, BuildingInstance.UnixStamp);
			BuildingInstance buildingInstance = TownLoader.Instantiate(persistentData, this.buildingServices.BuildingsController, this.buildingServices.BuildingConfigs, false);
			buildingInstance.isFree = request.isFree;
			buildingInstance.isFromStorage = request.isFromStorage;
			BuildingLocation.Selected = buildingInstance;
			return true;
		}

		// Token: 0x06003A50 RID: 14928 RVA: 0x00120722 File Offset: 0x0011EB22
		private void ShowNoPlaceForBuildingPopup()
		{
			PopupDialogRoot.ShowOkDialog(this.localizationService.GetText("ui.shared.building_no_space_title", new LocaParam[0]), this.localizationService.GetText("ui.shared.building_no_space_text", new LocaParam[0]), null, null);
		}

		// Token: 0x04006234 RID: 25140
		[SerializeField]
		private readonly bool ResetBuildingsOnLaunch;

		// Token: 0x04006235 RID: 25141
		[NonSerialized]
		public BuildingResourceServiceRoot buildingResourceService;

		// Token: 0x04006236 RID: 25142
		private TownOverheadUiRoot buildingsUi;

		// Token: 0x04006237 RID: 25143
		[SerializeField]
		private BuildingViewFactory buildingViewFactory;

		// Token: 0x04006238 RID: 25144
		private ConfigService configService;

		// Token: 0x04006239 RID: 25145
		private GameStateService gameStateService;

		// Token: 0x0400623A RID: 25146
		private ILocalizationService localizationService;

		// Token: 0x0400623B RID: 25147
		private ProgressionDataService.Service progressionService;

		// Token: 0x0400623C RID: 25148
		private AudioService audioService;

		// Token: 0x0400623D RID: 25149
		private TownUiRoot townUiRoot;

		// Token: 0x0400623E RID: 25150
		private QuestService questService;

		// Token: 0x0400623F RID: 25151
		private SeasonService seasonService;

		// Token: 0x04006240 RID: 25152
		[NonSerialized]
		private TownEnvironmentRoot townEnv;

		// Token: 0x04006241 RID: 25153
		public PositionHelper positionHelper;

		// Token: 0x04006242 RID: 25154
		public ForeshadowedBuildingContainer foreshadowedBuildings;

		// Token: 0x04006244 RID: 25156
		private TrackingService tracking;

		// Token: 0x04006245 RID: 25157
		public AwaitSignal onRequiredNumberOfBuildingsLoaded = new AwaitSignal();

		// Token: 0x04006246 RID: 25158
		public static string REQUIRED_BUILDING_RATIO_PP_KEY = "REQ_BLDG_RATIO";

		// Token: 0x04006247 RID: 25159
		public static float DEFAULT_REQUIRED_BUILDING_RATIO;

		// Token: 0x04006248 RID: 25160
		public static float requiredBuildingRatio = -1f;

		// Token: 0x04006249 RID: 25161
		public const float BUILDING_LOADING_TIMEOUT_SECS = 12f;

		// Token: 0x0400624A RID: 25162
		private int buildingsToLoadBehindLoadingScreenCount;

		// Token: 0x0400624B RID: 25163
		private int allBuildingsToLoadBeforeShadowRenderCount;

		// Token: 0x0400624C RID: 25164
		private int foreshadowedBuildingCount;

		// Token: 0x0400624D RID: 25165
		private int normalBuildingCount;

		// Token: 0x0400624E RID: 25166
		private Coroutine loadingTimeoutRoutine;

		// Token: 0x0400624F RID: 25167
		private TownEditorMode editorMode;
	}
}
