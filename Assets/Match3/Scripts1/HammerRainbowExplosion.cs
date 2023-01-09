using System.Collections.Generic;
using Match3.Scripts1.Puzzletown.Match3;
using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1
{
	// Token: 0x020005DA RID: 1498
	public struct HammerRainbowExplosion : IMatchGroup, IMatchWithAffectedFields, IHighlightPattern, IMatchResult
	{
		// Token: 0x060026D4 RID: 9940 RVA: 0x000AD8B4 File Offset: 0x000ABCB4
		public HammerRainbowExplosion(RainbowExplosion explosion)
		{
			this.explosion = explosion;
		}

		// Token: 0x1700060A RID: 1546
		// (get) Token: 0x060026D5 RID: 9941 RVA: 0x000AD8BD File Offset: 0x000ABCBD
		public Group Group
		{
			get
			{
				return this.explosion.Group;
			}
		}

		// Token: 0x1700060B RID: 1547
		// (get) Token: 0x060026D6 RID: 9942 RVA: 0x000AD8CA File Offset: 0x000ABCCA
		public bool ShouldHitAdjacentFields
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700060C RID: 1548
		// (get) Token: 0x060026D7 RID: 9943 RVA: 0x000AD8CD File Offset: 0x000ABCCD
		public List<IntVector2> Fields
		{
			get
			{
				return this.explosion.Fields;
			}
		}

		// Token: 0x1700060D RID: 1549
		// (get) Token: 0x060026D8 RID: 9944 RVA: 0x000AD8DA File Offset: 0x000ABCDA
		public List<IntVector2> HighlightPositions
		{
			get
			{
				return this.explosion.HighlightPositions;
			}
		}

		// Token: 0x1700060E RID: 1550
		// (get) Token: 0x060026D9 RID: 9945 RVA: 0x000AD8E7 File Offset: 0x000ABCE7
		public IntVector2 Position
		{
			get
			{
				return this.explosion.Center;
			}
		}

		// Token: 0x04005196 RID: 20886
		public RainbowExplosion explosion;
	}
}
