using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Match3.Scripts1.Audio;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Puzzletown.Town;
using Match3.Scripts1.Spine.Unity;
using Match3.Scripts1.Town;
using Match3.Scripts1.Wooga.UI;
using UnityEngine;

// Token: 0x02000966 RID: 2406
namespace Match3.Scripts1
{
	[RequireComponent(typeof(BuildingLocation))]
	public class BuildingMainView : AVisibleGameObject, IDataView<BuildingInstance>
	{
		// Token: 0x170008EA RID: 2282
		// (get) Token: 0x06003AA8 RID: 15016 RVA: 0x00122972 File Offset: 0x00120D72
		public BuildingInstance Data
		{
			get
			{
				return this.data;
			}
		}

		// Token: 0x170008EB RID: 2283
		// (get) Token: 0x06003AA9 RID: 15017 RVA: 0x0012297A File Offset: 0x00120D7A
		// (set) Token: 0x06003AAA RID: 15018 RVA: 0x00122982 File Offset: 0x00120D82
		public BuildingUiMode currentUiMode { get; private set; }

		// Token: 0x170008EC RID: 2284
		// (get) Token: 0x06003AAB RID: 15019 RVA: 0x0012298B File Offset: 0x00120D8B
		// (set) Token: 0x06003AAC RID: 15020 RVA: 0x00122993 File Offset: 0x00120D93
		public BuildingInstance data { get; private set; }

		// Token: 0x170008ED RID: 2285
		// (get) Token: 0x06003AAD RID: 15021 RVA: 0x0012299C File Offset: 0x00120D9C
		public float SelectedElevation
		{
			get
			{
				return (this.currentUiMode != BuildingUiMode.BlockedArea) ? 0.5f : 0.125f;
			}
		}

		// Token: 0x170008EE RID: 2286
		// (get) Token: 0x06003AAE RID: 15022 RVA: 0x001229B9 File Offset: 0x00120DB9
		public BuildingUiControlPanel controlButtons
		{
			get
			{
				return (!this.bottomPanelRoot) ? null : this.bottomPanelRoot.controlButtons;
			}
		}

		// Token: 0x170008EF RID: 2287
		// (get) Token: 0x06003AAF RID: 15023 RVA: 0x001229DC File Offset: 0x00120DDC
		// (set) Token: 0x06003AB0 RID: 15024 RVA: 0x001229E9 File Offset: 0x00120DE9
		public IntVector2 Position
		{
			get
			{
				return base.GetComponent<BuildingLocation>().Position;
			}
			set
			{
				base.GetComponent<BuildingLocation>().Position = value;
			}
		}

		// Token: 0x170008F0 RID: 2288
		// (get) Token: 0x06003AB1 RID: 15025 RVA: 0x001229F8 File Offset: 0x00120DF8
		public Vector3 FocusPosition
		{
			get
			{
				return base.transform.position + new Vector3((float)this.data.blueprint.size, 0f, (float)this.data.blueprint.size) * 0.5f;
			}
		}

		// Token: 0x170008F1 RID: 2289
		// (get) Token: 0x06003AB2 RID: 15026 RVA: 0x00122A4B File Offset: 0x00120E4B
		// (set) Token: 0x06003AB3 RID: 15027 RVA: 0x00122A53 File Offset: 0x00120E53
		private BuildingMainView.AnimationType AnimType
		{
			get
			{
				return this.animType;
			}
			set
			{
				this.WaitForSkeletonAndSetAnimationTo(value);
			}
		}

		// Token: 0x06003AB4 RID: 15028 RVA: 0x00122A5C File Offset: 0x00120E5C
		public void Init(ConfigService configService, TownOverheadUiRoot overheadUiRoot, TownBottomPanelRoot bottomPanel)
		{
			this.uiRoot = overheadUiRoot;
			this.audioService = this.uiRoot.audioService;
			this.bottomPanelRoot = bottomPanel;
			this.configService = configService;
		}

		// Token: 0x06003AB5 RID: 15029 RVA: 0x00122A84 File Offset: 0x00120E84
		public void LoadAsset(BuildingInstance data)
		{
			if (this.assetLoader == null)
			{
				this.assetLoader = base.GetComponent<BuildingAssetLoader>();
			}
			if (this.assetLoader != null)
			{
				this.assetLoader.Show(data);
			}
		}

