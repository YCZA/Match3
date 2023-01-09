using System;
using System.Collections.Generic;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x020007EB RID: 2027
	[Serializable]
	public class PendingRewards
	{
		// Token: 0x06003243 RID: 12867 RVA: 0x000ECA2C File Offset: 0x000EAE2C
		public PendingRewards(Materials materials, PendingRewards.TrackingCall trackingCall)
		{
			this.materials = materials;
			this.trackingCall = trackingCall;
		}

		// Token: 0x06003244 RID: 12868 RVA: 0x000ECA42 File Offset: 0x000EAE42
		public List<TownReward> AsTownRewards()
		{
			return TownReward.FromMaterials(this.materials);
		}

		// Token: 0x04005AB6 RID: 23222
		public Materials materials;

		// Token: 0x04005AB7 RID: 23223
		public PendingRewards.TrackingCall trackingCall;

		// Token: 0x020007EC RID: 2028
		[Serializable]
		public class TrackingCall
		{
			// Token: 0x04005AB8 RID: 23224
			public string evtName;

			// Token: 0x04005AB9 RID: 23225
			public string parametersJson;
		}
	}
}
