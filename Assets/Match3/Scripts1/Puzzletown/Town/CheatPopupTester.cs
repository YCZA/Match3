using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Match3.Scripts1.Audio;
using Match3.Scripts1.Puzzletown.Config;
using Match3.Scripts1.Puzzletown.Features.DailyGifts;
using Match3.Scripts1.Puzzletown.Flows;
using Match3.Scripts1.Puzzletown.GameStateSelector;
using Match3.Scripts1.Puzzletown.Match3;
using Match3.Scripts1.Puzzletown.Match3.Scoring;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Puzzletown.UI;
using Match3.Scripts1.Puzzletown.UI.Challenge;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.Signals;
using Wooga.UnityFramework;
using Match3.Scripts2.Building.Shop;
using Match3.Scripts2.PlayerData;

namespace Match3.Scripts1.Puzzletown.Town
{
	// Token: 0x020009B1 RID: 2481
	public static class CheatPopupTester
	{
		// Token: 0x06003C17 RID: 15383 RVA: 0x0012AB20 File Offset: 0x00128F20
		public static void ShowPopup(PopupWithParams popup, ILocalizationService locaService, ConfigService configService, LivesService livesService, FacebookService fbService, DailyGiftsService dailyGiftService, LevelOfDayService lodService, AudioService audioService, QuestService questService, AssetBundleService assetBundleService, SeasonService seasonService, BuildingResourceServiceRoot buildingResources)
		{
			switch (popup.popup)
			{
			case PopupToTest.EndOfContent:
				SceneManager.Instance.LoadScene<BannerEndOfContentRoot>(null);
				break;
			case PopupToTest.NotEnoughDiamonds:
				CheatPopupTester.ShowNotEnoughDiamonds(locaService);
				break;
			case PopupToTest.DiamondsBought:
				CheatPopupTester.ShowDiamondsBoughtPopup(locaService, configService);
				break;
			case PopupToTest.StarterPack:
				SceneManager.Instance.LoadScene<StarterPackRoot>(null);
				break;
			case PopupToTest.BuyBooster:
				CheatPopupTester.ShowBuyBoosterPopup(locaService);
				break;
			case PopupToTest.ConnErr:
				ConnectionErrorPopup.ShowAndWaitForClose().Start();
				break;
			case PopupToTest.ConnSucc:
				CheatPopupTester.ShowConnectionSuccessPopup();
				break;
			case PopupToTest.LivesBought:
				SceneManager.Instance.LoadSceneWithParams<PopupLivesPurchasedRoot, MaterialAmount>(new MaterialAmount("lives", 5, MaterialAmountUsage.Undefined, 0), null);
				break;
			case PopupToTest.JackpotPreview:
				SceneManager.Instance.LoadScene<PopupJackpotPreviewRoot>(null);
				break;
			case PopupToTest.FreeGift:
				CheatPopupTester.ShowFreeGiftPopup();
				break;
			case PopupToTest.LivesShopDiamonds:
				WooroutineRunner.StartCoroutine(CheatPopupTester.ShowLivesShopPopup(livesService, LivesShopOperation.SpendDiamonds), null);
				break;
			case PopupToTest.LivesShopFriends:
				WooroutineRunner.StartCoroutine(CheatPopupTester.ShowLivesShopPopup(livesService, LivesShopOperation.AskFriends), null);
				break;
			case PopupToTest.LivesShopAd:
				WooroutineRunner.StartCoroutine(CheatPopupTester.ShowLivesShopPopup(livesService, LivesShopOperation.WatchVideo), null);
				break;
			case PopupToTest.GameStateSelector:
				SceneManager.Instance.LoadSceneWithParams<GameStateSelectorRoot, MergeInfo>(new MergeInfo(new GameState(), new GameState()), null);
				break;
			case PopupToTest.TournamentTeaser:
				CheatPopupTester.ShowTournamentTeaser();
				break;
			case PopupToTest.TournamentQualifying:
				CheatPopupTester.ShowTournamentQualifying(false);
				break;
			case PopupToTest.TournamentQualified:
				CheatPopupTester.ShowTournamentQualifying(true);
				break;
			case PopupToTest.TournamentStandings:
				WooroutineRunner.StartCoroutine(CheatPopupTester.ShowTournamentStandingsPopup(fbService), null);
				break;
			case PopupToTest.TournamentOverLost:
				CheatPopupTester.ShowTournamentOverResults(43, configService);
				break;
			case PopupToTest.TournamentOverGold:
				CheatPopupTester.ShowTournamentOverResults(1, configService);
				break;
			case PopupToTest.TournamentOverSilver:
				CheatPopupTester.ShowTournamentOverResults(2, configService);
				break;
			case PopupToTest.TournamentOverBronze:
				CheatPopupTester.ShowTournamentOverResults(3, configService);
				break;
			case PopupToTest.TournamentOverTop10:
				CheatPopupTester.ShowTournamentOverResults(8, configService);
				break;
			case PopupToTest.TournamentDontGiveUp:
				WooroutineRunner.StartCoroutine(CheatPopupTester.ShowTournamentDontGiveUpRoutine(), null);
				break;
			case PopupToTest.TourPrizePrev:
				CheatPopupTester.ShowTournamentRewardPreview();
				break;
			case PopupToTest.GrandPrizeNoProgress:
				CheatPopupTester.ShowGrandPrizeNoProgressRoutine(buildingResources);
				break;
			case PopupToTest.GrandPrizeSomeProgress:
				CheatPopupTester.ShowGrandPrizeSomeProgressRoutine(buildingResources);
				break;
			case PopupToTest.GrandPrizeAllProgress:
				CheatPopupTester.ShowGrandPrizeAllProgressRoutine(buildingResources);
				break;
			case PopupToTest.SeasonalPromo:
			{
				TownUiRoot townUiRoot = global::UnityEngine.Object.FindObjectOfType<TownUiRoot>();
				WooroutineRunner.StartCoroutine(CheatPopupTester.ShowSeasonalPopupFlow(seasonService, townUiRoot.ShopDialog), null);
				break;
			}
			case PopupToTest.MissingAssets:
				WooroutineRunner.StartCoroutine(CheatPopupTester.ShowMissingAssetsPopupFlow(), null);
				break;
			case PopupToTest.DailyGift:
				CheatPopupTester.ShowDailyGiftPopup(dailyGiftService);
				break;
			case PopupToTest.LevelOfDayStart:
				WooroutineRunner.StartCoroutine(CheatPopupTester.ShowLevelOfDayStartRoutine(lodService), null);
				break;
			case PopupToTest.LevelOfDayRetry:
				WooroutineRunner.StartCoroutine(CheatPopupTester.ShowLevelOfDayRetryRoutine(lodService), null);
				break;
			case PopupToTest.ChallengeTeaser:
				CheatPopupTester.ShowChallengeTeaser();
				break;
			case PopupToTest.ChallengePreview:
				CheatPopupTester.ShowChallengePreview(configService);
				break;
			case PopupToTest.TreasureChest:
				CheatPopupTester.ShowTreasureChest();
				break;
			case PopupToTest.RateMeFlow:
				CheatPopupTester.ShowRateMeFlow();
				break;
			case PopupToTest.NewArea:
				WooroutineRunner.StartCoroutine(CheatPopupTester.ShowNewAreaRoutine(), null);
				break;
			case PopupToTest.AdWheelReward:
				WooroutineRunner.StartCoroutine(CheatPopupTester.ShowAdWheelRewardsRoutine(audioService, locaService, false), null);
				break;
			case PopupToTest.AdWheelRewardJackpot:
				WooroutineRunner.StartCoroutine(CheatPopupTester.ShowAdWheelRewardsRoutine(audioService, locaService, true), null);
				break;
			case PopupToTest.TournamentReward:
				WooroutineRunner.StartCoroutine(CheatPopupTester.ShowTournamentRewardsRoutine(), null);
				break;
			case PopupToTest.QuestComplete:
				WooroutineRunner.StartCoroutine(CheatPopupTester.ShowQuestCompletePopupRoutine(questService), null);
				break;
			case PopupToTest.AnniversaryPromo:
			{
				TownUiRoot townUiRoot2 = global::UnityEngine.Object.FindObjectOfType<TownUiRoot>();
				WooroutineRunner.StartCoroutine(CheatPopupTester.ShowAnniversaryPopupFlow(assetBundleService, configService.SbsConfig, townUiRoot2.ShopDialog), null);
				break;
			}
			case PopupToTest.PopupSale:
				SceneManager.Instance.LoadSceneWithParams<SalePopupRoot, string>("island", null);
				break;
			case PopupToTest.PopupDailyDealOffer:
				SceneManager.Instance.LoadScene<PopupDailyDealOfferRoot>(null);
				break;
			case PopupToTest.TownShopSeasonalInfo:
				SceneManager.Instance.LoadScene<TownShopSeasonalInfoRoot>(null);
				break;
			}
		}

