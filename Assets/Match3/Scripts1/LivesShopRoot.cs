using System;
using System.Collections;
using Match3.Scripts1.Audio;
using Match3.Scripts1.Puzzletown.Config;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Puzzletown.Town;
using Match3.Scripts1.Puzzletown.UI;
using Match3.Scripts1.Shared.Flows;
using Match3.Scripts1.UnityEngine;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.Signals;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;
using Match3.Scripts2.Shop;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020008A4 RID: 2212
namespace Match3.Scripts1
{
	public class LivesShopRoot : APtSceneRoot<TrackingService.PurchaseFlowContext>, IPersistentDialog, IHandler<LivesShopOperation, Button>, IHandler<PopupOperation>
	{
		// Token: 0x17000851 RID: 2129
		// (get) Token: 0x0600360A RID: 13834 RVA: 0x001052D4 File Offset: 0x001036D4
		protected override bool IsSetup
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600360B RID: 13835 RVA: 0x001052D7 File Offset: 0x001036D7
		private static string FormatTime(TimeSpan time)
		{
			return string.Format("{0:D2}:{1:D2}", time.Minutes, time.Seconds);
		}

		// Token: 0x0600360C RID: 13836 RVA: 0x001052FB File Offset: 0x001036FB
		protected override void Awake()
		{
			base.Awake();
			base.gameObject.SetActive(false);
		}

		// Token: 0x0600360D RID: 13837 RVA: 0x0010530F File Offset: 0x0010370F
		protected override void Go()
		{
			this.livesService.OnLifeTimerChanged.AddListener(new Action(this.UpdateLifeTimer));
			this.livesService.OnLivesChanged.AddListener(new Action(this.RefreshUI));
			this.RefreshUI();
		}

		// Token: 0x0600360E RID: 13838 RVA: 0x00105350 File Offset: 0x00103750
		protected override void OnDestroy()
		{
			base.OnDestroy();
			if (this.livesService != null)
			{
				this.livesService.OnLifeTimerChanged.RemoveListener(new Action(this.UpdateLifeTimer));
				this.livesService.OnLivesChanged.RemoveListener(new Action(this.RefreshUI));
			}
		}

		// Token: 0x0600360F RID: 13839 RVA: 0x001053A8 File Offset: 0x001037A8
		private void UpdateLifeTimer()
		{
			int num = Mathf.Max(0, this.livesService.SecondsRemaining);
			bool flag = num <= 0;
			this.heartIcon.SetActive(!flag);
			if (flag)
			{
				// 向好友索取生命, 改为"已满"
				this.NextLifeTimerText.text = this.localizationService.GetText("ui.dialog.lives.ask_friends", new LocaParam[0]);
			}
			else
			{
				this.NextLifeTimerText.text = LivesShopRoot.FormatTime(TimeSpan.FromSeconds((double)num));
			}
		}

		// Token: 0x06003610 RID: 13840 RVA: 0x00105424 File Offset: 0x00103824
		private IEnumerator StartJourneyRoutine(AFlow newJourney, Action onComplete = null)
		{
			yield return base.StartCoroutine(this.RunJourneyRoutine(newJourney, onComplete));
			yield break;
		}

		// Token: 0x06003611 RID: 13841 RVA: 0x00105450 File Offset: 0x00103850
		private IEnumerator RunJourneyRoutine(AFlow newJourney, Action onComplete)
		{
			yield return newJourney.Start();
			if (onComplete != null)
			{
				onComplete();
			}
			yield break;
		}

		// Token: 0x06003612 RID: 13842 RVA: 0x00105474 File Offset: 0x00103874
		private IEnumerator PayWithDiamondsRoutine(Button button)
		{
			int refillCost = this.configService.general.lives.diamonds_cost_to_refill;
			if (this.gameStateService.Resources.GetAmount("diamonds") < refillCost)
			{
				yield return new PurchaseDiamondsJourney(this._diamondsPurchaseContext, true).Start();
				if (this.gameStateService.Resources.GetAmount("diamonds") >= refillCost)
				{
					yield return this.CompleteDiamondsPurchase(button, refillCost);
				}
			}
			else
			{
				yield return this.CompleteDiamondsPurchase(button, refillCost);
			}
		}

