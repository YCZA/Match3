using Match3.Scripts1.Wooga.Signals;
using UnityEngine;

// Token: 0x020009E6 RID: 2534
namespace Match3.Scripts1
{
	public class TownOptionsInboxData
	{
		// Token: 0x040065E7 RID: 26087
		public FacebookData.Friend friend;

		// Token: 0x040065E8 RID: 26088
		public string requestId;

		// Token: 0x040065E9 RID: 26089
		public Sprite avatar;

		// Token: 0x040065EA RID: 26090
		public bool enableSend;

		// Token: 0x040065EB RID: 26091
		public readonly Signal<string, Sprite> OnAvatarAvailable = new Signal<string, Sprite>();
	}
}
