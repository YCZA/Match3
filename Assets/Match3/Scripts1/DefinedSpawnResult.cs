using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1
{
	// Token: 0x020005D3 RID: 1491
	public struct DefinedSpawnResult : IMatchResult
	{
		// Token: 0x060026B8 RID: 9912 RVA: 0x000AD5E4 File Offset: 0x000AB9E4
		public DefinedSpawnResult(IntVector2 position, Gem gem)
		{
			this.position = position;
			this.gem = gem;
		}

		// Token: 0x060026B9 RID: 9913 RVA: 0x000AD5F4 File Offset: 0x000AB9F4
		public override string ToString()
		{
			return string.Format("[DefinedSpawnResult: gem={0}]", this.gem);
		}

		// Token: 0x0400517E RID: 20862
		public IntVector2 position;

		// Token: 0x0400517F RID: 20863
		public Gem gem;
	}
}
