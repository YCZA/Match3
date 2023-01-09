using System;
using System.Collections;
using System.Collections.Generic;
using Match3.Scripts1.Puzzletown.Match3;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Puzzletown.UI;
using Match3.Scripts1.UnityEngine;
using Wooga.Coroutines;
using Wooga.UnityFramework;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200072A RID: 1834
namespace Match3.Scripts1
{
	public class M3_LevelOfDayStartRoot : M3_ALevelStartRoot
	{
		// Token: 0x06002D6D RID: 11629 RVA: 0x000D343C File Offset: 0x000D183C
		protected override void Go()
		{
			base.Go();
			this.portraitSaleNotificationView.Hide();
			this.landscapeSaleNotificationView.Hide();
			if (base.registeredFirst)
			{
				base.StartCoroutine(this.SetupForTestRoutine());
			}
			else
			{
				this.Show(this.parameters);
			}
			this.ExecuteOnChildren(delegate(CreateUiGrabCamera grabber)
			{
				grabber.RefreshBakedImage();
			}, false);
		}

		// Token: 0x06002D6E RID: 11630 RVA: 0x000D34B2 File Offset: 0x000D18B2
		protected override void OnEnable()
		{
			base.OnEnable();
			base.UpdateCurrentSalesBanner(AUiAdjuster.Orientation);
			AUiAdjuster.OnScreenOrientationChange.AddListener(new Action<ScreenOrientation>(base.UpdateCurrentSalesBanner));
		}

		// Token: 0x06002D6F RID: 11631 RVA: 0x000D34DB File Offset: 0x000D18DB
		protected override void OnDisable()
		{
			AUiAdjuster.OnScreenOrientationChange.RemoveListener(new Action<ScreenOrientation>(base.UpdateCurrentSalesBanner));
		}

		// Token: 0x06002D70 RID: 11632 RVA: 0x000D34F4 File Offset: 0x000D18F4
		protected void SetupLevelOfDayStatusDetails()
		{
			int currentTryCount = this.levelOfDayService.GetCurrentTryCount();
			int num = this.levelOfDayService.GetRemainingSeconds();
			if (base.registeredFirst)
			{
				int num2 = this.timeService.Now.ToUnixTimeStamp();
				num = this.testModel.endUTCTime - num2;
			}
			bool expired = num < 0;
			this.SetupTryStatus(currentTryCount);
			this.SetupTimer(num);
			this.SetupPlayButton(expired);
		}

		// Token: 0x06002D71 RID: 11633 RVA: 0x000D355D File Offset: 0x000D195D
		protected void SetupTryStatus(int triesSoFar)
		{
			this.tryStatus.Init(triesSoFar, false, 0f);
		}

		// Token: 0x06002D72 RID: 11634 RVA: 0x000D3574 File Offset: 0x000D1974
		protected void SetupTimer(int remainingSeconds)
		{
			bool flag = remainingSeconds >= 0;
			this.lodTimerLabel.Refresh(remainingSeconds, flag);
			if (flag)
			{
				this.levelOfDayService.OnTimerChanged.RemoveListener(new Action<int>(this.HandleTimerChanged));
				this.levelOfDayService.OnTimerChanged.AddListener(new Action<int>(this.HandleTimerChanged));
			}
		}

		// Token: 0x06002D73 RID: 11635 RVA: 0x000D35D4 File Offset: 0x000D19D4
		protected void HandleTimerChanged(int remainingSeconds)
		{
			this.lodTimerLabel.Refresh(remainingSeconds, remainingSeconds >= 0);
			if (remainingSeconds < 0)
			{
				this.SetupPlayButton(true);
			}
		}

		// Token: 0x06002D74 RID: 11636 RVA: 0x000D35F7 File Offset: 0x000D19F7
		protected void SetupPlayButton(bool expired)
		{
			this.playButton.interactable = !expired;
		}

		// Token: 0x06002D75 RID: 11637 RVA: 0x000D3608 File Offset: 0x000D1A08
		protected IEnumerator SetupForTestRoutine()
		{
			this.testModel.endUTCTime = this.timeService.Now.ToUnixTimeStamp() + 3600;
			Wooroutine<LevelConfig> configFetchRoutine = this.levelOfDayService.GetCurrentLevelOfDayConfig(this.testModel);
			yield return configFetchRoutine;
			this.parameters = configFetchRoutine.ReturnValue;
			this.Show(this.parameters);
			yield break;
		}

		// Token: 0x06002D76 RID: 11638 RVA: 0x000D3623 File Offset: 0x000D1A23
		protected override void RemoveListeners()
		{
			base.RemoveListeners();
			if (this.levelOfDayService != null)
			{
				this.levelOfDayService.OnTimerChanged.RemoveListener(new Action<int>(this.HandleTimerChanged));
			}
		}

		// Token: 0x06002D77 RID: 11639 RVA: 0x000D3652 File Offset: 0x000D1A52
		protected override void Show(LevelConfig level)
		{
			base.Show(level);
			this.SetupLevelOfDayStatusDetails();
			this.SetupRewardsView();
			base.UpdateCurrentSalesBanner(AUiAdjuster.Orientation);
		}

		// Token: 0x06002D78 RID: 11640 RVA: 0x000D3674 File Offset: 0x000D1A74
		private void SetupRewardsView()
		{
			int selectedTier = (int)this.parameters.SelectedTier;
			List<MaterialAmount> levelOfDayRewards = this.parameters.levelOfDayRewards;
			int diamonds = 0;
			foreach (MaterialAmount materialAmount in levelOfDayRewards)
			{
				string type = materialAmount.type;
				if (type != null)
				{
					if (type == "diamonds")
					{
						diamonds = materialAmount.amount;
					}
				}
			}
			M3LevelSelectionItemTier data = new M3LevelSelectionItemTier
			{
				name = this.m3ConfigService.GetTierName(selectedTier),
				levelPlayMode = LevelPlayMode.LevelOfTheDay,
				rewards = this.parameters.levelOfDayRewards,
				level = this.parameters,
				diamonds = diamonds,
				multiplier = this.configService.general.tier_factor[selectedTier].coin_multiplier,
				tier = selectedTier,
				// tournamentType = this.tournamentService.GetApparentOngoingTournamentType(),
				// tournamentPointMultiplier = this.tournamentService.GetCurrentScoreMultiplierForTier(selectedTier)
			};
			this.rewards.Show(data);
		}

		// Token: 0x0400570D RID: 22285
		[WaitForService(true, true)]
		private LevelOfDayService levelOfDayService;

		// Token: 0x0400570E RID: 22286
		[WaitForService(true, true)]
		private TimeService timeService;

		// Token: 0x0400570F RID: 22287
		[SerializeField]
		private M3LevelSelectionItemTierView rewards;

		// Token: 0x04005710 RID: 22288
		[SerializeField]
		private LevelOfDayModel testModel;

		// Token: 0x04005711 RID: 22289
		[SerializeField]
		private LevelOfDayTryStatusUI tryStatus;

		// Token: 0x04005712 RID: 22290
		[SerializeField]
		private TimerLabel lodTimerLabel;

		// Token: 0x04005713 RID: 22291
		[SerializeField]
		private Button playButton;
	}
}
