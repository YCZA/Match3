using System;
using System.Collections;
using System.Collections.Generic;
using Match3.Scripts1.Puzzletown.Flows;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Puzzletown.Tutorials;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.Signals;
using Match3.Scripts2.Env;
using UnityEngine;

namespace Match3.Scripts1.Town
{
	// Token: 0x020008EE RID: 2286
	public class BuildingInstance : IHandler<BuildingOperation>
	{
		// Token: 0x0600377B RID: 14203 RVA: 0x0010E358 File Offset: 0x0010C758
		public BuildingInstance(BuildingConfig config)
		{
			this.blueprint = config;
			this.sv = new BuildingInstance.PersistentData
			{
				blueprintName = ((this.blueprint == null) ? string.Empty : this.blueprint.name)
			};
			this.SetTrackingDetail(false);
			this.onSelected.AddListener(new Action<BuildingInstance, bool>(this.HandleSelected));
		}

		// Token: 0x0600377C RID: 14204 RVA: 0x0010E40C File Offset: 0x0010C80C
		public BuildingInstance(BuildingConfig config, BuildingInstance.PersistentData data)
		{
			this.blueprint = config;
			this.sv = data;
			this.isDecoTrophy = (this.blueprint != null && (this.blueprint.type & 128) != 0);
			bool flag = config.challenge_set > 0;
			if (flag)
			{
				this.cancelBatchMode = true;
			}
			this.SetTrackingDetail(flag);
			this.onSelected.AddListener(new Action<BuildingInstance, bool>(this.HandleSelected));
		}

		// Token: 0x17000884 RID: 2180
		// (get) Token: 0x0600377D RID: 14205 RVA: 0x0010E4D8 File Offset: 0x0010C8D8
		public BuildingState State
		{
			get
			{
				if (!this.sv.IsSaved)
				{
					return BuildingState.NotSaved;
				}
				if (!this.sv.IsRepaired)
				{
					return BuildingState.Destroyed;
				}
				if (!this.sv.IsUnveiled)
				{
					return BuildingState.ReadyToPlace;
				}
				MaterialAmount materialAmount = this.CalculateHarvestAmount();
				if (materialAmount.amount >= this.blueprint.harvest_minimum && materialAmount.amount > 0)
				{
					return BuildingState.Harvestable;
				}
				return BuildingState.Active;
			}
		}

		// Token: 0x17000885 RID: 2181
		// (get) Token: 0x0600377E RID: 14206 RVA: 0x0010E549 File Offset: 0x0010C949
		// (set) Token: 0x0600377F RID: 14207 RVA: 0x0010E551 File Offset: 0x0010C951
		public BuildingInstance.PersistentData sv { get; private set; }

		// Token: 0x17000886 RID: 2182
		// (get) Token: 0x06003780 RID: 14208 RVA: 0x0010E55A File Offset: 0x0010C95A
		// (set) Token: 0x06003781 RID: 14209 RVA: 0x0010E567 File Offset: 0x0010C967
		public IntVector2 position
		{
			get
			{
				return this.sv.position;
			}
			set
			{
				this.sv.position = value;
				this.controller.RefreshMap();
			}
		}

		// Token: 0x17000887 RID: 2183
		// (get) Token: 0x06003782 RID: 14210 RVA: 0x0010E580 File Offset: 0x0010C980
		public Vector3 worldCenter
		{
			get
			{
				return new Vector3(0.5f, 0f, 0.5f) * (float)this.blueprint.size;
			}
		}

		// Token: 0x17000888 RID: 2184
		// (get) Token: 0x06003783 RID: 14211 RVA: 0x0010E5A8 File Offset: 0x0010C9A8
		public Vector3 worldCenterTop
		{
			get
			{
				Vector3 a = new Vector3(0.28f, 1f, 0.4f);
				BuildingAssetLoader componentInChildren = this.view.GetComponentInChildren<BuildingAssetLoader>();
				if (componentInChildren && componentInChildren.BuildingAsset)
				{
					MeshFilter componentInChildren2 = componentInChildren.BuildingAsset.GetComponentInChildren<MeshFilter>();
					if (componentInChildren2 && componentInChildren2.sharedMesh && componentInChildren2.sharedMesh.bounds.max.y > 0f)
					{
						return this.worldCenter + a * componentInChildren2.sharedMesh.bounds.max.y * (float)this.blueprint.size;
					}
				}
				return this.worldCenter + a * (float)this.blueprint.size * 0.75f;
			}
		}

