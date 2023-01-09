using System;

// Token: 0x020009E0 RID: 2528
namespace Match3.Scripts1
{
	public class TownOptionsFriendData : TownOptionsInboxData
	{
		// Token: 0x040065CD RID: 26061
		public int index;

		// Token: 0x040065CE RID: 26062
		public int level;

		// Token: 0x040065CF RID: 26063
		public int harmony;

		// Token: 0x040065D0 RID: 26064
		public DateTime nextLifeAvailable;

		// Token: 0x040065D1 RID: 26065
		public TownOptionsFriendData.Type type;

		// Token: 0x020009E1 RID: 2529
		public enum Type
		{
			// Token: 0x040065D3 RID: 26067
			None,
			// Token: 0x040065D4 RID: 26068
			Me,
			// Token: 0x040065D5 RID: 26069
			Friend
		}
	}
}
