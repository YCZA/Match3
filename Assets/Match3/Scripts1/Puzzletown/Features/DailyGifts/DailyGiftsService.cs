using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Match3.Scripts1.Puzzletown.Services;
using Wooga.Coroutines;
using Wooga.UnityFramework;
using Match3.Scripts2.Env;
using Match3.Scripts2.PlayerData;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Features.DailyGifts
{
	// Token: 0x020008DD RID: 2269
	public class DailyGiftsService : AService
	{
		// Token: 0x0600372E RID: 14126 RVA: 0x0010D6EC File Offset: 0x0010BAEC
		public DailyGiftsService()
		{
			WooroutineRunner.StartCoroutine(this.InitRoutine(), null);
		}

		// Token: 0x17000870 RID: 2160
		// (get) Token: 0x0600372F RID: 14127 RVA: 0x0010D704 File Offset: 0x0010BB04
		public IEnumerable<DailyGiftsConfig.Day> Days
		{
			get
			{
				int num = this.gameState.DailyGifts.NumConsecutiveDays / this.Config.days.Count<DailyGiftsConfig.Day>();
				foreach (DailyGiftsConfig.Day day in this.Config.days)
				{
					day.adjustedDay = day.day + this.Config.days.Count<DailyGiftsConfig.Day>() * num;
				}
				return this.Config.days;
			}
		}

		// Token: 0x17000871 RID: 2161
		// (get) Token: 0x06003730 RID: 14128 RVA: 0x0010D7AC File Offset: 0x0010BBAC
		public int DaysSinceLastClaim
		{
			get
			{
				DateTime time = Scripts1.DateTimeExtensions.FromUnixTimeStamp(this.gameState.DailyGifts.LastReceived, DateTimeKind.Utc);
				return this.timeService.LocalNow.GetTotalDays() - time.GetTotalDays();
			}
		}

		// Token: 0x17000872 RID: 2162
		// (get) Token: 0x06003731 RID: 14129 RVA: 0x0010D7EC File Offset: 0x0010BBEC
		public DailyGiftsConfig.Day CurrentDay
		{
			get
			{
				if (this.DaysSinceLastClaim > 1)
				{
					this.Data.NumConsecutiveDays = 0;
				}
				return this.Config.days[this.Data.NumConsecutiveDays % this.Config.days.Count];
			}
		}

		// Token: 0x17000873 RID: 2163
		// (get) Token: 0x06003732 RID: 14130 RVA: 0x0010D840 File Offset: 0x0010BC40
		public bool IsAvailable
		{
			get
			{
				// eli key point 每日礼包弹出条件 
				return this.sbs.SbsConfig.feature_switches.daily_gifts_enabled && this.gameState.Progression.UnlockedLevel >= this.Config.general.unlock_level && this.timeService.IsTimeValid && this.DaysSinceLastClaim >= 1;
			}
		}

		// Token: 0x17000874 RID: 2164
		// (get) Token: 0x06003733 RID: 14131 RVA: 0x0010D8AB File Offset: 0x0010BCAB
		private DailyGiftsConfig Config
		{
			get
			{
				return this.sbs.SbsConfig.dailygifts;
			}
		}

		// Token: 0x17000875 RID: 2165
		// (get) Token: 0x06003734 RID: 14132 RVA: 0x0010D8BD File Offset: 0x0010BCBD
		private DailyGiftsDataService Data
		{
			get
			{
				return this.gameState.DailyGifts;
			}
		}

		// Token: 0x06003735 RID: 14133 RVA: 0x0010D8CC File Offset: 0x0010BCCC
		public void Claim(bool withBonus, DateTime claimTime)
		{
			claimTime = claimTime.ToLocalTime();
			DailyGiftsConfig.Day currentDay = this.CurrentDay;
			List<MaterialAmount> list = new List<MaterialAmount>();
			list.Add(new MaterialAmount(currentDay.reward_type, currentDay.reward_amount, MaterialAmountUsage.Undefined, 0));
			if (withBonus)
			{
				list.Add(new MaterialAmount(currentDay.bonus_type, currentDay.bonus_amount, MaterialAmountUsage.Undefined, 0));
				this.videoAdService.TrackClaim();
			}
			this.Data.NumConsecutiveDays = this.Data.NumConsecutiveDays + 1;
			this.Data.LastReceived = this.GetStartOfDay(claimTime);
			WoogaDebug.Log(new object[]
			{
				"claimtime",
				this.Data.LastReceived,
				claimTime
			});
			Materials materials = new Materials(list);
			this.gameState.Resources.AddPendingRewards(materials, this.tracking.GetDailyGiftTrackingCall(materials, withBonus, currentDay.day));
			this.gameState.Save(true);
		}

		// Token: 0x06003736 RID: 14134 RVA: 0x0010D9C4 File Offset: 0x0010BDC4
		public void ForceCurrentDay(int day)
		{
			if (GameEnvironment.IsProduction)
			{
				return;
			}
			this.Data.NumConsecutiveDays = Mathf.Max(0, day - 1);
			this.Data.LastReceived = this.timeService.LocalNow.AddDays(-1.0).ToUnixTimeStamp();
		}

		// Token: 0x06003737 RID: 14135 RVA: 0x0010DA1C File Offset: 0x0010BE1C
		public void ForceAvailable()
		{
			this.Data.LastReceived = this.timeService.LocalNow.AddDays(-1.0).ToUnixTimeStamp();
		}

		// Token: 0x06003738 RID: 14136 RVA: 0x0010DA55 File Offset: 0x0010BE55
		public void ForceReset()
		{
			this.Data.LastReceived = DateTime.MinValue.ToUnixTimeStamp();
		}

		// Token: 0x06003739 RID: 14137 RVA: 0x0010DA6C File Offset: 0x0010BE6C
		private int GetStartOfDay(DateTime time)
		{
			int days = (time - DateTime.MinValue).Days;
			return DateTime.MinValue.AddDays((double)days).ToUnixTimeStamp();
		}

		// Token: 0x0600373A RID: 14138 RVA: 0x0010DAA4 File Offset: 0x0010BEA4
		private IEnumerator InitRoutine()
		{
			yield return ServiceLocator.Instance.Inject(this);
			base.OnInitialized.Dispatch();
			yield break;
		}

		// Token: 0x04005F63 RID: 24419
		[WaitForService(true, true)]
		private GameStateService gameState;

		// Token: 0x04005F64 RID: 24420
		[WaitForService(true, true)]
		private SBSService sbs;

		// Token: 0x04005F65 RID: 24421
		[WaitForService(true, true)]
		private TimeService timeService;

		// Token: 0x04005F66 RID: 24422
		[WaitForService(true, true)]
		private TrackingService tracking;

		// Token: 0x04005F67 RID: 24423
		[WaitForService(true, true)]
		private IVideoAdService videoAdService;
	}
}
