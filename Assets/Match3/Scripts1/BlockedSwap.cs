using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1
{
	// Token: 0x020005C8 RID: 1480
	public struct BlockedSwap : IMatchResult
	{
		// Token: 0x06002684 RID: 9860 RVA: 0x000ACDB7 File Offset: 0x000AB1B7
		public BlockedSwap(IntVector2 from, IntVector2 to)
		{
			this.from = from;
			this.to = to;
		}

		// Token: 0x0400515E RID: 20830
		public IntVector2 from;

		// Token: 0x0400515F RID: 20831
		public IntVector2 to;
	}
}
