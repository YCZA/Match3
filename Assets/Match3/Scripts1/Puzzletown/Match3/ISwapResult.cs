using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020005E8 RID: 1512
	public interface ISwapResult : IExplosionResult, IMatchResult
	{
		// Token: 0x17000622 RID: 1570
		// (get) Token: 0x060026F1 RID: 9969
		IntVector2 From { get; }
	}
}
