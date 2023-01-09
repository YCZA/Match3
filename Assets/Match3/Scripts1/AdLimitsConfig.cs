using System;
using Match3.Scripts1.Puzzletown.Services;

// Token: 0x0200047B RID: 1147
namespace Match3.Scripts1
{
	[Serializable]
	public class AdLimitsConfig
	{
		// Token: 0x0600211D RID: 8477 RVA: 0x0008B7F8 File Offset: 0x00089BF8
		public int GetDailyLimitFor(AdPlacement placement)
		{
			switch (placement)
			{
				case AdPlacement.Unspecified:
					return this.global_daily_limit;
				case AdPlacement.AdWheel:
					return this.wheel_daily_limit;
				case AdPlacement.Challenges:
				case AdPlacement.Challenges_V2:
					return this.challenges_daily_limit;
				case AdPlacement.DailyGift:
					return this.gift_daily_limit;
				case AdPlacement.LivesShop:
					return this.lives_shop_daily_limit;
				default:
					return 0;
			}
		}

		// Token: 0x04004BC3 RID: 19395
		public int global_daily_limit;

		// Token: 0x04004BC4 RID: 19396
		public int wheel_daily_limit;

		// Token: 0x04004BC5 RID: 19397
		public int challenges_daily_limit;

		// Token: 0x04004BC6 RID: 19398
		public int gift_daily_limit;

		// Token: 0x04004BC7 RID: 19399
		public int lives_shop_daily_limit;
	}
}
