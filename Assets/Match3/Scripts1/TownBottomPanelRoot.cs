using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Puzzletown.UI;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.Signals;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000981 RID: 2433
namespace Match3.Scripts1
{
	public class TownBottomPanelRoot : APtSceneRoot, IHandler<TownOptionsCommand>
	{
		// Token: 0x17000900 RID: 2304
		// (get) Token: 0x06003B3F RID: 15167 RVA: 0x0012633E File Offset: 0x0012473E
		// (set) Token: 0x06003B40 RID: 15168 RVA: 0x00126346 File Offset: 0x00124746
		public bool IsInteractable { get; set; }

		// Token: 0x17000901 RID: 2305
		// (get) Token: 0x06003B41 RID: 15169 RVA: 0x0012634F File Offset: 0x0012474F
		public bool IsStorageAvailable
		{
			get
			{
				return this.gameStateService.Progression.UnlockedLevel > 9;
			}
		}

		// Token: 0x17000902 RID: 2306
		// (get) Token: 0x06003B42 RID: 15170 RVA: 0x00126365 File Offset: 0x00124765
		// (set) Token: 0x06003B43 RID: 15171 RVA: 0x00126370 File Offset: 0x00124770
		public TownBottomPanelRoot.UIState State
		{
			get
			{
				return this._state;
			}
			set
			{
				this._state = value;
				this.animator.SetBool("ControlButtons", value == TownBottomPanelRoot.UIState.MovementMode);
				this.animator.SetBool("HUD", value == TownBottomPanelRoot.UIState.InGameUI);
				this.animator.SetBool("NewDecoration", this.animator.GetBool("NewDecoration") && value == TownBottomPanelRoot.UIState.InGameUI);
				if (this._state == TownBottomPanelRoot.UIState.MovementMode)
				{
					BackButtonManager.Instance.AddAction(new Action(this.controlButtons.HandleCancelViaBackButton));
				}
				else
				{
					BackButtonManager.Instance.RemoveAction(new Action(this.controlButtons.HandleCancelViaBackButton));
				}
			}
		}

		// Token: 0x06003B44 RID: 15172 RVA: 0x0012641F File Offset: 0x0012481F
		public void ResetBuildingControlsAnimator()
		{
			this.animator.SetBool("SkipTimerExpanded", false);
		}

		// Token: 0x06003B45 RID: 15173 RVA: 0x00126432 File Offset: 0x00124832
		private bool IsVisible()
		{
			return this.State != TownBottomPanelRoot.UIState.None;
		}

		// Token: 0x06003B46 RID: 15174 RVA: 0x00126440 File Offset: 0x00124840
		public void Show(bool setVisible)
		{
			if (setVisible == this.IsVisible())
			{
				return;
			}
			if (!setVisible)
			{
				this._oldState = this.State;
				this.State = TownBottomPanelRoot.UIState.None;
			}
			else
			{
				this.State = this._oldState;
				this.UpdateTournamentStatus();
			}
		}

		// Token: 0x06003B47 RID: 15175 RVA: 0x00126480 File Offset: 0x00124880
		public bool IsButtonExpanded(BuildingOperation op)
		{
			string param = op + "Expanded";
			return Array.Find<AnimatorControllerParameter>(this.animator.parameters, (AnimatorControllerParameter p) => p.name == param) == null || this.animator.GetBool(param);
		}

		// Token: 0x06003B48 RID: 15176 RVA: 0x001264E3 File Offset: 0x001248E3
		public void SetButtonExpanded(BuildingOperation op, bool value)
		{
			this.animator.SetBool(op + "Expanded", value);
		}

		// Token: 0x06003B49 RID: 15177 RVA: 0x00126504 File Offset: 0x00124904
		private IEnumerator UpdateRoutine()
		{
			while (!this.initialized)
			{
				yield return null;
			}
			WaitForSeconds wait = new WaitForSeconds(1f);
			for (;;)
			{
				try
				{
					if (this.questService != null)
					{
						this.questIndicator.gameObject.SetActive(this.questService.HasNotification());
					}
				}
				catch (Exception ex)
				{
					WoogaDebug.LogWarning(new object[]
					{
						"Exception when checking notifications: " + ex.Message
					});
				}
				this.adAvailableHUDIcon.SetActive(this.ShouldShowAdAvailableNotification());
				// 1. 广告轮盘
				this.wheelHudIcon.SetActive(this.videoAdService.HasUnlockedVideoAd);
				// 隐藏fb按钮
				// this.facebookLoginIcon.SetActive(!this.facebookService.LoggedIn() && this.progression.UnlockedLevel > this.configService.general.balance.facebook_hud_icon_level && !this.gameStateService.GetSeenFlag("fb_connect"));
				// this.facebookLoginRewardIcon.SetActive(this.facebookLoginIcon.activeSelf && !this.gameStateService.Facebook.receivedLoginReward);
				this.travelIndicator.SetActive(this.contentUnlockService.GetLockedByContentIslandId() > 0);
				yield return wait;
			}
			yield break;
		}

