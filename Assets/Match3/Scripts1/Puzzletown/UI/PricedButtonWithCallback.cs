using System;

namespace Match3.Scripts1.Puzzletown.UI
{
	// Token: 0x02000875 RID: 2165
	public class PricedButtonWithCallback : BaseButtonWithCallback, IMaterialAmountBasedElement
	{
		// Token: 0x0600354F RID: 13647 RVA: 0x00100281 File Offset: 0x000FE681
		public PricedButtonWithCallback(MaterialAmount cost, Action callback) : base(callback)
		{
			this.cost = cost;
		}

		// Token: 0x06003550 RID: 13648 RVA: 0x00100291 File Offset: 0x000FE691
		public MaterialAmount GetMaterialAmount()
		{
			return this.cost;
		}

		// Token: 0x04005D3C RID: 23868
		public MaterialAmount cost;
	}
}
