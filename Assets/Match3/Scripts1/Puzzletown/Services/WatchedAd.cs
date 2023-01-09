using System;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x0200079F RID: 1951
	[Serializable]
	public class WatchedAd
	{
		// Token: 0x06002FD0 RID: 12240 RVA: 0x000E0F85 File Offset: 0x000DF385
		public WatchedAd()
		{
		}

		// Token: 0x06002FD1 RID: 12241 RVA: 0x000E0F8D File Offset: 0x000DF38D
		public WatchedAd(string placement, int count)
		{
			this.placement = placement;
			this.count = count;
		}

		// Token: 0x040058F0 RID: 22768
		public string placement;

		// Token: 0x040058F1 RID: 22769
		public int count;
	}
}
