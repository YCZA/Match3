using System;
using Wooga.Core.Utilities;

namespace Match3.Scripts1.Shared
{
	// Token: 0x02000B55 RID: 2901
	public static class TimeUtil
	{
		// Token: 0x170009BE RID: 2494
		// (get) Token: 0x060043D9 RID: 17369 RVA: 0x0015A5B3 File Offset: 0x001589B3
		public static DateTime UtcNow
		{
			get
			{
				return Scripts1.DateTimeExtensions.FromUnixTimeStamp(TimeUtil.UtcNowEpoch, DateTimeKind.Utc);
			}
		}

		// Token: 0x170009BF RID: 2495
		// (get) Token: 0x060043DA RID: 17370 RVA: 0x0015A5C0 File Offset: 0x001589C0
		public static int UtcNowEpoch
		{
			get
			{
				return TimeUtil.serverEpochTime + TimeUtil.SessionDuration;
			}
		}

		// Token: 0x170009C0 RID: 2496
		// (get) Token: 0x060043DB RID: 17371 RVA: 0x0015A5D0 File Offset: 0x001589D0
		public static DateTime Now
		{
			get
			{
				return TimeUtil.UtcNow.ToLocalTime();
			}
		}

		// Token: 0x170009C1 RID: 2497
		// (get) Token: 0x060043DC RID: 17372 RVA: 0x0015A5EA File Offset: 0x001589EA
		public static int UtcNowDeviceEpoch
		{
			get
			{
				return Time.EpochTime();
			}
		}

		// Token: 0x170009C2 RID: 2498
		// (get) Token: 0x060043DD RID: 17373 RVA: 0x0015A5F1 File Offset: 0x001589F1
		public static DateTime UtcNowDevice
		{
			get
			{
				return Scripts1.DateTimeExtensions.FromUnixTimeStamp(TimeUtil.UtcNowDeviceEpoch, DateTimeKind.Utc);
			}
		}

		// Token: 0x170009C3 RID: 2499
		// (get) Token: 0x060043DE RID: 17374 RVA: 0x0015A5FE File Offset: 0x001589FE
		public static int SessionDuration
		{
			get
			{
				return TimeUtil.UtcNowDeviceEpoch - TimeUtil.deviceEpochTime;
			}
		}

		// Token: 0x060043DF RID: 17375 RVA: 0x0015A60C File Offset: 0x00158A0C
		public static void Init(int serverEpochTime)
		{
			object @lock = TimeUtil._lock;
			lock (@lock)
			{
				TimeUtil.serverEpochTime = serverEpochTime;
				TimeUtil.deviceEpochTime = TimeUtil.UtcNowDeviceEpoch;
			}
		}

		// Token: 0x060043E0 RID: 17376 RVA: 0x0015A654 File Offset: 0x00158A54
		public static int GetUtcNowEpochTimePlus(TimeSpan timeSpan)
		{
			return TimeUtil.UtcNowEpoch + (int)timeSpan.TotalSeconds;
		}

		// Token: 0x060043E1 RID: 17377 RVA: 0x0015A664 File Offset: 0x00158A64
		public static DateTime GetUtcNowDateTimePlus(int seconds)
		{
			return Scripts1.DateTimeExtensions.FromUnixTimeStamp(TimeUtil.UtcNowEpoch + seconds, DateTimeKind.Utc);
		}

		// Token: 0x060043E2 RID: 17378 RVA: 0x0015A673 File Offset: 0x00158A73
		public static DateTime GetUtcNow()
		{
			return TimeUtil.UtcNow;
		}

		// Token: 0x04006C38 RID: 27704
		private static int serverEpochTime;

		// Token: 0x04006C39 RID: 27705
		private static int deviceEpochTime;

		// Token: 0x04006C3A RID: 27706
		private static readonly object _lock = new object();
	}
}
