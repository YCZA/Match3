using System;

namespace Match3.Scripts1.Wooga.Core.Exceptions.Network
{
	// Token: 0x02000358 RID: 856
	public class SbsUnknownNetworkException : SbsNetworkingRequestFailedException
	{
		// Token: 0x06001A00 RID: 6656 RVA: 0x00074F3E File Offset: 0x0007333E
		public SbsUnknownNetworkException(string errorMessage) : base(errorMessage)
		{
		}

		// Token: 0x06001A01 RID: 6657 RVA: 0x00074F47 File Offset: 0x00073347
		public SbsUnknownNetworkException(Exception ex) : base(ex)
		{
		}
	}
}
