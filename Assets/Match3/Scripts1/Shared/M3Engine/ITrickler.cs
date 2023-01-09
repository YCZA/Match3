using System.Collections.Generic;
using Match3.Scripts1.Puzzletown.Match3;

namespace Match3.Scripts1.Shared.M3Engine
{
	// Token: 0x02000AFF RID: 2815
	public interface ITrickler
	{
		// Token: 0x06004289 RID: 17033
		List<IMatchResult> Trickle(Fields fields);
	}
}