		// Token: 0x06003AB6 RID: 15030 RVA: 0x00122AC0 File Offset: 0x00120EC0
		public void Show(BuildingInstance data)
		{
			this.data = data;
			IEnumerable<IDataView<BuildingInstance>> ie = from c in base.GetComponents<IDataView<BuildingInstance>>()
				where !object.ReferenceEquals(c, this)
				select c;
			ie.ForEach(delegate(IDataView<BuildingInstance> c)
			{
				c.Show(data);
			});
			base.name = data.blueprint.name;
			this.AddListeners();
		}

		// Token: 0x06003AB7 RID: 15031 RVA: 0x00122B33 File Offset: 0x00120F33
		private void WaitForSkeletonAndSetAnimationTo(BuildingMainView.AnimationType value)
		{
			if (base.gameObject.activeInHierarchy)
			{
				base.StartCoroutine(this.WaitForSkeletonAndSetAnimationRoutine(value));
			}
		}

		// Token: 0x06003AB8 RID: 15032 RVA: 0x00122B54 File Offset: 0x00120F54
		private IEnumerator WaitForSkeletonAndSetAnimationRoutine(BuildingMainView.AnimationType value)
		{
			while (this.skeletonAnimation == null)
			{
				yield return null;
				this.TryGetSkeletonAnimation();
			}
			string animname = value.ToString();
			try
			{
				this.skeletonAnimation.state.SetAnimation(0, "placed down", false);
				this.skeletonAnimation.state.AddAnimation(0, animname, true, 0f);
			}
			catch (ArgumentException)
			{
			}
			this.animType = value;
			yield break;
		}

		// Token: 0x06003AB9 RID: 15033 RVA: 0x00122B76 File Offset: 0x00120F76
		private bool TryGetSkeletonAnimation()
		{
			if (this.skeletonAnimation == null)
			{
				this.skeletonAnimation = base.GetComponentInChildren<SkeletonAnimation>();
			}
			return this.skeletonAnimation != null;
		}

		// Token: 0x06003ABA RID: 15034 RVA: 0x00122BA1 File Offset: 0x00120FA1
		private void OnDestroy()
		{
			BuildingMainView.lastSelectedView = null;
			this.RemoveListeners();
		}

		// Token: 0x06003ABB RID: 15035 RVA: 0x00122BB0 File Offset: 0x00120FB0
		private void AddListeners()
		{
			if (this.data != null)
			{
				this.data.onSelected.AddListener(new Action<BuildingInstance, bool>(this.OnSelected));
				this.data.onClick.AddListener(new Action<BuildingInstance>(this.OnClick));
				this.data.onPositionChanged.AddListener(new Action<BuildingInstance>(this.OnPositionChanged));
			}
		}

		// Token: 0x06003ABC RID: 15036 RVA: 0x00122C1C File Offset: 0x0012101C
		private void RemoveListeners()
		{
			if (this.data != null)
			{
				this.data.onSelected.RemoveListener(new Action<BuildingInstance, bool>(this.OnSelected));
				this.data.onClick.RemoveListener(new Action<BuildingInstance>(this.OnClick));
				this.data.onPositionChanged.RemoveListener(new Action<BuildingInstance>(this.OnPositionChanged));
			}
		}

		// Token: 0x06003ABD RID: 15037 RVA: 0x00122C88 File Offset: 0x00121088
		public void InitCalloutController()
		{
			this.calloutController = base.gameObject.AddComponent<BuildingCalloutController>();
		}

		// Token: 0x06003ABE RID: 15038 RVA: 0x00122C9C File Offset: 0x0012109C
		private void FixedUpdate()
		{
			if (this.syncWobble)
			{
				this.SyncWobbleAnimation();
				this.syncWobble = false;
			}
			this.refreshTimer -= Time.fixedDeltaTime;
			if (this.refreshTimer >= 0f)
			{
				return;
			}
			this.refreshTimer = 1f;
			if (this.calloutController)
			{
				this.calloutController.Refresh(this.data, this.uiRoot);
			}
		}