		// Token: 0x06003B4A RID: 15178 RVA: 0x00126520 File Offset: 0x00124920
		private bool ShouldShowAdAvailableNotification()
		{
			return this.videoAdService.IsVideoAvailable(true) && this.videoAdService.IsAllowedToWatchAd(AdPlacement.AdWheel) && Scripts1.DateTimeExtensions.FromUnixTimeStamp(this.gameStateService.NextSpinAvailable, DateTimeKind.Utc) < DateTime.Now && Application.internetReachability != NetworkReachability.NotReachable;
		}

		// Token: 0x06003B4B RID: 15179 RVA: 0x0012657D File Offset: 0x0012497D
		public void Handle(TownOptionsCommand tab)
		{
			if (this.IsInteractable)
			{
				this.onOptionsTabClick.Dispatch(tab);
			}
			else
			{
				WoogaDebug.Log(new object[]
				{
					"TownBottomPanelRoot: UI not interactable."
				});
			}
		}

		// Token: 0x06003B4C RID: 15180 RVA: 0x001265B0 File Offset: 0x001249B0
		public void AnimatedClouds(bool openClouds)
		{
			this.cloudsCanvas.SetActive(true);
			this.cloudsAnimation.clip = this.cloudsOpening;
			this.cloudsAnimation[this.cloudsOpening.name].time = 0f;
			this.cloudsAnimation.Play();
		}

		// Token: 0x06003B4D RID: 15181 RVA: 0x00126606 File Offset: 0x00124A06
		public void DisableClouds()
		{
			this.cloudsCanvas.SetActive(false);
		}

		// Token: 0x06003B4E RID: 15182 RVA: 0x00126614 File Offset: 0x00124A14
		public void Refresh()
		{
			if (this.updateRoutine != null)
			{
				base.StopCoroutine(this.updateRoutine);
			}
			if (base.gameObject.activeInHierarchy)
			{
				this.pendingRefresh = false;
				this.UpdateTournamentStatus();
				this.updateRoutine = base.StartCoroutine(this.CheckForUpdates());
			}
			else
			{
				this.pendingRefresh = true;
			}
		}

		// Token: 0x06003B4F RID: 15183 RVA: 0x00126674 File Offset: 0x00124A74
		public void SetTropicamIndicator(bool state)
		{
			UiIndicator componentInChildren = this.buttonTakePicture.GetComponentInChildren<UiIndicator>(true);
			componentInChildren.gameObject.SetActive(state);
		}

		// Token: 0x06003B50 RID: 15184 RVA: 0x0012669A File Offset: 0x00124A9A
		public void HandleBuildingStored(BuildingConfig building, Vector3 postion)
		{
			base.StartCoroutine(this.StoreBuildingAnimationRoutine(building, postion));
		}

		// Token: 0x06003B51 RID: 15185 RVA: 0x001266AC File Offset: 0x00124AAC
		protected override void Go()
		{
			this.SetTropicamIndicator(!this.progression.Data.HasOpenedTropicam);
			this.SetupOffers();
			this.SetupBadgeController();
			this.challengesIcon.Init(this.progression, this.challengeService, this.loc);
			this.bankIcon.Init();
			this.levelOfDayPlayButton.Init(this.levelOfDayService);
			if (base.gameObject.activeSelf)
			{
				this.updateRoutine = base.StartCoroutine(this.CheckForUpdates());
				this.Refresh();
				base.Go();
			}
			this.wheelHudIcon.gameObject.SetActive(this.videoAdService.HasUnlockedVideoAd);
			this.buildingImage.enabled = false;
			this.initialized = true;
			this.IsInteractable = false;
		}

		// Token: 0x06003B52 RID: 15186 RVA: 0x0012677A File Offset: 0x00124B7A
		protected override void OnDisable()
		{
			if (this.updateRoutine != null)
			{
				base.StopCoroutine(this.updateRoutine);
			}
			if (this.questUpdateRoutine != null)
			{
				base.StopCoroutine(this.questUpdateRoutine);
				this.questUpdateRoutine = null;
			}
			base.OnDisable();
		}

