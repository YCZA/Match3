namespace Match3.Scripts1.Puzzletown.Flows
{
	// Token: 0x020004C6 RID: 1222
	public class QuestTaskCollectPauseFlow : PauseFlow
	{
		// Token: 0x0600223B RID: 8763 RVA: 0x000959CF File Offset: 0x00093DCF
		public QuestTaskCollectPauseFlow(QuestTaskData data) : base((data.type != QuestTaskType.collect_and_repair) ? 0.2f : 2f)
		{
		}

		// Token: 0x04004DA3 RID: 19875
		private const float COLLECT_TASK_PAUSE = 2f;

		// Token: 0x04004DA4 RID: 19876
		private const float DEFAULT_PAUSE = 0.2f;
	}
}
