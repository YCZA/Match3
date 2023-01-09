using System.Collections.Generic;
using Match3.Scripts1.Puzzletown.Match3;
using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1
{
	// Token: 0x020005F5 RID: 1525
	public struct RainbowMatch : IMatchWithGem, IHighlightPattern, IMatchGroup, IMatchResult
	{
		// Token: 0x06002731 RID: 10033 RVA: 0x000AE3B4 File Offset: 0x000AC7B4
		public RainbowMatch(Gem gem, Group group)
		{
			this.group = group;
			this.gem = gem;
			this.highlightPositions = group.Positions;
			this.highlightPositions.Add(gem.position);
		}

		// Token: 0x17000653 RID: 1619
		// (get) Token: 0x06002732 RID: 10034 RVA: 0x000AE3E2 File Offset: 0x000AC7E2
		public Gem Gem
		{
			get
			{
				return this.gem;
			}
		}

		// Token: 0x17000654 RID: 1620
		// (get) Token: 0x06002733 RID: 10035 RVA: 0x000AE3EA File Offset: 0x000AC7EA
		public Group Group
		{
			get
			{
				return this.group;
			}
		}

		// Token: 0x17000655 RID: 1621
		// (get) Token: 0x06002734 RID: 10036 RVA: 0x000AE3F2 File Offset: 0x000AC7F2
		public List<IntVector2> HighlightPositions
		{
			get
			{
				return this.highlightPositions;
			}
		}

		// Token: 0x17000656 RID: 1622
		// (get) Token: 0x06002735 RID: 10037 RVA: 0x000AE3FA File Offset: 0x000AC7FA
		public bool ShouldHitAdjacentFields
		{
			get
			{
				return true;
			}
		}

		// Token: 0x040051CB RID: 20939
		private readonly Gem gem;

		// Token: 0x040051CC RID: 20940
		private readonly Group group;

		// Token: 0x040051CD RID: 20941
		private readonly List<IntVector2> highlightPositions;
	}
}