		// Token: 0x06003B53 RID: 15187 RVA: 0x001267B7 File Offset: 0x00124BB7
		protected override void OnDestroy()
		{
			if (this.tournamentBadgeController != null)
			{
				this.tournamentBadgeController.RemoveListeners();
			}
			base.OnDestroy();
		}

		// Token: 0x06003B54 RID: 15188 RVA: 0x001267DC File Offset: 0x00124BDC
		protected override void OnEnable()
		{
			base.OnEnable();
			this.questUpdateRoutine = base.StartCoroutine(this.UpdateRoutine());
			if (this.pendingRefresh)
			{
				this.Refresh();
			}
			else
			{
				this.UpdateTournamentStatus();
			}
			this.State = this._state;
		}

		// Token: 0x06003B55 RID: 15189 RVA: 0x00126829 File Offset: 0x00124C29
		public void UpdateTournamentStatus()
		{
			this.tournamentBadgeController.UpdateTournamentStatus();
		}

		// Token: 0x06003B56 RID: 15190 RVA: 0x00126838 File Offset: 0x00124C38
		private void SetupBadgeController()
		{
			this.tournamentBadgeController.Init(this.tournamentService, this.gameStateService, this);
			this.diveForTreasureBadgeController.Init(this);
			this.pirateBreakoutBadgeController.Init(this);
			this.seasonalPromoBadgeUi.Init(this);
			this.saleBadgeUi.Init(this);
		}

		// Token: 0x06003B57 RID: 15191 RVA: 0x0012688D File Offset: 0x00124C8D
		private void SetupOffers()
		{
			this.starterPack = base.GetComponentInChildren<StarterPackController>();
			this.starterPack.Init(this.gameStateService, this.configService.general, this.progression, this.offersService, this.saleService);
		}

		// Token: 0x06003B58 RID: 15192 RVA: 0x001268CC File Offset: 0x00124CCC
		private IEnumerator CheckForUpdates()
		{
			WaitForSeconds waitTime = new WaitForSeconds(15f);
			for (;;)
			{
				int inboxNotificationCount = 0;
				if (this.helpShift != null)
				{
					if (this.notificationCount != this.helpShift.notificationCount)
					{
						this.notificationCount = this.helpShift.notificationCount;
						this.HelpshiftMessageCount.text = this.notificationCount.ToString();
						this.HelpshiftMessages.gameObject.SetActive(this.notificationCount > 0);
					}
					inboxNotificationCount += this.helpShift.inboxCount;
				}
				if (this.facebookService.LoggedIn())
				{
					this.facebookService.FetchRequests();
					Wooroutine<IEnumerable<FacebookData.Request>> requests = WooroutineRunner.StartWooroutine<IEnumerable<FacebookData.Request>>(this.facebookService.FetchRequests());
					yield return requests;
					List<FacebookData.Request> results = requests.ReturnValue.ToList<FacebookData.Request>();
					inboxNotificationCount += results.Count;
				}
				else
				{
					this.livesMessagesIndicator.SetActive(false);
				}
				if (inboxNotificationCount > 0)
				{
					this.livesMessagesIndicator.SetActive(true);
					this.livesMessageCount.text = inboxNotificationCount.ToString();
				}
				else
				{
					this.livesMessagesIndicator.SetActive(false);
				}
				yield return waitTime;
			}
			yield break;
		}

		// Token: 0x06003B59 RID: 15193 RVA: 0x001268E8 File Offset: 0x00124CE8
		private IEnumerator StoreBuildingAnimationRoutine(BuildingConfig building, Vector3 startPos)
		{
			yield return this.buildingResource.CacheBuildingIconAsync(building);
			Sprite buildingSprite = this.buildingResource.GetWrappedSpriteOrPlaceholder(building).asset;
			if (!buildingSprite)
			{
				yield break;
			}
			this.buildingImage.sprite = buildingSprite;
			this.buildingImage.rectTransform.sizeDelta = new Vector2(250, 250);
			this.buildingDoober.position = startPos;
			this.buildingDoober.localScale = Vector3.zero;
			this.buildingImage.enabled = true;
			yield return this.buildingDoober.transform.DOScale(Vector3.one, this.BUILDING_STORED_SCALE_DURATION).WaitForCompletion();
			yield return this.buildingDoober.transform.DOMove(this.shop.position, this.BUILDING_STORED_FLY_DURATION, false).WaitForCompletion();
			yield return this.buildingDoober.transform.DOScale(Vector3.zero, this.BUILDING_STORED_SCALE_DURATION).WaitForCompletion();
			this.buildingImage.enabled = false;
			yield break;
		}

