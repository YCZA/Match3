using System.Collections;
using Match3.Scripts1.Audio;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Puzzletown.UI;
using Match3.Scripts1.Shared.Flows;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.Signals;
using Wooga.UnityFramework;
using Match3.Scripts2.Android.DataStatistics;
using Match3.Scripts2.PlayerData;
using Match3.Scripts2.Shop; // using Firebase.Analytics;

// Token: 0x0200089D RID: 2205
namespace Match3.Scripts1
{
	public class BuyWithDiamondsJourney : AFlow
	{
		// Token: 0x060035FB RID: 13819 RVA: 0x001046E3 File Offset: 0x00102AE3
		public BuyWithDiamondsJourney(string header, string desc, MaterialAmount amount, MaterialAmount cost, TrackingService.PurchaseFlowContext purchaseContext, TownDiamondsPanelRoot.TownDiamondsPanelRootParameters purchaseDiamondsContext)
		{
			this.header = header;
			this.desc = desc;
			this.amount = amount;
			this.cost = cost;
			this._purchaseContext = purchaseContext;
			this._purchaseDiamondsContext = purchaseDiamondsContext;
		}

		// Token: 0x060035FC RID: 13820 RVA: 0x00104718 File Offset: 0x00102B18
		private IEnumerator PayWithDiamonds()
		{
			if (this.gameStateService.Resources.GetAmount(this.cost.type) < this.cost.amount)
			{
				yield return new PurchaseDiamondsJourney(this._purchaseDiamondsContext, true).Start();
				yield return this.gameStateService.Resources.Pay(new MaterialAmount[]
				{
					this.cost
				});
			}
			else
			{
				this.gameStateService.Resources.Pay(new MaterialAmount[]
				{
					this.cost
				});
				if (this._purchaseContext != null)
				{
					if (this.cost.type == "diamonds")
					{
						this.trackingService.TrackPurchase(this._purchaseContext, 0, this.cost.amount);
						this.audioService.PlaySFX(AudioId.DiamondsSpent, false, false, false);
					}
					else if (this.cost.type == "coins")
					{
						this.trackingService.TrackPurchase(this._purchaseContext, this.cost.amount, 0);
					}
				}
				yield return true;
			}
			yield break;
		}

		// Token: 0x060035FD RID: 13821 RVA: 0x00104734 File Offset: 0x00102B34
		private IEnumerator AskUserToPay()
		{
			AwaitSignal<bool> onClose = new AwaitSignal<bool>();
			Wooroutine<PopupDialogRoot> popupShowroutine = PopupDialogRoot.Show(new object[]
			{
				TextData.Title(this.header),
				TextData.Content(this.desc),
				new CloseButton(delegate()
				{
					onClose.Dispatch(false);
				}),
				this.amount,
				new PricedButtonWithCallback(this.cost, delegate()
				{
					onClose.Dispatch(true);
				})
			});
			yield return popupShowroutine;
			yield return popupShowroutine.ReturnValue.onDisabled.Await();
			if (!onClose.Dispatched)
			{
				yield return false;
			}
			else
			{
				Wooroutine<bool> payResult = WooroutineRunner.StartWooroutine<bool>(this.PayWithDiamonds());
				yield return payResult;
				if (payResult.ReturnValue)
				{
					// buried point: 购买道具
					DataStatistics.Instance.TriggerBuyBooster(amount.type);
					yield return true;
				}
				else
				{
					Wooroutine<bool> askUser = WooroutineRunner.StartWooroutine<bool>(this.AskUserToPay());
					yield return askUser;
					yield return askUser.ReturnValue;
				}
			}
			yield break;
		}

		// Token: 0x060035FE RID: 13822 RVA: 0x00104750 File Offset: 0x00102B50
		protected override IEnumerator FlowRoutine()
		{
			yield return ServiceLocator.Instance.Inject(this);
			Wooroutine<bool> askResult = WooroutineRunner.StartWooroutine<bool>(this.AskUserToPay());
			yield return askResult;
			yield return askResult.ReturnValue;
			yield break;
		}

		// Token: 0x04005DF5 RID: 24053
		[WaitForService(true, true)]
		private GameStateService gameStateService;

		// Token: 0x04005DF6 RID: 24054
		[WaitForService(true, true)]
		private TrackingService trackingService;

		// Token: 0x04005DF7 RID: 24055
		[WaitForService(true, true)]
		private AudioService audioService;

		// Token: 0x04005DF8 RID: 24056
		private string header;

		// Token: 0x04005DF9 RID: 24057
		private string desc;

		// Token: 0x04005DFA RID: 24058
		private MaterialAmount cost;

		// Token: 0x04005DFB RID: 24059
		private MaterialAmount amount;

		// Token: 0x04005DFC RID: 24060
		private TrackingService.PurchaseFlowContext _purchaseContext;

		// Token: 0x04005DFD RID: 24061
		private TownDiamondsPanelRoot.TownDiamondsPanelRootParameters _purchaseDiamondsContext;
	}
}
