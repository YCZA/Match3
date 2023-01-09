using System;

namespace Match3.Scripts1.Puzzletown.Config
{
	// Token: 0x020004A7 RID: 1191
	[Serializable]
	public class TournamentEventConfig : EventConfig
	{
		// Token: 0x0600219E RID: 8606 RVA: 0x0008CE4C File Offset: 0x0008B24C
		public TournamentEventConfig Copy()
		{
			TournamentEventConfig tournamentEventConfig = new TournamentEventConfig();
			tournamentEventConfig.id = this.id;
			tournamentEventConfig.start = this.start;
			tournamentEventConfig.end = this.end;
			if (this.config != null)
			{
				tournamentEventConfig.config = this.config.Copy();
			}
			return tournamentEventConfig;
		}

		// Token: 0x04004CBE RID: 19646
		public TournamentConfig config;
	}
}
