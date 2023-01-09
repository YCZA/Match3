using System;
using System.Collections.Generic;
using Match3.Scripts2.Env;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Config
{
	// Token: 0x02000483 RID: 1155
	[Serializable]
	public class GeneralConfig
	{
		// Token: 0x17000521 RID: 1313
		// (get) Token: 0x0600212F RID: 8495 RVA: 0x0008BB93 File Offset: 0x00089F93
		public LivesConfig lives
		{
			get
			{
				if (GameEnvironment.CurrentEnvironment == GameEnvironment.Environment.PRODUCTION)
				{
					return this.lives_prod;
				}
				if (GameEnvironment.CurrentEnvironment == GameEnvironment.Environment.STAGING)
				{
					return this.lives_stag;
				}
				return this.lives_dev;
			}
		}

		// Token: 0x06002130 RID: 8496 RVA: 0x0008BBBF File Offset: 0x00089FBF
		public MaterialAmount GetConversionPrice(MaterialAmount mat)
		{
			return new MaterialAmount("diamonds", Mathf.CeilToInt((float)mat.amount / (float)this.balance.buy_coins_per_diamond), MaterialAmountUsage.Undefined, 0);
		}

		// Token: 0x04004BFC RID: 19452
		public List<TierFactor> tier_factor;

		// Token: 0x04004BFD RID: 19453
		public LivesConfig lives_prod;

		// Token: 0x04004BFE RID: 19454
		public LivesConfig lives_stag;

		// Token: 0x04004BFF RID: 19455
		public LivesConfig lives_dev;

		// Token: 0x04004C00 RID: 19456
		public BalanceValue balance;

		// Token: 0x04004C01 RID: 19457
		public VillagerValue villager;

		// Token: 0x04004C02 RID: 19458
		public List<BoosterData> boosters;

		// Token: 0x04004C03 RID: 19459
		public TierUnlocked tier_unlocked;

		// Token: 0x04004C04 RID: 19460
		public TournamentSettings tournaments;

		// Token: 0x04004C05 RID: 19461
		public Notifications notifications;

		// Token: 0x04004C06 RID: 19462
		public RatingFilter rating_filter;

		// Token: 0x04004C07 RID: 19463
		public InviteFriendsConfig invite_friends;

		// Token: 0x04004C08 RID: 19464
		public ChapterUnlockBuildings chapter_buildings;

		// Token: 0x04004C09 RID: 19465
		public List<WheelPrize> spin_prizes;

		// Token: 0x04004C0A RID: 19466
		public List<JackpotPrize> jackpot_prizes;

		// Token: 0x04004C0B RID: 19467
		public WheelSettings wheel_settings;

		// Token: 0x04004C0C RID: 19468
		public List<LoadingScreenInfo> loading_screens;
	}
}
