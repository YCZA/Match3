using System;
using Match3.Scripts1.Town;
using UnityEngine;

// Token: 0x02000990 RID: 2448
namespace Match3.Scripts1
{
	public class BuildingUiHarvestCallout : ABuildingUiStateCallout
	{
		// Token: 0x17000908 RID: 2312
		// (get) Token: 0x06003B91 RID: 15249 RVA: 0x00127CD2 File Offset: 0x001260D2
		protected override Vector3 Pivot
		{
			get
			{
				return this.data.worldCenterTop;
			}
		}

		// Token: 0x06003B92 RID: 15250 RVA: 0x00127CDF File Offset: 0x001260DF
		public override void DoShow(BuildingInstance building)
		{
			building.onHarvest.AddListener(new Action<BuildingInstance, MaterialAmount>(this.HandleHarvest));
			this.Refresh(building);
		}

		// Token: 0x06003B93 RID: 15251 RVA: 0x00127D00 File Offset: 0x00126100
		public override void Refresh(BuildingInstance building)
		{
			if (this.townOverheadUiRoot == null || !this.townOverheadUiRoot.IsSetup)
			{
				return;
			}
			bool flag = building.State == BuildingState.Harvestable;
			bool flag2 = flag != this.IsVisible;
			if (flag2)
			{
				base.SetVisibility(flag);
				if (this.IsVisible)
				{
					this.materialView.image.sprite = this.materialView.manager.GetSimilar(building.blueprint.harvest_resource);
					base.RefreshPosition(building);
				}
			}
		}

		// Token: 0x06003B94 RID: 15252 RVA: 0x00127D90 File Offset: 0x00126190
		public override void Handle(BuildingOperation op)
		{
			if (op == BuildingOperation.Harvest)
			{
				this.data.OnClick(this.data);
			}
		}

		// Token: 0x06003B95 RID: 15253 RVA: 0x00127DB8 File Offset: 0x001261B8
		private void HandleHarvest(BuildingInstance building, MaterialAmount mat)
		{
			this.Refresh(building);
		}

		// Token: 0x0400638C RID: 25484
		public MaterialAmountView materialView;
	}
}
