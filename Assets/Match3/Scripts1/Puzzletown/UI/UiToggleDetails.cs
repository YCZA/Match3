using UnityEngine;

namespace Match3.Scripts1.Puzzletown.UI
{
	// Token: 0x020009F1 RID: 2545
	public class UiToggleDetails : UiToggle
	{
		// Token: 0x06003D6C RID: 15724 RVA: 0x00136561 File Offset: 0x00134961
		private void Start()
		{
			this.panel.SetActive(false);
		}

		// Token: 0x06003D6D RID: 15725 RVA: 0x0013656F File Offset: 0x0013496F
		protected override void OnValueChanged(bool value)
		{
			base.OnValueChanged(value);
			this.panel.SetActive(value);
		}

		// Token: 0x04006643 RID: 26179
		[SerializeField]
		private GameObject panel;
	}
}