		// Token: 0x17000889 RID: 2185
		// (get) Token: 0x06003784 RID: 14212 RVA: 0x0010E6A6 File Offset: 0x0010CAA6
		public IntRect Area
		{
			get
			{
				return new IntRect(this.position, IntVector2.One * this.blueprint.size);
			}
		}

		// Token: 0x1700088A RID: 2186
		// (get) Token: 0x06003785 RID: 14213 RVA: 0x0010E6C8 File Offset: 0x0010CAC8
		public static int UnixStamp
		{
			get
			{
				return (int)BuildingInstance.UnixStampf;
			}
		}

		// Token: 0x1700088B RID: 2187
		// (get) Token: 0x06003786 RID: 14214 RVA: 0x0010E6D0 File Offset: 0x0010CAD0
		public static double UnixStampf
		{
			get
			{
				return DateTime.UtcNow.Subtract(BuildingInstance.UNIX_START_TIME).TotalSeconds;
			}
		}

		// Token: 0x06003787 RID: 14215 RVA: 0x0010E6F8 File Offset: 0x0010CAF8
		public bool IsWaitingForRepairs(bool includeUnveil = false)
		{
			if (includeUnveil)
			{
				if (this.sv.IsRepaired && this.State != BuildingState.ReadyToPlace)
				{
					return false;
				}
			}
			else if (this.State != BuildingState.Destroyed)
			{
				return false;
			}
			if (this.blueprint.rubble_id == 1)
			{
				return true;
			}
			IEnumerable<QuestUIData> enumerable = this.quests.questManager.Filter(QuestProgress.Status.started);
			foreach (QuestUIData questUIData in enumerable)
			{
				foreach (QuestTaskData questTaskData in questUIData.data.Tasks)
				{
					if (questTaskData.type == QuestTaskType.collect_and_repair && (questTaskData.item == this.blueprint.name || questTaskData.action_target == this.sv.DecoSet))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06003788 RID: 14216 RVA: 0x0010E83C File Offset: 0x0010CC3C
		public void Handle(BuildingOperation op)
		{
			if (op != BuildingOperation.Confirm)
			{
				if (op != BuildingOperation.Cancel)
				{
					if (op != BuildingOperation.Store)
					{
						if (op == BuildingOperation.Repair)
						{
							this.Repair(false);
						}
					}
					else
					{
						// eli key point 收起建筑
						TutorialRunner tutorialRunner = global::UnityEngine.Object.FindObjectOfType<TutorialRunner>();
						if (tutorialRunner)
						{
							tutorialRunner.onMarkerEnabled.Dispatch("Storage");
						}
						this.tracking.TrackDecoItem(this.blueprint, "store", 0, string.Empty);
						this.controller.StoreBuilding(this.blueprint, 1);
						BuildingConfig val = BuildingLocation.Selected.blueprint;
						Vector3 val2 = CameraInputController.ScreenPosition(BuildingLocation.Selected.position);
						this.controller.onBuildingStored.Dispatch(val, val2);
						this.controller.DestroyBuilding(BuildingLocation.Selected);
						this.controller.RefreshMap();
						this.CloseBottomPanel();
					}
				}
				else
				{
					this.view.Position = this.position;
					CameraInputController.current.RestorePosition();
					this.CloseBottomPanel();
				}
			}
			else
			{
				if (this.isFromStorage)
				{
					this.trackingDetail = "storage";
					this.isFree = true;
					int storedBuildingsCount = this.controller.GetStoredBuildingsCount(this.blueprint);
					this.cancelBatchMode = (this.blueprint.batch_mode == 0 || storedBuildingsCount <= 1);
				}
				this.position = this.view.Position;
				if (!this.sv.IsSaved)
				{
					WooroutineRunner.StartCoroutine(this.TryPurchaseRoutine(), null);
				}
				else
				{
					BuildingLocation.Selected = null;
				}
			}
		}

		// Token: 0x06003789 RID: 14217 RVA: 0x0010E9D0 File Offset: 0x0010CDD0
		public void OnClick(BuildingInstance data)
		{
			if (this.CheckForEditorCheat())
			{
				return;
			}
			BuildingState state = this.State;
			if (state != BuildingState.ReadyToPlace)
			{
				if (state != BuildingState.Harvestable)
				{
					this.onClick.Dispatch(this);
				}
				else
				{
					this.Harvest();
				}
			}
			else
			{
				this.PlaceBuilding();
			}
		}

		// Token: 0x0600378A RID: 14218 RVA: 0x0010EA2C File Offset: 0x0010CE2C
		private bool CheckForEditorCheat()
		{
			if (global::UnityEngine.Input.GetKey(KeyCode.LeftShift) && GameEnvironment.CurrentEnvironment != GameEnvironment.Environment.PRODUCTION)
			{
				if (!this.sv.IsRepaired)
				{
					this.Repair(false);
				}
				else
				{
					this.SetTimer(BuildingTimer.Repair, -BuildingInstance.UnixStamp);
				}
				this.view.SwitchUiMode(this, BuildingUiMode.None);
				return true;
			}
			return false;
		}

		// Token: 0x0600378B RID: 14219 RVA: 0x0010EA8C File Offset: 0x0010CE8C
		public void RepairInModel()
		{
			this.sv.SetTimer(BuildingTimer.Repair, BuildingInstance.UnixStamp);
		}

		// Token: 0x0600378C RID: 14220 RVA: 0x0010EAA0 File Offset: 0x0010CEA0
		public void Repair(bool forceRepair = false)
		{
			if (this.sv.IsRepaired && !forceRepair)
			{
				return;
			}
			if (string.IsNullOrEmpty(this.sv.DecoSet))
			{
				this.tracking.TrackDecoItem(this.blueprint, "repair", 0, string.Empty);
			}
			this.SetTimer(BuildingTimer.Repair, 0);
			// eli key point 建筑被修复时清理地面
			this.controller.onBuildingRepaired.Dispatch(this);
			if (string.IsNullOrEmpty(this.sv.DecoSet))
			{
				this.tracking.TrackDecoItem(this.blueprint, "repair_unveil", 1, string.Empty);
			}
			this.view.SwitchUiMode(this, BuildingUiMode.None);
		}

		// Token: 0x0600378D RID: 14221 RVA: 0x0010EB4C File Offset: 0x0010CF4C
		public bool CanBeStored()
		{
			return (this.blueprint.type & 64) == 0 && this.sv.IsSaved && this.sv.IsUnveiled;
		}

		// Token: 0x0600378E RID: 14222 RVA: 0x0010EB7F File Offset: 0x0010CF7F
		private void CloseBottomPanel()
		{
			BuildingLocation.Selected = null;
			this.view.SwitchUiMode(this, BuildingUiMode.None);
		}

		// Token: 0x0600378F RID: 14223 RVA: 0x0010EB94 File Offset: 0x0010CF94
		private IEnumerator TryPurchaseRoutine()
		{
			// eli key point purchase
			this.view.bottomPanelRoot.Show(false);
			Wooroutine<bool> routine = new PurchaseBuildingFlow().Start(this);
			yield return routine;
			this.view.bottomPanelRoot.Show(true);
			if (BuildingLocation.Selected != null)
			{
				BuildingLocation.Selected.mustBePlaced = false;
			}
			bool purchase = false;
			try
			{
				purchase = routine.ReturnValue;
			}
			catch (Exception)
			{
				WoogaDebug.Log(new object[]
				{
					"Trying to double start PurchaseBuildingFlow."
				});
			}
			if (purchase)
			{
				this.PlaceBuilding();
				this.controller.AddBuildingToGamestate(this);
				if (this.isFromStorage)
				{
					this.controller.StoreBuilding(this.blueprint, -1);
				}
				if (this.blueprint.batch_mode != 0 && !this.cancelBatchMode)
				{
					IntVector2 b = IntVector2.Up;
					if (BuildingInstance.s_lastBuildingPosition != null)
					{
						if (BuildingInstance.s_lastBuildingPosition.Value.x < this.sv.position.x)
						{
							b = IntVector2.Right;
						}
						else if (BuildingInstance.s_lastBuildingPosition.Value.x > this.sv.position.x)
						{
							b = IntVector2.Left;
						}
						else if (BuildingInstance.s_lastBuildingPosition.Value.y < this.sv.position.y)
						{
							b = IntVector2.Up;
						}
						else if (BuildingInstance.s_lastBuildingPosition.Value.y > this.sv.position.y)
						{
							b = IntVector2.Down;
						}
					}
					BuildingInstance.PersistentData persistentData = new BuildingInstance.PersistentData
					{
						blueprintName = this.sv.blueprintName,
						position = this.sv.position + b
					};
					persistentData.SetTimer(BuildingTimer.Repair, BuildingInstance.UnixStamp);
					BuildingInstance buildingInstance = this.controller.CreateBuildingFromBlueprint(this.blueprint, persistentData, false, true);
					buildingInstance.isFromStorage = this.isFromStorage;
					int num = 0;
					while (!BuildingLocation.IsValidPlacement(persistentData.position, buildingInstance))
					{
						if (num >= IntVector2.Sides.Length)
						{
							persistentData.position = BuildingPlacementHelper.FindEmptySpotFor(buildingInstance, this.position, 100, 90);
							break;
						}
						persistentData.position = this.sv.position + IntVector2.Sides[num++];
					}
					buildingInstance.view.Show(buildingInstance);
					BuildingLocation.Selected = buildingInstance;
					buildingInstance.onPositionChanged.Dispatch(buildingInstance);
					BuildingInstance.s_lastBuildingPosition = new IntVector2?(this.sv.position);
					BlockerManager.global.Append(new PanCameraFlow(buildingInstance.view.FocusPosition, -100f, 1f, false));
					yield break;
				}
				BlockerManager.global.Append(PanCameraFlow.CreateZoomOutFlow());
				if (this.seasonService.GrandPrizeReady)
				{
					BuildingConfig grandPrizeBuildingConfig = this.seasonService.GetGrandPrizeBuildingConfig();
					BlockerManager.global.Append(new GrandPricePlaceBlocker(grandPrizeBuildingConfig));
				}
			}
			BuildingLocation.Selected = null;
			yield break;
		}

		// Token: 0x06003790 RID: 14224 RVA: 0x0010EBAF File Offset: 0x0010CFAF
		private void SetTimer(BuildingTimer timer, int deltaTimeInSeconds)
		{
			this.sv.SetTimer(timer, BuildingInstance.UnixStamp + deltaTimeInSeconds);
			this.onTimer.Dispatch(this);
		}

		// Token: 0x06003791 RID: 14225 RVA: 0x0010EBD0 File Offset: 0x0010CFD0
		private void PlaceBuilding()
		{
			if (!this.isFromStorage)
			{
				this.resources.AddMaterial(this.blueprint.Harmony, true);
				if (!this.seasonService.IsSeasonalsV3)
				{
					this.resources.AddMaterial(this.blueprint.SeasonCurrency, true);
				}
			}
			this.controller.onBuildingComplete.Dispatch(this);
			this.tracking.TrackDecoItem(this.blueprint, "unveil", 0, this.trackingDetail);
			this.SetTimer(BuildingTimer.Placed, 0);
		}

		// Token: 0x06003792 RID: 14226 RVA: 0x0010EC5C File Offset: 0x0010D05C
		private void Harvest()
		{
			MaterialAmount val = this.CalculateHarvestAmount();
			this.SetTimer(BuildingTimer.Harvest, 0);
			this.resources.AddMaterial(val.type, val.amount, true);
			this.onHarvest.Dispatch(this, val);
		}

		// Token: 0x06003793 RID: 14227 RVA: 0x0010ECA0 File Offset: 0x0010D0A0
		private MaterialAmount CalculateHarvestAmount()
		{
			if (!this.blueprint.CanHarvest)
			{
				return default(MaterialAmount);
			}
			int a = this.sv.HarvestTime / this.blueprint.harvest_timer;
			int amount = Mathf.Min(a, this.blueprint.harvest_maximum);
			return new MaterialAmount(this.blueprint.harvest_resource, amount, MaterialAmountUsage.Undefined, 0);
		}

		// Token: 0x06003794 RID: 14228 RVA: 0x0010ED04 File Offset: 0x0010D104
		private void HandleSelected(BuildingInstance self, bool selected)
		{
			this.controller.onBuildingSelected.Dispatch(self, selected);
			if (selected)
			{
				CameraInputController.current.StorePosition();
			}
		}

		// Token: 0x06003795 RID: 14229 RVA: 0x0010ED28 File Offset: 0x0010D128
		private void SetTrackingDetail(bool isFromWorkshop = false)
		{
			this.trackingDetail = string.Empty;
			if (this.blueprint != null)
			{
				if (!string.IsNullOrEmpty(this.blueprint.TrackingDetail))
				{
					this.trackingDetail = this.blueprint.TrackingDetail;
				}
				else if (isFromWorkshop)
				{
					this.trackingDetail = "workshop";
				}
				else if (this.blueprint.IsSeasonGrandPrice())
				{
					this.trackingDetail = "grand_prize";
				}
				else if (this.isDecoTrophy)
				{
					this.trackingDetail = "tournament";
				}
			}
		}

		// Token: 0x04005FBC RID: 24508
		public readonly Signal<BuildingInstance> onClick = new Signal<BuildingInstance>();

		// Token: 0x04005FBD RID: 24509
		public readonly Signal<BuildingInstance> onPositionChanged = new Signal<BuildingInstance>();

		// Token: 0x04005FBE RID: 24510
		public readonly Signal<BuildingInstance, bool> onSelected = new Signal<BuildingInstance, bool>();

		// Token: 0x04005FBF RID: 24511
		public readonly Signal<BuildingInstance> onTimer = new Signal<BuildingInstance>();

		// Token: 0x04005FC0 RID: 24512
		public readonly Signal<BuildingInstance, MaterialAmount> onHarvest = new Signal<BuildingInstance, MaterialAmount>();

		// Token: 0x04005FC1 RID: 24513
		public readonly Signal<BuildingInstance> onAssetUpdated = new Signal<BuildingInstance>();

		// Token: 0x04005FC2 RID: 24514
		public readonly bool isDecoTrophy;

		// Token: 0x04005FC3 RID: 24515
		public readonly BuildingConfig blueprint;

		// Token: 0x04005FC4 RID: 24516
		public BuildingMainView view;

		// Token: 0x04005FC5 RID: 24517
		public BuildingsController controller;

		// Token: 0x04005FC6 RID: 24518
		public ILocalizationService localization;

		// Token: 0x04005FC7 RID: 24519
		public TownPathfinding mapDataProvider;

		// Token: 0x04005FC8 RID: 24520
		public ResourceDataService resources;

		// Token: 0x04005FC9 RID: 24521
		public QuestService quests;

		// Token: 0x04005FCA RID: 24522
		public TrackingService tracking;

		// Token: 0x04005FCB RID: 24523
		public SeasonService seasonService;

		// Token: 0x04005FCC RID: 24524
		public int objectLayer;

		// Token: 0x04005FCD RID: 24525
		public bool isForeshadowing;

		// Token: 0x04005FCE RID: 24526
		public string trackingDetail;

		// Token: 0x04005FCF RID: 24527
		public bool cancelBatchMode;

		// Token: 0x04005FD0 RID: 24528
		public bool instantiateWithTimeSlicing = true;

		// Token: 0x04005FD1 RID: 24529
		public bool mustBePlaced;

		// Token: 0x04005FD2 RID: 24530
		public bool isFree;

		// Token: 0x04005FD3 RID: 24531
		public bool isFromStorage;

		// Token: 0x04005FD4 RID: 24532
		private static readonly int DEFAULT_TIME = new DateTime(1986, 1, 22).ToUnixTimeStamp();

		// Token: 0x04005FD5 RID: 24533
		private static readonly DateTime UNIX_START_TIME = new DateTime(1970, 1, 1);

		// Token: 0x04005FD6 RID: 24534
		private static IntVector2? s_lastBuildingPosition = null;

		// Token: 0x020008EF RID: 2287
		[Serializable]
		public class PersistentData
		{
			// Token: 0x1700088C RID: 2188
			// (get) Token: 0x06003798 RID: 14232 RVA: 0x0010EE0F File Offset: 0x0010D20F
			public bool IsSaved
			{
				get
				{
					return this.finishedAt != 0;
				}
			}

			// Token: 0x1700088D RID: 2189
			// (get) Token: 0x06003799 RID: 14233 RVA: 0x0010EE1D File Offset: 0x0010D21D
			public bool IsUnveiled
			{
				get
				{
					return BuildingInstance.PersistentData.IsPastTimestamp(this.unveiledAt);
				}
			}

			// Token: 0x1700088E RID: 2190
			// (get) Token: 0x0600379A RID: 14234 RVA: 0x0010EE2A File Offset: 0x0010D22A
			public bool IsRepaired
			{
				get
				{
					return BuildingInstance.PersistentData.IsPastTimestamp(this.repairedAt);
				}
			}

			// Token: 0x1700088F RID: 2191
			// (get) Token: 0x0600379B RID: 14235 RVA: 0x0010EE37 File Offset: 0x0010D237
			public int HarvestTime
			{
				get
				{
					return BuildingInstance.UnixStamp - this.startedProductionAt;
				}
			}

			// Token: 0x17000890 RID: 2192
			// (get) Token: 0x0600379C RID: 14236 RVA: 0x0010EE45 File Offset: 0x0010D245
			// (set) Token: 0x0600379D RID: 14237 RVA: 0x0010EE4D File Offset: 0x0010D24D
			public string DecoSet
			{
				get
				{
					return this.deco_set;
				}
				set
				{
					this.deco_set = value;
				}
			}

			// Token: 0x0600379E RID: 14238 RVA: 0x0010EE56 File Offset: 0x0010D256
			private static bool IsPastTimestamp(int eventAt)
			{
				return eventAt != 0 && eventAt <= BuildingInstance.UnixStamp;
			}

			// Token: 0x0600379F RID: 14239 RVA: 0x0010EE6C File Offset: 0x0010D26C
			public void SetTimer(BuildingTimer timer, int time)
			{
				switch (timer)
				{
				case BuildingTimer.Repair:
					this.repairedAt = time;
					this.startedProductionAt = time;
					this.unveiledAt = time;
					break;
				case BuildingTimer.Harvest:
					this.startedProductionAt = time;
					break;
				case BuildingTimer.Placed:
					this.finishedAt = time;
					this.unveiledAt = time;
					this.startedProductionAt = time;
					break;
				default:
					WoogaDebug.LogWarningFormatted("Timer {0} is not implemented", new object[]
					{
						timer
					});
					break;
				}
			}

			// Token: 0x060037A0 RID: 14240 RVA: 0x0010EEF0 File Offset: 0x0010D2F0
			private int GetTimer(BuildingTimer timer)
			{
				switch (timer)
				{
				case BuildingTimer.Unknown:
					return 0;
				case BuildingTimer.Repair:
					return this.repairedAt;
				case BuildingTimer.Harvest:
					return this.startedProductionAt;
				case BuildingTimer.Placed:
					return this.unveiledAt;
				}
				WoogaDebug.LogWarningFormatted("Timer {0} is not implemented", new object[]
				{
					timer
				});
				return 0;
			}

			// Token: 0x060037A1 RID: 14241 RVA: 0x0010EF54 File Offset: 0x0010D354
			public static BuildingInstance.PersistentData CreateFromConfig(StartBuilding building)
			{
				return new BuildingInstance.PersistentData
				{
					blueprintName = building.building_id,
					position = new IntVector2(building.pos_x, building.pos_y),
					finishedAt = BuildingInstance.DEFAULT_TIME,
					unveiledAt = BuildingInstance.DEFAULT_TIME,
					repairedAt = ((!building.destroyed) ? BuildingInstance.DEFAULT_TIME : 0),
					startedProductionAt = BuildingInstance.DEFAULT_TIME,
					deco_set = building.deco_set
				};
			}

			// Token: 0x060037A2 RID: 14242 RVA: 0x0010EFD4 File Offset: 0x0010D3D4
			public BuildingInstance.PersistentData Clone()
			{
				return new BuildingInstance.PersistentData
				{
					blueprintName = this.blueprintName,
					position = this.position,
					finishedAt = this.finishedAt,
					unveiledAt = this.unveiledAt,
					repairedAt = this.repairedAt,
					startedProductionAt = this.startedProductionAt
				};
			}

			// Token: 0x04005FD8 RID: 24536
			public string blueprintName;

			// Token: 0x04005FD9 RID: 24537
			public IntVector2 position;

			// Token: 0x04005FDA RID: 24538
			public int ownerArea;

			// Token: 0x04005FDB RID: 24539
			[SerializeField]
			private int finishedAt;

			// Token: 0x04005FDC RID: 24540
			[SerializeField]
			private int repairedAt;

			// Token: 0x04005FDD RID: 24541
			[SerializeField]
			private int unveiledAt;

			// Token: 0x04005FDE RID: 24542
			[SerializeField]
			private int startedProductionAt;

			// Token: 0x04005FDF RID: 24543
			[SerializeField]
			private string deco_set;
		}
	}
}
