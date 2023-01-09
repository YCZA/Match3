using System;

namespace Match3.Scripts1.Puzzletown.Config
{
	// Token: 0x02000491 RID: 1169
	[Serializable]
	public class WheelPrize
	{
		// Token: 0x17000522 RID: 1314
		// (get) Token: 0x06002140 RID: 8512 RVA: 0x0008BC69 File Offset: 0x0008A069
		public AdSpinPrize prizeType
		{
			get
			{
				return (AdSpinPrize)Enum.Parse(typeof(AdSpinPrize), this.prize, true);
			}
		}

		// Token: 0x04004C49 RID: 19529
		public int amount;

		// Token: 0x04004C4A RID: 19530
		public int position;

		// Token: 0x04004C4B RID: 19531
		public string prize;

		// Token: 0x04004C4C RID: 19532
		public int probability;
	}
}
