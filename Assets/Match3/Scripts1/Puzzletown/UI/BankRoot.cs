using System;
using System.Collections;
using AndroidTools.Tools;
using DG.Tweening;
using Match3.Scripts1.Audio;
using Match3.Scripts1.Puzzletown.Config;
using Match3.Scripts1.Puzzletown.Services;
using Wooga.Coroutines;
using Wooga.UnityFramework;
using Match3.Scripts2.Android.DataStatistics;
using Match3.Scripts2.PlayerData;
using Match3.Scripts2.Shop;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI; // using Firebase.Analytics;

namespace Match3.Scripts1.Puzzletown.UI
{
	// Token: 0x02000979 RID: 2425
	[LoadOptions(true, true, false)]
	public class BankRoot : APtSceneRoot<BankRoot.BankContext>, IDisposableDialog
	{
		// Token: 0x170008FE RID: 2302
		// (get) Token: 0x06003B21 RID: 15137 RVA: 0x00124B39 File Offset: 0x00122F39
		// (set) Token: 0x06003B22 RID: 15138 RVA: 0x00124B4B File Offset: 0x00122F4B
		private int LastSeenDiamonds
		{
			get
			{
				return this.gameStateService.Bank.NumberOfDiamondsLastSeen;
			}
			set
			{
				this.gameStateService.Bank.NumberOfDiamondsLastSeen = value;
			}
		}

		// Token: 0x170008FF RID: 2303
		// (get) Token: 0x06003B23 RID: 15139 RVA: 0x00124B5E File Offset: 0x00122F5E
		// (set) Token: 0x06003B24 RID: 15140 RVA: 0x00124B70 File Offset: 0x00122F70
		private int BankedDiamonds
		{
			get
			{
				return this.gameStateService.Bank.NumberOfBankedDiamonds;
			}
			set
			{
				this.gameStateService.Bank.NumberOfBankedDiamonds = value;
			}
		}

		// Token: 0x06003B25 RID: 15141 RVA: 0x00124B84 File Offset: 0x00122F84
		protected override void Go()
		{
			if (base.registeredFirst || this.parameters == BankRoot.BankContext.testing)
			{
				this.SetupTest();
			}
			for (int i = 0; i < this.configService.iapConfigDataList.iaps.Length; i++)
			{
				if (this.configService.iapConfigDataList.iaps[i].iap_name == "piggy_m")
				{
					this.iapData = this.configService.iapConfigDataList.iaps[i];
				}
			}
			// if (this.iapData.storeProduct != null)
			// {
				// this.priceLabel.text = this.iapData.storeProduct.metadata.localizedPriceString;
				this.priceLabel.text = "16.99";
				var productInfo = AndroidPay.GetProductByNumId(iapData.id);
				this.priceLabel.text = productInfo != null ? productInfo.price : "???";
			// }
			this.BankedDiamonds = Math.Min(this.BankedDiamonds, this.sbsService.SbsConfig.bank.balancing.full_amount);
			this.LastSeenDiamonds = Math.Min(this.LastSeenDiamonds, this.BankedDiamonds);
			this.thresholdLabel.text = this.sbsService.SbsConfig.bank.balancing.open_threshold.ToString();
			this.fullLabel.text = this.sbsService.SbsConfig.bank.balancing.full_amount.ToString();
			this.currentDiamondLabel.text = this.LastSeenDiamonds.ToString();
			float fillAmount = (float)this.LastSeenDiamonds / (float)this.sbsService.SbsConfig.bank.balancing.full_amount;
			base.StartCoroutine(this.SetFillAmount(fillAmount, this.LastSeenDiamonds.ToString(), this.LastSeenDiamonds, false));
			this.buyButton.interactable = (this.LastSeenDiamonds >= this.sbsService.SbsConfig.bank.balancing.open_threshold);
			this.closeButton.onClick.AddListener(new UnityAction(this.Close));
			this.closeButton.interactable = true;
			if (this.parameters != BankRoot.BankContext.testing)
			{
				this.buyButton.onClick.AddListener(new UnityAction(this.HandleBuy));
			}
			else
			{
				this.buyButton.onClick.AddListener(new UnityAction(this.Close));
			}
			bool active = this.bankService.EventABTestEnabled();
			if (this.bankService.EventABTestEnabled())
			{
				this.timer.SetTargetTime(this.gameStateService.Bank.EndTime, false, null);
			}
			this.timerContainer.gameObject.SetActive(active);
			this.dialog.Show();
			BackButtonManager.Instance.AddAction(new Action(this.Close));
			this.audioService.PlaySFX(AudioId.PopupShowDefault, false, false, false);
			base.StartCoroutine(this.AnimateProgressRoutine());
			if (this.parameters == BankRoot.BankContext.auto)
			{
				this.gameStateService.Bank.NextAutoShowTime = DateTime.UtcNow.AddHours((double)this.sbsService.SbsConfig.bank.balancing.hours_between_auto_show);
			}
			this.Track("open");
			this.gameStateService.SetSeenFlagWithTimestamp("bankSeen", DateTime.UtcNow);
		}

