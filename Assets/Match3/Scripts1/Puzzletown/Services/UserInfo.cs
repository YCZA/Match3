using System;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x020007DF RID: 2015
	[Serializable]
	public class UserInfo
	{
		// Token: 0x060031A7 RID: 12711 RVA: 0x000E9859 File Offset: 0x000E7C59
		public UserInfo(int notificationId)
		{
			this.NotificationId = notificationId;
		}

		// Token: 0x04005A37 RID: 23095
		public int NotificationId = -1;
	}
}
