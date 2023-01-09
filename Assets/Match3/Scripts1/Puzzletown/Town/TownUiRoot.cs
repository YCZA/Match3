using System;
using System.Collections;
using Match3.Scripts1.Puzzletown.Flows;
using Match3.Scripts1.Puzzletown.Match3;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Puzzletown.UI;
using Match3.Scripts1.Shared.Flows;
using Wooga.Coroutines;
using Wooga.UnityFramework;
using Match3.Scripts2.Building.Shop;
using Match3.Scripts2.PlayerData;
using Match3.Scripts2.Shop;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Town
{
	// Token: 0x02000A27 RID: 2599
	public class TownUiRoot : ASceneRoot
	{
		// Token: 0x17000930 RID: 2352
		// (get) Token: 0x06003E62 RID: 15970 RVA: 0x0013C834 File Offset: 0x0013AC34
		public TownBottomPanelRoot BottomPanel
		{
			get
			{
				return this.bottomPanel;
			}
		}

		// Token: 0x17000931 RID: 2353
		// (get) Token: 0x06003E63 RID: 15971 RVA: 0x0013C83C File Offset: 0x0013AC3C
		public TownResourcePanelRoot ResourcePanel
		{
			get
			{
				return this.resourcePanel;
			}
		}

		// Token: 0x17000932 RID: 2354
		// (get) Token: 0x06003E64 RID: 15972 RVA: 0x0013C844 File Offset: 0x0013AC44
		public TownShopRoot ShopDialog
		{
			get
			{
				return this.shopPanel;
			}
		}

		// Token: 0x17000933 RID: 2355
		// (get) Token: 0x06003E65 RID: 15973 RVA: 0x0013C84C File Offset: 0x0013AC4C
		public TownOptionsRoot OptionsMenu
		{
			get
			{
				return this.options;
			}
		}

		// Token: 0x06003E66 RID: 15974 RVA: 0x0013C854 File Offset: 0x0013AC54
		public void SetUpBuildingsController(BuildingsController controller)
		{
			this.buildings = controller;
			this.buildings.onBuildingStored.AddListener(new Action<BuildingConfig, Vector3>(this.bottomPanel.HandleBuildingStored));
		}

		// Token: 0x06003E67 RID: 15975 RVA: 0x0013C87E File Offset: 0x0013AC7E
		public void AllowUIInteraction(bool state)
		{
			if (this.bottomPanel != null)
			{
				this.bottomPanel.IsInteractable = state;
			}
		}

		// Token: 0x06003E68 RID: 15976 RVA: 0x0013C89D File Offset: 0x0013AC9D
		private void StartJourney(AFlow newJourney)
		{
			if (this.currentJourney != null)
			{
				return;
			}
			this.currentJourney = newJourney;
			WooroutineRunner.StartCoroutine(this.runCurrentJourney(), null);
		}
		
		// Token: 0x06003E69 RID: 15977 RVA: 0x0013C8C0 File Offset: 0x0013ACC0
		private IEnumerator runCurrentJourney()
		{
			yield return this.currentJourney.Start();
			this.currentJourney = null;
			yield break;
		}

		// Token: 0x06003E6A RID: 15978 RVA: 0x0013C8DB File Offset: 0x0013ACDB
		private void showRankCelebration(VillageRankHarmonyObserver observer)
		{
			BlockerManager.global.Append(new SceneLoaderFlow<PopupRankUpRoot, VillageRank>(observer.CurrentRankData, true, 1f));
		}

		// Token: 0x06003E6B RID: 15979 RVA: 0x0013C8F8 File Offset: 0x0013ACF8
		private void OnApplicationPause(bool pauseStatus)
		{
			if (!pauseStatus && this.stateService != null)
			{
				this.stateService.SaveVillageRank();
			}
		}

		// Token: 0x06003E6C RID: 15980 RVA: 0x0013C918 File Offset: 0x0013AD18
		protected override void Go()
		{
			this.resourcePanel.Show(this.bottomPanel.multipleHideThis[0] as Canvas, TownResourceElementDefaults.TownRootDefault, this.dailyDealsService.ShouldShowDailyDealPopout());
			this.villageRankObserver = new VillageRankHarmonyObserver(this.configService.SbsConfig.villagerankconfig, this.stateService.Resources);
			this.villageRankObserver.OnVillageRankChanged.AddListener(new Action<VillageRankHarmonyObserver>(this.showRankCelebration));
			this.villageRankObserver.OnVillageRankChanged.AddListener(delegate(VillageRankHarmonyObserver r)
			{
				this.stateService.SaveVillageRank();
			});
			this.resourcePanel.onPurchaseDiamonds.AddListener(delegate
			{
				this.StartJourney(new PurchaseDiamondsJourney(new TownDiamondsPanelRoot.TownDiamondsPanelRootParameters
				{
					source1 = "diamonds_hud",
					source2 = "island"
				}, false));
			});
			this.resourcePanel.onPurchaseLives.AddListener(delegate
			{
				this.StartJourney(new GetLivesJourneyTown());
			});
			this.resourcePanel.SetHarmonyObserver(this.villageRankObserver);
			this.options.SetHarmonyObserver(this.villageRankObserver);
			this.bottomPanel.onOptionsTabClick.AddListener(new Action<TownOptionsCommand>(this.HandleBottomPanelOperation));
			this.ShopDialog.onClose.AddListener(new Action(this.RefreshNewBuildingIndicator));
			this.RefreshNewBuildingIndicator();
			this.quests.questManager.OnQuestCollected.AddListener(new Action<QuestProgress>(this.HandleQuestCollected));
			this.progression.onLevelUnlocked.AddListener(new Action<int>(this.HandleLevelUnlocked));
		}

		// Token: 0x06003E6D RID: 15981 RVA: 0x0013CA8C File Offset: 0x0013AE8C
		private void HandleBottomPanelOperation(TownOptionsCommand op)
		{
			IEnumerator enumerator = null;
			switch (op)
			{
			case TownOptionsCommand.Settings:
			case TownOptionsCommand.Friends:
			case TownOptionsCommand.Inbox:
				this.options.Open(op);
				break;
			case TownOptionsCommand.Shop:
				if (BuildingLocation.Selected == null)
				{
					this.ShopDialog.Open();
				}
				break;
			case TownOptionsCommand.LoginFacebook:
				enumerator = this.LoadSceneCheckForInternet<PopupFacebookConnectRoot>();
				break;
			default:
				switch (op)
				{
				case TownOptionsCommand.Challenges:
					if (this.progression.UnlockedLevel < this.challengeService.Balancing.play_minimum_level)
					{
						enumerator = SceneManager.Instance.LoadScene<ChallengeTeaserV2Root>(null);
					}
					else if (this.challengeService.IsChallengeBundleAvailable())
					{
						enumerator = SceneManager.Instance.LoadScene<ChallengeV2Root>(null);
					}
					break;
				case TownOptionsCommand.Bank:
					if (Application.internetReachability != NetworkReachability.NotReachable)
					{
						enumerator = SceneManager.Instance.LoadSceneWithParams<BankRoot, BankRoot.BankContext>(BankRoot.BankContext.island, null);
					}
					else
					{
						// enumerator = SceneManager.Instance.LoadSceneWithParams<PopupFacebookRoot, FacebookPopupParameters>(new FacebookPopupParameters(FacebookPopupState.NoConnection, FacebookLoginContext.settings), null);
						StartCoroutine(PopupMissingAssetsRoot.TryShowRoutine(""));
					}
					break;
				case TownOptionsCommand.LevelOfDay:
					enumerator = this.StartNewLevelOfDayFlow();
					break;
				case TownOptionsCommand.Tropicam:
					enumerator = new TropicamFlow(this.bottomPanel, this.progression).Start();
					break;
				case TownOptionsCommand.IslandSwitch:
					this.bottomPanel.AnimatedClouds(true);
					enumerator = SceneManager.Instance.LoadSceneWithParams<WorldMapRoot, int>(-1, null);
					break;
				default:
					switch (op)
					{
					case TownOptionsCommand.Play:
						Debug.Log("eventsystem: on click play level");
						enumerator = new CoreGameFlow().Start(default(CoreGameFlow.Input));
						break;
					case TownOptionsCommand.ShowQuests:
						enumerator = new ShowQuestPopupFlow().Start<object>();
						break;
					case TownOptionsCommand.Wheel:
						enumerator = this.LoadSceneCheckForInternet<WheelRoot>();
						break;
					case TownOptionsCommand.StarterPack:
						// enumerator = this.LoadSceneCheckForInternet<StarterPackRoot>();
						// 改为商店中的powerbundle
						var newJourney = new PurchaseDiamondsJourney(new TownDiamondsPanelRoot.TownDiamondsPanelRootParameters
						{
							source1 = "diamonds_hud",
							source2 = "island",
							storeType = TownDiamondsPanelRoot.StoreType.StarterPack
						}, false);
						
						if (this.currentJourney != null)
						{
							break;
						}
						
						this.currentJourney = newJourney;
						enumerator = this.runCurrentJourney();
						break;
					}
					break;
				}
				break;
			}
			if (enumerator != null)
			{
				this.AllowUIInteraction(false);
				// 启动新场景
				IEnumerator routine = enumerator.ContinueWith(delegate()
				{
					// 新场景关闭后执行这里
					this.AllowUIInteraction(true);
				});
				WooroutineRunner.StartCoroutine(routine, null);
			}
		}

		// Token: 0x06003E6E RID: 15982 RVA: 0x0013CC64 File Offset: 0x0013B064
		private IEnumerator StartNewLevelOfDayFlow()
		{
			this.BottomPanel.levelOfDayPlayButton.ShowNotificationIcon(false);
			if (this.sbsService.SbsConfig.feature_switches.level_of_day_streak)
			{
				return SceneManager.Instance.LoadSceneWithParams<M3_LevelOfDaySelectionRoot, bool>(false, null);
			}
			return new CoreGameFlow().Start(new CoreGameFlow.Input(0, false, null, LevelPlayMode.LevelOfTheDay));
		}

		// Token: 0x06003E6F RID: 15983 RVA: 0x0013CCBC File Offset: 0x0013B0BC
		private IEnumerator LoadSceneCheckForInternet<T>() where T : ASceneRoot
		{
			if (Application.internetReachability != NetworkReachability.NotReachable)
			{
				return SceneManager.Instance.LoadScene<T>(null);
			}
			// return SceneManager.Instance.LoadSceneWithParams<PopupFacebookRoot, FacebookPopupParameters>(new FacebookPopupParameters(FacebookPopupState.NoConnection, FacebookLoginContext.settings), null);
			return PopupMissingAssetsRoot.TryShowRoutine("");
		}

		// Token: 0x06003E70 RID: 15984 RVA: 0x0013CCE6 File Offset: 0x0013B0E6
		private void HandleLevelUnlocked(int level)
		{
			this.RefreshNewBuildingIndicator();
		}

		// Token: 0x06003E71 RID: 15985 RVA: 0x0013CCEE File Offset: 0x0013B0EE
		private void HandleQuestCollected(QuestProgress progress)
		{
			this.RefreshNewBuildingIndicator();
		}

		// Token: 0x06003E72 RID: 15986 RVA: 0x0013CCF8 File Offset: 0x0013B0F8
		public void RefreshNewBuildingIndicator()
		{
			int num = 0;
			foreach (ShopTag selected in this.ShopDialog.shopTabs)
			{
				num += this.ShopDialog.GetBuildingConfigsByStoreTag(selected).CountIf(new Func<BuildingConfig, bool>(this.shopPanel.IsBuildingNewAndAvailable));
			}
			if (num > 0)
			{
				// 审核模式隐藏建筑商店的uiIndicator(显示新物品数量的红点提示)
				// #if REVIEW_VERSION
				// {
					// this.bottomPanel.newBuildingsIndicator.Hide();
				// }
				// #else
				// {
					this.bottomPanel.newBuildingsIndicator.Show(num.ToString());
				// }
				// #endif
			}
			else
			{
				this.bottomPanel.newBuildingsIndicator.Hide();
			}
		}

		// Token: 0x06003E73 RID: 15987 RVA: 0x0013CD8C File Offset: 0x0013B18C
		public float CollectMaterials(MaterialAmount mat, Transform source, bool allowResources = true)
		{
			string type = mat.type;
			switch (type)
			{
			case "lives":
			case "coins":
			case "harmony":
			case "diamonds":
				this.resourcePanel.CollectMaterials(mat, source, allowResources);
				goto IL_12E;
			case "season_currency":
				if (this.doobers != null)
				{
					return this.doobers.SpawnDoobers(mat, source, this.bottomPanel.shop, null);
				}
				return 0f;
			case "UnlimitedLives":
			case "lives_unlimited":
				return 0f;
			}
			if (mat.type.StartsWith("boost_"))
			{
				return 0f;
			}
			BlockerManager.global.Append(new SceneLoaderFlow<BannerNewCollectableRoot, string>(mat.type, false, 0f));
			IL_12E:
			return 0f;
		}

		// Token: 0x06003E74 RID: 15988 RVA: 0x0013CECC File Offset: 0x0013B2CC
		protected override void OnDestroy()
		{
			base.OnDestroy();
			if (this.villageRankObserver != null)
			{
				this.villageRankObserver.OnDestroy();
			}
			if (this.quests != null && this.quests.questManager != null)
			{
				this.quests.questManager.OnQuestCollected.RemoveListener(new Action<QuestProgress>(this.HandleQuestCollected));
			}
			if (this.progression != null)
			{
				this.progression.onLevelUnlocked.RemoveListener(new Action<int>(this.HandleLevelUnlocked));
			}
			if (this.buildings != null)
			{
				this.buildings.onBuildingStored.RemoveListener(new Action<BuildingConfig, Vector3>(this.bottomPanel.HandleBuildingStored));
			}
		}

		// Token: 0x06003E75 RID: 15989 RVA: 0x0013CF84 File Offset: 0x0013B384
		public void ShowUi(bool show)
		{
			if (!show)
			{
				this.ResourcePanel.Disable();
				this.bottomPanel.Disable();
				this.ShopDialog.Disable();
				this.OptionsMenu.Disable();
			}
			else
			{
				this.ResourcePanel.Enable();
				this.bottomPanel.Enable();
			}
		}

		// Token: 0x04006773 RID: 26483
		[WaitForRoot(true, true)]
		private TownBottomPanelRoot bottomPanel;

		// Token: 0x04006774 RID: 26484
		[WaitForRoot(false, true)]
		private TownResourcePanelRoot resourcePanel;

		// Token: 0x04006775 RID: 26485
		[WaitForRoot(true, true)]
		private TownShopRoot shopPanel;

		// Token: 0x04006776 RID: 26486
		[WaitForService(true, true)]
		private QuestService quests;

		// Token: 0x04006777 RID: 26487
		[WaitForService(true, true)]
		private ProgressionDataService.Service progression;

		// Token: 0x04006778 RID: 26488
		[WaitForService(true, true)]
		private AudioService audioService;

		// Token: 0x04006779 RID: 26489
		[WaitForService(true, true)]
		private GameStateService stateService;

		// Token: 0x0400677A RID: 26490
		[WaitForService(true, true)]
		private ConfigService configService;

		// Token: 0x0400677B RID: 26491
		[WaitForService(true, true)]
		private TrackingService trackingService;

		// Token: 0x0400677C RID: 26492
		[WaitForService(true, true)]
		private SBSService sbsService;

		// Token: 0x0400677D RID: 26493
		[WaitForService(true, true)]
		private DailyDealsService dailyDealsService;

		// Token: 0x0400677E RID: 26494
		[WaitForService(true, true)]
		private ChallengeService challengeService;

		// Token: 0x0400677F RID: 26495
		[WaitForRoot(false, false)]
		private DoobersRoot doobers;

		// Token: 0x04006780 RID: 26496
		[WaitForRoot(false, false)]
		private TownOptionsRoot options;

		// Token: 0x04006781 RID: 26497
		[WaitForRoot(false, false)]
		private EventSystemRoot eventRoot;

		// Token: 0x04006782 RID: 26498
		[WaitForRoot(false, false)]
		private TownEnvironmentRoot env;

		// Token: 0x04006783 RID: 26499
		public BuildingsController buildings;

		// Token: 0x04006784 RID: 26500
		private VillageRankHarmonyObserver villageRankObserver;

		// Token: 0x04006785 RID: 26501
		private AFlow currentJourney;
	}
}
