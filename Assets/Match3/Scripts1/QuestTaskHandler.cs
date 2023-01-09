

// Token: 0x02000938 RID: 2360
namespace Match3.Scripts1
{
	public abstract class QuestTaskHandler
	{
		// Token: 0x0600396D RID: 14701 RVA: 0x0011AFE3 File Offset: 0x001193E3
		protected QuestTaskHandler(QuestManager service)
		{
			this.questService = service;
		}

		// Token: 0x170008DD RID: 2269
		// (get) Token: 0x0600396E RID: 14702 RVA: 0x0011AFF2 File Offset: 0x001193F2
		// (set) Token: 0x0600396F RID: 14703 RVA: 0x0011AFFA File Offset: 0x001193FA
		// private protected QuestManager questService { protected get; private set; }
		private protected QuestManager questService { get; private set; }

		// Token: 0x06003970 RID: 14704
		public abstract bool TaskComplete(QuestProgress progress, int idx);

		// Token: 0x06003971 RID: 14705 RVA: 0x0011B003 File Offset: 0x00119403
		public virtual void OnCollected(QuestProgress progress, int idx)
		{
			progress.tasksProgress[idx].collected = true;
		}

		// Token: 0x06003972 RID: 14706 RVA: 0x0011B013 File Offset: 0x00119413
		public virtual int GetProgress(QuestProgress progress, int idx)
		{
			return progress.tasksProgress[idx].progress;
		}

		// Token: 0x06003973 RID: 14707
		public abstract string GetTaskType();
	}
}