		// Token: 0x06003C18 RID: 15384 RVA: 0x0012AEE8 File Offset: 0x001292E8
		private static IEnumerator ShowQuestCompletePopupRoutine(QuestService questService)
		{
			Wooroutine<PopupQuestCompleteRoot> popupQuestComplete = SceneManager.Instance.LoadScene<PopupQuestCompleteRoot>(null);
			yield return popupQuestComplete;
			List<QuestData> questList = questService.questManager.QuestConfigs.ToList<QuestData>();
			QuestData randomQuest = questList.RandomElement(false);
			popupQuestComplete.ReturnValue.Show(randomQuest);
			yield return popupQuestComplete.ReturnValue.onDestroyed;
			yield break;
		}

		// Token: 0x06003C19 RID: 15385 RVA: 0x0012AF04 File Offset: 0x00129304
		private static IEnumerator ShowTournamentRewardsRoutine()
		{
			Materials rewards = new Materials(new List<MaterialAmount>
			{
				new MaterialAmount("coins", 123, MaterialAmountUsage.Undefined, 0),
				new MaterialAmount("diamonds", 45, MaterialAmountUsage.Undefined, 0),
				new MaterialAmount("boost_pre_double_fish", 1, MaterialAmountUsage.Undefined, 0)
			});
			Wooroutine<TournamentRewardsRoot> rewardPopup = SceneManager.Instance.LoadSceneWithParams<TournamentRewardsRoot, Materials>(rewards, null);
			yield return rewardPopup;
			rewardPopup.ReturnValue.Show();
			yield return rewardPopup.ReturnValue.onDisabled.Await();
			yield break;
		}

