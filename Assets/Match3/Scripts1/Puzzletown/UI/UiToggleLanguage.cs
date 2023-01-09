using System.Linq;
using Match3.Scripts1.Localization;
using Match3.Scripts1.UnityEngine;
using UnityEngine;
using UnityEngine.UI;

namespace Match3.Scripts1.Puzzletown.UI
{
	// Token: 0x020009F3 RID: 2547
	public class UiToggleLanguage : UiToggle
	{
		// Token: 0x06003D72 RID: 15730 RVA: 0x001365B8 File Offset: 0x001349B8
		private void Start()
		{
			if (PTLocalizationConfig.AVAILABLE_LANGUAGES.Contains(this.language))
			{
				this.languagesGroup.RegisterToggle(this.toggle);
				this.toggle.group = this.languagesGroup;
			}
			else
			{
				base.gameObject.SetActive(false);
			}
		}

		// Token: 0x06003D73 RID: 15731 RVA: 0x0013660D File Offset: 0x00134A0D
		protected override void OnValueChanged(bool value)
		{
			base.OnValueChanged(value);
			if (value)
			{
				this.HandleOnParent(this.language);
			}
		}

		// Token: 0x04006645 RID: 26181
		public WoogaSystemLanguage language;

		// Token: 0x04006646 RID: 26182
		[SerializeField]
		private ToggleGroup languagesGroup;
	}
}
