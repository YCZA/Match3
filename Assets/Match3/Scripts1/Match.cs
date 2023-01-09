using Match3.Scripts1.Puzzletown.Match3;
using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1
{
	// Token: 0x020005F0 RID: 1520
	public struct Match : IMatchGroup, IMatchResult
	{
		// Token: 0x06002716 RID: 10006 RVA: 0x000AE032 File Offset: 0x000AC432
		public Match(Group group, bool shouldHitAdjacentFields = true)
		{
			this.group = group;
			this.shouldHitAdjacentFields = shouldHitAdjacentFields;
		}

		// Token: 0x1700063E RID: 1598
		// (get) Token: 0x06002717 RID: 10007 RVA: 0x000AE042 File Offset: 0x000AC442
		public Group Group
		{
			get
			{
				return this.group;
			}
		}

		// Token: 0x1700063F RID: 1599
		// (get) Token: 0x06002718 RID: 10008 RVA: 0x000AE04A File Offset: 0x000AC44A
		public bool ShouldHitAdjacentFields
		{
			get
			{
				return this.shouldHitAdjacentFields;
			}
		}

		// Token: 0x040051B8 RID: 20920
		private readonly Group group;

		// Token: 0x040051B9 RID: 20921
		private readonly bool shouldHitAdjacentFields;
	}
}
