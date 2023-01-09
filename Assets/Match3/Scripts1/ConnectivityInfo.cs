using System;

namespace Match3.Scripts1
{
	// Token: 0x020003A1 RID: 929
	public struct ConnectivityInfo
	{
		// Token: 0x06001C0F RID: 7183 RVA: 0x0007BA7A File Offset: 0x00079E7A
		public ConnectivityInfo(bool HasConnectivity, DateTime CreationUtcDateTime)
		{
			this.HasConnectivity = HasConnectivity;
			this.CreationUtcDateTime = CreationUtcDateTime;
		}

		// Token: 0x04004981 RID: 18817
		public DateTime CreationUtcDateTime;

		// Token: 0x04004982 RID: 18818
		public bool HasConnectivity;
	}
}
