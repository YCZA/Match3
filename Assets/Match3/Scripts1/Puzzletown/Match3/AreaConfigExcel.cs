using System;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020004E8 RID: 1256
	[Serializable]
	public class AreaConfigExcel
	{
		// Token: 0x1700054C RID: 1356
		// (get) Token: 0x060022CB RID: 8907 RVA: 0x0009A351 File Offset: 0x00098751
		public string LevelName
		{
			get
			{
				return this.file_name + this.tier;
			}
		}

		// Token: 0x04004E72 RID: 20082
		public int level_number;

		// Token: 0x04004E73 RID: 20083
		public int diamonds_a;

		// Token: 0x04004E74 RID: 20084
		public int diamonds_b;

		// Token: 0x04004E75 RID: 20085
		public int diamonds_c;

		// Token: 0x04004E76 RID: 20086
		public string file_name;

		// Token: 0x04004E77 RID: 20087
		public string tier;

		// Token: 0x04004E78 RID: 20088
		public string objective;

		// Token: 0x04004E79 RID: 20089
		public string unlocked_at_quest_completed;

		// Token: 0x04004E7A RID: 20090
		public string wait_for_dialog;
	}
}
