using System.Collections.Generic;
using Match3.Scripts1.Puzzletown.Match3;
using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1
{
	// Token: 0x02000600 RID: 1536
	public struct CannonExplosion : IMatchGroup, IHighlightPattern, ILinegemRotatingExplosion, IMatchWithAffectedFields, IMatchResult, IExplosionResult
	{
		// Token: 0x06002760 RID: 10080 RVA: 0x000AECCE File Offset: 0x000AD0CE
		public CannonExplosion(IntVector2 cannonPosition, Group group, List<IntVector2> explodingPositions, List<IntVector2> allPositions)
		{
			this.center = cannonPosition;
			this.group = group;
			this.highlightPositions = allPositions;
			this.fields = explodingPositions;
		}

		// Token: 0x1700065F RID: 1631
		// (get) Token: 0x06002761 RID: 10081 RVA: 0x000AECED File Offset: 0x000AD0ED
		public Group Group
		{
			get
			{
				return this.group;
			}
		}

		// Token: 0x17000660 RID: 1632
		// (get) Token: 0x06002762 RID: 10082 RVA: 0x000AECF5 File Offset: 0x000AD0F5
		public IntVector2 Center
		{
			get
			{
				return this.center;
			}
		}

		// Token: 0x17000661 RID: 1633
		// (get) Token: 0x06002763 RID: 10083 RVA: 0x000AECFD File Offset: 0x000AD0FD
		public List<IntVector2> Fields
		{
			get
			{
				return this.fields;
			}
		}

		// Token: 0x17000662 RID: 1634
		// (get) Token: 0x06002764 RID: 10084 RVA: 0x000AED05 File Offset: 0x000AD105
		public List<IntVector2> HighlightPositions
		{
			get
			{
				return this.highlightPositions;
			}
		}

		// Token: 0x17000663 RID: 1635
		// (get) Token: 0x06002765 RID: 10085 RVA: 0x000AED0D File Offset: 0x000AD10D
		public bool ShouldHitAdjacentFields
		{
			get
			{
				return false;
			}
		}

		// Token: 0x040051E3 RID: 20963
		private readonly Group group;

		// Token: 0x040051E4 RID: 20964
		private readonly IntVector2 center;

		// Token: 0x040051E5 RID: 20965
		private readonly List<IntVector2> fields;

		// Token: 0x040051E6 RID: 20966
		private readonly List<IntVector2> highlightPositions;
	}
}
