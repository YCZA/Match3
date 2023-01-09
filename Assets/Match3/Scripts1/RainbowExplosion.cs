using System.Collections.Generic;
using Match3.Scripts1.Puzzletown.Match3;
using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1
{
	// Token: 0x020005F2 RID: 1522
	public struct RainbowExplosion : IRainbowExplosion, IMatchGroup, IMatchWithAffectedFields, IHighlightPattern, ISwapResult, IMatchResult, IExplosionResult
	{
		// Token: 0x06002719 RID: 10009 RVA: 0x000AE054 File Offset: 0x000AC454
		public RainbowExplosion(Fields fields, GemColor targetColor, IntVector2 center, IntVector2 gemPos)
		{
			this.group = new Group();
			this.fields = new List<IntVector2>();
			this.center = center;
			this.from = gemPos;
			this.group = fields.GetAllMatchableGems(targetColor);
			this.group.Color = targetColor;
			if (!this.group.Includes(center))
			{
				this.group.Add(fields[center].gem);
			}
			if (!this.group.Includes(gemPos))
			{
				this.group.Add(fields[gemPos].gem);
			}
			this.highlightPositions = this.group.Positions;
			this.highlightPositions.AddRange(this.fields);
		}

		// Token: 0x17000640 RID: 1600
		// (get) Token: 0x0600271A RID: 10010 RVA: 0x000AE113 File Offset: 0x000AC513
		public IntVector2 Center
		{
			get
			{
				return this.center;
			}
		}

		// Token: 0x17000641 RID: 1601
		// (get) Token: 0x0600271B RID: 10011 RVA: 0x000AE11B File Offset: 0x000AC51B
		public IntVector2 From
		{
			get
			{
				return this.from;
			}
		}

		// Token: 0x17000642 RID: 1602
		// (get) Token: 0x0600271C RID: 10012 RVA: 0x000AE123 File Offset: 0x000AC523
		public Group Group
		{
			get
			{
				return this.group;
			}
		}

		// Token: 0x17000643 RID: 1603
		// (get) Token: 0x0600271D RID: 10013 RVA: 0x000AE12B File Offset: 0x000AC52B
		public List<IntVector2> Fields
		{
			get
			{
				return this.fields;
			}
		}

		// Token: 0x17000644 RID: 1604
		// (get) Token: 0x0600271E RID: 10014 RVA: 0x000AE133 File Offset: 0x000AC533
		public List<IntVector2> HighlightPositions
		{
			get
			{
				return this.highlightPositions;
			}
		}

		// Token: 0x17000645 RID: 1605
		// (get) Token: 0x0600271F RID: 10015 RVA: 0x000AE13B File Offset: 0x000AC53B
		public bool ShouldHitAdjacentFields
		{
			get
			{
				return true;
			}
		}

		// Token: 0x040051BA RID: 20922
		private readonly Group group;

		// Token: 0x040051BB RID: 20923
		private readonly IntVector2 from;

		// Token: 0x040051BC RID: 20924
		private readonly IntVector2 center;

		// Token: 0x040051BD RID: 20925
		private readonly List<IntVector2> fields;

		// Token: 0x040051BE RID: 20926
		private readonly List<IntVector2> highlightPositions;
	}
}
