using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.QStringUtil;

// Token: 0x0200093C RID: 2364
namespace Match3.Scripts1
{
	public class CollectAndRepairTaskHandler : ACompleteTierTaskHandler
	{
		// Token: 0x06003983 RID: 14723 RVA: 0x0011B336 File Offset: 0x00119736
		public CollectAndRepairTaskHandler(QuestManager questService, ProgressionDataService.Service progression) : base(questService, progression)
		{
		}

		// Token: 0x06003984 RID: 14724 RVA: 0x0011B340 File Offset: 0x00119740
		public override string GetTaskType()
		{
			return CollectAndRepairTaskHandler.taskTypeName;
		}

		// Token: 0x06003985 RID: 14725 RVA: 0x0011B347 File Offset: 0x00119747
		protected override bool ValidTask(string taskType)
		{
			return taskType.QCompare(CollectAndRepairTaskHandler.taskTypeName);
		}

		// Token: 0x06003986 RID: 14726 RVA: 0x0011B354 File Offset: 0x00119754
		public override void OnCollected(QuestProgress progress, int idx)
		{
			QuestData configData = progress.configData;
			progress.tasksProgress[idx].collected = true;
			// eli key point 完成任务后执行相应的事件(如先清理地面，然后对话)
			base.questService.TriggerAction(new RepairBuildingsByTagAction.command(configData.task_action_target[idx]));
			// 完成任务后不执行任何事件(如对话)
			// base.questService.TriggerAction(new DummyAction());
		}

		// Token: 0x06003987 RID: 14727 RVA: 0x0011B38E File Offset: 0x0011978E
		protected override void HandleTask(QuestProgress progress, int idx, string tierString)
		{
		}

		// Token: 0x040061BC RID: 25020
		private static string taskTypeName = "collect_and_repair";
	}
}
