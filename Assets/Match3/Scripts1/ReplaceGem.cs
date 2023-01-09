using Match3.Scripts1.Shared.M3Engine;

// Token: 0x020005B4 RID: 1460
namespace Match3.Scripts1
{
	public struct ReplaceGem : IMatchResult
	{
		// Token: 0x06002629 RID: 9769 RVA: 0x000AAAED File Offset: 0x000A8EED
		public ReplaceGem(Gem gem)
		{
			this.gem = gem;
		}

		// Token: 0x04005117 RID: 20759
		public Gem gem;
	}
}
