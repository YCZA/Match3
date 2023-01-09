using System.Collections.Generic;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Puzzletown.UI;
using Match3.Scripts1.Town;
using Match3.Scripts1.UnityEngine;
using UnityEngine;

// Token: 0x0200091E RID: 2334
namespace Match3.Scripts1
{
	public class PositionHelper : ABuildingUiView
	{
		// Token: 0x060038EB RID: 14571 RVA: 0x001183F1 File Offset: 0x001167F1
		protected void Awake()
		{
			this.materialBlock = new MaterialPropertyBlock();
			this._SpecialColor = Shader.PropertyToID("_SpecialColor");
		}

		// Token: 0x060038EC RID: 14572 RVA: 0x0011840E File Offset: 0x0011680E
		protected void OnDisable()
		{
			if (this.controlPanel != null)
			{
				this.controlPanel.ShowMessage(BuildingMessageType.None, null);
			}
		}

		// Token: 0x170008C3 RID: 2243
		// (get) Token: 0x060038ED RID: 14573 RVA: 0x0011842E File Offset: 0x0011682E
		// (set) Token: 0x060038EE RID: 14574 RVA: 0x00118440 File Offset: 0x00116840
		private IntVector2 Position
		{
			get
			{
				return IntVector2.ProjectToGridXZ(base.transform.position);
			}
			set
			{
				base.transform.position = value.ProjectToVector3XZ() + Vector3.up * this.building.view.SelectedElevation;
			}
		}

		// Token: 0x060038EF RID: 14575 RVA: 0x00118474 File Offset: 0x00116874
		private void Update()
		{
			if (this.building == null || this.building.view == null)
			{
				return;
			}
			base.transform.position = this.building.view.transform.position;
			if (this.oldPosition == this.Position && !this.forceUiHint)
			{
				return;
			}
			bool flag = BuildingLocation.IsValidPlacement(this.Position, this.building);
			this.oldPosition = this.Position;
			BuildingMessageType message = BuildingMessageType.None;
			int messageParam = 0;
			if (this.building.view.currentUiMode == BuildingUiMode.BlockedArea)
			{
				this.selectedObjectCamera.SetLayerVisible(ObjectLayer.InvalidPlacement, false);
				this.selectedObjectCamera.SetLayerVisible(ObjectLayer.ValidPlacement, true);
				this.ExecuteOnChildren(delegate(PositionHelperArrow arrow)
				{
					arrow.Hide();
				}, true);
				this.materialBlock.SetColor(this._SpecialColor, this.invalidPlacement);
				this.fullQuad.SetPropertyBlock(this.materialBlock);
			}
			else if (flag)
			{
				this.selectedObjectCamera.SetLayerVisible(ObjectLayer.InvalidPlacement, false);
				this.selectedObjectCamera.SetLayerVisible(ObjectLayer.ValidPlacement, true);
				this.materialBlock.SetColor(this._SpecialColor, this.defaultPlacement);
				this.fullQuad.SetPropertyBlock(this.materialBlock);
				this.ExecuteOnChildren(delegate(PositionHelperArrow arrow)
				{
					arrow.Show();
				}, true);
			}
			else
			{
				this.selectedObjectCamera.SetLayerVisible(ObjectLayer.InvalidPlacement, true);
				this.selectedObjectCamera.SetLayerVisible(ObjectLayer.ValidPlacement, false);
				this.materialBlock.SetColor(this._SpecialColor, this.defaultPlacement);
				this.fullQuad.SetPropertyBlock(this.materialBlock);
				this.ExecuteOnChildren(delegate(PositionHelperArrow arrow)
				{
					arrow.Show();
				}, true);
				foreach (GameObject gameObject in this.quads)
				{
					IntVector2 position = this.Position;
					bool flag2 = true;
					position.x += Mathf.FloorToInt(gameObject.transform.localPosition.x);
					position.y += Mathf.FloorToInt(gameObject.transform.localPosition.z);
					if (!this.building.mapDataProvider.IsPositionUnlocked(position))
					{
						messageParam = this.building.mapDataProvider.GetLocalArea(position.x, position.y);
						message = BuildingMessageType.AreaLocked;
						flag2 = false;
					}
					else if (this.building.mapDataProvider.HasRubble(position))
					{
						messageParam = this.building.mapDataProvider.GetLocalArea(position.x, position.y);
						message = BuildingMessageType.RubbleNotCleared;
						flag2 = false;
					}
					else
					{
						foreach (BuildingInstance buildingInstance in this.building.controller.Buildings)
						{
							if (buildingInstance != this.building && !buildingInstance.blueprint.IsRubble())
							{
								if (buildingInstance.Area.Contains(position))
								{
									flag2 = false;
									break;
								}
							}
						}
					}
					this.materialBlock.SetColor(this._SpecialColor, (!flag2) ? this.badPlacement : this.goodPlacement);
					gameObject.GetComponentInChildren<Renderer>().SetPropertyBlock(this.materialBlock);
				}
			}
			this.ShowUIHint(message, messageParam);
		}

