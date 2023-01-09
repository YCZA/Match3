using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000BAF RID: 2991
	[RequireComponent(typeof(RectTransform))]
	public class ReorderableListElement : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IEventSystemHandler
	{
		// Token: 0x0600460B RID: 17931 RVA: 0x00162CAC File Offset: 0x001610AC
		public void OnBeginDrag(PointerEventData eventData)
		{
			this.isValid = true;
			if (this._reorderableList == null)
			{
				return;
			}
			if (!this._reorderableList.IsDraggable || !this.IsGrabbable)
			{
				this._draggingObject = null;
				return;
			}
			if (!this._reorderableList.CloneDraggedObject)
			{
				this._draggingObject = this._rect;
				this._fromIndex = this._rect.GetSiblingIndex();
				if (this._reorderableList.OnElementRemoved != null)
				{
					this._reorderableList.OnElementRemoved.Invoke(new ReorderableList.ReorderableListEventStruct
					{
						DroppedObject = this._draggingObject.gameObject,
						IsAClone = this._reorderableList.CloneDraggedObject,
						SourceObject = ((!this._reorderableList.CloneDraggedObject) ? this._draggingObject.gameObject : base.gameObject),
						FromList = this._reorderableList,
						FromIndex = this._fromIndex
					});
				}
				if (!this.isValid)
				{
					this._draggingObject = null;
					return;
				}
			}
			else
			{
				GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(base.gameObject);
				this._draggingObject = gameObject.GetComponent<RectTransform>();
			}
			this._draggingObjectOriginalSize = base.gameObject.GetComponent<RectTransform>().rect.size;
			this._draggingObjectLE = this._draggingObject.GetComponent<LayoutElement>();
			this._draggingObject.SetParent(this._reorderableList.DraggableArea, true);
			this._draggingObject.SetAsLastSibling();
			this._fakeElement = new GameObject("Fake").AddComponent<RectTransform>();
			this._fakeElementLE = this._fakeElement.gameObject.AddComponent<LayoutElement>();
			this.RefreshSizes();
			if (this._reorderableList.OnElementGrabbed != null)
			{
				this._reorderableList.OnElementGrabbed.Invoke(new ReorderableList.ReorderableListEventStruct
				{
					DroppedObject = this._draggingObject.gameObject,
					IsAClone = this._reorderableList.CloneDraggedObject,
					SourceObject = ((!this._reorderableList.CloneDraggedObject) ? this._draggingObject.gameObject : base.gameObject),
					FromList = this._reorderableList,
					FromIndex = this._fromIndex
				});
				if (!this.isValid)
				{
					this.CancelDrag();
					return;
				}
			}
			this._isDragging = true;
		}

		// Token: 0x0600460C RID: 17932 RVA: 0x00162F18 File Offset: 0x00161318
		public void OnDrag(PointerEventData eventData)
		{
			if (!this._isDragging)
			{
				return;
			}
			if (!this.isValid)
			{
				this.CancelDrag();
				return;
			}
			Canvas componentInParent = this._draggingObject.GetComponentInParent<Canvas>();
			Vector3 position;
			RectTransformUtility.ScreenPointToWorldPointInRectangle(componentInParent.GetComponent<RectTransform>(), eventData.position, componentInParent.worldCamera, out position);
			this._draggingObject.position = position;
			EventSystem.current.RaycastAll(eventData, this._raycastResults);
			for (int i = 0; i < this._raycastResults.Count; i++)
			{
				this._currentReorderableListRaycasted = this._raycastResults[i].gameObject.GetComponent<ReorderableList>();
				if (this._currentReorderableListRaycasted != null)
				{
					break;
				}
			}
			if (this._currentReorderableListRaycasted == null || !this._currentReorderableListRaycasted.IsDropable)
			{
				this.RefreshSizes();
				this._fakeElement.transform.SetParent(this._reorderableList.DraggableArea, false);
			}
			else
			{
				if (this._fakeElement.parent != this._currentReorderableListRaycasted)
				{
					this._fakeElement.SetParent(this._currentReorderableListRaycasted.Content, false);
				}
				float num = float.PositiveInfinity;
				int siblingIndex = 0;
				float num2 = 0f;
				for (int j = 0; j < this._currentReorderableListRaycasted.Content.childCount; j++)
				{
					RectTransform component = this._currentReorderableListRaycasted.Content.GetChild(j).GetComponent<RectTransform>();
					if (this._currentReorderableListRaycasted.ContentLayout is VerticalLayoutGroup)
					{
						num2 = Mathf.Abs(component.position.y - position.y);
					}
					else if (this._currentReorderableListRaycasted.ContentLayout is HorizontalLayoutGroup)
					{
						num2 = Mathf.Abs(component.position.x - position.x);
					}
					else if (this._currentReorderableListRaycasted.ContentLayout is GridLayoutGroup)
					{
						num2 = Mathf.Abs(component.position.x - position.x) + Mathf.Abs(component.position.y - position.y);
					}
					if (num2 < num)
					{
						num = num2;
						siblingIndex = j;
					}
				}
				this.RefreshSizes();
				this._fakeElement.SetSiblingIndex(siblingIndex);
				this._fakeElement.gameObject.SetActive(true);
			}
		}

		// Token: 0x0600460D RID: 17933 RVA: 0x0016319C File Offset: 0x0016159C
		public void OnEndDrag(PointerEventData eventData)
		{
			this._isDragging = false;
			if (this._draggingObject != null)
			{
				if (this._currentReorderableListRaycasted != null && this._currentReorderableListRaycasted.IsDropable && (this.IsTransferable || this._currentReorderableListRaycasted == this._reorderableList))
				{
					ReorderableList.ReorderableListEventStruct reorderableListEventStruct = new ReorderableList.ReorderableListEventStruct
					{
						DroppedObject = this._draggingObject.gameObject,
						IsAClone = this._reorderableList.CloneDraggedObject,
						SourceObject = ((!this._reorderableList.CloneDraggedObject) ? this._draggingObject.gameObject : base.gameObject),
						FromList = this._reorderableList,
						FromIndex = this._fromIndex,
						ToList = this._currentReorderableListRaycasted,
						ToIndex = this._fakeElement.GetSiblingIndex()
					};
					ReorderableList.ReorderableListEventStruct arg = reorderableListEventStruct;
					if (this._reorderableList && this._reorderableList.OnElementDropped != null)
					{
						this._reorderableList.OnElementDropped.Invoke(arg);
					}
					if (!this.isValid)
					{
						this.CancelDrag();
						return;
					}
					this.RefreshSizes();
					this._draggingObject.SetParent(this._currentReorderableListRaycasted.Content, false);
					this._draggingObject.rotation = this._currentReorderableListRaycasted.transform.rotation;
					this._draggingObject.SetSiblingIndex(this._fakeElement.GetSiblingIndex());
					this._reorderableList.OnElementAdded.Invoke(arg);
					if (!this.isValid)
					{
						throw new Exception("It's too late to cancel the Transfer! Do so in OnElementDropped!");
					}
				}
				else if (this.isDroppableInSpace)
				{
					UnityEvent<ReorderableList.ReorderableListEventStruct> onElementDropped = this._reorderableList.OnElementDropped;
					ReorderableList.ReorderableListEventStruct reorderableListEventStruct = new ReorderableList.ReorderableListEventStruct
					{
						DroppedObject = this._draggingObject.gameObject,
						IsAClone = this._reorderableList.CloneDraggedObject,
						SourceObject = ((!this._reorderableList.CloneDraggedObject) ? this._draggingObject.gameObject : base.gameObject),
						FromList = this._reorderableList,
						FromIndex = this._fromIndex
					};
					onElementDropped.Invoke(reorderableListEventStruct);
				}
				else
				{
					this.CancelDrag();
				}
			}
			if (this._fakeElement != null)
			{
				global::UnityEngine.Object.Destroy(this._fakeElement.gameObject);
			}
		}

		// Token: 0x0600460E RID: 17934 RVA: 0x00163410 File Offset: 0x00161810
		private void CancelDrag()
		{
			this._isDragging = false;
			if (this._reorderableList.CloneDraggedObject)
			{
				global::UnityEngine.Object.Destroy(this._draggingObject.gameObject);
			}
			else
			{
				this.RefreshSizes();
				this._draggingObject.SetParent(this._reorderableList.Content, false);
				this._draggingObject.rotation = this._reorderableList.Content.transform.rotation;
				this._draggingObject.SetSiblingIndex(this._fromIndex);
				ReorderableList.ReorderableListEventStruct arg = new ReorderableList.ReorderableListEventStruct
				{
					DroppedObject = this._draggingObject.gameObject,
					IsAClone = this._reorderableList.CloneDraggedObject,
					SourceObject = ((!this._reorderableList.CloneDraggedObject) ? this._draggingObject.gameObject : base.gameObject),
					FromList = this._reorderableList,
					FromIndex = this._fromIndex,
					ToList = this._reorderableList,
					ToIndex = this._fromIndex
				};
				this._reorderableList.OnElementAdded.Invoke(arg);
				if (!this.isValid)
				{
					throw new Exception("Transfer is already Cancelled.");
				}
			}
			if (this._fakeElement != null)
			{
				global::UnityEngine.Object.Destroy(this._fakeElement.gameObject);
			}
		}

		// Token: 0x0600460F RID: 17935 RVA: 0x00163570 File Offset: 0x00161970
		private void RefreshSizes()
		{
			Vector2 sizeDelta = this._draggingObjectOriginalSize;
			if (this._currentReorderableListRaycasted != null && this._currentReorderableListRaycasted.IsDropable && this._currentReorderableListRaycasted.Content.childCount > 0)
			{
				Transform child = this._currentReorderableListRaycasted.Content.GetChild(0);
				if (child != null)
				{
					sizeDelta = child.GetComponent<RectTransform>().rect.size;
				}
			}
			this._draggingObject.sizeDelta = sizeDelta;
			LayoutElement fakeElementLE = this._fakeElementLE;
			float num = sizeDelta.y;
			this._draggingObjectLE.preferredHeight = num;
			fakeElementLE.preferredHeight = num;
			LayoutElement fakeElementLE2 = this._fakeElementLE;
			num = sizeDelta.x;
			this._draggingObjectLE.preferredWidth = num;
			fakeElementLE2.preferredWidth = num;
		}

		// Token: 0x06004610 RID: 17936 RVA: 0x00163639 File Offset: 0x00161A39
		public void Init(ReorderableList reorderableList)
		{
			this._reorderableList = reorderableList;
			this._rect = base.GetComponent<RectTransform>();
		}

		// Token: 0x04006D88 RID: 28040
		[Tooltip("Can this element be dragged?")]
		public bool IsGrabbable = true;

		// Token: 0x04006D89 RID: 28041
		[Tooltip("Can this element be transfered to another list")]
		public bool IsTransferable = true;

		// Token: 0x04006D8A RID: 28042
		[Tooltip("Can this element be dropped in space?")]
		public bool isDroppableInSpace;

		// Token: 0x04006D8B RID: 28043
		private readonly List<RaycastResult> _raycastResults = new List<RaycastResult>();

		// Token: 0x04006D8C RID: 28044
		private ReorderableList _currentReorderableListRaycasted;

		// Token: 0x04006D8D RID: 28045
		private RectTransform _draggingObject;

		// Token: 0x04006D8E RID: 28046
		private LayoutElement _draggingObjectLE;

		// Token: 0x04006D8F RID: 28047
		private Vector2 _draggingObjectOriginalSize;

		// Token: 0x04006D90 RID: 28048
		private RectTransform _fakeElement;

		// Token: 0x04006D91 RID: 28049
		private LayoutElement _fakeElementLE;

		// Token: 0x04006D92 RID: 28050
		private int _fromIndex;

		// Token: 0x04006D93 RID: 28051
		private bool _isDragging;

		// Token: 0x04006D94 RID: 28052
		private RectTransform _rect;

		// Token: 0x04006D95 RID: 28053
		private ReorderableList _reorderableList;

		// Token: 0x04006D96 RID: 28054
		internal bool isValid;
	}
}
