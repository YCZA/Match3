using System.Collections.Generic;
using Match3.Scripts1.Puzzletown.Match3;
using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1
{
	// Token: 0x020005D7 RID: 1495
	public struct FishExplosion : IExplosionResult, IMatchWithAffectedFields, IMatchGroup, IHighlightPattern, IWaterSpreadingResult, IMatchResult
	{
		// Token: 0x060026C3 RID: 9923 RVA: 0x000AD72C File Offset: 0x000ABB2C
		public FishExplosion(Fields fields, IntVector2 position, bool spreadWater = false)
		{
			this.group = new Group();
			this.fields = new List<IntVector2>();
			this.position = position;
			this.highlightPositions = new List<IntVector2>
			{
				position
			};
			this.spreadWater = spreadWater;
			if (!fields[position].GemBlocked && fields[position].gem.IsAffectedBySuperGems)
			{
				this.group.Add(fields[position].gem);
			}
			else
			{
				this.fields.Add(position);
			}
		}

		// Token: 0x170005FD RID: 1533
		// (get) Token: 0x060026C4 RID: 9924 RVA: 0x000AD7C0 File Offset: 0x000ABBC0
		public Group Group
		{
			get
			{
				return this.group;
			}
		}

		// Token: 0x170005FE RID: 1534
		// (get) Token: 0x060026C5 RID: 9925 RVA: 0x000AD7C8 File Offset: 0x000ABBC8
		public bool ShouldHitAdjacentFields
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170005FF RID: 1535
		// (get) Token: 0x060026C6 RID: 9926 RVA: 0x000AD7CB File Offset: 0x000ABBCB
		public List<IntVector2> Fields
		{
			get
			{
				return this.fields;
			}
		}

		// Token: 0x17000600 RID: 1536
		// (get) Token: 0x060026C7 RID: 9927 RVA: 0x000AD7D3 File Offset: 0x000ABBD3
		public IntVector2 Center
		{
			get
			{
				return this.position;
			}
		}

		// Token: 0x17000601 RID: 1537
		// (get) Token: 0x060026C8 RID: 9928 RVA: 0x000AD7DB File Offset: 0x000ABBDB
		public List<IntVector2> HighlightPositions
		{
			get
			{
				return this.highlightPositions;
			}
		}

		// Token: 0x17000602 RID: 1538
		// (get) Token: 0x060026C9 RID: 9929 RVA: 0x000AD7E3 File Offset: 0x000ABBE3
		public bool SpreadWater
		{
			get
			{
				return this.spreadWater;
			}
		}

		// Token: 0x0400518A RID: 20874
		private readonly Group group;

		// Token: 0x0400518B RID: 20875
		private readonly List<IntVector2> highlightPositions;

		// Token: 0x0400518C RID: 20876
		private readonly List<IntVector2> fields;

		// Token: 0x0400518D RID: 20877
		private readonly bool spreadWater;

		// Token: 0x0400518E RID: 20878
		private readonly IntVector2 position;
	}
}
