using System;

// Token: 0x02000A1E RID: 2590
namespace Match3.Scripts1
{
	[Flags]
	public enum AdSpinOperation
	{
		// Token: 0x04006743 RID: 26435
		None = 0,
		// Token: 0x04006744 RID: 26436
		Close = 1,
		// Token: 0x04006745 RID: 26437
		Spin = 2,
		// Token: 0x04006746 RID: 26438
		JackpotDetails = 4,
		// Token: 0x04006747 RID: 26439
		WatchAd = 8
	}
}
