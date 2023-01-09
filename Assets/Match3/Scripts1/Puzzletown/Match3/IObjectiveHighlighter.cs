using System.Collections.Generic;
using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x0200069B RID: 1691
	public interface IObjectiveHighlighter
	{
		// Token: 0x06002A36 RID: 10806
		List<IMatchResult> GetHighlights(Fields fields);

		// Token: 0x06002A37 RID: 10807
		bool IsValid(string objective);
	}
}