		// Token: 0x06003C1A RID: 15386 RVA: 0x0012AF18 File Offset: 0x00129318
		private static IEnumerator ShowAdWheelRewardsRoutine(AudioService audioService, ILocalizationService localizationService, bool isJackpot)
		{
			List<WheelPrize> prizes = new List<WheelPrize>
			{
				new WheelPrize
				{
					prize = "HammerBoost",
					amount = 1,
					position = 1,
					probability = 25
				},
				new WheelPrize
				{
					prize = "Diamonds",
					amount = 10,
					position = 2,
					probability = 25
				}
			};
			Wooroutine<PopupWheelRewardRoot> rewardPopup = SceneManager.Instance.LoadSceneWithParams<PopupWheelRewardRoot, List<WheelPrize>>(prizes, null);
			yield return rewardPopup;
			if (isJackpot)
			{
				audioService.PlaySFX(AudioId.AdWheelJackpot, false, false, false);
			}
			else
			{
				audioService.PlaySFX(AudioId.AdWheelYouWon, false, false, false);
			}
			rewardPopup.ReturnValue.titleLable.text = localizationService.GetText((!isJackpot) ? "spinwheel.reward.title" : "spinwheel.reward.jackpot.title", new LocaParam[0]);
			yield return rewardPopup.ReturnValue.onClose;
			yield break;
		}

		// Token: 0x06003C1B RID: 15387 RVA: 0x0012AF44 File Offset: 0x00129344
		private static IEnumerator ShowNewAreaRoutine()
		{
			yield return new SceneLoaderFlow<BannerNewAreaRoot>(true).ExecuteRoutine();
			yield break;
		}

		// Token: 0x06003C1C RID: 15388 RVA: 0x0012AF58 File Offset: 0x00129358
		private static void ShowRateMeFlow()
		{
			// SceneManager.Instance.LoadScene<PopupRatingRoot>(null);
		}

		// Token: 0x06003C1D RID: 15389 RVA: 0x0012AF66 File Offset: 0x00129366
		private static void ShowTreasureChest()
		{
			SceneManager.Instance.LoadSceneWithParams<BankRoot, BankRoot.BankContext>(BankRoot.BankContext.testing, null);
		}

