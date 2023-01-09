using System.Collections.Generic;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x02000740 RID: 1856
	public interface IPreloadTrigger
	{
		// Token: 0x1700071F RID: 1823
		// (get) Token: 0x06002DF9 RID: 11769
		IEnumerable<string> BundleNames { get; }

		// Token: 0x06002DFA RID: 11770
		bool ShouldPreload();
	}
}
