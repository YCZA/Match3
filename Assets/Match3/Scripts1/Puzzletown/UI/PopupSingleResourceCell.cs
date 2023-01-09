using UnityEngine;

namespace Match3.Scripts1.Puzzletown.UI
{
	// Token: 0x0200087A RID: 2170
	[RequireComponent(typeof(MaterialAmountView))]
	public class PopupSingleResourceCell : APopupCell<MaterialAmount>
	{
		// Token: 0x06003562 RID: 13666 RVA: 0x00100388 File Offset: 0x000FE788
		public override void Show(MaterialAmount data)
		{
			MaterialAmountView component = base.GetComponent<MaterialAmountView>();
			component.Show(data);
			if (data.type == "diamonds")
			{
				component.image.sprite = this.diamondsBundle;
			}
			component.label.enabled = (data.amount > 0);
		}

		// Token: 0x06003563 RID: 13667 RVA: 0x001003DF File Offset: 0x000FE7DF
		public override bool CanPresent(MaterialAmount data)
		{
			return true;
		}

		// Token: 0x04005D43 RID: 23875
		public Sprite diamondsBundle;
	}
}
