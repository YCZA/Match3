using System;
using System.Text;
using Wooga.Newtonsoft.Json;
using Wooga.Foundation.Json;

namespace Match3.Scripts1
{
	// Token: 0x020003FA RID: 1018
	public struct Report
	{
		// Token: 0x06001E65 RID: 7781 RVA: 0x00080FB7 File Offset: 0x0007F3B7
		[JsonConstructor]
		private Report(string endpoint, string payload)
		{
			this.Payload = payload;
			this.Endpoint = endpoint;
		}

		// Token: 0x06001E66 RID: 7782 RVA: 0x00080FC7 File Offset: 0x0007F3C7
		public static Report Create(Uri endpoint, string payload)
		{
			return new Report(endpoint.ToString(), payload);
		}

		// Token: 0x06001E67 RID: 7783 RVA: 0x00080FD5 File Offset: 0x0007F3D5
		public static Report Create(string data)
		{
			return JSON.Deserialize<Report>(data);
		}

		// Token: 0x06001E68 RID: 7784 RVA: 0x00080FDD File Offset: 0x0007F3DD
		public byte[] PayloadBytes()
		{
			return Report.Encoding.GetBytes(this.Payload);
		}

		// Token: 0x06001E69 RID: 7785 RVA: 0x00080FEF File Offset: 0x0007F3EF
		public string Serialize()
		{
			return JSON.Serialize(this, false, 1, ' ');
		}

		// Token: 0x04004A46 RID: 19014
		private static readonly Encoding Encoding = Encoding.UTF8;

		// Token: 0x04004A47 RID: 19015
		public readonly string Endpoint;

		// Token: 0x04004A48 RID: 19016
		public readonly string Payload;
	}
}
