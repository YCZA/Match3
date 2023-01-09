using System;
using Match3.Scripts1.Puzzletown.Services;

// Token: 0x02000949 RID: 2377
namespace Match3.Scripts1
{
	public class ReachTaskObserver : QuestTaskObserver
	{
		// Token: 0x060039C1 RID: 14785 RVA: 0x0011BEE8 File Offset: 0x0011A2E8
		public ReachTaskObserver(QuestManager manager, ResourceDataService resources) : base(manager)
		{
			this.Resources = resources;
			this.Resources.onChanged.AddListener(new Action<MaterialChange>(this.HandleResourceChanged));
			manager.OnQuestChanged.AddListener(delegate(QuestProgress progress)
			{
				if (progress.status == QuestProgress.Status.started)
				{
					this.HandleCheckAllResources();
				}
			});
		}

		// Token: 0x060039C2 RID: 14786 RVA: 0x0011BF38 File Offset: 0x0011A338
		private void HandleCheckAllResources()
		{
			foreach (MaterialAmount materialAmount in this.Resources.Current)
			{
				if (materialAmount.amount != 0)
				{
					MaterialChange newAmount = new MaterialChange(materialAmount.type, materialAmount.amount, materialAmount.amount);
					foreach (ReachTaskHandler reachTaskHandler in base.questManager.GetTaskHandlers<ReachTaskHandler>())
					{
						reachTaskHandler.HandleResourceChanged(newAmount);
					}
				}
			}
		}

		// Token: 0x060039C3 RID: 14787 RVA: 0x0011C010 File Offset: 0x0011A410
		private void HandleResourceChanged(MaterialChange change)
		{
			foreach (ReachTaskHandler reachTaskHandler in base.questManager.GetTaskHandlers<ReachTaskHandler>())
			{
				reachTaskHandler.HandleResourceChanged(change);
			}
		}

		// Token: 0x040061CE RID: 25038
		private readonly ResourceDataService Resources;
	}
}
