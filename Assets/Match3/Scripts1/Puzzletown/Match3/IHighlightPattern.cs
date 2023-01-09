using System.Collections.Generic;
using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020005DD RID: 1501
	public interface IHighlightPattern : IMatchResult
	{
		// Token: 0x17000616 RID: 1558
		// (get) Token: 0x060026E3 RID: 9955
		List<IntVector2> HighlightPositions { get; }
	}
}
