using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000901 RID: 2305
namespace Match3.Scripts1
{
	public class CameraInputRedirect : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IEventSystemHandler
	{
		// Token: 0x0600383C RID: 14396 RVA: 0x00112E94 File Offset: 0x00111294
		public void OnBeginDrag(PointerEventData evt)
		{
			if (!CameraInputController.current)
			{
				return;
			}
			CameraInputController.current.OnBeginDrag(evt);
		}

		// Token: 0x0600383D RID: 14397 RVA: 0x00112EB1 File Offset: 0x001112B1
		public void OnEndDrag(PointerEventData evt)
		{
			if (!CameraInputController.current)
			{
				return;
			}
			CameraInputController.current.OnEndDrag(evt);
		}

		// Token: 0x0600383E RID: 14398 RVA: 0x00112ECE File Offset: 0x001112CE
		public void OnDrag(PointerEventData evt)
		{
			if (!CameraInputController.current)
			{
				return;
			}
			CameraInputController.current.OnDrag(evt);
		}
	}
}
