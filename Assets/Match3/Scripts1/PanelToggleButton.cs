using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x0200085F RID: 2143
namespace Match3.Scripts1
{
	[RequireComponent(typeof(Button))]
	public class PanelToggleButton : MonoBehaviour
	{
		// Token: 0x060034F3 RID: 13555 RVA: 0x000FE342 File Offset: 0x000FC742
		public void Hide()
		{
			this.panelEnabled = false;
			this.Apply();
		}

		// Token: 0x060034F4 RID: 13556 RVA: 0x000FE351 File Offset: 0x000FC751
		private void Awake()
		{
			base.GetComponent<Button>().onClick.AddListener(new UnityAction(this.OnClick));
			this.Apply();
		}

		// Token: 0x060034F5 RID: 13557 RVA: 0x000FE375 File Offset: 0x000FC775
		private void OnClick()
		{
			this.panelEnabled = !this.isPanelActive;
			this.Apply();
		}

		// Token: 0x060034F6 RID: 13558 RVA: 0x000FE38C File Offset: 0x000FC78C
		private void Apply()
		{
			this.panel.gameObject.SetActive(this.panelEnabled);
		}

		// Token: 0x17000847 RID: 2119
		// (get) Token: 0x060034F7 RID: 13559 RVA: 0x000FE3A4 File Offset: 0x000FC7A4
		private bool isPanelActive
		{
			get
			{
				return this.panel.gameObject.activeSelf;
			}
		}

		// Token: 0x04005CE8 RID: 23784
		public CanvasGroup panel;

		// Token: 0x04005CE9 RID: 23785
		public bool panelEnabled;
	}
}
