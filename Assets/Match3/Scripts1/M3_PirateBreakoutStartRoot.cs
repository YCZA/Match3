using System;
using Match3.Scripts1.Puzzletown.Match3;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;
using UnityEngine;

// Token: 0x0200072C RID: 1836
namespace Match3.Scripts1
{
	public class M3_PirateBreakoutStartRoot : M3_ALevelStartRoot
	{
		// Token: 0x06002D89 RID: 11657 RVA: 0x000D3FF4 File Offset: 0x000D23F4
		protected override void Go()
		{
			base.Go();
			this.portraitSaleNotificationView.Hide();
			this.landscapeSaleNotificationView.Hide();
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

		// Token: 0x06002D8A RID: 11658 RVA: 0x000D404C File Offset: 0x000D244C
		protected override void OnEnable()
		{
			base.OnEnable();
			base.UpdateCurrentSalesBanner(AUiAdjuster.Orientation);
			AUiAdjuster.OnScreenOrientationChange.AddListener(new Action<ScreenOrientation>(base.UpdateCurrentSalesBanner));
		}

		// Token: 0x06002D8B RID: 11659 RVA: 0x000D4075 File Offset: 0x000D2475
		protected override void OnDisable()
		{
			base.OnDisable();
			AUiAdjuster.OnScreenOrientationChange.RemoveListener(new Action<ScreenOrientation>(base.UpdateCurrentSalesBanner));
		}

		// Token: 0x06002D8C RID: 11660 RVA: 0x000D4093 File Offset: 0x000D2493
		protected override void Show(LevelConfig level)
		{
			base.Show(level);
			this.SetupRewardsView(level);
			this.timer.SetTargetTime(this.gameStateService.PirateBreakout.EndTime, false, null);
			base.UpdateCurrentSalesBanner(AUiAdjuster.Orientation);
		}

		// Token: 0x06002D8D RID: 11661 RVA: 0x000D40CC File Offset: 0x000D24CC
		private void SetupRewardsView(LevelConfig level)
		{
			if (level == null)
			{
				return;
			}
			PirateBreakoutSetConfig pirateBreakoutSetConfig = level.LevelCollectionConfig as PirateBreakoutSetConfig;
			if (pirateBreakoutSetConfig == null)
			{
				WoogaDebug.LogError(new object[]
				{
					"Level is missing PirateBreakoutSetConfig - abort"
				});
				return;
			}
			TierConfig tierConfig = pirateBreakoutSetConfig.tiers[0];
			int selectedTier = (int)this.parameters.SelectedTier;
			M3LevelSelectionItemTier data = new M3LevelSelectionItemTier
			{
				name = tierConfig.name,
				levelPlayMode = LevelPlayMode.PirateBreakout,
				rewards = this.parameters.pirateBreakoutRewards,
				level = this.parameters,
				diamonds = 0,
				multiplier = this.configService.general.tier_factor[selectedTier].coin_multiplier,
				tier = selectedTier,
				// tournamentType = this.tournamentService.GetApparentOngoingTournamentType(),
				// tournamentPointMultiplier = this.tournamentService.GetCurrentScoreMultiplierForTier(selectedTier)
			};
			this.rewards.Show(data);
		}

		// Token: 0x04005723 RID: 22307
		[WaitForService(true, true)]
		private TimeService timeService;

		// Token: 0x04005724 RID: 22308
		[WaitForService(true, true)]
		private GameStateService gameStateService;

		// Token: 0x04005725 RID: 22309
		[SerializeField]
		private M3LevelSelectionItemTierView rewards;

		// Token: 0x04005726 RID: 22310
		[SerializeField]
		private CountdownTimer timer;
	}
}
