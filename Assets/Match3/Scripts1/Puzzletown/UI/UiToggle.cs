using Match3.Scripts1.UnityEngine;
using Match3.Scripts1.Wooga.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Match3.Scripts1.Puzzletown.UI
{
	// Token: 0x020009F0 RID: 2544
	public abstract class UiToggle : MonoBehaviour, IDataView<bool>
	{
		// Token: 0x06003D67 RID: 15719 RVA: 0x001364E1 File Offset: 0x001348E1
		protected void OnEnable()
		{
			this.toggle.onValueChanged.AddListener(new UnityAction<bool>(this.OnValueChanged));
		}

		// Token: 0x06003D68 RID: 15720 RVA: 0x00136500 File Offset: 0x00134900
		protected void OnDisable()
		{
			this.toggle.onValueChanged.RemoveListener(new UnityAction<bool>(this.OnValueChanged));
		}

		// Token: 0x06003D69 RID: 15721 RVA: 0x0013651F File Offset: 0x0013491F
		public void Show(bool value)
		{
			this.toggle.isOn = value;
			this.ShowOnChildren((!value) ? UiToggleState.Off : UiToggleState.On, true, true);
		}

		// Token: 0x06003D6A RID: 15722 RVA: 0x00136542 File Offset: 0x00134942
		protected virtual void OnValueChanged(bool value)
		{
			this.ShowOnChildren((!value) ? UiToggleState.Off : UiToggleState.On, true, true);
		}

		// Token: 0x04006642 RID: 26178
		[SerializeField]
		protected Toggle toggle;
	}
}
