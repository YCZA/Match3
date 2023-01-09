using System.Collections.Generic;
using Match3.Scripts1.Puzzletown.Match3;
using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1
{
	// Token: 0x020005CE RID: 1486
	public struct ClimberMoves : IMultipleStepResult, IMatchResult
	{
		// Token: 0x170005EC RID: 1516
		// (get) Token: 0x060026A2 RID: 9890 RVA: 0x000AD3C0 File Offset: 0x000AB7C0
		// (set) Token: 0x060026A3 RID: 9891 RVA: 0x000AD3C8 File Offset: 0x000AB7C8
		public List<List<IMatchResult>> Steps { get; set; }
	}
}
