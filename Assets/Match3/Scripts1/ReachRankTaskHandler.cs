using System;
using System.Runtime.CompilerServices;

// Token: 0x02000948 RID: 2376
namespace Match3.Scripts1
{
	public class ReachRankTaskHandler : ReachTaskHandler
	{
		// Token: 0x060039BA RID: 14778 RVA: 0x0011BD7A File Offset: 0x0011A17A
		public ReachRankTaskHandler(QuestManager questService, VillageRankHarmonyObserver harmonyObserver) : base(questService)
		{
			this.harmonyObserver = harmonyObserver;
		}

		// Token: 0x060039BB RID: 14779 RVA: 0x0011BD8A File Offset: 0x0011A18A
		private static bool ValidTask(string taskType)
		{
			return taskType == ReachRankTaskHandler.taskTypeName;
		}

		// Token: 0x060039BC RID: 14780 RVA: 0x0011BD97 File Offset: 0x0011A197
		public override string GetTaskType()
		{
			return ReachRankTaskHandler.taskTypeName;
		}

		// Token: 0x060039BD RID: 14781 RVA: 0x0011BDA0 File Offset: 0x0011A1A0
		public override bool TaskComplete(QuestProgress progress, int idx)
		{
			QuestData configData = progress.configData;
			return ReachRankTaskHandler.ValidTask(configData.task_type[idx]) && progress.tasksProgress[idx].progress >= configData.task_count[idx];
		}

		// Token: 0x060039BE RID: 14782 RVA: 0x0011BDE2 File Offset: 0x0011A1E2
		public override int GetProgress(QuestProgress progress, int idx)
		{
			return this.harmonyObserver.CurrentRank;
		}

		// Token: 0x060039BF RID: 14783 RVA: 0x0011BDF0 File Offset: 0x0011A1F0
		public override void HandleResourceChanged(MaterialChange newAmount)
		{
			QuestManager questService = base.questService;
			if (ReachRankTaskHandler._003C_003Ef__mg_0024cache0 == null)
			{
				ReachRankTaskHandler._003C_003Ef__mg_0024cache0 = new Func<string, bool>(ReachRankTaskHandler.ValidTask);
			}
			questService.ProcessFilteredTasks(ReachRankTaskHandler._003C_003Ef__mg_0024cache0, new Func<QuestProgress, int, bool>(this.TaskComplete), delegate(QuestProgress progress, int idx)
			{
				if (newAmount.name != "harmony")
				{
					return;
				}
				progress.tasksProgress[idx].progress = this.harmonyObserver.CurrentRank;
				if (this.TaskComplete(progress, idx))
				{
					progress.tasksProgress[idx].SetCompleted(progress.configData, idx);
					this.questService.CollectQuestTask(progress, idx);
				}
			});
		}

		// Token: 0x040061CB RID: 25035
		private VillageRankHarmonyObserver harmonyObserver;

		// Token: 0x040061CC RID: 25036
		private static string taskTypeName = "reach_rank";

		// Token: 0x040061CD RID: 25037
		[CompilerGenerated]
		private static Func<string, bool> _003C_003Ef__mg_0024cache0;
	}
}
