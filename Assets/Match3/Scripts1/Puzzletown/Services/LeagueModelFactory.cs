using System;
using System.Collections;
using System.Collections.Generic;
using Match3.Scripts1.Puzzletown.Config;
using Match3.Scripts1.Wooga.Leagues;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x0200080E RID: 2062
	public static class LeagueModelFactory
	{
		// Token: 0x060032E9 RID: 13033 RVA: 0x000EFDF4 File Offset: 0x000EE1F4
		public static IEnumerator ReturnInvalidModelRoutine(string userID)
		{
			yield return LeagueModelFactory.CreateInvalidModel(userID);
			yield break;
		}

		// Token: 0x060032EA RID: 13034 RVA: 0x000EFE10 File Offset: 0x000EE210
		public static LeagueModel FromSavedState(SerializedLeagueState state)
		{
			return new LeagueModel(state.registeredUserID, state.tier, false)
			{
				config = state.config,
				playerStatus = state.Status,
				playerCurrentPoints = state.currentPoints
			};
		}

		// Token: 0x060032EB RID: 13035 RVA: 0x000EFE58 File Offset: 0x000EE258
		public static LeagueModel CreateInvalidModel(string userID)
		{
			return new LeagueModel(userID, string.Empty, false);
		}

		// Token: 0x060032EC RID: 13036 RVA: 0x000EFE74 File Offset: 0x000EE274
		public static LeagueModel CreateModelForFetchInProgress(LeagueModel localState)
		{
			return new LeagueModel(localState.userID, localState.tier, localState.confirmedOver)
			{
				config = localState.config.Copy(),
				playerStatus = localState.playerStatus,
				playerCurrentPoints = localState.playerCurrentPoints,
				couldFetchFromServer = false,
				fetchInProgress = true
			};
		}

		// Token: 0x060032ED RID: 13037 RVA: 0x000EFED4 File Offset: 0x000EE2D4
		public static LeagueModel PlayerNotInLeague(TournamentEventConfig config, PlayerStateInLeague state)
		{
			return new LeagueModel(state.userID, state.tier, false)
			{
				config = config,
				playerStatus = state.status,
				playerCurrentPoints = state.currentPoints
			};
		}

		// Token: 0x060032EE RID: 13038 RVA: 0x000EFF18 File Offset: 0x000EE318
		public static LeagueModel Create(TournamentEventConfig evConfig, PlayerStateInLeague localState, string sbsUserID, StandingsQueryResponse response)
		{
			LeagueEntry[] standings = response.standings;
			PlayerLeagueStatus playerStatus = LeagueModelFactory.MergeLocalAndServerStatuses(evConfig, localState, response);
			int playerCurrentPoints = LeagueModelFactory.MergeLocalAndServerStatePoints(localState, response, sbsUserID);
			return new LeagueModel(sbsUserID, localState.tier, response.IsConfirmedOver)
			{
				couldFetchFromServer = response.couldFetchFromServer,
				config = evConfig,
				playerStatus = playerStatus,
				playerCurrentPoints = playerCurrentPoints,
				sortedStandings = standings
			};
		}

		// Token: 0x060032EF RID: 13039 RVA: 0x000EFF84 File Offset: 0x000EE384
		private static PlayerLeagueStatus MergeLocalAndServerStatuses(TournamentEventConfig config, PlayerStateInLeague localState, StandingsQueryResponse response)
		{
			if (!response.couldFetchFromServer)
			{
				return localState.status;
			}
			if (response.playerIsMemberOfLeague)
			{
				return PlayerLeagueStatus.Entered;
			}
			if (localState.status == PlayerLeagueStatus.Entered)
			{
				return (localState.currentPoints < config.config.pointsToQualify) ? PlayerLeagueStatus.NotQualified : PlayerLeagueStatus.NotEnteredButQualified;
			}
			return localState.status;
		}

		// Token: 0x060032F0 RID: 13040 RVA: 0x000EFFE4 File Offset: 0x000EE3E4
		private static int MergeLocalAndServerStatePoints(PlayerStateInLeague localState, StandingsQueryResponse response, string playersUserID)
		{
			if (response.couldFetchFromServer && response.playerIsMemberOfLeague)
			{
				foreach (LeagueEntry leagueEntry in response.standings)
				{
					if (leagueEntry.sbs_user_id == playersUserID)
					{
						return leagueEntry.points;
					}
				}
			}
			return localState.currentPoints;
		}

		// Token: 0x060032F1 RID: 13041 RVA: 0x000F0048 File Offset: 0x000EE448
		public static LeagueModel MockModelForTesting(TournamentResultTester testConfig = null)
		{
			if (LeagueModelFactory._modelForTesting != null)
			{
				return LeagueModelFactory._modelForTesting;
			}
			List<string> list = new List<string>
			{
				"621971161",
				"567133863",
				"656502801",
				"792525172",
				"583568041",
				"566551741",
				"653895121",
				"504365763",
				"620241875",
				"657716679"
			};
			LeagueModel leagueModel = new LeagueModel("testUser", string.Empty, false);
			leagueModel.couldFetchFromServer = true;
			leagueModel.config = new TournamentEventConfig();
			leagueModel.config.id = "mock";
			leagueModel.config.start = 10;
			leagueModel.config.end = 20;
			leagueModel.config.config = new TournamentConfig();
			leagueModel.config.config.name = "MockedLeague";
			leagueModel.config.config.tournamentType = ((!(testConfig == null)) ? testConfig.type : TournamentType.Strawberry);
			leagueModel.config.config.pointsToQualify = 0;
			leagueModel.playerStatus = PlayerLeagueStatus.Entered;
			leagueModel.playerCurrentPoints = 99;
			leagueModel.sortedStandings = new LeagueEntry[20];
			for (int i = 0; i < leagueModel.sortedStandings.Length; i++)
			{
				leagueModel.sortedStandings[i] = new LeagueEntry();
				leagueModel.sortedStandings[i].name = string.Format("Other_{0}", i);
				leagueModel.sortedStandings[i].points = 99 - i * 2;
				if (i % 9 != 0)
				{
					leagueModel.sortedStandings[i].user_data = new LeagueUserData
					{
						FBID = list[i % list.Count]
					};
				}
			}
			int num = (!(testConfig == null)) ? testConfig.position : 1;
			int num2 = num - 1;
			leagueModel.sortedStandings[num2].name = "Player";
			leagueModel.sortedStandings[num2].sbs_user_id = "testUser";
			leagueModel.sortedStandings[num2].user_data = new LeagueUserData
			{
				FBID = "632312058"
			};
			leagueModel.playerCurrentPoints = leagueModel.sortedStandings[num2].points;
			LeagueModelFactory.ConfigureTestRewards(leagueModel, num, testConfig);
			LeagueModelFactory._modelForTesting = leagueModel;
			return leagueModel;
		}

		// Token: 0x060032F2 RID: 13042 RVA: 0x000F02BC File Offset: 0x000EE6BC
		public static LeagueModel MockModelForQualifyingTesting(bool hasPlayerQualified)
		{
			LeagueModel leagueModel = LeagueModelFactory.MockModelForTesting(null);
			leagueModel.config.end = DateTime.UtcNow.ToUnixTimeStamp() + 505;
			leagueModel.playerStatus = ((!hasPlayerQualified) ? PlayerLeagueStatus.NotQualified : PlayerLeagueStatus.NotEnteredButQualified);
			leagueModel.config.config.pointsToQualify = 20;
			leagueModel.playerCurrentPoints = ((!hasPlayerQualified) ? 14 : 20);
			return leagueModel;
		}

		// Token: 0x060032F3 RID: 13043 RVA: 0x000F0328 File Offset: 0x000EE728
		public static LeagueModel MockModelForRewardsPreview()
		{
			LeagueModel leagueModel = LeagueModelFactory.MockModelForTesting(null);
			TournamentRewardConfig tournamentRewardConfig = new TournamentRewardConfig();
			tournamentRewardConfig.wood = new MaterialAmount[]
			{
				new MaterialAmount("boost_star", 1, MaterialAmountUsage.Undefined, 0),
				new MaterialAmount("boost_hammer", 1, MaterialAmountUsage.Undefined, 0)
			};
			tournamentRewardConfig.bronze = new MaterialAmount[]
			{
				new MaterialAmount("boost_pre_double_fish", 3, MaterialAmountUsage.Undefined, 0),
				new MaterialAmount("boost_star", 1, MaterialAmountUsage.Undefined, 0),
				new MaterialAmount("boost_hammer", 1, MaterialAmountUsage.Undefined, 0)
			};
			tournamentRewardConfig.silver = new MaterialAmount[]
			{
				new MaterialAmount("boost_pre_double_fish", 3, MaterialAmountUsage.Undefined, 0),
				new MaterialAmount("boost_rainbow", 1, MaterialAmountUsage.Undefined, 0),
				new MaterialAmount("boost_star", 1, MaterialAmountUsage.Undefined, 0),
				new MaterialAmount("boost_hammer", 1, MaterialAmountUsage.Undefined, 0)
			};
			tournamentRewardConfig.gold = new MaterialAmount[]
			{
				new MaterialAmount("boost_pre_rainbow", 3, MaterialAmountUsage.Undefined, 0),
				new MaterialAmount("boost_pre_double_fish", 3, MaterialAmountUsage.Undefined, 0),
				new MaterialAmount("boost_pre_bomb_line", 3, MaterialAmountUsage.Undefined, 0),
				new MaterialAmount("boost_rainbow", 3, MaterialAmountUsage.Undefined, 0),
				new MaterialAmount("boost_star", 3, MaterialAmountUsage.Undefined, 0),
				new MaterialAmount("boost_hammer", 3, MaterialAmountUsage.Undefined, 0)
			};
			leagueModel.config.config.rewards = tournamentRewardConfig;
			return leagueModel;
		}

		// Token: 0x060032F4 RID: 13044 RVA: 0x000F04FC File Offset: 0x000EE8FC
		public static LeagueModel MockResultTestModel(int playerPosition, TournamentType tType = TournamentType.Strawberry)
		{
			LeagueModel leagueModel = new LeagueModel("testUser", string.Empty, false);
			leagueModel.couldFetchFromServer = true;
			leagueModel.config = new TournamentEventConfig();
			leagueModel.config.id = "mock";
			leagueModel.config.start = 10;
			leagueModel.config.end = 20;
			leagueModel.config.config = new TournamentConfig();
			leagueModel.config.config.name = "MockedLeague";
			leagueModel.config.config.tournamentType = tType;
			leagueModel.config.config.pointsToQualify = 10;
			leagueModel.playerStatus = PlayerLeagueStatus.Entered;
			leagueModel.playerCurrentPoints = 99;
			leagueModel.sortedStandings = new LeagueEntry[50];
			for (int i = 0; i < leagueModel.sortedStandings.Length; i++)
			{
				leagueModel.sortedStandings[i] = new LeagueEntry();
				leagueModel.sortedStandings[i].name = string.Format("Other_{0}", i);
				leagueModel.sortedStandings[i].points = 99 - i * 2;
			}
			int num = playerPosition - 1;
			leagueModel.sortedStandings[num].name = "Player";
			leagueModel.sortedStandings[num].sbs_user_id = "testUser";
			leagueModel.sortedStandings[num].user_data = new LeagueUserData
			{
				FBID = "632312058"
			};
			leagueModel.playerCurrentPoints = leagueModel.sortedStandings[num].points;
			LeagueModelFactory.ConfigureTestRewards(leagueModel, playerPosition, null);
			return leagueModel;
		}

		// Token: 0x060032F5 RID: 13045 RVA: 0x000F0674 File Offset: 0x000EEA74
		public static void ConfigureTestRewards(LeagueModel lm, int position, TournamentResultTester testConfig)
		{
			TournamentRewardConfig tournamentRewardConfig = new TournamentRewardConfig();
			lm.config.config.rewards = tournamentRewardConfig;
			TournamentRewardCategory rewardCategoryFor = lm.config.config.rewards.GetRewardCategoryFor(position);
			if (rewardCategoryFor != TournamentRewardCategory.None)
			{
				MaterialAmount[] array = LeagueModelFactory.ParseTestRewardsConfig(testConfig);
				switch (rewardCategoryFor)
				{
				case TournamentRewardCategory.Gold:
					tournamentRewardConfig.gold = array;
					break;
				case TournamentRewardCategory.Silver:
					tournamentRewardConfig.silver = array;
					break;
				case TournamentRewardCategory.Bronze:
					tournamentRewardConfig.bronze = array;
					break;
				case TournamentRewardCategory.Wood:
					tournamentRewardConfig.wood = array;
					break;
				}
			}
		}

		// Token: 0x060032F6 RID: 13046 RVA: 0x000F070C File Offset: 0x000EEB0C
		public static MaterialAmount[] ParseTestRewardsConfig(TournamentResultTester testConfig)
		{
			List<MaterialAmount> list = new List<MaterialAmount>();
			if (testConfig == null)
			{
				list.Add(new MaterialAmount("coins", 999, MaterialAmountUsage.Undefined, 0));
				list.Add(new MaterialAmount("boost_hammer", 2, MaterialAmountUsage.Undefined, 0));
			}
			else
			{
				list.Add(new MaterialAmount("coins", testConfig.coins, MaterialAmountUsage.Undefined, 0));
				list.Add(new MaterialAmount("diamonds", testConfig.diamonds, MaterialAmountUsage.Undefined, 0));
				list.Add(new MaterialAmount("boost_hammer", testConfig.igBoostHammers, MaterialAmountUsage.Undefined, 0));
				list.Add(new MaterialAmount("boost_rainbow", testConfig.igBoostRainbows, MaterialAmountUsage.Undefined, 0));
				list.Add(new MaterialAmount("boost_star", testConfig.igBoostStars, MaterialAmountUsage.Undefined, 0));
				list.Add(new MaterialAmount("boost_pre_bomb_linegem", testConfig.pgBoostLineBombs, MaterialAmountUsage.Undefined, 0));
				list.Add(new MaterialAmount("boost_pre_double_fish", testConfig.pgBoostButterflies, MaterialAmountUsage.Undefined, 0));
				list.Add(new MaterialAmount("boost_pre_rainbow", testConfig.pgBoostRainbows, MaterialAmountUsage.Undefined, 0));
				list.RemoveAll((MaterialAmount mAmount) => mAmount.amount < 1);
			}
			return list.ToArray();
		}

		// Token: 0x04005B46 RID: 23366
		private static LeagueModel _modelForTesting;
	}
}
