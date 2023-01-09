using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.QStringUtil;

// Token: 0x0200093B RID: 2363
namespace Match3.Scripts1
{
	public class CollectAndInteractTaskHandler : ACompleteTierTaskHandler
	{
		// Token: 0x0600397D RID: 14717 RVA: 0x0011B2EA File Offset: 0x001196EA
		public CollectAndInteractTaskHandler(QuestManager questService, ProgressionDataService.Service progression) : base(questService, progression)
		{
		}

		// Token: 0x0600397E RID: 14718 RVA: 0x0011B2F4 File Offset: 0x001196F4
		public override string GetTaskType()
		{
			return CollectAndInteractTaskHandler.taskTypeName;
		}

		// Token: 0x0600397F RID: 14719 RVA: 0x0011B2FB File Offset: 0x001196FB
		protected override bool ValidTask(string taskType)
		{
			return taskType.QCompare(CollectAndInteractTaskHandler.taskTypeName);
		}

		// Token: 0x06003980 RID: 14720 RVA: 0x0011B308 File Offset: 0x00119708
		public override void OnCollected(QuestProgress progress, int idx)
		{
			progress.tasksProgress[idx].collected = true;
			base.questService.TriggerAction(new DummyAction());
		}

		// Token: 0x06003981 RID: 14721 RVA: 0x0011B328 File Offset: 0x00119728
		protected override void HandleTask(QuestProgress progress, int idx, string tierString)
		{
		}

		// Token: 0x040061BB RID: 25019
		private static string taskTypeName = "collect_and_interact";
	}
}
