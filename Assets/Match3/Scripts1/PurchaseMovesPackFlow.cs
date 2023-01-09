using System.Collections;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Puzzletown.UI;
using Match3.Scripts1.Shared.Flows;
using Wooga.Coroutines;
using Wooga.UnityFramework;
using Match3.Scripts2.Shop;

// Token: 0x020009D3 RID: 2515
namespace Match3.Scripts1
{
	public class PurchaseMovesPackFlow : AFlow
	{
		// Token: 0x06003CDF RID: 15583 RVA: 0x00132084 File Offset: 0x00130484
		public PurchaseMovesPackFlow(IAPData iapData)
		{
			this.iapData = iapData;
		}

		// Token: 0x17000916 RID: 2326
		// (get) Token: 0x06003CE0 RID: 15584 RVA: 0x00132093 File Offset: 0x00130493
		// (set) Token: 0x06003CE1 RID: 15585 RVA: 0x0013209B File Offset: 0x0013049B
		public PurchaseResult Result { get; private set; }

		// Token: 0x06003CE2 RID: 15586 RVA: 0x001320A4 File Offset: 0x001304A4
		protected override IEnumerator FlowRoutine()
		{
			yield return ServiceLocator.Instance.Inject(this);
			yield return SceneManager.Instance.Inject(this);
			yield return this.TryPurchaseRoutine(this.iapData);
		}

		// Token: 0x06003CE3 RID: 15587 RVA: 0x001320C0 File Offset: 0x001304C0
		private IEnumerator TryPurchaseRoutine(IAPData data)
		{
			Wooroutine<PurchaseResult> tryIap = this.iapService.TryPurchase(data);
			yield return tryIap;
			if (!tryIap.ReturnValue.success)
			{
				// string reason = tryIap.ReturnValue.failureReason.ToString();
				string reason = "@@@@@@";
				WoogaDebug.LogWarning(new object[]
				{
					"(PURCHASE FAILED) " + reason
				});
				yield return this.PurchaseFailed(data, reason);
			}
			this.Result = tryIap.ReturnValue;
			yield break;
		}

		// Token: 0x06003CE4 RID: 15588 RVA: 0x001320E4 File Offset: 0x001304E4
		private IEnumerator PurchaseFailed(IAPData data, string failureReason)
		{
			yield return PopupDialogRoot.Show(new object[]
			{
				TextData.Title(this.localizationService.GetText("ui.common.failed_title", new LocaParam[0])),
				IllustrationType.SadCharacter,
				TextData.Content(this.localizationService.GetText("ui.shop.purchase_failed_generic", new LocaParam[0])),
				new LabeledButtonWithCallback(this.localizationService.GetText("ui.shop.purchase_retry", new LocaParam[0]), null)
			});
			yield break;
		}

		// Token: 0x04006596 RID: 26006
		[WaitForService(true, true)]
		private IAPService iapService;

		// Token: 0x04006597 RID: 26007
		[WaitForService(true, true)]
		private TrackingService trackingService;

		// Token: 0x04006598 RID: 26008
		[WaitForService(true, true)]
		private ILocalizationService localizationService;

		// Token: 0x0400659A RID: 26010
		private IAPData iapData;
	}
}
