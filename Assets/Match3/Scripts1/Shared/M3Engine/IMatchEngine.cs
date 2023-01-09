using System.Collections.Generic;
using Match3.Scripts1.Wooga.Signals;

namespace Match3.Scripts1.Shared.M3Engine
{
	// Token: 0x02000AF9 RID: 2809
	public interface IMatchEngine
	{
		// Token: 0x17000994 RID: 2452
		// (get) Token: 0x06004280 RID: 17024
		// (set) Token: 0x06004281 RID: 17025
		bool IsResolvingMoves { get; set; }

		// Token: 0x17000995 RID: 2453
		// (get) Token: 0x06004282 RID: 17026
		Signal<List<List<IMatchResult>>> onStepCompleted { get; }

		// Token: 0x06004283 RID: 17027
		void ProcessMove(Move move);
	}
}
