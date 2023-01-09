using System;
using Match3.Scripts1.Puzzletown.Match3;
using Match3.Scripts1.Puzzletown.Services;

// Token: 0x02000946 RID: 2374
namespace Match3.Scripts1
{
	public class LevelComplete_ReachObserver : QuestTaskObserver
	{
		// Token: 0x060039B0 RID: 14768 RVA: 0x0011BAF4 File Offset: 0x00119EF4
		public LevelComplete_ReachObserver(QuestManager manager, ProgressionDataService.Service progression) : base(manager)
		{
			this.progression = progression;
			progression.onTierCompleted.AddListener(delegate(Level _)
			{
				this.CheckForLevelTiers();
			});
			manager.OnQuestChanged.AddListener(delegate(QuestProgress progress)
			{
				if (progress.status == QuestProgress.Status.started)
				{
					this.CheckForLevelTiers();
				}
			});
			int num = Enum.GetNames(typeof(AreaConfig.Tier)).Length;
			this.tierCounters = new int[num];
		}

		// Token: 0x060039B1 RID: 14769 RVA: 0x0011BB5C File Offset: 0x00119F5C
		private void CheckForLevelTiers()
		{
			for (int i = 0; i < this.tierCounters.Length; i++)
			{
				this.tierCounters[i] = this.progression.NumberOfCompletedLevelsAtTier(i);
			}
			foreach (CompleteTierReachTaskHandler completeTierReachTaskHandler in base.questManager.GetTaskHandlers<CompleteTierReachTaskHandler>())
			{
				for (int j = 0; j < this.tierCounters.Length; j++)
				{
					completeTierReachTaskHandler.UpdateTierCount(j, this.tierCounters[j]);
				}
			}
		}

		// Token: 0x040061C7 RID: 25031
		private readonly ProgressionDataService.Service progression;

		// Token: 0x040061C8 RID: 25032
		private readonly int[] tierCounters;
	}
}
