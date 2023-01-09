using System;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x02000828 RID: 2088
	public class VideoAdMetaData
	{
		// Token: 0x060033E1 RID: 13281 RVA: 0x000F8238 File Offset: 0x000F6638
		public VideoAdMetaData()
		{
			this.id = Guid.NewGuid().ToString("N");
		}

		// Token: 0x04005BE5 RID: 23525
		public readonly string id;

		// Token: 0x04005BE6 RID: 23526
		public string network;

		// Token: 0x04005BE7 RID: 23527
		public string ecpmPricing;

		// Token: 0x04005BE8 RID: 23528
		public string ecpmPricingType;
	}
}
