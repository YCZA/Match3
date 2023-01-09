using System.Collections.Generic;

namespace Match3.Scripts1.Shared.M3Engine
{
	// Token: 0x02000AF6 RID: 2806
	public interface IBoost
	{
		// Token: 0x06004279 RID: 17017
		List<IMatchResult> Apply();

		// Token: 0x0600427A RID: 17018
		bool IsValid();
	}
}
