namespace Match3.Scripts1.Wooga.Leagues
{
	// Token: 0x0200041C RID: 1052
	public class PointsUpdateResponse
	{
		// Token: 0x06001EF7 RID: 7927 RVA: 0x00082EE9 File Offset: 0x000812E9
		public PointsUpdateResponse(bool couldReachServer, int playersActualPoints, bool playerNotInLeague)
		{
			this.couldReachServer = couldReachServer;
			this.playersActualPoints = playersActualPoints;
			this.playerNotInLeague = playerNotInLeague;
		}

		// Token: 0x06001EF8 RID: 7928 RVA: 0x00082F06 File Offset: 0x00081306
		public static PointsUpdateResponse UserNotMember()
		{
			return new PointsUpdateResponse(true, 0, true);
		}

		// Token: 0x06001EF9 RID: 7929 RVA: 0x00082F10 File Offset: 0x00081310
		public static PointsUpdateResponse Failure(int currentPoints)
		{
			return new PointsUpdateResponse(false, currentPoints, false);
		}

		// Token: 0x06001EFA RID: 7930 RVA: 0x00082F1A File Offset: 0x0008131A
		public static PointsUpdateResponse Success(int currentPoints)
		{
			return new PointsUpdateResponse(true, currentPoints, false);
		}

		// Token: 0x04004AAC RID: 19116
		public readonly bool couldReachServer;

		// Token: 0x04004AAD RID: 19117
		public readonly int playersActualPoints;

		// Token: 0x04004AAE RID: 19118
		public readonly bool playerNotInLeague;
	}
}
