using System;
using Match3.Scripts1.Puzzletown.Services;

// Token: 0x0200093F RID: 2367
namespace Match3.Scripts1
{
	public class CollectTaskObserver : QuestTaskObserver
	{
		// Token: 0x06003992 RID: 14738 RVA: 0x0011B558 File Offset: 0x00119958
		public CollectTaskObserver(QuestManager manager, ResourceDataService resources) : base(manager)
		{
			resources.onChanged.AddListener(new Action<MaterialChange>(this.ProcessMaterialChange));
		}

		// Token: 0x06003993 RID: 14739 RVA: 0x0011B578 File Offset: 0x00119978
		public void ProcessMaterialChange(MaterialChange change)
		{
			MaterialAmount collected = new MaterialAmount(change.name, change.after - change.before, MaterialAmountUsage.Undefined, 0);
			if (collected.amount < 0)
			{
				return;
			}
			foreach (ACollectTaskHandler acollectTaskHandler in base.questManager.GetTaskHandlers<ACollectTaskHandler>())
			{
				acollectTaskHandler.CollectResource(collected);
			}
		}
	}
}
