using Match3.Scripts1.Puzzletown.Match3;
using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1
{
	// Token: 0x020005F7 RID: 1527
	public struct TreasureCollected : IScoreCollectedResult, IMatchResult
	{
		// Token: 0x0600273B RID: 10043 RVA: 0x000AE44F File Offset: 0x000AC84F
		public TreasureCollected(IntVector2 pos, Gem gem, IntVector2 createdFrom)
		{
			this.position = pos;
			this.gem = gem;
			this.createdFrom = createdFrom;
		}

		// Token: 0x17000659 RID: 1625
		// (get) Token: 0x0600273C RID: 10044 RVA: 0x000AE466 File Offset: 0x000AC866
		public string Type
		{
			get
			{
				return "treasure";
			}
		}

		// Token: 0x1700065A RID: 1626
		// (get) Token: 0x0600273D RID: 10045 RVA: 0x000AE46D File Offset: 0x000AC86D
		public IntVector2 CreatedFrom
		{
			get
			{
				return this.createdFrom;
			}
		}

		// Token: 0x040051D2 RID: 20946
		public IntVector2 position;

		// Token: 0x040051D3 RID: 20947
		public Gem gem;

		// Token: 0x040051D4 RID: 20948
		private readonly IntVector2 createdFrom;
	}
}
