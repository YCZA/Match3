using Match3.Scripts1.Wooga.Signals;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000885 RID: 2181
namespace Match3.Scripts1
{
	public class SwipePanel : MonoBehaviour, IBeginDragHandler, IDragHandler, IEventSystemHandler
	{
		// Token: 0x0600358D RID: 13709 RVA: 0x001011FF File Offset: 0x000FF5FF
		private void Start()
		{
			this.dpi = ((Screen.dpi != 0f) ? Screen.dpi : 240f);
		}

		// Token: 0x0600358E RID: 13710 RVA: 0x00101225 File Offset: 0x000FF625
		public void OnBeginDrag(PointerEventData eventData)
		{
			this.beginDrag = eventData.pressPosition;
			this.dragging = true;
		}

		// Token: 0x0600358F RID: 13711 RVA: 0x0010123C File Offset: 0x000FF63C
		public void OnDrag(PointerEventData eventData)
		{
			if (!this.dragging)
			{
				return;
			}
			if ((this.beginDrag - eventData.position).magnitude > 0.05f * this.dpi)
			{
				Vector2 vector = this.beginDrag - eventData.position;
				if (Mathf.Abs(vector.x) > Mathf.Abs(vector.y))
				{
					if (vector.x > 0f)
					{
						this.onSwipe.Dispatch(RelativeDirection.Right);
					}
					else
					{
						this.onSwipe.Dispatch(RelativeDirection.Left);
					}
				}
				else if (vector.y > 0f)
				{
					this.onSwipe.Dispatch(RelativeDirection.Up);
				}
				else
				{
					this.onSwipe.Dispatch(RelativeDirection.Down);
				}
				this.dragging = false;
			}
		}

		// Token: 0x04005D79 RID: 23929
		public readonly Signal<RelativeDirection> onSwipe = new Signal<RelativeDirection>();

		// Token: 0x04005D7A RID: 23930
		public const float SWIPE_ACTIVATION_DISTANCE = 0.05f;

		// Token: 0x04005D7B RID: 23931
		private Vector2 beginDrag;

		// Token: 0x04005D7C RID: 23932
		private bool dragging;

		// Token: 0x04005D7D RID: 23933
		private float dpi;
	}
}
