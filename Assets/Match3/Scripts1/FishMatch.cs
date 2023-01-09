using System.Collections.Generic;
using Match3.Scripts1.Puzzletown.Match3;
using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1
{
	// Token: 0x020005D6 RID: 1494
	public struct FishMatch : IMatchGroup, IHighlightPattern, IMatchResult
	{
		// Token: 0x060026BF RID: 9919 RVA: 0x000AD658 File Offset: 0x000ABA58
		public FishMatch(Group group)
		{
			this.group = group;
			this.highlightPositions = group.Positions;
			List<IntVector2> range = group.Positions.GetRange(0, 4);
			int num = 0;
			int num2 = 0;
			foreach (IntVector2 intVector in range)
			{
				num = ((intVector.x <= num) ? num : intVector.x);
				num2 = ((intVector.y <= num2) ? num2 : intVector.y);
			}
			this.fishOrigin = new IntVector2(num, num2);
			this.exploded = false;
		}

		// Token: 0x170005FA RID: 1530
		// (get) Token: 0x060026C0 RID: 9920 RVA: 0x000AD718 File Offset: 0x000ABB18
		public Group Group
		{
			get
			{
				return this.group;
			}
		}

		// Token: 0x170005FB RID: 1531
		// (get) Token: 0x060026C1 RID: 9921 RVA: 0x000AD720 File Offset: 0x000ABB20
		public bool ShouldHitAdjacentFields
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170005FC RID: 1532
		// (get) Token: 0x060026C2 RID: 9922 RVA: 0x000AD723 File Offset: 0x000ABB23
		public List<IntVector2> HighlightPositions
		{
			get
			{
				return this.highlightPositions;
			}
		}

		// Token: 0x04005186 RID: 20870
		private readonly Group group;

		// Token: 0x04005187 RID: 20871
		private readonly List<IntVector2> highlightPositions;

		// Token: 0x04005188 RID: 20872
		public bool exploded;

		// Token: 0x04005189 RID: 20873
		public IntVector2 fishOrigin;
	}
}
