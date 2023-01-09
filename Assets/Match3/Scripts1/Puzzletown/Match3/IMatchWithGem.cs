using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020005DF RID: 1503
	public interface IMatchWithGem : IMatchGroup, IMatchResult
	{
		// Token: 0x17000619 RID: 1561
		// (get) Token: 0x060026E6 RID: 9958
		Gem Gem { get; }
	}
}
