using System;
using Match3.Scripts1.Puzzletown.Services;

// Token: 0x02000945 RID: 2373
namespace Match3.Scripts1
{
	public class LevelComplete_CollectObserver : QuestTaskObserver
	{
		// Token: 0x060039AE RID: 14766 RVA: 0x0011BA6C File Offset: 0x00119E6C
		public LevelComplete_CollectObserver(QuestManager manager, ProgressionDataService.Service progression) : base(manager)
		{
			progression.onTierCompleted.AddListener(new Action<Level>(this.HandleTierCompleted));
		}

		// Token: 0x060039AF RID: 14767 RVA: 0x0011BA8C File Offset: 0x00119E8C
		private void HandleTierCompleted(Level level)
		{
			foreach (CompleteTierCollectTaskHandler completeTierCollectTaskHandler in base.questManager.GetTaskHandlers<CompleteTierCollectTaskHandler>())
			{
				completeTierCollectTaskHandler.CompleteTier(level.tier);
			}
		}
	}
}
