using Match3.Scripts1.Puzzletown.Match3;
using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1
{
	// Token: 0x02000610 RID: 1552
	public struct DefinedSpawnNeeded : IProcessableMatch, IMatchResult
	{
		// Token: 0x060027B2 RID: 10162 RVA: 0x000B0653 File Offset: 0x000AEA53
		public DefinedSpawnNeeded(IntVector2 position)
		{
			this.position = position;
			this.IsProcessed = false;
		}

		// Token: 0x17000678 RID: 1656
		// (get) Token: 0x060027B3 RID: 10163 RVA: 0x000B0663 File Offset: 0x000AEA63
		public IntVector2 Position
		{
			get
			{
				return this.position;
			}
		}

		// Token: 0x17000679 RID: 1657
		// (get) Token: 0x060027B4 RID: 10164 RVA: 0x000B066B File Offset: 0x000AEA6B
		// (set) Token: 0x060027B5 RID: 10165 RVA: 0x000B0673 File Offset: 0x000AEA73
		public bool IsProcessed { get; set; }

		// Token: 0x04005210 RID: 21008
		private readonly IntVector2 position;
	}
}
