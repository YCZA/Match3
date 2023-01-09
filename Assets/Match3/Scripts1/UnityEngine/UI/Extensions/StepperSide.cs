using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000BBC RID: 3004
	[RequireComponent(typeof(Selectable))]
	public class StepperSide : UIBehaviour, IPointerClickHandler, ISubmitHandler, IEventSystemHandler
	{
		// Token: 0x06004680 RID: 18048 RVA: 0x00165404 File Offset: 0x00163804
		protected StepperSide()
		{
		}

		// Token: 0x17000A38 RID: 2616
		// (get) Token: 0x06004681 RID: 18049 RVA: 0x0016540C File Offset: 0x0016380C
		private Selectable button
		{
			get
			{
				return base.GetComponent<Selectable>();
			}
		}

		// Token: 0x17000A39 RID: 2617
		// (get) Token: 0x06004682 RID: 18050 RVA: 0x00165414 File Offset: 0x00163814
		private Stepper stepper
		{
			get
			{
				return base.GetComponentInParent<Stepper>();
			}
		}

		// Token: 0x17000A3A RID: 2618
		// (get) Token: 0x06004683 RID: 18051 RVA: 0x0016541C File Offset: 0x0016381C
		private bool leftmost
		{
			get
			{
				return this.button == this.stepper.sides[0];
			}
		}

		// Token: 0x06004684 RID: 18052 RVA: 0x00165436 File Offset: 0x00163836
		public virtual void OnPointerClick(PointerEventData eventData)
		{
			if (eventData.button != PointerEventData.InputButton.Left)
			{
				return;
			}
			this.Press();
		}

		// Token: 0x06004685 RID: 18053 RVA: 0x0016544A File Offset: 0x0016384A
		public virtual void OnSubmit(BaseEventData eventData)
		{
			this.Press();
		}

		// Token: 0x06004686 RID: 18054 RVA: 0x00165454 File Offset: 0x00163854
		private void Press()
		{
			if (!this.button.IsActive() || !this.button.IsInteractable())
			{
				return;
			}
			if (this.leftmost)
			{
				this.stepper.StepDown();
			}
			else
			{
				this.stepper.StepUp();
			}
		}
	}
}
