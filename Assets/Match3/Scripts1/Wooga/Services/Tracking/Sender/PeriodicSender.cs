using System;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.Services.Tracking.LifeCycle;
using UnityEngine;

namespace Match3.Scripts1.Wooga.Services.Tracking.Sender
{
	// Token: 0x02000451 RID: 1105
	public class PeriodicSender : ILifeCycleReceiver
	{
		// Token: 0x06002016 RID: 8214 RVA: 0x00086662 File Offset: 0x00084A62
		public PeriodicSender(TimeSpan interval, ITrackingSender sender, MonoBehaviour coroutinesHost)
		{
			this._interval = interval;
			this._sender = sender;
			this._lastUpdate = global::Wooga.Core.Utilities.Time.Now();
			this._coroutinesHost = coroutinesHost;
		}

		// Token: 0x06002017 RID: 8215 RVA: 0x0008668C File Offset: 0x00084A8C
		public void Update(float deltaTime)
		{
			DateTime lastUpdate = global::Wooga.Core.Utilities.Time.Now();
			if (lastUpdate.Subtract(this._lastUpdate) > this._interval)
			{
				this._lastUpdate = lastUpdate;
				this._coroutinesHost.StartTask(this._sender.ConsumeAll());
			}
		}

		// Token: 0x06002018 RID: 8216 RVA: 0x000866DA File Offset: 0x00084ADA
		public void Awake()
		{
		}

		// Token: 0x06002019 RID: 8217 RVA: 0x000866DC File Offset: 0x00084ADC
		public void Start()
		{
		}

		// Token: 0x0600201A RID: 8218 RVA: 0x000866DE File Offset: 0x00084ADE
		public void OnApplicationPause(bool paused)
		{
		}

		// Token: 0x04004B63 RID: 19299
		private readonly TimeSpan _interval;

		// Token: 0x04004B64 RID: 19300
		private readonly ITrackingSender _sender;

		// Token: 0x04004B65 RID: 19301
		private readonly MonoBehaviour _coroutinesHost;

		// Token: 0x04004B66 RID: 19302
		private DateTime _lastUpdate;
	}
}
