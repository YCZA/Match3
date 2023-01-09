using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020005DE RID: 1502
	public interface IMatchGroup : IMatchResult
	{
		// Token: 0x17000617 RID: 1559
		// (get) Token: 0x060026E4 RID: 9956
		Group Group { get; }

		// Token: 0x17000618 RID: 1560
		// (get) Token: 0x060026E5 RID: 9957
		bool ShouldHitAdjacentFields { get; }
	}
}
