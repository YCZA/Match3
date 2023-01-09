using System.Collections.Generic;
using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020005BA RID: 1466
	public abstract class ACombineSupergemMatcher : ASwapMatcher
	{
		// Token: 0x06002637 RID: 9783 RVA: 0x000AAEC8 File Offset: 0x000A92C8
		public ACombineSupergemMatcher(Fields fields) : base(fields)
		{
		}

		// Token: 0x06002638 RID: 9784 RVA: 0x000AAEDC File Offset: 0x000A92DC
		protected override bool HasMatchInternal()
		{
			return this.HasSupergemCombination(this.from, this.to);
		}

		// Token: 0x06002639 RID: 9785 RVA: 0x000AAEF0 File Offset: 0x000A92F0
		protected override List<IMatchResult> FindMatches()
		{
			this.matchResults.Clear();
			this.CreateCombinedExplosion(this.from, this.to, this.matchResults);
			return this.matchResults;
		}

		// Token: 0x0600263A RID: 9786
		protected abstract bool HasSupergemCombination(Gem from, Gem to);

		// Token: 0x0600263B RID: 9787
		protected abstract void CreateCombinedExplosion(Gem from, Gem to, List<IMatchResult> matchResults);

		// Token: 0x04005120 RID: 20768
		private List<IMatchResult> matchResults = new List<IMatchResult>();
	}
}
