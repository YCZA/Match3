using System.Collections.Generic;
using System.Linq;
using Match3.Scripts1.Puzzletown.Config;
using UnityEngine;

// Token: 0x02000983 RID: 2435
namespace Match3.Scripts1
{
	public class TownBottomPanelShopTooltip : AVisibleGameObject
	{
		// Token: 0x06003B5D RID: 15197 RVA: 0x0012740C File Offset: 0x0012580C
		public void Show(List<Sprite> sprites, ChapterUnlockBuildings config)
		{
			if (sprites.Count > config.max_count)
			{
				this.dataSource.Show(this.AllItems(sprites.Take(config.max_count - 1), true));
			}
			else
			{
				this.dataSource.Show(this.AllItems(sprites, false));
			}
		}

		// Token: 0x06003B5E RID: 15198 RVA: 0x00127464 File Offset: 0x00125864
		public IEnumerable<ShopTooltipItem> AllItems(IEnumerable<Sprite> sprites, bool addShowMore)
		{
			foreach (Sprite sprite in sprites)
			{
				yield return new ShopTooltipItem
				{
					sprite = sprite
				};
			}
			if (addShowMore)
			{
				yield return new ShopTooltipItem
				{
					type = ShopTooltipItem.Type.Ellipsis
				};
			}
			yield break;
		}

		// Token: 0x0400637D RID: 25469
		public ShopTooltipDataSource dataSource;
	}
}
