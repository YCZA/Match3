using System.Collections.Generic;
using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020005E2 RID: 1506
	public interface IMultipleStepResult : IMatchResult
	{
		// Token: 0x1700061C RID: 1564
		// (get) Token: 0x060026E9 RID: 9961
		// (set) Token: 0x060026EA RID: 9962
		List<List<IMatchResult>> Steps { get; set; }
	}
}
