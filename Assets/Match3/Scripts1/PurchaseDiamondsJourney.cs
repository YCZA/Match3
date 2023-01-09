using System.Collections;
using Match3.Scripts1.Puzzletown.UI;
using Match3.Scripts1.Shared.Flows;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.Signals;
using Wooga.UnityFramework;
using Match3.Scripts2.Shop;
using UnityEngine;

// Token: 0x020009D2 RID: 2514
namespace Match3.Scripts1
{
	public class PurchaseDiamondsJourney : AFlow
	{
		// Token: 0x06003CDD RID: 15581 RVA: 0x00131C94 File Offset: 0x00130094
		public PurchaseDiamondsJourney(TownDiamondsPanelRoot.TownDiamondsPanelRootParameters context, bool showNotEnoughDialog = true)
		{
			WoogaDebug.Log(new object[]
			{
				"Starting a PurchaseDiamondsJourny"
			});
			if (context == null)
			{
				WoogaDebug.LogWarning(new object[]
				{
					"Null tracking context passed in"
				});
			}
			this._context = context;
			this._showNotEnoughDialog = showNotEnoughDialog;
		}

		// Token: 0x06003CDE RID: 15582 RVA: 0x00131CE4 File Offset: 0x001300E4
		protected override IEnumerator FlowRoutine()
		{
			yield return ServiceLocator.Instance.Inject(this);
			yield return SceneManager.Instance.Inject(this);
			if (Application.internetReachability != NetworkReachability.NotReachable)
			{
				bool goToShop = true;
				if (this._showNotEnoughDialog)
				{
					MaterialAmount diamonds = new MaterialAmount("diamonds", 0, MaterialAmountUsage.Undefined, 0);
					string title = string.Format(this.loc.GetText("ui.shared.purchase.title", new LocaParam[0]), diamonds.FormatName(this.loc));
					string content = string.Format(this.loc.GetText("ui.shared.purchase.diamonds.title", new LocaParam[0]), diamonds.Format(this.loc));
					string action = this.loc.GetText("ui.shared.purchase.diamonds.button", new LocaParam[0]);
					AwaitSignal<bool> onEnterConfirm = new AwaitSignal<bool>();
					Wooroutine<PopupDialogRoot> popup = PopupDialogRoot.Show(new object[]
					{
						TextData.Title(title),
						TextData.Content(content),
						new CloseButton(delegate()
						{
							onEnterConfirm.Dispatch(false);
						}),
						diamonds,
						new LabeledButtonWithCallback(action, delegate()
						{
							onEnterConfirm.Dispatch(true);
						})
					});
					yield return popup;
					yield return popup.ReturnValue.onDisabled.Await();
					goToShop = onEnterConfirm.Dispatched;
				}
				if (goToShop)
				{
					// this.shop.context = this._context;
					// yield return this.shop.TryToBuy();
				}
			}
			else
			{
				// Debug.LogError("popupfbroot");
				yield return PopupMissingAssetsRoot.TryShowRoutine("");
				// Wooroutine<PopupFacebookRoot> facebookPopup = SceneManager.Instance.LoadSceneWithParams<PopupFacebookRoot, FacebookPopupParameters>(new FacebookPopupParameters(FacebookPopupState.NoConnection, FacebookLoginContext.settings), null);
				// yield return facebookPopup;
				// yield return facebookPopup.ReturnValue.onDestroyed;
			}
		}

		// Token: 0x04006592 RID: 26002
		private TownDiamondsPanelRoot.TownDiamondsPanelRootParameters _context;

		// Token: 0x04006593 RID: 26003
		private bool _showNotEnoughDialog;

		// Token: 0x04006594 RID: 26004
		[WaitForService(true, true)]
		private ILocalizationService loc;

		// Token: 0x04006595 RID: 26005
		// [WaitForRoot(false, false)]
		// private TownDiamondsPanelRoot shop;
	}
}
