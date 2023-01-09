using System.Collections.Generic;
using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020005C5 RID: 1477
	public class LineGemsCombinedMatcher : ASwapMatcher
	{
		// Token: 0x0600267A RID: 9850 RVA: 0x000AC8B8 File Offset: 0x000AACB8
		public LineGemsCombinedMatcher(Fields fields) : base(fields)
		{
		}

		// Token: 0x0600267B RID: 9851 RVA: 0x000AC8CC File Offset: 0x000AACCC
		protected override bool HasMatchInternal()
		{
			return this.from.IsLineGem() && this.to.IsLineGem();
		}

		// Token: 0x0600267C RID: 9852 RVA: 0x000AC8EC File Offset: 0x000AACEC
		protected override List<IMatchResult> FindMatches()
		{
			this.matchResults.Clear();
			this.fields[this.to.position].gem.type = GemType.Undefined;
			this.fields[this.from.position].gem.type = GemType.Undefined;
			this.to.type = GemType.LineHorizontal;
			LineGemExplosion horizontal = new LineGemExplosion(this.fields, this.to);
			this.to.type = GemType.LineVertical;
			LineGemExplosion vertical = new LineGemExplosion(this.fields, this.to);
			StarExplosion starExplosion = new StarExplosion(horizontal, vertical, this.from.position);
			this.matchResults.Add(starExplosion);
			return this.matchResults;
		}

		// Token: 0x04005156 RID: 20822
		private List<IMatchResult> matchResults = new List<IMatchResult>();
	}
}
