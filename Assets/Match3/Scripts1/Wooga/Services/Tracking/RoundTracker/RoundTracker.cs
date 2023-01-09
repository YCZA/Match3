using System;
using System.Collections.Generic;
using System.IO;
using Wooga.Core.Utilities;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.Services.Tracking.Internal;
using Match3.Scripts1.Wooga.Services.Tracking.LifeCycle;
using Match3.Scripts1.Wooga.Services.Tracking.Tools;
using UnityEngine;

namespace Match3.Scripts1.Wooga.Services.Tracking.RoundTracker
{
	// Token: 0x0200044F RID: 1103
	public class RoundTracker : ILifeCycleReceiver
	{
		// Token: 0x06002004 RID: 8196 RVA: 0x000861A0 File Offset: 0x000845A0
		public RoundTracker(Tracker tracker, MonoBehaviour coroutinesHost)
		{
			this._tracker = tracker;
			this._coroutinesHost = coroutinesHost;
		}

		// Token: 0x170004F7 RID: 1271
		// (get) Token: 0x06002005 RID: 8197 RVA: 0x000861B6 File Offset: 0x000845B6
		// (set) Token: 0x06002006 RID: 8198 RVA: 0x000861BE File Offset: 0x000845BE
		public PersistentData<RoundPersistentData> PersistentRoundData { get; private set; }

		// Token: 0x06002007 RID: 8199 RVA: 0x000861C7 File Offset: 0x000845C7
		public virtual void StartLifecycle(string path)
		{
			this.LoadPersistentData(path);
			TrackingLifeCycleDispatcher.AddReceiver(this);
		}

		// Token: 0x06002008 RID: 8200 RVA: 0x000861D8 File Offset: 0x000845D8
		public Future TrackStartOfRound(int level, string levelName, Dictionary<string, object> data = null)
		{
			if (data == null)
			{
				data = new Dictionary<string, object>();
			}
			this.StartRound(level, levelName);
			this.AddParameters(data);
			this._roundWasStarted = true;
			return this._coroutinesHost.StartTask(this._tracker.Track("sor", data));
		}

		// Token: 0x06002009 RID: 8201 RVA: 0x00086224 File Offset: 0x00084624
		public Future TrackEndOfRound(int level, string levelName, int score, RoundTracker.RoundOutcome outcome, Dictionary<string, object> data = null)
		{
			if (level != this.PersistentRoundData.data.level || levelName != this.PersistentRoundData.data.levelName)
			{
				Log.Error(new object[]
				{
					string.Concat(new object[]
					{
						"Can't end round, round was not started yet for ",
						level,
						", ",
						levelName
					})
				});
				return null;
			}
			if (data == null)
			{
				data = new Dictionary<string, object>();
			}
			this.EndRound(score, outcome, data);
			this.AddParameters(data);
			return this._coroutinesHost.StartTask(this._tracker.Track("eor", data));
		}

		// Token: 0x0600200A RID: 8202 RVA: 0x000862D8 File Offset: 0x000846D8
		public void Awake()
		{
		}

		// Token: 0x0600200B RID: 8203 RVA: 0x000862DA File Offset: 0x000846DA
		public void Start()
		{
		}

		// Token: 0x0600200C RID: 8204 RVA: 0x000862DC File Offset: 0x000846DC
		public void Update(float deltaTime)
		{
		}

		// Token: 0x0600200D RID: 8205 RVA: 0x000862DE File Offset: 0x000846DE
		public void OnApplicationPause(bool paused)
		{
			if (!this._roundWasStarted)
			{
				return;
			}
			if (paused)
			{
				this.UpdateDuration();
				this.PersistentRoundData.Write();
			}
			else
			{
				this.UpdateRoundSessionStartTime();
			}
		}

		// Token: 0x0600200E RID: 8206 RVA: 0x0008630E File Offset: 0x0008470E
		public IRoundData GetRoundData()
		{
			return this.PersistentRoundData.data;
		}

		// Token: 0x0600200F RID: 8207 RVA: 0x0008631C File Offset: 0x0008471C
		protected virtual void LoadPersistentData(string path)
		{
			string path2 = Path.Combine(path, "roundTracker");
			this.PersistentRoundData = PersistentData.Create<RoundPersistentData>(path2);
			this.PersistentRoundData.Load();
		}

