using System;
using System.Collections;
using Match3.Scripts1.Puzzletown.Config;
using Match3.Scripts1.Puzzletown.Match3;
using Wooga.Coroutines;
using Wooga.UnityFramework;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x02000752 RID: 1874
	public class BankService : AWeeklyEventService
	{
		// Token: 0x06002E6B RID: 11883 RVA: 0x000D92E3 File Offset: 0x000D76E3
		public BankService()
		{
			WooroutineRunner.StartCoroutine(this.InitRoutine(), null);
		}

		// Token: 0x06002E6C RID: 11884 RVA: 0x000D92F8 File Offset: 0x000D76F8
		public void UpdateBankedDiamonds(LevelConfig level)
		{
			if (!level.IsCompleted && this.BankFeatureEnabled())
			{
				int val = this.gameStateService.Bank.NumberOfBankedDiamonds + this.GetBankedDiamondRewardForLevel(level);
				this.gameStateService.Bank.NumberOfBankedDiamonds = Math.Min(val, this.sbsService.SbsConfig.bank.balancing.full_amount);
			}
		}

		// Token: 0x06002E6D RID: 11885 RVA: 0x000D9364 File Offset: 0x000D7764
		public int GetBankedDiamondRewardForLevel(LevelConfig level)
		{
			int result = 0;
			bool flag = this.gameStateService.Bank.NumberOfBankedDiamonds == this.sbsService.SbsConfig.bank.balancing.full_amount;
			if (level.IsCompleted || !this.BankFeatureEnabled() || flag)
			{
				return result;
			}
			AreaConfig.Tier selectedTier = level.SelectedTier;
			if (selectedTier != AreaConfig.Tier.a)
			{
				if (selectedTier != AreaConfig.Tier.b)
				{
					if (selectedTier == AreaConfig.Tier.c)
					{
						result = this.sbsService.SbsConfig.bank.balancing.hard_diamond_reward;
					}
				}
				else
				{
					result = this.sbsService.SbsConfig.bank.balancing.medium_diamond_reward;
				}
			}
			else
			{
				result = this.sbsService.SbsConfig.bank.balancing.easy_diamond_reward;
			}
			return result;
		}

		// Token: 0x06002E6E RID: 11886 RVA: 0x000D9440 File Offset: 0x000D7840
		private IEnumerator InitRoutine()
		{
			yield return ServiceLocator.Instance.Inject(this);
			WooroutineRunner.StartCoroutine(base.WaitForSbsRoutine(), null);
			base.OnInitialized.Dispatch();
			yield break;
		}

		// Token: 0x06002E6F RID: 11887 RVA: 0x000D945C File Offset: 0x000D785C
		public bool BankFeatureEnabled()
		{
			// 审核版禁用bank
			// #if REVIEW_VERSION
				// return false;
			// #endif
			bool piggy_bank_enabled = this.sbsService.SbsConfig.feature_switches.piggy_bank_enabled;
			bool flag = this.progressionDataService.UnlockedLevel >= this.sbsService.SbsConfig.bank.balancing.unlock_level;
			bool flag2 = true;
			bool flag3 = true;
			if (this.EventABTestEnabled())
			{
				flag2 = !this.gameStateService.Bank.IsCurrentEventBought;
				flag3 = (this.timeService.LocalNow > this.DataService.StartTime && this.timeService.LocalNow < this.DataService.EndTime);
			}
			return piggy_bank_enabled && flag && flag2 && flag3;
		}

		// Token: 0x06002E70 RID: 11888 RVA: 0x000D9522 File Offset: 0x000D7922
		public bool EventABTestEnabled()
		{
			return this.sbsService.SbsConfig.feature_switches.piggy_bank_as_event;
		}

		// Token: 0x1700072A RID: 1834
		// (get) Token: 0x06002E71 RID: 11889 RVA: 0x000D9539 File Offset: 0x000D7939
		protected override WeeklyEventType WeeklyEventType
		{
			get
			{
				return WeeklyEventType.Bank;
			}
		}

		// Token: 0x1700072B RID: 1835
		// (get) Token: 0x06002E72 RID: 11890 RVA: 0x000D953D File Offset: 0x000D793D
		protected override AWeeklyEventDataService DataService
		{
			get
			{
				return this.gameStateService.Bank;
			}
		}

		// Token: 0x040057BE RID: 22462
		[WaitForService(true, true)]
		private SBSService sbsService;

		// Token: 0x040057BF RID: 22463
		[WaitForService(true, true)]
		private ProgressionDataService.Service progressionDataService;

		// Token: 0x040057C0 RID: 22464
		[WaitForService(true, true)]
		private TrackingService trackingService;
	}
}
