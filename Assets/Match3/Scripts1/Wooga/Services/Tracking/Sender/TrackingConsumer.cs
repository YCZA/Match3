using System.Collections.Generic;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.Services.TrackingCore.Networking;

namespace Match3.Scripts1.Wooga.Services.Tracking.Sender
{
	// Token: 0x02000454 RID: 1108
	public class TrackingConsumer : IConsumer
	{
		// Token: 0x06002020 RID: 8224 RVA: 0x00086732 File Offset: 0x00084B32
		public TrackingConsumer()
		{
			this._networking = new TrackingUnityWebRequestReporter();
		}

		// Token: 0x06002021 RID: 8225 RVA: 0x00086745 File Offset: 0x00084B45
		public IEnumerator<bool> Consume(string product)
		{
			return this._networking.Send(product).ContinueWith((long responseCode) => (int)responseCode >= 200);
		}

		// Token: 0x04004B69 RID: 19305
		private readonly TrackingUnityWebRequestReporter _networking;
	}
}
