using Match3.Scripts1.Wooga.Signals;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020006BC RID: 1724
	public class InputController : MonoBehaviour, IPointerUpHandler, IPointerClickHandler, IDragHandler, IPointerEnterHandler, IEventSystemHandler
	{
		// Token: 0x170006CC RID: 1740
		// (get) Token: 0x06002AF7 RID: 10999 RVA: 0x000C45FC File Offset: 0x000C29FC
		private float FieldWidth
		{
			get
			{
				if (InputController._fieldWidth == 0f)
				{
					Vector3 vector = Camera.main.WorldToScreenPoint(Vector3.zero);
					InputController._fieldWidth = Camera.main.WorldToScreenPoint(new Vector3(1f, 0f)).x - vector.x;
				}
				return InputController._fieldWidth;
			}
		}

		// Token: 0x06002AF8 RID: 11000 RVA: 0x000C465B File Offset: 0x000C2A5B
		public void OnPointerUp(PointerEventData eventData)
		{
			if (eventData.button == PointerEventData.InputButton.Right)
			{
				this.onRightClick.Dispatch(base.GetComponent<FieldView>());
				return;
			}
			this.hasSwapped = false;
		}

		// Token: 0x06002AF9 RID: 11001 RVA: 0x000C4684 File Offset: 0x000C2A84
		public void OnPointerClick(PointerEventData eventData)
		{
			FieldView component = base.GetComponent<FieldView>();
			this.onClicked.Dispatch(component.GridPosition);
		}

		// Token: 0x06002AFA RID: 11002 RVA: 0x000C46AC File Offset: 0x000C2AAC
		public void OnDrag(PointerEventData eventData)
		{
			if (eventData.pointerDrag && !this.hasSwapped)
			{
				Vector2 vector = eventData.position - eventData.pressPosition;
				float num = vector.magnitude / this.FieldWidth;
				if (num > 0.2f)
				{
					IntVector2 b = IntVector2.Zero;
					if (Mathf.Abs(vector.x) > Mathf.Abs(vector.y))
					{
						b = IntVector2.Right * (int)Mathf.Sign(vector.x);
					}
					else
					{
						b = IntVector2.Up * (int)Mathf.Sign(vector.y);
					}
					FieldView component = base.GetComponent<FieldView>();
					Move value = new Move(component.GridPosition, component.GridPosition + b, true, false, true);
					this.onSwapped.Dispatch(value);
					this.hasSwapped = true;
				}
			}
		}

		// Token: 0x06002AFB RID: 11003 RVA: 0x000C4790 File Offset: 0x000C2B90
		public void OnPointerEnter(PointerEventData eventData)
		{
			this.hasSwapped = false;
			if (eventData.pointerEnter && Input.GetMouseButton(0) && eventData.pointerDrag == null)
			{
				eventData.pointerDrag = eventData.pointerEnter;
			}
		}

		// Token: 0x0400543B RID: 21563
		public readonly Signal<Move> onSwapped = new Signal<Move>();

		// Token: 0x0400543C RID: 21564
		public readonly Signal<IntVector2> onClicked = new Signal<IntVector2>();

		// Token: 0x0400543D RID: 21565
		public readonly Signal<FieldView> onRightClick = new Signal<FieldView>();

		// Token: 0x0400543E RID: 21566
		private bool hasSwapped;

		// Token: 0x0400543F RID: 21567
		private const float SWAP_THRESHOLD_MIN = 0.2f;

		// Token: 0x04005440 RID: 21568
		private static float _fieldWidth;
	}
}