		// Token: 0x06003ABF RID: 15039 RVA: 0x00122D18 File Offset: 0x00121118
		private void SyncWobbleAnimation()
		{
			if (!this.skeletonAnimation)
			{
				return;
			}
			Spine.Animation animation = this.skeletonAnimation.skeleton.Data.FindAnimation("active");
			Spine.Animation animation2 = this.skeletonAnimation.skeleton.Data.FindAnimation("placed down");
			if (animation == null || animation.duration == 0f)
			{
				return;
			}
			if (animation2 == null)
			{
				return;
			}
			float speed = (float)this.skeletonAnimation.wobbleAnimationCycles / animation.duration;
			float num;
			switch (this.skeletonAnimation.wobbleAnimationStart)
			{
				case SkeletonRenderer.StartAnimationCycle.middleToRight:
					num = 0.25f;
					break;
				case SkeletonRenderer.StartAnimationCycle.right:
					num = 0.5f;
					break;
				case SkeletonRenderer.StartAnimationCycle.middleToLeft:
					num = 0.75f;
					break;
				default:
					num = 0f;
					break;
			}
			IEnumerator enumerator = this.anim.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					global::UnityEngine.AnimationState animationState = (global::UnityEngine.AnimationState)obj;
					animationState.speed = speed;
					animationState.time = animationState.length * num - animation2.duration * animationState.speed;
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
		}

		// Token: 0x06003AC0 RID: 15040 RVA: 0x00122E78 File Offset: 0x00121278
		private void OnSelected(BuildingInstance building, bool selected)
		{
			if (selected)
			{
				BuildingMainView.CloseLastPopup();
				bool flag = !building.mapDataProvider.HasRubble(building.position) && building.sv.IsRepaired;
				this.SwitchUiMode(building, flag ? BuildingUiMode.Movement : BuildingUiMode.BlockedArea);
				this.positionHelper.Show(building);
			}
			else
			{
				this.positionHelper.Show(building);
				this.positionHelper.Hide();
				this.SwitchUiMode(building, BuildingUiMode.None);
			}
			if (this.anim)
			{
				this.anim.enabled = (selected && this.currentUiMode != BuildingUiMode.BlockedArea);
				base.transform.rotation = Quaternion.identity;
				if (selected)
				{
					this.syncWobble = true;
				}
			}
			if (this.bottomPanelRoot)
			{
				this.bottomPanelRoot.State = ((!selected) ? TownBottomPanelRoot.UIState.InGameUI : TownBottomPanelRoot.UIState.MovementMode);
			}
			else
			{
				WoogaDebug.LogWarning(new object[]
				{
					"TownBottomPanelRoot is not loaded"
				});
			}
			this.uiRoot.inGameUi.SetVisible(!selected);
			CameraInputController.current.mainCamera.GetComponent<CameraPanManager>().SwitchDisplayMode(selected);
			CameraInputController.current.mainCamera.SetLayerVisible(ObjectLayer.DebugInfo, selected);
			CameraInputController.current.selectedObjectCamera.gameObject.SetActive(selected);
		}

		// Token: 0x06003AC1 RID: 15041 RVA: 0x00122FD8 File Offset: 0x001213D8
		private string RepairMessage(BuildingInstance building)
		{
			string text = string.Empty;
			foreach (QuestData questData in this.configService.SbsConfig.questconfig.quests)
			{
				foreach (QuestTaskData questTaskData in questData.Tasks)
				{
					if (questTaskData.type == QuestTaskType.collect_and_repair && questTaskData.action_target == building.sv.DecoSet)
					{
						text = questData.id;
					}
				}
			}
			if (string.IsNullOrEmpty(text))
			{
				return string.Empty;
			}
			string text2 = building.localization.GetText("quest.title." + text, new LocaParam[0]);
			return string.Format(building.localization.GetText("shop.buildings.not_repaired_yet", new LocaParam[0]), text2);
		}

		// Token: 0x06003AC2 RID: 15042 RVA: 0x001230E0 File Offset: 0x001214E0
		private void OnClick(BuildingInstance building)
		{
			if (this.currentUiMode != BuildingUiMode.None)
			{
				this.SwitchUiMode(building, BuildingUiMode.None);
				BuildingLocation.Selected = null;
			}
			else if (building.mapDataProvider.IsPositionUnlocked(building.position))
			{
				bool flag = !building.mapDataProvider.HasRubble(building.position) && building.sv.IsRepaired;
				BuildingUiMode type = (!flag) ? BuildingUiMode.BlockedArea : BuildingUiMode.Movement;
				this.SwitchUiMode(building, type);
			}
		}

		// Token: 0x06003AC3 RID: 15043 RVA: 0x0012315C File Offset: 0x0012155C
		private void OnPositionChanged(BuildingInstance building)
		{
			BuildingOperation buildingOperation = BuildingOperation.Cancel;
			IntVector2 location = BuildingLocation.GetLocation(building);
			if (this.data.CanBeStored())
			{
				buildingOperation |= BuildingOperation.Store;
			}
			if (BuildingLocation.IsValidPlacement(location, building))
			{
				buildingOperation |= BuildingOperation.Confirm;
			}
			else
			{
				buildingOperation |= BuildingOperation.ConfirmDisabled;
			}
			this.controlButtons.Show(building, buildingOperation);
		}

