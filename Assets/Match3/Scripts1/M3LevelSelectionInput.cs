using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x020006F7 RID: 1783
namespace Match3.Scripts1
{
	[RequireComponent(typeof(Collider))]
	public class M3LevelSelectionInput : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x06002C43 RID: 11331 RVA: 0x000CBFEE File Offset: 0x000CA3EE
		private void Awake()
		{
			this._collider = base.GetComponent<Collider>();
		}

		// Token: 0x06002C44 RID: 11332 RVA: 0x000CBFFC File Offset: 0x000CA3FC
		public void OnBeginDrag(PointerEventData evt)
		{
			this.Redirect<IBeginDragHandler>(evt, ExecuteEvents.beginDragHandler);
		}

		// Token: 0x06002C45 RID: 11333 RVA: 0x000CC00A File Offset: 0x000CA40A
		public void OnEndDrag(PointerEventData evt)
		{
			this.Redirect<IEndDragHandler>(evt, ExecuteEvents.endDragHandler);
		}

		// Token: 0x06002C46 RID: 11334 RVA: 0x000CC018 File Offset: 0x000CA418
		public void OnDrag(PointerEventData evt)
		{
			this.Redirect<IDragHandler>(evt, ExecuteEvents.dragHandler);
		}

		// Token: 0x06002C47 RID: 11335 RVA: 0x000CC026 File Offset: 0x000CA426
		public void OnPointerDown(PointerEventData evt)
		{
			this.Redirect<IPointerDownHandler>(evt, ExecuteEvents.pointerDownHandler);
		}

		// Token: 0x06002C48 RID: 11336 RVA: 0x000CC034 File Offset: 0x000CA434
		public void OnPointerUp(PointerEventData evt)
		{
			this.Redirect<IPointerUpHandler>(evt, ExecuteEvents.pointerUpHandler);
		}

		// Token: 0x06002C49 RID: 11337 RVA: 0x000CC042 File Offset: 0x000CA442
		public void OnPointerClick(PointerEventData evt)
		{
			if (!evt.dragging)
			{
				this.Redirect<IPointerClickHandler>(evt, ExecuteEvents.pointerClickHandler);
			}
		}

		// Token: 0x06002C4A RID: 11338 RVA: 0x000CC05C File Offset: 0x000CA45C
		private void Redirect<T>(PointerEventData evt, ExecuteEvents.EventFunction<T> handler) where T : IEventSystemHandler
		{
			Camera camera = this.dedicatedCamera;
			RaycastHit raycastHit;
			if (!this._collider.Raycast(camera.ScreenPointToRay(global::UnityEngine.Input.mousePosition), out raycastHit, 1000f))
			{
				return;
			}
			Vector2 textureCoord = raycastHit.textureCoord;
			evt.position = new Vector2((float)this.renderTexture.width * textureCoord.x, (float)this.renderTexture.height * textureCoord.y);
			this.raycastResults.Clear();
			this.raycaster.Raycast(evt, this.raycastResults);
			foreach (RaycastResult raycastResult in this.raycastResults)
			{
				evt.pointerPressRaycast = raycastResult;
				evt.pointerCurrentRaycast = raycastResult;
				if (ExecuteEvents.ExecuteHierarchy<T>(raycastResult.gameObject, evt, handler))
				{
					break;
				}
			}
		}

		// Token: 0x0400557E RID: 21886
		private List<RaycastResult> raycastResults = new List<RaycastResult>();

		// Token: 0x0400557F RID: 21887
		private Collider _collider;

		// Token: 0x04005580 RID: 21888
		public BaseRaycaster raycaster;

		// Token: 0x04005581 RID: 21889
		public RenderTexture renderTexture;

		// Token: 0x04005582 RID: 21890
		public Camera dedicatedCamera;
	}
}
