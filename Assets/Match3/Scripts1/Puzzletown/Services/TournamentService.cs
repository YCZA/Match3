using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Match3.Scripts1.Puzzletown.Config;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.Leagues;
using Match3.Scripts1.Wooga.Services;
using Wooga.UnityFramework;
using Match3.Scripts2.Env;
using Match3.Scripts2.PlayerData;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x02000814 RID: 2068
	public class TournamentService : AService, ITournamentService
	{
		// Token: 0x0600330C RID: 13068 RVA: 0x000F0D8C File Offset: 0x000EF18C
		public TournamentService()
		{
			if (Application.isPlaying)
			{
				this.CurrentContextID = 0;
				WooroutineRunner.StartCoroutine(this.InitRoutine(), null);
			}
		}

		// Token: 0x1700080E RID: 2062
		// (get) Token: 0x0600330D RID: 13069 RVA: 0x000F0DE5 File Offset: 0x000EF1E5
		// (set) Token: 0x0600330E RID: 13070 RVA: 0x000F0DED File Offset: 0x000EF1ED
		public int CurrentContextID { get; private set; }

		// Token: 0x1700080F RID: 2063
		// (get) Token: 0x0600330F RID: 13071 RVA: 0x000F0DF8 File Offset: 0x000EF1F8
		// (set) Token: 0x06003310 RID: 13072 RVA: 0x000F0E54 File Offset: 0x000EF254
		private bool SimulateBadNetwork
		{
			get
			{
				if (this.simulateBadNetwork == null)
				{
					if (GameEnvironment.IsProduction)
					{
						this.simulateBadNetwork = new bool?(false);
					}
					else
					{
						this.simulateBadNetwork = new bool?(PlayerPrefs.GetInt(TournamentService.SIM_BAD_NETWORK_PP_KEY, 0) == 1);
					}
				}
				return this.simulateBadNetwork.Value;
			}
			set
			{
				this.simulateBadNetwork = new bool?(value);
				PlayerPrefs.SetInt(TournamentService.SIM_BAD_NETWORK_PP_KEY, (!this.simulateBadNetwork.Value) ? 0 : 1);
			}
		}

		// Token: 0x17000810 RID: 2064
		// (get) Token: 0x06003311 RID: 13073 RVA: 0x000F0E84 File Offset: 0x000EF284
		private List<TournamentEventConfig> CurrentlyRunningEvents
		{
			get
			{
				List<TournamentEventConfig> result;
				if ((result = this.currentlyRunningEvents) == null)
				{
					result = (this.currentlyRunningEvents = this.GetCurrentRemoteLeagueConfigs());
				}
				return result;
			}
		}

		// Token: 0x17000811 RID: 2065
		// (get) Token: 0x06003312 RID: 13074 RVA: 0x000F0EAD File Offset: 0x000EF2AD
		public ITournamentServiceStatus Status
		{
			get
			{
				return this.status;
			}
		}

		// Token: 0x06003313 RID: 13075 RVA: 0x000F0EB8 File Offset: 0x000EF2B8
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(this.Status.ToString());
			stringBuilder.Append(string.Format("Simulate bad network? {0}", this.SimulateBadNetwork));
			return stringBuilder.ToString();
		}

		// Token: 0x06003314 RID: 13076 RVA: 0x000F0F00 File Offset: 0x000EF300
		private IEnumerator InitRoutine()
		{
			this.currentContext = SceneContext.MetaGame;
			yield return ServiceLocator.Instance.Inject(this);
			this.status = new TournamentServiceStatus(this, this.configService, this.progressionService, this.gameStateService);
			this.status.IsUserInterestedInTournament = true;
			this.pushNotifier = new TournamentPushNotifier(this.pushNotificationService, -1);
			WooroutineRunner.StartCoroutine(this.UpdatePointsAndStartTicking(true), null);
			base.OnInitialized.Dispatch();
			yield break;
		}

		// Token: 0x06003315 RID: 13077 RVA: 0x000F0F1C File Offset: 0x000EF31C
		private IEnumerator UpdatePointsAndStartTicking(bool fetchIfNotEntered = false)
		{
			if (this.status.IsBeingRefreshed || !SBS.IsAuthenticated())
			{
				yield break;
			}
			this.status.IsBeingRefreshed = true;
			this.RefreshActiveConfigs();
			if (this.ShouldClearCacheAndShowSpinner())
			{
				this.leagueModelCache.Clear();
			}
			yield return this.FixMergeArtifacts();
			yield return this.UpdatePointsOnServerRoutine(fetchIfNotEntered, string.Empty);
			yield return this.FetchCurrentStandings(fetchIfNotEntered);
			this.tickRoutine = WooroutineRunner.StartCoroutine(this.ticker.Tick(this, this.gameStateService, this.activeConfigs), null);
			this.status.IsBeingRefreshed = false;
			yield break;
		}

		// Token: 0x06003316 RID: 13078 RVA: 0x000F0F3E File Offset: 0x000EF33E
		private bool ShouldClearCacheAndShowSpinner()
		{
			return !this.OK_TO_SHOW_OUTDATED_INFO || this.DidLastFetchFail() || this.ActiveTournamentJustEnded();
		}

		// Token: 0x06003317 RID: 13079 RVA: 0x000F0F60 File Offset: 0x000EF360
		private bool DidLastFetchFail()
		{
			LeagueModel activeLeagueState = this.GetActiveLeagueState();
			return activeLeagueState != null && !activeLeagueState.couldFetchFromServer;
		}

		// Token: 0x06003318 RID: 13080 RVA: 0x000F0F86 File Offset: 0x000EF386
		private bool ActiveTournamentJustEnded()
		{
			return this.activeConfigs != null && this.activeConfigs.Count > 0 && this.activeConfigs[0].end < this.Now;
		}

		// Token: 0x17000812 RID: 2066
		// (get) Token: 0x06003319 RID: 13081 RVA: 0x000F0FC3 File Offset: 0x000EF3C3
		public virtual int Now
		{
			get
			{
				return this.timeService.Now.ToUnixTimeStamp();
			}
		}

		// Token: 0x0600331A RID: 13082 RVA: 0x000F0FD8 File Offset: 0x000EF3D8
		public void NotifyContextChange(SceneContext newContext)
		{
			if (newContext != this.currentContext)
			{
				this.CurrentContextID++;
				this.currentContext = newContext;
				SceneContext sceneContext = this.currentContext;
				if (sceneContext != SceneContext.InGame)
				{
					if (sceneContext == SceneContext.MetaGame)
					{
						WooroutineRunner.StartCoroutine(this.UpdatePointsAndStartTicking(false), null);
					}
				}
				else if (this.tickRoutine != null)
				{
					WooroutineRunner.Stop(this.tickRoutine);
				}
			}
		}

		// Token: 0x0600331B RID: 13083 RVA: 0x000F1050 File Offset: 0x000EF450
		public int GetCurrentScoreMultiplierForTier(int zeroBasedTier)
		{
			TournamentConfig tournamentConfig;
			return (!this.TryGetOngoingTournamentConfig(out tournamentConfig)) ? 1 : tournamentConfig.GetScoreMultiplierForZeroBasedTier(zeroBasedTier, false);
		}

		// Token: 0x0600331C RID: 13084 RVA: 0x000F1078 File Offset: 0x000EF478
		public TournamentType GetApparentOngoingTournamentType()
		{
			TournamentConfig tournamentConfig;
			return (!this.TryGetOngoingTournamentConfig(out tournamentConfig)) ? TournamentType.Undefined : tournamentConfig.tournamentType;
		}

		// Token: 0x0600331D RID: 13085 RVA: 0x000F10A0 File Offset: 0x000EF4A0
		public TournamentEventConfig GetActiveTournamentEventConfig()
		{
			if (this.activeConfigs != null && this.activeConfigs.Count > 0)
			{
				return (!this.activeConfigs[0].IsOngoing(this.Now)) ? null : this.activeConfigs[0];
			}
			return null;
		}

		// Token: 0x0600331E RID: 13086 RVA: 0x000F10F9 File Offset: 0x000EF4F9
		public TournamentEventConfig GetFirstUpcomingTournamentEventConfig()
		{
			return EventConfig.GetFirstUpcomingEventConfig<TournamentEventConfig>(this.Now, this.GetAllRemoteLeagueConfigs());
		}

		// Token: 0x0600331F RID: 13087 RVA: 0x000F110C File Offset: 0x000EF50C
		public bool HasPlayerEntered(TournamentEventConfig config)
		{
			string id = config.id;
			return this.gameStateService.Tournaments.IsPlayerInLeague(id);
		}

		// Token: 0x06003320 RID: 13088 RVA: 0x000F1134 File Offset: 0x000EF534
		public void CollectRewardsAndRemoveLocalState(LeagueModel leagueModel, Materials rewards)
		{
			if (rewards != null)
			{
				this.gameStateService.Resources.AddMaterials(rewards, false);
				this.trackingService.TrackTournamentRewards(leagueModel, rewards);
			}
			this.gameStateService.Tournaments.RemoveLocalState(leagueModel.config.id);
			this.gameStateService.Save(Application.isEditor);
			this.facebookService.CleanUnknownOthersImageFromCache();
			if (this.tickRoutine != null)
			{
				WooroutineRunner.Stop(this.tickRoutine);
			}
			WooroutineRunner.StartCoroutine(this.UpdatePointsAndStartTicking(false), null);
		}

		// Token: 0x06003321 RID: 13089 RVA: 0x000F11C0 File Offset: 0x000EF5C0
		public LeagueModel GetActiveLeagueState()
		{
			TournamentEventConfig tournamentEventConfig = (this.activeConfigs != null && this.activeConfigs.Count >= 1) ? this.activeConfigs[0] : new TournamentEventConfig
			{
				id = "-----"
			};
			LeagueModel result;
			LeagueModel leagueModel;
			if (!this.leagueModelCache.TryGet(tournamentEventConfig.id, out result) && this.gameStateService.Tournaments.TryGetLocalStateFor(tournamentEventConfig, out leagueModel))
			{
				if (this.status.IsBeingRefreshed || this.isFetching)
				{
					result = LeagueModelFactory.CreateModelForFetchInProgress(leagueModel);
				}
				else
				{
					WoogaDebug.LogWarning(new object[]
					{
						"TournamentService: No cached active league state, but local state exists, this should not happen."
					});
					result = leagueModel;
				}
			}
			return result;
		}

		// Token: 0x06003322 RID: 13090 RVA: 0x000F127C File Offset: 0x000EF67C
		public Wooroutine<LeagueModel> FetchLeagueState(bool withLeagueUpdate, string leagueID, bool clearCache = false)
		{
			this.isFetching = true;
			TournamentEventConfig leagueConfig = this.GetLeagueConfig(leagueID);
			if (leagueConfig != null)
			{
				if (clearCache)
				{
					this.leagueModelCache.Clear();
				}
				return (!withLeagueUpdate) ? WooroutineRunner.StartWooroutine<LeagueModel>(this.DoFetchLeagueStateRoutine(leagueConfig, false)) : WooroutineRunner.StartWooroutine<LeagueModel>(this.DoUpdateAndFetchRoutine(leagueID));
			}
			this.isFetching = false;
			string user_id = SBS.Authentication.GetUserContext().user_id;
			return WooroutineRunner.StartWooroutine<LeagueModel>(LeagueModelFactory.ReturnInvalidModelRoutine(user_id));
		}

		// Token: 0x06003323 RID: 13091 RVA: 0x000F12F6 File Offset: 0x000EF6F6
		public Wooroutine<LeagueModel> TryEnterLeague(string leagueID)
		{
			return WooroutineRunner.StartWooroutine<LeagueModel>(this.DoTryEnterLeague(leagueID));
		}

		// Token: 0x06003324 RID: 13092 RVA: 0x000F1304 File Offset: 0x000EF704
		public void CheatToggleSimulatedBadNetwork()
		{
			if (!GameEnvironment.IsProduction)
			{
				bool flag = this.SimulateBadNetwork;
				this.SimulateBadNetwork = !flag;
			}
		}

		// Token: 0x06003325 RID: 13093 RVA: 0x000F132C File Offset: 0x000EF72C
		private IEnumerator DoUpdateAndFetchRoutine(string leagueID)
		{
			yield return this.UpdatePointsOnServerRoutine(false, leagueID);
			TournamentEventConfig leagueConfig = this.GetLeagueConfig(leagueID);
			Wooroutine<LeagueModel> fetch = WooroutineRunner.StartWooroutine<LeagueModel>(this.DoFetchLeagueStateRoutine(leagueConfig, false));
			yield return fetch;
			this.leagueModelCache.SendNotifications();
			yield return fetch.ReturnValue;
			yield break;
		}

		// Token: 0x06003326 RID: 13094 RVA: 0x000F1350 File Offset: 0x000EF750
		private IEnumerator UpdatePointsOnServerRoutine(bool fetchIfNotEntered, string onlyInSpecificLeague = "")
		{
			List<SerializedLeagueState> playerLeagueStates = this.gameStateService.Tournaments.GetCurrentPlayerLeagueStates();
			bool needToUpdateLocalState = false;
			if (playerLeagueStates != null)
			{
				foreach (SerializedLeagueState playerLeagueState in playerLeagueStates)
				{
					if (playerLeagueState.PlayerHasEntered || fetchIfNotEntered)
					{
						if (string.IsNullOrEmpty(onlyInSpecificLeague) || !(playerLeagueState.config.id != onlyInSpecificLeague))
						{
							int current = playerLeagueState.currentPoints;
							int previous = playerLeagueState.previousPoints;
							PlayerLeagueStatus oldStatus = playerLeagueState.Status;
							yield return this.UpdatePointsOnServerForState(playerLeagueState);
							needToUpdateLocalState |= (current != playerLeagueState.currentPoints || previous != playerLeagueState.previousPoints || oldStatus != playerLeagueState.Status);
						}
					}
				}
				if (needToUpdateLocalState)
				{
					this.gameStateService.Tournaments.SetCurrentPlayerLeagueStates(playerLeagueStates);
					this.gameStateService.Save(false);
				}
			}
			yield break;
		}

		// Token: 0x06003327 RID: 13095 RVA: 0x000F137C File Offset: 0x000EF77C
		private IEnumerator FixMergeArtifacts()
		{
			string currentUserID = SBS.Authentication.GetUserContext().user_id;
			List<SerializedLeagueState> playerLeagueStates = this.gameStateService.Tournaments.GetCurrentPlayerLeagueStates();
			bool needToSaveAfter = false;
			HashSet<string> leaguesToEnter = new HashSet<string>();
			if (playerLeagueStates != null)
			{
				foreach (SerializedLeagueState serializedLeagueState in playerLeagueStates)
				{
					if (serializedLeagueState.hasEntered && serializedLeagueState.registeredUserID != currentUserID)
					{
						serializedLeagueState.hasEntered = false;
						serializedLeagueState.previousPoints = 0;
						leaguesToEnter.Add(serializedLeagueState.config.id);
						needToSaveAfter = true;
					}
				}
			}
			foreach (string leagueToEnter in leaguesToEnter)
			{
				yield return this.DoTryEnterLeague(leagueToEnter);
			}
			if (needToSaveAfter)
			{
				this.gameStateService.Save(false);
			}
			this.leagueModelCache.SendNotifications();
			yield break;
		}

		// Token: 0x06003328 RID: 13096 RVA: 0x000F1398 File Offset: 0x000EF798
		private TournamentEventConfig GetLeagueConfig(string leagueID)
		{
			return this.activeConfigs.FirstOrDefault((TournamentEventConfig config) => config.id == leagueID);
		}

		// Token: 0x06003329 RID: 13097 RVA: 0x000F13CC File Offset: 0x000EF7CC
		private IEnumerator UpdatePointsOnServerForState(SerializedLeagueState playerLeagueState)
		{
			if (playerLeagueState.currentPoints != playerLeagueState.previousPoints)
			{
				Wooroutine<PointsUpdateResponse> pointsUpdate = SBS.LeagueService.UpdatePoints(playerLeagueState.config.id, playerLeagueState.currentPoints, playerLeagueState.previousPoints).StartWooroutine<PointsUpdateResponse>();
				yield return pointsUpdate;
				PointsUpdateResponse result = pointsUpdate.ReturnValue;
				if (result.couldReachServer)
				{
					if (result.playerNotInLeague)
					{
						playerLeagueState.hasEntered = false;
					}
					else
					{
						playerLeagueState.currentPoints = result.playersActualPoints;
						playerLeagueState.previousPoints = result.playersActualPoints;
					}
				}
			}
			yield break;
		}

		// Token: 0x0600332A RID: 13098 RVA: 0x000F13E8 File Offset: 0x000EF7E8
		private bool TryGetOngoingTournamentConfig(out TournamentConfig config)
		{
			config = null;
			if (this.status.IsUserInterestedInTournament && this.status.IsUnlocked && this.activeConfigs.Count > 0 && this.activeConfigs[0].IsOngoing(this.Now))
			{
				config = this.activeConfigs[0].config;
			}
			return config != null;
		}

		// Token: 0x0600332B RID: 13099 RVA: 0x000F1460 File Offset: 0x000EF860
		private IEnumerator FetchCurrentStandings(bool fetchIfNotEntered)
		{
			if (this.activeConfigs != null && this.activeConfigs.Count > 0)
			{
				foreach (TournamentEventConfig config in this.activeConfigs)
				{
					yield return this.DoFetchLeagueStateRoutine(config, fetchIfNotEntered);
				}
			}
			this.leagueModelCache.SendNotifications();
			yield break;
		}

		// Token: 0x0600332C RID: 13100 RVA: 0x000F1484 File Offset: 0x000EF884
		private IEnumerator DoFetchLeagueStateRoutine(TournamentEventConfig leagueConfig, bool fetchIfNotEntered = false)
		{
			string leagueID = leagueConfig.id;
			PlayerStateInLeague localLeagueState = this.gameStateService.Tournaments.GetPlayerStateInLeague(leagueID);
			bool shouldSaveGameState = false;
			if (localLeagueState.status == PlayerLeagueStatus.None)
			{
				this.gameStateService.Tournaments.CreateLocalStateForLeague(leagueConfig);
				localLeagueState.status = PlayerLeagueStatus.NotQualified;
				shouldSaveGameState = true;
			}
			LeagueModel leagueModel;
			if (fetchIfNotEntered || localLeagueState.status == PlayerLeagueStatus.Entered)
			{
				Wooroutine<StandingsQueryResponse> fetchLeagueStandingsRoutine = SBS.LeagueService.GetLeagueStandings(leagueID).StartWooroutine<StandingsQueryResponse>();
				yield return fetchLeagueStandingsRoutine;
				if (this.SimulateBadNetwork)
				{
					yield return new WaitForSeconds(global::UnityEngine.Random.Range(8f, 16f));
				}
				StandingsQueryResponse result = fetchLeagueStandingsRoutine.ReturnValue;
				leagueModel = LeagueModelFactory.Create(leagueConfig, localLeagueState, SBS.Authentication.GetUserContext().user_id, result);
				if (result.couldFetchFromServer)
				{
					this.ConfirmThatLeagueIsOver(leagueModel);
					bool flag = this.gameStateService.Tournaments.NeedsSaveAfterUpdatingLocalState(leagueModel);
					shouldSaveGameState = (shouldSaveGameState || flag);
				}
			}
			else
			{
				this.gameStateService.Tournaments.TryGetLocalStateFor(leagueConfig, out leagueModel);
			}
			if (shouldSaveGameState)
			{
				this.gameStateService.Save(false);
			}
			this.isFetching = false;
			this.leagueModelCache.Add(leagueModel);
			this.pushNotifier.TryNotifyPlayerWhoGotKickedOutOfTop10(leagueModel, this.Now);
			yield return leagueModel;
			yield break;
		}

		// Token: 0x0600332D RID: 13101 RVA: 0x000F14B0 File Offset: 0x000EF8B0
		private IEnumerator DoTryEnterLeague(string leagueID)
		{
			this.isFetching = true;
			this.leagueModelCache.Clear();
			PlayerStateInLeague localState = this.gameStateService.Tournaments.GetPlayerStateInLeague(leagueID);
			if (localState.status != PlayerLeagueStatus.NotEnteredButQualified)
			{
				this.isFetching = false;
				yield break;
			}
			PlayerInLeague player = PlayerInLeageHelper.CreateNewUser(this.gameStateService, this.facebookService, localState.currentPoints);
			Wooroutine<StandingsQueryResponse> registerQuery = SBS.LeagueService.RegisterToLeague(leagueID, player).StartWooroutine<StandingsQueryResponse>();
			yield return registerQuery;
			StandingsQueryResponse registerResult = registerQuery.ReturnValue;
			Wooroutine<LeagueModel> createModelFromRegisterResultRoutine;
			if (!registerResult.couldFetchFromServer || !registerResult.playerIsMemberOfLeague)
			{
				createModelFromRegisterResultRoutine = WooroutineRunner.StartWooroutine<LeagueModel>(this.CreateRegFailedModelRoutine(leagueID, localState));
			}
			else
			{
				createModelFromRegisterResultRoutine = WooroutineRunner.StartWooroutine<LeagueModel>(this.CreateRegSuccessModelRoutine(leagueID, registerResult));
			}
			yield return createModelFromRegisterResultRoutine;
			LeagueModel leagueModel = createModelFromRegisterResultRoutine.ReturnValue;
			this.isFetching = false;
			this.leagueModelCache.Add(leagueModel);
			yield return leagueModel;
			yield break;
		}

		// Token: 0x0600332E RID: 13102 RVA: 0x000F14D4 File Offset: 0x000EF8D4
		private IEnumerator CreateRegSuccessModelRoutine(string leagueID, StandingsQueryResponse registerResult)
		{
			if (registerResult.playerHadAlreadyRegisteredBefore)
			{
				WoogaDebug.LogWarning(new object[]
				{
					"Tournament: User trying to double register."
				});
				this.isFetching = true;
				this.leagueModelCache.Clear();
				TournamentEventConfig leagueConfig = this.GetLeagueConfig(leagueID);
				Wooroutine<LeagueModel> fetchRoutine = WooroutineRunner.StartWooroutine<LeagueModel>(this.DoFetchLeagueStateRoutine(leagueConfig, true));
				yield return fetchRoutine;
				yield return fetchRoutine.ReturnValue;
			}
			else
			{
				PlayerStateInLeague playerStateInLeague = this.UpdateLocalStateAfterEnteringLeague(leagueID, SBS.Authentication.GetUserContext().user_id, registerResult.user.points, registerResult.tier);
				yield return LeagueModelFactory.Create(this.GetLeagueConfig(leagueID), playerStateInLeague, registerResult.user.sbs_user_id, registerResult);
			}
			yield break;
		}

		// Token: 0x0600332F RID: 13103 RVA: 0x000F1500 File Offset: 0x000EF900
		private IEnumerator CreateRegFailedModelRoutine(string leagueID, PlayerStateInLeague localState)
		{
			LeagueModel modelFromLocalState;
			if (this.gameStateService.Tournaments.TryGetLocalStateFor(this.GetLeagueConfig(leagueID), out modelFromLocalState))
			{
				yield return modelFromLocalState;
			}
			else
			{
				yield return LeagueModelFactory.PlayerNotInLeague(this.GetLeagueConfig(leagueID), localState);
			}
			yield break;
		}

		// Token: 0x06003330 RID: 13104 RVA: 0x000F152C File Offset: 0x000EF92C
		private PlayerStateInLeague UpdateLocalStateAfterEnteringLeague(string leagueID, string userID, int currentPoints, string tier)
		{
			TournamentEventConfig leagueConfig = this.GetLeagueConfig(leagueID);
			this.gameStateService.Tournaments.TryEnterLeague(leagueConfig, userID, currentPoints, tier);
			return this.gameStateService.Tournaments.GetPlayerStateInLeague(leagueID);
		}

		// Token: 0x06003331 RID: 13105 RVA: 0x000F1568 File Offset: 0x000EF968
		private void MergeRemoteAndLocalLeagueConfigs(TournamentEventConfig remoteConfig, SerializedLeagueState localState)
		{
			int pointsToQualify = localState.config.config.pointsToQualify;
			localState.config = remoteConfig;
			if (localState.PlayerHasEntered)
			{
				localState.config.config.pointsToQualify = pointsToQualify;
			}
		}

		// Token: 0x06003332 RID: 13106 RVA: 0x000F15AC File Offset: 0x000EF9AC
		private void RefreshActiveConfigs()
		{
			this.activeConfigs.Clear();
			this.currentlyRunningEvents = null;
			HashSet<string> hashSet = new HashSet<string>();
			int now = this.Now;
			List<string> list = new List<string>();
			foreach (SerializedLeagueState serializedLeagueState in this.gameStateService.Tournaments.GetCurrentPlayerLeagueStates())
			{
				TournamentEventConfig remoteConfig;
				if (!this.TryGetLatestRemoteConfigFor(serializedLeagueState.config.id, out remoteConfig))
				{
					WoogaDebug.Log(new object[]
					{
						"TournamentService: found saved state without remote config => league is assumed to be over!"
					});
					int finished = Mathf.Min(now - 1, serializedLeagueState.config.end);
					serializedLeagueState.SetFinished(finished);
				}
				else
				{
					this.MergeRemoteAndLocalLeagueConfigs(remoteConfig, serializedLeagueState);
				}
				if (this.ShouldRemoveLocalState(serializedLeagueState))
				{
					list.Add(serializedLeagueState.config.id);
				}
				else
				{
					this.activeConfigs.Add(serializedLeagueState.config);
					hashSet.Add(serializedLeagueState.config.id);
				}
			}
			foreach (string id in list)
			{
				this.gameStateService.Tournaments.RemoveLocalState(id);
			}
			foreach (TournamentEventConfig tournamentEventConfig in this.CurrentlyRunningEvents)
			{
				if (!hashSet.Contains(tournamentEventConfig.id))
				{
					this.activeConfigs.Add(tournamentEventConfig);
					hashSet.Add(tournamentEventConfig.id);
				}
			}
			List<TournamentEventConfig> list2 = this.activeConfigs;
			if (TournamentService._003C_003Ef__mg_0024cache0 == null)
			{
				TournamentService._003C_003Ef__mg_0024cache0 = new Comparison<TournamentEventConfig>(EventConfig.SortByEndTimeAscending<TournamentEventConfig>);
			}
			list2.Sort(TournamentService._003C_003Ef__mg_0024cache0);
		}

		// Token: 0x06003333 RID: 13107 RVA: 0x000F17BC File Offset: 0x000EFBBC
		private bool ShouldRemoveLocalState(SerializedLeagueState savedLeagueState)
		{
			return savedLeagueState.config.end <= this.Now && !savedLeagueState.PlayerHasEntered;
		}

		// Token: 0x06003334 RID: 13108 RVA: 0x000F17E0 File Offset: 0x000EFBE0
		private bool TryGetLatestRemoteConfigFor(string leagueID, out TournamentEventConfig latestConfig)
		{
			latestConfig = new TournamentEventConfig();
			foreach (TournamentEventConfig tournamentEventConfig in this.CurrentlyRunningEvents)
			{
				if (tournamentEventConfig.id == leagueID)
				{
					latestConfig = tournamentEventConfig;
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003335 RID: 13109 RVA: 0x000F185C File Offset: 0x000EFC5C
		private List<TournamentEventConfig> GetAllRemoteLeagueConfigs()
		{
			if (this.configService.SbsConfig._events != null && this.configService.SbsConfig._events.leagues != null)
			{
				return this.configService.SbsConfig._events.leagues;
			}
			return null;
		}

		// Token: 0x06003336 RID: 13110 RVA: 0x000F18B0 File Offset: 0x000EFCB0
		private List<TournamentEventConfig> GetCurrentRemoteLeagueConfigs()
		{
			List<TournamentEventConfig> list = new List<TournamentEventConfig>();
			int now = this.Now;
			List<TournamentEventConfig> allRemoteLeagueConfigs = this.GetAllRemoteLeagueConfigs();
			if (allRemoteLeagueConfigs != null)
			{
				list.AddRange(from config in allRemoteLeagueConfigs
				where config.IsOngoing(now)
				select config);
			}
			return list;
		}

		// Token: 0x04005B59 RID: 23385
		public static string SIM_BAD_NETWORK_PP_KEY = "SIM_BAD_NETWORK";

		// Token: 0x04005B5A RID: 23386
		[WaitForService(true, true)]
		public readonly ProgressionDataService.Service progressionService;

		// Token: 0x04005B5B RID: 23387
		[WaitForService(true, true)]
		public readonly ConfigService configService;

		// Token: 0x04005B5C RID: 23388
		[WaitForService(true, true)]
		protected GameStateService gameStateService;

		// Token: 0x04005B5D RID: 23389
		[WaitForService(true, true)]
		protected FacebookService facebookService;

		// Token: 0x04005B5E RID: 23390
		[WaitForService(true, true)]
		protected TrackingService trackingService;

		// Token: 0x04005B5F RID: 23391
		[WaitForService(true, true)]
		protected PushNotificationService pushNotificationService;

		// Token: 0x04005B60 RID: 23392
		[WaitForService(true, true)]
		protected TimeService timeService;

		// Token: 0x04005B61 RID: 23393
		public bool SHOW_FULLSCREEN_SPINNER;

		// Token: 0x04005B62 RID: 23394
		public bool OK_TO_SHOW_OUTDATED_INFO = true;

		// Token: 0x04005B64 RID: 23396
		private SceneContext currentContext;

		// Token: 0x04005B65 RID: 23397
		private Coroutine tickRoutine;

		// Token: 0x04005B66 RID: 23398
		private bool isFetching;

		// Token: 0x04005B67 RID: 23399
		public TournamentTimeTicker ticker = new TournamentTimeTicker();

		// Token: 0x04005B68 RID: 23400
		public LeagueModelCache leagueModelCache = new LeagueModelCache();

		// Token: 0x04005B69 RID: 23401
		private List<TournamentEventConfig> activeConfigs = new List<TournamentEventConfig>();

		// Token: 0x04005B6A RID: 23402
		private bool? simulateBadNetwork;

		// Token: 0x04005B6B RID: 23403
		private List<TournamentEventConfig> currentlyRunningEvents;

		// Token: 0x04005B6C RID: 23404
		private TournamentPushNotifier pushNotifier;

		// Token: 0x04005B6D RID: 23405
		private TournamentServiceStatus status;

		// Token: 0x04005B6E RID: 23406
		[CompilerGenerated]
		private static Comparison<TournamentEventConfig> _003C_003Ef__mg_0024cache0;
	}
}
