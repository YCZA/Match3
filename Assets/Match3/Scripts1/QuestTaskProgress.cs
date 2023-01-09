using System;

// Token: 0x02000931 RID: 2353
namespace Match3.Scripts1
{
	[Serializable]
	public class QuestTaskProgress
	{
		// Token: 0x0600393D RID: 14653 RVA: 0x00119F57 File Offset: 0x00118357
		public void SetCompleted(QuestData quest, int idx)
		{
			this.progress = quest.task_count[idx];
			this.collected = true;
		}

		// Token: 0x0400618F RID: 24975
		public int progress;

		// Token: 0x04006190 RID: 24976
		public bool collected;
	}
}
