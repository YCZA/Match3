using System;

namespace Wooga.Core.Utilities
{
	// Token: 0x020003B7 RID: 951
	public static class Time
	{
		// Token: 0x06001CB9 RID: 7353 RVA: 0x0007D450 File Offset: 0x0007B850
		public static DateTime Now()
		{
			object @lock = Time.Lock;
			DateTime now;
			lock (@lock)
			{
				now = DateTime.Now;
			}
			return now;
		}

		// Token: 0x06001CBA RID: 7354 RVA: 0x0007D48C File Offset: 0x0007B88C
		public static DateTime UtcNow()
		{
			return Time.Now().ToUniversalTime();
		}

		// Token: 0x06001CBB RID: 7355 RVA: 0x0007D4A8 File Offset: 0x0007B8A8
		public static int EpochTime()
		{
			return (int)Time.UtcNow().Subtract(Time.JANUARY_FIRST_1970).TotalSeconds;
		}

		// Token: 0x06001CBC RID: 7356 RVA: 0x0007D4D0 File Offset: 0x0007B8D0
		public static int EpochTime(DateTime time)
		{
			return (int)time.Subtract(Time.JANUARY_FIRST_1970).TotalSeconds;
		}

		// Token: 0x040049A5 RID: 18853
		private static readonly DateTime JANUARY_FIRST_1970 = new DateTime(1970, 1, 1);

		// Token: 0x040049A6 RID: 18854
		private static readonly object Lock = new object();
	}
}
