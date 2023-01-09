using Match3.Scripts1.Puzzletown.Match3;

namespace Match3.Scripts1.Shared.M3Engine
{
	// Token: 0x02000AF5 RID: 2805
	public interface IBoardFactory
	{
		// Token: 0x17000991 RID: 2449
		// (get) Token: 0x06004276 RID: 17014
		Fields Fields { get; }

		// Token: 0x17000992 RID: 2450
		// (get) Token: 0x06004277 RID: 17015
		GemFactory GemFactory { get; }

		// Token: 0x06004278 RID: 17016
		void CreateBoard();
	}
}
