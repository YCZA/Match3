using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Match3.Scripts1;
using Match3.Scripts1.Puzzletown.Build;
using Match3.Scripts1.Puzzletown.Features.DailyGifts;
using Match3.Scripts1.Puzzletown.GameStateSelector;
using Match3.Scripts1.Puzzletown.Match3;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Puzzletown.Town;
using Match3.Scripts1.Wooga.Core.Data;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.Services;
using Wooga.UnityFramework;
using Match3.Scripts2.Env;
using Match3.Scripts2.Start;
using UnityEngine;
using Version = Match3.Scripts1.Puzzletown.Build.Version;

namespace Match3.Scripts2.PlayerData
{
	// Token: 0x020007B1 RID: 1969
	public class GameStateService : AService
	{
		// Token: 0x06003023 RID: 12323 RVA: 0x000E1E7E File Offset: 0x000E027E
		public GameStateService()
		{
			WooroutineRunner.StartCoroutine(this.LoadRoutine(), null);
		}

		// Token: 0x17000782 RID: 1922
		// (get) Token: 0x06003024 RID: 12324 RVA: 0x000E1E9E File Offset: 0x000E029E
		public bool IsMyOwnState
		{
			get
			{
				return this.state != null && this.state.isSaveable;
			}
		}

		// Token: 0x17000783 RID: 1923
		// (get) Token: 0x06003025 RID: 12325 RVA: 0x000E1EBC File Offset: 0x000E02BC
		public static string DeviceId
		{
			get
			{
				return Scripts1.Wooga.Core.DeviceInfo.DeviceId.uniqueIdentifier;
			}
		}

		// Token: 0x17000784 RID: 1924
		// (get) Token: 0x06003026 RID: 12326 RVA: 0x000E1EC3 File Offset: 0x000E02C3
		public string InstallVersion
		{
			get
			{
				return this.state.installVersion;
			}
		}

		// Token: 0x17000785 RID: 1925
		// (get) Token: 0x06003027 RID: 12327 RVA: 0x000E1ED0 File Offset: 0x000E02D0
		public int InstallTimestamp
		{
			get
			{
				return this.state.installTimestamp;
			}
		}

		// Token: 0x06003028 RID: 12328 RVA: 0x000E1EE0 File Offset: 0x000E02E0
		public bool IsNewUserFromVersion(int major, int minor)
		{
			if (this.InstallVersion.IsNullOrEmpty())
			{
				return false;
			}
			Version version = new Version(this.InstallVersion);
			return version.major >= major && version.minor >= minor;
		}

		// Token: 0x06003029 RID: 12329 RVA: 0x000E1F26 File Offset: 0x000E0326
		private GameState GetState()
		{
			return this.state;
		}

		// Token: 0x17000786 RID: 1926
		// (get) Token: 0x0600302A RID: 12330 RVA: 0x000E1F2E File Offset: 0x000E032E
		// (set) Token: 0x0600302B RID: 12331 RVA: 0x000E1F67 File Offset: 0x000E0367
		public DateTime UnlimitedLivesEnd
		{
			get
			{
				if (this.state == null || this.state.adSpinData == null)
				{
					return DateTime.Now;
				}
				return Scripts1.DateTimeExtensions.FromUnixTimeStamp(this.state.adSpinData.unlimitedLivesEnd, DateTimeKind.Utc);
			}
			set
			{
				if (this.state == null || this.state.adSpinData == null)
				{
					return;
				}
				this.state.adSpinData.unlimitedLivesEnd = value.ToUnixTimeStamp();
			}
		}

		// Token: 0x17000787 RID: 1927
		// (get) Token: 0x0600302C RID: 12332 RVA: 0x000E1F9B File Offset: 0x000E039B
		// (set) Token: 0x0600302D RID: 12333 RVA: 0x000E1FD4 File Offset: 0x000E03D4
		public DateTime UnlimitedLivesStart
		{
			get
			{
				if (this.state == null || this.state.adSpinData == null)
				{
					return DateTime.Now;
				}
				return Scripts1.DateTimeExtensions.FromUnixTimeStamp(this.state.adSpinData.unlimitedLivesStart, DateTimeKind.Utc);
			}
			set
			{
				if (this.state == null || this.state.adSpinData == null)
				{
					return;
				}
				this.state.adSpinData.unlimitedLivesStart = value.ToUnixTimeStamp();
			}
		}

		// Token: 0x17000788 RID: 1928
		// (get) Token: 0x0600302E RID: 12334 RVA: 0x000E2008 File Offset: 0x000E0408
		// (set) Token: 0x0600302F RID: 12335 RVA: 0x000E201A File Offset: 0x000E041A
		public int SpinJackpotProgress
		{
			get
			{
				return this.state.adSpinData.jackpotProgress;
			}
			set
			{
				this.state.adSpinData.jackpotProgress = value;
			}
		}

		// Token: 0x17000789 RID: 1929
		// (get) Token: 0x06003030 RID: 12336 RVA: 0x000E202D File Offset: 0x000E042D
		// (set) Token: 0x06003031 RID: 12337 RVA: 0x000E203F File Offset: 0x000E043F
		public int NextSpinAvailable
		{
			get
			{
				return this.state.adSpinData.nextSpinAvailable;
			}
			set
			{
				this.state.adSpinData.nextSpinAvailable = value;
			}
		}

