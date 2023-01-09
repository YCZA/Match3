using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.QStringUtil;
using UnityEngine;

// Token: 0x02000941 RID: 2369
namespace Match3.Scripts1
{
	public class CompleteTierCollectTaskHandler : ACompleteTierTaskHandler
	{
		// Token: 0x0600399A RID: 14746 RVA: 0x0011B608 File Offset: 0x00119A08
		public CompleteTierCollectTaskHandler(QuestManager questService, ProgressionDataService.Service progression) : base(questService, progression)
		{
		}

		// Token: 0x0600399B RID: 14747 RVA: 0x0011B612 File Offset: 0x00119A12
		public override string GetTaskType()
		{
			return CompleteTierCollectTaskHandler.taskTypeName;
		}

		// Token: 0x0600399C RID: 14748 RVA: 0x0011B619 File Offset: 0x00119A19
		protected override bool ValidTask(string taskType)
		{
			return taskType.QCompare(CompleteTierCollectTaskHandler.taskTypeName);
		}

		// Token: 0x0600399D RID: 14749 RVA: 0x0011B628 File Offset: 0x00119A28
		protected override void HandleTask(QuestProgress progress, int idx, string tierString)
		{
			QuestData configData = progress.configData;
			if (tierString == configData.task_item[idx])
			{
				progress.tasksProgress[idx].progress = Mathf.Min(configData.task_count[idx], progress.tasksProgress[idx].progress + 1);
			}
			if (this.TaskComplete(progress, idx))
			{
				progress.tasksProgress[idx].collected = true;
			}
		}

		// Token: 0x040061C0 RID: 25024
		private static string taskTypeName = "complete_tier_collect";
	}
}
