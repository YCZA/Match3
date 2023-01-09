using Match3.Scripts1.Puzzletown.Config;

// Token: 0x02000647 RID: 1607
namespace Match3.Scripts1
{
	public struct TournamentScore
	{
		// Token: 0x060028C0 RID: 10432 RVA: 0x000B5EFA File Offset: 0x000B42FA
		public TournamentScore(TournamentType tournamentType, int collectedPoint, int multiplier)
		{
			this.tournamentType = tournamentType;
			this.multiplier = multiplier;
			this.collectedPoints = collectedPoint;
		}

		// Token: 0x170006AD RID: 1709
		// (get) Token: 0x060028C1 RID: 10433 RVA: 0x000B5F11 File Offset: 0x000B4311
		public int CollectedPoints
		{
			get
			{
				return this.collectedPoints;
			}
		}

		// Token: 0x170006AE RID: 1710
		// (get) Token: 0x060028C2 RID: 10434 RVA: 0x000B5F19 File Offset: 0x000B4319
		public int Multiplier
		{
			get
			{
				return this.multiplier;
			}
		}

		// Token: 0x170006AF RID: 1711
		// (get) Token: 0x060028C3 RID: 10435 RVA: 0x000B5F21 File Offset: 0x000B4321
		public TournamentType TournamentType
		{
			get
			{
				return this.tournamentType;
			}
		}

		// Token: 0x060028C4 RID: 10436 RVA: 0x000B5F29 File Offset: 0x000B4329
		public void Increment(int delta)
		{
			this.collectedPoints += delta;
		}

		// Token: 0x04005292 RID: 21138
		private int collectedPoints;

		// Token: 0x04005293 RID: 21139
		private readonly int multiplier;

		// Token: 0x04005294 RID: 21140
		private readonly TournamentType tournamentType;
	}
}
