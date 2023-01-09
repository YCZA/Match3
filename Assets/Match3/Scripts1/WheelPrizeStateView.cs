using Match3.Scripts1.Puzzletown.Config;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000969 RID: 2409
namespace Match3.Scripts1
{
	public class WheelPrizeStateView : MonoBehaviour
	{
		// Token: 0x170008F2 RID: 2290
		// (set) Token: 0x06003AC9 RID: 15049 RVA: 0x0012360C File Offset: 0x00121A0C
		public AdSpinPrize prizeType
		{
			set
			{
				if (value != AdSpinPrize.UnlimitedLives)
				{
					if (value != AdSpinPrize.Diamonds)
					{
						if (value == AdSpinPrize.Coins)
						{
							this.rewardImage.sprite = this.rewardSprites.GetSimilar("ui_icon_coins");
						}
					}
					else
					{
						this.rewardImage.sprite = this.rewardSprites.GetSimilar("ui_icon_diamonds");
					}
				}
				else
				{
					this.rewardImage.sprite = this.rewardSprites.GetSimilar("ui_icon_lives_unlimited");
				}
			}
		}

		// Token: 0x040062A6 RID: 25254
		public SpriteManager rewardSprites;

		// Token: 0x040062A7 RID: 25255
		public TextMeshProUGUI amountLabel;

		// Token: 0x040062A8 RID: 25256
		public TextMeshProUGUI timeLabel;

		// Token: 0x040062A9 RID: 25257
		public Image rewardImage;

		// Token: 0x040062AA RID: 25258
		public Image backgroundImage;

		// Token: 0x040062AB RID: 25259
		private const string LIVES_SPRITE = "ui_icon_lives_unlimited";

		// Token: 0x040062AC RID: 25260
		private const string DIAMONDS_SPRITE = "ui_icon_diamonds";

		// Token: 0x040062AD RID: 25261
		private const string COINS_SPRITE = "ui_icon_coins";
	}
}
