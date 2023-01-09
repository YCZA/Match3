using System;
using System.Collections;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Shared.Flows;
using Match3.Scripts1.Town;
using Wooga.Coroutines;
using Wooga.UnityFramework;
using Match3.Scripts2.Android.DataStatistics;
using Match3.Scripts2.PlayerData;
using Match3.Scripts2.Shop; // using Firebase.Analytics;

namespace Match3.Scripts1.Puzzletown.Flows
{
	// Token: 0x020004C3 RID: 1219
	public class PurchaseBuildingFlow : AFlowR<BuildingInstance, bool>
	{
		// Token: 0x0600222E RID: 8750 RVA: 0x0009465C File Offset: 0x00092A5C
		protected override IEnumerator FlowRoutine(BuildingInstance building)
		{
			if (PurchaseBuildingFlow.flowRunning)
			{
				yield return false;
				yield break;
			}
			PurchaseBuildingFlow.flowRunning = true;
			yield return ServiceLocator.Instance.Inject(this);
			this.sessionService.onRestart.AddListenerOnce(new Action(this.HandleRestart));
			TownDiamondsPanelRoot.TownDiamondsPanelRootParameters trackingEvent = new TownDiamondsPanelRoot.TownDiamondsPanelRootParameters
			{
				source1 = "building_shop",
				source2 = building.blueprint.name
			};
			if (building.isFree)
			{
				yield return true;
			}
			else
			{
				if (!this.state.Resources.HasMaterials(building.blueprint.costs))
				{
					// 宝石或钻石不够
					foreach (MaterialAmount ma in building.blueprint.costs)
					{
						int stock = this.state.Resources.GetAmount(ma.type);
						if (ma.amount > stock)
						{
							MaterialAmount missing = new MaterialAmount(ma.type, ma.amount - stock, MaterialAmountUsage.Undefined, 0);
							yield return new PurchaseFlow(new TrackingService.PurchaseFlowContext
							{
								det1 = "building",
								det2 = building.blueprint.name,
								det3 = building.blueprint.type.ToString(),
								det4 = building.blueprint.chapter_id.ToString()
							}, trackingEvent).Start(missing);
						}
					}
				}
				if (!this.state.Resources.HasMaterials(building.blueprint.costs))
				{
					// 取消兑换宝石，或取消购买钻石
					yield return false;
				}
				else
				{
					// buried point: 购买建筑
					DataStatistics.Instance.TriggerBuyBuilding(building.blueprint.name, building.blueprint.shop_tag);
					// 购买建筑
					Wooroutine<bool> routine = new PaymentFlow(new TrackingService.PurchaseFlowContext
					{
						det1 = "building",
						det2 = building.blueprint.name,
						det3 = building.blueprint.type.ToString(),
						det4 = building.blueprint.chapter_id.ToString()
					}, trackingEvent).Start(building.blueprint.costs);
					yield return routine;
					yield return routine.ReturnValue;
				}
			}
			this.sessionService.onRestart.RemoveListener(new Action(this.HandleRestart));
			PurchaseBuildingFlow.flowRunning = false;
			yield break;
		}

		// Token: 0x0600222F RID: 8751 RVA: 0x0009467E File Offset: 0x00092A7E
		private void HandleRestart()
		{
			PurchaseBuildingFlow.flowRunning = false;
		}

		// Token: 0x04004D8E RID: 19854
		[WaitForService(true, true)]
		private GameStateService state;

		// Token: 0x04004D8F RID: 19855
		[WaitForService(true, true)]
		private ILocalizationService loc;

		// Token: 0x04004D90 RID: 19856
		[WaitForService(true, true)]
		private ConfigService configService;

		// Token: 0x04004D91 RID: 19857
		[WaitForService(true, true)]
		private SessionService sessionService;

		// Token: 0x04004D92 RID: 19858
		public static bool flowRunning;
	}
}
