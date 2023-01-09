using System;
using Match3.Scripts1.Puzzletown.Services;

// Token: 0x02000940 RID: 2368
namespace Match3.Scripts1
{
	public abstract class ACompleteTierTaskHandler : QuestTaskHandler
	{
		// Token: 0x06003994 RID: 14740 RVA: 0x0011B19B File Offset: 0x0011959B
		public ACompleteTierTaskHandler(QuestManager questService, ProgressionDataService.Service progression) : base(questService)
		{
			this.progression = progression;
		}

		// Token: 0x06003995 RID: 14741 RVA: 0x0011B1AC File Offset: 0x001195AC
		public void CompleteTier(int levelTier)
		{
			string tierString = levelTier.ToString();
			base.questService.ProcessFilteredTasks(new Func<string, bool>(this.ValidTask), new Func<QuestProgress, int, bool>(this.TaskComplete), delegate(QuestProgress progress, int idx)
			{
				this.HandleTask(progress, idx, tierString);
			});
		}

		// Token: 0x06003996 RID: 14742 RVA: 0x0011B20C File Offset: 0x0011960C
		public override bool TaskComplete(QuestProgress progress, int idx)
		{
			QuestData configData = progress.configData;
			if (!this.ValidTask(configData.task_type[idx]))
			{
				return false;
			}
			int progress2 = this.GetProgress(progress, idx);
			int count = configData.Tasks[idx].count;
			return count <= progress2;
		}

		// Token: 0x06003997 RID: 14743
		protected abstract bool ValidTask(string taskType);

		// Token: 0x06003998 RID: 14744 RVA: 0x0011B258 File Offset: 0x00119658
		public override int GetProgress(QuestProgress progress, int idx)
		{
			QuestData configData = progress.configData;
			string[] levels = configData.Tasks[idx].levels;
			int num = 0;
			foreach (string name in levels)
			{
				Level level = Level.Parse(name);
				if (this.progression.GetTier(level.level) >= level.tier)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06003999 RID: 14745
		protected abstract void HandleTask(QuestProgress progress, int idx, string tierString);

		// Token: 0x040061BF RID: 25023
		private ProgressionDataService.Service progression;
	}
}
