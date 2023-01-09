using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Shared.Flows;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;
using Match3.Scripts2.Shop;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Flows
{
	// Token: 0x020004C2 RID: 1218
	public class PaymentFlow : AFlowR<IEnumerable<MaterialAmount>, bool>
	{
		// Token: 0x0600222B RID: 8747 RVA: 0x000943DE File Offset: 0x000927DE
		public PaymentFlow(TrackingService.PurchaseFlowContext purchaseFlowContext, TownDiamondsPanelRoot.TownDiamondsPanelRootParameters purchaseDiamondsContext)
		{
			WoogaDebug.Log(new object[]
			{
				"New Playment Flow"
			});
			this._purchaseFlowContext = purchaseFlowContext;
			this._purchaseDiamondsContext = purchaseDiamondsContext;
		}

		// Token: 0x0600222C RID: 8748 RVA: 0x00094408 File Offset: 0x00092808
		protected override IEnumerator FlowRoutine(IEnumerable<MaterialAmount> payment)
		{
			yield return ServiceLocator.Instance.Inject(this);
			if (this.state.Resources.Pay(payment))
			{
				int coins = payment.FirstOrDefault((MaterialAmount ma) => ma.type == "coins").amount;
				int diamonds = payment.FirstOrDefault((MaterialAmount ma) => ma.type == "diamonds").amount;
				this.trackingService.TrackPurchase(this._purchaseFlowContext, coins, diamonds);
				yield return true;
			}
			else
			{
				Coroutine purchase = new PurchaseFlow(this._purchaseFlowContext, this._purchaseDiamondsContext).Start(payment.FirstOrDefault<MaterialAmount>());
				yield return purchase;
				yield return this.state.Resources.Pay(payment);
			}
			yield break;
		}

		// Token: 0x04004D88 RID: 19848
		[WaitForService(true, true)]
		private GameStateService state;

		// Token: 0x04004D89 RID: 19849
		[WaitForService(true, true)]
		private ILocalizationService loc;

		// Token: 0x04004D8A RID: 19850
		[WaitForService(true, true)]
		private ConfigService configService;

		// Token: 0x04004D8B RID: 19851
		[WaitForService(true, true)]
		private TrackingService trackingService;

		// Token: 0x04004D8C RID: 19852
		private TrackingService.PurchaseFlowContext _purchaseFlowContext;

		// Token: 0x04004D8D RID: 19853
		private TownDiamondsPanelRoot.TownDiamondsPanelRootParameters _purchaseDiamondsContext;
	}
}
