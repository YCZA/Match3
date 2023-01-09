using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Match3.Scripts1.Audio;
using Match3.Scripts1.Audio.Scripts;
using Match3.Scripts1.Puzzletown.Features.DailyGifts;
using Match3.Scripts1.Puzzletown.Match3;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Puzzletown.Tutorials;
using Match3.Scripts1.Puzzletown.UI;
using Match3.Scripts1.Shared.UI;
using Match3.Scripts1.Town;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.Services.ErrorAnalytics;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;
using Match3.Scripts2.Shop;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Town
{
	// Token: 0x0200094C RID: 2380
	[LoadOptions(false, false, true)]
	[RequireComponent(typeof(TownSceneLoader))]
	[RequireComponent(typeof(MusicPlayer))]
	[RequireComponent(typeof(MusicLibrary))]
	public class TownMainRoot : APtSceneRoot
	{
		// Token: 0x170008DF RID: 2271
		// (get) Token: 0x060039D1 RID: 14801 RVA: 0x0011C4EE File Offset: 0x0011A8EE
		// (set) Token: 0x060039D2 RID: 14802 RVA: 0x0011C4F6 File Offset: 0x0011A8F6
		public bool initialLoad { get; set; }

		// Token: 0x170008E0 RID: 2272
		// (get) Token: 0x060039D3 RID: 14803 RVA: 0x0011C4FF File Offset: 0x0011A8FF
		public bool IsTutorialRunning
		{
			get
			{
				return this.tutorialRunner != null && this.tutorialRunner.IsRunning;
			}
		}

		// Token: 0x060039D4 RID: 14804 RVA: 0x0011C520 File Offset: 0x0011A920
		public void PopulateArea(int area, bool attemptingRepair)
		{
			this.townLoader.CreateAreaBuildings(area, attemptingRepair);
		}

		// Token: 0x060039D5 RID: 14805 RVA: 0x0011C52F File Offset: 0x0011A92F
		public static string GetSceneNameForIsland(int island)
		{
			return "TownEnvironment_" + island;
		}

		// Token: 0x060039D6 RID: 14806 RVA: 0x0011C541 File Offset: 0x0011A941
		public static Wooroutine<TownMainRoot> Load(int island, LoadOptions options = null)
		{
			SceneManager.Instance.MapTypeToSceneName(typeof(TownEnvironmentRoot), TownMainRoot.GetSceneNameForIsland(island));
			return SceneManager.Instance.LoadScene<TownMainRoot>(options);
		}

		// Token: 0x060039D7 RID: 14807 RVA: 0x0011C568 File Offset: 0x0011A968
		protected override void Awake()
		{
			base.Awake();
			this.cameraPan.gameObject.SetActive(false);
		}

		// Token: 0x060039D8 RID: 14808 RVA: 0x0011C584 File Offset: 0x0011A984
		protected override void Go()
		{
			this.DisableUserInput();
			this.townLoader = base.GetComponentInChildren<TownSceneLoader>(true);
			if (this.stateService.Debug.ShowCheatMenus)
			{
				SceneManager.Instance.LoadSceneWithParams<TownCheatsRoot, TownSceneLoader>(this.townLoader, null);
			}
			this.initialLoad = this.stateService.IsEmptyState();
			this.cameraInputController.cameraBounds = this.env.cameraBounds;
			this.cameraPan.villagers = this.villagers;
			this.env.cameraPan = this.cameraPan;
			this.townLoader.Init(this.stateService, this.configService, this.resourceService, this.overheadUi, this.questService, this.loc, this.ProgressionService, this.env, this.tracking, this.uiRoot, this.audioService, this.seasonService);
			this.uiRoot.ShopDialog.onBuildingClick.AddListener(new Action<BuildingShopView.BuildingBuildRequest>(this.townLoader.PlaceBuilding));
			this.uiRoot.SetUpBuildingsController(this.townLoader.buildingServices.BuildingsController);
			this.townLoader.buildingServices.BuildingsController.onPurchaseCancelled.AddListener(new Action(this.uiRoot.ShopDialog.OnBuildingPurchaseCancelled));
			this.townLoader.buildingServices.BuildingsController.onBuildingComplete.AddListener(new Action<BuildingInstance>(this.OnBuildingComplete));
			this.initialLoad = this.stateService.IsEmptyState();
			this.townLoader.buildingServices.BuildingsController.onBuildingHarvest.AddListener(new Action<BuildingInstance, MaterialAmount>(this.OnBuildingHarvest));
			this.townLoader.buildingServices.BuildingsController.onBuildingHarvest.AddListener(delegate(BuildingInstance instance, MaterialAmount amount)
			{
				this.tracking.TrackBuildingHarvest(instance, amount);
			});
			this.townLoader.positionHelper.controlPanel = this.uiRoot.BottomPanel.controlButtons;
			this.townLoader.positionHelper.configService = this.configService;
			this.villagers.Init();
			this.questActionHandlerManager = new QuestActionHandlerManager();
			this.questActionHandlerManager.RegisterHandler(new RepairBuildingsByTagAction(this.BuildingsController, this.uiRoot));
			this.questService.questManager.OnQuestComplete.AddListener(new Action<QuestProgress>(this.HandleQuestComplete));
			this.questService.questManager.OnQuestActionTriggers.AddListener(new Action<IQuestAction>(this.HandleQuestAction));
			this.PopulateAreas();
			townLoader.buildingServices.BuildingsController.RefreshMap();	// 初始建筑生成后，再刷新一遍地图
			this.InitAudio(this.stateService.Buildings.CurrentIsland);
			if (!this.stateService.isInteractable)
			{
				this.uiRoot.BottomPanel.Disable();
				this.uiRoot.ResourcePanel.Disable();
				this.overheadUi.canvas.enabled = false;
				base.StartCoroutine(this.WaitForVisitFriendRoot());
			}
			this.tracking.TrackFunnelEvent("020_loading_end", 20, null);
			BackButtonManager.Instance.AddAction(new Action(this.HandleBackButtonPressed));
			this.preloadService.Preload(null);
			this.session.NumberOfIslandLoads++;
			this.env.shadowsCamera.Render();
		}

		// Token: 0x060039D9 RID: 14809 RVA: 0x0011C8C8 File Offset: 0x0011ACC8
		public void StartView(bool tryStartNewContent = true, bool forceNewAreaBanner = false)
		{
			this.cameraPan.gameObject.SetActive(true);
			if (this.stateService.IsMyOwnState)
			{
				if (this.sbs.CheckForForcedAbTests(this.stateService.Debug.ForcedAbTests))
				{
					return;
				}
				this.DisableUserInput();
				if (tryStartNewContent)
				{
					BlockerManager.global.Append(new StartNewContentFlow(forceNewAreaBanner, false));
				}
				base.StartCoroutine(this.WaitForTutorialRunner());
				this.helpShiftService.UpdateMetaData();
				this.helpShiftService.UpdatePushData();
			}
			else
			{
				this.EnableUserInput();
			}
			this.pushNotificationService.SendFriendProgressNotifications();
			this.env.shadowsCamera.Render();
		}

		// Token: 0x060039DA RID: 14810 RVA: 0x0011C980 File Offset: 0x0011AD80
		private void PopulateAreas()
		{
			this.CleanUpAreaInfo();
			IEnumerable<int> source = this.configService.SbsConfig.islandareaconfig.GlobalAreasOnIsland(this.stateService.Buildings.CurrentIsland);
			int unlockedArea = this.questService.UnlockedAreaWithQuestAndEndOfContent;
			IEnumerable<int> enumerable = from a in source
			where a <= unlockedArea
			select a;
			foreach (int area in enumerable)
			{
				bool attemptingRepair = this.stateService.Buildings.IsAreaDeployed(area);
				this.PopulateArea(area, attemptingRepair);
			}
		}

		// Token: 0x060039DB RID: 14811 RVA: 0x0011CA44 File Offset: 0x0011AE44
		private IEnumerator PreLoadQuestAndLevelRoots()
		{
			this.collectables.Initialize();
			Wooroutine<QuestsPopupRoot> questsRootLoadRoutine = SceneManager.Instance.LoadScene<QuestsPopupRoot>(null);
			yield return questsRootLoadRoutine;
			questsRootLoadRoutine.ReturnValue.dialog.Hide();
			Wooroutine<M3_LevelSelectionUiRoot> levelRootLoadRoutine = SceneManager.Instance.LoadScene<M3_LevelSelectionUiRoot>(null);
			yield return levelRootLoadRoutine;
			levelRootLoadRoutine.ReturnValue.dialog.Hide();
			Debug.Log("PreLoadQuestAndLevelRoots End");
		}

		// Token: 0x060039DC RID: 14812 RVA: 0x0011CA60 File Offset: 0x0011AE60
		private void CleanUpAreaInfo()
		{
			int num = 0;
			foreach (QuestData questData in this.configService.SbsConfig.questconfig.quests)
			{
				foreach (QuestTaskData questTaskData in questData.Tasks)
				{
					if (questData.level > this.ProgressionService.UnlockedLevel)
					{
						break;
					}
					if (questTaskData.type == QuestTaskType.collect_and_repair && questTaskData.action_target.EndsWith("rubble"))
					{
						bool flag = this.questService.IsCollected(questData.id) || (this.questService.questManager.CurrentQuestData != null && this.questService.questManager.CurrentQuestData.id == questData.id && this.questService.questManager.GetTaskCollected(this.questService.questManager.CurrentQuestProgress, questTaskData.index));
						if (flag)
						{
							num++;
						}
					}
				}
			}
			int num2 = 0;
			if (num > 0)
			{
				int rubbleIdForOrder = this.configService.SbsConfig.islandareaconfig.GetRubbleIdForOrder(num);
				num2 = this.configService.SbsConfig.islandareaconfig.LastAreaCoveredByThisRubble(rubbleIdForOrder);
			}
			if (this.ProgressionService.LastRubbleAreaCleared != num2)
			{
				if (this.ProgressionService.LastRubbleAreaCleared != 0)
				{
					Log.Warning("Cleanup index is not what is should be for area. Repairing", string.Format("Cleanup index, {0}, is not what it should be for area {1}. Island {2}. Repairing", this.ProgressionService.LastRubbleAreaCleared, num2, this.stateService.Buildings.CurrentIsland), null);
				}
				this.ProgressionService.LastRubbleAreaCleared = num2;
			}
			EAHelper.AddBreadcrumb(string.Format("Town loading - CleanUpAreaInfo repairing Area {0}, LastUnlock Area: {1}, UnlockedAreaWithQuestAndEndOfContent: {2}", this.sbs.SbsConfig.feature_switches.repair_areas, this.ProgressionService.LastUnlockedArea, this.questService.UnlockedAreaWithQuestAndEndOfContent));
			if (this.sbs.SbsConfig.feature_switches.repair_areas && this.ProgressionService.LastUnlockedArea != this.questService.UnlockedAreaWithQuestAndEndOfContent)
			{
				if (this.ProgressionService.LastUnlockedArea != 0)
				{
					Log.Warning("Last unlocked area is not correct. Repairing", string.Format("Last unlocked area, {0}, is not correct, should be {1}. Repairing", this.ProgressionService.LastUnlockedArea, this.questService.UnlockedAreaWithQuestAndEndOfContent), null);
				}
				this.ProgressionService.LastUnlockedArea = this.questService.UnlockedAreaWithQuestAndEndOfContent;
				EAHelper.AddBreadcrumb(string.Format("Area repaired {0}, LastUnlock Area {1} is set to UnlockedAreaWithQuestAndEndOfContent: {2}", this.sbs.SbsConfig.feature_switches.repair_areas, this.ProgressionService.LastUnlockedArea, this.questService.UnlockedAreaWithQuestAndEndOfContent));
			}
		}

		// Token: 0x060039DD RID: 14813 RVA: 0x0011CD7C File Offset: 0x0011B17C
		private IEnumerator WaitForVisitFriendRoot()
		{
			Wooroutine<VisitFriendsRoot> loadVisitFriendRoot = SceneManager.Instance.LoadScene<VisitFriendsRoot>(null);
			yield return loadVisitFriendRoot;
			this.visitFriendsRoot = loadVisitFriendRoot.ReturnValue;
			yield break;
		}

		// Token: 0x060039DE RID: 14814 RVA: 0x0011CD98 File Offset: 0x0011B198
		private IEnumerator WaitForTutorialRunner()
		{
			if (this.stateService.IsMyOwnState)
			{
				this.uiRoot.AllowUIInteraction(true);
				this.tutorialRunner = base.GetComponent<TutorialRunner>();
				this.tutorialRunner.Run();
				yield return this.tutorialRunner.onInitialized;
				if (!this.tutorialRunner.IsRunning)
				{
					yield return this.WaitForBlockers();
					this.EnableUserInput();
					this.SetupPopups();
					this.giftLinksService.ProcessGiftLinks();
				}
			}
			else
			{
				this.EnableUserInput();
			}
			yield break;
		}

		// Token: 0x060039DF RID: 14815 RVA: 0x0011CDB4 File Offset: 0x0011B1B4
		private IEnumerator WaitForBlockers()
		{
			while (BlockerManager.global.HasBlockers)
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x060039E0 RID: 14816 RVA: 0x0011CDC8 File Offset: 0x0011B1C8
		public void EnableUserInput()
		{
			if (this.eventSystem != null)
			{
				this.eventSystem.Enable();
			}
			BackButtonManager.Instance.SetEnabled(true);
			if (this.uiRoot != null)
			{
				this.uiRoot.AllowUIInteraction(true);
			}
		}
		
		// 禁用用户输入
		public void DisableUserInput()
		{
			this.eventSystem.Disable();
			BackButtonManager.Instance.SetEnabled(false);
			this.uiRoot.AllowUIInteraction(false);
		}

		// Token: 0x060039E2 RID: 14818 RVA: 0x0011CE40 File Offset: 0x0011B240
		private void SetupPopups()
		{
			this.popupManager = base.gameObject.AddComponent<PopupManager>();
			this.popupManager.AddTrigger(new TownMainTrackingTrigger(this.ProgressionService, this.tracking));
			this.popupManager.AddTrigger(new WorldMapRoot.Trigger(this.contentUnlockService, this.uiRoot.BottomPanel));
			this.popupManager.AddTrigger(new TownRewardsRoot.Trigger(this.stateService));
			this.popupManager.AddTrigger(new DailyGiftsRoot.Trigger(this.dailyGiftsService));
			this.popupManager.AddTrigger(new GrandPrizePurchaseFlow.Trigger(this.stateService, this.seasonService));
			this.popupManager.AddTrigger(new PopupFreeGiftRoot.Trigger(this.configService, this.loc, this.stateService, this.tracking, this.giftLinksService, this.session.NumTrackedSessions, this.ProgressionService.UnlockedLevel));
			this.popupManager.AddTrigger(new BankRoot.Trigger(this.ProgressionService, this.sbs, this.stateService, this.bankService, this.session.NumberOfIslandLoads));
			this.popupManager.AddTrigger(new PopupRatingRoot.Trigger(this.session, this.configService, this.ProgressionService, this.sbs, this.tracking));
			this.popupManager.AddTrigger(new PopupSeasonalPromoRoot.Trigger(this.stateService, this.seasonService, this.uiRoot.ShopDialog, this.resourceService));
			this.popupManager.AddTrigger(new PromoPopupTrigger(this.stateService, this.abs, this.sbs.SbsConfig, this.session, this.uiRoot.ShopDialog));
			this.popupManager.AddTrigger(new PopupMissingAssetsRoot.Trigger(this.stateService, this.resourceService));
			this.popupManager.AddTrigger(new MFSTrigger(this.configService, this.stateService, this.facebook));
			this.popupManager.AddTrigger(new PopupTournamentNewKindIntroductionRoot.Trigger(this.popupManager, this.stateService, this.tournamentService));
			this.popupManager.AddTrigger(new TournamentTeaserRoot.Trigger(this.tournamentService, this.configService, this.stateService, this.ProgressionService));
			this.popupManager.AddTrigger(new TournamentTrigger(this.tournamentService));
			this.popupManager.AddTrigger(new NewDecorationsAvailableTrigger(this.uiRoot.BottomPanel, this.questService, this.stateService));
			this.popupManager.AddTrigger(new PopupDailyDealOfferRoot.Trigger(this.ProgressionService, this.sbs, this.stateService, this.TownDiamondsPanelRoot, this.session.NumberOfIslandLoads));
			this.popupManager.AddTrigger(new SalePopupRoot.Trigger(this.saleService, this.session, this.stateService));
			this.popupManager.Run(this.uiRoot);
		}

		// Token: 0x060039E3 RID: 14819 RVA: 0x0011D11C File Offset: 0x0011B51C
		protected override IEnumerator GoRoutine()
		{
			yield return this.townLoader.onRequiredNumberOfBuildingsLoaded;
			yield return this.PreLoadQuestAndLevelRoots();
		}

		// Token: 0x060039E4 RID: 14820 RVA: 0x0011D138 File Offset: 0x0011B538
		protected override void OnDestroy()
		{
			if (this.popupManager != null)
			{
				this.popupManager.Stop();
			}
			base.OnDestroy();
			if (this.questService != null && this.questService.questManager != null)
			{
				this.questService.questManager.OnQuestComplete.RemoveListener(new Action<QuestProgress>(this.HandleQuestComplete));
				this.questService.questManager.OnQuestActionTriggers.RemoveListener(new Action<IQuestAction>(this.HandleQuestAction));
			}
		}

		// Token: 0x060039E5 RID: 14821 RVA: 0x0011D1C4 File Offset: 0x0011B5C4
		private void HandleQuestComplete(QuestProgress questProgress)
		{
			// blockermanager可能是为了在播放对话之后再执行claimQuestFlow
			BlockerManager.global.Append(new Func<IEnumerator>(this.ClaimQuestFlow), false);
		}

		// Token: 0x060039E6 RID: 14822 RVA: 0x0011D1E0 File Offset: 0x0011B5E0
		private IEnumerator ClaimQuestFlow()
		{
			this.uiRoot.Disable();
			if (SceneManager.IsLoadingScreenShown() || SceneManager.IsPlayingMatch3)
			{
				WoogaDebug.LogWarning(new object[]
				{
					"TownMainRoot: Trying to show quests popup while on m3"
				});
				yield break;
			}
			Wooroutine<QuestsPopupRoot> popup = SceneManager.Instance.LoadScene<QuestsPopupRoot>(null);
			yield return popup;
			// eli key point 一组任务完成时
			// yield return new WaitForSeconds(1);
			popup.ReturnValue.Show();
			yield return popup.ReturnValue.OnInitialized;
			popup.ReturnValue.HideCancel();
		}

		// Token: 0x060039E7 RID: 14823 RVA: 0x0011D1FB File Offset: 0x0011B5FB
		private void HandleQuestAction(IQuestAction currentAction)
		{
			if (this.questActionHandlerManager != null)
			{
				this.questActionHandlerManager.HandleAction(currentAction);
			}
		}

		// Token: 0x170008E1 RID: 2273
		// (get) Token: 0x060039E8 RID: 14824 RVA: 0x0011D214 File Offset: 0x0011B614
		public BuildingsController BuildingsController
		{
			get
			{
				return this.townLoader.buildingServices.BuildingsController;
			}
		}

		// Token: 0x060039E9 RID: 14825 RVA: 0x0011D228 File Offset: 0x0011B628
		private void OnBuildingComplete(BuildingInstance b)
		{
			if (this.uiRoot && b.view && !b.isFromStorage)
			{
				this.uiRoot.CollectMaterials(b.blueprint.Harmony, b.view.transform, true);
				this.uiRoot.CollectMaterials(b.blueprint.SeasonCurrency, b.view.transform, true);
			}
			if (this.audioService != null)
			{
				this.audioService.PlaySFX(AudioId.BuildingComplete, false, false, false);
			}
		}

		// Token: 0x060039EA RID: 14826 RVA: 0x0011D2C5 File Offset: 0x0011B6C5
		private void OnBuildingHarvest(BuildingInstance b, MaterialAmount mat)
		{
			this.uiRoot.CollectMaterials(mat, b.view.transform, true);
		}

		// Token: 0x060039EB RID: 14827 RVA: 0x0011D2E0 File Offset: 0x0011B6E0
		private void HandleBackButtonPressed()
		{
			if (this.stateService.isInteractable)
			{
				WooroutineRunner.StartCoroutine(this.QuitRoutine(), null);
				BackButtonManager.Instance.AddAction(new Action(this.HandleBackButtonPressed));
			}
			else
			{
				this.TryReloadTownWithAnimatedLoadingScreen();
			}
		}

		// Token: 0x060039EC RID: 14828 RVA: 0x0011D320 File Offset: 0x0011B720
		private void TryReloadTownWithAnimatedLoadingScreen()
		{
			if (this.visitFriendsRoot != null)
			{
				this.visitFriendsRoot.ReloadTownWithAnimatedLoadingScreen();
			}
			else
			{
				WoogaDebug.LogWarning(new object[]
				{
					"Can't return from friend visit because visitFriendsRoot is null."
				});
			}
		}

		// Token: 0x060039ED RID: 14829 RVA: 0x0011D358 File Offset: 0x0011B758
		private IEnumerator QuitRoutine()
		{
			string title = this.loc.GetText("ui.confirm.quit.title", new LocaParam[0]);
			string body = this.loc.GetText("ui.confirm.quit.body", new LocaParam[0]);
			PopupSortingOrder topLayer = new PopupSortingOrder(UILayer.Top);
			Wooroutine<bool> response = PopupDialogRoot.ShowYesNoDialog(title, body, false, topLayer);
			yield return response;
			if (response.ReturnValue)
			{
				// 退出时保存游戏
				// stateService.Save(true);
				// stateService.SaveVillageRank();
				if (Application.platform == RuntimePlatform.Android)
				{
					//AndroidJavaObject @static = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
					//@static.Call<bool>("moveTaskToBack", new object[]
					//{
					//	true
					//});
					Application.Quit();
				}
				else
				{
					Application.Quit();
				}
			}
			else
			{
				BackButtonManager.Instance.AddAction(new Action(this.HandleBackButtonPressed));
			}
			yield break;
		}

		// 暂停时保存游戏
		private void OnApplicationPause(bool pauseStatus)
		{
			// stateService.Save(true);
			Debug.Log("暂停时同步保存");
			// stateService.SaveSync();
		}

		public static bool saveGameOnQuit = true;	 // 如果要执行清除存档的操作，那么退出游戏时不保存
		// 退出时保存游戏, 退出游戏，使用同步保存的方式
		private void OnApplicationQuit()
		{
			if (saveGameOnQuit)
			{
				// stateService.SaveSync();
			}
		}

		// Token: 0x060039EE RID: 14830 RVA: 0x0011D374 File Offset: 0x0011B774
		public void SpawnRewardDoobers(IEnumerable<MaterialAmount> rewards)
		{
			foreach (MaterialAmount mat in rewards)
			{
				this.uiRoot.CollectMaterials(mat, null, false);
			}
		}

		// Token: 0x060039EF RID: 14831 RVA: 0x0011D3D0 File Offset: 0x0011B7D0
		private void InitAudio(int islandIndex)
		{
			this.audioService.LoadSettings();
			this.musicPlayer = base.GetComponent<MusicPlayer>();
			string sceneNameForIsland = TownMainRoot.GetSceneNameForIsland(islandIndex);
			MusicLibrary component = base.GetComponent<MusicLibrary>();
			MusicLibrary.MusicCollection collectionByName = component.GetCollectionByName(sceneNameForIsland);
			if (collectionByName != null)
			{
				this.musicPlayer.SetMusicTracks(collectionByName.musicTracks);
			}
			this.musicPlayer.Init(this.audioService);
		}

		// Token: 0x040061E3 RID: 25059
		[WaitForRoot(true, false)]
		private BuildingResourceServiceRoot resourceService;

		// Token: 0x040061E4 RID: 25060
		[WaitForRoot(true, false)]
		private TownDiamondsPanelRoot TownDiamondsPanelRoot;

		// Token: 0x040061E5 RID: 25061
		[WaitForRoot(true, true)]
		[NonSerialized]
		public TownUiRoot uiRoot;

		// Token: 0x040061E6 RID: 25062
		[WaitForRoot(true, false)]
		[NonSerialized]
		public TownEnvironmentRoot env;

		// Token: 0x040061E7 RID: 25063
		[WaitForRoot(false, false)]
		private EventSystemRoot eventSystem;

		// Token: 0x040061E8 RID: 25064
		[WaitForRoot(true, false)]
		[NonSerialized]
		public VillagersControllerRoot villagers;

		// Token: 0x040061E9 RID: 25065
		[WaitForRoot(false, true)]
		[NonSerialized]
		private TownOverheadUiRoot overheadUi;

		// Token: 0x040061EA RID: 25066
		[WaitForService(true, true)]
		public AudioService audioService;

		// Token: 0x040061EB RID: 25067
		[WaitForService(true, true)]
		private QuestService questService;

		// Token: 0x040061EC RID: 25068
		[WaitForService(true, true)]
		private ILocalizationService loc;

		// Token: 0x040061ED RID: 25069
		[WaitForService(true, true)]
		private GameStateService stateService;

		// Token: 0x040061EE RID: 25070
		[WaitForService(true, true)]
		private TrackingService tracking;

		// Token: 0x040061EF RID: 25071
		[WaitForService(true, true)]
		private ConfigService configService;

		// Token: 0x040061F0 RID: 25072
		[WaitForService(true, true)]
		private ProgressionDataService.Service ProgressionService;

		// Token: 0x040061F1 RID: 25073
		[WaitForService(true, true)]
		private SessionService session;

		// Token: 0x040061F2 RID: 25074
		[WaitForService(true, true)]
		private FacebookService facebook;

		// Token: 0x040061F3 RID: 25075
		[WaitForService(true, true)]
		private TournamentService tournamentService;

		// Token: 0x040061F4 RID: 25076
		[WaitForService(true, true)]
		private GiftLinksService giftLinksService;

		// Token: 0x040061F5 RID: 25077
		[WaitForService(true, true)]
		private HelpshiftService helpShiftService;

		// Token: 0x040061F6 RID: 25078
		[WaitForService(true, true)]
		private AssetBundlePreloadService preloadService;

		// Token: 0x040061F7 RID: 25079
		[WaitForService(true, true)]
		private AssetBundleService abs;

		// Token: 0x040061F8 RID: 25080
		[WaitForService(true, true)]
		private SBSService sbs;

		// Token: 0x040061F9 RID: 25081
		[WaitForService(true, true)]
		private PushNotificationService pushNotificationService;

		// Token: 0x040061FA RID: 25082
		[WaitForService(true, true)]
		private DailyGiftsService dailyGiftsService;

		// Token: 0x040061FB RID: 25083
		[WaitForService(true, true)]
		private ContentUnlockService contentUnlockService;

		// Token: 0x040061FC RID: 25084
		[WaitForService(true, true)]
		private SeasonService seasonService;

		// Token: 0x040061FD RID: 25085
		[WaitForService(true, true)]
		private SaleService saleService;

		// Token: 0x040061FE RID: 25086
		[WaitForService(true, true)]
		private ChallengeService challengeService;

		// Token: 0x040061FF RID: 25087
		[WaitForService(true, true)]
		private BankService bankService;

		// Token: 0x04006200 RID: 25088
		[NonSerialized]
		public TownSceneLoader townLoader;

		// Token: 0x04006201 RID: 25089
		private MusicPlayer musicPlayer;

		// Token: 0x04006202 RID: 25090
		private VisitFriendsRoot visitFriendsRoot;

		// Token: 0x04006203 RID: 25091
		private PopupManager popupManager;

		// Token: 0x04006205 RID: 25093
		private QuestActionHandlerManager questActionHandlerManager;

		// Token: 0x04006206 RID: 25094
		[SerializeField]
		private CameraInputController cameraInputController;

		// Token: 0x04006207 RID: 25095
		[SerializeField]
		private CollectablesSpriteManager collectables;

		// Token: 0x04006208 RID: 25096
		public CameraPanManager cameraPan;

		// Token: 0x04006209 RID: 25097
		private TutorialRunner tutorialRunner;
	}
}
