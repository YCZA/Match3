using System;
using System.Collections;
using Match3.Scripts1.Puzzletown.Match3;
using Match3.Scripts1.Puzzletown.Match3.Scoring;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Puzzletown.Town;
using Match3.Scripts1.Puzzletown.UI;
using Match3.Scripts1.Shared.Flows;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.Signals;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;
using Match3.Scripts2.Shop;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Flows
{
	// Token: 0x020004B9 RID: 1209
	public class LevelSelectionFlow : AFlowR<LevelSelectionFlow.Input, LevelConfig>
	{
		// Token: 0x06002208 RID: 8712 RVA: 0x00091F54 File Offset: 0x00090354
		protected override IEnumerator FlowRoutine(LevelSelectionFlow.Input args)
		{
			this.selectedLevel = null;
			this.levelToSnap = args.levelToSnap;
			this.isLoadingScenes = true;
			BlockerManager.global.Append(new Func<IEnumerator>(this.BlockInputWhileLoadingRoutine), true);
			yield return ServiceLocator.Instance.Inject(this);
			// yield return this.LoadRequiredScene(args);
			this.isLoadingScenes = false;
			switch (args.levelPlayMode)
			{
			case LevelPlayMode.Regular:
				yield return this.SelectLevelInNormalProgressionRoutine(args);
				break;
			case LevelPlayMode.LevelOfTheDay:
				yield return this.SelectLevelOfTheDayRoutine();
				break;
			case LevelPlayMode.DiveForTreasure:
			case LevelPlayMode.PirateBreakout:
				yield return this.SelectWeeklyEventRoutine();
				break;
			}
			yield return this.selectedLevel;
		}

		// Token: 0x06002209 RID: 8713 RVA: 0x00091F78 File Offset: 0x00090378
		protected IEnumerator LoadRequiredScene(LevelSelectionFlow.Input args)
		{
			switch (args.levelPlayMode)
			{
			case LevelPlayMode.Regular:
				yield return this.LoadLevelMap(args.levelToSnap);
				break;
			case LevelPlayMode.LevelOfTheDay:
				yield return this.LoadLODStartScene();
				break;
			case LevelPlayMode.DiveForTreasure:
				yield return this.LoadDiveForTreasureStartScene(args.levelToSnap);
				break;
			case LevelPlayMode.PirateBreakout:
				yield return this.LoadPirateBreakoutStartScene(args.levelToSnap);
				break;
			}
			yield break;
		}

		// Token: 0x0600220A RID: 8714 RVA: 0x00091F9C File Offset: 0x0009039C
		protected IEnumerator BlockInputWhileLoadingRoutine()
		{
			while (this.isLoadingScenes)
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x0600220B RID: 8715 RVA: 0x00091FB8 File Offset: 0x000903B8
		protected IEnumerator LoadLODStartScene()
		{
			Wooroutine<LevelConfig> getConfigRoutine = this.levelOfDayService.GetCurrentLevelOfDayConfig(null);
			yield return getConfigRoutine;
			this.selectedLevel = getConfigRoutine.ReturnValue;
			Wooroutine<M3_LevelOfDayStartRoot> levelStartSceneLoadRoutine = SceneManager.Instance.LoadSceneWithParams<M3_LevelOfDayStartRoot, LevelConfig>(this.selectedLevel, null);
			yield return levelStartSceneLoadRoutine;
			this.levelStartScene = levelStartSceneLoadRoutine.ReturnValue;
			yield break;
		}

		// Token: 0x0600220C RID: 8716 RVA: 0x00091FD4 File Offset: 0x000903D4
		protected IEnumerator LoadLevelMap(int levelToSnap)
		{
			Wooroutine<M3_LevelSelectionUiRoot> mapLoadRoutine = SceneManager.Instance.LoadSceneWithParams<M3_LevelSelectionUiRoot, int>(levelToSnap, null);
			yield return mapLoadRoutine;
			this.levelSelectionUi = mapLoadRoutine.ReturnValue;
			yield break;
		}

		// Token: 0x0600220D RID: 8717 RVA: 0x00091FF8 File Offset: 0x000903F8
		protected IEnumerator SelectLevelOfTheDayRoutine()
		{
			this.levelStartScene.Hide(false);
			this.levelOfDayService.NotificationSeen = true;
			yield return this.levelStartScene.onCompleted;
			if (!this.levelStartScene.onCompleted.Dispatched)
			{
				this.levelStartScene.onCompleted.Clear();
				this.selectedLevel = null;
			}
			yield break;
		}

		// Token: 0x0600220E RID: 8718 RVA: 0x00092014 File Offset: 0x00090414
		private IEnumerator LoadDiveForTreasureStartScene(int level)
		{
			Wooroutine<LevelConfig> levelConfigGetRoutine = this.m3ConfigService.GetLevelConfig(M3ConfigService.AREA_NOT_SET, level, LevelPlayMode.DiveForTreasure, AreaConfig.Tier.undefined);
			yield return levelConfigGetRoutine;
			this.selectedLevel = levelConfigGetRoutine.ReturnValue;
			Wooroutine<M3_DiveForTreasureStartRoot> levelStartSceneLoadRoutine = SceneManager.Instance.LoadSceneWithParams<M3_DiveForTreasureStartRoot, LevelConfig>(this.selectedLevel, null);
			yield return levelStartSceneLoadRoutine;
			this.levelStartScene = levelStartSceneLoadRoutine.ReturnValue;
			this.levelStartScene.gameObject.SetActive(true);
			yield break;
		}

		// Token: 0x0600220F RID: 8719 RVA: 0x00092038 File Offset: 0x00090438
		private IEnumerator LoadPirateBreakoutStartScene(int level)
		{
			Wooroutine<LevelConfig> levelConfigGetRoutine = this.m3ConfigService.GetLevelConfig(M3ConfigService.AREA_NOT_SET, level, LevelPlayMode.PirateBreakout, AreaConfig.Tier.undefined);
			yield return levelConfigGetRoutine;
			this.selectedLevel = levelConfigGetRoutine.ReturnValue;
			Wooroutine<M3_PirateBreakoutStartRoot> levelStartSceneLoadRoutine = SceneManager.Instance.LoadSceneWithParams<M3_PirateBreakoutStartRoot, LevelConfig>(this.selectedLevel, null);
			yield return levelStartSceneLoadRoutine;
			this.levelStartScene = levelStartSceneLoadRoutine.ReturnValue;
			this.levelStartScene.gameObject.SetActive(true);
			yield break;
		}

		// Token: 0x06002210 RID: 8720 RVA: 0x0009205C File Offset: 0x0009045C
		private IEnumerator SelectWeeklyEventRoutine()
		{
			for (;;)
			{
				AwaitSignal<bool> selected = this.levelStartScene.onCompleted;
				yield return selected;
				if (!selected.Dispatched)
				{
					break;
				}
				Wooroutine<bool> checkLives = new CheckLivesJourney(new TownDiamondsPanelRoot.TownDiamondsPanelRootParameters
				{
					source1 = "out_of_lives",
					source2 = "level_start"
				}).Start<bool>();
				yield return checkLives;
				if (checkLives.ReturnValue)
				{
					goto Block_1;
				}
				this.levelStartScene.gameObject.SetActive(true);
				this.levelStartScene.onCompleted.Clear();
			}
			this.selectedLevel = null;
			Block_1:
			yield break;
		}

		// Token: 0x06002211 RID: 8721 RVA: 0x00092078 File Offset: 0x00090478
		private IEnumerator SelectRoutine(LevelSelectionFlow.Input args)
		{
			if (this.levelSelectionUi.OnInitialized.WasDispatched)
			{
				this.levelSelectionUi.GoAgain(this.levelToSnap);
			}
			this.levelSelectionUi.onCompleted.Clear();
			Level selectedLevelWrapper;
			Wooroutine<QuestsPopupRoot.Output> popup;
			for (;;)
			{
				this.levelSelectionUi.Enable();
				if (this.levelToSnap >= 0)
				{
					this.levelSelectionUi.Snap(this.levelToSnap);
				}
				this.levelSelectionUi.HideCloseButton(false);
				yield return this.levelSelectionUi.onCompleted;
				selectedLevelWrapper = this.levelSelectionUi.onCompleted.Dispatched;
				if (selectedLevelWrapper.Equals(default(Level)))
				{
					break;
				}
				DateTime? unlockDate = this.contentUnlockService.UnlockDateForLevel(selectedLevelWrapper.level);
				bool isLockedByDate = unlockDate != null;
				bool isLockedByEndOfContent = this.contentUnlockService.IsLockedByEndOfContent(selectedLevelWrapper.level);
				bool isLockedByQuest = this.m3ConfigService.IsLockedByQuest(selectedLevelWrapper.level);
				// 检查关卡状态
				if (isLockedByEndOfContent)
				{
					this.levelSelectionUi.onCompleted.Clear();
					Wooroutine<BannerEndOfContentRoot> loader = SceneManager.Instance.LoadScene<BannerEndOfContentRoot>(null);
					yield return loader;
					yield return loader.ReturnValue.onDestroyed;
				}
				else if (isLockedByDate)
				{
					this.levelSelectionUi.onCompleted.Clear();
					Wooroutine<PopupComingSoonRoot> loader2 = SceneManager.Instance.LoadSceneWithParams<PopupComingSoonRoot, DateTime>(unlockDate.Value, null);
					yield return loader2;
					yield return loader2.ReturnValue.onDestroyed;
				}
				else
				{
					// 是否被任务锁定
					if (!isLockedByQuest)
					{
						break;
					}
					this.levelSelectionUi.Close();
					popup = new LevelSelectionQuestPopupFlow().Start(args);
					yield return popup;
					if (!popup.ReturnValue.isTaskCompleted)
					{
						switch (popup.ReturnValue.selectedTaskType)
						{
						case QuestTaskType.unknown:
							this.levelToSnap = -1;
							this.levelSelectionUi.SkipReloadOnEnable = true;
							this.levelSelectionUi.onCompleted.Clear();
							continue;
						case QuestTaskType.collect_and_repair:
						case QuestTaskType.collect_and_interact:
							this.levelToSnap = popup.ReturnValue.selectedLevel;
							this.levelSelectionUi.onCompleted.Clear();
							continue;
						case QuestTaskType.reach_rank:
							goto IL_3C4;
						}
						break;
					}
					goto IL_40A;
				}
			}
			goto IL_456;
			IL_3C4:
			selectedLevelWrapper.level = -1;
			yield return selectedLevelWrapper;
			this.TryOpenShop();
			yield break;
			IL_40A:
			selectedLevelWrapper.level = -1;
			if (SceneManager.IsPlayingMatch3)
			{
				// Wooroutine<QuestsPopupRoot.Output> popup = null;
				new ReturnToIslandFlow(null, popup.ReturnValue.islandAction).Execute();
			}
			else
			{
				// Wooroutine<QuestsPopupRoot.Output> popup = null;
				popup.ReturnValue.islandAction();
			}
			IL_456:
			yield return selectedLevelWrapper;
			yield break;
		}

		// Token: 0x06002212 RID: 8722 RVA: 0x0009209C File Offset: 0x0009049C
		private void TryOpenShop()
		{
			TownUiRoot exists = global::UnityEngine.Object.FindObjectOfType<TownUiRoot>();
			if (exists)
			{
				SceneManager.Instance.LoadScene<PopupHowToRankUpRoot>(null);
			}
		}

		// Token: 0x06002213 RID: 8723 RVA: 0x000920C8 File Offset: 0x000904C8
		protected IEnumerator SelectLevelInNormalProgressionRoutine(LevelSelectionFlow.Input args)
		{
			LevelConfig levelConfig;
			while (true)
			{
				Debug.Log("<color=red>args:</color>"+args.score);
				// Wooroutine<Level> selectRoutine = WooroutineRunner.StartWooroutine<Level>(this.SelectRoutine(args));
				// yield return selectRoutine;
				// Level selectedLevelWrapper = selectRoutine.ReturnValue;
				Level selectedLevelWrapper = new Level(args.levelToSnap, (int)args.tier);
				if (selectedLevelWrapper.level < 0)
				{
					break;
				}
				if (selectedLevelWrapper.Equals(default(Level)))
				{
					this.levelSelectionUi.Close();
					yield break;
				}
				Wooroutine<LevelConfig> levelConfigLoadRoutine = this.m3ConfigService.GetLevelConfig(this.m3ConfigService.GetAreaForLevel(selectedLevelWrapper.level), selectedLevelWrapper.level, LevelPlayMode.Regular, (AreaConfig.Tier)selectedLevelWrapper.tier);
				yield return levelConfigLoadRoutine;
				levelConfig = levelConfigLoadRoutine.ReturnValue;
				// 打开关卡开始窗口
				Wooroutine<M3_LevelStartRoot> levelStartScene = SceneManager.Instance.LoadSceneWithParams<M3_LevelStartRoot, LevelConfig>(levelConfig, null);
				yield return levelStartScene;
				yield return levelStartScene.ReturnValue.OnInitialized;
				// this.levelSelectionUi.HideCloseButton(true);
				levelStartScene.ReturnValue.Hide(false);
				yield return levelStartScene.ReturnValue.onCompleted;
				if (!levelStartScene.ReturnValue.onCompleted.Dispatched)
				{
					// "开关关卡"界面关闭
					// this.levelSelectionUi.onCompleted.Clear();
					levelStartScene.ReturnValue.onCompleted.Clear();
					this.levelToSnap = levelConfig.Level.level;
					yield break;
				}
				else
				{
					// 开始关卡
					Wooroutine<bool> checkLives = new CheckLivesJourney(new TownDiamondsPanelRoot.TownDiamondsPanelRootParameters
					{
						source1 = "out_of_lives",
						source2 = "level_start"
					}).Start<bool>();
					yield return checkLives;
					if (checkLives.ReturnValue)
					{
						this.selectedLevel = levelConfig;
						yield break;
					}
				}
			}
			yield return null;
		}

		// Token: 0x04004D5C RID: 19804
		// [WaitForService(true, true)]
		// private TrackingService tracking;

		// Token: 0x04004D5D RID: 19805
		[WaitForService(true, true)]
		private LivesService livesService;

		// Token: 0x04004D5E RID: 19806
		[WaitForService(true, true)]
		private M3ConfigService m3ConfigService;

		// Token: 0x04004D5F RID: 19807
		[WaitForService(true, true)]
		private LevelOfDayService levelOfDayService;

		// Token: 0x04004D60 RID: 19808
		// [WaitForService(true, true)]
		// private QuestService questService;

		// Token: 0x04004D61 RID: 19809
		[WaitForService(true, true)]
		private ContentUnlockService contentUnlockService;

		// Token: 0x04004D62 RID: 19810
		[WaitForService(true, true)]
		private GameStateService gameStateService;

		// Token: 0x04004D63 RID: 19811
		protected M3_LevelSelectionUiRoot levelSelectionUi;

		// Token: 0x04004D64 RID: 19812
		protected M3_ALevelStartRoot levelStartScene;

		// Token: 0x04004D65 RID: 19813
		protected LevelConfig selectedLevel;

		// Token: 0x04004D66 RID: 19814
		protected bool isLoadingScenes;

		// Token: 0x04004D67 RID: 19815
		protected int levelToSnap;

		// Token: 0x020004BA RID: 1210
		public class Input
		{
			// Token: 0x04004D68 RID: 19816
			public int levelToSnap;

			// Token: 0x04004D69 RID: 19817
			public Match3Score score;

			// Token: 0x04004D6A RID: 19818
			public LevelPlayMode levelPlayMode;

			public AreaConfig.Tier tier;
		}
	}
}
