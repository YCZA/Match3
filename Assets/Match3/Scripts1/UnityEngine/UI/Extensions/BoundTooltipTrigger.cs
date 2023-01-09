using UnityEngine;
using UnityEngine.EventSystems;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000C13 RID: 3091
	[AddComponentMenu("UI/Extensions/Bound Tooltip/Tooltip Trigger")]
	public class BoundTooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler, IEventSystemHandler
	{
		// Token: 0x060048DD RID: 18653 RVA: 0x00174874 File Offset: 0x00172C74
		public void OnPointerEnter(PointerEventData eventData)
		{
			if (this.useMousePosition)
			{
				this.StartHover(new Vector3(eventData.position.x, eventData.position.y, 0f));
			}
			else
			{
				this.StartHover(base.transform.position + this.offset);
			}
		}

		// Token: 0x060048DE RID: 18654 RVA: 0x001748D9 File Offset: 0x00172CD9
		public void OnSelect(BaseEventData eventData)
		{
			this.StartHover(base.transform.position);
		}

		// Token: 0x060048DF RID: 18655 RVA: 0x001748EC File Offset: 0x00172CEC
		public void OnPointerExit(PointerEventData eventData)
		{
			this.StopHover();
		}

		// Token: 0x060048E0 RID: 18656 RVA: 0x001748F4 File Offset: 0x00172CF4
		public void OnDeselect(BaseEventData eventData)
		{
			this.StopHover();
		}

		// Token: 0x060048E1 RID: 18657 RVA: 0x001748FC File Offset: 0x00172CFC
		private void StartHover(Vector3 position)
		{
			BoundTooltipItem.Instance.ShowTooltip(this.text, position);
		}

		// Token: 0x060048E2 RID: 18658 RVA: 0x0017490F File Offset: 0x00172D0F
		private void StopHover()
		{
			BoundTooltipItem.Instance.HideTooltip();
		}

		// Token: 0x04006F77 RID: 28535
		[TextArea]
		public string text;

		// Token: 0x04006F78 RID: 28536
		public bool useMousePosition;

		// Token: 0x04006F79 RID: 28537
		public Vector3 offset;
	}
}
