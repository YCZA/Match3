

// Token: 0x02000928 RID: 2344
namespace Match3.Scripts1
{
	public abstract class QuestActionHandler<T> : IQuestActionHandler where T : class, IQuestAction
	{
		// Token: 0x06003905 RID: 14597 RVA: 0x00118C4E File Offset: 0x0011704E
		public bool CanHandle(IQuestAction action)
		{
			return action is T;
		}

		// Token: 0x06003906 RID: 14598 RVA: 0x00118C59 File Offset: 0x00117059
		public void Handle(IQuestAction action)
		{
			this.DoHandle(action as T);
		}

		// Token: 0x06003907 RID: 14599
		public abstract void DoHandle(T action);
	}
}
