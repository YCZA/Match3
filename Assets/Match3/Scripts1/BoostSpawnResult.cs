using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1
{
	// Token: 0x020005CB RID: 1483
	public struct BoostSpawnResult : IFinalMovement, IMatchResult
	{
		// Token: 0x06002692 RID: 9874 RVA: 0x000AD2E2 File Offset: 0x000AB6E2
		public BoostSpawnResult(Gem gem)
		{
			this.gem = gem;
			this.IsFinal = true;
		}

		// Token: 0x170005E3 RID: 1507
		// (get) Token: 0x06002693 RID: 9875 RVA: 0x000AD2F2 File Offset: 0x000AB6F2
		// (set) Token: 0x06002694 RID: 9876 RVA: 0x000AD2FA File Offset: 0x000AB6FA
		public bool IsFinal { get; set; }

		// Token: 0x170005E4 RID: 1508
		// (get) Token: 0x06002695 RID: 9877 RVA: 0x000AD303 File Offset: 0x000AB703
		public IntVector2 Position
		{
			get
			{
				return this.gem.position;
			}
		}

		// Token: 0x06002696 RID: 9878 RVA: 0x000AD310 File Offset: 0x000AB710
		public override string ToString()
		{
			return string.Format("[LastHurraySpawnResult: gem={0}]", this.gem);
		}

		// Token: 0x0400516B RID: 20843
		public Gem gem;
	}
}
