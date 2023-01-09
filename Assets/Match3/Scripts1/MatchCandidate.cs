using Match3.Scripts1.Puzzletown.Match3;
using Match3.Scripts1.Shared.M3Engine;

// Token: 0x020005B5 RID: 1461
namespace Match3.Scripts1
{
	public class MatchCandidate : IMatchGroup, IMatchResult
	{
		// Token: 0x0600262A RID: 9770 RVA: 0x000AAAF6 File Offset: 0x000A8EF6
		public MatchCandidate(Group group, IntVector2 start, IntVector2 target, bool isBestCandidate)
		{
			this.Group = group;
			this.target = target;
			this.start = start;
			this.isBestCandidate = isBestCandidate;
		}

		// Token: 0x170005D4 RID: 1492
		// (get) Token: 0x0600262B RID: 9771 RVA: 0x000AAB1B File Offset: 0x000A8F1B
		// (set) Token: 0x0600262C RID: 9772 RVA: 0x000AAB23 File Offset: 0x000A8F23
		public Group Group { get; set; }

		// Token: 0x170005D5 RID: 1493
		// (get) Token: 0x0600262D RID: 9773 RVA: 0x000AAB2C File Offset: 0x000A8F2C
		public bool ShouldHitAdjacentFields
		{
			get
			{
				return true;
			}
		}

		// Token: 0x04005119 RID: 20761
		public IntVector2 start;

		// Token: 0x0400511A RID: 20762
		public IntVector2 target;

		// Token: 0x0400511B RID: 20763
		public bool isBestCandidate;
	}
}
