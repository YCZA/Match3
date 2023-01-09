using UnityEngine;
using UnityEngine.EventSystems;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000BB2 RID: 2994
	[AddComponentMenu("UI/Extensions/RescalePanels/ResizePanel")]
	public class ResizePanel : MonoBehaviour, IPointerDownHandler, IDragHandler, IEventSystemHandler
	{
		// Token: 0x0600461B RID: 17947 RVA: 0x001639A8 File Offset: 0x00161DA8
		private void Awake()
		{
			this.rectTransform = base.transform.parent.GetComponent<RectTransform>();
			float width = this.rectTransform.rect.width;
			float height = this.rectTransform.rect.height;
			this.ratio = height / width;
			this.minSize = new Vector2(0.1f * width, 0.1f * height);
			this.maxSize = new Vector2(10f * width, 10f * height);
		}

		// Token: 0x0600461C RID: 17948 RVA: 0x00163A2E File Offset: 0x00161E2E
		public void OnPointerDown(PointerEventData data)
		{
			this.rectTransform.SetAsLastSibling();
			RectTransformUtility.ScreenPointToLocalPointInRectangle(this.rectTransform, data.position, data.pressEventCamera, out this.previousPointerPosition);
		}

		// Token: 0x0600461D RID: 17949 RVA: 0x00163A5C File Offset: 0x00161E5C
		public void OnDrag(PointerEventData data)
		{
			if (this.rectTransform == null)
			{
				return;
			}
			Vector2 vector = this.rectTransform.sizeDelta;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(this.rectTransform, data.position, data.pressEventCamera, out this.currentPointerPosition);
			Vector2 vector2 = this.currentPointerPosition - this.previousPointerPosition;
			vector += new Vector2(vector2.x, this.ratio * vector2.x);
			vector = new Vector2(Mathf.Clamp(vector.x, this.minSize.x, this.maxSize.x), Mathf.Clamp(vector.y, this.minSize.y, this.maxSize.y));
			this.rectTransform.sizeDelta = vector;
			this.previousPointerPosition = this.currentPointerPosition;
		}

		// Token: 0x04006DA3 RID: 28067
		public Vector2 minSize;

		// Token: 0x04006DA4 RID: 28068
		public Vector2 maxSize;

		// Token: 0x04006DA5 RID: 28069
		private RectTransform rectTransform;

		// Token: 0x04006DA6 RID: 28070
		private Vector2 currentPointerPosition;

		// Token: 0x04006DA7 RID: 28071
		private Vector2 previousPointerPosition;

		// Token: 0x04006DA8 RID: 28072
		private float ratio;
	}
}