		// Token: 0x1700078A RID: 1930
		// (get) Token: 0x06003032 RID: 12338 RVA: 0x000E2052 File Offset: 0x000E0452
		// (set) Token: 0x06003033 RID: 12339 RVA: 0x000E208B File Offset: 0x000E048B
		public DateTime lastVideoWatchedDate
		{
			get
			{
				if (this.state == null || this.state.adSpinData == null)
				{
					return DateTime.Now;
				}
				return Scripts1.DateTimeExtensions.FromUnixTimeStamp(this.state.adSpinData.lastWatchedAdData, DateTimeKind.Utc);
			}
			set
			{
				if (this.state == null || this.state.adSpinData == null)
				{
					return;
				}
				this.state.adSpinData.lastWatchedAdData = value.ToUnixTimeStamp();
			}
		}

		// Token: 0x1700078B RID: 1931
		// (get) Token: 0x06003034 RID: 12340 RVA: 0x000E20BF File Offset: 0x000E04BF
		// (set) Token: 0x06003035 RID: 12341 RVA: 0x000E20CC File Offset: 0x000E04CC
		public string PushToken
		{
			get
			{
				return this.state.pushToken;
			}
			set
			{
				this.state.pushToken = value;
			}
		}

		// Token: 0x06003036 RID: 12342 RVA: 0x000E20DA File Offset: 0x000E04DA
		public bool doLoadSpecificGameState(GameStateService.GameStateData newState)
		{
			if (newState == null || newState.format_version != 5)
			{
				return false;
			}
			this.state = newState.data;
			this.state.isSaveable = false;
			return true;
		}

		// Token: 0x06003037 RID: 12343 RVA: 0x000E2109 File Offset: 0x000E0509
		public void RestoreGameState()
		{
			// eli key point重载游戏
			UnityEngine.Debug.LogError("重载游戏");
			// this.state = APlayerPrefsObject<GameState>.Load();
		}

		// Token: 0x1700078C RID: 1932
		// (get) Token: 0x06003038 RID: 12344 RVA: 0x000E2116 File Offset: 0x000E0516
		// (set) Token: 0x06003039 RID: 12345 RVA: 0x000E211E File Offset: 0x000E051E
		public ResourceDataService Resources { get; private set; }

		// Token: 0x1700078D RID: 1933
		// (get) Token: 0x0600303A RID: 12346 RVA: 0x000E2127 File Offset: 0x000E0527
		// (set) Token: 0x0600303B RID: 12347 RVA: 0x000E212F File Offset: 0x000E052F
		public BuildingDataService Buildings { get; private set; }

		// Token: 0x1700078E RID: 1934
		// (get) Token: 0x0600303C RID: 12348 RVA: 0x000E2138 File Offset: 0x000E0538
		// (set) Token: 0x0600303D RID: 12349 RVA: 0x000E2140 File Offset: 0x000E0540
		public ProgressionDataService Progression { get; private set; }

		// Token: 0x1700078F RID: 1935
		// (get) Token: 0x0600303E RID: 12350 RVA: 0x000E2149 File Offset: 0x000E0549
		// (set) Token: 0x0600303F RID: 12351 RVA: 0x000E2151 File Offset: 0x000E0551
		public LivesDataService Lives { get; private set; }

		// Token: 0x17000790 RID: 1936
		// (get) Token: 0x06003040 RID: 12352 RVA: 0x000E215A File Offset: 0x000E055A
		// (set) Token: 0x06003041 RID: 12353 RVA: 0x000E2162 File Offset: 0x000E0562
		public QuestsDataService Quests { get; private set; }

		// Token: 0x17000791 RID: 1937
		// (get) Token: 0x06003042 RID: 12354 RVA: 0x000E216B File Offset: 0x000E056B
		// (set) Token: 0x06003043 RID: 12355 RVA: 0x000E2173 File Offset: 0x000E0573
		public FacebookDataService Facebook { get; private set; }

		// Token: 0x17000792 RID: 1938
		// (get) Token: 0x06003044 RID: 12356 RVA: 0x000E217C File Offset: 0x000E057C
		// (set) Token: 0x06003045 RID: 12357 RVA: 0x000E2184 File Offset: 0x000E0584
		public DebugDataService Debug { get; private set; }

		// Token: 0x17000793 RID: 1939
		// (get) Token: 0x06003046 RID: 12358 RVA: 0x000E218D File Offset: 0x000E058D
		// (set) Token: 0x06003047 RID: 12359 RVA: 0x000E2195 File Offset: 0x000E0595
		public TransactionDataService Transactions { get; private set; }

		// Token: 0x17000794 RID: 1940
		// (get) Token: 0x06003048 RID: 12360 RVA: 0x000E219E File Offset: 0x000E059E
		// (set) Token: 0x06003049 RID: 12361 RVA: 0x000E21A6 File Offset: 0x000E05A6
		public TournamentDataService Tournaments { get; private set; }

