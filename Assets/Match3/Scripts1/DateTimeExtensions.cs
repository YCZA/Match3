using System;
using System.Globalization;
using Match3.Scripts1.Shared;

// Token: 0x02000AC3 RID: 2755
namespace Match3.Scripts1
{
	public static class DateTimeExtensions
	{
		// Token: 0x0600416E RID: 16750 RVA: 0x001524E4 File Offset: 0x001508E4
		public static DateTime FromUnixTimeStamp(int unixTimeStamp, DateTimeKind kind = DateTimeKind.Utc)
		{
			DateTime result = new DateTime(1970, 1, 1, 0, 0, 0, 0, kind);
			result = result.AddSeconds((double)unixTimeStamp);
			return result;
		}

		// Token: 0x0600416F RID: 16751 RVA: 0x00152510 File Offset: 0x00150910
		public static int ToUnixTimeStamp(this DateTime time)
		{
			return (int)(time - new DateTime(1970, 1, 1)).TotalSeconds;
		}

		// Token: 0x06004170 RID: 16752 RVA: 0x00152538 File Offset: 0x00150938
		public static bool IsInPast(this DateTime time, bool useUtc = true)
		{
			DateTime d = (!useUtc) ? TimeUtil.Now : TimeUtil.UtcNow;
			return (d - time).TotalMilliseconds > 0.0;
		}

		// Token: 0x06004171 RID: 16753 RVA: 0x00152575 File Offset: 0x00150975
		public static bool IsInFuture(this DateTime time, bool useUtc = true)
		{
			return !time.IsInPast(useUtc);
		}

		// Token: 0x06004172 RID: 16754 RVA: 0x00152584 File Offset: 0x00150984
		public static bool IsToday(this DateTime time, bool useUtc = true)
		{
			DateTime dateTime = (!useUtc) ? TimeUtil.Now : TimeUtil.UtcNow;
			return time.Date == dateTime.Date;
		}

		// Token: 0x06004173 RID: 16755 RVA: 0x001525BC File Offset: 0x001509BC
		public static int GetTotalDays(this DateTime time)
		{
			return (time - DateTime.MinValue).Days;
		}

		// Token: 0x06004174 RID: 16756 RVA: 0x001525DC File Offset: 0x001509DC
		public static DateTime NextMidnight(this DateTime time)
		{
			return time.Date.AddDays(1.0);
		}

		// Token: 0x06004175 RID: 16757 RVA: 0x00152604 File Offset: 0x00150A04
		public static DateTime GetNextWeekDay(this DateTime time, DayOfWeek dayOfWeek)
		{
			int num = dayOfWeek - time.DayOfWeek;
			if (num <= 0)
			{
				num += 7;
			}
			return time.AddDays((double)num);
		}

		// Token: 0x06004176 RID: 16758 RVA: 0x00152630 File Offset: 0x00150A30
		public static DateTime SortableDateStringToDateTime(string time)
		{
			DateTime result;
			DateTime.TryParseExact(time, "s", CultureInfo.InvariantCulture, DateTimeStyles.None, out result);
			return result;
		}

		// Token: 0x04006AEE RID: 27374
		public const int DAYS_IN_WEEK = 7;
	}
}
