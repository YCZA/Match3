

// Token: 0x02000936 RID: 2358
namespace Match3.Scripts1
{
	public class QuestDataProgress
	{
		// Token: 0x0600394C RID: 14668 RVA: 0x0011A32D File Offset: 0x0011872D
		public QuestDataProgress(QuestTaskData taskData, QuestProgress progress, bool isComplete)
		{
			this.taskData = taskData;
			this.progress = progress;
			this.isComplete = isComplete;
		}

		// Token: 0x170008D1 RID: 2257
		// (get) Token: 0x0600394D RID: 14669 RVA: 0x0011A34A File Offset: 0x0011874A
		public int taskIdx
		{
			get
			{
				return this.taskData.index;
			}
		}

		// Token: 0x170008D2 RID: 2258
		// (get) Token: 0x0600394E RID: 14670 RVA: 0x0011A357 File Offset: 0x00118757
		public bool IsCollected
		{
			get
			{
				return this.progress.tasksProgress[this.taskIdx].collected;
			}
		}

		// Token: 0x040061A4 RID: 24996
		public readonly bool isComplete;

		// Token: 0x040061A5 RID: 24997
		public QuestTaskData taskData;

		// Token: 0x040061A6 RID: 24998
		public QuestProgress progress;
	}
}
