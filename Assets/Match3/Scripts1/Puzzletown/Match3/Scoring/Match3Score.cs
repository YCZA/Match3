using System;
using System.Collections.Generic;
using System.Text;
using Match3.Scripts1.Puzzletown.Services;

namespace Match3.Scripts1.Puzzletown.Match3.Scoring
{
	// Token: 0x0200063A RID: 1594
	[Serializable]
	public class Match3Score
	{
		// Token: 0x06002879 RID: 10361 RVA: 0x000B4DC8 File Offset: 0x000B31C8
		public Match3Score(LevelConfig config, LevelPlayMode levelPlayMode = LevelPlayMode.Regular)
		{
			this.cheated = false;
			this.movesLeft = config.data.moves;
			this.Config = config;
			this.hasCoins = config.HasCoins;
			this.levelPlayMode = levelPlayMode;
		}

		// Token: 0x170006A1 RID: 1697
		// (get) Token: 0x0600287A RID: 10362 RVA: 0x000B4E2B File Offset: 0x000B322B
		// (set) Token: 0x0600287B RID: 10363 RVA: 0x000B4E33 File Offset: 0x000B3233
		public LevelConfig Config { get; set; }

		// Token: 0x170006A2 RID: 1698
		// (get) Token: 0x0600287C RID: 10364 RVA: 0x000B4E3C File Offset: 0x000B323C
		public int MovesTaken
		{
			get
			{
				return this.Config.data.moves - this.movesLeft;
			}
		}

		// Token: 0x170006A3 RID: 1699
		// (get) Token: 0x0600287D RID: 10365 RVA: 0x000B4E55 File Offset: 0x000B3255
		public MaterialAmount CoinsCollected
		{
			get
			{
				this._coinsCollected.amount = this.amounts["coins"];
				return this._coinsCollected;
			}
		}

		// Token: 0x170006A4 RID: 1700
		// (get) Token: 0x0600287E RID: 10366 RVA: 0x000B4E78 File Offset: 0x000B3278
		public Materials AllCollected
		{
			get
			{
				return this.amounts;
			}
		}

		// Token: 0x170006A5 RID: 1701
		// (get) Token: 0x0600287F RID: 10367 RVA: 0x000B4E80 File Offset: 0x000B3280
		public Materials Rewards
		{
			get
			{
				Materials materials = new Materials();
				if (this.levelPlayMode == LevelPlayMode.LevelOfTheDay)
				{
					this.AppendLevelOfDayRewards(materials);
				}
				else if (this.levelPlayMode == LevelPlayMode.DiveForTreasure)
				{
					this.AppendDiveForTreasureRewards(materials);
				}
				else if (this.levelPlayMode == LevelPlayMode.PirateBreakout)
				{
					this.AppendPirateBreakoutRewards(materials);
				}
				else
				{
					this.AppendNormalRewards(materials);
				}
				MaterialAmount coinsCollected = this.CoinsCollected;
				coinsCollected.Mutltiply(this.Config.coinMultiplier);
				materials.Add(coinsCollected);
				return materials;
			}
		}

		// Token: 0x06002880 RID: 10368 RVA: 0x000B4F03 File Offset: 0x000B3303
		protected void AppendLevelOfDayRewards(Materials rewards)
		{
			rewards.Add(this.Config.levelOfDayRewards);
		}

		// Token: 0x06002881 RID: 10369 RVA: 0x000B4F16 File Offset: 0x000B3316
		protected void AppendDiveForTreasureRewards(Materials rewards)
		{
			rewards.Add(this.Config.diveForTreasureRewards);
		}

		// Token: 0x06002882 RID: 10370 RVA: 0x000B4F29 File Offset: 0x000B3329
		protected void AppendPirateBreakoutRewards(Materials rewards)
		{
			rewards.Add(this.Config.pirateBreakoutRewards);
		}

		// Token: 0x06002883 RID: 10371 RVA: 0x000B4F3C File Offset: 0x000B333C
		protected void AppendNormalRewards(Materials rewards)
		{
			rewards.Add(new MaterialAmount("diamonds", this.Config.Diamonds, MaterialAmountUsage.Undefined, 0));
			if (this.Config.seasonalRewards.amount > 0 && !this.Config.IsCompleted)
			{
				rewards.Add(this.Config.seasonalRewards);
			}
			if (!string.IsNullOrEmpty(this.Config.collectable) && !this.Config.IsCompleted)
			{
				rewards.Add(new MaterialAmount(this.Config.collectable, 1, MaterialAmountUsage.Undefined, 0));
			}
		}

