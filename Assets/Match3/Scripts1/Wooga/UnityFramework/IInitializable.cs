using Match3.Scripts1.Wooga.Signals;

namespace Wooga.UnityFramework
{
	// Token: 0x02000B69 RID: 2921
	public interface IInitializable
	{
		// Token: 0x170009CD RID: 2509
		// (get) Token: 0x0600444C RID: 17484
		AwaitSignal OnInitialized { get; }

		// Token: 0x0600444D RID: 17485
		void DeInit();
	}
}
