using System;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020004ED RID: 1261
	[Serializable]
	public class DiveForTreasureLevelConfigExcel
	{
		// Token: 0x1700054D RID: 1357
		// (get) Token: 0x060022D2 RID: 8914 RVA: 0x0009A4B8 File Offset: 0x000988B8
		public string tier
		{
			get
			{
				return (this.level <= 3) ? "a" : "c";
			}
		}

		// Token: 0x04004E89 RID: 20105
		public int set;

		// Token: 0x04004E8A RID: 20106
		public int level;

		// Token: 0x04004E8B RID: 20107
		public string file_name;

		// Token: 0x04004E8C RID: 20108
		public string objective;
	}
}
