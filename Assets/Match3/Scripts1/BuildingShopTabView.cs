using Match3.Scripts1.Wooga.UI;
using Match3.Scripts2.Building.Shop;
using UnityEngine.UI;

// Token: 0x02000A36 RID: 2614
namespace Match3.Scripts1
{
	public class BuildingShopTabView : AUiTabView<ShopTag>, IDataView<BuildingShopTabData>
	{
		// Token: 0x06003EBB RID: 16059 RVA: 0x0013ED40 File Offset: 0x0013D140
		public void Show(BuildingShopTabData param)
		{
			base.Show(param.data);
			if (this.image && this.manager)
			{
				this.image.sprite = (param.sprite ?? this.manager.GetSimilar(this._data.ToString()));
			}
			if (this.badge)
			{
				if (param.badgeCount > 0)
				{
					// 审核模式隐藏建筑商店的uiIndicator(显示新物品数量的红点提示)
					// #if REVIEW_VERSION
					// {
					// this.badge.Hide();
					// }
					// #else
					// {
					this.badge.Show(param.badgeCount.ToString());
					// }
					// #endif
				}
				else
				{
					this.badge.Hide();
				}
			}
		}

		// Token: 0x040067D8 RID: 26584
		public Image image;

		// Token: 0x040067D9 RID: 26585
		public SpriteManager manager;

		// Token: 0x040067DA RID: 26586
		public UiIndicator badge;
	}
}
