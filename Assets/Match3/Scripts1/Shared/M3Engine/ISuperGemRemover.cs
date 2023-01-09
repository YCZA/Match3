using System.Collections.Generic;
using Match3.Scripts1.Puzzletown.Match3;

namespace Match3.Scripts1.Shared.M3Engine
{
	// Token: 0x02000AFB RID: 2811
	public interface ISuperGemRemover
	{
		// Token: 0x06004286 RID: 17030
		List<IMatchResult> RemoveSuperGems(Fields fields, List<IMatchResult> results);
	}
}
