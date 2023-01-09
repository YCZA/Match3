using System;
using System.Collections.Generic;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x020007EA RID: 2026
	[Serializable]
	public class ResourceData
	{
		// Token: 0x04005AB3 RID: 23219
		public Materials less = new Materials();

		// Token: 0x04005AB4 RID: 23220
		public Materials more = new Materials(BoostsService.InitialBoosts);

		// Token: 0x04005AB5 RID: 23221
		public List<PendingRewards> pendingResources = new List<PendingRewards>();
	}
}
