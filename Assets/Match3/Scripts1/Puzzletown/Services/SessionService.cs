using System;
using System.Collections;
using System.IO;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.Signals;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x02000803 RID: 2051
	public class SessionService : AService
	{
		// Token: 0x060032AD RID: 12973 RVA: 0x000EF19B File Offset: 0x000ED59B
		public SessionService()
		{
			WooroutineRunner.StartCoroutine(this.LoadRoutine(), null);
		}

		// Token: 0x170007FA RID: 2042
		// (get) Token: 0x060032AE RID: 12974 RVA: 0x000EF1BB File Offset: 0x000ED5BB
		public static string CRASH_MARKER_PATH
		{
			get
			{
				return Application.persistentDataPath + "/crash_marker.txt";
			}
		}

		// Token: 0x170007FB RID: 2043
		// (get) Token: 0x060032AF RID: 12975 RVA: 0x000EF1CC File Offset: 0x000ED5CC
		public SessionService.RatingInfo ratingInfo
		{
			get
			{
				this.EnsureRatingInfo();
				return this._ratingInfo;
			}
		}

		// Token: 0x170007FC RID: 2044
		// (get) Token: 0x060032B0 RID: 12976 RVA: 0x000EF1DC File Offset: 0x000ED5DC
		public int NumTrackedSessions
		{
			get
			{
				//ISessionTrackerData sessionTrackerData = Tracking.sessionTracker.GetSessionTrackerData();
				//return (sessionTrackerData == null) ? 0 : sessionTrackerData.sessionCount;
				return 0;
			}
		}

		// Token: 0x170007FD RID: 2045
		// (get) Token: 0x060032B1 RID: 12977 RVA: 0x000EF208 File Offset: 0x000ED608
		public int SessionsToday
		{
			get
			{
				int numTrackedSessions = this.NumTrackedSessions;
				int @int = PlayerPrefs.GetInt("SessionService.DAILY_SESSION_DAY", 0);
				int num = DateTime.Today.ToUnixTimeStamp();
				if (@int != num)
				{
					PlayerPrefs.SetInt("SessionService.DAILY_SESSION_LAST_DAY_COUNT", numTrackedSessions - 1);
					PlayerPrefs.SetInt("SessionService.DAILY_SESSION_DAY", num);
					PlayerPrefs.Save();
				}
				return numTrackedSessions - PlayerPrefs.GetInt("SessionService.DAILY_SESSION_LAST_DAY_COUNT");
			}
		}

		// Token: 0x170007FE RID: 2046
		// (get) Token: 0x060032B2 RID: 12978 RVA: 0x000EF263 File Offset: 0x000ED663
		public int SessionsWithoutCrashes
		{
			get
			{
				return this.NumTrackedSessions - this.ratingInfo.lastCrashedSession;
			}
		}

		// Token: 0x170007FF RID: 2047
		// (get) Token: 0x060032B3 RID: 12979 RVA: 0x000EF277 File Offset: 0x000ED677
		// (set) Token: 0x060032B4 RID: 12980 RVA: 0x000EF27F File Offset: 0x000ED67F
		public int NumberOfIslandLoads { get; set; }

		// Token: 0x060032B5 RID: 12981 RVA: 0x000EF288 File Offset: 0x000ED688
		public override void DeInit()
		{
			this.onRestart.Dispatch();
		}

		// Token: 0x060032B6 RID: 12982 RVA: 0x000EF298 File Offset: 0x000ED698
		private void EnsureRatingInfo()
		{
			if (this._ratingInfo == null)
			{
				this._ratingInfo = APlayerPrefsObject<SessionService.RatingInfo>.Load();
				this._ratingInfo.nextReminderAtLevel = Mathf.Max(this._ratingInfo.nextReminderAtLevel, this.config.general.rating_filter.wait_num_levels);
				this._ratingInfo.Save();
			}
		}

		// Token: 0x060032B7 RID: 12983 RVA: 0x000EF2F6 File Offset: 0x000ED6F6
		private void CheckForCrashes()
		{
			if (File.Exists(SessionService.CRASH_MARKER_PATH))
			{
				this.ratingInfo.lastCrashedSession = this.NumTrackedSessions;
				this.ratingInfo.Save();
				File.Delete(SessionService.CRASH_MARKER_PATH);
			}
		}

		// Token: 0x060032B8 RID: 12984 RVA: 0x000EF330 File Offset: 0x000ED730
		private IEnumerator LoadRoutine()
		{
			yield return ServiceLocator.Instance.Inject(this);
			this.CheckForCrashes();
			base.OnInitialized.Dispatch();
			yield break;
		}

		// Token: 0x04005B17 RID: 23319
		private const string DAILY_SESSION_DAY = "SessionService.DAILY_SESSION_DAY";

		// Token: 0x04005B18 RID: 23320
		private const string DAILY_SESSION_LAST_DAY_COUNT = "SessionService.DAILY_SESSION_LAST_DAY_COUNT";

		// Token: 0x04005B19 RID: 23321
		public readonly Signal onRestart = new Signal();

		// Token: 0x04005B1A RID: 23322
		[WaitForService(true, true)]
		private ConfigService config;

		// Token: 0x04005B1B RID: 23323
		[WaitForService(true, true)]
		private TrackingService tracking;

		// Token: 0x04005B1C RID: 23324
		public int roundsPlayed;

		// Token: 0x04005B1D RID: 23325
		public bool wasLastRoundSuccesfull;

		// Token: 0x04005B1E RID: 23326
		private SessionService.RatingInfo _ratingInfo;

		// Token: 0x02000804 RID: 2052
		public class RatingInfo : APlayerPrefsObject<SessionService.RatingInfo>
		{
			// Token: 0x060032BA RID: 12986 RVA: 0x000EF353 File Offset: 0x000ED753
			public override string ToString()
			{
				return string.Format("LastCrashAtSession: {0}, NextReminderAtLevel: {1}, StopShowing: {2}", this.lastCrashedSession, this.nextReminderAtLevel, this.stopShowing);
			}

			// Token: 0x04005B20 RID: 23328
			public int lastCrashedSession;

			// Token: 0x04005B21 RID: 23329
			public int nextReminderAtLevel;

			// Token: 0x04005B22 RID: 23330
			public bool stopShowing;
		}
	}
}
