using Wooga.UnityFramework;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x02000738 RID: 1848
	public interface IAdjustService : IService, IInitializable
	{
		// Token: 0x06002DCD RID: 11725
		// void TrackPurchase(Product product);

		// Token: 0x06002DCE RID: 11726
		void TrackFbLogin();

		// Token: 0x06002DCF RID: 11727
		void TrackTutorialFinished();

		// Token: 0x06002DD0 RID: 11728
		void TrackQuestComplete(string questId);

		// Token: 0x06002DD1 RID: 11729
		void TrackLevelComplete(Level level);

		// Token: 0x06002DD2 RID: 11730
		void TrackThirdPurchase();

		// Token: 0x06002DD3 RID: 11731
		void TrackPurchaseScreenOpen();
	}
}
