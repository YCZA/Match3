using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Match3.Scripts1.Puzzletown.Config;
using Match3.Scripts1.Puzzletown.Services;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.Services;
using Wooga.UnityFramework;
using Match3.Scripts2.Env;
using Match3.Scripts2.PlayerData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Match3.Scripts1.Puzzletown
{
	public class LoadingScreenRoot : APtSceneRoot
	{
		public static bool isVisible
		{
			get
			{
				return LoadingScreenRoot.instance != null && LoadingScreenRoot.instance.gameObject.activeInHierarchy;
			}
		}

		public static void Await(CustomYieldInstruction inst)
		{
			WooroutineRunner.StartCoroutine(LoadingScreenRoot.LoadSceneRoutine(inst), null);
		}

		public static void Init()
		{
			WooroutineRunner.StartCoroutine(LoadingScreenRoot.InitRoutine(), null);
		}

		public static float HideAnimDuration
		{
			get
			{
				if (!LoadingScreenRoot.instance || !LoadingScreenRoot.instance.inAnimatedTransition)
				{
					return 0f;
				}
				return 0.4f;
			}
		}

		// Token: 0x06003439 RID: 13369 RVA: 0x000F8DA4 File Offset: 0x000F71A4
		public static IEnumerator PrepareAnimatedLoadingScreen(LoadingScreenConfig config, bool autoHide = true)
		{
			LoadingScreenRoot.TryDisableAndroidBackButton();
			if (!LoadingScreenRoot.instance)
			{
				yield return LoadingScreenRoot.InitRoutine();
			}
			LoadingScreenRoot.instance.hideAutomatically = autoHide;
			yield return LoadingScreenRoot.instance.ShowTransitionStartAnimation(config);
			yield break;
		}

		// Token: 0x0600343A RID: 13370 RVA: 0x000F8DC8 File Offset: 0x000F71C8
		private static IEnumerator InitRoutine()
		{
			if (!LoadingScreenRoot.instance)
			{
				Wooroutine<LoadingScreenRoot> loadingScreen = SceneManager.Instance.LoadScene<LoadingScreenRoot>(null);
				yield return loadingScreen;
				LoadingScreenRoot.instance = loadingScreen.ReturnValue;
			}
			if (SBS.IsAuthenticated())
			{
				LoadingScreenRoot.instance.userIdLabel.text = SBS.Authentication.GetUserContext().user_id;
			}
			LoadingScreenRoot.instance.hideAutomatically = true;
			LoadingScreenRoot.instance.inAnimatedTransition = false;
			LoadingScreenRoot.instance.Setup(false);
		}

		// Token: 0x0600343B RID: 13371 RVA: 0x000F8DDC File Offset: 0x000F71DC
		private static IEnumerator LoadSceneRoutine(CustomYieldInstruction inst)
		{
			LoadingScreenRoot.TryDisableAndroidBackButton();
			if (!LoadingScreenRoot.instance)
			{
				yield return WooroutineRunner.StartCoroutine(LoadingScreenRoot.InitRoutine(), null);
			}
			if (!LoadingScreenRoot.instance.inAnimatedTransition)
			{
				LoadingScreenRoot.instance.Setup(false);
				LoadingScreenRoot.instance.Enable();
			}
			yield return inst;
			if (LoadingScreenRoot.instance.hideAutomatically)
			{
				yield return LoadingScreenRoot.Hide();
			}
			yield break;
		}

		// Token: 0x0600343C RID: 13372 RVA: 0x000F8DF8 File Offset: 0x000F71F8
		public static IEnumerator Hide()
		{
			if (LoadingScreenRoot.instance.inAnimatedTransition)
			{
				yield return LoadingScreenRoot.instance.ShowTransitionEndAnimation();
			}
			LoadingScreenRoot.instance.inAnimatedTransition = false;
			LoadingScreenRoot.instance.hideAutomatically = true;
			LoadingScreenRoot.instance.transitionDisplay.SetActive(false);
			LoadingScreenRoot.instance.userIdLabel.text = string.Empty;
			LoadingScreenRoot.instance.logoGameObject.SetActive(false);
			LoadingScreenRoot.instance.Disable();
			yield break;
		}

		// Token: 0x0600343D RID: 13373 RVA: 0x000F8E0C File Offset: 0x000F720C
		private static void TryDisableAndroidBackButton()
		{
			if (BackButtonManager.HasInstance)
			{
				BackButtonManager.Instance.SetEnabled(false);
			}
		}

		// Token: 0x0600343E RID: 13374 RVA: 0x000F8E23 File Offset: 0x000F7223
		public void UpdateProgress(LoadingScreenUpdater.ProgressInfo info)
		{
			// 隐藏加载进度
			// this.labelProgress.text = string.Format("Progress {0}/{1} {2}", info.numCompletedSteps, info.numTotalSteps, info.completedStep);
			Debug.Log(string.Format("Progress {0}/{1} {2}", info.numCompletedSteps, info.numTotalSteps, info.completedStep));
		}

		// Token: 0x0600343F RID: 13375 RVA: 0x000F8E5E File Offset: 0x000F725E
		private void OnValidate()
		{
		}

		// Token: 0x06003440 RID: 13376 RVA: 0x000F8E60 File Offset: 0x000F7260
		protected override void Awake()
		{
			this.staticBackground.raycastTarget = true;
			base.Awake();
		}

		// Token: 0x06003441 RID: 13377 RVA: 0x000F8E74 File Offset: 0x000F7274
		private void Start()
		{
			global::UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			this.SetupProgressDisplay();
		}

		// Token: 0x06003442 RID: 13378 RVA: 0x000F8E88 File Offset: 0x000F7288
		private void SetupProgressDisplay()
		{
			bool active = !GameEnvironment.IsProduction;
			this.labelProgress.gameObject.SetActive(active);
			if (!GameEnvironment.IsProduction)
			{
				new LoadingScreenUpdater(this);
			}
		}

		// Token: 0x06003443 RID: 13379 RVA: 0x000F8EC0 File Offset: 0x000F72C0
		protected override void Go()
		{
			// this.tracking.TrackFunnelEvent("006_pre_loading_start", 6, null);
			this.bundledScreenConfigs = this.configsService.general.loading_screens;
			// LoadingScreenInfo seasonalLoadingScreen = this.GetSeasonalLoadingScreen();
			// if (seasonalLoadingScreen != null)
			// {
				// this.bundledScreenConfigs.Add(seasonalLoadingScreen);
			// }
			this.TryPreloadRelevantAssetBundles();
		}

		// Token: 0x06003444 RID: 13380 RVA: 0x000F8F14 File Offset: 0x000F7314
		protected override void OnDestroy()
		{
			LoadingScreenRoot.instance = null;
			base.OnDestroy();
		}

		// Token: 0x06003445 RID: 13381 RVA: 0x000F8F24 File Offset: 0x000F7324
		private void TryPreloadRelevantAssetBundles()
		{
			WoogaDebug.Log(new object[]
			{
				"Bundles: TryPreload."
			});
			if (this.bundledScreenConfigs != null)
			{
				List<string> list = this.FindRequiredBundles();
				if (list.Count < 1)
				{
					WoogaDebug.Log(new object[]
					{
						"Bundles: No required loading screen bundle found."
					});
				}
				foreach (string text in list)
				{
					WoogaDebug.LogFormatted("Bundles: Trying to preload : {0}", new object[]
					{
						text
					});
					this.abs.PreLoadBundle(text, null);
				}
			}
			else
			{
				WoogaDebug.LogWarning(new object[]
				{
					"Bundles: No loading screen bundles configured."
				});
			}
		}

		// Token: 0x06003446 RID: 13382 RVA: 0x000F8FF4 File Offset: 0x000F73F4
		private List<string> FindRequiredBundles()
		{
			List<string> list = new List<string>();
			WoogaDebug.LogFormatted("Bundles: bundled loading screen config count: {0}", new object[]
			{
				this.bundledScreenConfigs.Count
			});
			foreach (LoadingScreenInfo loadingScreenInfo in this.bundledScreenConfigs)
			{
				if (!string.IsNullOrEmpty(loadingScreenInfo.id) && !string.IsNullOrEmpty(loadingScreenInfo.assetBundle) && !list.Contains(loadingScreenInfo.assetBundle))
				{
					list.Add(loadingScreenInfo.assetBundle);
				}
			}
			return list;
		}

		// Token: 0x06003447 RID: 13383 RVA: 0x000F90B0 File Offset: 0x000F74B0
		public IEnumerator ShowTransitionStartAnimation(LoadingScreenConfig config)
		{
			// this.giftLinksService.AllowedToProcessGiftLinks = false;
			this.Setup(true);
			base.Enable();
			yield return new WaitForSeconds(this.WaitForAnimationLengthInSeconds());
			yield return this.ShowLoadingScreenOverlay(config);
			yield return this.FadeInBackgroundCover();
			yield break;
		}

		// Token: 0x06003448 RID: 13384 RVA: 0x000F90D4 File Offset: 0x000F74D4
		private IEnumerator FadeInBackgroundCover()
		{
			Color fadedToBlack = this.backgroundCoverImage.color;
			fadedToBlack.a = 0f;
			this.backgroundCoverImage.color = fadedToBlack;
			this.backgroundCoverImage.gameObject.SetActive(true);
			this.backgroundCoverImage.DOFade(1f, 0.2f);
			yield return new WaitForSeconds(0.22f);
			yield break;
		}

		// Token: 0x06003449 RID: 13385 RVA: 0x000F90F0 File Offset: 0x000F74F0
		private IEnumerator ShowLoadingScreenOverlay(LoadingScreenConfig config)
		{
			this.loadingBar.SetActive(true);
			this.logo.SetActive(true);
			if (config != LoadingScreenConfig.None)
			{
				yield return this.LoadTransitionForeground(config);
			}
			else
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x0600344A RID: 13386 RVA: 0x000F9114 File Offset: 0x000F7514
		private bool CanShow(LoadingScreenInfo loadingScreen)
		{
			bool flag = true;
			if (!string.IsNullOrEmpty(loadingScreen.season))
			{
				// SeasonConfig loadingScreenSeason = this.seasonService.GetLoadingScreenSeason(loadingScreen.season);
				SeasonConfig loadingScreenSeason = null;
				flag &= (loadingScreenSeason != null);
			}
			return flag && this.gameStateService != null && LoadingScreenConditionEvaluator.Evaluate(loadingScreen.condition, this.gameStateService);
		}

		// Token: 0x0600344B RID: 13387 RVA: 0x000F9174 File Offset: 0x000F7574
		private IEnumerator GetAvailableScreenIDs(LoadingScreenConfig config)
		{
			if (config == LoadingScreenConfig.None || this.bundledScreenConfigs == null || this.bundledScreenConfigs.Count < 1)
			{
				WoogaDebug.Log(new object[]
				{
					"Bundles: no screen configs to load."
				});
				yield return null;
			}
			else
			{
				List<LoadingScreenInfo> availableScreens = new List<LoadingScreenInfo>();
				Dictionary<string, bool> bundleStatusByID = new Dictionary<string, bool>();
				foreach (LoadingScreenInfo screenConfig in this.bundledScreenConfigs)
				{
					if (!bundleStatusByID.ContainsKey(screenConfig.assetBundle))
					{
						Wooroutine<bool> isCachedRoutine = this.abs.IsBundleCached(screenConfig.assetBundle);
						yield return isCachedRoutine.StartAndAwait();
						bool isCached = false;
						try
						{
							isCached = isCachedRoutine.ReturnValue;
						}
						catch (Exception ex)
						{
							WoogaDebug.Log(new object[]
							{
								ex.Message
							});
						}
						bundleStatusByID[screenConfig.assetBundle] = isCached;
					}
					if (bundleStatusByID[screenConfig.assetBundle] && this.CanShow(screenConfig))
					{
						availableScreens.Add(screenConfig);
					}
				}
				if (availableScreens.Count < 1)
				{
					WoogaDebug.Log(new object[]
					{
						"Bundles: No screens available."
					});
					yield return null;
				}
				else
				{
					yield return availableScreens;
				}
			}
			yield break;
		}

		// Token: 0x0600344C RID: 13388 RVA: 0x000F9198 File Offset: 0x000F7598
		private IEnumerator LoadTransitionForeground(LoadingScreenConfig config)
		{
			Wooroutine<List<LoadingScreenInfo>> idLoaderRoutine = WooroutineRunner.StartWooroutine<List<LoadingScreenInfo>>(this.GetAvailableScreenIDs(config));
			yield return idLoaderRoutine;
			bool couldLoadAvailableScreenIDs = true;
			List<LoadingScreenInfo> availableLoadingScreens = null;
			try
			{
				availableLoadingScreens = idLoaderRoutine.ReturnValue;
			}
			catch (Exception ex)
			{
				WoogaDebug.LogFormatted("Bundles: {0}", new object[]
				{
					ex.Message
				});
				couldLoadAvailableScreenIDs = false;
			}
			if (!couldLoadAvailableScreenIDs || availableLoadingScreens == null)
			{
				if (!couldLoadAvailableScreenIDs)
				{
					WoogaDebug.Log(new object[]
					{
						"Bundles: couldn't load available screen IDs"
					});
				}
				else
				{
					WoogaDebug.Log(new object[]
					{
						"Bundles: availableLoadingScreens == null"
					});
				}
				yield break;
			}
			int randomIndex = global::UnityEngine.Random.Range(0, availableLoadingScreens.Count);
			string fullPath = availableLoadingScreens[randomIndex].GetFullPath();
			Wooroutine<Sprite> screenLoader = this.abs.LoadAsset<Sprite>(availableLoadingScreens[randomIndex].assetBundle, fullPath);
			yield return screenLoader;
			Sprite screenSprite = null;
			try
			{
				screenSprite = screenLoader.ReturnValue;
			}
			catch (Exception ex2)
			{
				WoogaDebug.LogWarning(new object[]
				{
					string.Format("Bundles: Couldn't load screen {0}; {1}", fullPath, ex2.Message)
				});
			}
			if (screenSprite)
			{
				this.transitionForeground.sprite = screenSprite;
				this.transitionForeground.color = new Color(1f, 1f, 1f, 0f);
				this.transitionForeground.gameObject.SetActive(true);
				this.transitionForeground.DOFade(1f, 0.25f);
				yield return new WaitForSeconds(0.15f);
			}
			yield return null;
			yield break;
		}

		// Token: 0x0600344D RID: 13389 RVA: 0x000F91BA File Offset: 0x000F75BA
		private void HideLoadingScreenOverlay()
		{
			this.transitionForeground.gameObject.SetActive(false);
			this.loadingBar.SetActive(false);
			this.logo.SetActive(false);
			this.backgroundCoverImage.gameObject.SetActive(false);
		}

		// Token: 0x0600344E RID: 13390 RVA: 0x000F91F8 File Offset: 0x000F75F8
		public IEnumerator ShowTransitionEndAnimation()
		{
			this.HideLoadingScreenOverlay();
			this.transitionAnimator.SetBool("Closed", false);
			yield return new WaitForSeconds(this.WaitForAnimationLengthInSeconds());
			yield break;
		}

		// Token: 0x0600344F RID: 13391 RVA: 0x000F9214 File Offset: 0x000F7614
		protected float WaitForAnimationLengthInSeconds()
		{
			AnimatorClipInfo[] currentAnimatorClipInfo = this.transitionAnimator.GetCurrentAnimatorClipInfo(0);
			return (float)currentAnimatorClipInfo.Length;
		}

		// Token: 0x06003450 RID: 13392 RVA: 0x000F9234 File Offset: 0x000F7634
		protected void Setup(bool showTransitionAnimation)
		{
			this.inAnimatedTransition = showTransitionAnimation;
			this.transitionDisplay.SetActive(this.inAnimatedTransition);
			this.glowGameObject.SetActive(false);
			this.logo.SetActive(!this.inAnimatedTransition);
			this.staticBackground.gameObject.SetActive(!this.inAnimatedTransition);
			this.loadingBar.SetActive(!this.inAnimatedTransition);
			this.backgroundCoverImage.gameObject.SetActive(false);
			this.transitionForeground.gameObject.SetActive(false);
		}

		// Token: 0x06003451 RID: 13393 RVA: 0x000F92C8 File Offset: 0x000F76C8
		private LoadingScreenInfo GetSeasonalLoadingScreen()
		{
			// SeasonConfig activeSeason = this.seasonService.GetActiveSeason();
			// if (activeSeason == null)
			// {
			// 	return null;
			// }
			return new LoadingScreenInfo
			{
				// id = "loadingscreen-" + activeSeason.Primary,
				// season = activeSeason.Primary,
				// condition = "unlock_level_22",
				// assetBundle = activeSeason.PrimaryBundleName
			};
		}

		// Token: 0x04005C13 RID: 23571
		public const bool SHOW_LOGO_OVER_ANIMATED_BACKGROUND = true;

		// Token: 0x04005C14 RID: 23572
		public const bool SHOW_LOADING_BAR_OVER_ANIMATED_BACKGROUND = true;

		// Token: 0x04005C15 RID: 23573
		public const float BG_COVER_FADE_DURATION_SECONDS = 0.2f;

		// Token: 0x04005C16 RID: 23574
		public const float HIDE_ANIM_DURATION_SECONDS = 0.4f;

		// Token: 0x04005C17 RID: 23575
		public const float TRANSITION_FOREGROUND_FADE_IN_SECONDS = 0.25f;

		// Token: 0x04005C18 RID: 23576
		public const string LOADING_SCREEN_PREFIX = "loadingscreen-";

		// Token: 0x04005C19 RID: 23577
		[SerializeField]
		private TextMeshProUGUI userIdLabel;

		// Token: 0x04005C1A RID: 23578
		[SerializeField]
		private GameObject logoGameObject;

		// Token: 0x04005C1B RID: 23579
		[SerializeField]
		private GameObject glowGameObject;

		// Token: 0x04005C1C RID: 23580
		public static LoadingScreenRoot instance;

		// Token: 0x04005C1D RID: 23581
		[WaitForService(true, true)]
		private ConfigService configsService;

		// Token: 0x04005C1E RID: 23582
		// [WaitForService(true, true)]
		// private SeasonService seasonService;

		// Token: 0x04005C1F RID: 23583
		// [WaitForService(true, true)]
		// private TrackingService tracking;

		// Token: 0x04005C20 RID: 23584
		[WaitForService(true, true)]
		private AssetBundleService abs;

		// Token: 0x04005C21 RID: 23585
		[WaitForService(true, true)]
		private SBSService sbsService;

		// Token: 0x04005C22 RID: 23586
		[WaitForService(false, false)]
		private GameStateService gameStateService;

		// Token: 0x04005C23 RID: 23587
		// [WaitForService(false, false)]
		// private GiftLinksService giftLinksService;

		// Token: 0x04005C24 RID: 23588
		public Animator transitionAnimator;

		// Token: 0x04005C25 RID: 23589
		public GameObject transitionDisplay;

		// Token: 0x04005C26 RID: 23590
		public Image staticBackground;

		// Token: 0x04005C27 RID: 23591
		public Image transitionForeground;

		// Token: 0x04005C28 RID: 23592
		public GameObject logo;

		// Token: 0x04005C29 RID: 23593
		public GameObject loadingBar;

		// Token: 0x04005C2A RID: 23594
		public Image backgroundCoverImage;

		// Token: 0x04005C2B RID: 23595
		public TextMeshProUGUI labelProgress;

		// Token: 0x04005C2C RID: 23596
		[HideInInspector]
		public bool inAnimatedTransition;

		// Token: 0x04005C2D RID: 23597
		[HideInInspector]
		public bool hideAutomatically;

		// Token: 0x04005C2E RID: 23598
		private List<LoadingScreenInfo> bundledScreenConfigs;
	}
}
