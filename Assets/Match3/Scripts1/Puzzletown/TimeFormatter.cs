using System;
using System.Text;

namespace Match3.Scripts1.Puzzletown
{
	// Token: 0x02000888 RID: 2184
	public static class TimeFormatter
	{
		// Token: 0x06003590 RID: 13712 RVA: 0x00101318 File Offset: 0x000FF718
		public static string Duration(int seconds, ILocalizationService localization)
		{
			TimeSpan timeSpan = TimeSpan.FromSeconds((double)seconds);
			if (timeSpan.Hours > 0)
			{
				return string.Format(localization.GetText("ui.timer.format.hours_minutes_seconds", new LocaParam[0]), timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
			}
			if (timeSpan.Minutes > 0)
			{
				return string.Format(localization.GetText("ui.timer.format.minutes_seconds", new LocaParam[0]), timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
			}
			return string.Format(localization.GetText("ui.timer.format.seconds", new LocaParam[0]), timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
		}

		// Token: 0x06003591 RID: 13713 RVA: 0x001013F8 File Offset: 0x000FF7F8
		public static string FormatTime(TimeSpan time)
		{
			if (time.Hours <= 0)
			{
				return string.Format("{0:D2}:{1:D2}", time.Minutes, time.Seconds);
			}
			if (time.Hours > 9)
			{
				return string.Format("{0:D2}:{1:D2}:{2:D2}", time.Hours, time.Minutes, time.Seconds);
			}
			return string.Format("{0:D1}:{1:D2}:{2:D2}", time.Hours, time.Minutes, time.Seconds);
		}

		// Token: 0x06003592 RID: 13714 RVA: 0x001014A0 File Offset: 0x000FF8A0
		public static string ShortDuration(int seconds, ILocalizationService localization)
		{
			TimeSpan timeSpan = TimeSpan.FromSeconds((double)seconds);
			string text = localization.GetText("ui.timer.hours", new LocaParam[0]);
			string text2 = localization.GetText("ui.timer.minutes", new LocaParam[0]);
			string text3 = localization.GetText("ui.timer.seconds", new LocaParam[0]);
			TimeFormatter.sb.Length = 0;
			string value;
			string value2;
			int value3;
			int num;
			if (timeSpan.Hours > 0)
			{
				value = text;
				value2 = text2;
				value3 = timeSpan.Hours;
				num = timeSpan.Minutes;
			}
			else
			{
				if (timeSpan.Minutes <= 0)
				{
					return timeSpan.Seconds + text3;
				}
				value = text2;
				value2 = text3;
				value3 = timeSpan.Minutes;
				num = timeSpan.Seconds;
			}
			TimeFormatter.sb.Append(value3);
			TimeFormatter.sb.Append(value);
			if (num > 0)
			{
				TimeFormatter.sb.Append(" ");
				TimeFormatter.sb.Append(num);
				TimeFormatter.sb.Append(value2);
			}
			return TimeFormatter.sb.ToString();
		}

		// Token: 0x04005D8A RID: 23946
		private static StringBuilder sb = new StringBuilder(16);
	}
}
