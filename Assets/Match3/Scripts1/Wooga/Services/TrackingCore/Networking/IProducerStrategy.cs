using System.Collections;

namespace Match3.Scripts1.Wooga.Services.TrackingCore.Networking
{
	// Token: 0x02000470 RID: 1136
	public interface IProducerStrategy
	{
		// Token: 0x060020E9 RID: 8425
		IEnumerator Produce(string input);

		// Token: 0x060020EA RID: 8426
		IEnumerator Consume(IConsumer consumer);

		// Token: 0x060020EB RID: 8427
		IEnumerator Map(IConsumer consumer);

		// Token: 0x060020EC RID: 8428
		int AvailableCount();

		// Token: 0x060020ED RID: 8429
		void Close();
	}
}
