using System.Collections.Generic;
using Match3.Scripts1.Shared.M3Engine;

// Token: 0x020005B1 RID: 1457
namespace Match3.Scripts1
{
	public struct ShuffleResult : IMatchResult
	{
		// Token: 0x06002618 RID: 9752 RVA: 0x000AA1FE File Offset: 0x000A85FE
		public ShuffleResult(List<IMatchResult> results)
		{
			this.results = results;
		}

		// Token: 0x04005109 RID: 20745
		public List<IMatchResult> results;
	}
}
