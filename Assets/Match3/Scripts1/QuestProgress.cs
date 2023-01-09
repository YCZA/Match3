using System;

// Token: 0x02000932 RID: 2354
namespace Match3.Scripts1
{
	[Serializable]
	public class QuestProgress
	{
		// Token: 0x0600393F RID: 14655 RVA: 0x00119F78 File Offset: 0x00118378
		public static QuestProgress CreateCompleted(QuestData quest, QuestProgress.Status desiredStatus)
		{
			QuestProgress questProgress = QuestProgress.Create(quest);
			questProgress.status = desiredStatus;
			for (int i = 0; i < quest.task_count.Length; i++)
			{
				questProgress.tasksProgress[i].SetCompleted(quest, i);
			}
			return questProgress;
		}

		// Token: 0x06003940 RID: 14656 RVA: 0x00119FBC File Offset: 0x001183BC
		public static QuestProgress CreateWithStatus(QuestData quest, QuestProgress.Status desiredStatus)
		{
			QuestProgress questProgress = QuestProgress.Create(quest);
			questProgress.status = desiredStatus;
			for (int i = 0; i < quest.task_count.Length; i++)
			{
				questProgress.tasksProgress[i].collected = false;
				questProgress.tasksProgress[i].progress = quest.task_count[i];
			}
			return questProgress;
		}

		// Token: 0x06003941 RID: 14657 RVA: 0x0011A014 File Offset: 0x00118414
		public static QuestProgress Create(QuestData quest)
		{
			QuestProgress questProgress = new QuestProgress
			{
				questID = quest.id,
				status = QuestProgress.Status.locked,
				tasksProgress = new QuestTaskProgress[quest.task_count.Length]
			};
			for (int i = 0; i < quest.task_count.Length; i++)
			{
				questProgress.tasksProgress[i] = new QuestTaskProgress();
			}
			questProgress.configData = quest;
			return questProgress;
		}

		// Token: 0x04006191 RID: 24977
		public QuestData configData;

		// Token: 0x04006192 RID: 24978
		public string questID;

		// Token: 0x04006193 RID: 24979
		public QuestProgress.Status status;

		// Token: 0x04006194 RID: 24980
		public QuestTaskProgress[] tasksProgress;

		// Token: 0x02000933 RID: 2355
		[Flags]
		public enum Status
		{
			// Token: 0x04006196 RID: 24982
			undefined = 0,
			// Token: 0x04006197 RID: 24983
			locked = 1,
			// Token: 0x04006198 RID: 24984
			unstarted = 2,
			// Token: 0x04006199 RID: 24985
			started = 4,
			// Token: 0x0400619A RID: 24986
			completed = 8,
			// Token: 0x0400619B RID: 24987
			collected = 16,
			// Token: 0x0400619C RID: 24988
			active = 4,
			// Token: 0x0400619D RID: 24989
			done = 24,
			// Token: 0x0400619E RID: 24990
			unlocked = 28,
			// Token: 0x0400619F RID: 24991
			chapterIntro = 29
		}
	}
}
