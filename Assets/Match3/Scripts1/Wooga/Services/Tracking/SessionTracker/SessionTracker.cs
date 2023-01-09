using System;
using System.Collections.Generic;
using System.IO;
using System.Timers;
using Wooga.Core.Utilities;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.Services.Tracking.Calls;
using Match3.Scripts1.Wooga.Services.Tracking.Internal;
using Match3.Scripts1.Wooga.Services.Tracking.LifeCycle;
using Match3.Scripts1.Wooga.Services.Tracking.Parameters;
using Match3.Scripts1.Wooga.Services.Tracking.Tools;
using UnityEngine;

namespace Match3.Scripts1.Wooga.Services.Tracking.SessionTracker
{
	// Token: 0x02000457 RID: 1111
	public class SessionTracker : ILifeCycleReceiver, IParameterProvider
	{
		// Token: 0x06002027 RID: 8231 RVA: 0x000868D0 File Offset: 0x00084CD0
		public SessionTracker(Tracker tracker, TrackingConfiguration configuration, MonoBehaviour coroutinesHost)
		{
			this._tracker = tracker;
			this._configuration = configuration;
			this._coroutinesHost = coroutinesHost;
		}

		// Token: 0x06002028 RID: 8232 RVA: 0x00086927 File Offset: 0x00084D27
		public void Init(string path)
		{
			this._persistentDataPath = Path.Combine(path, "sessionTracker");
			this.LoadPersistentData();
			this._state = SessionTracker.StateId.Initialized;
		}

		// Token: 0x06002029 RID: 8233 RVA: 0x00086948 File Offset: 0x00084D48
		public void AddParametersTo(Dictionary<string, object> data)
		{
			data["sid"] = this._sessionId;
			SessionTracker.SessionTrackerPersistentData persistentDataData = this.getPersistentDataData();
			if (persistentDataData != null)
			{
				int num = (int)global::Wooga.Core.Utilities.Time.UtcNow().Subtract(this._sessionStart).TotalSeconds;
				data["pt"] = this._persistentData.data.totalPlaytimeSeconds + num;
				if (!data.ContainsKey("sc"))
				{
					data["sc"] = this._persistentData.data.sessionCount;
				}
			}
		}

		// Token: 0x0600202A RID: 8234 RVA: 0x000869E4 File Offset: 0x00084DE4
		public ISessionTrackerData GetSessionTrackerData()
		{
			int num = (int)global::Wooga.Core.Utilities.Time.UtcNow().Subtract(this._sessionStart).TotalSeconds;
			SessionTracker.SessionTrackerPersistentData persistentDataData = this.getPersistentDataData();
			if (persistentDataData != null)
			{
				return new SessionTracker.SessionTrackerPersistentData
				{
					sessionCount = persistentDataData.sessionCount,
					totalPlaytimeSeconds = persistentDataData.totalPlaytimeSeconds + num
				};
			}
			return null;
		}

		// Token: 0x0600202B RID: 8235 RVA: 0x00086A44 File Offset: 0x00084E44
		public void Awake()
		{
			this._coroutinesHost.StartTask(this._tracker.Track("awake", new Dictionary<string, object>
			{
				{
					"awake_id",
					this._awakeId
				}
			}));
		}

		// Token: 0x0600202C RID: 8236 RVA: 0x00086A85 File Offset: 0x00084E85
		public void Start()
		{
			this._startupDelayTimer = new Timer(2000.0);
			this._startupDelayTimer.Elapsed += delegate(object sender, ElapsedEventArgs e)
			{
				this._shouldStart = true;
				this._startupDelayTimer.Stop();
				this._startupDelayTimer.Dispose();
				this._startupDelayTimer = null;
			};
			this._startupDelayTimer.Start();
		}

		// Token: 0x0600202D RID: 8237 RVA: 0x00086AC0 File Offset: 0x00084EC0
		public void Update(float deltaTime)
		{
			if (this._sessionStartData != null)
			{
				double num = (double)global::Wooga.Core.Utilities.Time.EpochTime();
				if (num > (double)this._nextTrySendSessionStart)
				{
					this.TrySendStartSession(this._sessionStartData);
				}
			}
			if (this._shouldStart)
			{
				this.StartNewSession();
				this._shouldStart = false;
			}
		}

