using Match3.Scripts1.Puzzletown.Config;

// Token: 0x02000A64 RID: 2660
namespace Match3.Scripts1
{
	public struct TournamentTrophy
	{
		// Token: 0x06003FB8 RID: 16312 RVA: 0x001467EF File Offset: 0x00144BEF
		public TournamentTrophy(TournamentType t, int pPos)
		{
			this.type = t;
			this.position = pPos;
		}

		// Token: 0x0400695D RID: 26973
		public TournamentType type;

		// Token: 0x0400695E RID: 26974
		public int position;
	}
}
