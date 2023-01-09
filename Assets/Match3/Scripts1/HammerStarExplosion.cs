using System.Collections.Generic;
using Match3.Scripts1.Puzzletown.Match3;
using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1
{
	// Token: 0x020005DB RID: 1499
	public struct HammerStarExplosion : ILinegemRotatingExplosion, IMatchWithAffectedFields, IMatchGroup, IHighlightPattern, IExplosionResult, IMatchResult
	{
		// Token: 0x060026DA RID: 9946 RVA: 0x000AD8F4 File Offset: 0x000ABCF4
		public HammerStarExplosion(StarExplosion explosion)
		{
			this.explosion = explosion;
		}

		// Token: 0x1700060F RID: 1551
		// (get) Token: 0x060026DB RID: 9947 RVA: 0x000AD8FD File Offset: 0x000ABCFD
		public Group Group
		{
			get
			{
				return this.explosion.Group;
			}
		}

		// Token: 0x17000610 RID: 1552
		// (get) Token: 0x060026DC RID: 9948 RVA: 0x000AD90A File Offset: 0x000ABD0A
		public bool ShouldHitAdjacentFields
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000611 RID: 1553
		// (get) Token: 0x060026DD RID: 9949 RVA: 0x000AD90D File Offset: 0x000ABD0D
		public IntVector2 Center
		{
			get
			{
				return this.explosion.Center;
			}
		}

		// Token: 0x17000612 RID: 1554
		// (get) Token: 0x060026DE RID: 9950 RVA: 0x000AD91A File Offset: 0x000ABD1A
		public List<IntVector2> Fields
		{
			get
			{
				return this.explosion.Fields;
			}
		}

		// Token: 0x17000613 RID: 1555
		// (get) Token: 0x060026DF RID: 9951 RVA: 0x000AD927 File Offset: 0x000ABD27
		public List<IntVector2> HighlightPositions
		{
			get
			{
				return this.explosion.HighlightPositions;
			}
		}

		// Token: 0x04005197 RID: 20887
		public StarExplosion explosion;
	}
}
