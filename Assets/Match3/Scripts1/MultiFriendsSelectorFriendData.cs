using Match3.Scripts1.Wooga.Signals;
using UnityEngine;

// Token: 0x020008B0 RID: 2224
namespace Match3.Scripts1
{
	public class MultiFriendsSelectorFriendData
	{
		// Token: 0x04005E51 RID: 24145
		public FacebookData.Friend friend;

		// Token: 0x04005E52 RID: 24146
		public MultiFriendsSelectorFriendData.State CurrentState;

		// Token: 0x04005E53 RID: 24147
		public Sprite avatar;

		// Token: 0x04005E54 RID: 24148
		public readonly Signal<string, Sprite> OnAvatarAvailable = new Signal<string, Sprite>();

		// Token: 0x020008B1 RID: 2225
		public enum State
		{
			// Token: 0x04005E56 RID: 24150
			Selected,
			// Token: 0x04005E57 RID: 24151
			Unselected
		}
	}
}
