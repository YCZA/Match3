using System.Collections;
using Match3.Scripts1.Wooga.Services.TrackingCore.Networking;

namespace Match3.Scripts1.Wooga.Services.Tracking.Sender
{
	// Token: 0x02000452 RID: 1106
	public class Sender : ITrackingSender
	{
		// Token: 0x0600201B RID: 8219 RVA: 0x000866E0 File Offset: 0x00084AE0
		public Sender(IProducerStrategy requestQueue, IConsumer consumer)
		{
			this._requestQueue = requestQueue;
			this._consumer = consumer;
		}

		// Token: 0x0600201C RID: 8220 RVA: 0x000866F6 File Offset: 0x00084AF6
		public virtual IEnumerator Queue(string url)
		{
			return this._requestQueue.Produce(url);
		}

		// Token: 0x0600201D RID: 8221 RVA: 0x00086704 File Offset: 0x00084B04
		public IEnumerator ConsumeAll()
		{
			return (this._requestQueue.AvailableCount() <= 0) ? global::Wooga.Coroutines.Coroutines.Empty() : this._requestQueue.Map(this._consumer);
		}

		// Token: 0x04004B67 RID: 19303
		private readonly IProducerStrategy _requestQueue;

		// Token: 0x04004B68 RID: 19304
		private readonly IConsumer _consumer;
	}
}
