using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200083C RID: 2108
namespace Match3.Scripts1
{
	public abstract class APurchasedPopupRoot : APtSceneRoot<MaterialAmount>
	{
		// Token: 0x0600345E RID: 13406 RVA: 0x000FA5CB File Offset: 0x000F89CB
		protected override void Go()
		{
			this.labelAmount.text = this.parameters.amount.ToString();
		}

		// Token: 0x04005C3C RID: 23612
		[SerializeField]
		private TextMeshProUGUI labelAmount;

		// Token: 0x04005C3D RID: 23613
		[SerializeField]
		private Button buttonClose;

		// Token: 0x04005C3E RID: 23614
		[SerializeField]
		private Button buttonConfirm;
	}
}