		// Token: 0x06003AC4 RID: 15044 RVA: 0x001231AE File Offset: 0x001215AE
		public static void CloseLastPopup()
		{
			if (BuildingMainView.lastSelectedView && BuildingMainView.lastSelectedView.currentUiMode != BuildingUiMode.None)
			{
				BuildingMainView.lastSelectedView.SwitchUiMode(BuildingMainView.lastSelectedView.data, BuildingUiMode.None);
			}
		}

		// Token: 0x06003AC5 RID: 15045 RVA: 0x001231E4 File Offset: 0x001215E4
		public void SwitchUiMode(BuildingInstance building, BuildingUiMode type)
		{
			BuildingMainView.lastSelectedView = building.view;
			this.currentUiMode = type;
			bool flag = BuildingLocation.IsValidPlacement(this.Position, building);
			if (type != BuildingUiMode.BlockedArea)
			{
				if (type != BuildingUiMode.Movement)
				{
					if (this.AnimType != BuildingMainView.AnimationType.idle)
					{
						this.AnimType = BuildingMainView.AnimationType.idle;
					}
				}
				else
				{
					this.AnimType = BuildingMainView.AnimationType.active;
					BuildingLocation.Selected = this.data;
					BuildingOperation buildingOperation = BuildingOperation.Cancel;
					if (flag)
					{
						buildingOperation |= BuildingOperation.Confirm;
					}
					else
					{
						buildingOperation |= BuildingOperation.ConfirmDisabled;
					}
					if (building.CanBeStored())
					{
						buildingOperation |= BuildingOperation.Store;
					}
					this.controlButtons.Show(building, buildingOperation);
					if (building.State != BuildingState.NotSaved)
					{
						this.audioService.PlaySFX(AudioId.PickupObject, false, false, false);
					}
				}
			}
			else
			{
				BuildingLocation.Selected = this.data;
				this.AnimType = BuildingMainView.AnimationType.idle;
				this.controlButtons.Show(building, BuildingOperation.Cancel);
				this.controlButtons.buildingName.gameObject.SetActive(false);
				if (!building.mapDataProvider.HasRubble(building.position))
				{
					this.controlButtons.buildingIncome.text = this.RepairMessage(building);
				}
				else
				{
					// eli key point 区域未解锁
					this.controlButtons.buildingIncome.text = building.localization.GetText("building.info.area_locked", new LocaParam[0]);
				}
				this.audioService.PlaySFX(AudioId.BuildingInfoPopup, false, false, false);
			}
		}

		// Token: 0x04006288 RID: 25224
		[NonSerialized]
		public PositionHelper positionHelper;

		// Token: 0x04006289 RID: 25225
		public bool syncWobble;

		// Token: 0x0400628A RID: 25226
		public TownBottomPanelRoot bottomPanelRoot;

		// Token: 0x0400628B RID: 25227
		public BuildingAssetLoader assetLoader;

		// Token: 0x0400628C RID: 25228
		public global::UnityEngine.Animation anim;

		// Token: 0x0400628D RID: 25229
		private const float SELECTED_ELEVATION = 0.5f;

		// Token: 0x0400628E RID: 25230
		private const float INVALID_ELEVATION = 0.125f;

		// Token: 0x0400628F RID: 25231
		private const float REFRESH_INTERVAL = 1f;

		// Token: 0x04006290 RID: 25232
		private static BuildingMainView lastSelectedView;

		// Token: 0x04006291 RID: 25233
		private TownOverheadUiRoot uiRoot;

		// Token: 0x04006292 RID: 25234
		private AudioService audioService;

		// Token: 0x04006293 RID: 25235
		private ConfigService configService;

		// Token: 0x04006294 RID: 25236
		private BuildingCalloutController calloutController;

		// Token: 0x04006295 RID: 25237
		private float refreshTimer;

		// Token: 0x04006296 RID: 25238
		private SkeletonAnimation skeletonAnimation;

		// Token: 0x04006297 RID: 25239
		private BuildingMainView.AnimationType animType;

		// Token: 0x02000967 RID: 2407
		public enum AnimationType
		{
			// Token: 0x0400629B RID: 25243
			idle,
			// Token: 0x0400629C RID: 25244
			active
		}
	}
}
