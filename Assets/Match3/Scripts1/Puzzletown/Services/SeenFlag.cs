using System;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x020007AC RID: 1964
	[Serializable]
	public class SeenFlag
	{
		// Token: 0x1700077D RID: 1917
		// (get) Token: 0x06003019 RID: 12313 RVA: 0x000E1CC0 File Offset: 0x000E00C0
		// (set) Token: 0x0600301A RID: 12314 RVA: 0x000E1CCE File Offset: 0x000E00CE
		public DateTime TimeStamp
		{
			get
			{
				return Scripts1.DateTimeExtensions.FromUnixTimeStamp(this.timestamp, DateTimeKind.Utc);
			}
			set
			{
				this.timestamp = value.ToUniversalTime().ToUnixTimeStamp();
			}
		}

		// Token: 0x0400592E RID: 22830
		public string flagName;

		// Token: 0x0400592F RID: 22831
		public int viewCount;

		// Token: 0x04005930 RID: 22832
		public int timestamp = DateTime.MinValue.ToUnixTimeStamp();
	}
}
