using System;

namespace Match3.Scripts1.Wooga.Services.TrackingCore.Networking
{
	// Token: 0x02000472 RID: 1138
	public class NetworkingRequestFailedException : Exception
	{
		// Token: 0x060020EF RID: 8431 RVA: 0x0008ACBA File Offset: 0x000890BA
		public NetworkingRequestFailedException(string errorMessage) : base(errorMessage)
		{
		}

		// Token: 0x060020F0 RID: 8432 RVA: 0x0008ACC3 File Offset: 0x000890C3
		public NetworkingRequestFailedException(Exception ex) : this(ex.Message)
		{
			this.OriginalException = ex;
		}

		// Token: 0x17000514 RID: 1300
		// (get) Token: 0x060020F1 RID: 8433 RVA: 0x0008ACD8 File Offset: 0x000890D8
		// (set) Token: 0x060020F2 RID: 8434 RVA: 0x0008ACE0 File Offset: 0x000890E0
		public Exception OriginalException { get; private set; }
	}
}
