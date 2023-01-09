using System.Collections.Generic;
using Match3.Scripts1.Puzzletown.Match3;

namespace Match3.Scripts1.Shared.M3Engine
{
	// Token: 0x02000AFA RID: 2810
	public interface IMatcher
	{
		// Token: 0x06004284 RID: 17028
		IEnumerable<IMatchResult> FindMatches(Fields fields, Move move, List<Group> groups);

		// Token: 0x06004285 RID: 17029
		IEnumerable<IMatchResult> RemoveMatches(Fields fields, Move move, List<Group> groups);
	}
}
