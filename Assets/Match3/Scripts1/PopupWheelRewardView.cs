using Match3.Scripts1.Puzzletown.Config;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000968 RID: 2408
namespace Match3.Scripts1
{
	public class PopupWheelRewardView : MonoBehaviour
	{
		// Token: 0x06003AC7 RID: 15047 RVA: 0x001234AC File Offset: 0x001218AC
		public void ShowPrize(WheelPrize prize)
		{
			this.rewardAmountLabel.text = prize.amount.ToString();
			switch (prize.prizeType)
			{
				case AdSpinPrize.UnlimitedLives:
					this.rewardAmountLabel.text = (prize.amount / 60).ToString();
					this.rewardImage.sprite = this.rewardSprites.GetSimilar("ui_icon_lives_unlimited");
					break;
				case AdSpinPrize.Diamonds:
					this.rewardTimeLabel.gameObject.SetActive(false);
					this.rewardImage.sprite = this.rewardSprites.GetSimilar("ui_icon_bunch_diamond");
					break;
				case AdSpinPrize.Coins:
					this.rewardTimeLabel.gameObject.SetActive(false);
					this.rewardImage.sprite = this.rewardSprites.GetSimilar("ui_icon_bunch_coin");
					break;
				case AdSpinPrize.Starboost:
					this.rewardTimeLabel.gameObject.SetActive(false);
					this.rewardImage.sprite = this.rewardSprites.GetSimilar("ui_icon_boost_star");
					break;
				case AdSpinPrize.HammerBoost:
					this.rewardTimeLabel.gameObject.SetActive(false);
					this.rewardImage.sprite = this.rewardSprites.GetSimilar("ui_icon_boost_hammer");
					break;
			}
		}

		// Token: 0x0400629D RID: 25245
		public Image rewardImage;

		// Token: 0x0400629E RID: 25246
		public TextMeshProUGUI rewardAmountLabel;

		// Token: 0x0400629F RID: 25247
		public TextMeshProUGUI rewardTimeLabel;

		// Token: 0x040062A0 RID: 25248
		public SpriteManager rewardSprites;

		// Token: 0x040062A1 RID: 25249
		private const string UNLIMITED_LIVES_SPRITE = "ui_icon_lives_unlimited";

		// Token: 0x040062A2 RID: 25250
		private const string DIAMONDS_SPRITE = "ui_icon_bunch_diamond";

		// Token: 0x040062A3 RID: 25251
		private const string COINS_SPRITE = "ui_icon_bunch_coin";

		// Token: 0x040062A4 RID: 25252
		private const string STAR_BOOST_SPRITE = "ui_icon_boost_star";

		// Token: 0x040062A5 RID: 25253
		private const string HAMMER_BOOST_SPRITE = "ui_icon_boost_hammer";
	}
}
