using System.Collections.Generic;

namespace Match3.Scripts1.Wooga.Services.TrackingCore.Networking
{
	// Token: 0x0200046D RID: 1133
	public interface IConsumer
	{
		// Token: 0x060020DF RID: 8415
		IEnumerator<bool> Consume(string product);
	}
}
