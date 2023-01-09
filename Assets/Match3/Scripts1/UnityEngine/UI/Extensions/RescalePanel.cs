using UnityEngine;
using UnityEngine.EventSystems;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000BB1 RID: 2993
	[AddComponentMenu("UI/Extensions/RescalePanels/RescalePanel")]
	public class RescalePanel : MonoBehaviour, IPointerDownHandler, IDragHandler, IEventSystemHandler
	{
		// Token: 0x06004617 RID: 17943 RVA: 0x001637FC File Offset: 0x00161BFC
		private void Awake()
		{
			this.rectTransform = base.transform.parent.GetComponent<RectTransform>();
			this.goTransform = base.transform.parent;
			this.thisRectTransform = base.GetComponent<RectTransform>();
			this.sizeDelta = this.thisRectTransform.sizeDelta;
		}

		// Token: 0x06004618 RID: 17944 RVA: 0x0016384D File Offset: 0x00161C4D
		public void OnPointerDown(PointerEventData data)
		{
			this.rectTransform.SetAsLastSibling();
			RectTransformUtility.ScreenPointToLocalPointInRectangle(this.rectTransform, data.position, data.pressEventCamera, out this.previousPointerPosition);
		}

		// Token: 0x06004619 RID: 17945 RVA: 0x00163878 File Offset: 0x00161C78
		public void OnDrag(PointerEventData data)
		{
			if (this.rectTransform == null)
			{
				return;
			}
			Vector3 vector = this.goTransform.localScale;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(this.rectTransform, data.position, data.pressEventCamera, out this.currentPointerPosition);
			Vector2 vector2 = this.currentPointerPosition - this.previousPointerPosition;
			vector += new Vector3(-vector2.y * 0.001f, -vector2.y * 0.001f, 0f);
			vector = new Vector3(Mathf.Clamp(vector.x, this.minSize.x, this.maxSize.x), Mathf.Clamp(vector.y, this.minSize.y, this.maxSize.y), 1f);
			this.goTransform.localScale = vector;
			this.previousPointerPosition = this.currentPointerPosition;
			float num = this.sizeDelta.x / this.goTransform.localScale.x;
			Vector2 vector3 = new Vector2(num, num);
			this.thisRectTransform.sizeDelta = vector3;
		}

		// Token: 0x04006D9B RID: 28059
		public Vector2 minSize;

		// Token: 0x04006D9C RID: 28060
		public Vector2 maxSize;

		// Token: 0x04006D9D RID: 28061
		private RectTransform rectTransform;

		// Token: 0x04006D9E RID: 28062
		private Transform goTransform;

		// Token: 0x04006D9F RID: 28063
		private Vector2 currentPointerPosition;

		// Token: 0x04006DA0 RID: 28064
		private Vector2 previousPointerPosition;

		// Token: 0x04006DA1 RID: 28065
		private RectTransform thisRectTransform;

		// Token: 0x04006DA2 RID: 28066
		private Vector2 sizeDelta;
	}
}