		// Token: 0x06003C1E RID: 15390 RVA: 0x0012AF78 File Offset: 0x00129378
		private static IEnumerator ShowLevelOfDayStartRoutine(LevelOfDayService levelOfDayService)
		{
			Wooroutine<LevelConfig> getConfigRoutine = levelOfDayService.GetCurrentLevelOfDayConfig(null);
			yield return getConfigRoutine;
			LevelConfig levelConfig = getConfigRoutine.ReturnValue;
			Wooroutine<M3_LevelOfDayStartRoot> loadRoutine = SceneManager.Instance.LoadSceneWithParams<M3_LevelOfDayStartRoot, LevelConfig>(levelConfig, null);
			yield return loadRoutine;
			loadRoutine.ReturnValue.Hide(false);
			yield break;
		}

		// Token: 0x06003C1F RID: 15391 RVA: 0x0012AF94 File Offset: 0x00129394
		private static IEnumerator ShowLevelOfDayRetryRoutine(LevelOfDayService levelOfDayService)
		{
			Wooroutine<LevelConfig> getConfigRoutine = levelOfDayService.GetCurrentLevelOfDayConfig(null);
			yield return getConfigRoutine;
			LevelConfig levelConfig = getConfigRoutine.ReturnValue;
			Match3Score score = new Match3Score(levelConfig, LevelPlayMode.LevelOfTheDay);
			score.success = false;
			score.isUITestingOnly = true;
			SceneManager.Instance.LoadSceneWithParams<M3_LevelOfDayFailedRoot, Match3Score>(score, null);
			yield break;
		}

		// Token: 0x06003C20 RID: 15392 RVA: 0x0012AFB0 File Offset: 0x001293B0
		private static List<FacebookData.Friend> MockTellAFriendData(int friendCount)
		{
			List<FacebookData.Friend> list = new List<FacebookData.Friend>();
			List<string> list2 = new List<string>
			{
				"621971161",
				"567133863",
				"656502801",
				"583568041"
			};
			List<string> list3 = new List<string>
			{
				"John",
				"Paul",
				"George",
				"Ringo"
			};
			int num = 0;
			while (num < list2.Count && num < friendCount)
			{
				FacebookData.Friend item = new FacebookData.Friend(list2[num], list3[num]);
				list.Add(item);
				num++;
			}
			list.Shuffle<FacebookData.Friend>();
			return list;
		}

		// Token: 0x06003C21 RID: 15393 RVA: 0x0012B074 File Offset: 0x00129474
		private static IEnumerator ShowMissingAssetsPopupFlow()
		{
			bool hasBeenTriggered = PopupMissingAssetsRoot.Trigger.hasBeenTriggered;
			PopupMissingAssetsRoot.Trigger trigger = new PopupMissingAssetsRoot.Trigger(null, null);
			yield return trigger.Run();
			PopupMissingAssetsRoot.Trigger.hasBeenTriggered = hasBeenTriggered;
			yield break;
		}

		// Token: 0x06003C22 RID: 15394 RVA: 0x0012B088 File Offset: 0x00129488
		private static IEnumerator ShowSeasonalPopupFlow(SeasonService seasonService, TownShopRoot shop)
		{
			PopupSeasonalPromoRoot.Trigger trigger = new PopupSeasonalPromoRoot.Trigger(null, seasonService, shop, null);
			yield return trigger.Run();
			yield break;
		}

		// Token: 0x06003C23 RID: 15395 RVA: 0x0012B0AC File Offset: 0x001294AC
		private static IEnumerator ShowAnniversaryPopupFlow(AssetBundleService assetBundleService, PTConfig config, TownShopRoot shop)
		{
			PromoPopupTrigger trigger = new PromoPopupTrigger(null, assetBundleService, config, null, shop);
			yield return trigger.Run();
			yield break;
		}

		// Token: 0x06003C24 RID: 15396 RVA: 0x0012B0D5 File Offset: 0x001294D5
		private static void ShowDailyGiftPopup(DailyGiftsService service)
		{
			service.ForceCurrentDay(global::UnityEngine.Random.Range(4, 12));
			SceneManager.Instance.LoadScene<DailyGiftsRoot>(null);
		}

