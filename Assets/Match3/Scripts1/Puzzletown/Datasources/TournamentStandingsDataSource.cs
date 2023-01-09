namespace Match3.Scripts1.Puzzletown.Datasources
{
	// Token: 0x02000A5B RID: 2651
	public class TournamentStandingsDataSource : ArrayDataSource<TournamentStandingView, TournamentStandingData>
	{
		// Token: 0x06003F83 RID: 16259 RVA: 0x001456EC File Offset: 0x00143AEC
		public override void Cleanup()
		{
			if (this.dataArray != null)
			{
				foreach (TournamentStandingData tournamentStandingData in this.dataArray)
				{
					if (tournamentStandingData.fbData != null)
					{
						tournamentStandingData.fbData.Dispose();
					}
				}
			}
			base.Cleanup();
		}
	}
}
