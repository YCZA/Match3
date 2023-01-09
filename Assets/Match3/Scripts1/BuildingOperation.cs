using System;

// Token: 0x02000962 RID: 2402
namespace Match3.Scripts1
{
	[Flags]
	public enum BuildingOperation
	{
		// Token: 0x0400627C RID: 25212
		None = 0,
		// Token: 0x0400627D RID: 25213
		Confirm = 1,
		// Token: 0x0400627E RID: 25214
		Cancel = 2,
		// Token: 0x0400627F RID: 25215
		ConfirmDisabled = 32,
		// Token: 0x04006280 RID: 25216
		Store = 64,
		// Token: 0x04006281 RID: 25217
		Harvest = 128,
		// Token: 0x04006282 RID: 25218
		Repair = 256,
		// Token: 0x04006283 RID: 25219
		Any = -1
	}
}
