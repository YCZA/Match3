using System.Collections.Generic;
using Match3.Scripts1.Shared.M3Engine;
using Match3.Scripts1.Wooga.Signals;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020005C7 RID: 1479
	public class RainbowExplosionMatcher : ASwapMatcher
	{
		// Token: 0x06002681 RID: 9857 RVA: 0x000ACBCC File Offset: 0x000AAFCC
		public RainbowExplosionMatcher(Fields fields) : base(fields)
		{
		}

		// Token: 0x06002682 RID: 9858 RVA: 0x000ACBEC File Offset: 0x000AAFEC
		protected override bool HasMatchInternal()
		{
			this.isFromRainbow = (this.from.color == GemColor.Rainbow);
			this.isFromMatchable = this.from.IsMatchable;
			this.isToRainbow = (this.to.color == GemColor.Rainbow);
			this.isToMatchable = this.to.IsMatchable;
			return (this.isFromRainbow && this.isToRainbow) || (this.isFromRainbow && this.isToMatchable) || (this.isToRainbow && this.isFromMatchable);
		}

		// Token: 0x06002683 RID: 9859 RVA: 0x000ACC88 File Offset: 0x000AB088
		protected override List<IMatchResult> FindMatches()
		{
			this.matchResults.Clear();
			Move move = new Move(this.from.position, this.to.position, false, false, true);
			if ((this.isFromRainbow && this.isToMatchable) || (this.isToRainbow && this.isFromMatchable))
			{
				GemColor targetColor = (!this.isFromMatchable) ? this.to.color : this.from.color;
				IntVector2 center = (!this.isFromRainbow) ? move.to : move.from;
				IntVector2 gemPos = (!this.isFromMatchable) ? move.to : move.from;
				this.onRainbowExplosionCreated.Dispatch();
				this.matchResults.Add(new RainbowExplosion(this.fields, targetColor, center, gemPos));
			}
			else if (this.isFromRainbow && this.isToRainbow)
			{
				this.matchResults.Add(new SuperRainbowExplosion(this.fields, move));
			}
			return this.matchResults;
		}

		// Token: 0x04005158 RID: 20824
		private List<IMatchResult> matchResults = new List<IMatchResult>();

		// Token: 0x04005159 RID: 20825
		public readonly Signal onRainbowExplosionCreated = new Signal();

		// Token: 0x0400515A RID: 20826
		private bool isFromRainbow;

		// Token: 0x0400515B RID: 20827
		private bool isFromMatchable;

		// Token: 0x0400515C RID: 20828
		private bool isToRainbow;

		// Token: 0x0400515D RID: 20829
		private bool isToMatchable;
	}
}
