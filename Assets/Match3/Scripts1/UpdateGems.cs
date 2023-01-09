using Match3.Scripts1.Puzzletown.Match3;
using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1
{
	// Token: 0x020005F8 RID: 1528
	public struct UpdateGems : IMatchGroup, IMatchResult
	{
		// Token: 0x0600273E RID: 10046 RVA: 0x000AE475 File Offset: 0x000AC875
		public UpdateGems(Group group)
		{
			this.group = group;
		}

		// Token: 0x1700065B RID: 1627
		// (get) Token: 0x0600273F RID: 10047 RVA: 0x000AE47E File Offset: 0x000AC87E
		public Group Group
		{
			get
			{
				return this.group;
			}
		}

		// Token: 0x1700065C RID: 1628
		// (get) Token: 0x06002740 RID: 10048 RVA: 0x000AE486 File Offset: 0x000AC886
		public bool ShouldHitAdjacentFields
		{
			get
			{
				return false;
			}
		}

		// Token: 0x040051D5 RID: 20949
		private readonly Group group;
	}
}
