using System;
using System.Collections;
using System.Collections.Generic;
using Match3.Scripts1.Audio;
using Match3.Scripts1.Puzzletown.Config;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Puzzletown.Town;
using Match3.Scripts1.Shared.UI;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.Signals;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Match3.Scripts1.Puzzletown.UI
{
	// Token: 0x0200086D RID: 2157
	[LoadOptions(true, false, false)]
	public class PopupFreeGiftRoot : APtSceneRoot<Gift>, IHandler<PopupOperation>, IDisposableDialog
	{
		// Token: 0x06003534 RID: 13620 RVA: 0x000FF724 File Offset: 0x000FDB24
		public static IEnumerator TestAllGifts(ConfigService configService)
		{
			foreach (Gift gift in configService.SbsConfig.giftconfig.free_gifts)
			{
				Wooroutine<PopupFreeGiftRoot> scene = SceneManager.Instance.LoadSceneWithParams<PopupFreeGiftRoot, Gift>(gift, null);
				yield return scene;
				yield return scene.ReturnValue.onClose;
				yield return new WaitForSeconds(2f);
			}
			yield break;
		}

		// Token: 0x06003535 RID: 13621 RVA: 0x000FF740 File Offset: 0x000FDB40
		protected override void Go()
		{
			this.dialog.Show();
			BackButtonManager.Instance.AddAction(new Action(this.Close));
			this.audioService.PlaySFX(AudioId.PopupShowDefault, false, false, false);
			this.amountLabel.text = this.parameters.gift_amount.ToString();
			this.prizeImage.sprite = this.rewardSprites.GetSimilar(this.parameters.gift_type);
			this.bodyTextLabel.text = this.localizationService.GetText(this.parameters.body_textkey, new LocaParam[0]);
			if (!string.IsNullOrEmpty(this.parameters.id))
			{
				base.StartCoroutine(this.ExecuteGiftRoutine());
			}
			else
			{
				base.StartCoroutine(this.WaitForCloseButtonRoutine());
			}
		}

		// Token: 0x06003536 RID: 13622 RVA: 0x000FF820 File Offset: 0x000FDC20
		private IEnumerator WaitForCloseButtonRoutine()
		{
			yield return this.onCloseButtonPressed;
			this.Close();
			yield break;
		}

		// Token: 0x06003537 RID: 13623 RVA: 0x000FF83C File Offset: 0x000FDC3C
		private IEnumerator ExecuteGiftRoutine()
		{
			yield return new WaitForSeconds(this.dialog.open.length);
			MaterialAmount giftAmount = new MaterialAmount(this.parameters.gift_type, this.parameters.gift_amount, MaterialAmountUsage.Undefined, 0);
			if (this.parameters.gift_type == "lives_unlimited")
			{
				this.livesService.StartUnlimitedLives(this.parameters.gift_amount);
				yield return this.onCloseButtonPressed;
			}
			else
			{
				yield return this.onCloseButtonPressed;
				this.gameStateService.Resources.AddMaterial(giftAmount, true);
				this.townUI.CollectMaterials(giftAmount, this.prizeImage.gameObject.transform, true);
				while (Doober.ActiveDoobers > 0)
				{
					yield return null;
				}
			}
			this.gameStateService.SetSeenFlag(this.parameters.id);
			this.Close();
			yield break;
		}

		// Token: 0x06003538 RID: 13624 RVA: 0x000FF858 File Offset: 0x000FDC58
		public void Close()
		{
			this.dialog.Hide();
			BackButtonManager.Instance.RemoveAction(new Action(this.Close));
			this.audioService.PlaySFX(AudioId.PopupHideDefault, false, false, false);
			this.onClose.Dispatch();
		}

		// Token: 0x06003539 RID: 13625 RVA: 0x000FF8A8 File Offset: 0x000FDCA8
		public void Handle(PopupOperation evt)
		{
			if (evt == PopupOperation.Close || evt == PopupOperation.OK)
			{
				this.TrackFreeGift(this.parameters.gift_type, this.parameters.gift_amount);
				this.onCloseButtonPressed.Dispatch();
			}
		}

		// Token: 0x0600353A RID: 13626 RVA: 0x000FF8F4 File Offset: 0x000FDCF4
		private void TrackFreeGift(string rewardType, int amount)
		{
			string purchaseResourceKey = TrackingService.GetPurchaseResourceKey(rewardType);
			this.trackingService.TrackFreeGifts(purchaseResourceKey, amount, this.parameters.gift_link_id);
		}

		// Token: 0x04005D1F RID: 23839
		[WaitForService(true, true)]
		private GameStateService gameStateService;

		// Token: 0x04005D20 RID: 23840
		[WaitForService(true, true)]
		private TrackingService trackingService;

		// Token: 0x04005D21 RID: 23841
		[WaitForService(true, true)]
		private AudioService audioService;

		// Token: 0x04005D22 RID: 23842
		[WaitForService(true, true)]
		private ILocalizationService localizationService;

		// Token: 0x04005D23 RID: 23843
		[WaitForService(true, true)]
		private LivesService livesService;

		// Token: 0x04005D24 RID: 23844
		[WaitForRoot(false, false)]
		private TownUiRoot townUI;

		// Token: 0x04005D25 RID: 23845
		[SerializeField]
		private Image prizeImage;

		// Token: 0x04005D26 RID: 23846
		[SerializeField]
		private TextMeshProUGUI amountLabel;

		// Token: 0x04005D27 RID: 23847
		[SerializeField]
		private TextMeshProUGUI bodyTextLabel;

		// Token: 0x04005D28 RID: 23848
		[SerializeField]
		private AnimatedUi dialog;

		// Token: 0x04005D29 RID: 23849
		[SerializeField]
		private SpriteManager rewardSprites;

		// Token: 0x04005D2A RID: 23850
		public readonly AwaitSignal onClose = new AwaitSignal();

		// Token: 0x04005D2B RID: 23851
		private readonly AwaitSignal onCloseButtonPressed = new AwaitSignal();

		// Token: 0x0200086E RID: 2158
		public class Trigger : PopupManager.Trigger
		{
			// Token: 0x0600353B RID: 13627 RVA: 0x000FF920 File Offset: 0x000FDD20
			public Trigger(ConfigService configService, ILocalizationService localizationService, GameStateService gameStateService, TrackingService trackingService, GiftLinksService giftLinksService, int sessionCount, int highestUnlockedLevel)
			{
				this.configService = configService;
				this.localizationService = localizationService;
				this.trackingService = trackingService;
				this.giftLinksService = giftLinksService;
				this.sessionCount = sessionCount;
				this.highestUnlockedLevel = highestUnlockedLevel;
				this.gameStateService = gameStateService;
				this.gifts = new List<Gift>();
			}

			// Token: 0x0600353C RID: 13628 RVA: 0x000FF974 File Offset: 0x000FDD74
			public override bool ShouldTrigger()
			{
				bool flag = false;
				List<Gift> list = this.configService.SbsConfig.giftconfig.free_gifts.FindAll((Gift gift) => gift.trigger_detail == this.sessionCount);
				if (list.Count > 0)
				{
					Gift gift2 = list.Find((Gift gift) => this.highestUnlockedLevel >= gift.min_level && this.highestUnlockedLevel <= gift.max_level);
					if (gift2 != null && !this.gameStateService.GetSeenFlag(gift2.id))
					{
						gift2.valid = true;
						this.gifts.Add(gift2);
						flag = true;
					}
				}
				if (this.giftLinksService.ClaimableGifts.Count > 0)
				{
					this.gifts.AddRange(this.giftLinksService.ClaimableGifts);
					flag = true;
				}
				this.giftLinksService.AllowedToProcessGiftLinks = !flag;
				return flag;
			}

			// Token: 0x0600353D RID: 13629 RVA: 0x000FFA38 File Offset: 0x000FDE38
			public override IEnumerator Run()
			{
				foreach (Gift gift in this.gifts)
				{
					if (gift.valid)
					{
						Wooroutine<PopupFreeGiftRoot> popup = SceneManager.Instance.LoadSceneWithParams<PopupFreeGiftRoot, Gift>(gift, null);
						yield return popup;
						yield return popup.ReturnValue.onDestroyed;
					}
					else
					{
						this.trackingService.TrackUi("gift_link", gift.id, "open", gift.error_code, new object[0]);
						PopupSortingOrder topLayerSortingOrder = new PopupSortingOrder(UILayer.Top);
						yield return PopupDialogRoot.ShowOkDialog(this.localizationService.GetText("ui.gift_link.title", new LocaParam[0]), this.localizationService.GetText("ui.gift_link.body.invalid", new LocaParam[0]), null, topLayerSortingOrder);
					}
				}
				this.giftLinksService.ClaimableGifts.Clear();
				this.giftLinksService.AllowedToProcessGiftLinks = true;
				yield break;
			}

			// Token: 0x04005D2C RID: 23852
			private readonly ConfigService configService;

			// Token: 0x04005D2D RID: 23853
			private readonly ILocalizationService localizationService;

			// Token: 0x04005D2E RID: 23854
			private readonly TrackingService trackingService;

			// Token: 0x04005D2F RID: 23855
			private readonly GiftLinksService giftLinksService;

			// Token: 0x04005D30 RID: 23856
			private readonly GameStateService gameStateService;

			// Token: 0x04005D31 RID: 23857
			private readonly int sessionCount;

			// Token: 0x04005D32 RID: 23858
			private readonly int highestUnlockedLevel;

			// Token: 0x04005D33 RID: 23859
			private List<Gift> gifts;
		}
	}
}
