using System;
using System.Collections;
using System.Collections.Generic;
using AndroidTools;
using AndroidTools.Tools;
using Match3.Scripts1;
using Match3.Scripts1.Puzzletown;
using Match3.Scripts1.Puzzletown.Build;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Puzzletown.Town;
using Match3.Scripts1.Puzzletown.UI;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.Legal;
using Wooga.UnityFramework;
using Match3.Scripts2.Android.DataStatistics;
using Match3.Scripts2.Env;
using Match3.Scripts2.Network;
using Match3.Scripts2.PlayerData;
using Match3.Scripts2.Shop;
// using New.Plugins.IllegalWordsDetection;
using UnityEngine;
using UnityEngine.UI;
using SceneManager = Wooga.UnityFramework.SceneManager;

// Token: 0x020008B8 RID: 2232
namespace Match3.Scripts2.Start
{
	[LoadOptions(false, false, true)]
	public class StartRoot : APtSceneRoot<bool>
	{
		protected override bool IsSetup
		{
			get
			{
				return true;
			}
		}

		protected override void Awake()
		{
			// android
			// new GameObject().AddComponent<AndroidAgent>();
		
			// 生产环境关闭log
			UnityEngine.Debug.unityLogger.logEnabled = !GameEnvironment.IsProduction;
		
			// bool enableErrorHandling = !Application.isEditor;
			bool enableErrorHandling = false;
			// ErrorAnalytics.Init(SbsEnvironment.CurrentId, 75, enableErrorHandling, null, false);
			StartRoot.isFirstSession = !APlayerPrefsObject<GameState>.Exists();
			if (!StartRoot.isFirstSession)
			{
				// AUiAdjuster.orientationChangesEnabled = true;
				AUiAdjuster.orientationChangesEnabled = false;
				AUiAdjuster.SetOrientationLock(false);
			}

			// 初始化敏感词
			// BadWordsDetection.Init();
			// 载入防沉迷场景
// #if AAK
// 		UnityEngine.SceneManagement.SceneManager.LoadScene("AntiAddictionScene", LoadSceneMode.Additive);
// #endif
			// 载入EventSystem
			// StartCoroutine(StartEventSystem());
			// 开始按钮事件
			startButton.onClick.RemoveAllListeners();
			startButton.onClick.AddListener(()=>
			{
				// 开始游戏按钮事件
// #if AAK
// 			AntiAddictionCtrl.Instance.LoginAAK(null, () =>
// 			{
// 				AAUIController.Instance.CloseAAView();
// #endif
				DataStatistics.Instance.TriggerEnterGameEvent(2, (int) Time.time, SystemInfo.deviceUniqueIdentifier, BuildVersion.Version, GameEnvironment.CurrentPlatform.ToString());
				startButton.gameObject.SetActive(false);;
				StartCoroutine(LoadGameData());
// #if AAK
// 			});
// #endif
			});
		}

		private void Start()
		{
			DataStatistics.Instance.TriggerEnterGameEvent(1, (int) Time.time, SystemInfo.deviceUniqueIdentifier, BuildVersion.Version, GameEnvironment.CurrentPlatform.ToString());
		}

		private IEnumerator LoadGameData()
		{
			// 检查网络连接
			logginIn.gameObject.SetActive(true);
			if (Application.internetReachability == NetworkReachability.NotReachable)
			{
				// 弹出无网的窗口
				Debug.LogError("网络连接失败");
				networkTipWindow.Show("The network connection has been interrupted. Please make sure the network is normal to continue.");
				while (networkTipWindow.gameObject.activeSelf)
				{
					yield return null;
				}

				yield return new WaitForSeconds(1);
				yield return LoadGameData();
				yield break;
			}
		
			// 登录
			Wooroutine<List<object>> login = WooroutineRunner.StartWooroutine<List<object>>(ToServer.Instance.Login());
			yield return login;
			if ((bool)login.ReturnValue[0] == false)
			{
				string err = (string) login.ReturnValue[1];
				if (!string.IsNullOrEmpty(err))
				{
					networkTipWindow.Show(err);
				
					while (networkTipWindow.gameObject.activeSelf)
					{
						yield return null;
					}
				}

				yield return new WaitForSeconds(1);
				yield return LoadGameData();
				yield break;
			}
		
			// 载入游戏
			DataStatistics.Instance.TriggerEnterGameEvent(3, (int) Time.time, SystemInfo.deviceUniqueIdentifier, BuildVersion.Version, GameEnvironment.CurrentPlatform.ToString());
			base.Awake();
			foreach (Behaviour behaviour in this.multipleHideThis)
			{
				behaviour.enabled = true;
			}
			// 载入loadingscreen场景
			LoadingScreenRoot.Init();
			base.StartCoroutine(this.DisableStartupLoadingSpinner());
			if (GameEnvironment.CurrentEnvironment != GameEnvironment.Environment.PRODUCTION)
			{
				// eli key point 显示debuginfo相关UI
				// SceneManager.Instance.LoadScene<DebugInfoRoot>(null);
			}
			if (Application.isEditor)
			{
				// SceneManager.Instance.LoadScene<IphoneXOverlayRoot>(null);	// 不知道有什么用，会报错
			}
		}

