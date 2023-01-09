using Match3.Scripts1.Puzzletown.Match3;
using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1
{
	// Token: 0x020005CD RID: 1485
	public struct ClimberCollected : IScoreCollectedResult, IProcessableMatch, IMatchResult
	{
		// Token: 0x0600269D RID: 9885 RVA: 0x000AD383 File Offset: 0x000AB783
		public ClimberCollected(Gem gem)
		{
			this.position = gem.position;
			this.gem = gem;
			this.isProcessed = false;
		}

		// Token: 0x170005E9 RID: 1513
		// (get) Token: 0x0600269E RID: 9886 RVA: 0x000AD3A0 File Offset: 0x000AB7A0
		public IntVector2 Position
		{
			get
			{
				return this.position;
			}
		}

		// Token: 0x170005EA RID: 1514
		// (get) Token: 0x0600269F RID: 9887 RVA: 0x000AD3A8 File Offset: 0x000AB7A8
		public string Type
		{
			get
			{
				return "climber";
			}
		}

		// Token: 0x170005EB RID: 1515
		// (get) Token: 0x060026A0 RID: 9888 RVA: 0x000AD3AF File Offset: 0x000AB7AF
		// (set) Token: 0x060026A1 RID: 9889 RVA: 0x000AD3B7 File Offset: 0x000AB7B7
		public bool IsProcessed
		{
			get
			{
				return this.isProcessed;
			}
			set
			{
				this.isProcessed = value;
			}
		}

		// Token: 0x0400516F RID: 20847
		private readonly IntVector2 position;

		// Token: 0x04005170 RID: 20848
		private bool isProcessed;

		// Token: 0x04005171 RID: 20849
		public Gem gem;
	}
}
