using System.Collections.Generic;

namespace Match3.Scripts1.Wooga.Core.Network
{
	// Token: 0x02000363 RID: 867
	public interface ISbsNetworking
	{
		// Token: 0x06001A17 RID: 6679
		IEnumerator<SbsResponse> Send(SbsRequest sbsRequest);
	}
}
