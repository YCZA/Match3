using System;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000942 RID: 2370
namespace Match3.Scripts1
{
	public class CompleteTierReachTaskHandler : QuestTaskHandler
	{
		// Token: 0x0600399F RID: 14751 RVA: 0x0011B69F File Offset: 0x00119A9F
		public CompleteTierReachTaskHandler(QuestManager questService) : base(questService)
		{
		}

		// Token: 0x060039A0 RID: 14752 RVA: 0x0011B6A8 File Offset: 0x00119AA8
		private static bool ValidTask(string taskType)
		{
			return taskType == CompleteTierReachTaskHandler.taskTypeName;
		}

		// Token: 0x060039A1 RID: 14753 RVA: 0x0011B6B5 File Offset: 0x00119AB5
		public override string GetTaskType()
		{
			return CompleteTierReachTaskHandler.taskTypeName;
		}

		// Token: 0x060039A2 RID: 14754 RVA: 0x0011B6BC File Offset: 0x00119ABC
		public override bool TaskComplete(QuestProgress progress, int idx)
		{
			QuestData configData = progress.configData;
			return CompleteTierReachTaskHandler.ValidTask(configData.task_type[idx]) && configData.task_count[idx] <= progress.tasksProgress[idx].progress;
		}

		// Token: 0x060039A3 RID: 14755 RVA: 0x0011B700 File Offset: 0x00119B00
		public void UpdateTierCount(int tier, int count)
		{
			string tierString = tier.ToString();
			QuestManager questService = base.questService;
			if (CompleteTierReachTaskHandler._003C_003Ef__mg_0024cache0 == null)
			{
				CompleteTierReachTaskHandler._003C_003Ef__mg_0024cache0 = new Func<string, bool>(CompleteTierReachTaskHandler.ValidTask);
			}
			questService.ProcessFilteredTasks(CompleteTierReachTaskHandler._003C_003Ef__mg_0024cache0, new Func<QuestProgress, int, bool>(this.TaskComplete), delegate(QuestProgress progress, int idx)
			{
				QuestData configData = progress.configData;
				if (tierString == configData.task_item[idx])
				{
					progress.tasksProgress[idx].progress = Mathf.Min(configData.task_count[idx], count);
				}
				if (this.TaskComplete(progress, idx))
				{
					progress.tasksProgress[idx].collected = true;
				}
			});
		}

		// Token: 0x040061C1 RID: 25025
		private static string taskTypeName = "complete_tier_reach";

		// Token: 0x040061C2 RID: 25026
		[CompilerGenerated]
		private static Func<string, bool> _003C_003Ef__mg_0024cache0;
	}
}