		private IEnumerator StartEventSystem()
		{
			Wooroutine<EventSystemRoot> eventSystem = SceneManager.Instance.LoadScene<EventSystemRoot>(null);
			yield return eventSystem;
			eventSystem.ReturnValue.Enable();
		}
	
		protected override IEnumerator GoRoutine()
		{
			// eli key point: 强制更新
			// Wooroutine<bool> forceUpdate = new ForceUpdate().Start<bool>();
			// yield return forceUpdate;
			// if (forceUpdate.ReturnValue)
			// {
			// yield break;
			// }
			if (!StartRoot.isFirstSession)
			{
				Wooroutine<SBSService> sbsServiceRoutine = ServiceLocator.Instance.Await<SBSService>(false);
				yield return sbsServiceRoutine;
				if (sbsServiceRoutine.ReturnValue.IsAuthenticated)
				{
					Terms.FetchState();
				}
			}
			// AUiAdjuster.orientationChangesEnabled = true;
			AUiAdjuster.orientationChangesEnabled = false;
			AUiAdjuster.VideoMode mode = AUiAdjuster.GetVideoMode();
			if (StartRoot.isFirstSession)
			{
				ScreenOrientation forcedOrientation = ScreenOrientation.Portrait;
				if (mode.isTablet)
				{
					forcedOrientation = ScreenOrientation.LandscapeLeft;
				}
				yield return AUiAdjuster.ForceScreenSimilarOrientation(forcedOrientation);
			}
			mode = AUiAdjuster.GetVideoMode();
			this.LoadStartScene().ShowLoadingScreen();
		
			yield return this.tracking.Await(delegate(TrackingService t)
			{
				this.tracking = t;
			});
			this.tracking.TrackFunnelEvent("005_game_open", 5, null);
			if (StartRoot.isFirstSession)
			{
				this.tracking.TrackFunnelEvent("015_loading_start", 15, null);
				yield return this.configService.Await(delegate(ConfigService c)
				{
					this.configService = c;
				});
				if (AUiAdjuster.GetVideoMode().isTablet && this.configService.FeatureSwitchesConfig.force_landscape_tablets)
				{
					yield return AUiAdjuster.ForceScreenSimilarOrientation(ScreenOrientation.LandscapeLeft);
				}
				else
				{
					AUiAdjuster.SetOrientationLock(false);
				}
			}
			SceneManager.onLoadSceneNonAdditive.AddListener(new Action(BackButtonManager.Instance.HandleLoadSceneNonAdditive));
		}

		// Token: 0x06003680 RID: 13952 RVA: 0x001090E0 File Offset: 0x001074E0
		private IEnumerator DisableStartupLoadingSpinner()
		{
			while (LoadingScreenRoot.instance == null)
			{
				yield return null;
			}
			yield return LoadingScreenRoot.instance.OnInitialized;
			this.spinnerCanvas.gameObject.SetActive(false);
			yield break;
		}

		// Token: 0x06003681 RID: 13953 RVA: 0x001090FB File Offset: 0x001074FB
		private Coroutine LoadStartScene()
		{
			return WooroutineRunner.StartCoroutine(this.WaitForScenesToLoad(), null);
		}