		// Token: 0x17000795 RID: 1941
		// (get) Token: 0x0600304A RID: 12362 RVA: 0x000E21AF File Offset: 0x000E05AF
		// (set) Token: 0x0600304B RID: 12363 RVA: 0x000E21B7 File Offset: 0x000E05B7
		public ChallengeDataService Challenges { get; private set; }

		// Token: 0x17000796 RID: 1942
		// (get) Token: 0x0600304C RID: 12364 RVA: 0x000E21C0 File Offset: 0x000E05C0
		// (set) Token: 0x0600304D RID: 12365 RVA: 0x000E21C8 File Offset: 0x000E05C8
		public BankDataService Bank { get; private set; }

		// Token: 0x17000797 RID: 1943
		// (get) Token: 0x0600304E RID: 12366 RVA: 0x000E21D1 File Offset: 0x000E05D1
		// (set) Token: 0x0600304F RID: 12367 RVA: 0x000E21D9 File Offset: 0x000E05D9
		public DiveForTreasureDataService DiveForTreasure { get; private set; }

		// Token: 0x17000798 RID: 1944
		// (get) Token: 0x06003050 RID: 12368 RVA: 0x000E21E2 File Offset: 0x000E05E2
		// (set) Token: 0x06003051 RID: 12369 RVA: 0x000E21EA File Offset: 0x000E05EA
		public PirateBreakOutDataService PirateBreakout { get; private set; }

		// Token: 0x17000799 RID: 1945
		// (get) Token: 0x06003052 RID: 12370 RVA: 0x000E21F3 File Offset: 0x000E05F3
		// (set) Token: 0x06003053 RID: 12371 RVA: 0x000E21FB File Offset: 0x000E05FB
		public DailyDealsDataService DailyDeals { get; private set; }

		// Token: 0x1700079A RID: 1946
		// (get) Token: 0x06003054 RID: 12372 RVA: 0x000E2204 File Offset: 0x000E0604
		// (set) Token: 0x06003055 RID: 12373 RVA: 0x000E220C File Offset: 0x000E060C
		public DailyGiftsDataService DailyGifts { get; private set; }

		// Token: 0x1700079B RID: 1947
		// (get) Token: 0x06003056 RID: 12374 RVA: 0x000E2215 File Offset: 0x000E0615
		// (set) Token: 0x06003057 RID: 12375 RVA: 0x000E221D File Offset: 0x000E061D
		public SeasonalDataService SeasonalData { get; private set; }

		// Token: 0x1700079C RID: 1948
		// (get) Token: 0x06003058 RID: 12376 RVA: 0x000E2226 File Offset: 0x000E0626
		// (set) Token: 0x06003059 RID: 12377 RVA: 0x000E222E File Offset: 0x000E062E
		public AdViewDataService AdViews { get; private set; }

		// Token: 0x1700079D RID: 1949
		// (get) Token: 0x0600305A RID: 12378 RVA: 0x000E2237 File Offset: 0x000E0637
		// (set) Token: 0x0600305B RID: 12379 RVA: 0x000E223F File Offset: 0x000E063F
		public LevelOfDayDataService LevelOfDayData { get; private set; }

		// Token: 0x1700079E RID: 1950
		// (get) Token: 0x0600305C RID: 12380 RVA: 0x000E2248 File Offset: 0x000E0648
		// (set) Token: 0x0600305D RID: 12381 RVA: 0x000E2250 File Offset: 0x000E0650
		public PromoPopupDataService PromoPopupData { get; private set; }

		// Token: 0x1700079F RID: 1951
		// (get) Token: 0x0600305E RID: 12382 RVA: 0x000E2259 File Offset: 0x000E0659
		// (set) Token: 0x0600305F RID: 12383 RVA: 0x000E2261 File Offset: 0x000E0661
		public SaleDataService Sale { get; private set; }

		// Token: 0x06003060 RID: 12384 RVA: 0x000E226C File Offset: 0x000E066C
		public void SaveVillageRank()
		{
			if (this.state != null && this.sbsService.IsAuthenticated)
			{
				GameStateService.EventProgress dftLevel = new GameStateService.EventProgress(this.state.diveForTreasureData.id, this.state.diveForTreasureData.level);
				GameStateService.EventProgress pbLevel = new GameStateService.EventProgress(this.state.pirateBreakoutData.id, this.state.pirateBreakoutData.level);
				GameStateService.villageRankBucketData villageRankBucketData = new GameStateService.villageRankBucketData
				{
					Harmony = this.Resources.GetAmount("harmony"),
					Level = this.Progression.UnlockedLevel,
					language = this.localizationService.Language,
					LodStreak = this.state.levelOfDayData.currentDay,
					DftLevel = dftLevel,
					PbLevel = pbLevel
				};
				SBS.KeyValueStore.WriteJsonToBucket("rankstate", JsonUtility.ToJson(villageRankBucketData), 0, new SbsMergeHandler(this.RankMergeHandler), null).Catch(delegate(Exception ex)
				{
					WoogaDebug.Log(new object[]
					{
						ex,
						"failed writing to rank bucket"
					});
				}).Start();
			}
		}

