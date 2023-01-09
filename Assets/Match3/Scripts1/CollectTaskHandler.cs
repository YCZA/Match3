using System;
using System.Runtime.CompilerServices;
using Match3.Scripts1.Puzzletown.Services;
using UnityEngine;

// Token: 0x0200093E RID: 2366
namespace Match3.Scripts1
{
	public class CollectTaskHandler : ACollectTaskHandler
	{
		// Token: 0x0600398B RID: 14731 RVA: 0x0011B3A5 File Offset: 0x001197A5
		public CollectTaskHandler(QuestManager questService) : base(questService)
		{
		}

		// Token: 0x0600398C RID: 14732 RVA: 0x0011B3AE File Offset: 0x001197AE
		private static bool ValidTask(string taskType)
		{
			return taskType == CollectTaskHandler.taskTypeName;
		}

		// Token: 0x0600398D RID: 14733 RVA: 0x0011B3BB File Offset: 0x001197BB
		public override string GetTaskType()
		{
			return CollectTaskHandler.taskTypeName;
		}

		// Token: 0x0600398E RID: 14734 RVA: 0x0011B3C4 File Offset: 0x001197C4
		public override bool TaskComplete(QuestProgress progress, int idx)
		{
			QuestData configData = progress.configData;
			return CollectTaskHandler.ValidTask(configData.task_type[idx]) && configData.task_count[idx] <= progress.tasksProgress[idx].progress;
		}

		// Token: 0x0600398F RID: 14735 RVA: 0x0011B408 File Offset: 0x00119808
		public override void CollectResource(MaterialAmount collected)
		{
			QuestManager questService = base.questService;
			if (CollectTaskHandler._003C_003Ef__mg_0024cache0 == null)
			{
				CollectTaskHandler._003C_003Ef__mg_0024cache0 = new Func<string, bool>(CollectTaskHandler.ValidTask);
			}
			questService.ProcessFilteredTasks(CollectTaskHandler._003C_003Ef__mg_0024cache0, new Func<QuestProgress, int, bool>(this.TaskComplete), delegate(QuestProgress progress, int idx)
			{
				QuestData configData = progress.configData;
				if (collected.type == configData.task_item[idx])
				{
					progress.tasksProgress[idx].progress = Mathf.Min(configData.task_count[idx], progress.tasksProgress[idx].progress + collected.amount);
				}
				if (this.TaskComplete(progress, idx))
				{
					progress.tasksProgress[idx].collected = true;
				}
			});
		}

		// Token: 0x06003990 RID: 14736 RVA: 0x0011B46C File Offset: 0x0011986C
		public void CollectResources(Materials collected)
		{
			foreach (MaterialAmount collected2 in collected)
			{
				this.CollectResource(collected2);
			}
		}

		// Token: 0x040061BD RID: 25021
		private static string taskTypeName = "collect";

		// Token: 0x040061BE RID: 25022
		[CompilerGenerated]
		private static Func<string, bool> _003C_003Ef__mg_0024cache0;
	}
}
