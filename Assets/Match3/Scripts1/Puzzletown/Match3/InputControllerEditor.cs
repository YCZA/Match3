using Match3.Scripts1.Wooga.Signals;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x0200055D RID: 1373
	public class InputControllerEditor : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IEventSystemHandler
	{
		// Token: 0x06002412 RID: 9234 RVA: 0x000A0640 File Offset: 0x0009EA40
		public void OnPointerEnter(PointerEventData eventData)
		{
			if (eventData.pointerPress)
			{
				this.GetFieldViewAndClick(eventData.pointerEnter);
			}
		}

		// Token: 0x06002413 RID: 9235 RVA: 0x000A065E File Offset: 0x0009EA5E
		public void OnPointerDown(PointerEventData eventData)
		{
			if (eventData.pointerEnter)
			{
				this.GetFieldViewAndClick(eventData.pointerEnter);
			}
		}

		// Token: 0x06002414 RID: 9236 RVA: 0x000A067C File Offset: 0x0009EA7C
		private void GetFieldViewAndClick(GameObject go)
		{
			FieldView component = go.GetComponent<FieldView>();
			IntVector2 value = new IntVector2(component.GridPosition);
			this.onClicked.Dispatch(value);
		}

		// Token: 0x04004F8C RID: 20364
		public readonly Signal<IntVector2> onClicked = new Signal<IntVector2>();
	}
}
