using System;
using System.Collections.Generic;
using System.Linq;
using Match3.Scripts1.Puzzletown.Services;
using UnityEngine;

// Token: 0x0200081B RID: 2075
namespace Match3.Scripts1
{
	[Serializable]
	public class PTRoundTracker
	{
		// Token: 0x06003359 RID: 13145 RVA: 0x000F408C File Offset: 0x000F248C
		public PTRoundTracker()
		{
			this.Reset(DateTime.UtcNow);
		}

		// Token: 0x17000817 RID: 2071
		// (get) Token: 0x0600335A RID: 13146 RVA: 0x000F409F File Offset: 0x000F249F
		public int RoundCountPlayed
		{
			get
			{
				return this.RoundsPlayed;
			}
		}

		// Token: 0x0600335B RID: 13147 RVA: 0x000F40A8 File Offset: 0x000F24A8
		private int HoursSinceReset(DateTime currentTime)
		{
			return (currentTime - PTRoundTracker.ConvertFromUnixTimestamp(this.TrackStartTime)).Hours;
		}

		// Token: 0x0600335C RID: 13148 RVA: 0x000F40D0 File Offset: 0x000F24D0
		private int RoundsSince(DateTime currentTime, TimeSpan span)
		{
			if (this.SessionLog.Count == 0)
			{
				return 0;
			}
			DateTime t = currentTime - span;
			int num = 0;
			int num2 = this.SessionLog.Count - 1;
			DateTime t2 = PTRoundTracker.ConvertFromUnixTimestamp(this.SessionLog[num2]);
			while (t2 > t && num <= num2)
			{
				t2 = PTRoundTracker.ConvertFromUnixTimestamp(this.SessionLog[num2 - num]);
				num++;
			}
			return num;
		}

		// Token: 0x0600335D RID: 13149 RVA: 0x000F414C File Offset: 0x000F254C
		public void CompactLog(DateTime currentTime, TimeSpan keepFor)
		{
			int num = this.RoundsSince(currentTime, keepFor);
			this.SessionLog = this.SessionLog.Skip(this.SessionLog.Count - num).ToList<double>();
		}

		// Token: 0x0600335E RID: 13150 RVA: 0x000F4188 File Offset: 0x000F2588
		public void Reset(DateTime resetTime)
		{
			this.TrackStartTime = PTRoundTracker.ConvertToUnixTimestamp(resetTime);
			this.SessionLog = new List<double>();
			this.First24H = 0;
			this.ConsecutiveWin = 0;
			this.ConsecutiveLoss = 0;
			this.RoundsPlayed = 0;
			this.RoundsWon = 0;
			this.RoundsLost = 0;
		}

		// Token: 0x0600335F RID: 13151 RVA: 0x000F41D8 File Offset: 0x000F25D8
		public void RegisterRound(TrackingService.RoundOutcome outcome)
		{
			DateTime utcNow = DateTime.UtcNow;
			this.SessionLog.Add(PTRoundTracker.ConvertToUnixTimestamp(utcNow));
			if (this.HoursSinceReset(utcNow) <= 24)
			{
				this.First24H++;
			}
			this.RoundsPlayed++;
			if (outcome == TrackingService.RoundOutcome.Won)
			{
				this.RegisterWin();
			}
			else
			{
				this.RegisterLoss();
			}
		}

		// Token: 0x06003360 RID: 13152 RVA: 0x000F423E File Offset: 0x000F263E
		private void RegisterWin()
		{
			this.ConsecutiveWin++;
			this.ConsecutiveLoss = 0;
			this.RoundsWon++;
		}

		// Token: 0x06003361 RID: 13153 RVA: 0x000F4263 File Offset: 0x000F2663
		private void RegisterLoss()
		{
			this.ConsecutiveWin = 0;
			this.ConsecutiveLoss++;
			this.RoundsLost++;
		}

		// Token: 0x06003362 RID: 13154 RVA: 0x000F4288 File Offset: 0x000F2688
		public void PopulateTracking(DateTime currentTime, Dictionary<string, object> data)
		{
			data["tws"] = this.ConsecutiveWin;
			data["tfs"] = this.ConsecutiveLoss;
			data["rpf24"] = this.First24H;
			data["rp24"] = this.RoundsSince(currentTime, PTRoundTracker.TwentyFourH);
			data["rp_7d"] = this.RoundsSince(currentTime, PTRoundTracker.SevenD);
		}

		// Token: 0x06003363 RID: 13155 RVA: 0x000F430F File Offset: 0x000F270F
		public string GetJson()
		{
			return JsonUtility.ToJson(this);
		}

		// Token: 0x06003364 RID: 13156 RVA: 0x000F4317 File Offset: 0x000F2717
		public void FromJson(string json)
		{
			if (string.IsNullOrEmpty(json))
			{
				return;
			}
			JsonUtility.FromJsonOverwrite(json, this);
		}

		// Token: 0x06003365 RID: 13157 RVA: 0x000F432C File Offset: 0x000F272C
		public int GetRoundsCountSevenDays()
		{
			DateTime utcNow = DateTime.UtcNow;
			return this.RoundsSince(utcNow, PTRoundTracker.SevenD);
		}

		// Token: 0x06003366 RID: 13158 RVA: 0x000F434C File Offset: 0x000F274C
		private static DateTime ConvertFromUnixTimestamp(double timestamp)
		{
			DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
			return dateTime.AddSeconds(timestamp);
		}

		// Token: 0x06003367 RID: 13159 RVA: 0x000F4374 File Offset: 0x000F2774
		private static double ConvertToUnixTimestamp(DateTime date)
		{
			DateTime d = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
			return Math.Floor((date.ToUniversalTime() - d).TotalSeconds);
		}

		// Token: 0x06003368 RID: 13160 RVA: 0x000F43B0 File Offset: 0x000F27B0
		public void PopulateGameTracking(Dictionary<string, object> data)
		{
			data["tr_pit"] = this.RoundsPlayed;
			data["trl_pit"] = this.RoundsLost;
			data["trw_pit"] = this.RoundsWon;
		}

		// Token: 0x04005B83 RID: 23427
		[SerializeField]
		private double TrackStartTime;

		// Token: 0x04005B84 RID: 23428
		[SerializeField]
		private List<double> SessionLog;

		// Token: 0x04005B85 RID: 23429
		[SerializeField]
		private int First24H;

		// Token: 0x04005B86 RID: 23430
		[SerializeField]
		private int ConsecutiveWin;

		// Token: 0x04005B87 RID: 23431
		[SerializeField]
		private int ConsecutiveLoss;

		// Token: 0x04005B88 RID: 23432
		[SerializeField]
		private int RoundsPlayed;

		// Token: 0x04005B89 RID: 23433
		[SerializeField]
		private int RoundsWon;

		// Token: 0x04005B8A RID: 23434
		[SerializeField]
		private int RoundsLost;

		// Token: 0x04005B8B RID: 23435
		private static readonly TimeSpan TwentyFourH = new TimeSpan(24, 0, 0);

		// Token: 0x04005B8C RID: 23436
		private static readonly TimeSpan SevenD = new TimeSpan(7, 0, 0, 0);
	}
}
