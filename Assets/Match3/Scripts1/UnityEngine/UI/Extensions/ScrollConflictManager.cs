using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000C29 RID: 3113
	[RequireComponent(typeof(ScrollRect))]
	[AddComponentMenu("UI/Extensions/Scrollrect Conflict Manager")]
	public class ScrollConflictManager : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IEventSystemHandler
	{
		// Token: 0x06004971 RID: 18801 RVA: 0x00177504 File Offset: 0x00175904
		private void Awake()
		{
			this._myScrollRect = base.GetComponent<ScrollRect>();
			this.scrollOtherHorizontally = this._myScrollRect.vertical;
			if (this.scrollOtherHorizontally)
			{
				if (this._myScrollRect.horizontal)
				{
					global::UnityEngine.Debug.Log("You have added the SecondScrollRect to a scroll view that already has both directions selected");
				}
				if (!this.ParentScrollRect.horizontal)
				{
					global::UnityEngine.Debug.Log("The other scroll rect doesnt support scrolling horizontally");
				}
			}
			else if (!this.ParentScrollRect.vertical)
			{
				global::UnityEngine.Debug.Log("The other scroll rect doesnt support scrolling vertically");
			}
		}

		// Token: 0x06004972 RID: 18802 RVA: 0x0017758C File Offset: 0x0017598C
		public void OnBeginDrag(PointerEventData eventData)
		{
			float num = Mathf.Abs(eventData.position.x - eventData.pressPosition.x);
			float num2 = Mathf.Abs(eventData.position.y - eventData.pressPosition.y);
			if (this.scrollOtherHorizontally)
			{
				if (num > num2)
				{
					this.scrollOther = true;
					this._myScrollRect.enabled = false;
					this.ParentScrollRect.OnBeginDrag(eventData);
				}
			}
			else if (num2 > num)
			{
				this.scrollOther = true;
				this._myScrollRect.enabled = false;
				this.ParentScrollRect.OnBeginDrag(eventData);
			}
		}

		// Token: 0x06004973 RID: 18803 RVA: 0x0017763D File Offset: 0x00175A3D
		public void OnEndDrag(PointerEventData eventData)
		{
			if (this.scrollOther)
			{
				this.scrollOther = false;
				this._myScrollRect.enabled = true;
				this.ParentScrollRect.OnEndDrag(eventData);
			}
		}

		// Token: 0x06004974 RID: 18804 RVA: 0x00177669 File Offset: 0x00175A69
		public void OnDrag(PointerEventData eventData)
		{
			if (this.scrollOther)
			{
				this.ParentScrollRect.OnDrag(eventData);
			}
		}

		// Token: 0x04006FCF RID: 28623
		public ScrollRect ParentScrollRect;

		// Token: 0x04006FD0 RID: 28624
		private ScrollRect _myScrollRect;

		// Token: 0x04006FD1 RID: 28625
		private bool scrollOther;

		// Token: 0x04006FD2 RID: 28626
		private bool scrollOtherHorizontally;
	}
}
