using System.Collections.Generic;
using System.Net;

namespace Match3.Scripts1.Wooga.Services.TrackingCore.Networking
{
	// Token: 0x02000471 RID: 1137
	public interface ISender
	{
		// Token: 0x060020EE RID: 8430
		IEnumerator<HttpStatusCode> Post(string endpoint, byte[] body, Dictionary<string, string> headers);
	}
}
