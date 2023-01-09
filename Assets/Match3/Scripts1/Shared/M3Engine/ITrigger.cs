using System.Collections.Generic;
using Match3.Scripts1.Puzzletown.Match3;

namespace Match3.Scripts1.Shared.M3Engine
{
	// Token: 0x02000B00 RID: 2816
	public interface ITrigger
	{
		// Token: 0x0600428A RID: 17034
		void ExecuteTrigger(Fields fields, List<List<IMatchResult>> allResults);
	}
}
