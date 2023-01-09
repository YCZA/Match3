using System.Collections.Generic;
using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020005E0 RID: 1504
	public interface IMatchWithAffectedFields : IMatchResult
	{
		// Token: 0x1700061A RID: 1562
		// (get) Token: 0x060026E7 RID: 9959
		List<IntVector2> Fields { get; }
	}
}
