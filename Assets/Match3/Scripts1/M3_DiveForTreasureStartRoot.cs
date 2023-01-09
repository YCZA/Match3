using System;
using Match3.Scripts1.Puzzletown.Match3;
using Match3.Scripts1.Puzzletown.UI;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;
using UnityEngine;

// Token: 0x02000729 RID: 1833
namespace Match3.Scripts1
{
	public class M3_DiveForTreasureStartRoot : M3_ALevelStartRoot
	{
		// Token: 0x06002D67 RID: 11623 RVA: 0x000D3227 File Offset: 0x000D1627
		protected override void Go()
		{
			base.Go();
			if (this.parameters == null)
			{
				base.Hide(true);
				return;
			}
			if (!base.registeredFirst)
			{
				this.Show(this.parameters);
			}
		}

		// Token: 0x06002D68 RID: 11624 RVA: 0x000D325E File Offset: 0x000D165E
		protected override void Show(LevelConfig level)
		{
			base.Show(level);
			this.SetupRewardsView(level);
			this.timer.SetTargetTime(this.gameStateService.DiveForTreasure.EndTime, false, null);
			base.UpdateCurrentSalesBanner(AUiAdjuster.Orientation);
		}

		// Token: 0x06002D69 RID: 11625 RVA: 0x000D3298 File Offset: 0x000D1698
		private void SetupRewardsView(LevelConfig level)
		{
			if (level == null)
			{
				return;
			}
			DiveForTreasureSetConfig diveForTreasureSetConfig = level.LevelCollectionConfig as DiveForTreasureSetConfig;
			if (diveForTreasureSetConfig == null)
			{
				WoogaDebug.LogError(new object[]
				{
					"Level is missing DiveForTreasureSetConfig - abort"
				});
				return;
			}
			TierConfig tierConfig = diveForTreasureSetConfig.tiers[0];
			int selectedTier = (int)this.parameters.SelectedTier;
			M3LevelSelectionItemTier data = new M3LevelSelectionItemTier
			{
				name = tierConfig.name,
				levelPlayMode = LevelPlayMode.DiveForTreasure,
				rewards = this.parameters.diveForTreasureRewards,
				level = this.parameters,
				diamonds = 0,
				multiplier = this.configService.general.tier_factor[selectedTier].coin_multiplier,
				tier = selectedTier,
				// tournamentType = this.tournamentService.GetApparentOngoingTournamentType(),
				// tournamentPointMultiplier = this.tournamentService.GetCurrentScoreMultiplierForTier(selectedTier)
			};
			this.rewards.Show(data);
		}

		// Token: 0x06002D6A RID: 11626 RVA: 0x000D3388 File Offset: 0x000D1788
		protected override void OnEnable()
		{
			base.OnEnable();
			if (this.dftRoot != null)
			{
				this.dftRoot.UpdateStartViewVisibility(true);
			}
			base.UpdateCurrentSalesBanner(AUiAdjuster.Orientation);
			AUiAdjuster.OnScreenOrientationChange.AddListener(new Action<ScreenOrientation>(base.UpdateCurrentSalesBanner));
		}

		// Token: 0x06002D6B RID: 11627 RVA: 0x000D33DC File Offset: 0x000D17DC
		protected override void OnDisable()
		{
			base.OnDisable();
			AUiAdjuster.OnScreenOrientationChange.RemoveListener(new Action<ScreenOrientation>(base.UpdateCurrentSalesBanner));
			if (this.dftRoot != null && !this.onCompleted.Dispatched)
			{
				this.dftRoot.UpdateStartViewVisibility(false);
			}
		}

		// Token: 0x04005708 RID: 22280
		[WaitForService(true, true)]
		private TimeService timeService;

		// Token: 0x04005709 RID: 22281
		[WaitForService(true, true)]
		private GameStateService gameStateService;

		// Token: 0x0400570A RID: 22282
		[SerializeField]
		private M3LevelSelectionItemTierView rewards;

		// Token: 0x0400570B RID: 22283
		[SerializeField]
		private CountdownTimer timer;

		// Token: 0x0400570C RID: 22284
		public DiveForTreasureRoot dftRoot;
	}
}
