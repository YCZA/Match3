using Match3.Scripts1.Puzzletown.Match3;
using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1
{
	// Token: 0x0200060A RID: 1546
	public struct ClimberGemCollected : IProcessableMatch, IMatchResult
	{
		// Token: 0x06002790 RID: 10128 RVA: 0x000AFA98 File Offset: 0x000ADE98
		public ClimberGemCollected(Gem originGem, IntVector2 climber)
		{
			this.origin = originGem;
			this.climberPos = climber;
			this.IsProcessed = false;
		}

		// Token: 0x17000670 RID: 1648
		// (get) Token: 0x06002791 RID: 10129 RVA: 0x000AFAAF File Offset: 0x000ADEAF
		public Gem Origin
		{
			get
			{
				return this.origin;
			}
		}

		// Token: 0x17000671 RID: 1649
		// (get) Token: 0x06002792 RID: 10130 RVA: 0x000AFAB7 File Offset: 0x000ADEB7
		public IntVector2 ClimberPos
		{
			get
			{
				return this.climberPos;
			}
		}

		// Token: 0x17000672 RID: 1650
		// (get) Token: 0x06002793 RID: 10131 RVA: 0x000AFABF File Offset: 0x000ADEBF
		// (set) Token: 0x06002794 RID: 10132 RVA: 0x000AFAC7 File Offset: 0x000ADEC7
		public bool IsProcessed { get; set; }

		// Token: 0x040051FD RID: 20989
		private Gem origin;

		// Token: 0x040051FE RID: 20990
		private IntVector2 climberPos;
	}
}
