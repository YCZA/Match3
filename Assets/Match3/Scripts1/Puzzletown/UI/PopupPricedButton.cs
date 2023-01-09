using Match3.Scripts1.UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Match3.Scripts1.Puzzletown.UI
{
	// Token: 0x02000876 RID: 2166
	public class PopupPricedButton : APopupCell<PricedButtonWithCallback>
	{
		// Token: 0x06003552 RID: 13650 RVA: 0x001002A1 File Offset: 0x000FE6A1
		private void OnEnable()
		{
			this.ExecuteOnChild(delegate(Button button)
			{
				button.onClick.AddListener(new UnityAction(this.OnClick));
			});
		}

		// Token: 0x06003553 RID: 13651 RVA: 0x001002B5 File Offset: 0x000FE6B5
		private void OnDisable()
		{
			this.ExecuteOnChild(delegate(Button button)
			{
				button.onClick.RemoveListener(new UnityAction(this.OnClick));
			});
		}

		// Token: 0x06003554 RID: 13652 RVA: 0x001002C9 File Offset: 0x000FE6C9
		public override void Show(PricedButtonWithCallback data)
		{
			this.data = data;
			this.ShowOnChildren(data.cost, false, true);
		}

		// Token: 0x06003555 RID: 13653 RVA: 0x001002E0 File Offset: 0x000FE6E0
		public override bool CanPresent(PricedButtonWithCallback data)
		{
			return true;
		}

		// Token: 0x06003556 RID: 13654 RVA: 0x001002E3 File Offset: 0x000FE6E3
		private void OnClick()
		{
			this.HandleOnParent(this.data.callback);
		}

		// Token: 0x04005D3D RID: 23869
		private PricedButtonWithCallback data;
	}
}
