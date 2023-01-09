using System;

namespace Match3.Scripts1.Wooga.Core.Exceptions.Network
{
	// Token: 0x02000354 RID: 852
	public class SbsConnectTimeoutException : SbsNetworkingRequestFailedException
	{
		// Token: 0x060019F3 RID: 6643 RVA: 0x00074ED8 File Offset: 0x000732D8
		public SbsConnectTimeoutException(string errorMessage) : base(errorMessage)
		{
		}

		// Token: 0x060019F4 RID: 6644 RVA: 0x00074EE1 File Offset: 0x000732E1
		public SbsConnectTimeoutException(Exception ex) : base(ex)
		{
		}
	}
}