		// Token: 0x06003B5A RID: 15194 RVA: 0x00126914 File Offset: 0x00124D14
		public IEnumerator ShowNewDecoRoutine(int chapter)
		{
			yield return this.buildingResource.LoadSprites();
			yield return new WaitForSeconds((float)this.configService.general.chapter_buildings.fadein_time);
			while (this.State != TownBottomPanelRoot.UIState.InGameUI)
			{
				yield return null;
			}
			List<BuildingConfig> buildings = new List<BuildingConfig>();
			int islandId = this.gameStateService.Buildings.CurrentIsland;
			foreach (BuildingConfig buildingConfig in this.configService.buildingConfigList.buildings)
			{
				if (buildingConfig.chapter_id == chapter && buildingConfig.island_id == islandId)
				{
					buildings.Add(buildingConfig);
				}
			}
			buildings.Sort((BuildingConfig a, BuildingConfig b) => b.harmony.CompareTo(a.harmony));
			if (buildings.Count > 0)
			{
				while (!base.gameObject.activeInHierarchy)
				{
					yield return null;
				}
				List<Sprite> buildingSprites = buildings.ConvertAll<Sprite>((BuildingConfig b) => this.buildingResource.GetWrappedSpriteOrPlaceholder(b).asset);
				this.shopTooltip.Show(buildingSprites, this.configService.general.chapter_buildings);
				this.animator.SetBool("NewDecoration", true);
				base.StartCoroutine(this.HideNewDeco());
			}
			bool shouldSaveSeenFlag = buildings.Count > 0;
			yield return shouldSaveSeenFlag;
			yield break;
		}

		// Token: 0x06003B5B RID: 15195 RVA: 0x00126938 File Offset: 0x00124D38
		private IEnumerator HideNewDeco()
		{
			yield return new WaitForSeconds((float)this.configService.general.chapter_buildings.fadeout_time);
			this.animator.SetBool("NewDecoration", false);
			yield break;
		}

		// Token: 0x0400633D RID: 25405
		[WaitForRoot(false, false)]
		private BuildingResourceServiceRoot buildingResource;

		// Token: 0x0400633E RID: 25406
		[WaitForService(true, true)]
		private HelpshiftService helpShift;

		// Token: 0x0400633F RID: 25407
		[WaitForService(true, true)]
		private FacebookService facebookService;

		// Token: 0x04006340 RID: 25408
		[WaitForService(true, true)]
		public ILocalizationService loc;

		// Token: 0x04006341 RID: 25409
		[WaitForService(true, true)]
		public AudioService audioService;

		// Token: 0x04006342 RID: 25410
		[WaitForService(true, true)]
		public QuestService questService;

		// Token: 0x04006343 RID: 25411
		[WaitForService(true, true)]
		public SBSService sbsService;

		// Token: 0x04006344 RID: 25412
		[WaitForService(true, true)]
		public ProgressionDataService.Service progression;

		// Token: 0x04006345 RID: 25413
		[WaitForService(true, true)]
		private ConfigService configService;

		// Token: 0x04006346 RID: 25414
		[WaitForService(true, true)]
		private GameStateService gameStateService;

		// Token: 0x04006347 RID: 25415
		[WaitForService(true, true)]
		private IVideoAdService videoAdService;

		// Token: 0x04006348 RID: 25416
		[WaitForService(true, true)]
		private OffersService offersService;

		// Token: 0x04006349 RID: 25417
		[WaitForService(true, true)]
		private TournamentService tournamentService;

		// Token: 0x0400634A RID: 25418
		[WaitForService(true, true)]
		private ChallengeService challengeService;

		// Token: 0x0400634B RID: 25419
		[WaitForService(true, true)]
		private BankService bankService;

		// Token: 0x0400634C RID: 25420
		[WaitForService(true, true)]
		private LevelOfDayService levelOfDayService;

		// Token: 0x0400634D RID: 25421
		[WaitForService(true, true)]
		private ContentUnlockService contentUnlockService;

		// Token: 0x0400634E RID: 25422
		[WaitForService(true, true)]
		private SaleService saleService;

		// Token: 0x0400634F RID: 25423
		public UiIndicator newBuildingsIndicator;

		// Token: 0x04006350 RID: 25424
		public BuildingUiControlPanel controlButtons;

		// Token: 0x04006351 RID: 25425
		public Image buildingImage;

