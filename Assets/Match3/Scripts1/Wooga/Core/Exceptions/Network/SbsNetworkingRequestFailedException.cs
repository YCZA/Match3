using System;

namespace Match3.Scripts1.Wooga.Core.Exceptions.Network
{
	// Token: 0x02000355 RID: 853
	public class SbsNetworkingRequestFailedException : Exception
	{
		// Token: 0x060019F5 RID: 6645 RVA: 0x00074EA9 File Offset: 0x000732A9
		public SbsNetworkingRequestFailedException(string errorMessage) : base(errorMessage)
		{
		}

		// Token: 0x060019F6 RID: 6646 RVA: 0x00074EB2 File Offset: 0x000732B2
		public SbsNetworkingRequestFailedException(Exception ex) : this(ex.Message)
		{
			this.OriginalException = ex;
		}

		// Token: 0x17000415 RID: 1045
		// (get) Token: 0x060019F7 RID: 6647 RVA: 0x00074EC7 File Offset: 0x000732C7
		// (set) Token: 0x060019F8 RID: 6648 RVA: 0x00074ECF File Offset: 0x000732CF
		public Exception OriginalException { get; private set; }
	}
}
