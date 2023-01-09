using System.Collections.Generic;
using Match3.Scripts1.Puzzletown.Match3;
using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1
{
	// Token: 0x020005EF RID: 1519
	public struct LineGemMatch : IMatchWithGem, IHighlightPattern, IMatchGroup, IMatchResult
	{
		// Token: 0x06002711 RID: 10001 RVA: 0x000ADFFB File Offset: 0x000AC3FB
		public LineGemMatch(Gem gem, Group group)
		{
			this.gem = gem;
			this.group = group;
			this.highlightPositions = group.Positions;
		}

		// Token: 0x1700063A RID: 1594
		// (get) Token: 0x06002712 RID: 10002 RVA: 0x000AE017 File Offset: 0x000AC417
		public Gem Gem
		{
			get
			{
				return this.gem;
			}
		}

		// Token: 0x1700063B RID: 1595
		// (get) Token: 0x06002713 RID: 10003 RVA: 0x000AE01F File Offset: 0x000AC41F
		public Group Group
		{
			get
			{
				return this.group;
			}
		}

		// Token: 0x1700063C RID: 1596
		// (get) Token: 0x06002714 RID: 10004 RVA: 0x000AE027 File Offset: 0x000AC427
		public List<IntVector2> HighlightPositions
		{
			get
			{
				return this.highlightPositions;
			}
		}

		// Token: 0x1700063D RID: 1597
		// (get) Token: 0x06002715 RID: 10005 RVA: 0x000AE02F File Offset: 0x000AC42F
		public bool ShouldHitAdjacentFields
		{
			get
			{
				return true;
			}
		}

		// Token: 0x040051B5 RID: 20917
		private readonly Gem gem;

		// Token: 0x040051B6 RID: 20918
		private readonly Group group;

		// Token: 0x040051B7 RID: 20919
		private readonly List<IntVector2> highlightPositions;
	}
}