		// Token: 0x06003613 RID: 13843 RVA: 0x00105496 File Offset: 0x00103896
		private Coroutine CompleteDiamondsPurchase(Button button, int refillCost)
		{
			return WooroutineRunner.StartCoroutine(this.CompleteDiamondsPurchaseRoutine(button, refillCost), null);
		}

		// Token: 0x06003614 RID: 13844 RVA: 0x001054A8 File Offset: 0x001038A8
		private IEnumerator CompleteDiamondsPurchaseRoutine(Button button, int refillCost)
		{
			// 爱心飞的动画
			// yield return new WaitForSeconds(this.resourcePanel.CollectMaterials(new MaterialAmount("lives", this.livesService.MaxLives, MaterialAmountUsage.Undefined, 0), button.transform, true));
			this.gameStateService.Resources.Pay(new MaterialAmount[]
			{
				new MaterialAmount("diamonds", refillCost, MaterialAmountUsage.Undefined, 0)
			});
			this.livesService.Refill();
			this.trackingService.TrackPurchase(this.purchaseContext, 0, refillCost);
			this.Close();
			yield return this.ShowPurchasedPopup(new MaterialAmount("lives", this.livesService.CurrentLives, MaterialAmountUsage.Undefined, 0));
			this.onPurchaseCompleted.Dispatch(true);
			yield break;
		}

		// Token: 0x06003615 RID: 13845 RVA: 0x001054D4 File Offset: 0x001038D4
		private IEnumerator ShowPurchasedPopup(MaterialAmount amount)
		{
			Wooroutine<PopupLivesPurchasedRoot> scene = SceneManager.Instance.LoadSceneWithParams<PopupLivesPurchasedRoot, MaterialAmount>(amount, null);
			yield return scene;
			yield return scene.ReturnValue.onDisabled.Await();
			yield break;
		}

		// Token: 0x06003616 RID: 13846 RVA: 0x001054F0 File Offset: 0x001038F0
		protected override void OnEnable()
		{
			base.OnEnable();
			this.dialog.Show();
			BackButtonManager.Instance.AddAction(new Action(this.CloseViaBackButton));
			this.audioService.PlaySFX(AudioId.PopupShowDefault, false, false, false);
			this.RefreshUI();
		}

		// Token: 0x06003617 RID: 13847 RVA: 0x0010553E File Offset: 0x0010393E
		private void CloseViaBackButton()
		{
			this.Handle(PopupOperation.Close);
		}

		// Token: 0x06003618 RID: 13848 RVA: 0x00105547 File Offset: 0x00103947
		private void Close()
		{
			this.dialog.Hide();
			BackButtonManager.Instance.RemoveAction(new Action(this.CloseViaBackButton));
			this.audioService.PlaySFX(AudioId.PopupHideDefault, false, false, false);
		}

		// Token: 0x06003619 RID: 13849 RVA: 0x00105580 File Offset: 0x00103980
		private void RefreshUI()
		{
			LivesConfig lives = this.configService.general.lives;
			int currentLives = this.livesService.CurrentLives;
			LivesShopOperation livesShopOperation = this.operationToTest;
			if (!this.isTesting)
			{
				if (currentLives <= lives.ask_friend_thresh)
				{
					// 隐藏askFriends按钮
					// livesShopOperation |= LivesShopOperation.AskFriends;
				}
				if (this.ShouldVideoButtonBeVisible(lives, currentLives))
				{
					livesShopOperation |= LivesShopOperation.WatchVideo;
				}
				if (currentLives <= lives.diamonds_purchase_thresh)
				{
					livesShopOperation |= LivesShopOperation.SpendDiamonds;
				}
			}
			this.ShowOnChildren(livesShopOperation, true, true);
			bool flag = (!this.isTesting) ? this.ShouldVideoButtonBeEnabled() : (livesShopOperation == LivesShopOperation.WatchVideo);
			this.watchVideoButtonText.text = ((!flag) ? this.localizationService.GetText("ui.lives.watch_ad.no_ads_available", new LocaParam[0]) : this.localizationService.GetText("ui.shared.watch_video", new LocaParam[0]));
			this.watchVideoButton.interactable = flag;
			int num = Mathf.Max(0, lives.max_lives - currentLives);
			this.BigLivesText.text = currentLives.ToString();
			this.FullRefilAmountText.text = "+" + num;
			this.RefillDiamondCost.text = lives.diamonds_cost_to_refill.ToString();
			this.UpdateLifeTimer();
		}

