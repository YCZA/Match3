using System.Collections;
using System.Collections.Generic;
using Wooga.Coroutines;

namespace Match3.Scripts1.Wooga.Services.TrackingCore.Networking
{
	// Token: 0x0200046E RID: 1134
	public class InMemoryStringProducer : IProducerStrategy
	{
		// Token: 0x060020E0 RID: 8416 RVA: 0x0008AADA File Offset: 0x00088EDA
		public InMemoryStringProducer()
		{
			this.PersistedData = new List<InMemoryStringRecord>();
		}

		// Token: 0x060020E1 RID: 8417 RVA: 0x0008AAED File Offset: 0x00088EED
		public IEnumerator Produce(string input)
		{
			this.PersistedData.Add(InMemoryStringRecord.Create(input));
			return global::Wooga.Coroutines.Coroutines.Empty();
		}

		// Token: 0x060020E2 RID: 8418 RVA: 0x0008AB08 File Offset: 0x00088F08
		public IEnumerator Consume(IConsumer consumer)
		{
			InMemoryStringRecord record = this.FirstNotConsumed();
			if (record == null)
			{
				return global::Wooga.Coroutines.Coroutines.Empty();
			}
			record.Consumed = true;
			return consumer.Consume(record.Data).ContinueWith(delegate(bool result)
			{
				if (result)
				{
					this.PersistedData.Remove(record);
				}
				else
				{
					record.Consumed = false;
				}
			});
		}

		// Token: 0x060020E3 RID: 8419 RVA: 0x0008AB70 File Offset: 0x00088F70
		public int AvailableCount()
		{
			int num = 0;
			for (int i = 0; i < this.PersistedData.Count; i++)
			{
				if (!this.PersistedData[i].Consumed)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x060020E4 RID: 8420 RVA: 0x0008ABB6 File Offset: 0x00088FB6
		public void Close()
		{
		}

		// Token: 0x060020E5 RID: 8421 RVA: 0x0008ABB8 File Offset: 0x00088FB8
		private InMemoryStringRecord FirstNotConsumed()
		{
			for (int i = 0; i < this.PersistedData.Count; i++)
			{
				InMemoryStringRecord inMemoryStringRecord = this.PersistedData[i];
				if (!inMemoryStringRecord.Consumed)
				{
					return inMemoryStringRecord;
				}
			}
			return null;
		}

		// Token: 0x060020E6 RID: 8422 RVA: 0x0008ABFC File Offset: 0x00088FFC
		public IEnumerator Map(IConsumer consumer)
		{
			IEnumerator enumerator = global::Wooga.Coroutines.Coroutines.Empty();
			int num = this.AvailableCount();
			for (int i = 0; i < num; i++)
			{
				enumerator = enumerator.ContinueWith(() => this.Consume(consumer));
			}
			return enumerator;
		}

		// Token: 0x04004BA9 RID: 19369
		private readonly List<InMemoryStringRecord> PersistedData;
	}
}
