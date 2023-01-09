using System.Collections;
using System.IO;
using Wooga.Core.Utilities;
using Match3.Scripts1.Wooga.Services.TrackingCore.Networking;

namespace Match3.Scripts1.Wooga.Services.Tracking.RequestQueue
{
	// Token: 0x0200044B RID: 1099
	public class RequestQueue : IProducerStrategy
	{
		// eli key point tracking.requestqueue
		public RequestQueue(string path, IProducerStrategy producer = null)
		{
			if (producer != null)
			{
				this._producer = producer;
			}
			else
			{
				path = Path.Combine(path, PERSISTENT_DATA_FOLDERNAME);
				if (!Directory.Exists(path))
				{
					Directory.CreateDirectory(path);
				}
				string text = Path.Combine(path, DB_FILE);
				PersistentSqliteStringProducer producer2 = new PersistentSqliteStringProducer(text, 1024L);
				IcloudBackup.Exclude(text);
				Log.Warning(new object[]
				{
					"using Sqlite IProducerStrategy (Device)"
				});
				this._producer = new SequentialProducer(producer2);
			}
		}

		// Token: 0x06001FE1 RID: 8161 RVA: 0x00086028 File Offset: 0x00084428
		public IEnumerator Produce(string input)
		{
			if (this._producer.AvailableCount() >= MaxNumQueuedCalls)
			{
				Log.Warning(new object[]
				{
					string.Concat(new object[]
					{
						"[cannot queue ",
						input,
						" because there are already ",
						MaxNumQueuedCalls,
						" calls queued"
					})
				});
				return null;
			}
			return this._producer.Produce(input);
		}

		// Token: 0x06001FE2 RID: 8162 RVA: 0x0008609A File Offset: 0x0008449A
		public IEnumerator Consume(IConsumer consumer)
		{
			return this._producer.Consume(consumer);
		}

		// Token: 0x06001FE3 RID: 8163 RVA: 0x000860A8 File Offset: 0x000844A8
		public IEnumerator Map(IConsumer consumer)
		{
			return this._producer.Map(consumer);
		}

		// Token: 0x06001FE4 RID: 8164 RVA: 0x000860B6 File Offset: 0x000844B6
		public int AvailableCount()
		{
			return this._producer.AvailableCount();
		}

		// Token: 0x06001FE5 RID: 8165 RVA: 0x000860C3 File Offset: 0x000844C3
		public void Close()
		{
			this._producer.Close();
		}

		// Token: 0x04004B4B RID: 19275
		public const string PERSISTENT_DATA_FOLDERNAME = "requestQueueData";

		// Token: 0x04004B4C RID: 19276
		public const string DB_FILE = "trackingRequests.sql";

		// Token: 0x04004B4D RID: 19277
		public const int MaxNumQueuedCalls = 500;

		// Token: 0x04004B4E RID: 19278
		private bool _executing;

		// Token: 0x04004B4F RID: 19279
		private readonly IProducerStrategy _producer;
	}
}
