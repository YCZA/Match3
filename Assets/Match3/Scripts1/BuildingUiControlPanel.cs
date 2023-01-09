using System;
using System.Text;
using Match3.Scripts1.Audio;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Puzzletown.UI;
using Match3.Scripts1.Town;
using Match3.Scripts1.UnityEngine;
using TMPro;

// Token: 0x0200098F RID: 2447
namespace Match3.Scripts1
{
	public class BuildingUiControlPanel : AVisibleGameObject, IHandler<BuildingOperation>
	{
		// Token: 0x17000906 RID: 2310
		// (get) Token: 0x06003B86 RID: 15238 RVA: 0x00127906 File Offset: 0x00125D06
		// (set) Token: 0x06003B87 RID: 15239 RVA: 0x0012790E File Offset: 0x00125D0E
		public BuildingOperation showing { get; private set; }

		// Token: 0x17000907 RID: 2311
		// (get) Token: 0x06003B88 RID: 15240 RVA: 0x00127917 File Offset: 0x00125D17
		private AudioService AudioService
		{
			get
			{
				if (this._audioService == null)
				{
					this._audioService = this.bottomPanelRoot.audioService;
				}
				return this._audioService;
			}
		}

		// Token: 0x06003B89 RID: 15241 RVA: 0x0012793B File Offset: 0x00125D3B
		private void Awake()
		{
			this.Hide();
		}

		// Token: 0x06003B8A RID: 15242 RVA: 0x00127944 File Offset: 0x00125D44
		public void Handle(BuildingOperation op)
		{
			if (this.bottomPanelRoot.State != TownBottomPanelRoot.UIState.MovementMode)
			{
				return;
			}
			if (!this.bottomPanelRoot.IsButtonExpanded(op))
			{
				this.bottomPanelRoot.SetButtonExpanded(op, true);
			}
			else
			{
				this.data.Handle(op);
			}
			if (op == BuildingOperation.Confirm)
			{
				this.AudioService.PlaySFX(AudioId.PlaceObject, false, false, false);
			}
		}

		// Token: 0x06003B8B RID: 15243 RVA: 0x001279B0 File Offset: 0x00125DB0
		public void ShowMessage(BuildingMessageType message, string text = null)
		{
			this.ShowOnChildren(message, true, true);
			this.ExecuteOnChild(message, delegate(MessageUi t)
			{
				t.text = text;
			});
		}

		// Token: 0x06003B8C RID: 15244 RVA: 0x001279E8 File Offset: 0x00125DE8
		public void Show(BuildingInstance building, BuildingOperation op)
		{
			if ((op & BuildingOperation.Cancel) == BuildingOperation.Cancel && building.mustBePlaced)
			{
				op ^= BuildingOperation.Cancel;
			}
			if (!this.bottomPanelRoot.IsStorageAvailable)
			{
				op &= ~BuildingOperation.Store;
			}
			this.data = building;
			this.showing = op;
			this.bottomPanelRoot.ResetBuildingControlsAnimator();
			base.SetVisibility(op != BuildingOperation.None);
			this.bottomPanelRoot.State = ((op != BuildingOperation.None) ? TownBottomPanelRoot.UIState.MovementMode : TownBottomPanelRoot.UIState.InGameUI);
			this.buildingName.gameObject.SetActive(true);
			this.buildingName.text = this.bottomPanelRoot.loc.GetText(building.blueprint.Name_LocaleKey, new LocaParam[0]);
			if (building.sv.IsRepaired && !building.isFromStorage)
			{
				if (building.blueprint.Harvest.amount > 0)
				{
					BuildingUiControlPanel.ShowMaterialAmountArray(this.buildingIncome, building.localization, new MaterialAmount[]
					{
						building.blueprint.Harmony,
						building.blueprint.Harvest
					});
				}
				else
				{
					BuildingUiControlPanel.ShowMaterialAmountArray(this.buildingIncome, building.localization, new MaterialAmount[]
					{
						building.blueprint.Harmony
					});
				}
			}
			else
			{
				BuildingUiControlPanel.ShowMaterialAmountArray(this.buildingIncome, building.localization, new MaterialAmount[0]);
			}
			this.ShowOnChildren(op, true, true);
		}

		// Token: 0x06003B8D RID: 15245 RVA: 0x00127B70 File Offset: 0x00125F70
		public void Show(AreaClouds cpa)
		{
			this.bottomPanelRoot.State = TownBottomPanelRoot.UIState.MovementMode;
			this.showing = BuildingOperation.Confirm;
			this.data = cpa;
			this.buildingName.gameObject.SetActive(true);
			this.buildingName.text = string.Format(this.bottomPanelRoot.loc.GetText("ui.controlButtons.area_locked.title", new LocaParam[0]), cpa.area);
			this.buildingIncome.text = this.bottomPanelRoot.loc.GetText("ui.controlButtons.area_locked.details", new LocaParam[0]);
			this.Show();
			this.ShowOnChildren(this.showing, true, true);
		}

		// Token: 0x06003B8E RID: 15246 RVA: 0x00127C18 File Offset: 0x00126018
		public static void ShowMaterialAmountArray(TMP_Text label, ILocalizationService loc, params MaterialAmount[] data)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (MaterialAmount materialAmount in data)
			{
				if (materialAmount.amount > 0)
				{
					stringBuilder.AppendFormat("+{0}{1} ", materialAmount.ToString(loc), materialAmount.SpriteName);
				}
			}
			label.text = stringBuilder.ToString();
		}

		// Token: 0x06003B8F RID: 15247 RVA: 0x00127C82 File Offset: 0x00126082
		public void HandleCancelViaBackButton()
		{
			if ((this.showing & BuildingOperation.Cancel) == BuildingOperation.Cancel)
			{
				this.Handle(BuildingOperation.Cancel);
			}
			else
			{
				BackButtonManager.Instance.AddAction(new Action(this.HandleCancelViaBackButton));
			}
		}

		// Token: 0x04006386 RID: 25478
		private AudioService _audioService;

		// Token: 0x04006387 RID: 25479
		private IHandler<BuildingOperation> data;

		// Token: 0x04006388 RID: 25480
		public TownBottomPanelRoot bottomPanelRoot;

		// Token: 0x04006389 RID: 25481
		public TMP_Text buildingName;

		// Token: 0x0400638A RID: 25482
		public TMP_Text buildingIncome;
	}
}
