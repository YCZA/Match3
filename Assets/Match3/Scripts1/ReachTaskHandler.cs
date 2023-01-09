using System;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000947 RID: 2375
namespace Match3.Scripts1
{
	public class ReachTaskHandler : QuestTaskHandler
	{
		// Token: 0x060039B4 RID: 14772 RVA: 0x0011BC28 File Offset: 0x0011A028
		public ReachTaskHandler(QuestManager questService) : base(questService)
		{
		}

		// Token: 0x060039B5 RID: 14773 RVA: 0x0011BC31 File Offset: 0x0011A031
		private static bool ValidTask(string taskType)
		{
			return taskType == ReachTaskHandler.taskTypeName;
		}

		// Token: 0x060039B6 RID: 14774 RVA: 0x0011BC3E File Offset: 0x0011A03E
		public override string GetTaskType()
		{
			return ReachTaskHandler.taskTypeName;
		}

		// Token: 0x060039B7 RID: 14775 RVA: 0x0011BC48 File Offset: 0x0011A048
		public override bool TaskComplete(QuestProgress progress, int idx)
		{
			QuestData configData = progress.configData;
			return ReachTaskHandler.ValidTask(configData.task_type[idx]) && configData.task_count[idx] <= progress.tasksProgress[idx].progress;
		}

		// Token: 0x060039B8 RID: 14776 RVA: 0x0011BC8C File Offset: 0x0011A08C
		public virtual void HandleResourceChanged(MaterialChange newAmount)
		{
			QuestManager questService = base.questService;
			if (ReachTaskHandler._003C_003Ef__mg_0024cache0 == null)
			{
				ReachTaskHandler._003C_003Ef__mg_0024cache0 = new Func<string, bool>(ReachTaskHandler.ValidTask);
			}
			questService.ProcessFilteredTasks(ReachTaskHandler._003C_003Ef__mg_0024cache0, new Func<QuestProgress, int, bool>(this.TaskComplete), delegate(QuestProgress progress, int idx)
			{
				QuestData configData = progress.configData;
				if (newAmount.name == configData.task_item[idx])
				{
					progress.tasksProgress[idx].progress = Mathf.Min(configData.task_count[idx], newAmount.after);
				}
				if (this.TaskComplete(progress, idx))
				{
					progress.tasksProgress[idx].collected = true;
				}
			});
		}

		// Token: 0x040061C9 RID: 25033
		private static string taskTypeName = "reach";

		// Token: 0x040061CA RID: 25034
		[CompilerGenerated]
		private static Func<string, bool> _003C_003Ef__mg_0024cache0;
	}
}
