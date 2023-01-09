using System.Collections.Generic;
using Match3.Scripts1.Puzzletown.Match3;

namespace Match3.Scripts1.Shared.M3Engine
{
	// Token: 0x02000AFE RID: 2814
	public interface IMoveProcessor
	{
		// Token: 0x06004288 RID: 17032
		List<IMatchResult> Process(Move move, Fields fields);
	}
}
