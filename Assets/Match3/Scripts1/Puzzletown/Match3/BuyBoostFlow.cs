using System;
using System.Collections;
using Match3.Scripts1.Puzzletown.Config;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Shared.Flows;
using Wooga.Coroutines;
using Wooga.UnityFramework;
using Match3.Scripts2.Shop;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x02000721 RID: 1825
	public class BuyBoostFlow : AFlow<BuyBoostFlow.Input>
	{
		// Token: 0x06002D2E RID: 11566 RVA: 0x000D1A9C File Offset: 0x000CFE9C
		public BuyBoostFlow(BoostsUiRoot boostsUiRoot)
		{
			this.ingameBoostsUiRoot = boostsUiRoot;
		}

		// Token: 0x06002D2F RID: 11567 RVA: 0x000D1AAB File Offset: 0x000CFEAB
		protected void ClearFlowRunning()
		{
			BuyBoostFlow.flowRunning = false;
		}

		// Token: 0x06002D30 RID: 11568 RVA: 0x000D1AB4 File Offset: 0x000CFEB4
		protected override IEnumerator FlowRoutine(BuyBoostFlow.Input input)
		{
			if (BuyBoostFlow.flowRunning)
			{
				yield break;
			}
			BuyBoostFlow.flowRunning = true;
			yield return ServiceLocator.Instance.Inject(this);
			this.sessionService.onRestart.AddListenerOnce(new Action(this.ClearFlowRunning));
			BoosterData data = this.configService.general.boosters.Find((BoosterData e) => e.type == input.info.name);
			TownDiamondsPanelRoot.TownDiamondsPanelRootParameters trackingEvent = new TownDiamondsPanelRoot.TownDiamondsPanelRootParameters
			{
				source1 = input.origin + "_boosts",
				source2 = data.type
			};
			Wooroutine<bool> buyJourney = new BuyJourney(new TrackingService.PurchaseFlowContext
			{
				det1 = "boosts",
				det2 = input.origin,
				det3 = data.type,
				det4 = data.amount.ToString()
			}, trackingEvent).Start(data);
			yield return buyJourney;
			if (buyJourney.ReturnValue)
			{
				this.boostsService.AddBoost(data.type, data.amount);
			}
			else if (this.ingameBoostsUiRoot != null)
			{
				this.ingameBoostsUiRoot.UnselectActive();
			}
			this.ClearFlowRunning();
			this.sessionService.onRestart.RemoveListener(new Action(this.ClearFlowRunning));
			yield break;
		}

		// Token: 0x040056BA RID: 22202
		[WaitForService(true, true)]
		private ConfigService configService;

		// Token: 0x040056BB RID: 22203
		[WaitForService(true, true)]
		private BoostsService boostsService;

		// Token: 0x040056BC RID: 22204
		[WaitForService(true, true)]
		private SessionService sessionService;

		// Token: 0x040056BD RID: 22205
		private BoostsUiRoot ingameBoostsUiRoot;

		// Token: 0x040056BE RID: 22206
		private static bool flowRunning;

		// Token: 0x02000722 RID: 1826
		public class Input
		{
			// Token: 0x06002D31 RID: 11569 RVA: 0x000D1AD6 File Offset: 0x000CFED6
			public Input(BoostViewData info, string origin)
			{
				this.info = info;
				this.origin = origin;
			}

			// Token: 0x040056BF RID: 22207
			public BoostViewData info;

			// Token: 0x040056C0 RID: 22208
			public string origin;
		}
	}
}