		// Token: 0x04006352 RID: 25426
		public Transform buildingDoober;

		// Token: 0x04006353 RID: 25427
		public RectTransform shop;

		// Token: 0x04006354 RID: 25428
		public float BUILDING_STORED_SCALE_DURATION = 0.1f;

		// Token: 0x04006355 RID: 25429
		public float BUILDING_STORED_FLY_DURATION = 0.5f;

		// Token: 0x04006356 RID: 25430
		public readonly Signal<TownOptionsCommand> onOptionsTabClick = new Signal<TownOptionsCommand>();

		// Token: 0x04006357 RID: 25431
		private const int REFRESH_FREQUENCY = 15;

		// Token: 0x04006358 RID: 25432
		private Coroutine updateRoutine;

		// Token: 0x04006359 RID: 25433
		private Coroutine questUpdateRoutine;

		// Token: 0x0400635A RID: 25434
		[SerializeField]
		private TMP_Text HelpshiftMessageCount;

		// Token: 0x0400635B RID: 25435
		[SerializeField]
		private GameObject HelpshiftMessages;

		// Token: 0x0400635C RID: 25436
		[SerializeField]
		private TMP_Text livesMessageCount;

		// Token: 0x0400635D RID: 25437
		[SerializeField]
		private GameObject livesMessagesIndicator;

		// Token: 0x0400635E RID: 25438
		[SerializeField]
		private Button buttonTakePicture;

		// Token: 0x0400635F RID: 25439
		[SerializeField]
		private Animator animator;

		// Token: 0x04006360 RID: 25440
		[SerializeField]
		[AutoSet]
		private StarterPackController starterPack;

		// Token: 0x04006361 RID: 25441
		[SerializeField]
		[AutoSet]
		private ChallengesIconController challengesIcon;

		// Token: 0x04006362 RID: 25442
		[SerializeField]
		[AutoSet]
		private BankIconController bankIcon;

		// Token: 0x04006363 RID: 25443
		[SerializeField]
		private TournamentBadgeUIController tournamentBadgeController;

		// Token: 0x04006364 RID: 25444
		[SerializeField]
		private DiveForTreasureBadgeController diveForTreasureBadgeController;

		// Token: 0x04006365 RID: 25445
		[SerializeField]
		private PirateBreakoutBadgeController pirateBreakoutBadgeController;

		// Token: 0x04006366 RID: 25446
		[SerializeField]
		private SeasonalPromoBadgeUi seasonalPromoBadgeUi;

		// Token: 0x04006367 RID: 25447
		[SerializeField]
		private SaleBadgeUi saleBadgeUi;

		// Token: 0x04006368 RID: 25448
		[SerializeField]
		public LevelOfDayPlayButtonUI levelOfDayPlayButton;

		// Token: 0x04006369 RID: 25449
		[SerializeField]
		public GameObject cloudsCanvas;

		// Token: 0x0400636A RID: 25450
		[SerializeField]
		public Animation cloudsAnimation;

		// Token: 0x0400636B RID: 25451
		[SerializeField]
		private AnimationClip cloudsOpening;

		// Token: 0x0400636C RID: 25452
		public GameObject adAvailableHUDIcon;

		// Token: 0x0400636D RID: 25453
		public GameObject facebookLoginIcon;

		// Token: 0x0400636E RID: 25454
		public GameObject facebookLoginRewardIcon;

		// Token: 0x0400636F RID: 25455
		public TownBottomPanelShopTooltip shopTooltip;

		// Token: 0x04006370 RID: 25456
		public UiIndicator questIndicator;

		// Token: 0x04006371 RID: 25457
		public GameObject travelIndicator;

		// Token: 0x04006372 RID: 25458
		public GameObject wheelHudIcon;

		// Token: 0x04006373 RID: 25459
		private bool initialized;

		// Token: 0x04006374 RID: 25460
		private TownBottomPanelRoot.UIState _oldState = TownBottomPanelRoot.UIState.InGameUI;

		// Token: 0x04006375 RID: 25461
		private TownBottomPanelRoot.UIState _state = TownBottomPanelRoot.UIState.InGameUI;

		// Token: 0x04006377 RID: 25463
		private int notificationCount = -1;

		// Token: 0x04006378 RID: 25464
		private bool pendingRefresh;

		// Token: 0x02000982 RID: 2434
		public enum UIState
		{
			// Token: 0x0400637A RID: 25466
			None,
			// Token: 0x0400637B RID: 25467
			InGameUI,
			// Token: 0x0400637C RID: 25468
			MovementMode
		}
	}
}
