using System;
using System.Collections;
using Match3.Scripts1.Wooga.Core.DeviceInfo;
using Wooga.Coroutines;
using Wooga.UnityFramework;
using Match3.Scripts2.Network;
using UnityEngine;
using UnityEngine.Networking;

// Token: 0x02000805 RID: 2053
namespace Match3.Scripts1
{
	public class TimeService : AService
	{
		// Token: 0x060032BB RID: 12987 RVA: 0x000EF42C File Offset: 0x000ED82C
		// public TimeService(TrackingService tracking)
		public TimeService()
		{
			// this.tracking = tracking;
			this.LoadInfo();
			WooroutineRunner.StartCoroutine(this.PingSbsRoutine(0), null);
			base.OnInitialized.Dispatch();
			this.WasManipulated();
			TimeService.allowTimeCheating = false;
		}

		// Token: 0x17000800 RID: 2048
		// (get) Token: 0x060032BC RID: 12988 RVA: 0x000EF488 File Offset: 0x000ED888
		// (set) Token: 0x060032BD RID: 12989 RVA: 0x000EF490 File Offset: 0x000ED890
		public bool IsTimeValid { get; private set; }

		// Token: 0x17000801 RID: 2049
		// (get) Token: 0x060032BE RID: 12990 RVA: 0x000EF49C File Offset: 0x000ED89C
		public DateTime Now
		{
			get
			{
				if (TimeService.allowTimeCheating)
				{
					return DateTime.UtcNow;
				}
				if (!this.IsTimeValid)
				{
					return DateTime.UtcNow;
				}
				DateTime value = this.serverDateTime.Add(DateTime.UtcNow - this.localTime);
				return DateTime.SpecifyKind(value, DateTimeKind.Utc);
			}
		}

		// Token: 0x17000802 RID: 2050
		// (get) Token: 0x060032BF RID: 12991 RVA: 0x000EF4F0 File Offset: 0x000ED8F0
		public DateTime LocalNow
		{
			get
			{
				return this.Now.ToLocalTime();
			}
		}

		// Token: 0x060032C0 RID: 12992 RVA: 0x000EF510 File Offset: 0x000ED910
		public bool WasManipulated()
		{
			this.uptime = (double)TimeService.GetUpTime();
			this.bootTime = (int)BootTime.GetBootTime();
			return this.DidBootTimeChange(this.bootTime) && (this.bootTime < this.clockInfo.lastBootTime || (this.bootTime > this.clockInfo.lastBootTime && this.uptime > this.clockInfo.lastUptime));
		}

		// Token: 0x060032C1 RID: 12993 RVA: 0x000EF58C File Offset: 0x000ED98C
		public override void OnSuspend()
		{
			if (!this.WasManipulated())
			{
				this.clockInfo.lastBootTime = this.bootTime;
				this.clockInfo.lastUptime = this.uptime;
				PlayerPrefs.SetString(this.clockInfo.GetType().Name, JsonUtility.ToJson(this.clockInfo));
				PlayerPrefs.Save();
			}
		}

		// Token: 0x060032C2 RID: 12994 RVA: 0x000EF5EB File Offset: 0x000ED9EB
		public override void OnResume()
		{
			WooroutineRunner.StartCoroutine(this.PingSbsRoutine(0), null);
			if (this.WasManipulated())
			{
				WooroutineRunner.StartCoroutine(this.TrackRoutine(), null);
			}
			else
			{
				this.didTrackSession = false;
			}
		}

		// Token: 0x060032C3 RID: 12995 RVA: 0x000EF620 File Offset: 0x000EDA20
		public static long GetUpTime()
		{
			long num = BootTime.GetBootTime();
			DateTime d = Scripts1.DateTimeExtensions.FromUnixTimeStamp((int)num, DateTimeKind.Utc);
			return (long)(DateTime.UtcNow - d).TotalSeconds;
		}

		// Token: 0x060032C4 RID: 12996 RVA: 0x000EF650 File Offset: 0x000EDA50
		private bool DidBootTimeChange(int bootTime)
		{
			return this.clockInfo.lastBootTime != 0 && Mathf.Abs(bootTime - this.clockInfo.lastBootTime) > 60;
		}

		// Token: 0x060032C5 RID: 12997 RVA: 0x000EF67C File Offset: 0x000EDA7C
		private IEnumerator TrackRoutine()
		{
			if (!this.didTrackSession)
			{
				int offset = this.bootTime - this.clockInfo.lastBootTime;
				// yield return this.tracking.OnInitialized;
				// this.tracking.TrackEvent(new object[]
				// {
				// 	"cheat_detected",
				// 	offset
				// });
				this.didTrackSession = true;
			}
			yield break;
		}

		// Token: 0x060032C6 RID: 12998 RVA: 0x000EF698 File Offset: 0x000EDA98
		private void LoadInfo()
		{
			string @string = PlayerPrefs.GetString(typeof(TimeService.ClockInfo).Name, null);
			if (@string != null)
			{
				this.clockInfo = JsonUtility.FromJson<TimeService.ClockInfo>(@string);
			}
			if (this.clockInfo == null)
			{
				this.clockInfo = new TimeService.ClockInfo();
			}
		}

		private IEnumerator PingSbsRoutine(int numRetries = 0)
		{
			this.IsTimeValid = TimeService.allowTimeCheating;
			// UnityWebRequest request = new UnityWebRequest("https://api.sbs.wooga.com/ping", "GET");
			UnityWebRequest request = UnityWebRequest.Get(URL.Ping);
			yield return request.SendWebRequest();
			if (request.responseCode == 200L)
			{
				// string responseHeader = request.GetResponseHeader("X-SBS-DATE");
				// this.serverTime = DateTime.Parse(responseHeader).ToUniversalTime();
				serverDateTime = DateTime.Parse(request.downloadHandler.text);
				Debug.Log("serverDateTime: "+serverDateTime);
				this.localTime = DateTime.UtcNow;
				this.IsTimeValid = true;
			}
			else if (numRetries < 10)
			{
				float waitTime = Mathf.Pow(2f, (float)numRetries);
				yield return new WaitForSeconds(waitTime);
				yield return this.PingSbsRoutine(numRetries + 1);
			}
		}

		// Token: 0x04005B23 RID: 23331
		// private const string URL = "https://api.sbs.wooga.com/ping";

		// Token: 0x04005B24 RID: 23332
		private const int NUM_RETRIES = 10;

		// Token: 0x04005B25 RID: 23333
		public static bool allowTimeCheating;

		// Token: 0x04005B26 RID: 23334
		// private TrackingService tracking;

		// Token: 0x04005B27 RID: 23335
		private TimeService.ClockInfo clockInfo;

		// Token: 0x04005B28 RID: 23336
		private double uptime;

		// Token: 0x04005B29 RID: 23337
		private int bootTime;

		// Token: 0x04005B2A RID: 23338
		private bool didTrackSession;

		// Token: 0x04005B2B RID: 23339
		private DateTime serverDateTime = DateTime.MinValue;

		// Token: 0x04005B2C RID: 23340
		private DateTime localTime = DateTime.UtcNow;

		// Token: 0x02000806 RID: 2054
		[Serializable]
		public class ClockInfo
		{
			// Token: 0x04005B2E RID: 23342
			public int lastBootTime;

			// Token: 0x04005B2F RID: 23343
			public double lastUptime;
		}
	}
}
