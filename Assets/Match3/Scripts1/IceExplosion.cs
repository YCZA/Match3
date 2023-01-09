using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1
{
	// Token: 0x0200061B RID: 1563
	public struct IceExplosion : IMatchResult
	{
		// Token: 0x060027DF RID: 10207 RVA: 0x000B14D7 File Offset: 0x000AF8D7
		public IceExplosion(IntVector2 pos, Gem gem)
		{
			this.position = pos;
			this.gem = gem;
		}

		// Token: 0x04005225 RID: 21029
		public IntVector2 position;

		// Token: 0x04005226 RID: 21030
		public Gem gem;
	}
}
