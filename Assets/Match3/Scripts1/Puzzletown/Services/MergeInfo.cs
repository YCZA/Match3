using Match3.Scripts2.PlayerData;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x020007B0 RID: 1968
	public class MergeInfo
	{
		// Token: 0x06003022 RID: 12322 RVA: 0x000E1E68 File Offset: 0x000E0268
		public MergeInfo(GameState localState, GameState serverState)
		{
			this.localState = localState;
			this.serverState = serverState;
		}

		// Token: 0x0400593C RID: 22844
		public GameState localState;

		// Token: 0x0400593D RID: 22845
		public GameState serverState;
	}
}