		// Token: 0x06003061 RID: 12385 RVA: 0x000E23A0 File Offset: 0x000E07A0
		private IEnumerator WaitForNextSave()
		{
			yield return new WaitForSeconds(30f);
			yield return null;
			this.Save(false);
			yield break;
		}

		// eli key point 保存游戏数据, 这是对外调用的接口
		public void Save(bool forceSave = false)
		{
			UnityEngine.Debug.Log(forceSave?"强制保存存档":"保存存档(只是触发，不一定真的保存)");
			if (this.state != null && !this.savingBlocked && this.state.isSaveable)
			{
				if (forceSave || Time.time > this.lastSyncTime + 30f)
				{
					if (this.queuedSave != null)
					{
						WooroutineRunner.Stop(this.queuedSave);
						this.queuedSave = null;
					}
					WooroutineRunner.StartCoroutine(this.SaveRoutine(), null);
				}
				else if (this.queuedSave == null)
				{
					this.queuedSave = WooroutineRunner.StartCoroutine(this.WaitForNextSave(), null);
				}
			}
		}

		public void SaveSync()
		{
			state.SaveSync();
		}

		// Token: 0x06003063 RID: 12387 RVA: 0x000E2458 File Offset: 0x000E0858
		protected IEnumerator SaveRoutine()
		{
			this.state.MigrateOldDeviceIdToCurrent();
			string lastSave = this.state.LastSaveHash(GameStateService.DeviceId);
			this.state.AddSaveHash(GameStateService.DeviceId);
			// eli key point 保存游戏数据到本地
			this.state.Save();
			// eli key point 保存游戏数据到服务器
			yield return WooroutineRunner.StartCoroutine(this.WriteJsonToBucket(), null);
			// 合并存档？？
			if (this.mergeInfo != null)
			{
				this.mergeInfo.serverState.MigrateOldDeviceIdToCurrent();
				if (this.mergeInfo.serverState.isOverride)
				{
					this.TakeServerState(this.mergeInfo.serverState);
				}
				else if (GameStateService.DeviceId == this.mergeInfo.serverState.lastDeviceId)
				{
					yield return WooroutineRunner.StartCoroutine(this.KeepLocalStateRoutine(), null);
				}
				else if (lastSave == this.mergeInfo.serverState.LastSaveHash(GameStateService.DeviceId))
				{
					this.TakeServerState(this.mergeInfo.serverState);
				}
				else
				{
					yield return this.UserSelectedGameState(this.mergeInfo.localState, this.mergeInfo.serverState);
				}
				this.mergeInfo = null;
			}
			yield break;
		}

		// Token: 0x06003064 RID: 12388 RVA: 0x000E2473 File Offset: 0x000E0873
		private Wooroutine<GameState> UserSelectedGameState(GameState localState, GameState serverState)
		{
			return WooroutineRunner.StartWooroutine<GameState>(this.ShowGamestateSelector(localState, serverState));
		}

		// Token: 0x06003065 RID: 12389 RVA: 0x000E2484 File Offset: 0x000E0884
		private IEnumerator ShowGamestateSelector(GameState localState, GameState serverState)
		{
			bool previousSaveable = this.savingBlocked;
			this.savingBlocked = false;
			GameState result = localState;
			yield return SceneManager.Instance.Await<TownMainRoot>(true);
			Wooroutine<GameStateSelectorRoot> scene = SceneManager.Instance.LoadSceneWithParams<GameStateSelectorRoot, MergeInfo>(new MergeInfo(localState, serverState), null);
			yield return scene;
			yield return scene.ReturnValue.onCompleted;
			bool useServerState = scene.ReturnValue.onCompleted.Dispatched;
			result = ((!useServerState) ? localState : serverState);
			result.isOverride = true;
			this.savingBlocked = previousSaveable;
			if (useServerState)
			{
				this.TakeServerState(serverState);
			}
			else
			{
				yield return WooroutineRunner.StartCoroutine(this.KeepLocalStateRoutine(), null);
			}
			yield return result;
			yield break;
		}

		// Token: 0x06003066 RID: 12390 RVA: 0x000E24AD File Offset: 0x000E08AD
		private void TakeServerState(GameState serverState)
		{
			this.state = serverState;
			this.state.isOverride = true;
			this.state.Save();
			PTReloader.ReloadGame("Taking server's player state", false);
		}

		// Token: 0x06003067 RID: 12391 RVA: 0x000E24D8 File Offset: 0x000E08D8
		private IEnumerator KeepLocalStateRoutine()
		{
			this.state.isOverride = true;
			this.state.AddSaveHash(GameStateService.DeviceId);
			yield return WooroutineRunner.StartCoroutine(this.WriteJsonToBucket(), null);
			yield break;
		}

