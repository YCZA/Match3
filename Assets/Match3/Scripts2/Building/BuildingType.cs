using System;

// Token: 0x020008E6 RID: 2278
namespace Match3.Scripts2.Building
{
	[Flags]
	public enum BuildingType
	{
		// Token: 0x04005F92 RID: 24466
		Undefined,
		// Token: 0x04005F93 RID: 24467
		Building,
		// Token: 0x04005F94 RID: 24468
		Monument,
		// Token: 0x04005F95 RID: 24469
		Flower = 4,
		// Token: 0x04005F96 RID: 24470
		Road = 8,
		// Token: 0x04005F97 RID: 24471
		Walkable = 16,
		// Token: 0x04005F98 RID: 24472
		Deco = 14,
		// Token: 0x04005F99 RID: 24473
		Rubble = 32,
		// Token: 0x04005F9A RID: 24474
		Unique = 64,
		// Token: 0x04005F9B RID: 24475
		Trophy = 128,
		// Token: 0x04005F9C RID: 24476
		Storage = 256,
		// Token: 0x04005F9D RID: 24477
		Alphabet = 512,
		// Token: 0x04005F9E RID: 24478
		Friend = 1024
	}
}
