using System;
using System.Net;

namespace Match3.Scripts1.Wooga.Core.Exceptions.Network
{
	// Token: 0x02000357 RID: 855
	public class SbsServerException : SbsNetworkingRequestFailedException
	{
		// Token: 0x060019FB RID: 6651 RVA: 0x00074EFC File Offset: 0x000732FC
		public SbsServerException(HttpStatusCode statusCode) : this(string.Format("Server returned status code: {0}", statusCode))
		{
			this.StatusCode = statusCode;
		}

		// Token: 0x060019FC RID: 6652 RVA: 0x00074F1B File Offset: 0x0007331B
		public SbsServerException(string errorMessage) : base(errorMessage)
		{
		}

		// Token: 0x060019FD RID: 6653 RVA: 0x00074F24 File Offset: 0x00073324
		public SbsServerException(Exception ex) : base(ex)
		{
		}

		// Token: 0x17000416 RID: 1046
		// (get) Token: 0x060019FE RID: 6654 RVA: 0x00074F2D File Offset: 0x0007332D
		// (set) Token: 0x060019FF RID: 6655 RVA: 0x00074F35 File Offset: 0x00073335
		public HttpStatusCode StatusCode { get; private set; }
	}
}