		// Token: 0x0600202E RID: 8238 RVA: 0x00086B14 File Offset: 0x00084F14
		public void OnApplicationPause(bool paused)
		{
			if (this._state != SessionTracker.StateId.Running && this._state != SessionTracker.StateId.Ended)
			{
				return;
			}
			if (paused)
			{
				this.EndSession();
			}
			else if ((int)(global::Wooga.Core.Utilities.Time.UtcNow() - this._lastSessionEndUtc).TotalSeconds > 60)
			{
				this.StartSession();
			}
			else
			{
				this._state = SessionTracker.StateId.Running;
			}
		}

		// Token: 0x0600202F RID: 8239 RVA: 0x00086B7D File Offset: 0x00084F7D
		protected virtual void LoadPersistentData()
		{
			this._persistentData = PersistentData.Create<SessionTracker.SessionTrackerPersistentData>(this._persistentDataPath);
			this._persistentData.Load();
		}

		// Token: 0x06002030 RID: 8240 RVA: 0x00086B9B File Offset: 0x00084F9B
		private SessionTracker.SessionTrackerPersistentData getPersistentDataData()
		{
			if (this._persistentData == null || this._persistentData.data == null)
			{
				this.LoadPersistentData();
			}
			return (this._persistentData != null) ? this._persistentData.data : null;
		}

		// Token: 0x06002031 RID: 8241 RVA: 0x00086BDC File Offset: 0x00084FDC
		protected virtual void HandleTrackAppInstall()
		{
			DeviceInformation.Track();
			if (!this._tracker.HasAlreadyTrackedOnce("apa"))
			{
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				dictionary["ref"] = this.GetInstallReferrer();
				this._coroutinesHost.StartTask(this._tracker.TrackOnce("apa", dictionary));
			}
		}

		// Token: 0x06002032 RID: 8242 RVA: 0x00086C38 File Offset: 0x00085038
		private string GetInstallReferrer()
		{
			// AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.wooga.sdk.InstallReferrerReceiver");
			// return androidJavaClass.GetStatic<string>("referrerString");
			return "";
		}

		// Token: 0x06002033 RID: 8243 RVA: 0x00086C5B File Offset: 0x0008505B
		private void StartNewSession()
		{
			this.HandleTrackAppInstall();
			this.StartSession();
		}

		// Token: 0x06002034 RID: 8244 RVA: 0x00086C6C File Offset: 0x0008506C
		protected virtual void StartSession()
		{
			Log.Info(new object[]
			{
				"SESSION START"
			});
			this._state = SessionTracker.StateId.Starting;
			Dictionary<string, object> data = new Dictionary<string, object>
			{
				{
					"awake_id",
					this._awakeId
				}
			};
			this._sessionStart = global::Wooga.Core.Utilities.Time.UtcNow();
			this._sessionId = Guid.NewGuid().ToString("N");
			SessionTracker.SessionTrackerPersistentData persistentDataData = this.getPersistentDataData();
			if (persistentDataData != null)
			{
				persistentDataData.sessionCount++;
			}
			this.TrySendStartSession(data);
		}

		// Token: 0x06002035 RID: 8245 RVA: 0x00086CF4 File Offset: 0x000850F4
		private bool TrySendStartSession(Dictionary<string, object> data)
		{
			bool flag = true;
			if (this._configuration.onSessionStart != null)
			{
				flag = this._configuration.onSessionStart(data);
			}
			Log.Debug(new object[]
			{
				"should send session start? " + flag
			});
			if (flag)
			{
				this._coroutinesHost.StartTask(this._tracker.Track("pgr", data));
				this._nextTrySendSessionStart = float.MaxValue;
				this._lastSessionEndUtc = DateTime.UtcNow;
				this._sessionStartData = null;
				this._state = SessionTracker.StateId.Running;
			}
			else
			{
				this._sessionStartData = data;
				this._nextTrySendSessionStart = (float)global::Wooga.Core.Utilities.Time.EpochTime() + 0.5f;
			}
			return flag;
		}