		// Token: 0x060038F0 RID: 14576 RVA: 0x00118874 File Offset: 0x00116C74
		private void ShowUIHint(BuildingMessageType message, int messageParam)
		{
			string format = string.Empty;
			if (message != BuildingMessageType.AreaLocked)
			{
				if (message == BuildingMessageType.RubbleNotCleared)
				{
					format = this.building.localization.GetText("ui.shared.building_clear_rubble", new LocaParam[0]);
				}
			}
			else if (this.configService.areas.areas.Count > messageParam - 1 && messageParam > 0)
			{
				messageParam = this.configService.areas.areas[messageParam - 1].levels[0].level;
				format = this.building.localization.GetText("ui.shared.building_area_locked", new LocaParam[0]);
			}
			else
			{
				message = BuildingMessageType.None;
			}
			if (message == BuildingMessageType.None && this.building.isDecoTrophy && this.building.mustBePlaced)
			{
				message = BuildingMessageType.PlaceDecoTrophy;
				messageParam = 0;
				format = this.building.localization.GetText("ui.tournaments.place_deco_trophy", new LocaParam[0]);
			}
			this.controlPanel.ShowMessage(message, string.Format(format, messageParam));
			this.forceUiHint = false;
		}

		// Token: 0x060038F1 RID: 14577 RVA: 0x0011899C File Offset: 0x00116D9C
		public override void Show(BuildingInstance building)
		{
			this.building = building;
			this.Show();
			foreach (GameObject obj in this.quads)
			{
				global::UnityEngine.Object.Destroy(obj);
			}
			this.quads.Clear();
			this.oldPosition = new IntVector2(int.MaxValue, int.MaxValue);
			int size = building.blueprint.size;
			for (int i = 0; i < size; i++)
			{
				for (int j = 0; j < size; j++)
				{
					GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(this.invalidQuad);
					gameObject.transform.localPosition = new Vector3((float)i + 0.5f, 0f, (float)j + 0.5f);
					gameObject.transform.SetParent(base.transform, false);
					gameObject.gameObject.SetActive(true);
					this.quads.Add(gameObject);
				}
			}
			this.ShowOnChildren(size, true, true);
			this.invalidQuad.gameObject.SetActive(false);
			this.shadow.transform.localPosition = new Vector3(0f, -building.view.SelectedElevation + 0.01f, 0f);
			this.forceUiHint = true;
		}

		// Token: 0x04006139 RID: 24889
		private int _SpecialColor;

		// Token: 0x0400613A RID: 24890
		private BuildingInstance building;

		// Token: 0x0400613B RID: 24891
		private MaterialPropertyBlock materialBlock;

		// Token: 0x0400613C RID: 24892
		public BuildingUiControlPanel controlPanel;

		// Token: 0x0400613D RID: 24893
		public ConfigService configService;

		// Token: 0x0400613E RID: 24894
		private List<GameObject> quads = new List<GameObject>();

		// Token: 0x0400613F RID: 24895
		private IntVector2 oldPosition = IntVector2.Zero;

		// Token: 0x04006140 RID: 24896
		public GameObject invalidQuad;

		// Token: 0x04006141 RID: 24897
		public GameObject shadow;

		// Token: 0x04006142 RID: 24898
		public Renderer fullQuad;

		// Token: 0x04006143 RID: 24899
		public Color goodPlacement = Color.green;

		// Token: 0x04006144 RID: 24900
		public Color badPlacement = Color.red;

		// Token: 0x04006145 RID: 24901
		public Color invalidPlacement = Color.white;

		// Token: 0x04006146 RID: 24902
		public Color defaultPlacement = Color.green;

		// Token: 0x04006147 RID: 24903
		public Camera selectedObjectCamera;

		// Token: 0x04006148 RID: 24904
		private bool forceUiHint;
	}
}
