using UnityEngine;

// Token: 0x0200097E RID: 2430
namespace Match3.Scripts1
{
	public struct ShopTooltipItem
	{
		// Token: 0x04006337 RID: 25399
		public Sprite sprite;

		// Token: 0x04006338 RID: 25400
		public ShopTooltipItem.Type type;

		// Token: 0x0200097F RID: 2431
		public enum Type
		{
			// Token: 0x0400633A RID: 25402
			ItemPreview,
			// Token: 0x0400633B RID: 25403
			Ellipsis
		}
	}
}