		// Token: 0x06003C25 RID: 15397 RVA: 0x0012B0F4 File Offset: 0x001294F4
		private static IEnumerator ShowTournamentDontGiveUpRoutine()
		{
			TournamentScore score = new TournamentScore(TournamentType.Bomb, 12, 5);
			Wooroutine<PopupTournamentLevelFailRoot> loadRoutine = SceneManager.Instance.LoadSceneWithParams<PopupTournamentLevelFailRoot, TournamentScore>(score, null);
			yield return loadRoutine;
			PopupTournamentLevelFailRoot popup = loadRoutine.ReturnValue;
			string multiplierAsString = string.Format("_x{0}", 5);
			popup.tournamentMultiplierIcon.sprite = popup.multiplierSprites.GetSimilar(multiplierAsString);
			loadRoutine.ReturnValue.Show();
			yield break;
		}

		// Token: 0x06003C26 RID: 15398 RVA: 0x0012B108 File Offset: 0x00129508
		private static void ShowTournamentTeaser()
		{
			SceneManager.Instance.LoadScene<TournamentTeaserRoot>(null);
		}

		// Token: 0x06003C27 RID: 15399 RVA: 0x0012B116 File Offset: 0x00129516
		private static void ShowGrandPrizeNoProgressRoutine(BuildingResourceServiceRoot buildingResources)
		{
			GrandPrizePurchaseFlow.CreateWithMockData(new SeasonPrizeInfo("easter_2019", 0, null, 0)).Start();
		}

		// Token: 0x06003C28 RID: 15400 RVA: 0x0012B130 File Offset: 0x00129530
		private static void ShowGrandPrizeSomeProgressRoutine(BuildingResourceServiceRoot buildingResources)
		{
			GrandPrizePurchaseFlow.CreateWithMockData(new SeasonPrizeInfo("easter_2019", 30, null, 0)).Start();
		}

		// Token: 0x06003C29 RID: 15401 RVA: 0x0012B14B File Offset: 0x0012954B
		private static void ShowGrandPrizeAllProgressRoutine(BuildingResourceServiceRoot buildingResources)
		{
			GrandPrizePurchaseFlow.CreateWithMockData(new SeasonPrizeInfo("easter_2019", 99999, null, 0)).Start();
		}

		// Token: 0x06003C2A RID: 15402 RVA: 0x0012B16C File Offset: 0x0012956C
		private static void ShowChallengePreview(ConfigService configService)
		{
			List<BuildingConfig> list = new List<BuildingConfig>();
			foreach (BuildingConfig buildingConfig in configService.buildingConfigList.buildings)
			{
				if (buildingConfig.challenge_set == 5)
				{
					list.Add(buildingConfig);
				}
			}
			SceneManager.Instance.LoadSceneWithParams<ChallengeV2InfoRoot, List<BuildingConfig>>(list, null);
		}

		// Token: 0x06003C2B RID: 15403 RVA: 0x0012B1C3 File Offset: 0x001295C3
		private static void ShowChallengeTeaser()
		{
			SceneManager.Instance.LoadScene<ChallengeTeaserRoot>(null);
		}

		// Token: 0x06003C2C RID: 15404 RVA: 0x0012B1D1 File Offset: 0x001295D1
		private static void ShowTournamentRewardPreview()
		{
			SceneManager.Instance.LoadSceneWithParams<TournamentRewardPreviewRoot, LeagueModel>(LeagueModelFactory.MockModelForRewardsPreview(), null);
		}

		// Token: 0x06003C2D RID: 15405 RVA: 0x0012B1E4 File Offset: 0x001295E4
		private static IEnumerator ShowTournamentStandingsPopup(FacebookService fbService)
		{
			LeagueModel lm = LeagueModelFactory.MockModelForTesting(null);
			yield return WooroutineRunner.StartCoroutine(CheatPopupTester.DoShowStandings(lm), null);
			fbService.CleanUnknownOthersImageFromCache();
			yield break;
		}

		// Token: 0x06003C2E RID: 15406 RVA: 0x0012B200 File Offset: 0x00129600
		private static void ShowTournamentQualifying(bool hasQualified)
		{
			LeagueModel leagueModel = LeagueModelFactory.MockModelForQualifyingTesting(hasQualified);
			WooroutineRunner.StartCoroutine(CheatPopupTester.ShowQualifyingPopup(leagueModel), null);
		}

