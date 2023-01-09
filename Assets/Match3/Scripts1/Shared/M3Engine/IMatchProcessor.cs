using System.Collections.Generic;
using Match3.Scripts1.Puzzletown.Match3;

namespace Match3.Scripts1.Shared.M3Engine
{
	// Token: 0x02000AFC RID: 2812
	public interface IMatchProcessor
	{
		// Token: 0x06004287 RID: 17031
		IEnumerable<IMatchResult> Process(Fields fields, List<IMatchResult> results);
	}
}
