using System.Collections;
using Match3.Scripts1.Wooga.Services.ErrorAnalytics.Payload;
using Match3.Scripts1.Wooga.Services.TrackingCore.Networking;

namespace Match3.Scripts1.Wooga.Services.ErrorAnalytics.Internal
{
	// Token: 0x020003F5 RID: 1013
	public interface IReporter : IConsumer
	{
		// Token: 0x06001E58 RID: 7768
		IEnumerator ReportStart(string payload, Information.Sbs sbsInfo, Information.App appInfo, string customUserId);

		// Token: 0x06001E59 RID: 7769
		IEnumerator ReportError(string payload, Information.Sbs sbsInfo, Information.App appInfo, string customUserId);

		// Token: 0x06001E5A RID: 7770
		IEnumerator DeliverReportsInBackground();
	}
}
