using System.Collections.Generic;
using Match3.Scripts1.Puzzletown.Match3;
using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1
{
	// Token: 0x020005F3 RID: 1523
	public struct RainbowSuperGemExplosion : IRainbowExplosion, IMatchGroup, IMatchWithAffectedFields, IHighlightPattern, ISwapResult, IMatchResult, IExplosionResult
	{
		// Token: 0x06002720 RID: 10016 RVA: 0x000AE140 File Offset: 0x000AC540
		public RainbowSuperGemExplosion(Gem rainbow, Group group, Gem gem)
		{
			this.group = new Group(rainbow)
			{
				Color = gem.color
			};
			this.fields = group.Positions;
			this.center = rainbow.position;
			this.superGem = gem;
			this.highlightPositions = group.Positions;
			this.highlightPositions.Add(rainbow.position);
			this.showSupergemPositions = new List<IntVector2>();
			this.from = gem.position;
		}

		// Token: 0x17000646 RID: 1606
		// (get) Token: 0x06002721 RID: 10017 RVA: 0x000AE1BE File Offset: 0x000AC5BE
		public Group Group
		{
			get
			{
				return this.group;
			}
		}

		// Token: 0x17000647 RID: 1607
		// (get) Token: 0x06002722 RID: 10018 RVA: 0x000AE1C6 File Offset: 0x000AC5C6
		public List<IntVector2> Fields
		{
			get
			{
				return this.fields;
			}
		}

		// Token: 0x17000648 RID: 1608
		// (get) Token: 0x06002723 RID: 10019 RVA: 0x000AE1CE File Offset: 0x000AC5CE
		public IntVector2 Center
		{
			get
			{
				return this.center;
			}
		}

		// Token: 0x17000649 RID: 1609
		// (get) Token: 0x06002724 RID: 10020 RVA: 0x000AE1D6 File Offset: 0x000AC5D6
		public IntVector2 From
		{
			get
			{
				return this.from;
			}
		}

		// Token: 0x1700064A RID: 1610
		// (get) Token: 0x06002725 RID: 10021 RVA: 0x000AE1DE File Offset: 0x000AC5DE
		public List<IntVector2> HighlightPositions
		{
			get
			{
				return this.highlightPositions;
			}
		}

		// Token: 0x1700064B RID: 1611
		// (get) Token: 0x06002726 RID: 10022 RVA: 0x000AE1E6 File Offset: 0x000AC5E6
		public List<IntVector2> ShowSupergemPositions
		{
			get
			{
				return this.showSupergemPositions;
			}
		}

		// Token: 0x1700064C RID: 1612
		// (get) Token: 0x06002727 RID: 10023 RVA: 0x000AE1EE File Offset: 0x000AC5EE
		public bool ShouldHitAdjacentFields
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002728 RID: 10024 RVA: 0x000AE1F1 File Offset: 0x000AC5F1
		public void AddShowSupergemPosition(IntVector2 pos)
		{
			this.showSupergemPositions.Add(pos);
		}

		// Token: 0x040051BF RID: 20927
		public Gem superGem;

		// Token: 0x040051C0 RID: 20928
		private readonly Group group;

		// Token: 0x040051C1 RID: 20929
		private readonly IntVector2 from;

		// Token: 0x040051C2 RID: 20930
		private readonly IntVector2 center;

		// Token: 0x040051C3 RID: 20931
		private readonly List<IntVector2> fields;

		// Token: 0x040051C4 RID: 20932
		private readonly List<IntVector2> highlightPositions;

		// Token: 0x040051C5 RID: 20933
		private readonly List<IntVector2> showSupergemPositions;
	}
}
