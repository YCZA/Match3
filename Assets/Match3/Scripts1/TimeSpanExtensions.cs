using System;
using System.Collections.Generic;
using System.Text;

// Token: 0x02000AD4 RID: 2772
namespace Match3.Scripts1
{
	public static class TimeSpanExtensions
	{
		// Token: 0x060041C0 RID: 16832 RVA: 0x00153594 File Offset: 0x00151994
		public static string ToSimpleString(this TimeSpan timeSpan)
		{
			string text = string.Format("{0:00}:{1:00}:{2:00}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
			if (timeSpan.TotalDays >= 1.0)
			{
				text = string.Format("{0:00}:{1}", (int)timeSpan.TotalDays, text);
			}
			return text;
		}

		// Token: 0x060041C1 RID: 16833 RVA: 0x001535FF File Offset: 0x001519FF
		public static string ToMinuteAndSecondsString(this TimeSpan timeSpan, bool ceilSeconds = true)
		{
			if (ceilSeconds)
			{
				timeSpan = TimeSpan.FromSeconds(Math.Round(timeSpan.TotalSeconds));
			}
			return string.Format("{0:00}:{1:00}", timeSpan.Minutes, timeSpan.Seconds);
		}

		// Token: 0x060041C2 RID: 16834 RVA: 0x0015363C File Offset: 0x00151A3C
		public static string ToString(this TimeSpan timeSpan, int compartments, Dictionary<TimeSpanField, string[]> fieldSuffixes = null, bool addLastSuffix = false, TimeSpanField minimumField = TimeSpanField.Second, TimeSpanField maxField = TimeSpanField.Day)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			int num2 = (int)(TimeSpanField.Millisecond - minimumField);
			fieldSuffixes = (fieldSuffixes ?? TimeSpanExtensions.DEFAULT_SUFFIXES);
			string text = null;
			if (maxField <= TimeSpanField.Day && ((int)timeSpan.TotalDays > 0 || num < compartments - 4 + num2) && num < compartments)
			{
				text = TimeSpanExtensions.AddToString(stringBuilder, TimeSpanField.Day, maxField, timeSpan.TotalDays, (double)timeSpan.Days, "{0:0}{1}", fieldSuffixes);
				num++;
			}
			if (maxField <= TimeSpanField.Hour && ((int)timeSpan.TotalHours > 0 || num > 0 || num < compartments - 3 + num2) && num < compartments)
			{
				text = TimeSpanExtensions.AddToString(stringBuilder, TimeSpanField.Hour, maxField, timeSpan.TotalHours, (double)timeSpan.Hours, "{0:00}{1}", fieldSuffixes);
				num++;
			}
			if (maxField <= TimeSpanField.Minute && ((int)timeSpan.TotalMinutes > 0 || num > 0 || num < compartments - 2 + num2) && num < compartments)
			{
				text = TimeSpanExtensions.AddToString(stringBuilder, TimeSpanField.Minute, maxField, timeSpan.TotalMinutes, (double)timeSpan.Minutes, "{0:00}{1}", fieldSuffixes);
				num++;
			}
			if (maxField <= TimeSpanField.Second && ((int)timeSpan.TotalSeconds > 0 || num > 0 || num < compartments - 1 + num2) && num < compartments)
			{
				text = TimeSpanExtensions.AddToString(stringBuilder, TimeSpanField.Second, maxField, timeSpan.TotalSeconds, (double)timeSpan.Seconds, "{0:00}{1}", fieldSuffixes);
				num++;
			}
			if (maxField <= TimeSpanField.Millisecond && ((int)timeSpan.TotalMilliseconds > 0 || num > num2) && num < compartments)
			{
				text = TimeSpanExtensions.AddToString(stringBuilder, TimeSpanField.Millisecond, maxField, timeSpan.TotalMilliseconds, (double)timeSpan.Milliseconds, "{0:000}{1}", fieldSuffixes);
			}
			if (!addLastSuffix && text != null)
			{
				stringBuilder.Remove(stringBuilder.Length - text.Length, text.Length);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060041C3 RID: 16835 RVA: 0x00153818 File Offset: 0x00151C18
		private static string AddToString(StringBuilder builder, TimeSpanField field, TimeSpanField maxField, double amountTotal, double amountSingle, string format, Dictionary<TimeSpanField, string[]> suffixes)
		{
			double num = (maxField - field != 0) ? amountSingle : amountTotal;
			string suffix = TimeSpanExtensions.GetSuffix(field, num, suffixes);
			builder.Append(string.Format(format, num, suffix));
			return suffix;
		}

		// Token: 0x060041C4 RID: 16836 RVA: 0x00153856 File Offset: 0x00151C56
		private static string GetSuffix(TimeSpanField field, double amount, Dictionary<TimeSpanField, string[]> suffixes)
		{
			return suffixes.GetValueOrDefault(field, new string[]
			{
				":",
				":"
			})[(amount < 2.0) ? 0 : 1];
		}

		// Token: 0x04006AF2 RID: 27378
		private static readonly Dictionary<TimeSpanField, string[]> DEFAULT_SUFFIXES = new Dictionary<TimeSpanField, string[]>
		{
			{
				TimeSpanField.Day,
				new string[]
				{
					"d ",
					"d "
				}
			},
			{
				TimeSpanField.Hour,
				new string[]
				{
					":",
					":"
				}
			},
			{
				TimeSpanField.Minute,
				new string[]
				{
					":",
					":"
				}
			},
			{
				TimeSpanField.Second,
				new string[]
				{
					":",
					":"
				}
			}
		};
	}
}
