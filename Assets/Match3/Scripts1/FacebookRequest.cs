using System;
using System.Collections.Generic;
using System.Linq;
using Wooga.Foundation.Json;

// Token: 0x02000775 RID: 1909
namespace Match3.Scripts1
{
	[Serializable]
	public class FacebookRequest : PendingFacebookOperation
	{
		// Token: 0x06002F3F RID: 12095 RVA: 0x000DCC7C File Offset: 0x000DB07C
		public static FacebookRequest Create(IEnumerable<string> recipientIDs, string title, string message, Dictionary<string, object> data, string trackingType, string context1, string context2)
		{
			FacebookRequest facebookRequest = new FacebookRequest();
			try
			{
				facebookRequest.Op = PendingFacebookOperation.OpType.Request;
				facebookRequest.recipients = recipientIDs.ToList<string>();
				facebookRequest.title = title;
				facebookRequest.message = message;
				facebookRequest.data = JSON.Serialize(data, false, 1, ' ');
				facebookRequest.originalData = data;
				facebookRequest.trackingType = trackingType;
				facebookRequest.context1 = context1;
				facebookRequest.context2 = context2;
			}
			catch (Exception ex)
			{
				WoogaDebug.LogWarning(new object[]
				{
					"Failed to create request: " + ex.Message
				});
				facebookRequest = null;
			}
			return facebookRequest;
		}

		// Token: 0x04005857 RID: 22615
		public string message;

		// Token: 0x04005858 RID: 22616
		public List<string> recipients;

		// Token: 0x04005859 RID: 22617
		public string data;

		// Token: 0x0400585A RID: 22618
		public string title;

		// Token: 0x0400585B RID: 22619
		public Dictionary<string, object> originalData;

		// Token: 0x0400585C RID: 22620
		public string context1 = string.Empty;

		// Token: 0x0400585D RID: 22621
		public string context2 = string.Empty;

		// Token: 0x0400585E RID: 22622
		public string trackingType = string.Empty;
	}
}
