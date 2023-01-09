using System;

// Token: 0x02000810 RID: 2064
namespace Match3.Scripts1
{
	public class ServerBasedClock
	{
		// Token: 0x17000809 RID: 2057
		// (get) Token: 0x060032FE RID: 13054 RVA: 0x000F09E8 File Offset: 0x000EEDE8
		public int Now
		{
			get
			{
				if (!this.IsInitialized)
				{
					return this.UtcNow;
				}
				int num = this.UtcNow - this.lastSavedLocalTime;
				return this.lastSavedServerTime + num;
			}
		}

		// Token: 0x1700080A RID: 2058
		// (get) Token: 0x060032FF RID: 13055 RVA: 0x000F0A1D File Offset: 0x000EEE1D
		public bool IsInitialized
		{
			get
			{
				return this.lastSavedServerTime != -1;
			}
		}

		// Token: 0x1700080B RID: 2059
		// (get) Token: 0x06003300 RID: 13056 RVA: 0x000F0A2B File Offset: 0x000EEE2B
		private int UtcNow
		{
			get
			{
				return DateTime.UtcNow.ToUnixTimeStamp();
			}
		}

		// Token: 0x06003301 RID: 13057 RVA: 0x000F0A37 File Offset: 0x000EEE37
		public void SaveServerTime(DateTime serverTime)
		{
			this.lastSavedServerTime = serverTime.ToUnixTimeStamp();
			this.lastSavedLocalTime = this.UtcNow;
		}

		// Token: 0x04005B4A RID: 23370
		public const int NOT_INITIALIZED = -1;

		// Token: 0x04005B4B RID: 23371
		private int lastSavedServerTime = -1;

		// Token: 0x04005B4C RID: 23372
		private int lastSavedLocalTime;
	}
}
