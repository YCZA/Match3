using System;

namespace Match3.Scripts1.Wooga.Leagues
{
	// Token: 0x0200041B RID: 1051
	[Serializable]
	public class PlayerInLeague
	{
		// Token: 0x06001EF6 RID: 7926 RVA: 0x00082EAC File Offset: 0x000812AC
		public PlayerInLeague(string name, string facebookUserID, int points, string tier = "")
		{
			this.name = name;
			this.user_data = new LeagueUserData
			{
				FBID = facebookUserID
			};
			this.points = points;
			this.tier = tier;
		}

		// Token: 0x04004AA8 RID: 19112
		public readonly string name;

		// Token: 0x04004AA9 RID: 19113
		public readonly int points;

		// Token: 0x04004AAA RID: 19114
		public readonly LeagueUserData user_data;

		// Token: 0x04004AAB RID: 19115
		public readonly string tier;
	}
}
