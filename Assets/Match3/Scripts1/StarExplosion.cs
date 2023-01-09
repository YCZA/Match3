using System.Collections.Generic;
using Match3.Scripts1.Puzzletown.Match3;
using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1
{
	// Token: 0x020005EE RID: 1518
	public struct StarExplosion : ILinegemRotatingExplosion, IMatchWithAffectedFields, IMatchGroup, IHighlightPattern, ISwapResult, IExplosionResult, IMatchResult
	{
		// Token: 0x0600270A RID: 9994 RVA: 0x000ADF08 File Offset: 0x000AC308
		public StarExplosion(LineGemExplosion horizontal, LineGemExplosion vertical, IntVector2 from)
		{
			this.center = horizontal.Center;
			this.from = from;
			this.group = horizontal.Group;
			this.fields = horizontal.Fields;
			this.highlightPositions = horizontal.HighlightPositions;
			this.group.RemoveDuplicates(vertical.Group);
			this.group.Merge(vertical.Group);
			this.fields.AddRangeElementsIfNotAlreadyPresent(vertical.Fields, false);
			this.highlightPositions.AddRangeElementsIfNotAlreadyPresent(vertical.HighlightPositions, false);
			this.blockingPosStartHor = horizontal.blockingPosStart;
			this.blockingPosEndHor = horizontal.blockingPosEnd;
			this.blockingPosStartVert = vertical.blockingPosStart;
			this.blockingPosEndVert = vertical.blockingPosEnd;
		}

		// Token: 0x17000634 RID: 1588
		// (get) Token: 0x0600270B RID: 9995 RVA: 0x000ADFD0 File Offset: 0x000AC3D0
		public IntVector2 Center
		{
			get
			{
				return this.center;
			}
		}

		// Token: 0x17000635 RID: 1589
		// (get) Token: 0x0600270C RID: 9996 RVA: 0x000ADFD8 File Offset: 0x000AC3D8
		public IntVector2 From
		{
			get
			{
				return this.from;
			}
		}

		// Token: 0x17000636 RID: 1590
		// (get) Token: 0x0600270D RID: 9997 RVA: 0x000ADFE0 File Offset: 0x000AC3E0
		public Group Group
		{
			get
			{
				return this.group;
			}
		}

		// Token: 0x17000637 RID: 1591
		// (get) Token: 0x0600270E RID: 9998 RVA: 0x000ADFE8 File Offset: 0x000AC3E8
		public bool ShouldHitAdjacentFields
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000638 RID: 1592
		// (get) Token: 0x0600270F RID: 9999 RVA: 0x000ADFEB File Offset: 0x000AC3EB
		public List<IntVector2> Fields
		{
			get
			{
				return this.fields;
			}
		}

		// Token: 0x17000639 RID: 1593
		// (get) Token: 0x06002710 RID: 10000 RVA: 0x000ADFF3 File Offset: 0x000AC3F3
		public List<IntVector2> HighlightPositions
		{
			get
			{
				return this.highlightPositions;
			}
		}

		// Token: 0x040051AC RID: 20908
		public readonly int blockingPosStartHor;

		// Token: 0x040051AD RID: 20909
		public readonly int blockingPosEndHor;

		// Token: 0x040051AE RID: 20910
		public readonly int blockingPosStartVert;

		// Token: 0x040051AF RID: 20911
		public readonly int blockingPosEndVert;

		// Token: 0x040051B0 RID: 20912
		private readonly Group group;

		// Token: 0x040051B1 RID: 20913
		private readonly IntVector2 center;

		// Token: 0x040051B2 RID: 20914
		private readonly IntVector2 from;

		// Token: 0x040051B3 RID: 20915
		private readonly List<IntVector2> fields;

		// Token: 0x040051B4 RID: 20916
		private readonly List<IntVector2> highlightPositions;
	}
}
