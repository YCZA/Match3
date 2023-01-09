using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1
{
	// Token: 0x020005D4 RID: 1492
	public struct DirtExplosion : IMatchResult
	{
		// Token: 0x060026BA RID: 9914 RVA: 0x000AD60B File Offset: 0x000ABA0B
		public DirtExplosion(IntVector2 pos, Gem gem, int newHp, IntVector2 createdFrom)
		{
			this.position = pos;
			this.gem = gem;
			this.newHp = newHp;
			this.createdFrom = createdFrom;
		}

		// Token: 0x170005F7 RID: 1527
		// (get) Token: 0x060026BB RID: 9915 RVA: 0x000AD62A File Offset: 0x000ABA2A
		public IntVector2 CreatedFrom
		{
			get
			{
				return this.createdFrom;
			}
		}

		// Token: 0x04005180 RID: 20864
		public IntVector2 position;

		// Token: 0x04005181 RID: 20865
		public Gem gem;

		// Token: 0x04005182 RID: 20866
		public readonly int newHp;

		// Token: 0x04005183 RID: 20867
		private readonly IntVector2 createdFrom;
	}
}
