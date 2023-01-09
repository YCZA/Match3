using System.Collections.Generic;
using Match3.Scripts1.Wooga.Signals;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000899 RID: 2201
namespace Match3.Scripts1
{
	public class EventProxy : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, IDragHandler, IPointerEnterHandler, IBeginDragHandler, IEventSystemHandler
	{
		// Token: 0x060035E8 RID: 13800 RVA: 0x001036C6 File Offset: 0x00101AC6
		public void OnPointerDown(PointerEventData eventData)
		{
			this.pointerPress = this.GetHovered(eventData);
			eventData.pointerPress = this.pointerPress;
			if (!this.altInputMode)
			{
				this.Redirect<IPointerDownHandler>(eventData, ExecuteEvents.pointerDownHandler);
			}
		}

		// Token: 0x060035E9 RID: 13801 RVA: 0x001036F8 File Offset: 0x00101AF8
		public void OnPointerUp(PointerEventData eventData)
		{
			if (!this.altInputMode)
			{
				this.Redirect<IPointerUpHandler>(eventData, ExecuteEvents.pointerUpHandler);
			}
		}

		// Token: 0x060035EA RID: 13802 RVA: 0x00103714 File Offset: 0x00101B14
		public void OnPointerClick(PointerEventData eventData)
		{
			if (this.pointerPress == this.GetHovered(eventData))
			{
				this.onClicked.Dispatch();
				if (!this.altInputMode)
				{
					this.Redirect<IPointerClickHandler>(eventData, ExecuteEvents.pointerClickHandler);
				}
				else
				{
					this.Redirect<IPointerDownHandler>(eventData, ExecuteEvents.pointerDownHandler);
					this.Redirect<IPointerUpHandler>(eventData, ExecuteEvents.pointerUpHandler);
				}
			}
		}

		// Token: 0x060035EB RID: 13803 RVA: 0x00103777 File Offset: 0x00101B77
		public void OnDrag(PointerEventData eventData)
		{
			if (!this.isRedirectingDrag)
			{
				return;
			}
			this.onClicked.Dispatch();
			this.Redirect<IDragHandler>(eventData, ExecuteEvents.dragHandler);
		}

		// Token: 0x060035EC RID: 13804 RVA: 0x0010379C File Offset: 0x00101B9C
		public void OnBeginDrag(PointerEventData eventData)
		{
			if (!this.isRedirectingDrag)
			{
				return;
			}
			eventData.pointerDrag = this.pointerPress;
			eventData.pointerPress = this.pointerPress;
		}

		// Token: 0x060035ED RID: 13805 RVA: 0x001037C4 File Offset: 0x00101BC4
		public void OnPointerEnter(PointerEventData eventData)
		{
			if (this.isPointerEnterStartingDrag && eventData.pointerPress)
			{
				GameObject hovered = this.GetHovered(eventData);
				eventData.pointerEnter = hovered;
				this.Redirect<IPointerEnterHandler>(eventData, ExecuteEvents.pointerEnterHandler);
			}
		}

		// Token: 0x060035EE RID: 13806 RVA: 0x00103808 File Offset: 0x00101C08
		private void Redirect<T>(PointerEventData eventData, ExecuteEvents.EventFunction<T> func) where T : IEventSystemHandler
		{
			if (!this.isRedirecting)
			{
				return;
			}
			GameObject hovered = this.GetHovered(eventData);
			if (this.CanPress(hovered))
			{
				ExecuteEvents.ExecuteHierarchy<T>(hovered, eventData, func);
				this.lockToObject = null;
			}
		}

		// Token: 0x060035EF RID: 13807 RVA: 0x00103848 File Offset: 0x00101C48
		private GameObject GetHovered(PointerEventData eventData)
		{
			EventSystem.current.RaycastAll(eventData, this.results);
			int num = this.results.FindIndex((RaycastResult r) => r.gameObject == this.blocker) + 1;
			return (num >= this.results.Count) ? null : this.results[num].gameObject;
		}

		// Token: 0x060035F0 RID: 13808 RVA: 0x001038AB File Offset: 0x00101CAB
		private bool CanPress(GameObject selected)
		{
			return !this.lockToObject || selected == this.lockToObject;
		}

		// Token: 0x04005DD6 RID: 24022
		public readonly Signal onClicked = new Signal();

		// Token: 0x04005DD7 RID: 24023
		[SerializeField]
		private GameObject blocker;

		// Token: 0x04005DD8 RID: 24024
		[HideInInspector]
		public GameObject lockToObject;

		// Token: 0x04005DD9 RID: 24025
		public bool isRedirecting;

		// Token: 0x04005DDA RID: 24026
		public bool isPointerEnterStartingDrag;

		// Token: 0x04005DDB RID: 24027
		public bool isRedirectingDrag;

		// Token: 0x04005DDC RID: 24028
		private GameObject pointerPress;

		// Token: 0x04005DDD RID: 24029
		private List<RaycastResult> results = new List<RaycastResult>();

		// Token: 0x04005DDE RID: 24030
		public bool altInputMode;
	}
}
