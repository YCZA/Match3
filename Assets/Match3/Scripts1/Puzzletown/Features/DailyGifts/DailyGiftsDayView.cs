using Match3.Scripts1.Wooga.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Match3.Scripts1.Puzzletown.Features.DailyGifts
{
	// Token: 0x020008D9 RID: 2265
	public class DailyGiftsDayView : ATableViewReusableCell, IDataView<DailyGiftsDayView.Data>
	{
		// Token: 0x1700086F RID: 2159
		// (get) Token: 0x0600371B RID: 14107 RVA: 0x0010CD75 File Offset: 0x0010B175
		public override int reusableId
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x0600371C RID: 14108 RVA: 0x0010CD78 File Offset: 0x0010B178
		public void Show(DailyGiftsDayView.Data data)
		{
			string name = this.animationDefault.name;
			if (data.isCollected)
			{
				name = this.animationCollected.name;
				if (data.needsCollectedAnimation)
				{
					name = this.animationPendingToCollected.name;
					data.needsCollectedAnimation = false;
				}
			}
			else if (data.isCurrentDay)
			{
				if (data.isAnimationFinished)
				{
					name = this.animationPending.name;
				}
				else
				{
					name = this.animationDefaultToPending.name;
					data.isAnimationFinished = true;
				}
			}
			if (data.config.is_jackpot && !data.isCollected)
			{
				this.background.sprite = this.bgTextureJackpot;
			}
			else
			{
				this.background.sprite = this.bgTextureDefault;
			}
			// 动画有问题，会修改文字颜色
			base.GetComponent<Animation>().Play(name);
			bool flag = data.config.is_jackpot || data.isCollected;
			MaterialAmount[] array = (!flag) ? this.giftRewards : data.config.Rewards;
			this.reward.Show(array[0]);
			this.reward.HideAmounts(!flag);
			this.bonus.Show(array[1]);
			this.bonus.HideAmounts(!flag);
			this.labelDayNumber.text = data.config.adjustedDay.ToString();
		}

		// Token: 0x04005F38 RID: 24376
		[SerializeField]
		private Image background;

		// Token: 0x04005F39 RID: 24377
		[SerializeField]
		private Sprite bgTextureDefault;

		// Token: 0x04005F3A RID: 24378
		[SerializeField]
		private Sprite bgTextureJackpot;

		// Token: 0x04005F3B RID: 24379
		[SerializeField]
		private MaterialAmountView reward;

		// Token: 0x04005F3C RID: 24380
		[SerializeField]
		private MaterialAmountView bonus;

		// Token: 0x04005F3D RID: 24381
		[SerializeField]
		private TextMeshProUGUI labelDayNumber;

		// Token: 0x04005F3E RID: 24382
		[SerializeField]
		private AnimationClip animationDefaultToPending;

		// Token: 0x04005F3F RID: 24383
		[SerializeField]
		private AnimationClip animationPendingToCollected;

		// Token: 0x04005F40 RID: 24384
		[SerializeField]
		private AnimationClip animationPending;

		// Token: 0x04005F41 RID: 24385
		[SerializeField]
		private AnimationClip animationCollected;

		// Token: 0x04005F42 RID: 24386
		[SerializeField]
		private AnimationClip animationDefault;

		// Token: 0x04005F43 RID: 24387
		private MaterialAmount[] giftRewards = new MaterialAmount[]
		{
			new MaterialAmount("gift", -1, MaterialAmountUsage.Undefined, 0),
			new MaterialAmount("gift", -1, MaterialAmountUsage.Undefined, 0)
		};

		// Token: 0x020008DA RID: 2266
		public class Data
		{
			// Token: 0x0600371D RID: 14109 RVA: 0x0010CEFB File Offset: 0x0010B2FB
			public Data(DailyGiftsConfig.Day config, bool isCurrentDay, bool isCollected, bool needsCollectedAnimation)
			{
				this.config = config;
				this.isCurrentDay = isCurrentDay;
				this.isCollected = isCollected;
				this.needsCollectedAnimation = needsCollectedAnimation;
			}

			// Token: 0x04005F44 RID: 24388
			public DailyGiftsConfig.Day config;

			// Token: 0x04005F45 RID: 24389
			public bool isCurrentDay;

			// Token: 0x04005F46 RID: 24390
			public bool isCollected;

			// Token: 0x04005F47 RID: 24391
			public bool needsCollectedAnimation;

			// Token: 0x04005F48 RID: 24392
			public bool isAnimationFinished;
		}
	}
}