		// Token: 0x0600361A RID: 13850 RVA: 0x001056C5 File Offset: 0x00103AC5
		private bool ShouldVideoButtonBeVisible(LivesConfig livesConfig, int currentLives)
		{
			return this.videoAdService.HasUnlockedVideoAd && currentLives <= livesConfig.watch_video_thresh && this.configService.FeatureSwitchesConfig.ads_out_of_lives;
		}

		// Token: 0x0600361B RID: 13851 RVA: 0x001056F6 File Offset: 0x00103AF6
		private bool ShouldVideoButtonBeEnabled()
		{
			return this.videoAdService.IsVideoAvailable(true) && Application.internetReachability != NetworkReachability.NotReachable && this.videoAdService.IsAllowedToWatchAd(AdPlacement.LivesShop);
		}

		// Token: 0x0600361C RID: 13852 RVA: 0x00105722 File Offset: 0x00103B22
		public Coroutine TryToBuy(string moreLivesTrigger, TownDiamondsPanelRoot.TownDiamondsPanelRootParameters diamondsPurchaseContext)
		{
			return WooroutineRunner.StartCoroutine(this.TryToBuyRoutine(moreLivesTrigger, diamondsPurchaseContext), null);
		}

		// Token: 0x0600361D RID: 13853 RVA: 0x00105734 File Offset: 0x00103B34
		public IEnumerator TestLayoutRoutine(LivesShopOperation opToTest)
		{
			base.Enable();
			base.gameObject.SetActive(true);
			this.isTesting = true;
			this.operationToTest = opToTest;
			this.RefreshUI();
			yield return this.onPurchaseCompleted.Await<bool>();
			this.operationToTest = LivesShopOperation.Undefined;
			this.isTesting = false;
			yield break;
		}

		// Token: 0x0600361E RID: 13854 RVA: 0x00105758 File Offset: 0x00103B58
		private IEnumerator TryToBuyRoutine(string moreLivesTrigger, TownDiamondsPanelRoot.TownDiamondsPanelRootParameters diamondsPurchaseContext)
		{
			this._diamondsPurchaseContext = diamondsPurchaseContext;
			this.purchaseContext = new TrackingService.PurchaseFlowContext
			{
				det1 = "lives",
				det2 = moreLivesTrigger
			};
			base.Enable();
			base.gameObject.SetActive(true);
			this.RefreshUI();
			yield return this.onPurchaseCompleted.Await<bool>();
			yield break;
		}

		// Token: 0x0600361F RID: 13855 RVA: 0x00105784 File Offset: 0x00103B84
		private IEnumerator WatchVideoRoutine()
		{
			this.Close();
			Coroutine watchVideoFlow = new WatchVideoFlow().Start();
			yield return watchVideoFlow;
			this.onPurchaseCompleted.Dispatch(false);
			yield break;
		}

		// Token: 0x06003620 RID: 13856 RVA: 0x0010579F File Offset: 0x00103B9F
		public void Handle(PopupOperation op)
		{
			if (op == PopupOperation.Close)
			{
				this.Close();
				this.onPurchaseCompleted.Dispatch(false);
			}
		}

		// Token: 0x06003621 RID: 13857 RVA: 0x001057C4 File Offset: 0x00103BC4
		public void Handle(LivesShopOperation op, Button button)
		{
			if (!this.isTesting)
			{
				WooroutineRunner.StartCoroutine(this.HandleRoutine(op, button), null);
			}
			else
			{
				this.Handle(PopupOperation.Close);
			}
		}