		// Token: 0x06003B26 RID: 15142 RVA: 0x00124EE4 File Offset: 0x001232E4
		private void Track(string action)
		{
			string trigger = this.parameters.ToString();
			this.trackingService.TrackBankEvent(action, this.BankedDiamonds, this.sbsService.SbsConfig.bank.balancing.full_amount, this.gameStateService.Bank.TotalNumberOfPigyBanksLifeTime, trigger, (!this.bankService.EventABTestEnabled()) ? null : this.gameStateService.Bank.EventData);
		}

		// Token: 0x06003B27 RID: 15143 RVA: 0x00124F68 File Offset: 0x00123368
		private IEnumerator AnimateProgressRoutine()
		{
			// buried point: 完成银行任务1
			if (LastSeenDiamonds < sbsService.SbsConfig.bank.balancing.open_threshold &&
			    BankedDiamonds >= sbsService.SbsConfig.bank.balancing.open_threshold)
			{
				DataStatistics.Instance.TriggerCompleteBankTask1();
			}
			// buried point: 完成银行任务2
			if (LastSeenDiamonds < sbsService.SbsConfig.bank.balancing.full_amount &&
			    BankedDiamonds >= sbsService.SbsConfig.bank.balancing.full_amount)
			{
				DataStatistics.Instance.TriggerCompleteBankTask2();
			}
			
			int animatedSeenDiamonds = this.LastSeenDiamonds;
			this.LastSeenDiamonds = this.BankedDiamonds;
			if (this.BankedDiamonds != animatedSeenDiamonds)
			{
				yield return new WaitForSeconds(0.5f);
				MaterialAmount amount = new MaterialAmount("diamonds", this.BankedDiamonds - animatedSeenDiamonds, MaterialAmountUsage.Undefined, 0);
				this.doobers.SpawnDoobers(amount, this.doobersStartLocation, this.doobersEndLocation, new Action<int>(this.PlayDooberSFX));
				base.StartCoroutine(this.AnimateChestShaking());
				yield return new WaitForSeconds(1.25f);
				this.audioService.PlaySFX(AudioId.AdWheelJackpotCharge, false, false, false);
			}
			float fillAmount = (float)this.BankedDiamonds / (float)this.sbsService.SbsConfig.bank.balancing.full_amount;
			yield return this.SetFillAmount(fillAmount, this.BankedDiamonds.ToString(), animatedSeenDiamonds, this.BankedDiamonds != animatedSeenDiamonds);
			bool isOverBuyThreshold = this.BankedDiamonds >= this.sbsService.SbsConfig.bank.balancing.open_threshold;
			this.buyButton.interactable = isOverBuyThreshold;
			this.thresholdShakeAnimation.enabled = isOverBuyThreshold;
			this.gameStateService.Save(false);
			yield break;
		}

