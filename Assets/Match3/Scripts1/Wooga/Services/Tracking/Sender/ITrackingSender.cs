using System.Collections;

namespace Match3.Scripts1.Wooga.Services.Tracking.Sender
{
	// Token: 0x02000453 RID: 1107
	public interface ITrackingSender
	{
		// Token: 0x0600201E RID: 8222
		IEnumerator Queue(string url);

		// Token: 0x0600201F RID: 8223
		IEnumerator ConsumeAll();
	}
}
