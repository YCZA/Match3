using System;

// Token: 0x020008A8 RID: 2216
namespace Match3.Scripts1
{
	[Flags]
	public enum MultiFriendsSelectOperation
	{
		// Token: 0x04005E2B RID: 24107
		Undefined = 0,
		// Token: 0x04005E2C RID: 24108
		SelectAll = 1,
		// Token: 0x04005E2D RID: 24109
		AskSelected = 2,
		// Token: 0x04005E2E RID: 24110
		AskAnyone = 4,
		// Token: 0x04005E2F RID: 24111
		AskSelectedDisabled = 8
	}
}