		// Token: 0x06003682 RID: 13954 RVA: 0x0010910C File Offset: 0x0010750C
		private IEnumerator WaitForScenesToLoad()
		{
			Wooroutine<GameStateService> gameState = ServiceLocator.Instance.Await<GameStateService>(true);
			yield return gameState;
			int currentIsland = gameState.ReturnValue.Buildings.CurrentIsland;
			if (currentIsland > 0)
			{
				Wooroutine<AssetBundleService> abs = ServiceLocator.Instance.Await<AssetBundleService>(true);
				yield return abs;
				string sceneName = TownMainRoot.GetSceneNameForIsland(currentIsland);
				string bundleName = SceneManager.Instance.GetSceneBundleName(sceneName);
				Wooroutine<bool> isAvailableRoutine = abs.ReturnValue.IsBundleAvailable(bundleName);
				yield return isAvailableRoutine;
				if (!isAvailableRoutine.ReturnValue)
				{
					gameState.ReturnValue.Buildings.CurrentIsland = 0;
				}
			}
			yield return this.SetupNewIslandUiLayoutABTest();
			Wooroutine<TownMainRoot> startRoot = TownMainRoot.Load(gameState.ReturnValue.Buildings.CurrentIsland, new LoadOptions(true, false, true));
			yield return startRoot;
			SceneManager.Instance.Unload(this);
			yield return startRoot.ReturnValue.OnInitialized;
			Coroutine legalTerms = new WoogaLegalTermsFlow().Start();
			yield return legalTerms;
			// eli key point 屏蔽隐私协议
			if (!this.parameters && !StartRoot.isFirstSession || true)
			{
				// 载入开始界面
				Wooroutine<StartScreenRoot> startScreen = SceneManager.Instance.LoadScene<StartScreenRoot>(null);
				yield return startScreen;
				yield return startScreen.ReturnValue.onDestroyed;
			}
			else if (StartRoot.isFirstSession)
			{
				yield return this.progression.Await(delegate(ProgressionDataService.Service p)
				{
					this.progression = p;
				});
				TownMainRoot townMain = UnityEngine.Object.FindObjectOfType<TownMainRoot>();
				if (townMain != null)
				{
					townMain.uiRoot.ResourcePanel.gameObject.SetActive(false);
					townMain.uiRoot.BottomPanel.State = TownBottomPanelRoot.UIState.None;
					yield return new WaitForSeconds(0.5f);
				}
				CameraInputController.current.Zoom = CameraInputController.current.zoomLimit.y;
				this.progression.onGameStarted.Dispatch();
				this.progression.onLevelUnlocked.Dispatch(0);
				// 审核模式不显示章节intro
				// #if !REVIEW_VERSION
				yield return this.ShowFirstIntro();
				// #endif
				startRoot.ReturnValue.StartView(true, false);
			}
			DataStatistics.Instance.TriggerEnterGameEvent(6, (int) Time.time, SystemInfo.deviceUniqueIdentifier, BuildVersion.Version, GameEnvironment.CurrentPlatform.ToString());
			Debug.Log("场景载入完毕");
		
			// 补单查询, 使用三星海外sdk的话，对api调用顺序有要求，所以在这里调用补单查询的api
			// 另外补单查询也要在所有的场景载入完毕后调用，因为发放奖励需要调用"奖励窗口"
			Debug.Log("补单查询");
			AndroidPay.ReplenishmentQuery();
			Wooroutine<IAPService> iapService = ServiceLocator.Instance.Await<IAPService>(true);
			yield return iapService;
			iapService.ReturnValue.InitStoreItem(WooroutineRunner.Stop, e=>WooroutineRunner.StartCoroutine(e));
		}

		// Token: 0x06003683 RID: 13955 RVA: 0x00109128 File Offset: 0x00107528
		private IEnumerator SetupNewIslandUiLayoutABTest()
		{
			yield return this.configService.Await(delegate(ConfigService c)
			{
				this.configService = c;
			});
			bool enabled = this.configService.FeatureSwitchesConfig.new_island_ui_layout;
			SceneManager.Instance.MapTypeToSceneName(typeof(TownBottomPanelRoot), (!enabled) ? "TownBottomPanel" : "TownBottomPanelV2");
			SceneManager.Instance.MapTypeToSceneName(typeof(BannerNewCollectableRoot), (!enabled) ? "BannerNewCollectable" : "BannerNewCollectableV2");
			yield break;
		}

		// Token: 0x06003684 RID: 13956 RVA: 0x00109144 File Offset: 0x00107544
		private IEnumerator ShowFirstIntro()
		{
			Wooroutine<BannerNewChapterRoot> intro = SceneManager.Instance.LoadSceneWithParams<BannerNewChapterRoot, int>(1, null);
			yield return intro;
			this.firstIntro = intro.ReturnValue;
			yield return new WaitForSeconds(1.25f);
			BlockerManager.global.Append(new Func<IEnumerator>(this.WaitForEndOfIntro), false);
			yield break;
		}

		// Token: 0x06003685 RID: 13957 RVA: 0x00109160 File Offset: 0x00107560
		private IEnumerator WaitForEndOfIntro()
		{
			yield return this.firstIntro.onDestroyed;
			if (this.configService == null)
			{
				yield return this.configService.Await(delegate(ConfigService c)
				{
					this.configService = c;
				});
			}
			yield return this.externalGamesService.Await(delegate(ExternalGamesService s)
			{
				this.externalGamesService = s;
			});
			bool silentLogin = !this.configService.FeatureSwitchesConfig.initial_login_to_external_games_service;
			this.externalGamesService.LogIn(true, silentLogin);
			yield break;
		}

		// Token: 0x04005E7F RID: 24191
		private const float ANIMATION_ALPHA_DELAY = 1.25f;

		// Token: 0x04005E80 RID: 24192
		private const float UI_ANIMATION_DURATION = 0.5f;

		// Token: 0x04005E81 RID: 24193
		private TrackingService tracking;

		// Token: 0x04005E82 RID: 24194
		private ProgressionDataService.Service progression;

		// Token: 0x04005E83 RID: 24195
		private ExternalGamesService externalGamesService;

		// Token: 0x04005E84 RID: 24196
		private ConfigService configService;

		// Token: 0x04005E85 RID: 24197
		[SerializeField]
		private Canvas spinnerCanvas;

		[SerializeField] private Button startButton;
		[SerializeField] private NetworkTip networkTipWindow;
		[SerializeField] private GameObject logginIn;

		// Token: 0x04005E86 RID: 24198
		public static bool isFirstSession;

		// Token: 0x04005E87 RID: 24199
		private BannerNewChapterRoot firstIntro;
	}
}
