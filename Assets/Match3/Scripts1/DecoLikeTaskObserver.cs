using System.Collections.Generic;

// Token: 0x02000944 RID: 2372
namespace Match3.Scripts1
{
	public class DecoLikeTaskObserver : QuestTaskObserver
	{
		// Token: 0x060039AC RID: 14764 RVA: 0x0011B9F5 File Offset: 0x00119DF5
		public DecoLikeTaskObserver(QuestManager manager) : base(manager)
		{
			this.buildTasks = base.questManager.GetTaskHandlers<DecoLikeTaskHandlerHandler>();
		}

		// Token: 0x060039AD RID: 14765 RVA: 0x0011BA10 File Offset: 0x00119E10
		public void OnBuildingBuilt(BuildingConfig newBuilding)
		{
			foreach (DecoLikeTaskHandlerHandler decoLikeTaskHandlerHandler in this.buildTasks)
			{
				decoLikeTaskHandlerHandler.OnBuildingPurchased(newBuilding);
			}
		}

		// Token: 0x040061C6 RID: 25030
		private IEnumerable<DecoLikeTaskHandlerHandler> buildTasks;
	}
}
