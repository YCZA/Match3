using System;
using Match3.Scripts1.PTTracking;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using Match3.Scripts1.Puzzletown.Build;
using Match3.Scripts1.Puzzletown.Config;
using Match3.Scripts1.Puzzletown.Match3;
using Match3.Scripts1.Puzzletown.Match3.Scoring;
using Match3.Scripts1.Town;
using Match3.Scripts1.Wooga.Core.DeviceInfo;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.Legal;
using Match3.Scripts1.Wooga.Services.ErrorAnalytics;
using Match3.Scripts1.Wooga.Services.Tracking;
using Match3.Scripts1.Wooga.Services.Tracking.Parameters;
using Match3.Scripts1.Wooga.Services.Tracking.SessionTracker;
using Match3.Scripts1.Wooga.Services.Tracking.Tools.PersistentDataImpl;
using Match3.Scripts1.Wooga.Signals;
using Wooga.UnityFramework;
using Match3.Scripts2.Env;
using Match3.Scripts2.PlayerData;
using Match3.Scripts2.Shop;
using UnityEngine;
using Wooga.Foundation.Json; //using Facebook.Unity;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x0200081E RID: 2078
	public class TrackingService : AService
	{
		// Token: 0x0600337B RID: 13179 RVA: 0x000F4637 File Offset: 0x000F2A37
		public TrackingService()
		{
			WooroutineRunner.StartCoroutine(this.LoadRoutine(), null);
		}

		// 反初始化(关闭数据库)
		public override void DeInit()
		{
			base.DeInit();
			Tracking.CleanUp();
			SqlitePersistentData.Close();
		}

		// Token: 0x1700081E RID: 2078
		// (get) Token: 0x0600337C RID: 13180 RVA: 0x000F4662 File Offset: 0x000F2A62
		private static string TrackingEndPoint
		{
			get
			{
				// return "pta.t.wooga.com/w";
				return "amegvfklooisadfkljaadsf233333";
			}
		}

		// Token: 0x1700081F RID: 2079
		// (get) Token: 0x0600337D RID: 13181 RVA: 0x000F4669 File Offset: 0x000F2A69
		public string trackerPath
		{
			get
			{
				return Path.Combine(Application.persistentDataPath, "pt_roundTracker");
			}
		}

		// Token: 0x17000820 RID: 2080
		// (get) Token: 0x0600337E RID: 13182 RVA: 0x000F467A File Offset: 0x000F2A7A
		public int RoundsPlayed
		{
			get
			{
				return this.roundTrack.RoundCountPlayed;
			}
		}

		// Token: 0x0600337F RID: 13183 RVA: 0x000F4687 File Offset: 0x000F2A87
		public int GetSevenDaysRoundCount()
		{
			if (this.roundTrack != null)
			{
				return this.roundTrack.GetRoundsCountSevenDays();
			}
			Log.Warning("Could not retrieve the rounds played count", null, null);
			return -1;
		}

		// Token: 0x06003380 RID: 13184 RVA: 0x000F46B0 File Offset: 0x000F2AB0
		private IEnumerator LoadRoutine()
		{
			yield return ServiceLocator.Instance.Inject(this);
			this.roundTrack = new PTRoundTracker();
			this.LoadRoundTracking();
			this.woogaParams = new TrackingService.WoogaParameters(this.localNotificationService, this.settings, this.pushNotificationService, this.sbsService.ConfigService.PersonalizationString);
			this.gameParams = new TrackingService.GameParameters(this.gameState, this.progression, this.roundTrack, this.facebookService);
			Tracking.AddParameterProvider(this.woogaParams);
			Tracking.AddParameterProvider(this.gameParams);
			Tracking.Init(TrackingService.TrackingEndPoint, null);
			Tracking.onSessionEnd = new Action<Dictionary<string, object>>(this.OnSessionEnd);
			Tracking.onSessionStart = new SessionTracker.SessionStartCallback(this.OnSessionStart);
			Tracking.Environment = GameEnvironment.CurrentTrackingEnvironment;
			//Tracking.sbsUserId = SBS.Authentication.GetUserContext().user_id;
			MemoryWarningDelegate.SetCallback(new Action(this.OnLowMemory));
			Signal<QuestProgress, int> onQuestTaskCompleted = this.questService.questManager.OnQuestTaskCompleted;
			if (TrackingService._003C_003Ef__mg_0024cache0 == null)
			{
				TrackingService._003C_003Ef__mg_0024cache0 = new Action<QuestProgress, int>(TrackingService.HandleQuestTaskComplete);
			}
			onQuestTaskCompleted.AddListener(TrackingService._003C_003Ef__mg_0024cache0);
			Signal<QuestProgress> onQuestChanged = this.questService.questManager.OnQuestChanged;
			if (TrackingService._003C_003Ef__mg_0024cache1 == null)
			{
				TrackingService._003C_003Ef__mg_0024cache1 = new Action<QuestProgress>(TrackingService.HandleQuestChanged);
			}
			onQuestChanged.AddListener(TrackingService._003C_003Ef__mg_0024cache1);
			Signal<QuestProgress> onQuestCollected = this.questService.questManager.OnQuestCollected;
			if (TrackingService._003C_003Ef__mg_0024cache2 == null)
			{
				TrackingService._003C_003Ef__mg_0024cache2 = new Action<QuestProgress>(TrackingService.HandleQuestComplete);
			}
			onQuestCollected.AddListener(TrackingService._003C_003Ef__mg_0024cache2);
			this.questService.questManager.OnQuestCollected.AddListener(delegate(QuestProgress progress)
			{
				QuestData configData = progress.configData;
				this.TrackQuestCollected(configData, progress);
			});
			this.questService.questManager.OnQuestChanged.AddListener(delegate(QuestProgress progress)
			{
				QuestData configData = progress.configData;
				if (progress.status != QuestProgress.Status.started)
				{
					return;
				}
				if (configData.id == "elsie3_x")
				{
					this.TrackFunnelEvent("192_quest_open_ancestor_apology", 192, null);
				}
				else if (configData.id == "elsie4_x")
				{
					this.TrackFunnelEvent("345_quest_open_home_where_hut_is", 345, null);
				}
				else if (configData.id == "cubby1_x")
				{
					this.TrackFunnelEvent("440_quest_open_guard_kitty", 440, null);
				}
			});
			this.progression.onAreaUnlocked.AddListener(new Action<int>(this.TrackAreaUnlock));
			Tracking.abTestGroups = this.sbsService.ConfigService.AbTests;
			base.OnInitialized.Dispatch();
			//this.facebookService.OnRequestSent.AddListener(new Action<FacebookRequest, IAppRequestResult>(this.TrackRequest));
			this.funnelTrack = new TrackingSet(Application.persistentDataPath + "/funnelTrack.json.txt");
			this.funnelTrack.Init();
			this.TrackFunnelEvent("005_game_open", 5, null);
			yield return null;
			yield break;
		}

		// Token: 0x06003381 RID: 13185 RVA: 0x000F46CC File Offset: 0x000F2ACC
		public void Track(PendingRewards.TrackingCall call)
		{
			Dictionary<string, object> dictionary = JSON.Deserialize<Dictionary<string, object>>(call.parametersJson);
			Tracking.Track(call.evtName, dictionary);
			WoogaDebug.Log(new object[]
			{
				"Track Call",
				call.evtName,
				dictionary
			});
		}

		// Token: 0x06003382 RID: 13186 RVA: 0x000F4714 File Offset: 0x000F2B14
		public void TrackFunnelEvent(string eventID, int stepNumber, string st1 = null)
		{
			if (this.funnelTrack.DidInsert(eventID))
			{
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				dictionary["step_name"] = eventID;
				dictionary["step_number"] = stepNumber;
				if (!string.IsNullOrEmpty(st1))
				{
					dictionary["st1"] = st1;
				}
				Tracking.Track("funnel", dictionary);
				WoogaDebug.Log(new object[]
				{
					"Track: funnel",
					dictionary
				});
			}
		}

		// Token: 0x06003383 RID: 13187 RVA: 0x000F4790 File Offset: 0x000F2B90
		public void TrackGameStateSelection(bool recommendedServer, bool selectedServer, GameState serverGameState)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			ResourceDataService resourceDataService = new ResourceDataService(() => serverGameState);
			dictionary["user_choice"] = ((!selectedServer) ? "local" : "server");
			dictionary["sug"] = ((!recommendedServer) ? "local" : "server");
			dictionary["server_coins"] = resourceDataService.GetAmount("coins").ToString();
			dictionary["server_dia"] = resourceDataService.GetAmount("diamonds").ToString();
			dictionary["server_lvl"] = (serverGameState.progression.tiers.Count + 1).ToString();
			dictionary["server_ts"] = serverGameState.timestamp.ToString();
			dictionary["server_points"] = resourceDataService.GetAmount("harmony").ToString();
			Tracking.Track("mig", dictionary);
			WoogaDebug.Log(new object[]
			{
				"Track: migration"
			});
		}

		// Token: 0x06003384 RID: 13188 RVA: 0x000F48E0 File Offset: 0x000F2CE0
		public void TrackRewards(string reward_type, string reward_info1, string reward_info2, string reward_info3, int coins, int diamonds)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["action"] = "gained";
			dictionary["round_id"] = this.currentOrLastRoundID;
			dictionary["coins_amt"] = coins;
			dictionary["dia_amt"] = diamonds;
			dictionary["det1"] = reward_type;
			dictionary["det2"] = reward_info1;
			dictionary["det3"] = reward_info2;
			dictionary["det4"] = reward_info3;
			Tracking.Track("res", dictionary);
			WoogaDebug.Log(new object[]
			{
				"Track: res",
				dictionary
			});
		}

		// Token: 0x06003385 RID: 13189 RVA: 0x000F498C File Offset: 0x000F2D8C
		public PendingRewards.TrackingCall GetDailyGiftTrackingCall(Materials rewards, bool withBonus, int day)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["action"] = "gained";
			dictionary["det1"] = "daily_gift";
			dictionary["det2"] = "day_" + day;
			if (withBonus)
			{
				dictionary["det3"] = "with_bonus";
			}
			this.AddMaterials(dictionary, rewards);
			string parametersJson = JSON.Serialize(dictionary, false, 1, ' ');
			return new PendingRewards.TrackingCall
			{
				evtName = "res",
				parametersJson = parametersJson
			};
		}

		// Token: 0x06003386 RID: 13190 RVA: 0x000F4A1C File Offset: 0x000F2E1C
		public PendingRewards.TrackingCall GetWeeklyEventRewardCall(AWeeklyEventDataService dataService, Materials rewards, int level)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["action"] = "gained";
			dictionary["det1"] = ((dataService.EventType != WeeklyEventType.DiveForTreasure) ? "pirate_Breakout_chest" : "treasure_diving_chest");
			dictionary["det2"] = dataService.EventId;
			dictionary["det3"] = level;
			this.AddMaterials(dictionary, rewards);
			string parametersJson = JSON.Serialize(dictionary, false, 1, ' ');
			return new PendingRewards.TrackingCall
			{
				evtName = "res",
				parametersJson = parametersJson
			};
		}

		// Token: 0x06003387 RID: 13191 RVA: 0x000F4AB3 File Offset: 0x000F2EB3
		protected string GetDecoTrophyWon(LeagueModel league)
		{
			return league.GetDecoTrophyWon().AsString();
		}

		// Token: 0x06003388 RID: 13192 RVA: 0x000F4AC0 File Offset: 0x000F2EC0
		public void TrackGrandPrizePurchase(SeasonPrizeInfo info, SeasonService.PriceInfo pricing, string grandPriceItem)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["action"] = "spent";
			dictionary["det1"] = "grand_prize_offer";
			dictionary["det2"] = info.name;
			dictionary["det3"] = grandPriceItem;
			dictionary["det4"] = (int)pricing.discountPercent;
			dictionary["dia_amt"] = pricing.discountPrice;
			Tracking.Track("res", dictionary);
			WoogaDebug.Log(new object[]
			{
				"Track: res",
				dictionary
			});
		}

		// Token: 0x06003389 RID: 13193 RVA: 0x000F4B64 File Offset: 0x000F2F64
		public void TrackTournamentRewards(LeagueModel league, Materials rewards)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["action"] = "gained";
			dictionary["det1"] = "tournament";
			dictionary["det2"] = league.config.id;
			dictionary["det3"] = league.GetPlayerPosition();
			dictionary["trophy"] = this.GetDecoTrophyWon(league);
			this.AddMaterials(dictionary, rewards);
			Tracking.Track("res", dictionary);
			WoogaDebug.Log(new object[]
			{
				"Track: res",
				dictionary
			});
		}

		// Token: 0x0600338A RID: 13194 RVA: 0x000F4C00 File Offset: 0x000F3000
		public void TrackFreeGifts(string reward_type, int reward_amout, string gift_link_id)
		{
			bool flag = !string.IsNullOrEmpty(gift_link_id);
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["action"] = "gained";
			dictionary["det1"] = ((!flag) ? "free_gift" : "gift_link");
			dictionary["det2"] = ((!flag) ? "session_start" : gift_link_id);
			dictionary[reward_type] = reward_amout;
			Tracking.Track("res", dictionary);
			WoogaDebug.Log(new object[]
			{
				"Track: res",
				dictionary
			});
		}

		// Token: 0x0600338B RID: 13195 RVA: 0x000F4C98 File Offset: 0x000F3098
		public void TrackBankEvent(string action, int bankedDiamonds, int maxDiamonds, int totalPigyBanks, string trigger, WeeklyEventData eventData)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["pb_action"] = action;
			dictionary["pb_dia"] = bankedDiamonds;
			dictionary["pb_max_dia"] = maxDiamonds;
			dictionary["pb_trigger"] = trigger;
			dictionary["pb_nb"] = totalPigyBanks;
			this.AddEventData(dictionary, eventData);
			Tracking.Track("piggy", dictionary);
			WoogaDebug.Log(new object[]
			{
				"Track: piggy",
				dictionary
			});
		}

		// Token: 0x0600338C RID: 13196 RVA: 0x000F4D24 File Offset: 0x000F3124
		private void AddEventData(Dictionary<string, object> dict, WeeklyEventData eventData)
		{
			if (eventData != null)
			{
				dict["ev_id"] = eventData.id;
				dict["ev_start"] = eventData.StartDateLocal.ToString("yyyy-MM-dd");
			}
		}

		// Token: 0x0600338D RID: 13197 RVA: 0x000F4D68 File Offset: 0x000F3168
		public void TrackChallengeV2Unlocked(int index)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["c_evt"] = "challenge_unlocked";
			dictionary["c_tp"] = this.gameState.Resources.GetAmount("paws");
			dictionary["cs_start"] = DateTime.Now.ToUnixTimeStamp();
			ChallengeGoal challengeGoal = this.gameState.Challenges.CurrentChallenges[index];
			dictionary["c1_id"] = challengeGoal.id;
			dictionary["c1_amt_req"] = challengeGoal.goal;
			dictionary["c1_amt_done"] = this.gameState.Resources.GetCollectedTotal(challengeGoal.type) - challengeGoal.start;
			Tracking.Track("chal", dictionary);
			WoogaDebug.Log(new object[]
			{
				"Track: chal",
				dictionary
			});
		}

		// Token: 0x0600338E RID: 13198 RVA: 0x000F4E58 File Offset: 0x000F3258
		public PendingRewards.TrackingCall TrackChallengeEvent(string eventType, bool trackImmediately = true, ChallengeGoal completedChallenge = null, int pawReward = 0, int pawsSpent = 0)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["c_evt"] = eventType;
			dictionary["c_tp"] = this.gameState.Resources.GetAmount("paws");
			dictionary["cs_start"] = this.gameState.Challenges.ChallengeStartTime.ToUnixTimeStamp();
			dictionary["cs_time_left"] = (int)(this.gameState.Challenges.ChallengeExpireTime - DateTime.Now).TotalSeconds;
			dictionary["cs_te"] = this.gameState.Challenges.NumberOfBoughtExtentions;
			dictionary["c_pb"] = this.gameState.Challenges.CurrentAdBonus * 10;
			dictionary["c_ps"] = pawsSpent;
			for (int i = 0; i < this.gameState.Challenges.CurrentChallenges.Count; i++)
			{
				ChallengeGoal challengeGoal = this.gameState.Challenges.CurrentChallenges[i];
				dictionary[string.Format("c{0}_id", i + 1)] = challengeGoal.id;
				dictionary[string.Format("c{0}_amt_req", i + 1)] = challengeGoal.goal;
				dictionary[string.Format("c{0}_amt_done", i + 1)] = this.gameState.Resources.GetCollectedTotal(challengeGoal.type) - challengeGoal.start;
			}
			if (eventType == "collect_paws" && completedChallenge != null)
			{
				dictionary["c_id"] = completedChallenge.id;
				dictionary["c_diff"] = completedChallenge.difficulty.ToString();
				dictionary["c_paws"] = pawReward;
			}
			if (trackImmediately)
			{
				Tracking.Track("chal", dictionary);
				WoogaDebug.Log(new object[]
				{
					"Track: chal",
					dictionary
				});
			}
			return new PendingRewards.TrackingCall
			{
				evtName = "chal",
				parametersJson = JSON.Serialize(dictionary, false, 1, ' ')
			};
		}

		// Token: 0x0600338F RID: 13199 RVA: 0x000F50A4 File Offset: 0x000F34A4
		public void TrackChallengeResources(string eventType, string detail, Materials rewards)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["action"] = eventType;
			dictionary["det1"] = "challenges";
			dictionary["det2"] = detail;
			dictionary["dia_amt"] = rewards["diamonds"];
			dictionary["coins_amt"] = rewards["coins"];
			dictionary["pre_bomb"] = rewards["boost_pre_bomb_linegem"];
			dictionary["pre_fish"] = rewards["boost_pre_double_fish"];
			dictionary["pre_rainbow"] = rewards["boost_pre_rainbow"];
			dictionary["in_hammer"] = rewards["boost_hammer"];
			dictionary["in_rainbow"] = rewards["boost_rainbow"];
			dictionary["in_star"] = rewards["boost_star"];
			if (detail == "claim_box")
			{
				dictionary["blueprints"] = 3;
			}
			Tracking.Track("res", dictionary);
			WoogaDebug.Log(new object[]
			{
				"Track: res",
				dictionary
			});
		}

		// Token: 0x06003390 RID: 13200 RVA: 0x000F51FC File Offset: 0x000F35FC
		public void TrackAdWheel(string action, Dictionary<string, object> newEvent = null)
		{
			if (newEvent == null)
			{
				newEvent = new Dictionary<string, object>();
			}
			newEvent["action"] = action;
			newEvent["conc_spins"] = this.gameState.SpinJackpotProgress;
			newEvent["spin_rdy"] = ((!this.videoAdService.FreeSpinAvailable) ? "0" : "1");
			TimeSpan timeSpan = Scripts1.DateTimeExtensions.FromUnixTimeStamp(this.gameState.NextSpinAvailable, DateTimeKind.Utc) - DateTime.Now;
			newEvent["spin_cdown"] = ((timeSpan.TotalSeconds <= 0.0) ? 0 : ((int)timeSpan.TotalSeconds));
			newEvent["ad_rdy"] = ((!this.videoAdService.IsVideoAvailable(false) || timeSpan.TotalSeconds > 0.0) ? "0" : "1");
			if (this.videoAdService.FreeSpinAvailable)
			{
				newEvent["video_adid"] = this.videoAdService.FreeSpinVideoAdId;
			}
			Tracking.Track("adw", newEvent);
			WoogaDebug.Log(new object[]
			{
				"Track: event",
				newEvent
			});
		}

		// Token: 0x06003391 RID: 13201 RVA: 0x000F5340 File Offset: 0x000F3740
		public void TrackWoogaVideoAds(int adtime, string orgn, string user_action, string provider = "fairbid", VideoAdMetaData metaData = null)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			WheelSettings wheel_settings = this.configService.general.wheel_settings;
			int dailyLimitFor = this.sbsService.SbsConfig.ad_limits.GetDailyLimitFor(AdPlacement.Unspecified);
			int totalAdsWatchedToday = this.gameState.AdViews.TotalAdsWatchedToday;
			dictionary["adtime"] = adtime;
			dictionary["dailyviewcap"] = dailyLimitFor;
			dictionary["currentviewcap"] = dailyLimitFor - totalAdsWatchedToday;
			dictionary["deltatime"] = wheel_settings.spin_cooldown;
			dictionary["orgn"] = orgn;
			dictionary["provider"] = provider;
			dictionary["user_action"] = user_action;
			dictionary["safe_dk_id"] = this.safeDkService.GetUserId();
			if (metaData != null)
			{
				if (!string.IsNullOrEmpty(metaData.id))
				{
					dictionary["video_adid"] = metaData.id;
				}
				if (!string.IsNullOrEmpty(metaData.network))
				{
					dictionary["network"] = metaData.network;
				}
				if (!string.IsNullOrEmpty(metaData.ecpmPricing))
				{
					dictionary["pricing"] = metaData.ecpmPricing;
				}
				if (!string.IsNullOrEmpty(metaData.ecpmPricingType))
				{
					dictionary["pricing_type"] = metaData.ecpmPricingType;
				}
			}
			Tracking.Track("video_ads", dictionary);
			WoogaDebug.Log(new object[]
			{
				"Track: event",
				dictionary
			});
		}

		// Token: 0x06003392 RID: 13202 RVA: 0x000F54C8 File Offset: 0x000F38C8
		public void TrackAdWheelReward(List<WheelPrize> prizes, bool isJackpot)
		{
			if (prizes == null || prizes.Count < 1)
			{
				return;
			}
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["jackpot"] = ((!isJackpot) ? 0 : 1);
			dictionary["rew1"] = prizes[0].prize;
			dictionary["amount1"] = ((prizes[0].prizeType != AdSpinPrize.UnlimitedLives) ? prizes[0].amount : (prizes[0].amount / 60));
			this.TrackAdWheelResources(prizes[0], isJackpot);
			if (prizes.Count > 1)
			{
				dictionary["rew2"] = prizes[1].prize;
				dictionary["amount2"] = ((prizes[1].prizeType != AdSpinPrize.UnlimitedLives) ? prizes[1].amount : (prizes[1].amount / 60));
				this.TrackAdWheelResources(prizes[1], isJackpot);
			}
			this.TrackAdWheel("claim", dictionary);
		}

		// Token: 0x06003393 RID: 13203 RVA: 0x000F55F0 File Offset: 0x000F39F0
		private void TrackAdWheelResources(WheelPrize prize, bool isJackpot)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["action"] = "gained";
			string resourceKeyFromName = this.GetResourceKeyFromName(prize.prizeType.ToString());
			if (!string.IsNullOrEmpty(resourceKeyFromName))
			{
				dictionary["det1"] = "ad_wheel";
				dictionary["det2"] = ((!isJackpot) ? 0 : 1);
				dictionary[resourceKeyFromName] = ((prize.prizeType != AdSpinPrize.UnlimitedLives) ? prize.amount : (prize.amount / 60));
			}
			Tracking.Track("res", dictionary);
			WoogaDebug.Log(new object[]
			{
				"Track: res",
				dictionary
			});
		}

		// Token: 0x06003394 RID: 13204 RVA: 0x000F56B4 File Offset: 0x000F3AB4
		private string GetResourceKeyFromName(string resourceName)
		{
			if (resourceName != null)
			{
				if (resourceName == "UnlimitedLives")
				{
					return "u_lives";
				}
				if (resourceName == "Diamonds")
				{
					return "dia_amt";
				}
				if (resourceName == "Coins")
				{
					return "coins_amt";
				}
				if (resourceName == "Starboost")
				{
					return "in_star";
				}
				if (resourceName == "HammerBoost")
				{
					return "in_hammer";
				}
			}
			return string.Empty;
		}

		// Token: 0x06003395 RID: 13205 RVA: 0x000F5740 File Offset: 0x000F3B40
		public static string GetPurchaseResourceKey(string resourceName)
		{
			switch (resourceName)
			{
			case "diamonds":
				return "dia_amt";
			case "coins":
				return "coins_amt";
			case "lives_unlimited":
				return "u_lives";
			case "boost_pre_rainbow":
				return "pre_rainbow";
			case "boost_pre_bomb_linegem":
				return "pre_bomb";
			case "boost_pre_double_fish":
				return "pre_fish";
			case "boost_rainbow":
				return "in_rainbow";
			case "boost_star":
				return "in_star";
			case "boost_hammer":
				return "in_hammer";
			}
			return string.Empty;
		}

		// Token: 0x06003396 RID: 13206 RVA: 0x000F5858 File Offset: 0x000F3C58
		public void TrackEvent(params object[] parameters)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			for (int i = 0; i < parameters.Length; i++)
			{
				dictionary["st" + (i + 1)] = parameters[i];
			}
			Tracking.Track("event", dictionary);
			WoogaDebug.Log(new object[]
			{
				"Track: event",
				dictionary
			});
		}

		// Token: 0x06003397 RID: 13207 RVA: 0x000F58BC File Offset: 0x000F3CBC
		public void TrackPurchase(TrackingService.PurchaseFlowContext context, int coins, int diamonds)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["action"] = context.action;
			dictionary["round_id"] = this.currentOrLastRoundID;
			dictionary["coins_amt"] = coins;
			dictionary["dia_amt"] = diamonds;
			dictionary["det1"] = context.det1;
			dictionary["det2"] = context.det2;
			dictionary["det3"] = context.det3;
			dictionary["det4"] = context.det4;
			Tracking.Track("res", dictionary);
			WoogaDebug.Log(new object[]
			{
				"Track: res",
				dictionary
			});
		}

		// Token: 0x06003398 RID: 13208 RVA: 0x000F597C File Offset: 0x000F3D7C
		public void TrackUi(string ui, string ui_det, string action, string action_det, params object[] subtypes)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["ui"] = ui;
			dictionary["ui_det"] = ui_det;
			dictionary["action"] = action;
			if (action_det != null)
			{
				dictionary["action_det"] = action_det;
			}
			this.AddSubtypes(dictionary, subtypes);
			if (Tracking.IsInitialized)
			{
				Tracking.Track("ui", dictionary);
			}
			WoogaDebug.Log(new object[]
			{
				"Track: ui",
				dictionary
			});
		}

		// Token: 0x06003399 RID: 13209 RVA: 0x000F59FC File Offset: 0x000F3DFC
		public void TrackGdprPopup(string action)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["action"] = action;
			dictionary["ltp_version"] = TermsLocalisation.CurrentVersion.ToString();
			if (Tracking.IsInitialized)
			{
				Tracking.Track("ltp", dictionary);
			}
			WoogaDebug.Log(new object[]
			{
				"Track: gdpr",
				dictionary
			});
		}

		// Token: 0x0600339A RID: 13210 RVA: 0x000F5A68 File Offset: 0x000F3E68
		public void TrackWeeklyEventBadgeOpen(LevelPlayMode playMode)
		{
			AWeeklyEventDataService dataServiceForPlayMode = this.gameState.GetDataServiceForPlayMode(playMode);
			if (dataServiceForPlayMode != null)
			{
				this.TrackUi("event_map", playMode.ToString(), "open", string.Empty, new object[]
				{
					dataServiceForPlayMode.StartTime.ToString("yyyy-MM-dd")
				});
			}
		}

		// Token: 0x0600339B RID: 13211 RVA: 0x000F5AC8 File Offset: 0x000F3EC8
		public void TrackTournament(string id, string tier, string action, string qualifyingObject, int qualifyingAmount, int start, int end, int score, string playerName, int rank = 0, int players = 0)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["t_id"] = id;
			dictionary["t_action"] = action;
			dictionary["t_q_object"] = qualifyingObject;
			dictionary["t_q_amount"] = qualifyingAmount;
			dictionary["t_start"] = start;
			dictionary["t_end"] = end;
			dictionary["t_score"] = score.ToString();
			dictionary["t_player_name"] = playerName;
			dictionary["t_rank"] = rank;
			dictionary["t_players"] = players;
			dictionary["t_tier"] = ((!string.IsNullOrEmpty(tier)) ? tier : string.Empty);
			Tracking.Track("tour", dictionary);
			WoogaDebug.Log(new object[]
			{
				"Track: tour",
				dictionary
			});
		}

		// Token: 0x0600339C RID: 13212 RVA: 0x000F5BC4 File Offset: 0x000F3FC4
		private void AddSubtypes(Dictionary<string, object> newEvent, object[] subtypes)
		{
			if (subtypes == null)
			{
				return;
			}
			for (int i = 0; i < subtypes.Length; i++)
			{
				newEvent["st" + (i + 1)] = subtypes[i];
			}
		}

		// Token: 0x0600339D RID: 13213 RVA: 0x000F5C07 File Offset: 0x000F4007
		public void AddPurchaseTracking(Dictionary<string, object> dict)
		{
			this.gameParams.AddParametersTo(dict);
			this.woogaParams.AddParametersTo(dict);
			Tracking.baseParameters.AddParametersTo(dict);
			Tracking.sessionTracker.AddParametersTo(dict);
		}

		// Token: 0x0600339E RID: 13214 RVA: 0x000F5C38 File Offset: 0x000F4038
		public void TrackDecoItem(BuildingConfig building, string action, int upgradeLevel, string trackingDetail = "")
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["obj_id"] = building.name;
			dictionary["action"] = action;
			dictionary["cat"] = building.category;
			dictionary["up_lvl"] = upgradeLevel;
			if (!string.IsNullOrEmpty(trackingDetail))
			{
				dictionary["st1"] = trackingDetail;
			}
			dictionary["harm_amt"] = building.harmony;
			SeasonConfig activeSeason = this.seasonService.GetActiveSeason();
			if (building.season_currency > 0 && activeSeason != null)
			{
				dictionary["sea_cur"] = activeSeason.Primary;
				dictionary["sea_ev_id"] = activeSeason.id;
				dictionary["sea_cur_amt"] = building.season_currency;
			}
			Tracking.Track("deco", dictionary);
			WoogaDebug.Log(new object[]
			{
				"Track: deco",
				dictionary
			});
		}

		// Token: 0x0600339F RID: 13215 RVA: 0x000F5D34 File Offset: 0x000F4134
		public void TrackAreaUnlock(int area)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["area"] = area;
			Tracking.Track("ru", dictionary);
			WoogaDebug.Log(new object[]
			{
				"Track: ru",
				dictionary
			});
		}

		// Token: 0x060033A0 RID: 13216 RVA: 0x000F5D7C File Offset: 0x000F417C
		public void TrackBuildingHarvest(BuildingInstance instance, MaterialAmount amount)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["action"] = "gained";
			dictionary["det1"] = "harvest_building";
			dictionary["det2"] = instance.blueprint.name;
			dictionary["coins_amt"] = ((!(amount.type == "coins")) ? 0 : amount.amount);
			dictionary["dia_amt"] = ((!(amount.type == "diamonds")) ? 0 : amount.amount);
			Tracking.Track("res", dictionary);
			WoogaDebug.Log(new object[]
			{
				"Track: res",
				dictionary
			});
		}

		// Token: 0x060033A1 RID: 13217 RVA: 0x000F5E50 File Offset: 0x000F4250
		public void TrackQuestCollected(QuestData data, QuestProgress progress)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["action"] = "gained";
			dictionary["det1"] = "quest_rewards";
			dictionary["det2"] = data.id;
			dictionary["coins_amt"] = ((!(data.rewardItem == "coins")) ? 0 : data.rewardCount);
			dictionary["dia_amt"] = ((!(data.rewardItem == "diamonds")) ? 0 : data.rewardCount);
			Tracking.Track("res", dictionary);
			WoogaDebug.Log(new object[]
			{
				"Track: res",
				dictionary
			});
		}

		// Token: 0x060033A2 RID: 13218 RVA: 0x000F5F18 File Offset: 0x000F4318
		public void TrackVillageRank(VillageRank rank)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["action"] = "gained";
			dictionary["det1"] = "village_rank_up";
			dictionary["det2"] = rank.village_rank;
			dictionary["coins_amt"] = rank.reward_coins;
			dictionary["dia_amt"] = rank.reward_diamonds;
			dictionary["pre_bomb"] = ((!(rank.reward_booster_type == "boost_pre_bomb_linegem")) ? 0 : rank.reward_booster_amount);
			dictionary["pre_fish"] = ((!(rank.reward_booster_type == "boost_pre_double_fish")) ? 0 : rank.reward_booster_amount);
			dictionary["pre_rainbow"] = ((!(rank.reward_booster_type == "boost_pre_rainbow")) ? 0 : rank.reward_booster_amount);
			dictionary["in_hammer"] = ((!(rank.reward_booster_type == "boost_hammer")) ? 0 : rank.reward_booster_amount);
			dictionary["in_rainbow"] = ((!(rank.reward_booster_type == "boost_rainbow")) ? 0 : rank.reward_booster_amount);
			dictionary["in_star"] = ((!(rank.reward_booster_type == "boost_star")) ? 0 : rank.reward_booster_amount);
			Tracking.Track("res", dictionary);
			WoogaDebug.Log(new object[]
			{
				"Track: res",
				dictionary
			});
		}

		// Token: 0x060033A3 RID: 13219 RVA: 0x000F60D8 File Offset: 0x000F44D8
		public void TrackBuyFromShop(string packageName, int coins, int diamonds, string context, IAPContent[] contents)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["action"] = "gained";
			dictionary["det1"] = context;
			dictionary["det2"] = packageName;
			if (IAPData.IsDailyDealName(packageName))
			{
				dictionary["det3"] = "daily_deal";
				dictionary["det4"] = this.gameState.DailyDeals.CurrentDeal.offer_id;
			}
			else if (IAPData.IsMoreMovesPackName(packageName))
			{
				dictionary["det3"] = this.currentOrLastLevelName;
				dictionary["round_id"] = this.currentOrLastRoundID;
			}
			dictionary["coins_amt"] = coins;
			dictionary["dia_amt"] = diamonds;
			if (this.configService.FeatureSwitchesConfig.new_shop_layout)
			{
				int i = 0;
				while (i < contents.Length)
				{
					IAPContent iapcontent = contents[i];
					string item_resource = iapcontent.item_resource;
					if (item_resource == null)
					{
						goto IL_11C;
					}
					if (!(item_resource == "diamonds") && !(item_resource == "coins"))
					{
						goto IL_11C;
					}
					IL_13D:
					i++;
					continue;
					IL_11C:
					dictionary.Add(TrackingService.GetPurchaseResourceKey(iapcontent.item_resource), iapcontent.item_amount);
					goto IL_13D;
				}
			}
			Tracking.Track("res", dictionary);
			WoogaDebug.Log(new object[]
			{
				"Track: res",
				dictionary
			});
		}

		// Token: 0x060033A4 RID: 13220 RVA: 0x000F6254 File Offset: 0x000F4654
		public void TrackDiamondsPurchaseAction(string action, IAPData iapData, IAPContent[] contents, TownDiamondsPanelRoot.TownDiamondsPanelRootParameters context, string failureReason = "")
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["action"] = action;
			if (action == "purchase_failed")
			{
				dictionary["st1"] = failureReason;
			}
			this.AddEventData(dictionary, context.eventData);
			if (context.eventType != WeeklyEventType.Undefined)
			{
				dictionary["ev_type"] = context.eventType.ToString();
			}
			dictionary["source1"] = context.source1;
			dictionary["source2"] = context.source2;
			if (iapData != null)
			{
				dictionary["package_id"] = iapData.iap_name;
				if (iapData.IsDailyDeal)
				{
					for (int i = 0; i < contents.Length; i++)
					{
						this.AddIAPResourceToEvent(dictionary, contents[i], i + 1);
					}
					dictionary["st2"] = this.gameState.DailyDeals.CurrentDeal.offer_id;
				}
				else
				{
					int num = (!this.configService.FeatureSwitchesConfig.new_shop_layout) ? Mathf.Min(2, contents.Length) : contents.Length;
					for (int j = 0; j < num; j++)
					{
						IAPContent iapcontent = contents[j];
						if (this.configService.FeatureSwitchesConfig.new_shop_layout)
						{
							string item_resource = iapcontent.item_resource;
							if (item_resource == null)
							{
								goto IL_182;
							}
							if (!(item_resource == "diamonds") && !(item_resource == "coins"))
							{
								goto IL_182;
							}
							this.AddIAPResourceToEvent(dictionary, iapcontent, j + 1);
							goto IL_1F2;
							IL_182:
							dictionary[TrackingService.GetPurchaseResourceKey(iapcontent.item_resource)] = iapcontent.item_amount;
						}
						else
						{
							string item_resource2 = iapcontent.item_resource;
							if (item_resource2 != null)
							{
								if (item_resource2 == "diamonds" || item_resource2 == "coins")
								{
									this.AddIAPResourceToEvent(dictionary, iapcontent, j + 1);
								}
							}
						}
						IL_1F2:;
					}
				}
			}
			dictionary["price"] = ((iapData == null) ? 0 : iapData.price);
			// dictionary["cur"] = ((iapData == null || iapData.storeProduct == null) ? "EUR" : iapData.storeProduct.metadata.isoCurrencyCode);
			dictionary["cur"] = "@@@@@@@@@@@@@";
			Tracking.Track("pur", dictionary);
			WoogaDebug.Log(new object[]
			{
				"Track: pur",
				dictionary
			});
		}

		// Token: 0x060033A5 RID: 13221 RVA: 0x000F64D8 File Offset: 0x000F48D8
		private void AddIAPResourceToEvent(Dictionary<string, object> targetEvent, IAPContent content, int index)
		{
			targetEvent[string.Format("item{0}", index)] = content.item_resource;
			targetEvent[string.Format("amount{0}", index)] = content.item_amount;
		}

		// Token: 0x060033A6 RID: 13222 RVA: 0x000F6518 File Offset: 0x000F4918
		public void TrackMoreMovesPurchaseAction(string action, IAPData iapData, IAPContent[] contents, TrackingService.MoreMovesPurchaseTrackingContext context, string failureReason = "")
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["action"] = action;
			if (action == "purchase_failed")
			{
				dictionary["st1"] = failureReason;
			}
			dictionary["source1"] = context.source1;
			dictionary["source2"] = context.source2;
			if (iapData != null)
			{
				dictionary["package_id"] = iapData.iap_name;
				dictionary["item1"] = "post_moves";
				dictionary["amount1"] = context.moveCount;
				if (contents != null)
				{
					for (int i = 0; i < contents.Length; i++)
					{
						this.AddIAPResourceToEvent(dictionary, contents[i], i + 2);
					}
				}
				dictionary["price"] = iapData.price;
				// if (iapData.storeProduct != null && iapData.storeProduct.metadata != null && iapData.storeProduct.metadata.isoCurrencyCode != null)
				// {
				dictionary["cur"] = "@@@@@@@@@@";
				// }
			}
			Tracking.Track("pur", dictionary);
			WoogaDebug.Log(new object[]
			{
				"Track: pur",
				dictionary
			});
		}

		// Token: 0x060033A7 RID: 13223 RVA: 0x000F6660 File Offset: 0x000F4A60
		//public void TrackRequest(FacebookRequest request, IAppRequestResult result)
		//{
		//	Dictionary<string, object> dictionary = new Dictionary<string, object>();
		//	dictionary["request_id"] = result.RequestID;
		//	if (!request.recipients.Any((string r) => r.StartsWith("AV")))
		//	{
		//		dictionary["receivers"] = string.Join(",", request.recipients.ToArray());
		//	}
		//	dictionary["num_sent"] = request.recipients.Count<string>();
		//	this.AddFacebookFriends(dictionary);
		//	dictionary["type"] = request.trackingType;
		//	dictionary["st1"] = request.context1;
		//	dictionary["st2"] = request.context2;
		//	Tracking.Track("rqs", dictionary);
		//	WoogaDebug.Log(new object[]
		//	{
		//		"Track: rqs",
		//		dictionary
		//	});
		//}

		// Token: 0x060033A8 RID: 13224 RVA: 0x000F6748 File Offset: 0x000F4B48
		public void TrackRequestResponse(FacebookData.Request request, string context, string context2)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["request_id"] = request.ID;
			dictionary["sender"] = request.fromID;
			this.AddFacebookFriends(dictionary);
			dictionary["type"] = request.type;
			dictionary["st1"] = context;
			dictionary["st2"] = context2;
			Tracking.Track("rqr", dictionary);
			WoogaDebug.Log(new object[]
			{
				"Track: rqr",
				dictionary
			});
		}

		// Token: 0x060033A9 RID: 13225 RVA: 0x000F67D0 File Offset: 0x000F4BD0
		private void AddFacebookFriends(Dictionary<string, object> newEvent)
		{
			if (this.facebookService.LoggedIn())
			{
				newEvent["pl_friends"] = this.facebookService.FriendsPlayingCount;
				newEvent["t_friends"] = this.facebookService.FriendsTotalCount;
			}
		}

		// Token: 0x060033AA RID: 13226 RVA: 0x000F6824 File Offset: 0x000F4C24
		private void OnSessionEnd(Dictionary<string, object> data)
		{
			data["startlvl"] = this.sessionStartLevel;
			data["rounds"] = this.sessionRoundCounter;
			data["sd"] = (int)(DateTime.UtcNow - this.sessionStartTime).TotalSeconds;
		}

		// Token: 0x060033AB RID: 13227 RVA: 0x000F6886 File Offset: 0x000F4C86
		private bool OnSessionStart(Dictionary<string, object> data)
		{
			this.sessionStartLevel = this.progression.CurrentLevel;
			this.sessionRoundCounter = 0;
			this.sessionStartTime = DateTime.UtcNow;
			data["safe_dk_id"] = this.safeDkService.GetUserId();
			return true;
		}

		// Token: 0x060033AC RID: 13228 RVA: 0x000F68C4 File Offset: 0x000F4CC4
		private void OnLowMemory()
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["st1"] = "lowMemory";
			dictionary["st2"] = string.Format(CultureInfo.InvariantCulture, "{0:0.0}", new object[]
			{
				(float)GC.GetTotalMemory(false) / 1048576f
			});
			Tracking.Track("event", dictionary);
			WoogaDebug.Log(new object[]
			{
				"Track: event",
				dictionary
			});
		}

		// Token: 0x060033AD RID: 13229 RVA: 0x000F6940 File Offset: 0x000F4D40
		private static void HandleQuestTaskComplete(QuestProgress progress, int idx)
		{
			QuestData configData = progress.configData;
			if (progress.status == QuestProgress.Status.started)
			{
				int taskId = idx + 1;
				TrackingService.SendQuestEvent("task_complete", configData, progress, taskId);
			}
		}

		// Token: 0x060033AE RID: 13230 RVA: 0x000F6974 File Offset: 0x000F4D74
		private static void HandleQuestChanged(QuestProgress progress)
		{
			QuestData configData = progress.configData;
			if (progress.status == QuestProgress.Status.started)
			{
				TrackingService.SendQuestEvent("unlock", configData, progress, 0);
			}
		}

		// Token: 0x060033AF RID: 13231 RVA: 0x000F69A4 File Offset: 0x000F4DA4
		private static void HandleQuestComplete(QuestProgress progress)
		{
			QuestData configData = progress.configData;
			TrackingService.SendQuestEvent("complete", configData, progress, 0);
		}

		// Token: 0x060033B0 RID: 13232 RVA: 0x000F69C8 File Offset: 0x000F4DC8
		private static void SendQuestEvent(string eventType, QuestData data, QuestProgress progress, int taskId = 0)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["quest_action"] = eventType;
			dictionary["quest_id"] = data.id;
			if (taskId > 0)
			{
				dictionary["task_complete"] = taskId;
			}
			int num = 0;
			int count = data.Tasks.Count;
			while (num < 5 && num < count)
			{
				string str = (num + 1).ToString();
				dictionary["o" + str + "_name"] = data.task_type[num] + "_" + data.task_item[num];
				dictionary["o" + str + "_amount"] = data.task_count[num];
				num++;
			}
			dictionary["r1_name"] = data.rewardItem;
			dictionary["r1_amount"] = data.rewardCount;
			dictionary["tasks"] = data.Tasks.Count;
			Tracking.Track("quests", dictionary);
			WoogaDebug.Log(new object[]
			{
				"Track: quests",
				dictionary
			});
		}

		// Token: 0x060033B1 RID: 13233 RVA: 0x000F6B00 File Offset: 0x000F4F00
		public void StartOfRound(LevelConfig levelConfig)
		{
			if (this.currentRound != null)
			{
				WoogaDebug.LogError(new object[]
				{
					"Somehow called StartOfRound before calling EndOfRound"
				});
			}
			this.currentRound = new PTSingleRoundTracker(DateTime.UtcNow);
			this.sessionRoundCounter++;
			this.currentOrLastRoundID = this.currentRound.RoundId;
			this.currentOrLastLevelName = levelConfig.Name;
		}

		// Token: 0x060033B2 RID: 13234 RVA: 0x000F6B66 File Offset: 0x000F4F66
		public void CheatEndOfRound()
		{
			this.currentRound = null;
		}

		// Token: 0x060033B3 RID: 13235 RVA: 0x000F6B70 File Offset: 0x000F4F70
		public void EndOfRound(TrackingService.RoundOutcome outcome, Match3Score levelScore, LevelConfig levelConfig, LeagueModel leagueModel, int tryCountForLevelOfTheDay, int levelOfDayStreak)
		{
			DateTime utcNow = DateTime.UtcNow;
			if (this.currentRound != null)
			{
				this.currentRound.SetCompleted(utcNow, outcome);
				this.roundTrack.RegisterRound(outcome);
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				this.roundTrack.PopulateTracking(utcNow, dictionary);
				this.currentRound.PopulateTracking(dictionary);
				levelScore.PopulateTracking(dictionary);
				levelConfig.PopulateTracking(levelScore, this.seasonService, dictionary);
				this.PopulateLeagueTracking(levelScore, levelConfig, leagueModel, dictionary);
				if (this.bankService.BankFeatureEnabled())
				{
					dictionary["pb_nb"] = this.gameState.Bank.TotalNumberOfPigyBanksLifeTime;
					dictionary["n_pb_dia"] = this.bankService.GetBankedDiamondRewardForLevel(levelConfig);
				}
				dictionary["lod"] = tryCountForLevelOfTheDay;
				dictionary["lod_st"] = levelOfDayStreak;
				this.AddFacebookFriends(dictionary);
				AWeeklyEventDataService dataServiceForPlayMode = this.gameState.GetDataServiceForPlayMode(levelScore.levelPlayMode);
				if (dataServiceForPlayMode != null)
				{
					dictionary["ev_type"] = levelScore.levelPlayMode.ToString();
					dictionary["ev_id"] = dataServiceForPlayMode.EventId;
					dictionary["ev_start"] = dataServiceForPlayMode.StartTime.ToString("yyyy-MM-dd");
				}
				Tracking.Track("eor", dictionary);
				WoogaDebug.Log(new object[]
				{
					"Track: eor",
					dictionary
				});
			}
			this.currentRound = null;
			this.roundTrack.CompactLog(utcNow, TimeSpan.FromDays(7.0));
			this.SaveRoundTracking();
		}

		// Token: 0x060033B4 RID: 13236 RVA: 0x000F6D0C File Offset: 0x000F510C
		private void PopulateLeagueTracking(Match3Score score, LevelConfig level, LeagueModel league, Dictionary<string, object> newEvent)
		{
			if (!league.IsValid())
			{
				return;
			}
			newEvent["t_id"] = league.config.id;
			newEvent["t_tier"] = ((!string.IsNullOrEmpty(league.tier)) ? league.tier : string.Empty);
			newEvent["t_status"] = league.playerStatus.ToString().ToLower();
			newEvent["t_sc_name"] = league.config.config.tournamentType.ToString();
			newEvent["t_sc_amt"] = score.tournamentScore.CollectedPoints * league.config.config.GetScoreMultiplierForZeroBasedTier((int)level.SelectedTier, score.Config.IsCompleted);
		}

		// Token: 0x060033B5 RID: 13237 RVA: 0x000F6DF0 File Offset: 0x000F51F0
		private void SaveRoundTracking()
		{
			string json = this.roundTrack.GetJson();
			File.WriteAllText(this.trackerPath, json);
		}

		// Token: 0x060033B6 RID: 13238 RVA: 0x000F6E18 File Offset: 0x000F5218
		private void LoadRoundTracking()
		{
			if (File.Exists(this.trackerPath))
			{
				string json = File.ReadAllText(this.trackerPath);
				this.roundTrack.FromJson(json);
			}
		}

		// Token: 0x060033B7 RID: 13239 RVA: 0x000F6E50 File Offset: 0x000F5250
		private void AddMaterials(Dictionary<string, object> newEvent, Materials rewards)
		{
			int num = rewards["coins"];
			int num2 = rewards["diamonds"];
			int num3 = rewards["boost_pre_bomb_linegem"];
			int num4 = rewards["boost_pre_double_fish"];
			int num5 = rewards["boost_pre_rainbow"];
			int num6 = rewards["boost_hammer"];
			int num7 = rewards["boost_rainbow"];
			int num8 = rewards["boost_star"];
			int num9 = rewards["lives"];
			int num10 = rewards["UnlimitedLives"];
			if (num > 0)
			{
				newEvent["coins_amt"] = num;
			}
			if (num2 > 0)
			{
				newEvent["dia_amt"] = num2;
			}
			if (num3 > 0)
			{
				newEvent["pre_bomb"] = num3;
			}
			if (num4 > 0)
			{
				newEvent["pre_fish"] = num4;
			}
			if (num5 > 0)
			{
				newEvent["pre_rainbow"] = num5;
			}
			if (num6 > 0)
			{
				newEvent["in_hammer"] = num6;
			}
			if (num7 > 0)
			{
				newEvent["in_rainbow"] = num7;
			}
			if (num8 > 0)
			{
				newEvent["in_star"] = num8;
			}
			if (num9 > 0)
			{
				newEvent["lives"] = num9;
			}
			if (num10 > 0)
			{
				newEvent["u_lives"] = num10;
			}
		}

		// Token: 0x04005B94 RID: 23444
		public const string VIDEO_ADID = "video_adid";

		// Token: 0x04005B95 RID: 23445
		[WaitForService(true, true)]
		private GameStateService gameState;

		// Token: 0x04005B96 RID: 23446
		[WaitForService(true, true)]
		private GameSettingsService settings;

		// Token: 0x04005B97 RID: 23447
		[WaitForService(true, true)]
		private ProgressionDataService.Service progression;

		// Token: 0x04005B98 RID: 23448
		[WaitForService(true, true)]
		private QuestService questService;

		// Token: 0x04005B99 RID: 23449
		[WaitForService(true, true)]
		private FacebookService facebookService;

		// Token: 0x04005B9A RID: 23450
		[WaitForService(true, true)]
		private LocalNotificationService localNotificationService;

		// Token: 0x04005B9B RID: 23451
		[WaitForService(true, true)]
		private PushNotificationService pushNotificationService;

		// Token: 0x04005B9C RID: 23452
		[WaitForService(false, true)]
		private IVideoAdService videoAdService;

		// Token: 0x04005B9D RID: 23453
		[WaitForService(true, true)]
		private ConfigService configService;

		// Token: 0x04005B9E RID: 23454
		[WaitForService(true, true)]
		private SBSService sbsService;

		// Token: 0x04005B9F RID: 23455
		[WaitForService(false, true)]
		private BankService bankService;

		// Token: 0x04005BA0 RID: 23456
		[WaitForService(true, true)]
		private SafeDkService safeDkService;

		// Token: 0x04005BA1 RID: 23457
		[WaitForService(true, true)]
		private SeasonService seasonService;

		// Token: 0x04005BA2 RID: 23458
		private PTRoundTracker roundTrack;

		// Token: 0x04005BA3 RID: 23459
		private PTSingleRoundTracker currentRound;

		// Token: 0x04005BA4 RID: 23460
		private TrackingService.WoogaParameters woogaParams;

		// Token: 0x04005BA5 RID: 23461
		private TrackingService.GameParameters gameParams;

		// Token: 0x04005BA6 RID: 23462
		private string currentOrLastRoundID = string.Empty;

		// Token: 0x04005BA7 RID: 23463
		private string currentOrLastLevelName = string.Empty;

		// Token: 0x04005BA8 RID: 23464
		private TrackingSet funnelTrack;

		// Token: 0x04005BA9 RID: 23465
		private int sessionStartLevel;

		// Token: 0x04005BAA RID: 23466
		private int sessionRoundCounter;

		// Token: 0x04005BAB RID: 23467
		private DateTime sessionStartTime;

		// Token: 0x04005BAC RID: 23468
		[CompilerGenerated]
		private static Action<QuestProgress, int> _003C_003Ef__mg_0024cache0;

		// Token: 0x04005BAD RID: 23469
		[CompilerGenerated]
		private static Action<QuestProgress> _003C_003Ef__mg_0024cache1;

		// Token: 0x04005BAE RID: 23470
		[CompilerGenerated]
		private static Action<QuestProgress> _003C_003Ef__mg_0024cache2;

		// Token: 0x0200081F RID: 2079
		public enum RoundOutcome
		{
			// Token: 0x04005BB2 RID: 23474
			Lost,
			// Token: 0x04005BB3 RID: 23475
			Won,
			// Token: 0x04005BB4 RID: 23476
			Cancelled
		}

		// Token: 0x02000820 RID: 2080
		public class PurchaseFlowContext
		{
			// Token: 0x04005BB5 RID: 23477
			public string round_id = string.Empty;

			// Token: 0x04005BB6 RID: 23478
			public string det1 = string.Empty;

			// Token: 0x04005BB7 RID: 23479
			public string det2 = string.Empty;

			// Token: 0x04005BB8 RID: 23480
			public string det3 = string.Empty;

			// Token: 0x04005BB9 RID: 23481
			public string det4 = string.Empty;

			// Token: 0x04005BBA RID: 23482
			public string action = "spent";
		}

		// Token: 0x02000821 RID: 2081
		public class MoreMovesPurchaseTrackingContext
		{
			// Token: 0x04005BBB RID: 23483
			public string source1;

			// Token: 0x04005BBC RID: 23484
			public string source2;

			// Token: 0x04005BBD RID: 23485
			public int moveCount;
		}

		// Token: 0x02000822 RID: 2082
		private class GameParameters : IParameterProvider
		{
			// Token: 0x060033BB RID: 13243 RVA: 0x000F7041 File Offset: 0x000F5441
			public GameParameters(GameStateService gameState, ProgressionDataService.Service progression, PTRoundTracker roundTrack, FacebookService facebookService)
			{
				this._gameState = gameState;
				this._progression = progression;
				this._roundTrack = roundTrack;
				this._facebookService = facebookService;
			}

			// Token: 0x060033BC RID: 13244 RVA: 0x000F7068 File Offset: 0x000F5468
			public void AddParametersTo(Dictionary<string, object> data)
			{
				data["env"] = (int)GameEnvironment.CurrentTrackingEnvironment;
				data["energy"] = this._gameState.Resources.GetAmount("lives");
				data["harm"] = this._gameState.Resources.GetAmount("harmony");
				data["dia"] = this._gameState.Resources.GetAmount("diamonds");
				data["coins"] = this._gameState.Resources.GetAmount("coins");
				data["in_ha_bal"] = this._gameState.Resources.GetAmount("boost_hammer");
				data["in_ra_bal"] = this._gameState.Resources.GetAmount("boost_rainbow");
				data["in_st_bal"] = this._gameState.Resources.GetAmount("boost_star");
				data["pre_bo_bal"] = this._gameState.Resources.GetAmount("boost_pre_bomb_linegem");
				data["pre_fi_bal"] = this._gameState.Resources.GetAmount("boost_pre_double_fish");
				data["pre_ra_bal"] = this._gameState.Resources.GetAmount("boost_pre_rainbow");
				data["max_lvl"] = this._progression.UnlockedLevel;
				data["max_area"] = this._progression.LastAreaIgnoringQuestAndEocLock;
				data["ls"] = ((AUiAdjuster.SimilarOrientation != ScreenOrientation.LandscapeLeft) ? 0 : 1);
				if (this._facebookService.LoggedIn())
				{
					data["s"] = this._facebookService.FB_MY_ID;
				}
				int tier = this._progression.GetTier(this._progression.UnlockedLevel);
				int num = Math.Min(2, tier + 1);
				AreaConfig.Tier tier2 = (AreaConfig.Tier)num;
				string str = tier2.ToString();
				data["max_lvln"] = this._progression.UnlockedLevel.ToString().PadLeft(4, '0') + str;
				this._roundTrack.PopulateGameTracking(data);
			}

			// Token: 0x04005BBE RID: 23486
			private readonly GameStateService _gameState;

			// Token: 0x04005BBF RID: 23487
			private readonly ProgressionDataService.Service _progression;

			// Token: 0x04005BC0 RID: 23488
			private readonly PTRoundTracker _roundTrack;

			// Token: 0x04005BC1 RID: 23489
			private readonly FacebookService _facebookService;
		}

		// Token: 0x02000823 RID: 2083
		private class WoogaParameters : IParameterProvider
		{
			// Token: 0x060033BD RID: 13245 RVA: 0x000F72EA File Offset: 0x000F56EA
			public WoogaParameters(LocalNotificationService localNotificationService, GameSettingsService settings, PushNotificationService pushNotificationService, string personalizationString)
			{
				this.localNotificationService = localNotificationService;
				this.settings = settings;
				this.pushNotificationService = pushNotificationService;
				this.personalizationString = personalizationString;
			}

			// Token: 0x060033BE RID: 13246 RVA: 0x000F7310 File Offset: 0x000F5710
			public void AddParametersTo(Dictionary<string, object> data)
			{
				data["version"] = BuildVersion.ShortVersion;
				data["game_lng"] = PTLocalizationService.ToCrowdinCode(this.settings.Language);
				string value = this.localNotificationService.startedByLocalNotifcation;
				if (!string.IsNullOrEmpty(value))
				{
					data["source"] = value;
				}
				else
				{
					value = this.pushNotificationService.PushNotificationStartId;
					if (!string.IsNullOrEmpty(value))
					{
						data["source"] = value;
					}
				}
				if (this.settings.HasToggle(ToggleSetting.AdPersonalisation))
				{
					data["gdpr_ad"] = this.settings.GetToggle(ToggleSetting.AdPersonalisation);
				}
				data["gdpr_exp"] = true;
				data["pers_segments"] = this.personalizationString;
			}

			// Token: 0x04005BC2 RID: 23490
			private readonly LocalNotificationService localNotificationService;

			// Token: 0x04005BC3 RID: 23491
			private readonly PushNotificationService pushNotificationService;

			// Token: 0x04005BC4 RID: 23492
			private readonly GameSettingsService settings;

			// Token: 0x04005BC5 RID: 23493
			private readonly string personalizationString;
		}
	}
}
