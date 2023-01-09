using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x020008FD RID: 2301
namespace Match3.Scripts1
{
	public class CameraDragBlocker : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IEventSystemHandler
	{
		// Token: 0x0600380A RID: 14346 RVA: 0x00111FF4 File Offset: 0x001103F4
		public void OnPointerDown(PointerEventData eventData)
		{
			CameraInputController.currentDragObject = base.gameObject;
		}

		// Token: 0x0600380B RID: 14347 RVA: 0x00112001 File Offset: 0x00110401
		public void OnPointerUp(PointerEventData eventData)
		{
			CameraInputController.currentDragObject = null;
		}
	}
}
