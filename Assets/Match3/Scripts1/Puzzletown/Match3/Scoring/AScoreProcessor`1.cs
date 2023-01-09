using Match3.Scripts1.Shared.M3Engine;
using Match3.Scripts1.Shared.Processors;

namespace Match3.Scripts1.Puzzletown.Match3.Scoring
{
	// Token: 0x02000637 RID: 1591
	public abstract class AScoreProcessor<T> : AProcessor<IMatchResult, T>, IScoreProcessor, IProcessor<IMatchResult> where T : IMatchResult
	{
		// Token: 0x0600286A RID: 10346 RVA: 0x000B4DB7 File Offset: 0x000B31B7
		public AScoreProcessor(Match3Score score)
		{
			this.score = score;
		}

		// Token: 0x04005267 RID: 21095
		protected Match3Score score;
	}
}
