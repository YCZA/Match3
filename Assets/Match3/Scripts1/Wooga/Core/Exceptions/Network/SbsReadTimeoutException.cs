using System;

namespace Match3.Scripts1.Wooga.Core.Exceptions.Network
{
	// Token: 0x02000356 RID: 854
	public class SbsReadTimeoutException : SbsNetworkingRequestFailedException
	{
		// Token: 0x060019F9 RID: 6649 RVA: 0x00074EEA File Offset: 0x000732EA
		public SbsReadTimeoutException(string errorMessage) : base(errorMessage)
		{
		}

		// Token: 0x060019FA RID: 6650 RVA: 0x00074EF3 File Offset: 0x000732F3
		public SbsReadTimeoutException(Exception ex) : base(ex)
		{
		}
	}
}
