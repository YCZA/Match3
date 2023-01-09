using System.Threading;

namespace Match3.Scripts1.Helpshift
{
	// Token: 0x020001CF RID: 463
	public class APICallInfo
	{
		// Token: 0x06000D4F RID: 3407 RVA: 0x0001FCDB File Offset: 0x0001E0DB
		public APICallInfo(string instanceIdentifier, string methodIdentifier, string apiName, object[] args)
		{
			this.instanceIdentifier = instanceIdentifier;
			this.methodIdentifier = methodIdentifier;
			this.apiName = apiName;
			this.args = args;
		}

		// Token: 0x06000D50 RID: 3408 RVA: 0x0001FD00 File Offset: 0x0001E100
		public APICallInfo(ManualResetEvent resetEvent)
		{
			this.resetEvent = resetEvent;
		}

		// Token: 0x04003F7B RID: 16251
		public string instanceIdentifier;

		// Token: 0x04003F7C RID: 16252
		public string methodIdentifier;

		// Token: 0x04003F7D RID: 16253
		public string apiName;

		// Token: 0x04003F7E RID: 16254
		public object[] args;

		// Token: 0x04003F7F RID: 16255
		public ManualResetEvent resetEvent;
	}
}
