using System;
using System.Collections;
using Wooga.Core.Utilities;
using Wooga.Coroutines;

namespace Match3.Scripts1.Wooga.Services.TrackingCore.Networking
{
	// Token: 0x02000478 RID: 1144
	public class SequentialProducer : IProducerStrategy
	{
		// Token: 0x06002110 RID: 8464 RVA: 0x0008B358 File Offset: 0x00089758
		public SequentialProducer(IProducerStrategy producer)
		{
			this._producer = producer;
		}

		// Token: 0x06002111 RID: 8465 RVA: 0x0008B367 File Offset: 0x00089767
		public IEnumerator Produce(string input)
		{
			return this._producer.Produce(input);
		}

		// Token: 0x06002112 RID: 8466 RVA: 0x0008B375 File Offset: 0x00089775
		public IEnumerator Consume(IConsumer consumer)
		{
			return this._producer.Consume(consumer);
		}

		// Token: 0x06002113 RID: 8467 RVA: 0x0008B383 File Offset: 0x00089783
		public int AvailableCount()
		{
			return this._producer.AvailableCount();
		}

		// Token: 0x06002114 RID: 8468 RVA: 0x0008B390 File Offset: 0x00089790
		public void Close()
		{
			this._producer.Close();
		}

		// Token: 0x06002115 RID: 8469 RVA: 0x0008B3A0 File Offset: 0x000897A0
		public IEnumerator Map(IConsumer consumer)
		{
			return this.WaitForCompletion().ContinueWith(() => this.PostQueueOfReports(consumer)).Catch(delegate(Exception exception)
			{
				Log.Error(new object[]
				{
					exception.Message
				});
				this._executing = false;
			});
		}

		// Token: 0x06002116 RID: 8470 RVA: 0x0008B3E9 File Offset: 0x000897E9
		private IEnumerator PostQueueOfReports(IConsumer consumer)
		{
			if (this._producer.AvailableCount() > 0)
			{
				this._executing = true;
				return this._producer.Map(consumer).ContinueWith(() => this._executing = false);
			}

			return global::Wooga.Coroutines.Coroutines.Empty();
		}

		// Token: 0x06002117 RID: 8471 RVA: 0x0008B428 File Offset: 0x00089828
		private IEnumerator WaitForCompletion()
		{
			while (this._executing)
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x04004BB6 RID: 19382
		private readonly IProducerStrategy _producer;

		// Token: 0x04004BB7 RID: 19383
		private bool _executing;
	}
}
