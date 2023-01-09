using Match3.Scripts1.UnityEngine;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.UI
{
	// Token: 0x020009F4 RID: 2548
	public class UiToggleSetting : UiToggle, ICategorised<ToggleSetting>
	{
		// Token: 0x06003D75 RID: 15733 RVA: 0x00136630 File Offset: 0x00134A30
		public ToggleSetting GetSetting()
		{
			return this.setting;
		}

		// Token: 0x06003D76 RID: 15734 RVA: 0x00136638 File Offset: 0x00134A38
		protected override void OnValueChanged(bool value)
		{
			base.OnValueChanged(value);
			this.HandleOnParent(this.setting, value);
		}

		// Token: 0x06003D77 RID: 15735 RVA: 0x0013664E File Offset: 0x00134A4E
		public string GetEditorDescription()
		{
			return this.setting.ToString();
		}

		// Token: 0x06003D78 RID: 15736 RVA: 0x00136661 File Offset: 0x00134A61
		public ToggleSetting GetCategory()
		{
			return this.setting;
		}

		// Token: 0x04006647 RID: 26183
		[SerializeField]
		private ToggleSetting setting;
	}
}
