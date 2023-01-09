using Match3.Scripts1.Puzzletown.Config;
using Match3.Scripts1.Shared.M3Engine;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020005E9 RID: 1513
	public interface ITournamentScoreMatch : IMatchResult
	{
		// Token: 0x17000623 RID: 1571
		// (get) Token: 0x060026F2 RID: 9970
		TournamentType Type { get; }

		// Token: 0x17000624 RID: 1572
		// (get) Token: 0x060026F3 RID: 9971
		IntVector2 Position { get; }

		// Token: 0x17000625 RID: 1573
		// (get) Token: 0x060026F4 RID: 9972
		Vector3 ReleasePosition { get; }
	}
}
