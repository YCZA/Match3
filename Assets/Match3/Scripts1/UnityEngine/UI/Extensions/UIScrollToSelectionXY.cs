using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000C3B RID: 3131
	[AddComponentMenu("UI/Extensions/UI ScrollTo Selection XY")]
	[RequireComponent(typeof(ScrollRect))]
	public class UIScrollToSelectionXY : MonoBehaviour
	{
		// Token: 0x060049DC RID: 18908 RVA: 0x00179BAB File Offset: 0x00177FAB
		private void Start()
		{
			this.targetScrollRect = base.GetComponent<ScrollRect>();
			this.scrollWindow = this.targetScrollRect.GetComponent<RectTransform>();
		}

		// Token: 0x060049DD RID: 18909 RVA: 0x00179BCA File Offset: 0x00177FCA
		private void Update()
		{
			this.ScrollRectToLevelSelection();
		}

		// Token: 0x060049DE RID: 18910 RVA: 0x00179BD4 File Offset: 0x00177FD4
		private void ScrollRectToLevelSelection()
		{
			EventSystem current = EventSystem.current;
			bool flag = this.targetScrollRect == null || this.layoutListGroup == null || this.scrollWindow == null;
			if (flag)
			{
				return;
			}
			RectTransform rectTransform = (!(current.currentSelectedGameObject != null)) ? null : current.currentSelectedGameObject.GetComponent<RectTransform>();
			if (rectTransform != this.targetScrollObject)
			{
				this.scrollToSelection = true;
			}
			if (rectTransform == null || !this.scrollToSelection || rectTransform.transform.parent != this.layoutListGroup.transform)
			{
				return;
			}
			bool flag2 = false;
			bool flag3 = false;
			if (this.targetScrollRect.vertical)
			{
				float num = -rectTransform.anchoredPosition.y;
				float y = this.layoutListGroup.anchoredPosition.y;
				float num2 = y - num;
				this.targetScrollRect.verticalNormalizedPosition += num2 / this.layoutListGroup.sizeDelta.y * Time.deltaTime * this.scrollSpeed;
				flag3 = (Mathf.Abs(num2) < 2f);
			}
			if (this.targetScrollRect.horizontal)
			{
				float num3 = -rectTransform.anchoredPosition.x;
				float x = this.layoutListGroup.anchoredPosition.x;
				float num4 = x - num3;
				this.targetScrollRect.horizontalNormalizedPosition += num4 / this.layoutListGroup.sizeDelta.x * Time.deltaTime * this.scrollSpeed;
				flag2 = (Mathf.Abs(num4) < 2f);
			}
			if (flag2 && flag3)
			{
				this.scrollToSelection = false;
			}
			this.targetScrollObject = rectTransform;
		}

		// Token: 0x04007025 RID: 28709
		public float scrollSpeed = 10f;

		// Token: 0x04007026 RID: 28710
		[SerializeField]
		private RectTransform layoutListGroup;

		// Token: 0x04007027 RID: 28711
		private RectTransform targetScrollObject;

		// Token: 0x04007028 RID: 28712
		private bool scrollToSelection = true;

		// Token: 0x04007029 RID: 28713
		private RectTransform scrollWindow;

		// Token: 0x0400702A RID: 28714
		private RectTransform currentCanvas;

		// Token: 0x0400702B RID: 28715
		private ScrollRect targetScrollRect;
	}
}