		// Token: 0x06002036 RID: 8246 RVA: 0x00086DAC File Offset: 0x000851AC
		protected virtual void EndSession()
		{
			Log.Info(new object[]
			{
				"SESSION END"
			});
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			if (this._configuration.onSessionEnd != null)
			{
				this._configuration.onSessionEnd(dictionary);
			}
			this._coroutinesHost.StartTask(this._tracker.Track("send", dictionary));
			SessionTracker.SessionTrackerPersistentData persistentDataData = this.getPersistentDataData();
			if (persistentDataData != null)
			{
				int num = (int)global::Wooga.Core.Utilities.Time.UtcNow().Subtract(this._sessionStart).TotalSeconds;
				persistentDataData.totalPlaytimeSeconds += num;
				this._persistentData.Write();
			}
			this._lastSessionEndUtc = global::Wooga.Core.Utilities.Time.UtcNow();
			this._state = SessionTracker.StateId.Ended;
		}

		// Token: 0x04004B6B RID: 19307
		public const string PERSISTENT_DATA_FILENAME = "sessionTracker";

		// Token: 0x04004B6C RID: 19308
		public const float START_SESSION_CALL_RETRY_DELAY = 0.5f;

		// Token: 0x04004B6D RID: 19309
		public const int SESSION_MAX_BREAK_DURATATION_SECONDS = 60;

		// Token: 0x04004B6E RID: 19310
		private DateTime _sessionStart;

		// Token: 0x04004B6F RID: 19311
		private DateTime _lastSessionEndUtc = DateTime.MinValue;

		// Token: 0x04004B70 RID: 19312
		private string _sessionId;

		// Token: 0x04004B71 RID: 19313
		private string _awakeId = Guid.NewGuid().ToString();

		// Token: 0x04004B72 RID: 19314
		private Dictionary<string, object> _sessionStartData;

		// Token: 0x04004B73 RID: 19315
		protected Tracker _tracker;

		// Token: 0x04004B74 RID: 19316
		private readonly TrackingConfiguration _configuration;

		// Token: 0x04004B75 RID: 19317
		protected PersistentData<SessionTracker.SessionTrackerPersistentData> _persistentData;

		// Token: 0x04004B76 RID: 19318
		private float _nextTrySendSessionStart = float.MaxValue;

		// Token: 0x04004B77 RID: 19319
		private SessionTracker.StateId _state;

		// Token: 0x04004B78 RID: 19320
		private string _persistentDataPath;

		// Token: 0x04004B79 RID: 19321
		private MonoBehaviour _coroutinesHost;

		// Token: 0x04004B7A RID: 19322
		public const int START_DELAY = 2000;

		// Token: 0x04004B7B RID: 19323
		private Timer _startupDelayTimer;

		// Token: 0x04004B7C RID: 19324
		private bool _shouldStart;

		// Token: 0x02000458 RID: 1112
		// (Invoke) Token: 0x06002039 RID: 8249
		public delegate bool SessionStartCallback(Dictionary<string, object> data);

		// Token: 0x02000459 RID: 1113
		public enum StateId
		{
			// Token: 0x04004B7E RID: 19326
			None,
			// Token: 0x04004B7F RID: 19327
			Initialized,
			// Token: 0x04004B80 RID: 19328
			Starting,
			// Token: 0x04004B81 RID: 19329
			Running,
			// Token: 0x04004B82 RID: 19330
			Ended
		}

		// Token: 0x0200045A RID: 1114
		protected class SessionTrackerPersistentData : ISessionTrackerData
		{
			// Token: 0x170004FA RID: 1274
			// (get) Token: 0x0600203D RID: 8253 RVA: 0x00086E94 File Offset: 0x00085294
			// (set) Token: 0x0600203E RID: 8254 RVA: 0x00086E9C File Offset: 0x0008529C
			public int sessionCount { get; set; }

			// Token: 0x170004FB RID: 1275
			// (get) Token: 0x0600203F RID: 8255 RVA: 0x00086EA5 File Offset: 0x000852A5
			// (set) Token: 0x06002040 RID: 8256 RVA: 0x00086EAD File Offset: 0x000852AD
			public int totalPlaytimeSeconds { get; set; }
		}
	}
}
