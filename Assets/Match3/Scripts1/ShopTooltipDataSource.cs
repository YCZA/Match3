

// Token: 0x0200097D RID: 2429
namespace Match3.Scripts1
{
	public class ShopTooltipDataSource : ArrayDataSource<ShopTooltipItemView, ShopTooltipItem>
	{
		// Token: 0x06003B3B RID: 15163 RVA: 0x001262B8 File Offset: 0x001246B8
		public override int GetReusableIdForIndex(int index)
		{
			return (int)this.GetDataForIndex(index).type;
		}
	}
}
