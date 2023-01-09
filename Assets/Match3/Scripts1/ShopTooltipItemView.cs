using Match3.Scripts1.Wooga.UI;
using UnityEngine.UI;

// Token: 0x02000980 RID: 2432
namespace Match3.Scripts1
{
	public class ShopTooltipItemView : ATableViewReusableCell<ShopTooltipItem.Type>, IDataView<ShopTooltipItem>
	{
		// Token: 0x06003B3D RID: 15165 RVA: 0x001262DC File Offset: 0x001246DC
		public void Show(ShopTooltipItem item)
		{
			if (this.image)
			{
				this.image.sprite = item.sprite;
			}
		}

		// Token: 0x0400633C RID: 25404
		public Image image;
	}
}
