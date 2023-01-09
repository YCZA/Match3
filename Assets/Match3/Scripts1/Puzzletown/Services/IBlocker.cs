using System.Collections;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x02000753 RID: 1875
	public interface IBlocker
	{
		// Token: 0x06002E73 RID: 11891
		IEnumerator ExecuteRoutine();

		// Token: 0x1700072C RID: 1836
		// (get) Token: 0x06002E74 RID: 11892
		bool BlockInput { get; }
	}
}
