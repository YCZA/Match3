using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000C16 RID: 3094
	[RequireComponent(typeof(RectTransform))]
	[AddComponentMenu("UI/Extensions/UI Window Base")]
	public class UIWindowBase : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IEventSystemHandler
	{
		// Token: 0x060048F6 RID: 18678 RVA: 0x0017560C File Offset: 0x00173A0C
		private void Start()
		{
			this.m_transform = base.GetComponent<RectTransform>();
			this.m_originalCoods = this.m_transform.position;
			this.m_canvas = base.GetComponentInParent<Canvas>();
			this.m_canvasRectTransform = this.m_canvas.GetComponent<RectTransform>();
		}

		// Token: 0x060048F7 RID: 18679 RVA: 0x00175648 File Offset: 0x00173A48
		private void Update()
		{
			if (UIWindowBase.ResetCoords)
			{
				this.resetCoordinatePosition();
			}
		}

		// Token: 0x060048F8 RID: 18680 RVA: 0x0017565C File Offset: 0x00173A5C
		public void OnDrag(PointerEventData eventData)
		{
			if (this._isDragging)
			{
				Vector3 b = this.ScreenToCanvas(eventData.position) - this.ScreenToCanvas(eventData.position - eventData.delta);
				this.m_transform.localPosition += b;
			}
		}

		// Token: 0x060048F9 RID: 18681 RVA: 0x001756C0 File Offset: 0x00173AC0
		public void OnBeginDrag(PointerEventData eventData)
		{
			if (eventData.pointerCurrentRaycast.gameObject == null)
			{
				return;
			}
			if (eventData.pointerCurrentRaycast.gameObject.name == base.name)
			{
				this._isDragging = true;
			}
		}

		// Token: 0x060048FA RID: 18682 RVA: 0x00175711 File Offset: 0x00173B11
		public void OnEndDrag(PointerEventData eventData)
		{
			this._isDragging = false;
		}

		// Token: 0x060048FB RID: 18683 RVA: 0x0017571A File Offset: 0x00173B1A
		private void resetCoordinatePosition()
		{
			this.m_transform.position = this.m_originalCoods;
			UIWindowBase.ResetCoords = false;
		}

		// Token: 0x060048FC RID: 18684 RVA: 0x00175734 File Offset: 0x00173B34
		private Vector3 ScreenToCanvas(Vector3 screenPosition)
		{
			Vector2 sizeDelta = this.m_canvasRectTransform.sizeDelta;
			Vector3 result;
			Vector2 vector;
			Vector2 vector2;
			if (this.m_canvas.renderMode == RenderMode.ScreenSpaceOverlay || (this.m_canvas.renderMode == RenderMode.ScreenSpaceCamera && this.m_canvas.worldCamera == null))
			{
				result = screenPosition;
				vector = Vector2.zero;
				vector2 = sizeDelta;
			}
			else
			{
				Ray ray = this.m_canvas.worldCamera.ScreenPointToRay(screenPosition);
				Plane plane = new Plane(this.m_canvasRectTransform.forward, this.m_canvasRectTransform.position);
				float d;
				if (!plane.Raycast(ray, out d))
				{
					throw new Exception("Is it practically possible?");
				}
				Vector3 position = ray.origin + ray.direction * d;
				result = this.m_canvasRectTransform.InverseTransformPoint(position);
				vector = -Vector2.Scale(sizeDelta, this.m_canvasRectTransform.pivot);
				vector2 = Vector2.Scale(sizeDelta, Vector2.one - this.m_canvasRectTransform.pivot);
			}
			result.x = Mathf.Clamp(result.x, vector.x + (float)this.KeepWindowInCanvas, vector2.x - (float)this.KeepWindowInCanvas);
			result.y = Mathf.Clamp(result.y, vector.y + (float)this.KeepWindowInCanvas, vector2.y - (float)this.KeepWindowInCanvas);
			return result;
		}

		// Token: 0x04006F95 RID: 28565
		private RectTransform m_transform;

		// Token: 0x04006F96 RID: 28566
		private bool _isDragging;

		// Token: 0x04006F97 RID: 28567
		public static bool ResetCoords;

		// Token: 0x04006F98 RID: 28568
		private Vector3 m_originalCoods = Vector3.zero;

		// Token: 0x04006F99 RID: 28569
		private Canvas m_canvas;

		// Token: 0x04006F9A RID: 28570
		private RectTransform m_canvasRectTransform;

		// Token: 0x04006F9B RID: 28571
		public int KeepWindowInCanvas = 5;
	}
}
