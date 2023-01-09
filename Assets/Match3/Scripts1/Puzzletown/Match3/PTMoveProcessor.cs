using System.Collections.Generic;
using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x0200062E RID: 1582
	public class PTMoveProcessor : IMoveProcessor
	{
		// Token: 0x0600283F RID: 10303 RVA: 0x000B3A90 File Offset: 0x000B1E90
		public List<IMatchResult> Process(Move move, Fields fields)
		{
			this.results.Clear();
			if (fields.IsSwapPossible(move.from, move.to))
			{
				fields.SwapGems(move.from, move.to);
				this.results.Add(move);
			}
			else
			{
				this.results.Add(new BlockedSwap(move.from, move.to));
			}
			return this.results;
		}

		// Token: 0x04005245 RID: 21061
		private List<IMatchResult> results = new List<IMatchResult>();
	}
}
