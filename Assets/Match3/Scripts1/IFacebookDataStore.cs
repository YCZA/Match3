using System.Collections.Generic;

// Token: 0x02000772 RID: 1906
namespace Match3.Scripts1
{
	public interface IFacebookDataStore
	{
		// Token: 0x06002F38 RID: 12088
		void Save();

		// Token: 0x06002F39 RID: 12089
		List<FacebookData.FacebookRequestLog> GetRequestLog();

		// Token: 0x06002F3A RID: 12090
		void SetRequestLog(List<FacebookData.FacebookRequestLog> newLog);

		// Token: 0x1700075C RID: 1884
		// (get) Token: 0x06002F3B RID: 12091
		List<FacebookData.Request> Requests { get; }

		// Token: 0x1700075D RID: 1885
		// (get) Token: 0x06002F3C RID: 12092
		Queue<PendingFacebookOperation> PendingOps { get; }
	}
}
