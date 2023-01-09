using System.Collections.Generic;

// Token: 0x02000929 RID: 2345
namespace Match3.Scripts1
{
	public class QuestActionHandlerManager
	{
		// Token: 0x06003909 RID: 14601 RVA: 0x0011906A File Offset: 0x0011746A
		public void RegisterHandler(IQuestActionHandler newHandler)
		{
			this._handlers.Add(newHandler);
		}

		// Token: 0x0600390A RID: 14602 RVA: 0x00119078 File Offset: 0x00117478
		public void HandleAction(IQuestAction currentAction)
		{
			foreach (IQuestActionHandler questActionHandler in this._handlers)
			{
				if (questActionHandler.CanHandle(currentAction))
				{
					questActionHandler.Handle(currentAction);
				}
			}
		}

		// Token: 0x0400615D RID: 24925
		private readonly List<IQuestActionHandler> _handlers = new List<IQuestActionHandler>();
	}
}
