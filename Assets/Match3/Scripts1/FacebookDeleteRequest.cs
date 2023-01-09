using System;

// Token: 0x02000776 RID: 1910
namespace Match3.Scripts1
{
	[Serializable]
	public class FacebookDeleteRequest : PendingFacebookOperation
	{
		// Token: 0x06002F41 RID: 12097 RVA: 0x000DCD24 File Offset: 0x000DB124
		public static FacebookDeleteRequest Create(string requestID)
		{
			return new FacebookDeleteRequest
			{
				Op = PendingFacebookOperation.OpType.DeleteLifeRequest,
				requestID = requestID
			};
		}

		// Token: 0x0400585F RID: 22623
		public string requestID;
	}
}
