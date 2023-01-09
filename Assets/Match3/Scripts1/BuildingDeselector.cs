using Match3.Scripts1.Puzzletown.Flows;
using Match3.Scripts1.Puzzletown.Town;
using Match3.Scripts1.Town;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x020008F7 RID: 2295
namespace Match3.Scripts1
{
	public class BuildingDeselector : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler, IEventSystemHandler
	{
		// Token: 0x060037E9 RID: 14313 RVA: 0x0011198D File Offset: 0x0010FD8D
		public void OnPointerDown(PointerEventData evt)
		{
			this.tryingToDeselect = true;
		}

		// Token: 0x060037EA RID: 14314 RVA: 0x00111998 File Offset: 0x0010FD98
		public void OnPointerUp(PointerEventData evt)
		{
			if (this.tryingToDeselect)
			{
				if (PurchaseBuildingFlow.flowRunning)
				{
					return;
				}
				BuildingInstance selected = BuildingLocation.Selected;
				if (selected != null)
				{
					if (selected.mustBePlaced)
					{
						return;
					}
					BuildingLocation.Selected = null;
				}
				BuildingMainView.CloseLastPopup();
				Vector3 mousePosition = CameraInputController.MousePosition;
				IntVector2 intVector = IntVector2.ProjectToGridXZ(mousePosition);
				if (this.env.map)
				{
					this.env.map.onAreaClick.Dispatch(this.env.map.GetLocalArea(intVector.x, intVector.y));
				}
			}
		}

		// Token: 0x060037EB RID: 14315 RVA: 0x00111A33 File Offset: 0x0010FE33
		public void OnDrag(PointerEventData evt)
		{
			this.tryingToDeselect = false;
		}

		// Token: 0x04006013 RID: 24595
		public TownEnvironmentRoot env;

		// Token: 0x04006014 RID: 24596
		private bool tryingToDeselect;
	}
}
