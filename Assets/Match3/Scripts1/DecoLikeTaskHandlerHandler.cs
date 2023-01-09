using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000943 RID: 2371
namespace Match3.Scripts1
{
	public class DecoLikeTaskHandlerHandler : QuestTaskHandler
	{
		// Token: 0x060039A5 RID: 14757 RVA: 0x0011B7F8 File Offset: 0x00119BF8
		public DecoLikeTaskHandlerHandler(QuestManager questService, VillagerConfig _villageConfig) : base(questService)
		{
			this.villageConfig = _villageConfig;
		}

		// Token: 0x060039A6 RID: 14758 RVA: 0x0011B808 File Offset: 0x00119C08
		private static bool ValidTask(string taskType)
		{
			return taskType == DecoLikeTaskHandlerHandler.taskTypeName;
		}

		// Token: 0x060039A7 RID: 14759 RVA: 0x0011B815 File Offset: 0x00119C15
		public override string GetTaskType()
		{
			return DecoLikeTaskHandlerHandler.taskTypeName;
		}

		// Token: 0x060039A8 RID: 14760 RVA: 0x0011B81C File Offset: 0x00119C1C
		public override bool TaskComplete(QuestProgress progress, int idx)
		{
			QuestData configData = progress.configData;
			return DecoLikeTaskHandlerHandler.ValidTask(configData.task_type[idx]) && configData.task_count[idx] <= progress.tasksProgress[idx].progress;
		}

		// Token: 0x060039A9 RID: 14761 RVA: 0x0011B860 File Offset: 0x00119C60
		public bool likesBuilding(string buildingTag, IEnumerable<string> villagerTags)
		{
			return !string.IsNullOrEmpty(buildingTag) && villagerTags.Any((string cTag) => buildingTag == cTag);
		}

		// Token: 0x060039AA RID: 14762 RVA: 0x0011B8A0 File Offset: 0x00119CA0
		public void OnBuildingPurchased(BuildingConfig newBuilding)
		{
			QuestManager questService = base.questService;
			if (DecoLikeTaskHandlerHandler._003C_003Ef__mg_0024cache0 == null)
			{
				DecoLikeTaskHandlerHandler._003C_003Ef__mg_0024cache0 = new Func<string, bool>(DecoLikeTaskHandlerHandler.ValidTask);
			}
			questService.ProcessFilteredTasks(DecoLikeTaskHandlerHandler._003C_003Ef__mg_0024cache0, new Func<QuestProgress, int, bool>(this.TaskComplete), delegate(QuestProgress progress, int idx)
			{
				QuestData configData = progress.configData;
				VillagerData villager = this.villageConfig.GetVillager(configData.task_item[idx]);
				if (villager == null)
				{
					WoogaDebug.LogError(new object[]
					{
						"Could not find villager specified in quest: " + configData.task_item[idx] + " - " + configData.id
					});
					return;
				}
				if (this.likesBuilding(newBuilding.tag, villager.likes_deco_tag))
				{
					progress.tasksProgress[idx].progress = Mathf.Min(configData.task_count[idx], progress.tasksProgress[idx].progress + 1);
				}
				if (this.TaskComplete(progress, idx))
				{
					progress.tasksProgress[idx].collected = true;
				}
			});
		}

		// Token: 0x040061C3 RID: 25027
		private readonly VillagerConfig villageConfig;

		// Token: 0x040061C4 RID: 25028
		private static string taskTypeName = "build_liked_deco";

		// Token: 0x040061C5 RID: 25029
		[CompilerGenerated]
		private static Func<string, bool> _003C_003Ef__mg_0024cache0;
	}
}
