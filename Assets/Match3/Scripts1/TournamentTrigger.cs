using System.Collections;
using Match3.Scripts1.Puzzletown.Flows;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Wooga.Services;

// Token: 0x02000A63 RID: 2659
namespace Match3.Scripts1
{
	public class TournamentTrigger : PopupManager.Trigger
	{
		// Token: 0x06003FB5 RID: 16309 RVA: 0x0014670C File Offset: 0x00144B0C
		public TournamentTrigger(TournamentService tournamentService)
		{
			this.tournamentService = tournamentService;
		}

		// Token: 0x06003FB6 RID: 16310 RVA: 0x0014671B File Offset: 0x00144B1B
		public override bool ShouldTrigger()
		{
			return this.tournamentService.Status.IsUnlocked && !this.tournamentService.Status.IsBeingRefreshed && SBS.IsAuthenticated();
		}

		// Token: 0x06003FB7 RID: 16311 RVA: 0x00146750 File Offset: 0x00144B50
		public override IEnumerator Run()
		{
			yield return new CheckForTournamentNotificationsFlow().Start();
			yield break;
		}

		// Token: 0x0400695C RID: 26972
		private readonly TournamentService tournamentService;
	}
}
