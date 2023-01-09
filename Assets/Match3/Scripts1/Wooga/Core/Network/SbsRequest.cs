using System;
using System.Collections.Generic;
using Wooga.Core.Utilities;
using Match3.Scripts1.Wooga.Services.Authentication;

namespace Match3.Scripts1.Wooga.Core.Network
{
	// Token: 0x02000365 RID: 869
	public class SbsRequest
	{
		// Token: 0x06001A22 RID: 6690 RVA: 0x00075CBF File Offset: 0x000740BF
		public SbsRequest()
		{
			this.TimeoutInSeconds = 60;
			this.Protocol = "https";
			// this.Host = "api.sbs.wooga.com";
			this.Host = "apboao23333";
			this.Headers = new Dictionary<string, string>();
			this.Timestamp = Time.Now();
		}

		// Token: 0x17000417 RID: 1047
		// (get) Token: 0x06001A23 RID: 6691 RVA: 0x00075CFB File Offset: 0x000740FB
		// (set) Token: 0x06001A24 RID: 6692 RVA: 0x00075D03 File Offset: 0x00074103
		public int Retries { get; set; }

		// Token: 0x17000418 RID: 1048
		// (get) Token: 0x06001A25 RID: 6693 RVA: 0x00075D0C File Offset: 0x0007410C
		// (set) Token: 0x06001A26 RID: 6694 RVA: 0x00075D14 File Offset: 0x00074114
		public int TimeoutInSeconds { get; set; }

		// Token: 0x17000419 RID: 1049
		// (get) Token: 0x06001A27 RID: 6695 RVA: 0x00075D1D File Offset: 0x0007411D
		// (set) Token: 0x06001A28 RID: 6696 RVA: 0x00075D25 File Offset: 0x00074125
		public string Protocol { get; set; }

		// Token: 0x1700041A RID: 1050
		// (get) Token: 0x06001A29 RID: 6697 RVA: 0x00075D2E File Offset: 0x0007412E
		// (set) Token: 0x06001A2A RID: 6698 RVA: 0x00075D36 File Offset: 0x00074136
		public string Host { get; set; }

		// Token: 0x1700041B RID: 1051
		// (get) Token: 0x06001A2B RID: 6699 RVA: 0x00075D3F File Offset: 0x0007413F
		// (set) Token: 0x06001A2C RID: 6700 RVA: 0x00075D47 File Offset: 0x00074147
		public string Path { get; set; }

		// Token: 0x1700041C RID: 1052
		// (get) Token: 0x06001A2D RID: 6701 RVA: 0x00075D50 File Offset: 0x00074150
		// (set) Token: 0x06001A2E RID: 6702 RVA: 0x00075D58 File Offset: 0x00074158
		public string ETag { get; set; }

		// Token: 0x1700041D RID: 1053
		// (get) Token: 0x06001A2F RID: 6703 RVA: 0x00075D61 File Offset: 0x00074161
		// (set) Token: 0x06001A30 RID: 6704 RVA: 0x00075D69 File Offset: 0x00074169
		public bool UseSignature { get; set; }

		// Token: 0x1700041E RID: 1054
		// (get) Token: 0x06001A31 RID: 6705 RVA: 0x00075D72 File Offset: 0x00074172
		// (set) Token: 0x06001A32 RID: 6706 RVA: 0x00075D7A File Offset: 0x0007417A
		public string Signature { get; set; }

		// Token: 0x1700041F RID: 1055
		// (get) Token: 0x06001A33 RID: 6707 RVA: 0x00075D83 File Offset: 0x00074183
		// (set) Token: 0x06001A34 RID: 6708 RVA: 0x00075D8B File Offset: 0x0007418B
		public bool IsUriEscaped { get; set; }

		// Token: 0x17000420 RID: 1056
		// (get) Token: 0x06001A35 RID: 6709 RVA: 0x00075D94 File Offset: 0x00074194
		// (set) Token: 0x06001A36 RID: 6710 RVA: 0x00075D9C File Offset: 0x0007419C
		public HttpMethod Method { get; set; }

		// Token: 0x17000421 RID: 1057
		// (get) Token: 0x06001A37 RID: 6711 RVA: 0x00075DA5 File Offset: 0x000741A5
		// (set) Token: 0x06001A38 RID: 6712 RVA: 0x00075DAD File Offset: 0x000741AD
		public Dictionary<string, string> Headers { get; set; }

		// Token: 0x17000422 RID: 1058
		// (get) Token: 0x06001A39 RID: 6713 RVA: 0x00075DB6 File Offset: 0x000741B6
		// (set) Token: 0x06001A3A RID: 6714 RVA: 0x00075DBE File Offset: 0x000741BE
		public string Body { get; set; }

		// Token: 0x17000423 RID: 1059
		// (get) Token: 0x06001A3B RID: 6715 RVA: 0x00075DC7 File Offset: 0x000741C7
		// (set) Token: 0x06001A3C RID: 6716 RVA: 0x00075DCF File Offset: 0x000741CF
		public DateTime Timestamp { get; set; }

		// Token: 0x17000424 RID: 1060
		// (get) Token: 0x06001A3D RID: 6717 RVA: 0x00075DD8 File Offset: 0x000741D8
		// (set) Token: 0x06001A3E RID: 6718 RVA: 0x00075DE0 File Offset: 0x000741E0
		public bool IsForceReload { get; set; }

		// Token: 0x17000425 RID: 1061
		// (get) Token: 0x06001A3F RID: 6719 RVA: 0x00075DE9 File Offset: 0x000741E9
		// (set) Token: 0x06001A40 RID: 6720 RVA: 0x00075DF1 File Offset: 0x000741F1
		public bool SendAsBytes { get; set; }

		// Token: 0x17000426 RID: 1062
		// (get) Token: 0x06001A41 RID: 6721 RVA: 0x00075DFA File Offset: 0x000741FA
		// (set) Token: 0x06001A42 RID: 6722 RVA: 0x00075E02 File Offset: 0x00074202
		public UserContext UserContext { get; set; }

		// Token: 0x04004878 RID: 18552
		public const int REQUEST_TIMEOUT_DEFAULT = 60;

		// Token: 0x04004879 RID: 18553
		// public const string REQUEST_HOST_DEFAULT = "api.sbs.wooga.com";

		// Token: 0x0400487A RID: 18554
		// public const string REQUEST_HOST_IPV4 = "api-fallback.sbs.wooga.com";

		// Token: 0x0400487B RID: 18555
		// public const string REQUEST_PROTOCOL_DEFAULT = "https";
	}
}
