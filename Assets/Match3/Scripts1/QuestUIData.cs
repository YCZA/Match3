using System;

// Token: 0x0200092F RID: 2351
namespace Match3.Scripts1
{
	public class QuestUIData
	{
		// Token: 0x06003917 RID: 14615 RVA: 0x00119316 File Offset: 0x00117716
		public QuestUIData(QuestProgress progress, QuestData data)
		{
			this.progress = progress;
			this.data = data;
		}

		// Token: 0x04006180 RID: 24960
		public QuestProgress progress;

		// Token: 0x04006181 RID: 24961
		public QuestData data;

		// Token: 0x04006182 RID: 24962
		public DateTime? unlockDate;
	}
}