		// Token: 0x06003C2F RID: 15407 RVA: 0x0012B224 File Offset: 0x00129624
		private static void ShowTournamentOverResults(int playerPosition, ConfigService configService)
		{
			LeagueModel leagueModel = LeagueModelFactory.MockResultTestModel(playerPosition, TournamentType.Strawberry);
			WooroutineRunner.StartCoroutine(CheatPopupTester.ShowResultsPopup(leagueModel, configService), null);
		}

		// Token: 0x06003C30 RID: 15408 RVA: 0x0012B248 File Offset: 0x00129648
		private static IEnumerator ShowQualifyingPopup(LeagueModel leagueModel)
		{
			Wooroutine<TournamentQualifyingRoot> tourneyPopup = SceneManager.Instance.LoadSceneWithParams<TournamentQualifyingRoot, LeagueModel>(leagueModel, null);
			yield return tourneyPopup;
			yield return tourneyPopup.ReturnValue.onClose.Await<TournamentPopupFlowOperation>();
			yield break;
		}

		// Token: 0x06003C31 RID: 15409 RVA: 0x0012B264 File Offset: 0x00129664
		private static IEnumerator ShowResultsPopup(LeagueModel leagueModel, ConfigService configService)
		{
			Wooroutine<TournamentResultsV2Root> tourneyPopup = SceneManager.Instance.LoadSceneWithParams<TournamentResultsV2Root, LeagueModel>(leagueModel, null);
			yield return tourneyPopup;
			tourneyPopup.ReturnValue.Show();
			AwaitSignal<TournamentPopupFlowOperation> waitForTourneyPopupClose = tourneyPopup.ReturnValue.onClose.Await<TournamentPopupFlowOperation>();
			yield return waitForTourneyPopupClose;
			DecoTrophyItemWon decoTrophyToPlace = leagueModel.GetDecoTrophyWon();
			BuildingConfig buildingConfig = configService.buildingConfigList.GetConfig(decoTrophyToPlace);
			yield return new ForceUserPlaceDecoFlow(buildingConfig).Start();
			yield break;
		}

		// Token: 0x06003C32 RID: 15410 RVA: 0x0012B288 File Offset: 0x00129688
		private static IEnumerator DoShowStandings(LeagueModel param)
		{
			Wooroutine<TournamentStandingsRoot> standingsPopup = SceneManager.Instance.LoadSceneWithParams<TournamentStandingsRoot, LeagueModel>(param, null);
			yield return standingsPopup;
			standingsPopup.ReturnValue.Show();
			AwaitSignal<bool> close = standingsPopup.ReturnValue.onClose.Await<bool>();
			yield return close;
			yield break;
		}

		// Token: 0x06003C33 RID: 15411 RVA: 0x0012B2A4 File Offset: 0x001296A4
		private static IEnumerator ShowLivesShopPopup(LivesService livesService, LivesShopOperation opToTest)
		{
			while (!livesService.IsCurrentlyUnlimitedLives && livesService.CurrentLives > 0)
			{
				livesService.UseLife();
			}
			Wooroutine<LivesShopRoot> lsPopup = SceneManager.Instance.LoadScene<LivesShopRoot>(null);
			yield return lsPopup;
			LivesShopRoot popupInstance = lsPopup.ReturnValue;
			yield return lsPopup.ReturnValue.TestLayoutRoutine(opToTest);
			yield break;
		}

		// Token: 0x06003C34 RID: 15412 RVA: 0x0012B2C8 File Offset: 0x001296C8
		private static void ShowFreeGiftPopup()
		{
			Gift gift = new Gift();
			gift.body_textkey = "ui.gift_link.body.valid";
			gift.id = string.Empty;
			gift.gift_link_id = string.Empty;
			gift.gift_type = "lives_unlimited";
			gift.gift_amount = 1;
			SceneManager.Instance.LoadSceneWithParams<PopupFreeGiftRoot, Gift>(gift, null);
		}

