using System.Collections.Generic;
using Match3.Scripts1.Puzzletown.Match3;
using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1
{
	// Token: 0x02000633 RID: 1587
	public struct TrickleResult : IMultipleStepResult, IMatchResult
	{
		// Token: 0x17000697 RID: 1687
		// (get) Token: 0x06002853 RID: 10323 RVA: 0x000B460F File Offset: 0x000B2A0F
		// (set) Token: 0x06002854 RID: 10324 RVA: 0x000B4617 File Offset: 0x000B2A17
		public List<List<IMatchResult>> Steps { get; set; }
	}
}
