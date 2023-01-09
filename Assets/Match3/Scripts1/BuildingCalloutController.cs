using Match3.Scripts1.Puzzletown.Town;
using Match3.Scripts1.Town;
using UnityEngine;

// Token: 0x0200095D RID: 2397
namespace Match3.Scripts1
{
	public class BuildingCalloutController : MonoBehaviour
	{
		// Token: 0x06003A85 RID: 14981 RVA: 0x00121E14 File Offset: 0x00120214
		public void Refresh(BuildingInstance building, TownOverheadUiRoot root)
		{
			if (root == null || !root.IsSetup)
			{
				return;
			}
			if (this.lastState != building.State)
			{
				this.lastState = building.State;
				if (this.callout == null && this.lastState == BuildingState.Harvestable)
				{
					this.callout = global::UnityEngine.Object.Instantiate<BuildingUiHarvestCallout>(root.harvestCallout, root.harvestCallout.transform.parent, false);
					this.callout.AssignTownOverheadUiRoot(root);
				}
				if (this.callout != null)
				{
					this.callout.Show(building);
				}
			}
		}

		// Token: 0x04006268 RID: 25192
		private BuildingState lastState;

		// Token: 0x04006269 RID: 25193
		private ABuildingUiStateCallout callout;
	}
}
