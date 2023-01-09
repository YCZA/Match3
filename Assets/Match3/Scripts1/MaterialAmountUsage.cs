using System;

// Token: 0x020006D9 RID: 1753
namespace Match3.Scripts1
{
	[Flags]
	public enum MaterialAmountUsage
	{
		// Token: 0x040054B6 RID: 21686
		Undefined = 0,
		// Token: 0x040054B7 RID: 21687
		Income = 1,
		// Token: 0x040054B8 RID: 21688
		Price = 2,
		// Token: 0x040054B9 RID: 21689
		Purchase = 4,
		// Token: 0x040054BA RID: 21690
		Reward = 8
	}
}
