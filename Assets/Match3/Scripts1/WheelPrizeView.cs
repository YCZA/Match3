using Match3.Scripts1.Puzzletown.Config;
using UnityEngine;

// Token: 0x0200096A RID: 2410
namespace Match3.Scripts1
{
	public class WheelPrizeView : MonoBehaviour
	{
		// Token: 0x06003ACB RID: 15051 RVA: 0x0012369A File Offset: 0x00121A9A
		public void SetPrize(WheelPrize prize)
		{
			this.SetPrizeForView(this.active, prize);
			this.SetPrizeForView(this.pending, prize);
		}

		// Token: 0x06003ACC RID: 15052 RVA: 0x001236B8 File Offset: 0x00121AB8
		private void SetPrizeForView(WheelPrizeStateView view, WheelPrize prize)
		{
			view.prizeType = prize.prizeType;
			view.amountLabel.text = prize.amount.ToString();
			if (prize.prizeType != AdSpinPrize.UnlimitedLives)
			{
				view.timeLabel.gameObject.SetActive(false);
			}
			else
			{
				view.amountLabel.text = (prize.amount / 60).ToString();
			}
		}

		// Token: 0x040062AE RID: 25262
		public WheelPrizeStateView active;

		// Token: 0x040062AF RID: 25263
		public WheelPrizeStateView pending;
	}
}
