using UnityEngine;
using UnityEngine.EventSystems;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000BB0 RID: 2992
	[AddComponentMenu("UI/Extensions/RescalePanels/RescaleDragPanel")]
	public class RescaleDragPanel : MonoBehaviour, IPointerDownHandler, IDragHandler, IEventSystemHandler
	{
		// Token: 0x06004612 RID: 17938 RVA: 0x00163658 File Offset: 0x00161A58
		private void Awake()
		{
			Canvas componentInParent = base.GetComponentInParent<Canvas>();
			if (componentInParent != null)
			{
				this.canvasRectTransform = (componentInParent.transform as RectTransform);
				this.panelRectTransform = (base.transform.parent as RectTransform);
				this.goTransform = base.transform.parent;
			}
		}

		// Token: 0x06004613 RID: 17939 RVA: 0x001636B0 File Offset: 0x00161AB0
		public void OnPointerDown(PointerEventData data)
		{
			this.panelRectTransform.SetAsLastSibling();
			RectTransformUtility.ScreenPointToLocalPointInRectangle(this.panelRectTransform, data.position, data.pressEventCamera, out this.pointerOffset);
		}

		// Token: 0x06004614 RID: 17940 RVA: 0x001636DC File Offset: 0x00161ADC
		public void OnDrag(PointerEventData data)
		{
			if (this.panelRectTransform == null)
			{
				return;
			}
			Vector2 screenPoint = this.ClampToWindow(data);
			Vector2 a;
			if (RectTransformUtility.ScreenPointToLocalPointInRectangle(this.canvasRectTransform, screenPoint, data.pressEventCamera, out a))
			{
				this.panelRectTransform.localPosition = a - new Vector2(this.pointerOffset.x * this.goTransform.localScale.x, this.pointerOffset.y * this.goTransform.localScale.y);
			}
		}

		// Token: 0x06004615 RID: 17941 RVA: 0x00163778 File Offset: 0x00161B78
		private Vector2 ClampToWindow(PointerEventData data)
		{
			Vector2 position = data.position;
			Vector3[] array = new Vector3[4];
			this.canvasRectTransform.GetWorldCorners(array);
			float x = Mathf.Clamp(position.x, array[0].x, array[2].x);
			float y = Mathf.Clamp(position.y, array[0].y, array[2].y);
			Vector2 result = new Vector2(x, y);
			return result;
		}

		// Token: 0x04006D97 RID: 28055
		private Vector2 pointerOffset;

		// Token: 0x04006D98 RID: 28056
		private RectTransform canvasRectTransform;

		// Token: 0x04006D99 RID: 28057
		private RectTransform panelRectTransform;

		// Token: 0x04006D9A RID: 28058
		private Transform goTransform;
	}
}