		// Token: 0x06003068 RID: 12392 RVA: 0x000E24F4 File Offset: 0x000E08F4
		private IEnumerator WriteJsonToBucket()
		{
			if (!this.sbsService.IsAuthenticated)
			{
				yield break;
			}
			this.lastSyncTime = Time.time;
			bool previousIsOverride = this.state.isOverride;
			this.state.isOverride = false;
			string stateToWrite = JsonUtility.ToJson(this.state);
			this.state.isOverride = previousIsOverride;
			yield return SBS.KeyValueStore.WriteJsonToBucket("gamestate", stateToWrite, 5, new SbsMergeHandler(this.MergeHandler), null).Catch(delegate(Exception ex)
			{
				WoogaDebug.Log(new object[]
				{
					ex
				});
			}).Finally(delegate()
			{
				this.state.isOverride = false;
			});
			yield break;
		}

		// Token: 0x06003069 RID: 12393 RVA: 0x000E250F File Offset: 0x000E090F
		public bool IsEmptyState()
		{
			return this.Buildings.BuildingsData.Buildings.Count == 0;
		}

		// Token: 0x170007A0 RID: 1952
		// (get) Token: 0x0600306A RID: 12394 RVA: 0x000E2529 File Offset: 0x000E0929
		// (set) Token: 0x0600306B RID: 12395 RVA: 0x000E2536 File Offset: 0x000E0936
		public string PlayerName
		{
			get
			{
				return this.state.playerName;
			}
			set
			{
				this.state.playerName = value;
				this.AddGlobalReplaceKey(new GlobalReplaceKey("userName", value));
			}
		}

		// Token: 0x170007A1 RID: 1953
		// (get) Token: 0x0600306C RID: 12396 RVA: 0x000E2555 File Offset: 0x000E0955
		// (set) Token: 0x0600306D RID: 12397 RVA: 0x000E2562 File Offset: 0x000E0962
		public bool KeepGameLanguageEn
		{
			get
			{
				return this.state.keepGameLanguageEn;
			}
			set
			{
				this.state.keepGameLanguageEn = value;
			}
		}

		// Token: 0x170007A2 RID: 1954
		// (get) Token: 0x0600306E RID: 12398 RVA: 0x000E2570 File Offset: 0x000E0970
		public float TotalUserSpendUSD
		{
			get
			{
				float num = 0f;
				foreach (Transaction transaction in this.state.transactionData)
				{
					num += (float)transaction.priceUsd / 100f;
				}
				return num;
			}
		}

		// Token: 0x0600306F RID: 12399 RVA: 0x000E25E4 File Offset: 0x000E09E4
		public void SetSeenFlag(string flagName)
		{
			UnityEngine.Debug.LogWarning("SetSeenFlag: " + flagName);
			SeenFlag seenFlag2 = this.state.seenFlags.Find((SeenFlag seenFlag) => seenFlag.flagName == flagName);
			if (seenFlag2 != null)
			{
				seenFlag2.viewCount++;
			}
			else
			{
				seenFlag2 = new SeenFlag();
				seenFlag2.flagName = flagName;
				seenFlag2.viewCount = 1;
				seenFlag2.TimeStamp = DateTime.MinValue;
				this.state.seenFlags.Add(seenFlag2);
			}
		}

		// Token: 0x06003070 RID: 12400 RVA: 0x000E266C File Offset: 0x000E0A6C
		public void SetSeenFlagWithTimestamp(string flagName, DateTime date)
		{
			this.SetSeenFlag(flagName);
			SeenFlag seenFlag = this.FindSeenFlag(flagName);
			seenFlag.TimeStamp = date;
		}

		// Token: 0x06003071 RID: 12401 RVA: 0x000E2690 File Offset: 0x000E0A90
		public bool IsSeenFlagTimestampSet(string flagName)
		{
			SeenFlag seenFlag = this.FindSeenFlag(flagName);
			return seenFlag != null && seenFlag.TimeStamp != DateTime.MinValue;
		}

		// Token: 0x06003072 RID: 12402 RVA: 0x000E26C0 File Offset: 0x000E0AC0
		public DateTime GetTimeStamp(string flagName)
		{
			SeenFlag seenFlag = this.FindSeenFlag(flagName);
			return seenFlag.TimeStamp;
		}

		// Token: 0x06003073 RID: 12403 RVA: 0x000E26DC File Offset: 0x000E0ADC
		private SeenFlag FindSeenFlag(string flagName)
		{
			return this.state.seenFlags.Find((SeenFlag seenFlag) => seenFlag.flagName == flagName);
		}

		// Token: 0x06003074 RID: 12404 RVA: 0x000E2712 File Offset: 0x000E0B12
		public bool GetSeenFlag(string flagName)
		{
			return this.FindSeenFlag(flagName) != null;
		}

		// Token: 0x06003075 RID: 12405 RVA: 0x000E2724 File Offset: 0x000E0B24
		public int GetSeenFlagCount(string flagName)
		{
			SeenFlag seenFlag = this.FindSeenFlag(flagName);
			if (seenFlag != null)
			{
				return seenFlag.viewCount;
			}
			return 0;
		}

		// Token: 0x06003076 RID: 12406 RVA: 0x000E2748 File Offset: 0x000E0B48
		public void RemoveSeenFlag(string flagName)
		{
			this.state.seenFlags.RemoveAll((SeenFlag flag) => flag.flagName == flagName);
		}

