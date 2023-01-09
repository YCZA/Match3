using System.Collections;
using Match3.Scripts1.Town;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Tutorials
{
	// Token: 0x02000A82 RID: 2690
	[CreateAssetMenu(fileName = "ScriptTutorialPlaceBuilding", menuName = "Puzzletown/Tutorials/Create/TutorialPlaceBuilding")]
	public class TutorialPlaceBuilding : ATutorialScript
	{
		// Token: 0x06004031 RID: 16433 RVA: 0x0014A3FC File Offset: 0x001487FC
		protected override IEnumerator ExecuteRoutine()
		{
			while (TutorialPlaceBuilding.previouslySelected == BuildingLocation.Selected || BuildingLocation.Selected == null || !Camera.main)
			{
				yield return null;
			}
			TutorialPlaceBuilding.previouslySelected = BuildingLocation.Selected;
			BuildingLocation.Selected.trackingDetail = "tutorial";
			IntVector2 availablePosition = BuildingPlacementHelper.FindEmptySpotFor(BuildingLocation.Selected, this.gridPosition, 100, 90);
			BuildingLocation.Selected.position = availablePosition;
			BuildingLocation.Selected.view.Position = availablePosition;
			BuildingLocation.Selected.onPositionChanged.Dispatch(BuildingLocation.Selected);
			BuildingLocation.Selected.view.controlButtons.Show(BuildingLocation.Selected, BuildingOperation.Confirm);
			BuildingLocation.Selected.mustBePlaced = true;
			BuildingLocation.Selected.cancelBatchMode = this.cancelBatchMode;
			CameraPanManager panManager = Camera.main.GetComponent<CameraPanManager>();
			while (panManager.isPanning)
			{
				yield return null;
			}
			panManager.PanTo(BuildingLocation.Selected.view.FocusPosition, 0.75f, true, 1f);
			yield break;
		}

		// Token: 0x040069DB RID: 27099
		public IntVector2 gridPosition;

		// Token: 0x040069DC RID: 27100
		public bool cancelBatchMode;

		// Token: 0x040069DD RID: 27101
		private static BuildingInstance previouslySelected;
	}
}