		// Token: 0x06002010 RID: 8208 RVA: 0x0008634C File Offset: 0x0008474C
		protected virtual void StartRound(int level, string levelName)
		{
			this.PersistentRoundData.data.level = level;
			this.PersistentRoundData.data.levelName = levelName;
			this.PersistentRoundData.data.roundId = Guid.NewGuid().ToString("N");
			this.PersistentRoundData.data.playedRoundsInTotal++;
			this.PersistentRoundData.data.lastRoundSessionStart = global::Wooga.Core.Utilities.Time.UtcNow();
			this.PersistentRoundData.data.roundDuration = 0.0;
			LevelData levelData = this.GetLevelData(levelName);
			levelData.roundTries++;
			this.PersistentRoundData.Write();
		}

		// Token: 0x06002011 RID: 8209 RVA: 0x00086404 File Offset: 0x00084804
		protected virtual void EndRound(int score, RoundTracker.RoundOutcome outcome, Dictionary<string, object> data)
		{
			if (outcome != RoundTracker.RoundOutcome.RoundLost)
			{
				if (outcome == RoundTracker.RoundOutcome.RoundWon)
				{
					this.PersistentRoundData.data.wonRoundsInTotal++;
				}
			}
			else
			{
				this.PersistentRoundData.data.lostRoundsInTotal++;
			}
			this.UpdateDuration();
			data["duration"] = (int)this.PersistentRoundData.data.roundDuration;
			data["score"] = score;
			data["outcome"] = (int)outcome;
			this.PersistentRoundData.Write();
		}

		// Token: 0x06002012 RID: 8210 RVA: 0x000864B4 File Offset: 0x000848B4
		public virtual void AddParameters(Dictionary<string, object> data)
		{
			data["round_id"] = this.PersistentRoundData.data.roundId;
			data["rlvl"] = this.PersistentRoundData.data.level;
			data["rlvln"] = this.PersistentRoundData.data.levelName;
			data["t_fails"] = this.PersistentRoundData.data.lostRoundsInTotal;
			data["tw_rounds"] = this.PersistentRoundData.data.wonRoundsInTotal;
			data["t_rounds"] = this.PersistentRoundData.data.playedRoundsInTotal;
			LevelData levelData = this.GetLevelData(this.PersistentRoundData.data.levelName);
			data["rlvl_rounds"] = levelData.roundTries;
		}

		// Token: 0x06002013 RID: 8211 RVA: 0x000865A4 File Offset: 0x000849A4
		protected virtual LevelData GetLevelData(string levelName)
		{
			if (!this.PersistentRoundData.data.levelData.ContainsKey(levelName))
			{
				this.PersistentRoundData.data.levelData[levelName] = new LevelData();
			}
			return this.PersistentRoundData.data.levelData[levelName];
		}

		// Token: 0x06002014 RID: 8212 RVA: 0x00086600 File Offset: 0x00084A00
		protected virtual void UpdateDuration()
		{
			DateTime lastRoundSessionStart = this.PersistentRoundData.data.lastRoundSessionStart;
			this.PersistentRoundData.data.roundDuration += global::Wooga.Core.Utilities.Time.UtcNow().Subtract(lastRoundSessionStart).TotalSeconds;
		}

		// Token: 0x06002015 RID: 8213 RVA: 0x0008664B File Offset: 0x00084A4B
		protected virtual void UpdateRoundSessionStartTime()
		{
			this.PersistentRoundData.data.lastRoundSessionStart = global::Wooga.Core.Utilities.Time.UtcNow();
		}

		// Token: 0x04004B5A RID: 19290
		public const string PERSISTENT_DATA_FILENAME = "roundTracker";

		// Token: 0x04004B5B RID: 19291
		private readonly Tracker _tracker;

		// Token: 0x04004B5C RID: 19292
		private bool _roundWasStarted;

		// Token: 0x04004B5D RID: 19293
		private MonoBehaviour _coroutinesHost;

		// Token: 0x02000450 RID: 1104
		public enum RoundOutcome
		{
			// Token: 0x04004B60 RID: 19296
			RoundLost,
			// Token: 0x04004B61 RID: 19297
			RoundWon,
			// Token: 0x04004B62 RID: 19298
			RoundQuitOrCanceled
		}
	}
}
