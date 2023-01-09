using System;
using System.Collections.Generic;

namespace Match3.Scripts1.Puzzletown.Config
{
	// Token: 0x02000492 RID: 1170
	[Serializable]
	public class JackpotPrize
	{
		// Token: 0x17000523 RID: 1315
		// (get) Token: 0x06002142 RID: 8514 RVA: 0x0008BC90 File Offset: 0x0008A090
		public List<WheelPrize> prizes
		{
			get
			{
				if (this.convertedPrizes == null)
				{
					this.convertedPrizes = new List<WheelPrize>();
					WheelPrize wheelPrize = new WheelPrize();
					wheelPrize.prize = this.prize1;
					wheelPrize.amount = this.amount1;
					this.convertedPrizes.Add(wheelPrize);
					if (!string.IsNullOrEmpty(this.prize2))
					{
						WheelPrize wheelPrize2 = new WheelPrize();
						wheelPrize2.prize = this.prize2;
						wheelPrize2.amount = this.amount2;
						this.convertedPrizes.Add(wheelPrize2);
					}
				}
				return this.convertedPrizes;
			}
		}

		// Token: 0x04004C4D RID: 19533
		public string prize1;

		// Token: 0x04004C4E RID: 19534
		public int amount1;

		// Token: 0x04004C4F RID: 19535
		public string prize2;

		// Token: 0x04004C50 RID: 19536
		public int amount2;

		// Token: 0x04004C51 RID: 19537
		public int probability;

		// Token: 0x04004C52 RID: 19538
		private List<WheelPrize> convertedPrizes;
	}
}
