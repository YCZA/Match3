using System;
using System.Collections.Generic;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x0200078C RID: 1932
	[Serializable]
	public class FBRequestCache
	{
		// Token: 0x0400589B RID: 22683
		public List<FacebookData.Request> Requests = new List<FacebookData.Request>();

		// Token: 0x0400589C RID: 22684
		public Queue<PendingFacebookOperation> PendingFBOps = new Queue<PendingFacebookOperation>();
	}
}
