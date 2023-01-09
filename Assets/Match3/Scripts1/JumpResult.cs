using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1
{
	// Token: 0x020005EB RID: 1515
	public struct JumpResult : IMatchResult
	{
		// Token: 0x060026F9 RID: 9977 RVA: 0x000AD977 File Offset: 0x000ABD77
		public JumpResult(IntVector2 origin, IntVector2 target, GemColor color)
		{
			this.origin = origin;
			this.target = target;
			this.color = color;
		}

		// Token: 0x0400519C RID: 20892
		public IntVector2 origin;

		// Token: 0x0400519D RID: 20893
		public IntVector2 target;

		// Token: 0x0400519E RID: 20894
		public GemColor color;
	}
}
