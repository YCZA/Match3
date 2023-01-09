using System;

// Token: 0x02000A22 RID: 2594
namespace Match3.Scripts1
{
	[Flags]
	public enum PopupOperation
	{
		// Token: 0x04006756 RID: 26454
		None = 0,
		// Token: 0x04006757 RID: 26455
		Close = 1,
		// Token: 0x04006758 RID: 26456
		OK = 2,
		// Token: 0x04006759 RID: 26457
		Details = 4,
		// Token: 0x0400675A RID: 26458
		Back = 8,
		// Token: 0x0400675B RID: 26459
		Skip = 16
	}
}
