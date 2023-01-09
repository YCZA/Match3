using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Match3.Scripts1.Wooga.Core.ThreadSafe;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.Services.Tracking.Internal;
using Match3.Scripts1.Wooga.Services.Tracking.LifeCycle;
using Match3.Scripts1.Wooga.Services.Tracking.Parameters;
using Match3.Scripts1.Wooga.Services.Tracking.Sender;
using UnityEngine;

namespace Match3.Scripts1.Wooga.Services.Tracking
{
	// Token: 0x0200046B RID: 1131
	public class TrackingClient
	{
		// Token: 0x060020D0 RID: 8400 RVA: 0x0008A7E4 File Offset: 0x00088BE4
		public TrackingClient(string hostName, ITrackingSender sender, string persistentDataDirectory, TrackingConfiguration configuration, Tracker tracker = null, MonoBehaviour coroutinesHost = null)
		{
			this.InitPersistentDataDirectory(persistentDataDirectory);
			this._coroutinesHost = ((!(coroutinesHost == null)) ? coroutinesHost : CoroutineRunner.Instance);
			this._sender = sender;
			this._configuration = configuration;
			this._tracker = (tracker ?? new Tracker(this._sender, hostName, this._configuration));
			this._configuration.parameterProviders.Add(this.baseParameters);
			this.InitSessionTracker();
			this.InitLifeCycle();
		}

		// Token: 0x17000510 RID: 1296
		// (get) Token: 0x060020D1 RID: 8401 RVA: 0x0008A86F File Offset: 0x00088C6F
		// (set) Token: 0x060020D2 RID: 8402 RVA: 0x0008A877 File Offset: 0x00088C77
		public string persistentDataPath { get; private set; }

		// Token: 0x17000511 RID: 1297
		// (get) Token: 0x060020D3 RID: 8403 RVA: 0x0008A880 File Offset: 0x00088C80
		public RoundTracker.RoundTracker roundTracker
		{
			get
			{
				if (TrackingClient._roundTracker == null)
				{
					TrackingClient._roundTracker = new RoundTracker.RoundTracker(this._tracker, this._coroutinesHost);
				}
				return TrackingClient._roundTracker;
			}
		}

		// Token: 0x17000512 RID: 1298
		// (get) Token: 0x060020D4 RID: 8404 RVA: 0x0008A8A7 File Offset: 0x00088CA7
		public BaseParameters baseParameters
		{
			get
			{
				if (this._baseParameters == null)
				{
					this._baseParameters = new BaseParameters();
				}
				return this._baseParameters;
			}
		}

		// Token: 0x17000513 RID: 1299
		// (get) Token: 0x060020D5 RID: 8405 RVA: 0x0008A8C5 File Offset: 0x00088CC5
		public SessionTracker.SessionTracker sessionTracker
		{
			get
			{
				if (this._sessionTracker == null)
				{
					this._sessionTracker = new SessionTracker.SessionTracker(this._tracker, this._configuration, this._coroutinesHost);
				}
				return this._sessionTracker;
			}
		}

		// Token: 0x060020D6 RID: 8406 RVA: 0x0008A8F5 File Offset: 0x00088CF5
		private void InitPersistentDataDirectory(string dataDir)
		{
			this.persistentDataPath = Path.Combine(Unity3D.Paths.ApplicationDataPath ?? string.Empty, dataDir);
			if (!Directory.Exists(this.persistentDataPath))
			{
				Directory.CreateDirectory(this.persistentDataPath);
			}
		}

		// Token: 0x060020D7 RID: 8407 RVA: 0x0008A930 File Offset: 0x00088D30
		private void InitSessionTracker()
		{
			this.sessionTracker.Init(this.persistentDataPath);
			this._configuration.parameterProviders.Add(this.sessionTracker);
			TrackingLifeCycleDispatcher.AddReceiver(this.sessionTracker);
		}

		// Token: 0x060020D8 RID: 8408 RVA: 0x0008A964 File Offset: 0x00088D64
		private void InitLifeCycle()
		{
			TrackingLifeCycleDispatcher.AddReceiver(new PeriodicSender(TimeSpan.FromMinutes(4.0), this._sender, this._coroutinesHost));
		}

		// Token: 0x060020D9 RID: 8409 RVA: 0x0008A98A File Offset: 0x00088D8A
		public IEnumerator Track(string callName, Dictionary<string, object> data)
		{
			return this._tracker.Track(callName, data);
		}

		// Token: 0x060020DA RID: 8410 RVA: 0x0008A999 File Offset: 0x00088D99
		public IEnumerator TrackOnce(string callName, Dictionary<string, object> data)
		{
			return this._tracker.TrackOnce(callName, data);
		}

		// Token: 0x060020DB RID: 8411 RVA: 0x0008A9A8 File Offset: 0x00088DA8
		internal void MarkAsSent(string callId)
		{
			this._tracker.MarkAsSent(callId);
		}

		// Token: 0x04004BA1 RID: 19361
		private static RoundTracker.RoundTracker _roundTracker;

		// Token: 0x04004BA2 RID: 19362
		private BaseParameters _baseParameters;

		// Token: 0x04004BA3 RID: 19363
		private SessionTracker.SessionTracker _sessionTracker;

		// Token: 0x04004BA4 RID: 19364
		private readonly Tracker _tracker;

		// Token: 0x04004BA5 RID: 19365
		private readonly ITrackingSender _sender;

		// Token: 0x04004BA6 RID: 19366
		private readonly TrackingConfiguration _configuration;

		// Token: 0x04004BA7 RID: 19367
		private readonly MonoBehaviour _coroutinesHost;
	}
}
