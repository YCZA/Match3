using System;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x02000703 RID: 1795
	[Flags]
	public enum M3_BannerType
	{
		// Token: 0x040055B9 RID: 21945
		Undefined = 0,
		// Token: 0x040055BA RID: 21946
		LevelStart = 1,
		// Token: 0x040055BB RID: 21947
		OutOfMoves = 2,
		// Token: 0x040055BC RID: 21948
		Shuffle = 8,
		// Token: 0x040055BD RID: 21949
		OutOfMovesDiscount = 16,
		// Token: 0x040055BE RID: 21950
		OutOfMovesFree = 32,
		// Token: 0x040055BF RID: 21951
		OutOfMovesNoDiamonds = 64
	}
}
