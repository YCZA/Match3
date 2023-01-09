using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1
{
	// Token: 0x02000605 RID: 1541
	public struct ChameleonMove : IMatchResult
	{
		// Token: 0x0600277A RID: 10106 RVA: 0x000AF764 File Offset: 0x000ADB64
		public ChameleonMove(Move move)
		{
			this.move = move;
		}

		// Token: 0x17000668 RID: 1640
		// (get) Token: 0x0600277B RID: 10107 RVA: 0x000AF76D File Offset: 0x000ADB6D
		public Move Move
		{
			get
			{
				return this.move;
			}
		}

		// Token: 0x040051F1 RID: 20977
		private readonly Move move;
	}
}
