using System.Collections.Generic;
using Match3.Scripts1.Wooga.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020006FB RID: 1787
	public class M3LevelSelectionItemTierView : ATableViewReusableCell<M3LevelSelectionItemTierView.State>, IDataView<M3LevelSelectionItemTier>
	{
		// Token: 0x17000702 RID: 1794
		// (get) Token: 0x06002C59 RID: 11353 RVA: 0x000CC322 File Offset: 0x000CA722
		private Transform CoinBonusRoot
		{
			get
			{
				return this.coinBonus.transform.parent.parent;
			}
		}

		// Token: 0x06002C5A RID: 11354 RVA: 0x000CC33C File Offset: 0x000CA73C
		public void Show(M3LevelSelectionItemTier data)
		{
			if (!data.level.IsCompleted || data.levelPlayMode == LevelPlayMode.LevelOfTheDay || data.levelPlayMode == LevelPlayMode.DiveForTreasure || data.levelPlayMode == LevelPlayMode.PirateBreakout)
			{
				if (this.coinBonus)
				{
					this.CoinBonusRoot.gameObject.SetActive(data.level.HasCoins);
					this.coinBonus.sprite = this.coinMultiplierSprites.GetSimilar("x" + data.multiplier);
				}
				if (this.diamondReward)
				{
					this.diamondReward.transform.parent.gameObject.SetActive(data.diamonds > 0);
					this.diamondReward.sprite = this.diamondsRewardSprites.GetSimilar("_" + data.multiplier);
				}
				if (this.seasonalReward)
				{
					this.seasonalReward.transform.parent.parent.gameObject.SetActive(data.showSeasonRewards);
					this.seasonalReward.sprite = data.seasonSprite;
					if (this.labelSeasonalAmount)
					{
						this.labelSeasonalAmount.text = data.seasonalCurrencyRewardAmount.ToString();
					}
				}
				if (this.collectable)
				{
					if (data.rewards[0].amount > 0)
					{
						this.collectable.gameObject.SetActive(true);
						this.collectable.Show(data.rewards[0]);
					}
					else
					{
						this.collectable.gameObject.SetActive(false);
					}
				}
				else if (data.rewards != null)
				{
					for (int i = 0; i < data.rewards.Count; i++)
					{
						if (i < this.extraRewards.Count)
						{
							this.extraRewards[i].Show(data.rewards[i]);
						}
					}
				}
			}
			else
			{
				if (this.collectable)
				{
					this.collectable.gameObject.SetActive(false);
				}
				if (this.coinBonus)
				{
					this.CoinBonusRoot.gameObject.SetActive(false);
				}
				if (this.diamondReward)
				{
					this.diamondReward.transform.parent.gameObject.SetActive(false);
				}
			}
			if (this.labelDiamondAmount)
			{
				this.labelDiamondAmount.text = data.diamonds.ToString();
			}
			if (this.labelTitle)
			{
				this.labelTitle.text = data.name;
			}
			this.ShowTournamentInfo(data);
		}

		// Token: 0x06002C5B RID: 11355 RVA: 0x000CC62C File Offset: 0x000CAA2C
		private void ShowTournamentInfo(M3LevelSelectionItemTier data)
		{
			if (this.tournamentRewardContainer != null)
			{
				this.tournamentTaskImage.sprite = this.tournamentTaskSpriteManager.GetSprite(data.tournamentType);
				this.tournamentPointMultiplierImage.sprite = this.tournamentMultiplierSpriteManager.GetSimilar(data.TournamentMultiplierAsString);
				this.tournamentRewardContainer.gameObject.SetActive(data.HasTournamentToShow);
			}
		}

		// Token: 0x04005590 RID: 21904
		public TMP_Text labelTitle;

		// Token: 0x04005591 RID: 21905
		public TMP_Text labelDiamondAmount;

		// Token: 0x04005592 RID: 21906
		public Image coinBonus;

		// Token: 0x04005593 RID: 21907
		public MaterialAmountView collectable;

		// Token: 0x04005594 RID: 21908
		public List<MaterialAmountView> extraRewards;

		// Token: 0x04005595 RID: 21909
		public SpriteManager coinMultiplierSprites;

		// Token: 0x04005596 RID: 21910
		public Image diamondReward;

		// Token: 0x04005597 RID: 21911
		public SpriteManager diamondsRewardSprites;

		// Token: 0x04005598 RID: 21912
		public GameObject tournamentRewardContainer;

		// Token: 0x04005599 RID: 21913
		public Image tournamentTaskImage;

		// Token: 0x0400559A RID: 21914
		public TournamentTaskSpriteManager tournamentTaskSpriteManager;

		// Token: 0x0400559B RID: 21915
		public TMP_Text labelSeasonalAmount;

		// Token: 0x0400559C RID: 21916
		public Image seasonalReward;

		// Token: 0x0400559D RID: 21917
		public Image tournamentPointMultiplierImage;

		// Token: 0x0400559E RID: 21918
		public SpriteManager tournamentMultiplierSpriteManager;

		// Token: 0x020006FC RID: 1788
		public enum State
		{
			// Token: 0x040055A0 RID: 21920
			Pending,
			// Token: 0x040055A1 RID: 21921
			Current,
			// Token: 0x040055A2 RID: 21922
			Focus,
			// Token: 0x040055A3 RID: 21923
			Completed,
			// Token: 0x040055A4 RID: 21924
			Hidden,
			// Token: 0x040055A5 RID: 21925
			CompletedCurrent
		}
	}
}
