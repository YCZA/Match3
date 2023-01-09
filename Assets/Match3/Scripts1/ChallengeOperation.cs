using System;

// Token: 0x0200075D RID: 1885
namespace Match3.Scripts1
{
	[Flags]
	public enum ChallengeOperation
	{
		// Token: 0x040057EF RID: 22511
		None = 0,
		// Token: 0x040057F0 RID: 22512
		Close = 1,
		// Token: 0x040057F1 RID: 22513
		WatchAd = 2,
		// Token: 0x040057F2 RID: 22514
		Play = 4,
		// Token: 0x040057F3 RID: 22515
		OpenMysteryBox = 8,
		// Token: 0x040057F4 RID: 22516
		AddTime = 16,
		// Token: 0x040057F5 RID: 22517
		NoThanks = 32,
		// Token: 0x040057F6 RID: 22518
		Help = 64,
		// Token: 0x040057F7 RID: 22519
		AddPaws = 128
	}
}
