using System;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x020007AA RID: 1962
	[Serializable]
	public class Transaction
	{
		// Token: 0x06003016 RID: 12310 RVA: 0x000E1C83 File Offset: 0x000E0083
		public Transaction(string itemId, int purchaseDate, int priceUsd)
		{
			this.itemId = itemId;
			this.purchaseDate = purchaseDate;
			this.priceUsd = priceUsd;
		}

		// Token: 0x04005924 RID: 22820
		public string itemId;

		// Token: 0x04005925 RID: 22821
		public int purchaseDate;

		// Token: 0x04005926 RID: 22822
		public int priceUsd;
	}
}
