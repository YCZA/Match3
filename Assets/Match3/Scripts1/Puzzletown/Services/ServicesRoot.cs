using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.Services.ErrorAnalytics;
using Wooga.UnityFramework;
using Match3.Scripts2.Env;
using Match3.Scripts2.PlayerData;
using Match3.Scripts2.Shop;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x02000802 RID: 2050
	[LoadOptions(true, true, true)]
	public class ServicesRoot : ASceneRoot
	{
		// Token: 0x170007F9 RID: 2041
		// (get) Token: 0x060032A4 RID: 12964 RVA: 0x000EEA4B File Offset: 0x000ECE4B
		// (set) Token: 0x060032A5 RID: 12965 RVA: 0x000EEA53 File Offset: 0x000ECE53
		public bool AllServicesStarted { get; private set; }

		// Token: 0x060032A6 RID: 12966 RVA: 0x000EEA5C File Offset: 0x000ECE5C
		protected override void Awake()
		{
			WooroutineRunner.StartCoroutine(this.CheckForTimeoutRoutine(), null);
			if (!ErrorAnalytics.isInitialized)
			{
				// ErrorAnalytics.Init(SbsEnvironment.CurrentId, 50, true, null, false);
			}
			this.exHandler = new ExceptionHandlerService();
			ServiceLocator.Instance.Register<ExceptionHandlerService>(this.exHandler);
			if (GameEnvironment.CurrentEnvironment != GameEnvironment.Environment.PRODUCTION)
			{
				WoogaDebug.LogLevel = WoogaDebug.LogLevels.Info;
			}
			base.Awake();
			global::UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			base.StartCoroutine(this.InitDependenciesRoutine());
		}

		// Token: 0x060032A7 RID: 12967 RVA: 0x000EEADC File Offset: 0x000ECEDC
		private IEnumerator InitDependenciesRoutine()
		{
			GameSettingsService gameSettingsService = ServiceLocator.Instance.Register<GameSettingsService>(new GameSettingsService());
			ServiceLocator.Instance.Register<ILocalizationService>(new PTLocalizationService(gameSettingsService));
			SBSService sbs = ServiceLocator.Instance.Register<SBSService>(new SBSService());
			this.gameStateService = ServiceLocator.Instance.Register<GameStateService>(new GameStateService());
			yield return sbs.OnInitialized;
			ServiceLocator.Instance.Register<AssetBundleService>(new AssetBundleService());
			this.configService = ServiceLocator.Instance.Register<ConfigService>(new ConfigService());
			yield return this.configService.OnInitialized;
			if (!this.configService.IsValid)
			{
				yield break;
			}
			ServiceLocator.Instance.Register<M3ConfigService>(new M3ConfigService());
			ServiceLocator.Instance.Register<ContentUnlockService>(new ContentUnlockService());
			/**
			ServiceLocator.Instance.Register<FacebookService>(new FacebookService());
			**/
			ServiceLocator.Instance.Register<ProgressionDataService.Service>(new ProgressionDataService.Service());
			// ServiceLocator.Instance.Register<TimeService>(new TimeService(trackingService));
			ServiceLocator.Instance.Register<TimeService>(new TimeService());
			/**
			ServiceLocator.Instance.Register<QuestService>(new QuestService());
			TrackingService trackingService = ServiceLocator.Instance.Register<TrackingService>(new TrackingService());
			ServiceLocator.Instance.Register<SessionService>(new SessionService());
			// eli key point 在编辑器中不会用到adjustservice
			// if (Application.isEditor)
			// {
				ServiceLocator.Instance.Register<IAdjustService>(new AdjustServiceEditor());
			// }
			// else
			// {
				// ServiceLocator.Instance.Register<IAdjustService>(new AdjustService(trackingService));
			// }
			this.exHandler.Configure(sbs.SbsConfig.exceptions);
			**/
			IAPService iapService = ServiceLocator.Instance.Register<IAPService>(new IAPService());
			ServiceLocator.Instance.Register<LivesService>(new LivesService());
			yield return this.gameStateService.OnInitialized;
			/**
			ServiceLocator.Instance.Register<FPSService>(new FPSService());
			ServiceLocator.Instance.Register<DailyGiftsService>(new DailyGiftsService());
			**/
			ServiceLocator.Instance.Register<AssetBundlePreloadService>(new AssetBundlePreloadService());
			ServiceLocator.Instance.Register<AudioService>(new AudioService());
			/**
			ServiceLocator.Instance.Register<HelpshiftService>(new HelpshiftService());
			ServiceLocator.Instance.Register<DebugSettingsService>(new DebugSettingsService());
			**/
			ServiceLocator.Instance.Register<BoostsService>(new BoostsService(this.gameStateService.Resources));
			ServiceLocator.Instance.Register<LevelOfDayService>(new LevelOfDayService());
			/**
			ServiceLocator.Instance.Register<AdminService>(new AdminService());
			ServiceLocator.Instance.Register<PushNotificationService>(new PushNotificationService());
			ServiceLocator.Instance.Register<IVideoAdService>(new FairbidVideoAdService());
			ServiceLocator.Instance.Register<GiftLinksService>(new GiftLinksService());
			ServiceLocator.Instance.Register<LocalNotificationService>(new LocalNotificationService());
			ServiceLocator.Instance.Register<OffersService>(new OffersService(this.configService, this.gameStateService, iapService));
			ServiceLocator.Instance.Register<TournamentService>(new TournamentService());
			ServiceLocator.Instance.Register<ExternalGamesService>(new ExternalGamesService());
			ServiceLocator.Instance.Register<ChallengeService>(new ChallengeService());
			ServiceLocator.Instance.Register<BankService>(new BankService());
			ServiceLocator.Instance.Register<DiveForTreasureService>(new DiveForTreasureService());
			ServiceLocator.Instance.Register<PirateBreakoutService>(new PirateBreakoutService());
			ServiceLocator.Instance.Register<DailyDealsService>(new DailyDealsService());
			ServiceLocator.Instance.Register<SeasonService>(new SeasonService());
			ServiceLocator.Instance.Register<SaleService>(new SaleService());
			ServiceLocator.Instance.Register<SafeDkService>(new SafeDkService());
			**/
			this.AllServicesStarted = true;
			yield break;
		}

		// Token: 0x060032A8 RID: 12968 RVA: 0x000EEAF8 File Offset: 0x000ECEF8
		private IEnumerator CheckForTimeoutRoutine()
		{
			float startTime = Time.time;
			bool warningSent = false;
			while (!this.IsInitialized())
			{
				float t = Time.time - startTime;
				if (t >= 10f && !warningSent)
				{
					warningSent = true;
					Dictionary<string, object> dictionary = new Dictionary<string, object>();
					dictionary["Uninitialized Services"] = ServiceLocator.Instance.NotInitalizedServices().ToList<string>();
					dictionary["InitializedServices"] = ServiceLocator.Instance.InitializedServices().ToList<string>();
					Log.Warning("InitializationTimeout", "Initialization of services took longer than " + 10f + "s", dictionary);
				}
				yield return null;
			}
			yield break;
		}

		// Token: 0x060032A9 RID: 12969 RVA: 0x000EEB14 File Offset: 0x000ECF14
		private bool IsInitialized()
		{
			return ServiceLocator.Instance.Has(typeof(TrackingService), true) && ServiceLocator.Instance.Has(typeof(SBSService), true) && ServiceLocator.Instance.Has(typeof(GameStateService), true);
		}

		// Token: 0x060032AA RID: 12970 RVA: 0x000EEB70 File Offset: 0x000ECF70
		private void OnApplicationPause(bool paused)
		{
			if (paused == this.gamePaused)
			{
				return;
			}
			this.gamePaused = paused;
			if (this.gamePaused)
			{
				PTReloader.goToBackgroundTime = DateTime.UtcNow;
        // 通知相关
				// ServiceLocator.Instance.OnSuspend();
			}
			else if (this.AllServicesStarted && this.configService != null && this.configService.FeatureSwitchesConfig.reload_after_background && PTReloader.ShouldReloadGame())
			{
				PTReloader.ReloadGame("Too long time spent in background", false);
			}
			else
			{
				ServiceLocator.Instance.OnResume();
			}
		}

		// Token: 0x060032AB RID: 12971 RVA: 0x000EEC04 File Offset: 0x000ED004
		private void OnApplicationFocus(bool hasFocus)
		{
			this.OnApplicationPause(!hasFocus);
		}

		// Token: 0x060032AC RID: 12972 RVA: 0x000EEC10 File Offset: 0x000ED010
		private void OnApplicationQuit()
		{
			//eli key point 当程序退出时执行
			ServiceLocator.Instance.UnregisterAll();
		}

		// Token: 0x04005B12 RID: 23314
		private GameStateService gameStateService;

		// Token: 0x04005B13 RID: 23315
		private ConfigService configService;

		// Token: 0x04005B15 RID: 23317
		private ExceptionHandlerService exHandler;

		// Token: 0x04005B16 RID: 23318
		private bool gamePaused;
	}
}
