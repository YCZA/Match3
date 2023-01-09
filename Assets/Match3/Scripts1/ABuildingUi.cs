using System;
using Match3.Scripts1.Town;
using UnityEngine;

// Token: 0x02000984 RID: 2436
namespace Match3.Scripts1
{
	public abstract class ABuildingUi : ABuildingUiView
	{
		// Token: 0x17000903 RID: 2307
		// (get) Token: 0x06003B60 RID: 15200
		protected abstract Vector3 Pivot { get; }

		// Token: 0x06003B61 RID: 15201 RVA: 0x0012768C File Offset: 0x00125A8C
		public override void Show(BuildingInstance building)
		{
			this.data = building;
			this.AddListeners();
			this.RefreshPosition(building);
		}

		// Token: 0x06003B62 RID: 15202 RVA: 0x001276A4 File Offset: 0x00125AA4
		protected void AddListeners()
		{
			this.data.onPositionChanged.AddListener(new Action<BuildingInstance>(this.RefreshPosition));
			this.data.onSelected.AddListener(new Action<BuildingInstance, bool>(this.HandleSelected));
			this.data.onAssetUpdated.AddListener(new Action<BuildingInstance>(this.RefreshPosition));
		}

		// Token: 0x06003B63 RID: 15203 RVA: 0x00127705 File Offset: 0x00125B05
		protected void OnDestroy()
		{
			this.RemoveListeners();
		}

		// Token: 0x06003B64 RID: 15204 RVA: 0x00127710 File Offset: 0x00125B10
		protected void RemoveListeners()
		{
			if (this.data != null)
			{
				this.data.onPositionChanged.RemoveListener(new Action<BuildingInstance>(this.RefreshPosition));
				this.data.onSelected.RemoveListener(new Action<BuildingInstance, bool>(this.HandleSelected));
				this.data.onAssetUpdated.RemoveListener(new Action<BuildingInstance>(this.RefreshPosition));
			}
		}

		// Token: 0x06003B65 RID: 15205 RVA: 0x0012777C File Offset: 0x00125B7C
		private void HandleSelected(BuildingInstance building, bool selected)
		{
			this.RefreshPosition(building);
		}

		// Token: 0x06003B66 RID: 15206 RVA: 0x00127788 File Offset: 0x00125B88
		public void RefreshPosition(BuildingInstance building)
		{
			if (building == null || building.view == null || this == null)
			{
				return;
			}
			IntVector2 location = BuildingLocation.GetLocation(building);
			Vector3 position = new Vector3((float)location.x, 0f, (float)location.y) + this.Pivot;
			base.SetPosition(position);
		}

		// Token: 0x0400637E RID: 25470
		protected BuildingInstance data;
	}
}