		// Token: 0x06003B28 RID: 15144 RVA: 0x00124F83 File Offset: 0x00123383
		private void PlayDooberSFX(int amountOfDiamondsReceived)
		{
			this.audioService.PlaySFX(AudioId.DiamondsAcquired, false, false, false);
		}

		// Token: 0x06003B29 RID: 15145 RVA: 0x00124F9C File Offset: 0x0012339C
		private IEnumerator SetFillAmount(float fillAmount, string fillLabelAmount, int animatedSeenDiamonds, bool animate = true)
		{
			if (animate)
			{
				float previousFillAmount = this.fillImage.fillAmount;
				this.fillImage.DOFillAmount(fillAmount, 1f);
				this.sparkles.DOAnchorMin(new Vector2(this.sparkles.anchorMin.x, fillAmount), 1f, false);
				this.sparkles.DOAnchorMax(new Vector2(this.sparkles.anchorMax.x, fillAmount), 1f, false);
				this.amountLabelRect.DOAnchorMin(new Vector2(this.amountLabelRect.anchorMin.x, fillAmount), 1f, false);
				this.amountLabelRect.DOAnchorMax(new Vector2(this.amountLabelRect.anchorMax.x, fillAmount), 1f, false);
				base.StartCoroutine(this.AnimateTextFieldCounter(this.currentDiamondLabel, animatedSeenDiamonds, this.BankedDiamonds, 1f));
				if (fillAmount >= 0.5f)
				{
					base.StartCoroutine(this.AnimateDiamondPile(previousFillAmount, fillAmount));
					yield return new WaitForSeconds(1f);
					if (animatedSeenDiamonds < this.sbsService.SbsConfig.bank.balancing.open_threshold)
					{
						yield return this.WaitForChestOpenAnimationRoutine();
					}
				}
				else
				{
					yield return new WaitForSeconds(1f);
				}
			}
			else
			{
				this.fillImage.fillAmount = fillAmount;
				this.sparkles.anchorMin = new Vector2(this.sparkles.anchorMin.x, fillAmount);
				this.sparkles.anchorMax = new Vector2(this.sparkles.anchorMax.x, fillAmount);
				this.amountLabelRect.anchorMin = new Vector2(this.sparkles.anchorMin.x, fillAmount);
				this.amountLabelRect.anchorMax = new Vector2(this.sparkles.anchorMax.x, fillAmount);
				this.currentDiamondLabel.text = fillLabelAmount;
				if (fillAmount >= 0.5f)
				{
					this.chestAnimation.Play("BankChestOpenedState");
					this.diamondPileAnimation.Play("BankChestDiamondsIncreasing");
					yield return null;
					this.diamondPileAnimation["BankChestDiamondsIncreasing"].time = 2f * fillAmount - 1f;
					this.diamondPileAnimation["BankChestDiamondsIncreasing"].speed = 0f;
					this.diamondPileAnimation.Play("BankChestDiamondsIncreasing");
				}
			}
			yield break;
		}

		// Token: 0x06003B2A RID: 15146 RVA: 0x00124FD4 File Offset: 0x001233D4
		private IEnumerator WaitForChestOpenAnimationRoutine()
		{
			this.chestAnimation.Play("BankChestOpening");
			this.audioService.PlaySFX(AudioId.TreasureChestKeyReleased, false, false, false);
			float openAnimationLength = this.chestAnimation["BankChestOpening"].length;
			yield return new WaitForSeconds(openAnimationLength - 1f);
			this.audioService.PlaySFX(AudioId.TreasureChestOpen, false, false, false);
			yield return new WaitForSeconds(1f);
			yield break;
		}

		// Token: 0x06003B2B RID: 15147 RVA: 0x00124FF0 File Offset: 0x001233F0
		private IEnumerator AnimateTextFieldCounter(TextMeshProUGUI textLabel, int startValue, int targetValue, float animationTime)
		{
			float elapsedTime = 0f;
			float ratio = 0f;
			while (ratio < 1f)
			{
				elapsedTime += Time.deltaTime;
				ratio = elapsedTime / animationTime;
				textLabel.text = (startValue + (int)((float)(targetValue - startValue) * ratio)).ToString();
				yield return null;
			}
			textLabel.text = targetValue.ToString();
			yield break;
		}