		// Token: 0x06003077 RID: 12407 RVA: 0x000E277F File Offset: 0x000E0B7F
		public void ResetAllSeenFlags()
		{
			this.state.seenFlags = new List<SeenFlag>();
		}

		// Token: 0x06003078 RID: 12408 RVA: 0x000E2794 File Offset: 0x000E0B94
		public void AddGlobalReplaceKey(GlobalReplaceKey newKey)
		{
			bool flag = false;
			foreach (GlobalReplaceKey globalReplaceKey in this.state.replaceKeys)
			{
				if (globalReplaceKey.key == newKey.key)
				{
					globalReplaceKey.value = newKey.value;
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				this.state.replaceKeys.Add(newKey);
			}
			this.Save(false);
		}

		// Token: 0x06003079 RID: 12409 RVA: 0x000E2838 File Offset: 0x000E0C38
		public void ForceGameState(string json)
		{
			if (GameEnvironment.IsProduction)
			{
				return;
			}
			this.state = GameState.FromJson(json);
			this.Save(true);
		}

		// Token: 0x170007A3 RID: 1955
		// (get) Token: 0x0600307A RID: 12410 RVA: 0x000E2858 File Offset: 0x000E0C58
		public List<GlobalReplaceKey> GlobalReplaceKeys
		{
			get
			{
				return this.state.replaceKeys;
			}
		}

		// Token: 0x0600307B RID: 12411 RVA: 0x000E2868 File Offset: 0x000E0C68
		public IEnumerator DoReloadData<T>(string bucketName, T localState, Action<T> setNewState) where T : class, new()
		{
			// eli key point 连接后并认证才会生成wdkcache.3.db(不知道是做什么的)
			// 没有通过认证则直接使用本地存档
			if (!this.sbsService.IsAuthenticated)
			{
				setNewState(localState);
				yield break;
			}
			Wooroutine<SbsReadResult> readOp = WooroutineRunner.StartWooroutine<SbsReadResult>(SBS.KeyValueStore.ReadFromBucket(bucketName, null, this.sbsService.SbsConfig.sbs_timeouts.kvsRead));
			yield return readOp;
			T newState = (T)((object)null);
			try
			{
				SbsReadResult returnValue = readOp.ReturnValue;
				if (returnValue.Data != null && returnValue.Data.FormatVersion == 5)
				{
					newState = JsonUtility.FromJson<T>(returnValue.Data.Data);
				}
			}
			catch (Exception ex)
			{
				WoogaDebug.LogWarning(new object[]
				{
					"GameStateService",
					ex.Message
				});
			}
			T obj;
			if ((obj = newState) == null)
			{
				obj = localState;
			}
			setNewState(obj);
			this.hasInitialLoad = true;
			yield break;
		}

		// eli key point: reloadGameData
		public IEnumerator doReloadGameData()
		{
			GameState localState = APlayerPrefsObject<GameState>.Load();
			// Wooroutine<GameState> gameState = WooroutineRunner.StartWooroutine<GameState>(APlayerPrefsObject<GameState>.LoadFromServer());
			// yield return gameState;
			// GameState localState = gameState.ReturnValue;
			yield return this.DoReloadData<GameState>("gamestate", localState, delegate(GameState newState)
			{
				this.state = GameStateService.Max(newState, localState, false);
			});
			if (StartRoot.isFirstSession && this.state.installVersion.IsNullOrEmpty())
			{
				// this.state.installVersion = BuildVersion.ShortVersion;
				this.state.installVersion = BuildVersion.Version;
			}
			// 刚载入游戏就保存。。。
			this.Save(true);
		}

		// Token: 0x0600307D RID: 12413 RVA: 0x000E28B4 File Offset: 0x000E0CB4
		public IEnumerator ReloadGameDataWithSelect()
		{
			GameState serverState = new GameState();
			yield return this.DoReloadData<GameState>("gamestate", this.state, delegate(GameState newState)
			{
				serverState = newState;
			});
			yield return this.UserSelectedGameState(this.state, serverState);
			yield break;
		}

		// Token: 0x0600307E RID: 12414 RVA: 0x000E28D0 File Offset: 0x000E0CD0
		private IEnumerator LoadRoutine()
		{
			yield return ServiceLocator.Instance.Inject(this);
			yield return this.doReloadGameData();
			
			// 初始化名字
			this.AddGlobalReplaceKey(new GlobalReplaceKey("userName", localizationService.GetText("name.dialog.box.default.text")));
			
			this.CreateDataServices();
			this.SaveVillageRank();
			base.OnInitialized.Dispatch();
		}

		// Token: 0x0600307F RID: 12415 RVA: 0x000E28EC File Offset: 0x000E0CEC
		public static GameState Max(GameState a, GameState b, bool ignoreOverride = false)
		{
			if (!ignoreOverride)
			{
				if (a.isOverride)
				{
					return a;
				}
				if (b.isOverride)
				{
					return b;
				}
			}
			if (a.progression.tiers.Count != b.progression.tiers.Count)
			{
				return (a.progression.tiers.Count < b.progression.tiers.Count) ? b : a;
			}
			int num = a.progression.tiers.Sum();
			int num2 = b.progression.tiers.Sum();
			if (num != num2)
			{
				return (num <= num2) ? b : a;
			}
			ResourceDataService resourceDataService = new ResourceDataService(() => a);
			ResourceDataService resourceDataService2 = new ResourceDataService(() => b);
			int amount = resourceDataService.GetAmount("harmony");
			int amount2 = resourceDataService2.GetAmount("harmony");
			if (amount != amount2)
			{
				return (amount <= amount2) ? b : a;
			}
			DailyGiftsData dailyGiftsData = a.dailyGiftData ?? new DailyGiftsData();
			DailyGiftsData dailyGiftsData2 = b.dailyGiftData ?? new DailyGiftsData();
			if (dailyGiftsData.lastReceived != dailyGiftsData2.lastReceived)
			{
				return (dailyGiftsData.lastReceived <= dailyGiftsData2.lastReceived) ? b : a;
			}
			LevelOfDayModel levelOfDayModel = a.levelOfDayData ?? new LevelOfDayModel();
			LevelOfDayModel other = b.levelOfDayData ?? new LevelOfDayModel();
			int num3 = levelOfDayModel.CompareProgress(other);
			if (num3 != 0)
			{
				return (num3 <= 0) ? b : a;
			}
			int num4 = a.diveForTreasureData.CompareProgress(b.diveForTreasureData);
			if (num4 != 0)
			{
				return (num4 <= 0) ? b : a;
			}
			int num5 = a.pirateBreakoutData.CompareProgress(b.pirateBreakoutData);
			if (num5 != 0)
			{
				return (num5 <= 0) ? b : a;
			}
			int amount3 = resourceDataService.GetAmount("diamonds");
			int amount4 = resourceDataService2.GetAmount("diamonds");
			if (amount3 != amount4)
			{
				return (amount3 <= amount4) ? b : a;
			}
			int amount5 = resourceDataService.GetAmount("coins");
			int amount6 = resourceDataService2.GetAmount("coins");
			if (amount5 != amount6)
			{
				return (amount5 <= amount6) ? b : a;
			}
			return (a.timestamp < b.timestamp) ? b : a;
		}

		// Token: 0x06003080 RID: 12416 RVA: 0x000E2C50 File Offset: 0x000E1050
		private void CreateDataServices()
		{
			this.Resources = new ResourceDataService(new Func<GameState>(this.GetState));
			this.Buildings = new BuildingDataService(new Func<GameState>(this.GetState), this.configService);
			this.Progression = new ProgressionDataService(new Func<GameState>(this.GetState));
			this.Lives = new LivesDataService(new Func<GameState>(this.GetState));
			this.Quests = new QuestsDataService(new Func<GameState>(this.GetState));
			this.Tournaments = new TournamentDataService(new Func<GameState>(this.GetState));
			this.Challenges = new ChallengeDataService(new Func<GameState>(this.GetState));
			this.Bank = new BankDataService(new Func<GameState>(this.GetState), this.configService);
			this.DiveForTreasure = new DiveForTreasureDataService(new Func<GameState>(this.GetState));
			this.PirateBreakout = new PirateBreakOutDataService(new Func<GameState>(this.GetState));
			this.DailyDeals = new DailyDealsDataService(new Func<GameState>(this.GetState));
			this.Facebook = new FacebookDataService(delegate()
			{
				this.Save(true);
			}, new Func<GameState>(this.GetState));
			this.Debug = new DebugDataService(new Func<GameState>(this.GetState));
			this.Transactions = new TransactionDataService(new Func<GameState>(this.GetState));
			this.DailyGifts = new DailyGiftsDataService(new Func<GameState>(this.GetState));
			this.SeasonalData = new SeasonalDataService(new Func<GameState>(this.GetState));
			this.AdViews = new AdViewDataService(new Func<GameState>(this.GetState));
			this.LevelOfDayData = new LevelOfDayDataService(new Func<GameState>(this.GetState));
			this.PromoPopupData = new PromoPopupDataService(new Func<GameState>(this.GetState));
			this.Sale = new SaleDataService(new Func<GameState>(this.GetState));
			// 购买后会保存
			this.Resources.onNeedsSave.AddListener(delegate
			{
				this.Save(false);
			});
		}

		// Token: 0x06003081 RID: 12417 RVA: 0x000E2E60 File Offset: 0x000E1260
		private ISbsData MergeHandler(ISbsData myData, ISbsData serverData)
		{
			WoogaDebug.LogWarning(new object[]
			{
				"MergeHandler"
			});
			GameState gameState = JsonUtility.FromJson<GameState>(myData.Data);
			GameState gameState2 = JsonUtility.FromJson<GameState>(serverData.Data);
			if (this.state.isOverride)
			{
				gameState.isOverride = false;
				myData.Data = JsonUtility.ToJson(gameState);
				this.state.isOverride = false;
				return myData;
			}
			this.mergeInfo = new MergeInfo(gameState, gameState2);
			GameState gameState3 = GameStateService.Max(gameState2, gameState, false);
			if (gameState3 == gameState2)
			{
				WoogaDebug.Log(new object[]
				{
					"server state wins!"
				});
			}
			else
			{
				WoogaDebug.Log(new object[]
				{
					"local state wins!"
				});
			}
			return (gameState3 != gameState) ? serverData : myData;
		}

		// Token: 0x06003082 RID: 12418 RVA: 0x000E2F1F File Offset: 0x000E131F
		private ISbsData RankMergeHandler(ISbsData myData, ISbsData serverData)
		{
			return myData;
		}

		// Token: 0x06003083 RID: 12419 RVA: 0x000E2F22 File Offset: 0x000E1322
		public void Reset()
		{
			this.state = new GameState();
			this.Save(true);
		}

		// Token: 0x06003084 RID: 12420 RVA: 0x000E2F36 File Offset: 0x000E1336
		public override void OnSuspend()
		{
			this.UnlimitedLivesStart = DateTime.Now;
			this.Save(true);
		}

		// Token: 0x06003085 RID: 12421 RVA: 0x000E2F4A File Offset: 0x000E134A
		public override void OnResume()
		{
			if (DateTime.Now < this.UnlimitedLivesStart)
			{
				this.UnlimitedLivesEnd = DateTime.Now;
			}
			this.UnlimitedLivesStart = DateTime.Now;
			if (this.hasInitialLoad)
			{
				this.Save(true);
			}
		}

		// Token: 0x170007A4 RID: 1956
		// (get) Token: 0x06003086 RID: 12422 RVA: 0x000E2F89 File Offset: 0x000E1389
		// (set) Token: 0x06003087 RID: 12423 RVA: 0x000E2F96 File Offset: 0x000E1396
		public bool isInteractable
		{
			get
			{
				return this.state.isSaveable;
			}
			set
			{
				this.state.isSaveable = value;
			}
		}

		// Token: 0x06003088 RID: 12424 RVA: 0x000E2FA4 File Offset: 0x000E13A4
		public AWeeklyEventDataService GetDataServiceForPlayMode(LevelPlayMode playMode)
		{
			if (playMode == LevelPlayMode.DiveForTreasure)
			{
				return this.DiveForTreasure;
			}
			if (playMode != LevelPlayMode.PirateBreakout)
			{
				return null;
			}
			return this.PirateBreakout;
		}

		// Token: 0x0400593E RID: 22846
		[WaitForService(true, true)]
		private SBSService sbsService;

		// Token: 0x0400593F RID: 22847
		[WaitForService(false, true)]
		private ILocalizationService localizationService;

		// Token: 0x04005940 RID: 22848
		[WaitForService(true, true)]
		private ConfigService configService;

		// Token: 0x04005941 RID: 22849
		public const float SYNC_INTERVAL = 30f;

		// Token: 0x04005942 RID: 22850
		public const string BUCKET_NAME = "gamestate";

		// Token: 0x04005943 RID: 22851
		private const int FORMAT_VERSION = 5;

		// Token: 0x04005944 RID: 22852
		public const string RANK_BUCKET_NAME = "rankstate";

		// Token: 0x04005945 RID: 22853
		private const int RANK_FORMAT_VERSION = 0;

		// Token: 0x04005946 RID: 22854
		private MergeInfo mergeInfo;

		// Token: 0x04005947 RID: 22855
		private float lastSyncTime = float.MinValue;

		// Token: 0x04005948 RID: 22856
		private GameState state;

		// Token: 0x04005949 RID: 22857
		private Coroutine queuedSave;

		// Token: 0x0400594A RID: 22858
		public bool savingBlocked;

		// Token: 0x0400594B RID: 22859
		private bool hasInitialLoad;

		// Token: 0x020007B2 RID: 1970
		[Serializable]
		public class GameStateData
		{
			// Token: 0x04005961 RID: 22881
			public int format_version;

			// Token: 0x04005962 RID: 22882
			public GameState data;
		}

		// Token: 0x020007B3 RID: 1971
		[Serializable]
		public struct villageRankBucketData
		{
			// Token: 0x04005963 RID: 22883
			public int Harmony;

			// Token: 0x04005964 RID: 22884
			public int Level;

			// Token: 0x04005965 RID: 22885
			public int LodStreak;

			// Token: 0x04005966 RID: 22886
			public GameStateService.EventProgress DftLevel;

			// Token: 0x04005967 RID: 22887
			public GameStateService.EventProgress PbLevel;

			// Token: 0x04005968 RID: 22888
			public string sbsId;

			// Token: 0x04005969 RID: 22889
			public WoogaSystemLanguage language;
		}

		// Token: 0x020007B4 RID: 1972
		[Serializable]
		public struct EventProgress
		{
			// Token: 0x0600308D RID: 12429 RVA: 0x000E2FFB File Offset: 0x000E13FB
			public EventProgress(string eventId, int progress)
			{
				this.eventId = eventId;
				this.progress = progress;
			}

			// Token: 0x0400596A RID: 22890
			public string eventId;

			// Token: 0x0400596B RID: 22891
			public int progress;
		}
	}
}
