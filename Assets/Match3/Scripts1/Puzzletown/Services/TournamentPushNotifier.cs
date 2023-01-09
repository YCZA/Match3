namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x02000813 RID: 2067
	public class TournamentPushNotifier
	{
		// Token: 0x06003308 RID: 13064 RVA: 0x000F0C09 File Offset: 0x000EF009
		public TournamentPushNotifier(IPushNotificationService pService, int topPlayersCount = -1)
		{
			this.pushNotificationService = pService;
			this.topPlayersCount = ((topPlayersCount > 0) ? topPlayersCount : TournamentTop10Snapshot.TopPlayersCount);
		}

		// Token: 0x1700080D RID: 2061
		// (get) Token: 0x06003309 RID: 13065 RVA: 0x000F0C30 File Offset: 0x000EF030
		public bool HasSnapshot
		{
			get
			{
				return this._lastSnapshot != null;
			}
		}

		// Token: 0x0600330A RID: 13066 RVA: 0x000F0C40 File Offset: 0x000EF040
		public bool TryNotifyPlayerWhoGotKickedOutOfTop10(LeagueModel model, int now)
		{
			bool result = false;
			TournamentTop10Snapshot tournamentTop10Snapshot = new TournamentTop10Snapshot(model, this.topPlayersCount);
			if (tournamentTop10Snapshot.IsValid(now) && this._lastSnapshot != null && this._lastSnapshot.IsValid(now) && tournamentTop10Snapshot.leagueID == this._lastSnapshot.leagueID && this._lastSnapshot.isPlayerOutOfTop10 && !tournamentTop10Snapshot.isPlayerOutOfTop10)
			{
				result = this.TrySendNotificationToAffectedPlayer(this._lastSnapshot, model);
			}
			this._lastSnapshot = tournamentTop10Snapshot;
			return result;
		}

		// Token: 0x0600330B RID: 13067 RVA: 0x000F0CD0 File Offset: 0x000EF0D0
		private bool TrySendNotificationToAffectedPlayer(TournamentTop10Snapshot lastSnapshot, LeagueModel currentModel)
		{
			if (this.pushNotificationService == null)
			{
				return false;
			}
			for (int i = this.topPlayersCount; i < currentModel.sortedStandings.Length; i++)
			{
				string sbs_user_id = currentModel.sortedStandings[i].sbs_user_id;
				if (sbs_user_id == currentModel.userID)
				{
					WoogaDebug.LogWarning(new object[]
					{
						"TournamentPushNotifier: Player is both in and out of top 10, this shouldn't happen."
					});
					return false;
				}
				if (!string.IsNullOrEmpty(sbs_user_id) && lastSnapshot.ContainsUserID(sbs_user_id) && this.pushNotificationService != null)
				{
					this.pushNotificationService.SendNotificationToStranger(sbs_user_id, "out_of_ten");
					WoogaDebug.Log(new object[]
					{
						"Sending out-of-top-10 push notification to: ",
						sbs_user_id
					});
					return true;
				}
			}
			return false;
		}

		// Token: 0x04005B56 RID: 23382
		private TournamentTop10Snapshot _lastSnapshot;

		// Token: 0x04005B57 RID: 23383
		private readonly IPushNotificationService pushNotificationService;

		// Token: 0x04005B58 RID: 23384
		public readonly int topPlayersCount;
	}
}