		// Token: 0x06003622 RID: 13858 RVA: 0x001057EC File Offset: 0x00103BEC
		private IEnumerator HandleRoutine(LivesShopOperation op, Button button)
		{
			if (op != LivesShopOperation.AskFriends)
			{
				if (op != LivesShopOperation.SpendDiamonds)
				{
					if (op == LivesShopOperation.WatchVideo)
					{
						if (Application.internetReachability != NetworkReachability.NotReachable)
						{
							WooroutineRunner.StartCoroutine(this.WatchVideoRoutine(), null);
						}
						else
						{
							SceneManager.Instance.LoadSceneWithParams<PopupFacebookRoot, FacebookPopupParameters>(new FacebookPopupParameters(FacebookPopupState.NoConnection, FacebookLoginContext.settings), null);
						}
					}
				}
				else
				{
					yield return this.PayWithDiamondsRoutine(button);
				}
			}
			else if (Application.internetReachability != NetworkReachability.NotReachable)
			{
				yield return new AskFriendJourney().Start();
			}
			else
			{
				SceneManager.Instance.LoadSceneWithParams<PopupFacebookRoot, FacebookPopupParameters>(new FacebookPopupParameters(FacebookPopupState.NoConnection, FacebookLoginContext.settings), null);
			}
			yield break;
		}

		// Token: 0x04005E0B RID: 24075
		[WaitForService(true, true)]
		private ILocalizationService localizationService;

		// Token: 0x04005E0C RID: 24076
		[WaitForService(true, true)]
		private LivesService livesService;

		// Token: 0x04005E0D RID: 24077
		[WaitForService(true, true)]
		private IVideoAdService videoAdService;

		// Token: 0x04005E0E RID: 24078
		[SerializeField]
		private GameObject LivesShopPanel;

		// Token: 0x04005E0F RID: 24079
		[SerializeField]
		private GameObject AskFriendBubble;

		// Token: 0x04005E10 RID: 24080
		[SerializeField]
		private TextMeshProUGUI BigLivesText;

		// Token: 0x04005E11 RID: 24081
		[WaitForService(true, true)]
		private GameStateService gameStateService;

		// Token: 0x04005E12 RID: 24082
		[WaitForService(true, true)]
		private ConfigService configService;

		// Token: 0x04005E13 RID: 24083
		[WaitForService(true, true)]
		private TrackingService trackingService;

		// Token: 0x04005E14 RID: 24084
		[WaitForService(true, true)]
		private AudioService audioService;

		// Token: 0x04005E15 RID: 24085
		[WaitForRoot(false, false)]
		private TownResourcePanelRoot resourcePanel;

		// Token: 0x04005E16 RID: 24086
		[SerializeField]
		private TextMeshProUGUI FullRefilAmountText;

		// Token: 0x04005E17 RID: 24087
		[SerializeField]
		private TextMeshProUGUI RefillDiamondCost;

		// Token: 0x04005E18 RID: 24088
		[SerializeField]
		private TextMeshProUGUI NextLifeTimerText;

		// Token: 0x04005E19 RID: 24089
		[SerializeField]
		private GameObject heartIcon;

		// Token: 0x04005E1A RID: 24090
		[SerializeField]
		private Button watchVideoButton;

		// Token: 0x04005E1B RID: 24091
		[SerializeField]
		private TextMeshProUGUI watchVideoButtonText;

		// Token: 0x04005E1C RID: 24092
		[SerializeField]
		private GameObject RemainingTimeBubble;

		// Token: 0x04005E1D RID: 24093
		[SerializeField]
		private AnimatedUi dialog;

		// Token: 0x04005E1E RID: 24094
		public Signal<bool> onPurchaseCompleted = new Signal<bool>();

		// Token: 0x04005E1F RID: 24095
		private TrackingService.PurchaseFlowContext purchaseContext;

		// Token: 0x04005E20 RID: 24096
		private TownDiamondsPanelRoot.TownDiamondsPanelRootParameters _diamondsPurchaseContext;

		// Token: 0x04005E21 RID: 24097
		private bool isTesting;

		// Token: 0x04005E22 RID: 24098
		private LivesShopOperation operationToTest;
	}
}