		// Token: 0x06002884 RID: 10372 RVA: 0x000B4FDB File Offset: 0x000B33DB
		public bool AreCoinsCollectable()
		{
			return this.hasCoins;
		}

		// Token: 0x06002885 RID: 10373 RVA: 0x000B4FE3 File Offset: 0x000B33E3
		public void AddAmount(MaterialAmount amount)
		{
			this.amounts.Add(amount);
		}

		// Token: 0x06002886 RID: 10374 RVA: 0x000B4FF4 File Offset: 0x000B33F4
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder("\n");
			foreach (MaterialAmount materialAmount in this.amounts)
			{
				stringBuilder.Append(materialAmount.type);
				stringBuilder.Append(":");
				stringBuilder.Append(materialAmount.amount);
				stringBuilder.Append('\n');
			}
			return string.Format("[Match3Score: success:{1}, amounts={0}]", stringBuilder, this.success);
		}

		// Token: 0x170006A6 RID: 1702
		// (get) Token: 0x06002887 RID: 10375 RVA: 0x000B5098 File Offset: 0x000B3498
		public IEnumerable<MaterialAmount> ObjectivesNeeded
		{
			get
			{
				foreach (MaterialAmount objective in this.Config.data.objectives)
				{
					int collectedAmount = 0;
					foreach (MaterialAmount materialAmount in this.AllCollected)
					{
						if (materialAmount.type == objective.type)
						{
							collectedAmount = materialAmount.amount;
						}
					}
					yield return new MaterialAmount(objective.type, objective.amount - collectedAmount, MaterialAmountUsage.Undefined, 0);
				}
				yield break;
			}
		}

		// Token: 0x04005268 RID: 21096
		public Materials amounts = new Materials();

		// Token: 0x04005269 RID: 21097
		public bool cheated;

		// Token: 0x0400526A RID: 21098
		public int movesLeft;

		// Token: 0x0400526B RID: 21099
		public bool success;

		// Token: 0x0400526C RID: 21100
		public bool cancelled;

		// Token: 0x0400526D RID: 21101
		public LevelPlayMode levelPlayMode;

		// Token: 0x0400526E RID: 21102
		public bool isUITestingOnly;

		// Token: 0x0400526F RID: 21103
		public int scorePreHurrah;

		// Token: 0x04005270 RID: 21104
		public int scorePostHurrah;

		// Token: 0x04005271 RID: 21105
		public int lineGemsActivated;

		// Token: 0x04005272 RID: 21106
		public int fishActivated;

		// Token: 0x04005273 RID: 21107
		public int rainbowsActivated;

		// Token: 0x04005274 RID: 21108
		public int bombsActivated;

		// Token: 0x04005275 RID: 21109
		public int preGameBombLinegemUsed;

		// Token: 0x04005276 RID: 21110
		public int preGameDoubleFishUsed;

		// Token: 0x04005277 RID: 21111
		public int preGameRainbowsUsed;

		// Token: 0x04005278 RID: 21112
		public int ingameHammerUsed;

		// Token: 0x04005279 RID: 21113
		public int ingameStarUsed;

		// Token: 0x0400527A RID: 21114
		public int ingameRainbowUsed;

		// Token: 0x0400527B RID: 21115
		public int levelCascades;

		// Token: 0x0400527C RID: 21116
		public int postMoves;

		// Token: 0x0400527D RID: 21117
		public int reshuffles;

		// Token: 0x0400527E RID: 21118
		public bool wantsRetry;

		// Token: 0x0400527F RID: 21119
		public bool showLevelMap;

		// Token: 0x04005280 RID: 21120
		public bool hasCoins;

		// Token: 0x04005281 RID: 21121
		public TournamentScore tournamentScore;

		// Token: 0x04005282 RID: 21122
		public int bankedDiamonds;

		// Token: 0x04005284 RID: 21124
		private MaterialAmount _coinsCollected = new MaterialAmount("coins", 0, MaterialAmountUsage.Undefined, 0);
	}
}