		// Token: 0x06003B2C RID: 15148 RVA: 0x00125024 File Offset: 0x00123424
		private IEnumerator AnimateDiamondPile(float startFill, float targetFill)
		{
			float startAdjusted = 2f * startFill - 1f;
			float targetAdjusted = 2f * targetFill - 1f;
			this.diamondPileAnimation["BankChestDiamondsIncreasing"].time = startAdjusted;
			this.diamondPileAnimation["BankChestDiamondsIncreasing"].speed = 0.25f;
			this.diamondPileAnimation.Play("BankChestDiamondsIncreasing");
			while (this.diamondPileAnimation["BankChestDiamondsIncreasing"].time < targetAdjusted)
			{
				yield return null;
			}
			this.diamondPileAnimation["BankChestDiamondsIncreasing"].time = targetAdjusted;
			this.diamondPileAnimation["BankChestDiamondsIncreasing"].speed = 0f;
			this.diamondPileAnimation.Play("BankChestDiamondsIncreasing");
			yield return null;
			yield break;
		}

		// Token: 0x06003B2D RID: 15149 RVA: 0x00125050 File Offset: 0x00123450
		private IEnumerator AnimateChestShaking()
		{
			yield return new WaitForSeconds(1.25f);
			this.chestShakeAnimation.wrapMode = WrapMode.Loop;
			this.chestShakeAnimation.Play("BankChestShaking");
			yield return new WaitForSeconds(1.5f);
			this.chestShakeAnimation.wrapMode = WrapMode.ClampForever;
			yield break;
		}

		// Token: 0x06003B2E RID: 15150 RVA: 0x0012506C File Offset: 0x0012346C
		private void SetupTest()
		{
			this.LastSeenDiamonds = (int)((float)this.sbsService.SbsConfig.bank.balancing.full_amount * this.previousAmount);
			this.BankedDiamonds = (int)((float)this.sbsService.SbsConfig.bank.balancing.full_amount * this.targetAmount);
		}

		// Token: 0x06003B2F RID: 15151 RVA: 0x001250CB File Offset: 0x001234CB
		private void HandleBuy()
		{
			base.StartCoroutine(this.HandleBuyRoutine());
		}

		// Token: 0x06003B30 RID: 15152 RVA: 0x001250DC File Offset: 0x001234DC
		private IEnumerator HandleBuyRoutine()
		{
			this.buyButton.interactable = false;
			BackButtonManager.Instance.RemoveAction(new Action(this.Close));
			this.closeButton.interactable = false;
			this.Track("claim");
			TownDiamondsPanelRoot.TownDiamondsPanelRootParameters context = new TownDiamondsPanelRoot.TownDiamondsPanelRootParameters();
			context.source1 = "piggy_bank";
			context.source2 = this.parameters.ToString();
			if (this.bankService.EventABTestEnabled())
			{
				context.eventType = WeeklyEventType.Bank;
				context.eventData = this.gameStateService.Bank.EventData;
			}
			this.iapData.context = "piggy_bank";
			this.diamondsPanelRoot.context = context;
			Wooroutine<PurchaseResult> purchaseRoutine = this.diamondsPanelRoot.TryPurchaseWithResult(this.iapData);
			yield return purchaseRoutine;
			PurchaseResult purchaseResult = purchaseRoutine.ReturnValue;
			if (purchaseResult.success)
			{
				// buried point: 购买银行宝箱
				DataStatistics.Instance.TriggerBuyBankChest(LastSeenDiamonds);
				this.gameStateService.Bank.OnBuy();
				this.gameStateService.Save(false);
				this.Close();
			}
			else
			{
				this.buyButton.interactable = (this.BankedDiamonds >= this.sbsService.SbsConfig.bank.balancing.open_threshold);
				this.closeButton.interactable = true;
				BackButtonManager.Instance.AddAction(new Action(this.Close));
			}
		}

