using System.Collections.Generic;
using Match3.Scripts1.Puzzletown.Match3;
using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1
{
	// Token: 0x020005D9 RID: 1497
	public struct HammerFieldMatch : IMatchWithAffectedFields, IMatchGroup, IMatchResult
	{
		// Token: 0x060026D0 RID: 9936 RVA: 0x000AD864 File Offset: 0x000ABC64
		public HammerFieldMatch(Field field)
		{
			this.position = field.gridPosition;
			this.fields = new List<IntVector2>
			{
				this.position
			};
			this.group = new Group();
		}

		// Token: 0x17000607 RID: 1543
		// (get) Token: 0x060026D1 RID: 9937 RVA: 0x000AD8A1 File Offset: 0x000ABCA1
		public List<IntVector2> Fields
		{
			get
			{
				return this.fields;
			}
		}

		// Token: 0x17000608 RID: 1544
		// (get) Token: 0x060026D2 RID: 9938 RVA: 0x000AD8A9 File Offset: 0x000ABCA9
		public Group Group
		{
			get
			{
				return this.group;
			}
		}

		// Token: 0x17000609 RID: 1545
		// (get) Token: 0x060026D3 RID: 9939 RVA: 0x000AD8B1 File Offset: 0x000ABCB1
		public bool ShouldHitAdjacentFields
		{
			get
			{
				return true;
			}
		}

		// Token: 0x04005193 RID: 20883
		public IntVector2 position;

		// Token: 0x04005194 RID: 20884
		private List<IntVector2> fields;

		// Token: 0x04005195 RID: 20885
		private Group group;
	}
}