		// Token: 0x06003C35 RID: 15413 RVA: 0x0012B31B File Offset: 0x0012971B
		private static void ShowConnectionSuccessPopup()
		{
			SceneManager.Instance.LoadSceneWithParams<PopupFacebookRoot, FacebookPopupParameters>(new FacebookPopupParameters(FacebookPopupState.Connected, FacebookLoginContext.settings), null);
		}

		// Token: 0x06003C36 RID: 15414 RVA: 0x0012B330 File Offset: 0x00129730
		private static void ShowBuyBoosterPopup(ILocalizationService locaService)
		{
			MaterialAmount cost = new MaterialAmount("diamonds", 99, MaterialAmountUsage.Undefined, 0);
			MaterialAmount materialAmount = new MaterialAmount("boost_hammer", 4, MaterialAmountUsage.Undefined, 0);
			string text = "boost_";
			string text2;
			string text3;
			if (materialAmount.type.StartsWith(text))
			{
				string arg = materialAmount.type.Substring(text.Length);
				text2 = locaService.GetText(string.Format("ui.boosts.ingame.add.{0}.title", arg), new LocaParam[0]);
				text3 = locaService.GetText(string.Format("ui.boosts.ingame.add.{0}.body", arg), new LocaParam[0]);
			}
			else
			{
				text2 = string.Format(locaService.GetText("ui.shared.purchase.title", new LocaParam[0]), materialAmount.FormatName(locaService));
				text3 = string.Format(locaService.GetText("ui.shared.purchase.content", new LocaParam[0]), materialAmount.Format(locaService), cost.Format(locaService));
			}
			object[] array = new object[5];
			array[0] = TextData.Title(text2);
			array[1] = TextData.Content(text3);
			array[2] = new CloseButton(delegate()
			{
			});
			array[3] = materialAmount;
			array[4] = new PricedButtonWithCallback(cost, delegate()
			{
			});
			PopupDialogRoot.Show(array).Start();
		}

		// Token: 0x06003C37 RID: 15415 RVA: 0x0012B48C File Offset: 0x0012988C
		private static void ShowDiamondsBoughtPopup(ILocalizationService locaService, ConfigService configService)
		{
			string text = locaService.GetText("ui.shared.purchase.diamonds.success", new LocaParam[0]);
			string text2 = string.Format(locaService.GetText("ui.shared.purchase.diamonds.report", new LocaParam[0]), locaService.GetText("shop.diamonds.title.diamonds_m", new LocaParam[0]), "1$");
			string text3 = locaService.GetText("ui.shared.purchase.diamonds.proceed", new LocaParam[0]);
			List<object> list = new List<object>();
			list.Add(TextData.Title(text));
			list.Add(TextData.Content(text2));
			List<MaterialAmount> list2 = new List<MaterialAmount>
			{
				new MaterialAmount("diamonds", 999, MaterialAmountUsage.Undefined, 0)
			};
			if (configService.FeatureSwitchesConfig.new_shop_layout)
			{
				list.Add(new Bundle
				{
					materials = list2
				});
			}
			else
			{
				list.Add(list2);
			}
			list.Add(new LabeledButtonWithCallback(text3, null));
			PopupDialogRoot.Show(list.ToArray()).Start();
		}

		// Token: 0x06003C38 RID: 15416 RVA: 0x0012B588 File Offset: 0x00129988
		private static void ShowNotEnoughDiamonds(ILocalizationService locaService)
		{
			MaterialAmount materialAmount = new MaterialAmount("diamonds", 99, MaterialAmountUsage.Undefined, 0);
			string text = string.Format(locaService.GetText("ui.shared.purchase.title", new LocaParam[0]), materialAmount.FormatName(locaService));
			string text2 = string.Format(locaService.GetText("ui.shared.purchase.diamonds.title", new LocaParam[0]), materialAmount.Format(locaService));
			string text3 = locaService.GetText("ui.shared.purchase.diamonds.button", new LocaParam[0]);
			object[] array = new object[5];
			array[0] = TextData.Title(text);
			array[1] = TextData.Content(text2);
			array[2] = new CloseButton(delegate()
			{
			});
			array[3] = new MaterialAmount("diamonds", 0, MaterialAmountUsage.Undefined, 0);
			array[4] = new LabeledButtonWithCallback(text3, delegate()
			{
			});
			PopupDialogRoot.Show(array).Start();
		}
	}
}
