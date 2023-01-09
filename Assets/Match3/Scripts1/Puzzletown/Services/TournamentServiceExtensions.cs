using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x02000815 RID: 2069
	public static class TournamentServiceExtensions
	{
		// Token: 0x06003338 RID: 13112 RVA: 0x000F2C90 File Offset: 0x000F1090
		public static LeagueTimeState GetTimeState(this TournamentService service, LeagueModel model)
		{
			if (service != null && model.IsValid())
			{
				int now = service.Now;
				return (model.config.start <= now) ? ((model.config.end <= now) ? LeagueTimeState.Past : LeagueTimeState.Running) : LeagueTimeState.Upcoming;
			}
			return LeagueTimeState.None;
		}

		// Token: 0x06003339 RID: 13113 RVA: 0x000F2CE8 File Offset: 0x000F10E8
		public static void ConfirmThatLeagueIsOver(this TournamentService service, LeagueModel model)
		{
			if (service != null && model.IsValid() && model.confirmedOver && service.GetTimeState(model) == LeagueTimeState.Running)
			{
				int end = Mathf.Min(service.Now - 1, model.config.end);
				model.config.end = end;
			}
		}

		// Token: 0x0600333A RID: 13114 RVA: 0x000F2D43 File Offset: 0x000F1143
		public static bool NeedsPlayersAttention(this TournamentService service, LeagueModel model)
		{
			return service != null && model.IsValid() && (service.GetTimeState(model) == LeagueTimeState.Past || model.playerStatus == PlayerLeagueStatus.NotEnteredButQualified);
		}
	}
}