		// Token: 0x06003B31 RID: 15153 RVA: 0x001250F7 File Offset: 0x001234F7
		private void Close()
		{
			this.audioService.PlaySFX(AudioId.PopupHideDefault, false, false, false);
			BackButtonManager.Instance.RemoveAction(new Action(this.Close));
			this.dialog.Hide();
		}

		// Token: 0x040062FF RID: 25343
		public const string IAP_NAME = "piggy_m";

		// Token: 0x04006300 RID: 25344
		public const string IAP_CONTENTS = "piggy_open_items";

		// Token: 0x04006301 RID: 25345
		public const string BANK_SEEN_FLAG_NAME = "bankSeen";

		// Token: 0x04006302 RID: 25346
		private const string BANK_WINDOW = "piggy_bank";

		// Token: 0x04006303 RID: 25347
		private const string OPEN_ANIMATION_NAME = "BankChestOpening";

		// Token: 0x04006304 RID: 25348
		private const string OPEN_STATE_ANIMATION_NAME = "BankChestOpenedState";

		// Token: 0x04006305 RID: 25349
		private const string DIAMONDS_INCREASING_ANIMATION_NAME = "BankChestDiamondsIncreasing";

		// Token: 0x04006306 RID: 25350
		private const string CHEST_SHAKING_ANIMATION_NAME = "BankChestShaking";

		// Token: 0x04006307 RID: 25351
		private const float OPENING_WAIT_TIME = 0.5f;

		// Token: 0x04006308 RID: 25352
		private const float DOOBER_DELAY = 1.25f;

		// Token: 0x04006309 RID: 25353
		private const float SLIDER_ANIMATION_TIME = 1f;

		// Token: 0x0400630A RID: 25354
		private const float CHEST_SHAKE_DELAY = 1.25f;

		// Token: 0x0400630B RID: 25355
		private const float CHEST_SHAKE_DURATION = 1.5f;

		// Token: 0x0400630C RID: 25356
		private const float OPEN_SFX_EFFECTIVE_DURATION = 1f;

		// Token: 0x0400630D RID: 25357
		[WaitForService(true, true)]
		private AudioService audioService;

		// Token: 0x0400630E RID: 25358
		[WaitForService(true, true)]
		private GameStateService gameStateService;

		// Token: 0x0400630F RID: 25359
		[WaitForService(true, true)]
		private SBSService sbsService;

		// Token: 0x04006310 RID: 25360
		[WaitForService(true, true)]
		private ConfigService configService;

		// Token: 0x04006311 RID: 25361
		[WaitForService(true, true)]
		private BankService bankService;

		// Token: 0x04006312 RID: 25362
		[WaitForService(true, true)]
		private TrackingService trackingService;

		// Token: 0x04006313 RID: 25363
		[WaitForRoot(false, false)]
		private TownDiamondsPanelRoot diamondsPanelRoot;

		// Token: 0x04006314 RID: 25364
		[WaitForRoot(false, false)]
		private DoobersRoot doobers;

		// Token: 0x04006315 RID: 25365
		[SerializeField]
		private AnimatedUi dialog;

		// Token: 0x04006316 RID: 25366
		[SerializeField]
		private Button closeButton;

		// Token: 0x04006317 RID: 25367
		[SerializeField]
		private Button buyButton;

		// Token: 0x04006318 RID: 25368
		[SerializeField]
		private TextMeshProUGUI currentDiamondLabel;

		// Token: 0x04006319 RID: 25369
		[SerializeField]
		private TextMeshProUGUI thresholdLabel;

		// Token: 0x0400631A RID: 25370
		[SerializeField]
		private TextMeshProUGUI fullLabel;

		// Token: 0x0400631B RID: 25371
		[SerializeField]
		private TextMeshProUGUI priceLabel;

