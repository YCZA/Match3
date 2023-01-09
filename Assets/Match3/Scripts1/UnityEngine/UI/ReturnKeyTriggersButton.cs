using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Match3.Scripts1.UnityEngine.UI
{
	// Token: 0x02000C28 RID: 3112
	[RequireComponent(typeof(InputField))]
	[AddComponentMenu("UI/Extensions/Return Key Trigger")]
	public class ReturnKeyTriggersButton : MonoBehaviour, ISubmitHandler, IEventSystemHandler
	{
		// Token: 0x0600496D RID: 18797 RVA: 0x00177474 File Offset: 0x00175874
		private void Start()
		{
			this._system = EventSystem.current;
		}

		// Token: 0x0600496E RID: 18798 RVA: 0x00177481 File Offset: 0x00175881
		private void RemoveHighlight()
		{
			this.button.OnPointerExit(new PointerEventData(this._system));
		}

		// Token: 0x0600496F RID: 18799 RVA: 0x0017749C File Offset: 0x0017589C
		public void OnSubmit(BaseEventData eventData)
		{
			if (this.highlight)
			{
				this.button.OnPointerEnter(new PointerEventData(this._system));
			}
			this.button.OnPointerClick(new PointerEventData(this._system));
			if (this.highlight)
			{
				base.Invoke("RemoveHighlight", this.highlightDuration);
			}
		}

		// Token: 0x04006FCB RID: 28619
		private EventSystem _system;

		// Token: 0x04006FCC RID: 28620
		public Button button;

		// Token: 0x04006FCD RID: 28621
		private bool highlight = true;

		// Token: 0x04006FCE RID: 28622
		public float highlightDuration = 0.2f;
	}
}
