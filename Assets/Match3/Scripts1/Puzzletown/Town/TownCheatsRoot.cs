using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Match3.Scripts1.Puzzletown.Config;
using Match3.Scripts1.Puzzletown.Features.DailyGifts;
using Match3.Scripts1.Puzzletown.Flows;
using Match3.Scripts1.Puzzletown.Match3;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Puzzletown.UI;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.Legal;
using Match3.Scripts1.Wooga.Services;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Town
{
	// eli key point: town cheats 没有找到相关的场景文件
	public class TownCheatsRoot : APtSceneRoot<TownSceneLoader>, IHandler<Cheats>, IHandler<CheatCategory>
	{
		// Token: 0x1700090F RID: 2319
		// (get) Token: 0x06003C56 RID: 15446 RVA: 0x0012C99A File Offset: 0x0012AD9A
		// (set) Token: 0x06003C57 RID: 15447 RVA: 0x0012C9A1 File Offset: 0x0012ADA1
		public static bool EditMode
		{
			get
			{
				return TownCheatsRoot.editMode;
			}
			set
			{
				TownCheatsRoot.editMode = value;
			}
		}

		// Token: 0x06003C58 RID: 15448 RVA: 0x0012C9AC File Offset: 0x0012ADAC
		protected override void Go()
		{
			this.townLoader = this.parameters;
			this.categoryDataSource.Show(from kvp in this.mapCategoryToCheats
			select kvp.Key);
			this.Handle(CheatCategory.Economy);
			// this.statusView.Init(this.progressionService, this.contentUnlockService, this.questService);
			// this.statusView.transform.SetAsLastSibling();
		}

		// Token: 0x06003C59 RID: 15449 RVA: 0x0012CA2C File Offset: 0x0012AE2C
		public void Handle(CheatCategory category)
		{
			switch (category)
			{
			case CheatCategory.ProgressionGifts:
				WooroutineRunner.StartCoroutine(PopupFreeGiftRoot.TestAllGifts(this.config), null);
				break;
			case CheatCategory.ConfigAbTests:
				this.abTestChooser.gameObject.SetActive(true);
				this.abTestChooser.ShowAndGetForcedAbTestStringRoutine(this.sbs.ConfigService, this.statusView).ContinueWith(delegate(AbTestChooser.ChosenAbTests abtests)
				{
					this.abTestChooser.gameObject.SetActive(false);
					if (abtests.shouldBeApplied)
					{
						this.stateService.Debug.ForcedAbTests = abtests.abTests;
						this.stateService.Save(true);
						this.sbs.CheckForForcedAbTests(abtests.abTests);
					}
				}).Start();
				break;
			case CheatCategory.SwitchGameState:
				GamestateSwitcherRoot.Show();
				break;
			case CheatCategory.ChapterIntros:
				this.UpdateChapterIntroStatus();
				this.dataSource.Show(this.mapCategoryToCheats[category]);
				break;
			case CheatCategory.Dialogues:
				DialogueSwitcherRoot.Show();
				break;
			default:
				if (category != CheatCategory.Tournament)
				{
					// 会报错，注释掉
					// this.dataSource.Show(this.mapCategoryToCheats[category]);
				}
				else
				{
					// this.statusView.OverrideText(this.tournamentService.ToString());
					// this.dataSource.Show(this.mapCategoryToCheats[category]);
				}
				break;
			case CheatCategory.UiTests:
				WooroutineRunner.StartCoroutine(this.PopupTestingRoutine(), null);
				break;
			case CheatCategory.Ads:
				this.UpdateAdStatus();
				this.dataSource.Show(this.mapCategoryToCheats[category]);
				break;
			case CheatCategory.LevelOfDay:
				this.UpdateLODStatus();
				this.dataSource.Show(this.mapCategoryToCheats[category]);
				break;
			case CheatCategory.WeeklyEvents:
				this.statusView.OverrideText(this.diveForTreasureService.ToString());
				this.dataSource.Show(this.mapCategoryToCheats[category]);
				break;
			case CheatCategory.ContentUnlock:
			{
				string txt = string.Format("Content Unlock Enabled: {0}", !this.contentUnlockService.IsUnlockDisabled);
				this.statusView.OverrideText(txt);
				this.dataSource.Show(this.mapCategoryToCheats[category]);
				break;
			}
			}
		}

		// eli key point cheat
		// Token: 0x06003C5A RID: 15450 RVA: 0x0012CC34 File Offset: 0x0012B034
		public void Handle(Cheats cheat)
		{
			if (this.stateService == null)
			{
				global::UnityEngine.Debug.LogWarning("No state service loaded");
				return;
			}
			bool flag = true;
			switch (cheat)
			{
			case Cheats.Add1Life:
				this.livesService.AddLives(1, "作弊工具");
				break;
			case Cheats.UseLife:
				this.livesService.UseLife("作弊工具");
				break;
			case Cheats.Add1000SeasonalCurrency:
				this.stateService.Resources.AddMaterial("earned_season_currency", 1000, true);
				break;
			case Cheats.Use1000SeasonalCurrency:
				this.LoseResource(1000, "earned_season_currency");
				break;
			case Cheats.Add1000Coins:
				this.stateService.Resources.AddMaterial("coins", 1000, true, "作弊工具");
				break;
			case Cheats.Add100Diamonds:
				this.stateService.Resources.AddMaterial("diamonds", 100, true, "作弊工具");
				break;
			case Cheats.Add100Happiness:
				this.stateService.Resources.AddMaterial("harmony", 100, true, "作弊工具");
				break;
			case Cheats.Add10GrandPrizeProgress:
				this.stateService.Resources.AddMaterial("season_currency", 10, true);
				break;
			case Cheats.UnlockTournament:
			{
				this.tournamentService.Status.CheatUnlock();
				TownBottomPanelRoot townBottomPanelRoot = global::UnityEngine.Object.FindObjectOfType<TownBottomPanelRoot>();
				if (townBottomPanelRoot != null)
				{
					townBottomPanelRoot.UpdateTournamentStatus();
				}
				this.statusView.OverrideText(this.tournamentService.ToString());
				flag = false;
				break;
			}
			case Cheats.GoToNextArea:
				this.progressionService.CheatToNextArea();
				this.questService.CollectAllQuestsForLevel(this.progressionService.UnlockedLevel);
				break;
			case Cheats.GoToNextLevel:
			{
				int unlockedLevel = this.progressionService.UnlockedLevel;
				// 审核版只有330关
				// #if REVIEW_VERSION
				// {
				// 	if (unlockedLevel > 330)
				// 		return;
				// }
				// #endif
				if (this.config.general.tier_unlocked.unlock_tier_c_at_level > unlockedLevel)
				{
					this.progressionService.UnlockedLevel++;
				}
				else
				{
					int unlockLevel = this.progressionService.CheatUnlockNextLevel();
					this.questService.CollectAllQuestsForLevel(unlockLevel);
				}
				break;
			}
			case Cheats.GoToPreviousLevel:
				this.progressionService.UnlockedLevel--;
				break;
			case Cheats.ResetProgress:
				this.stateService.Reset();
				this.progressionService.CurrentLevel = this.progressionService.UnlockedLevel;
				PTReloader.ReloadGame("Cheat reset game state", false);
				break;
			case Cheats.DailyGiftUnlock:
				this.dailyGiftsService.ForceAvailable();
				WoogaDebug.Log(new object[]
				{
					this.dailyGiftsService.DaysSinceLastClaim
				});
				this.stateService.Save(true);
				PTReloader.ReloadGame("Cheat daily gift unlock", true);
				break;
			case Cheats.DailyGiftUnlock5:
				this.dailyGiftsService.ForceAvailable();
				this.stateService.DailyGifts.NumConsecutiveDays += 5;
				WoogaDebug.Log(new object[]
				{
					this.dailyGiftsService.DaysSinceLastClaim
				});
				this.stateService.Save(true);
				PTReloader.ReloadGame("Cheat daily gift unlock", true);
				break;
			case Cheats.DailyGiftReset:
				this.dailyGiftsService.ForceReset();
				this.stateService.AdViews.SetNumberOfAdsWatchedToday(AdPlacement.DailyGift, 0);
				this.stateService.Save(true);
				PTReloader.ReloadGame("Cheat daily gift reset", true);
				break;
			case Cheats.CompleteCurrentQuest:
				this.questService.questManager.DebugFinishQuest();
				break;
			case Cheats.ReloadGame:
				PTReloader.ReloadGame("Cheat reload game", false);
				break;
			case Cheats.ToggleLocaKeys:
			{
				WoogaSystemLanguage lang = this.language;
				if (this.locaService.Language != WoogaSystemLanguage.LocaKeys)
				{
					this.language = this.locaService.Language;
					lang = WoogaSystemLanguage.LocaKeys;
				}
				this.locaService.ChangeLanguage(lang);
				WooroutineRunner.StartCoroutine(this.RefreshTownMain(), null);
				break;
			}
			case Cheats.ScheduleNotfications:
				this.notificationService.OnSuspend();
				break;
			case Cheats.LoadFriendsGameState:
				WooroutineRunner.StartCoroutine(this.LoadEmptyFriendStateRoutine(), null);
				break;
			case Cheats.OpenRatingFilter:
			{
				PopupRatingRoot.Trigger trigger = new PopupRatingRoot.Trigger(null, null, null, this.sbs, this.trackingService);
				WooroutineRunner.StartCoroutine(trigger.Run(), null);
				break;
			}
			case Cheats.SkipOfferCooldown:
				this.offers.SkipCooldown();
				break;
			case Cheats.HideCheats:
			{
				this.stateService.Debug.ForceHideDebugMode = true;
				DebugInfoRoot debugInfoRoot = global::UnityEngine.Object.FindObjectOfType<DebugInfoRoot>();
				if (debugInfoRoot)
				{
					global::UnityEngine.Object.Destroy(debugInfoRoot.gameObject);
				}
				this.stateService.Save(false);
				base.Destroy();
				break;
			}
			case Cheats.ResetSeenFlags:
				this.stateService.ResetAllSeenFlags();
				break;
			case Cheats.OpenFullQuestData:
				SceneManager.Instance.LoadScene<QuestsPopupRoot>(null);
				QuestsPopupRoot.debugShowAllQuests = true;
				break;
			case Cheats.IntroShowAll:
				this.HideCheatMenu();
				WooroutineRunner.StartCoroutine(this.ShowChapterIntrosRoutine(0), null);
				break;
			case Cheats.IntroShowFrom:
				this.HideCheatMenu();
				WooroutineRunner.StartCoroutine(this.ShowChapterIntrosRoutine(this.selectedChapterIntro), null);
				break;
			case Cheats.IntroShowNext:
				this.selectedChapterIntro = Mathf.Clamp(this.selectedChapterIntro + 1, 0, this.config.chapter.chapters.Length);
				this.UpdateChapterIntroStatus();
				flag = false;
				break;
			case Cheats.IntroShowPrev:
				this.selectedChapterIntro = Mathf.Clamp(this.selectedChapterIntro - 1, 0, this.config.chapter.chapters.Length);
				this.UpdateChapterIntroStatus();
				flag = false;
				break;
			case Cheats.IntroShowFrom10:
				this.selectedChapterIntro = 10;
				this.UpdateChapterIntroStatus();
				flag = false;
				break;
			case Cheats.IntroShowFrom15:
				this.selectedChapterIntro = 15;
				this.UpdateChapterIntroStatus();
				flag = false;
				break;
			case Cheats.ToggleAreaHelpers:
				TownCheatsRoot.s_areasVisible = !TownCheatsRoot.s_areasVisible;
				Camera.main.SetLayerVisible(ObjectLayer.EditorHelpers, TownCheatsRoot.s_areasVisible);
				break;
			case Cheats.ToggleShaderLod:
				Shader.globalMaximumLOD = ((Shader.globalMaximumLOD != 400) ? 400 : 550);
				break;
			case Cheats.ToggleCharacters:
				this.ToggleObject("VillagersController");
				break;
			case Cheats.ToggleBuildings:
				this.ToggleObject("Town Manager");
				break;
			case Cheats.Toggle25D:
				this.ToggleObject("Grp_2.5D");
				break;
			case Cheats.ToggleOcean:
				this.ToggleObject("Ocean");
				break;
			case Cheats.ToggleLand:
				this.ToggleObject("Land");
				break;
			case Cheats.ChallengeAssignNext:
				this.challengeService.AssignNextChallenges(-1);
				break;
			case Cheats.ChallengeNextDecoSet:
				this.challengeService.AssignNextDecoSet(true);
				break;
			case Cheats.ChallengeAdd100Paws:
				this.stateService.Resources.AddMaterial("paws", 100, true);
				break;
			case Cheats.ChallengeCompleteNextChallenge:
				foreach (ChallengeGoal challengeGoal in this.stateService.Challenges.CurrentChallenges)
				{
					int collectedTotal = this.stateService.Resources.GetCollectedTotal(challengeGoal.type);
					int num = collectedTotal - challengeGoal.start;
					if (num < challengeGoal.goal)
					{
						this.stateService.Resources.AddMaterial(challengeGoal.type, challengeGoal.goal - num, true);
						break;
					}
				}
				break;
			case Cheats.DynamicBuildings:
				this.HideCheatMenu();
				WooroutineRunner.StartCoroutine(this.DynamicBuildingsTestRoutine(), null);
				break;
			case Cheats.UseRightLandscapeOrientation:
				AUiAdjuster.EditorUseRightOrientation = true;
				break;
			case Cheats.UseLeftLandscapeOrientation:
				AUiAdjuster.EditorUseRightOrientation = false;
				break;
			case Cheats.ResetAllAds:
				this.stateService.NextSpinAvailable = 0;
				this.stateService.AdViews.ClearDailyAdCounters();
				this.UpdateAdStatus();
				flag = false;
				break;
			case Cheats.ResetChallengeAds:
				this.stateService.AdViews.SetNumberOfAdsWatchedToday(AdPlacement.Challenges, 0);
				this.UpdateAdStatus();
				flag = false;
				break;
			case Cheats.ResetWheelAds:
				this.stateService.NextSpinAvailable = 0;
				this.stateService.AdViews.SetNumberOfAdsWatchedToday(AdPlacement.AdWheel, 0);
				this.UpdateAdStatus();
				flag = false;
				break;
			case Cheats.ResetDailyGiftAds:
				this.stateService.AdViews.SetNumberOfAdsWatchedToday(AdPlacement.DailyGift, 0);
				this.UpdateAdStatus();
				flag = false;
				break;
			case Cheats.RefreshAdStats:
				this.UpdateAdStatus();
				flag = false;
				break;
			case Cheats.ShowAdsTestSuit:
				this.videoAds.DebugShowTestSuite();
				break;
			case Cheats.Add100BankedDiamonds:
				this.stateService.Bank.NumberOfBankedDiamonds += 100;
				break;
			case Cheats.LODReset:
			{
				this.levelOfDayService.CheatReset();
				this.UpdateLODStatus();
				TownBottomPanelRoot townBottomPanelRoot2 = global::UnityEngine.Object.FindObjectOfType<TownBottomPanelRoot>();
				if (townBottomPanelRoot2 != null)
				{
					townBottomPanelRoot2.levelOfDayPlayButton.ShowNotificationIcon(true);
				}
				flag = false;
				break;
			}
			case Cheats.LODAddTry:
				this.levelOfDayService.CheatChangeTries(1);
				this.UpdateLODStatus();
				flag = false;
				break;
			case Cheats.LODRemoveTry:
				this.levelOfDayService.CheatChangeTries(-1);
				this.UpdateLODStatus();
				flag = false;
				break;
			case Cheats.LODComplete:
				this.levelOfDayService.CheatComplete(true);
				this.UpdateLODStatus();
				flag = false;
				break;
			case Cheats.LODUncomplete:
				this.levelOfDayService.CheatComplete(false);
				this.UpdateLODStatus();
				flag = false;
				break;
			case Cheats.LODStartSoon:
				this.levelOfDayService.CheatStartSoon();
				this.UpdateLODStatus();
				flag = false;
				break;
			case Cheats.LODEndSoon:
				this.levelOfDayService.CheatEndSoon();
				this.UpdateLODStatus();
				flag = false;
				break;
			case Cheats.LODToggleUnlocked:
				this.levelOfDayService.CheatToggleUnlocked();
				this.UpdateLODStatus();
				flag = false;
				break;
			case Cheats.Lose1000Coins:
				this.LoseResource(1000, "coins");
				break;
			case Cheats.Lose100Diamonds:
				this.LoseResource(100, "diamonds");
				break;
			case Cheats.LosePreGameBoosts:
				this.LosePreGameBoosts();
				break;
			case Cheats.LoseInGameBoosts:
				this.LoseInGameBoosts();
				break;
			case Cheats.SimulateBadNetwork:
				this.tournamentService.CheatToggleSimulatedBadNetwork();
				this.statusView.OverrideText(this.tournamentService.ToString());
				flag = false;
				break;
			case Cheats.PlaceNextTrophy:
			{
				this.HideCheatMenu();
				this.tournamentTrophyIndex++;
				this.tournamentTrophyIndex %= Enum.GetNames(typeof(DecoTrophyItemWon)).Length;
				this.tournamentTrophyIndex = ((this.tournamentTrophyIndex != 0) ? this.tournamentTrophyIndex : 1);
				DecoTrophyItemWon decoTrophy = (DecoTrophyItemWon)this.tournamentTrophyIndex;
				BuildingConfig buildingConfig = this.configService.buildingConfigList.GetConfig(decoTrophy);
				new ForceUserPlaceDecoFlow(buildingConfig).Start();
				break;
			}
			case Cheats.NextDailyDeal:
				this.dailyDealsService.CheatMenuNextDailyDeal();
				break;
			case Cheats.ExpireCurrentDailyDeal:
				this.stateService.DailyDeals.CurrentDealExpirationDate = DateTime.Now;
				break;
			case Cheats.ResetDailyDeals:
				this.dailyDealsService.ResetDailyDeals();
				break;
			case Cheats.ClearTransactionHistory:
				this.stateService.Transactions.ClearTransactionData();
				break;
			case Cheats.LODAddDayToStreak:
				this.levelOfDayService.CheatModifyStreak(1);
				this.UpdateLODStatus();
				flag = false;
				break;
			case Cheats.LODSubtractDayToStreak:
				this.levelOfDayService.CheatModifyStreak(-1);
				this.UpdateLODStatus();
				flag = false;
				break;
			case Cheats.CrashRestart:
				throw new UnityException("Simple Exception");
			case Cheats.CrashKill:
				WoogaDebug.LogError(new object[]
				{
					// "The AssetBundle 'https://cdn.wooga.com/cdn-puzzletown/release-abm-2.0/Android/hd/buildings_seasonal_statues_2018_634b910e31d7e0f7ce69bee18cc5ca92032560aa_Android_hd_1' could not be loaded because it is not compatible with this newer version of the Unity runtime. Rebuild the AssetBundle to fix this error."
					"The AssetBundle ........."
				});
				break;
			case Cheats.ResetDiveForTreasureProgress:
				this.stateService.DiveForTreasure.Level = 1;
				break;
			case Cheats.ResetPirateBreakoutProgress:
				this.stateService.PirateBreakout.Level = 1;
				break;
			case Cheats.EnableTimeCheating:
				TimeService.allowTimeCheating = true;
				break;
			case Cheats.DisableTimeCheating:
				TimeService.allowTimeCheating = false;
				break;
			case Cheats.PushyToSelf:
				this.pushNotificationService.SendNotificationToStranger(SBS.Authentication.GetUserContext().user_id, "catching");
				break;
			case Cheats.ToggleContentUnlock:
				this.contentUnlockService.IsUnlockDisabled = !this.contentUnlockService.IsUnlockDisabled;
				break;
			case Cheats.SaleReset:
				this.stateService.Sale.BoughtSaleId = string.Empty;
				break;
			case Cheats.GdprReset:
				Terms.DebugReset();
				break;
			case Cheats.ResetFruitTournamentPopups:
				this.stateService.RemoveSeenFlag("NewTournamentMigrationPopUp");
				this.stateService.RemoveSeenFlag(PopupTournamentNewKindIntroductionRoot.GetSeenFlagNameOfTournament(TournamentType.Banana));
				this.stateService.RemoveSeenFlag(PopupTournamentNewKindIntroductionRoot.GetSeenFlagNameOfTournament(TournamentType.Plum));
				this.stateService.RemoveSeenFlag(PopupTournamentNewKindIntroductionRoot.GetSeenFlagNameOfTournament(TournamentType.Apple));
				this.stateService.RemoveSeenFlag(PopupTournamentNewKindIntroductionRoot.GetSeenFlagNameOfTournament(TournamentType.Starfruit));
				this.stateService.RemoveSeenFlag(PopupTournamentNewKindIntroductionRoot.GetSeenFlagNameOfTournament(TournamentType.Grape));
				break;
			case Cheats.ChallengeV2TutorialReset:
				this.progressionService.ResetTutorial("challengesV2Tutorial");
				break;
			case Cheats.BankEventReset:
				this.stateService.Bank.DebugReset();
				break;
			case Cheats.SeasonalsV3ProgressReset:
				this.seasonService.DebugReset();
				break;
			case Cheats.SeasonalsV3TutorialReset:
				this.progressionService.ResetTutorial("seasonalsV3ShopTutorial");
				break;
			}
			if (this && flag)
			{
				// 缺少UI, 所以不刷新
				// this.statusView.Refresh();
			}
		}

		// Token: 0x06003C5B RID: 15451 RVA: 0x0012D8FC File Offset: 0x0012BCFC
		private void LoseResource(int amount, string material)
		{
			int num = this.stateService.Resources.Current[material];
			if (num > 0)
			{
				int amount2 = -1 * Mathf.Min(num, Mathf.Abs(amount));
				this.stateService.Resources.AddMaterial(material, amount2, true);
			}
		}

		// Token: 0x06003C5C RID: 15452 RVA: 0x0012D94C File Offset: 0x0012BD4C
		private void LoseBoost(string material)
		{
			int num = this.stateService.Resources.Current[material];
			if (num > 0)
			{
				this.boostService.AddBoost(material, -num);
			}
		}

		// Token: 0x06003C5D RID: 15453 RVA: 0x0012D985 File Offset: 0x0012BD85
		private void LosePreGameBoosts()
		{
			this.LoseBoost("boost_pre_bomb_linegem");
			this.LoseBoost("boost_pre_double_fish");
			this.LoseBoost("boost_pre_rainbow");
		}

		// Token: 0x06003C5E RID: 15454 RVA: 0x0012D9A8 File Offset: 0x0012BDA8
		private void LoseInGameBoosts()
		{
			this.LoseBoost("boost_hammer");
			this.LoseBoost("boost_star");
			this.LoseBoost("boost_rainbow");
		}

		// Token: 0x06003C5F RID: 15455 RVA: 0x0012D9CB File Offset: 0x0012BDCB
		private void UpdateAdStatus()
		{
			// this.statusView.OverrideText(this.videoAds.ToString());
		}

		// Token: 0x06003C60 RID: 15456 RVA: 0x0012D9E3 File Offset: 0x0012BDE3
		private void UpdateLODStatus()
		{
			this.statusView.OverrideText(this.levelOfDayService.ToString());
		}

		// Token: 0x06003C61 RID: 15457 RVA: 0x0012D9FB File Offset: 0x0012BDFB
		private void UpdateChapterIntroStatus()
		{
			this.statusView.OverrideText(string.Format("Selected chapter: {0}", this.selectedChapterIntro));
		}

		// Token: 0x06003C62 RID: 15458 RVA: 0x0012DA20 File Offset: 0x0012BE20
		private IEnumerator LoadEmptyFriendStateRoutine()
		{
			GameStateService.GameStateData state = JsonUtility.FromJson<GameStateService.GameStateData>(this.testState.text);
			if (this.stateService.doLoadSpecificGameState(state))
			{
				TownLoadingFlowWithTransition.Input loadingScreenConfig = new TownLoadingFlowWithTransition.Input(LoadingScreenConfig.Random);
				Wooroutine<TownMainRoot> townLoadFlow = new TownLoadingFlowWithTransition().Start(loadingScreenConfig);
				yield return townLoadFlow;
				TownMainRoot townScene = townLoadFlow.ReturnValue;
				townScene.StartView(true, false);
			}
			yield break;
		}

		// Token: 0x06003C63 RID: 15459 RVA: 0x0012DA3C File Offset: 0x0012BE3C
		private void ToggleObject(string name)
		{
			if (!this.toggles.ContainsKey(name))
			{
				this.toggles[name] = GameObject.Find(name);
			}
			this.toggles[name].SetActive(!this.toggles[name].activeSelf);
		}

		// Token: 0x06003C64 RID: 15460 RVA: 0x0012DA94 File Offset: 0x0012BE94
		private void HideCheatMenu()
		{
			PanelToggleButton componentInChildren = base.GetComponentInChildren<PanelToggleButton>();
			if (componentInChildren != null)
			{
				componentInChildren.Hide();
			}
		}

		// Token: 0x06003C65 RID: 15461 RVA: 0x0012DABC File Offset: 0x0012BEBC
		private IEnumerator DynamicBuildingsTestRoutine()
		{
			this.dynamicBuildingTestSelector.gameObject.SetActive(true);
			Coroutine selectorRoutine = WooroutineRunner.StartCoroutine(this.dynamicBuildingTestSelector.SelectTestRoutine(), null);
			yield return selectorRoutine;
			this.dynamicBuildingTestSelector.gameObject.SetActive(false);
			yield break;
		}

		// Token: 0x06003C66 RID: 15462 RVA: 0x0012DAD8 File Offset: 0x0012BED8
		private IEnumerator PopupTestingRoutine()
		{
			// 暂时没有该测试场景
			// this.testablePopupSelector.gameObject.SetActive(true);
			// Wooroutine<PopupWithParams> popupSelectorRoutine = WooroutineRunner.StartWooroutine<PopupWithParams>(this.testablePopupSelector.SelectPopupRoutine());
			// yield return popupSelectorRoutine;
			// this.testablePopupSelector.gameObject.SetActive(false);
			// PopupWithParams selected = popupSelectorRoutine.ReturnValue;

			PopupWithParams selected = new PopupWithParams();
			selected.popup = PopupToTest.DailyGift;
			selected.isCancelled = false;
			selected.parameters = null;
			if (selected != null && !selected.isCancelled)
			{
				this.ShowTestPopup(selected);
				yield break;
			}
			yield break;
		}

		// Token: 0x06003C67 RID: 15463 RVA: 0x0012DAF4 File Offset: 0x0012BEF4
		private void ShowTestPopup(PopupWithParams popup)
		{
			this.HideCheatMenu();
			CheatPopupTester.ShowPopup(popup, this.locaService, this.configService, this.livesService, this.fbService, this.dailyGiftsService, this.levelOfDayService, this.audioService, this.questService, this.assetBundleService, this.seasonService, this.buildingResources);
		}

		// Token: 0x06003C68 RID: 15464 RVA: 0x0012DB50 File Offset: 0x0012BF50
		private IEnumerator ShowChapterIntrosRoutine(int fromChapter)
		{
			int chapterCount = this.config.chapter.chapters.Length;
			for (int chapter = fromChapter; chapter <= chapterCount; chapter++)
			{
				Wooroutine<BannerNewChapterRoot> intro = SceneManager.Instance.LoadSceneWithParams<BannerNewChapterRoot, int>(chapter, null);
				yield return intro;
				yield return intro.ReturnValue.onDestroyed;
			}
			yield break;
		}

		// Token: 0x06003C69 RID: 15465 RVA: 0x0012DB74 File Offset: 0x0012BF74
		private IEnumerator RefreshTownMain()
		{
			TownLoadingFlowWithTransition townRefreshFlow = new TownLoadingFlowWithTransition();
			TownLoadingFlowWithTransition.Input emptyConfig = new TownLoadingFlowWithTransition.Input(LoadingScreenConfig.None);
			Wooroutine<TownMainRoot> returnToTownFlow = townRefreshFlow.Start(emptyConfig);
			yield return returnToTownFlow;
			returnToTownFlow.ReturnValue.StartView(true, false);
			yield break;
		}

		public void StartLevelImmediate(string levelId)
		{
			StartCoroutine(StartLevelImmediateRoutine(levelId));
		}
		private IEnumerator StartLevelImmediateRoutine(string levelId)
		{
			AreaConfig.Tier tier = (AreaConfig.Tier)Enum.Parse(typeof(AreaConfig.Tier), levelId.Substring(levelId.Length - 1));
			int level = int.Parse(levelId.Substring(0, levelId.Length - 1));
			
			Wooroutine<LevelConfig> config = m3ConfigService.GetLevelConfig(m3ConfigService.GetAreaForLevel(level), level, LevelPlayMode.Regular, tier);
			yield return config;
			CoreGameFlow flow = new CoreGameFlow();
			CoreGameFlow.Input input = new CoreGameFlow.Input(-1, true, config.ReturnValue, LevelPlayMode.Regular);
			flow.Start(input);
		}
		// Token: 0x040064DB RID: 25819
		[WaitForService(true, true)]
		private GameStateService stateService;

		// Token: 0x040064DC RID: 25820
		[WaitForService(true, true)]
		private ProgressionDataService.Service progressionService;

		// Token: 0x040064DD RID: 25821
		[WaitForService(true, true)]
		private LivesService livesService;

		// Token: 0x040064DE RID: 25822
		[WaitForService(true, true)]
		private QuestService questService;

		// Token: 0x040064DF RID: 25823
		[WaitForService(true, true)]
		private ConfigService config;

		[WaitForService(true, true)]
		private M3ConfigService m3ConfigService;
		
		// Token: 0x040064E0 RID: 25824
		[WaitForService(true, true)]
		private SBSService sbs;

		// Token: 0x040064E1 RID: 25825
		[WaitForService(true, true)]
		private AudioService audioService;

		// Token: 0x040064E2 RID: 25826
		[WaitForService(true, true)]
		private ILocalizationService locaService;

		// Token: 0x040064E3 RID: 25827
		[WaitForService(true, true)]
		private LocalNotificationService notificationService;

		// Token: 0x040064E4 RID: 25828
		[WaitForService(true, true)]
		private OffersService offers;

		// Token: 0x040064E5 RID: 25829
		[WaitForService(true, true)]
		private TournamentService tournamentService;

		// Token: 0x040064E6 RID: 25830
		[WaitForService(true, true)]
		private ConfigService configService;

		// Token: 0x040064E7 RID: 25831
		[WaitForService(true, true)]
		private FacebookService fbService;

		// Token: 0x040064E8 RID: 25832
		[WaitForService(true, true)]
		private ChallengeService challengeService;

		// Token: 0x040064E9 RID: 25833
		[WaitForService(true, true)]
		private DailyGiftsService dailyGiftsService;

		// Token: 0x040064EA RID: 25834
		[WaitForService(true, true)]
		private IVideoAdService videoAds;

		// Token: 0x040064EB RID: 25835
		[WaitForService(true, true)]
		private LevelOfDayService levelOfDayService;

		// Token: 0x040064EC RID: 25836
		[WaitForService(true, true)]
		private BoostsService boostService;

		// Token: 0x040064ED RID: 25837
		[WaitForService(true, true)]
		private DailyDealsService dailyDealsService;

		// Token: 0x040064EE RID: 25838
		[WaitForService(true, true)]
		private DiveForTreasureService diveForTreasureService;

		// Token: 0x040064EF RID: 25839
		[WaitForService(true, true)]
		private PushNotificationService pushNotificationService;

		// Token: 0x040064F0 RID: 25840
		[WaitForService(true, true)]
		private ContentUnlockService contentUnlockService;

		// Token: 0x040064F1 RID: 25841
		[WaitForService(true, true)]
		private AssetBundleService assetBundleService;

		// Token: 0x040064F2 RID: 25842
		[WaitForService(true, true)]
		private TrackingService trackingService;

		// Token: 0x040064F3 RID: 25843
		[WaitForService(true, true)]
		private SeasonService seasonService;

		// Token: 0x040064F4 RID: 25844
		[WaitForRoot(false, false)]
		private EventSystemRoot eventSystemRoot;

		// Token: 0x040064F5 RID: 25845
		[WaitForRoot(false, false)]
		private BuildingResourceServiceRoot buildingResources;

		// Token: 0x040064F6 RID: 25846
		private static bool editModeInitialized;

		// Token: 0x040064F7 RID: 25847
		private static bool editMode;

		// Token: 0x040064F8 RID: 25848
		public static bool s_areasVisible;

		// Token: 0x040064F9 RID: 25849
		public CheatDataSource dataSource;

		// Token: 0x040064FA RID: 25850
		public CategoryDataSource categoryDataSource;

		// Token: 0x040064FB RID: 25851
		public CheatProgressionDisplay statusView;

		// Token: 0x040064FC RID: 25852
		public TownSceneLoader townLoader;

		// Token: 0x040064FD RID: 25853
		public TextAsset testState;

		// Token: 0x040064FE RID: 25854
		public AbTestChooser abTestChooser;

		// Token: 0x040064FF RID: 25855
		public CheatPopupTestSelector testablePopupSelector;

		// Token: 0x04006500 RID: 25856
		public CheatDynamicBuildingsTestSelector dynamicBuildingTestSelector;

		// Token: 0x04006501 RID: 25857
		private Dictionary<string, GameObject> toggles = new Dictionary<string, GameObject>();

		// Token: 0x04006502 RID: 25858
		private int selectedChapterIntro;

		// Token: 0x04006503 RID: 25859
		private int tournamentTrophyIndex = 24;

		// Token: 0x04006504 RID: 25860
		private WoogaSystemLanguage language;

		// Token: 0x04006505 RID: 25861
		private Dictionary<CheatCategory, Cheats[]> mapCategoryToCheats = new Dictionary<CheatCategory, Cheats[]>
		{
			{
				CheatCategory.Economy,
				new Cheats[]
				{
					Cheats.Add1Life,
					Cheats.UseLife,
					Cheats.Add1000Coins,
					Cheats.Add1000SeasonalCurrency,
					Cheats.Use1000SeasonalCurrency,
					Cheats.Add100Diamonds,
					Cheats.Add100Happiness,
					Cheats.Add100BankedDiamonds,
					Cheats.Lose1000Coins,
					Cheats.Add10GrandPrizeProgress,
					Cheats.Lose100Diamonds,
					Cheats.LosePreGameBoosts,
					Cheats.LoseInGameBoosts,
					Cheats.ClearTransactionHistory
				}
			},
			{
				CheatCategory.Tournament,
				new Cheats[]
				{
					Cheats.UnlockTournament,
					Cheats.SimulateBadNetwork,
					Cheats.PlaceNextTrophy
				}
			},
			{
				CheatCategory.DailyGift,
				new Cheats[]
				{
					Cheats.DailyGiftUnlock,
					Cheats.DailyGiftUnlock5,
					Cheats.DailyGiftReset
				}
			},
			{
				CheatCategory.Challenges,
				new Cheats[]
				{
					Cheats.ChallengeAssignNext,
					Cheats.ChallengeAdd100Paws,
					Cheats.ChallengeCompleteNextChallenge,
					Cheats.ChallengeNextDecoSet,
					Cheats.ChallengeV2TutorialReset
				}
			},
			{
				CheatCategory.Progression,
				new Cheats[]
				{
					Cheats.GoToNextArea,
					Cheats.GoToNextLevel,
					Cheats.ResetProgress,
					Cheats.CompleteCurrentQuest,
					Cheats.OpenFullQuestData
				}
			},
			{
				CheatCategory.Island,
				new Cheats[]
				{
					Cheats.ToggleEditMode,
					Cheats.ToggleAreaHelpers,
					Cheats.ToggleShaderLod,
					Cheats.ToggleCharacters,
					Cheats.ToggleBuildings,
					Cheats.Toggle25D,
					Cheats.ToggleOcean,
					Cheats.ToggleOcean,
					Cheats.ToggleLocaKeys,
					Cheats.DynamicBuildings
				}
			},
			{
				CheatCategory.ProgressionGifts,
				new Cheats[0]
			},
			{
				CheatCategory.ConfigAbTests,
				new Cheats[0]
			},
			{
				CheatCategory.SwitchGameState,
				new Cheats[0]
			},
			{
				CheatCategory.ChapterIntros,
				new Cheats[]
				{
					Cheats.IntroShowAll,
					Cheats.IntroShowFrom,
					Cheats.IntroShowNext,
					Cheats.IntroShowPrev,
					Cheats.IntroShowFrom10,
					Cheats.IntroShowFrom15
				}
			},
			{
				CheatCategory.Dialogues,
				new Cheats[0]
			},
			{
				CheatCategory.Other,
				new Cheats[]
				{
					Cheats.ReloadGame,
					Cheats.UpdateLoca,
					Cheats.ResetSeenFlags,
					Cheats.ScheduleNotfications,
					Cheats.LoadFriendsGameState,
					Cheats.HideCheats,
					Cheats.EnableTimeCheating,
					Cheats.DisableTimeCheating,
					Cheats.PushyToSelf,
					Cheats.OpenRatingFilter,
					Cheats.GdprReset,
					Cheats.ResetFruitTournamentPopups,
					Cheats.BankEventReset
				}
			},
			{
				CheatCategory.UiTests,
				new Cheats[0]
			},
			{
				CheatCategory.Ads,
				new Cheats[]
				{
					Cheats.ResetAllAds,
					Cheats.ResetChallengeAds,
					Cheats.ResetWheelAds,
					Cheats.ResetDailyGiftAds,
					Cheats.RefreshAdStats,
					Cheats.ShowAdsTestSuit
				}
			},
			{
				CheatCategory.LevelOfDay,
				new Cheats[]
				{
					Cheats.LODReset,
					Cheats.LODAddTry,
					Cheats.LODRemoveTry,
					Cheats.LODComplete,
					Cheats.LODUncomplete,
					Cheats.LODStartSoon,
					Cheats.LODEndSoon,
					Cheats.LODToggleUnlocked,
					Cheats.LODAddDayToStreak,
					Cheats.LODSubtractDayToStreak
				}
			},
			{
				CheatCategory.WeeklyEvents,
				new Cheats[]
				{
					Cheats.ResetDiveForTreasureProgress,
					Cheats.ResetPirateBreakoutProgress
				}
			},
			{
				CheatCategory.DailyDeals,
				new Cheats[]
				{
					Cheats.NextDailyDeal,
					Cheats.ExpireCurrentDailyDeal,
					Cheats.ResetDailyDeals
				}
			},
			{
				CheatCategory.ContentUnlock,
				new Cheats[]
				{
					Cheats.ToggleContentUnlock
				}
			},
			{
				CheatCategory.ErrorHandling,
				new Cheats[]
				{
					Cheats.CrashRestart,
					Cheats.CrashKill
				}
			},
			{
				CheatCategory.Sale,
				new Cheats[]
				{
					Cheats.SaleReset
				}
			},
			{
				CheatCategory.Seasonals,
				new Cheats[]
				{
					Cheats.Add1000SeasonalCurrency,
					Cheats.Use1000SeasonalCurrency,
					Cheats.Add10GrandPrizeProgress,
					Cheats.SeasonalsV3ProgressReset,
					Cheats.SeasonalsV3TutorialReset
				}
			}
		};
	}
}
