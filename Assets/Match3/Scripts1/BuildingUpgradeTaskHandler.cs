using System;
using System.Runtime.CompilerServices;
using Match3.Scripts1.Town;
using UnityEngine;

// Token: 0x0200093A RID: 2362
namespace Match3.Scripts1
{
	public class BuildingUpgradeTaskHandler : QuestTaskHandler
	{
		// Token: 0x06003977 RID: 14711 RVA: 0x0011B042 File Offset: 0x00119442
		public BuildingUpgradeTaskHandler(QuestManager questService) : base(questService)
		{
		}

		// Token: 0x06003978 RID: 14712 RVA: 0x0011B04B File Offset: 0x0011944B
		private static bool ValidTask(string taskType)
		{
			return taskType == BuildingUpgradeTaskHandler.taskTypeName;
		}

		// Token: 0x06003979 RID: 14713 RVA: 0x0011B058 File Offset: 0x00119458
		public override string GetTaskType()
		{
			return BuildingUpgradeTaskHandler.taskTypeName;
		}

		// Token: 0x0600397A RID: 14714 RVA: 0x0011B060 File Offset: 0x00119460
		public override bool TaskComplete(QuestProgress progress, int idx)
		{
			QuestData configData = progress.configData;
			return BuildingUpgradeTaskHandler.ValidTask(configData.task_type[idx]) && configData.task_count[idx] <= progress.tasksProgress[idx].progress;
		}

		// Token: 0x0600397B RID: 14715 RVA: 0x0011B0A4 File Offset: 0x001194A4
		public void HandleBuildingUpgraded(BuildingInstance building)
		{
			QuestManager questService = base.questService;
			if (BuildingUpgradeTaskHandler._003C_003Ef__mg_0024cache0 == null)
			{
				BuildingUpgradeTaskHandler._003C_003Ef__mg_0024cache0 = new Func<string, bool>(BuildingUpgradeTaskHandler.ValidTask);
			}
			questService.ProcessFilteredTasks(BuildingUpgradeTaskHandler._003C_003Ef__mg_0024cache0, new Func<QuestProgress, int, bool>(this.TaskComplete), delegate(QuestProgress progress, int idx)
			{
				QuestData configData = progress.configData;
				if (building.blueprint.name == configData.task_item[idx])
				{
					progress.tasksProgress[idx].progress = Mathf.Min(configData.task_count[idx], progress.tasksProgress[idx].progress + 1);
				}
				if (this.TaskComplete(progress, idx))
				{
					progress.tasksProgress[idx].collected = true;
				}
			});
		}

		// Token: 0x040061B9 RID: 25017
		private static string taskTypeName = "upgrade";

		// Token: 0x040061BA RID: 25018
		[CompilerGenerated]
		private static Func<string, bool> _003C_003Ef__mg_0024cache0;
	}
}