		// Token: 0x0400631C RID: 25372
		[SerializeField]
		private Transform doobersStartLocation;

		// Token: 0x0400631D RID: 25373
		[SerializeField]
		private Transform doobersEndLocation;

		// Token: 0x0400631E RID: 25374
		[SerializeField]
		private Image fillImage;

		// Token: 0x0400631F RID: 25375
		[SerializeField]
		private RectTransform sparkles;

		// Token: 0x04006320 RID: 25376
		[SerializeField]
		private RectTransform amountLabelRect;

		// Token: 0x04006321 RID: 25377
		[SerializeField]
		private Animation chestAnimation;

		// Token: 0x04006322 RID: 25378
		[SerializeField]
		private Animation diamondPileAnimation;

		// Token: 0x04006323 RID: 25379
		[SerializeField]
		private Animation chestShakeAnimation;

		// Token: 0x04006324 RID: 25380
		[SerializeField]
		private Animation thresholdShakeAnimation;

		// Token: 0x04006325 RID: 25381
		[SerializeField]
		private CountdownTimer timer;

		// Token: 0x04006326 RID: 25382
		[SerializeField]
		private GameObject timerContainer;

		// Token: 0x04006327 RID: 25383
		[Header("Testing")]
		[Range(0f, 1f)]
		[SerializeField]
		private float previousAmount;

		// Token: 0x04006328 RID: 25384
		[Range(0f, 1f)]
		[SerializeField]
		private float targetAmount;

		// Token: 0x04006329 RID: 25385
		private IAPData iapData;

		// Token: 0x0200097A RID: 2426
		public class Trigger : PopupManager.Trigger
		{
			// Token: 0x06003B32 RID: 15154 RVA: 0x0012512E File Offset: 0x0012352E
			public Trigger(ProgressionDataService.Service progressionService, SBSService sbsService, GameStateService gameStateService, BankService bankService, int numberOfPopupRuns)
			{
				this.progressionService = progressionService;
				this.sbsService = sbsService;
				this.numberOfPopupRuns = numberOfPopupRuns;
				this.gameStateService = gameStateService;
				this.bankService = bankService;
			}

			// Token: 0x06003B33 RID: 15155 RVA: 0x0012515C File Offset: 0x0012355C
			public override bool ShouldTrigger()
			{
				return this.numberOfPopupRuns > 1 && this.bankService.BankFeatureEnabled() && Application.internetReachability != NetworkReachability.NotReachable && DateTime.UtcNow > this.gameStateService.Bank.NextAutoShowTime && this.gameStateService.Bank.NumberOfBankedDiamonds >= this.sbsService.SbsConfig.bank.balancing.open_threshold && this.progressionService.UnlockedLevel >= this.sbsService.SbsConfig.bank.balancing.unlock_level;
			}

			// Token: 0x06003B34 RID: 15156 RVA: 0x0012520C File Offset: 0x0012360C
			public override IEnumerator Run()
			{
				Wooroutine<BankRoot> scene = SceneManager.Instance.LoadSceneWithParams<BankRoot, BankRoot.BankContext>(BankRoot.BankContext.auto, null);
				yield return scene;
				yield return scene.ReturnValue.onDestroyed;
				yield break;
			}

			// Token: 0x0400632A RID: 25386
			private ProgressionDataService.Service progressionService;

			// Token: 0x0400632B RID: 25387
			private SBSService sbsService;

			// Token: 0x0400632C RID: 25388
			private GameStateService gameStateService;

			// Token: 0x0400632D RID: 25389
			private BankService bankService;

			// Token: 0x0400632E RID: 25390
			private int numberOfPopupRuns;
		}

		// Token: 0x0200097B RID: 2427
		public enum BankContext
		{
			// Token: 0x04006330 RID: 25392
			island,
			// Token: 0x04006331 RID: 25393
			auto,
			// Token: 0x04006332 RID: 25394
			testing
		}
	}
}
