using Match3.Scripts1.Puzzletown.Services;

namespace Match3.Scripts1
{
	// Token: 0x020007BA RID: 1978
	public struct PlayerStateInLeague
	{
		// Token: 0x060030B4 RID: 12468 RVA: 0x000E4464 File Offset: 0x000E2864
		public static PlayerStateInLeague None()
		{
			return new PlayerStateInLeague
			{
				status = PlayerLeagueStatus.None,
				userID = string.Empty,
				currentPoints = 0,
				previousPoints = 0,
				tier = string.Empty
			};
		}

		// Token: 0x0400597B RID: 22907
		public PlayerLeagueStatus status;

		// Token: 0x0400597C RID: 22908
		public string userID;

		// Token: 0x0400597D RID: 22909
		public int currentPoints;

		// Token: 0x0400597E RID: 22910
		public int previousPoints;

		// Token: 0x0400597F RID: 22911
		public string tier;
	}
}
