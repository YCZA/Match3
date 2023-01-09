using System.Collections.Generic;
using Match3.Scripts1.Puzzletown.Match3;
using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1
{
	// Token: 0x020005CA RID: 1482
	public struct BombMatch : IMatchWithGem, IHighlightPattern, IMatchGroup, IMatchResult
	{
		// Token: 0x0600268D RID: 9869 RVA: 0x000AD2AB File Offset: 0x000AB6AB
		public BombMatch(Gem gem, Group group)
		{
			this.group = group;
			this.gem = gem;
			this.highlightPositions = group.Positions;
		}

		// Token: 0x170005DF RID: 1503
		// (get) Token: 0x0600268E RID: 9870 RVA: 0x000AD2C7 File Offset: 0x000AB6C7
		public Gem Gem
		{
			get
			{
				return this.gem;
			}
		}

		// Token: 0x170005E0 RID: 1504
		// (get) Token: 0x0600268F RID: 9871 RVA: 0x000AD2CF File Offset: 0x000AB6CF
		public Group Group
		{
			get
			{
				return this.group;
			}
		}

		// Token: 0x170005E1 RID: 1505
		// (get) Token: 0x06002690 RID: 9872 RVA: 0x000AD2D7 File Offset: 0x000AB6D7
		public List<IntVector2> HighlightPositions
		{
			get
			{
				return this.highlightPositions;
			}
		}

		// Token: 0x170005E2 RID: 1506
		// (get) Token: 0x06002691 RID: 9873 RVA: 0x000AD2DF File Offset: 0x000AB6DF
		public bool ShouldHitAdjacentFields
		{
			get
			{
				return true;
			}
		}

		// Token: 0x04005168 RID: 20840
		private readonly Gem gem;

		// Token: 0x04005169 RID: 20841
		private readonly Group group;

		// Token: 0x0400516A RID: 20842
		private readonly List<IntVector2> highlightPositions;
	}
}
