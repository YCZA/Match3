using System.Collections;
using Match3.Scripts1.Puzzletown.Config;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Shared.Flows;
using Wooga.Coroutines;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;
using Match3.Scripts2.Shop;

// Token: 0x0200089A RID: 2202
namespace Match3.Scripts1
{
	public class BuyJourney : AFlowR<BoosterData, bool>
	{
		// Token: 0x060035F2 RID: 13810 RVA: 0x001038E0 File Offset: 0x00101CE0
		public BuyJourney(TrackingService.PurchaseFlowContext purchaseContext, TownDiamondsPanelRoot.TownDiamondsPanelRootParameters purchaseDiamondsContext)
		{
			this._purchaseContext = purchaseContext;
			this._purchaseDiamondsContext = purchaseDiamondsContext;
		}

		// Token: 0x060035F3 RID: 13811 RVA: 0x001038F8 File Offset: 0x00101CF8
		protected override IEnumerator FlowRoutine(BoosterData input)
		{
			yield return ServiceLocator.Instance.Inject(this);
			MaterialAmount refillCost = new MaterialAmount("diamonds", input.cost, MaterialAmountUsage.Undefined, 0);
			MaterialAmount refillAmount = new MaterialAmount(input.type, input.amount, MaterialAmountUsage.Undefined, 0);
			string boost_ = "boost_";
			string title;
			string content;
			if (refillAmount.type.StartsWith(boost_))
			{
				string arg = refillAmount.type.Substring(boost_.Length);
				title = this.loc.GetText(string.Format("ui.boosts.ingame.add.{0}.title", arg), new LocaParam[0]);
				content = this.loc.GetText(string.Format("ui.boosts.ingame.add.{0}.body", arg), new LocaParam[0]);
			}
			else
			{
				title = string.Format(this.loc.GetText("ui.shared.purchase.title", new LocaParam[0]), refillAmount.FormatName(this.loc));
				content = string.Format(this.loc.GetText("ui.shared.purchase.content", new LocaParam[0]), refillAmount.Format(this.loc), refillCost.Format(this.loc));
			}
			Wooroutine<bool> buyJourney = new BuyWithDiamondsJourney(title, content, refillAmount, refillCost, this._purchaseContext, this._purchaseDiamondsContext).Start<bool>();
			yield return buyJourney;
			yield return buyJourney.ReturnValue;
			yield break;
		}

		// Token: 0x04005DDF RID: 24031
		[WaitForService(true, true)]
		private ILocalizationService loc;

		// Token: 0x04005DE0 RID: 24032
		[WaitForService(true, true)]
		private GameStateService gameState;

		// Token: 0x04005DE1 RID: 24033
		private TrackingService.PurchaseFlowContext _purchaseContext;

		// Token: 0x04005DE2 RID: 24034
		private TownDiamondsPanelRoot.TownDiamondsPanelRootParameters _purchaseDiamondsContext;
	}
}
