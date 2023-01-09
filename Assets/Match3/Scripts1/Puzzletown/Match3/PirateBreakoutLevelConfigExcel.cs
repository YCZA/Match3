using System;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020004F6 RID: 1270
	[Serializable]
	public class PirateBreakoutLevelConfigExcel
	{
		// Token: 0x17000569 RID: 1385
		// (get) Token: 0x06002301 RID: 8961 RVA: 0x0009B085 File Offset: 0x00099485
		public string tier
		{
			get
			{
				return (this.level <= 3) ? "a" : "c";
			}
		}

		// Token: 0x04004EC5 RID: 20165
		public int set;

		// Token: 0x04004EC6 RID: 20166
		public int level;

		// Token: 0x04004EC7 RID: 20167
		public string file_name;

		// Token: 0x04004EC8 RID: 20168
		public string objective;
	}
}
