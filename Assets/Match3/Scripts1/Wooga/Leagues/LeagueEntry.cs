using System;

namespace Match3.Scripts1.Wooga.Leagues
{
	// Token: 0x02000418 RID: 1048
	[Serializable]
	public class LeagueEntry
	{
		// Token: 0x06001EE9 RID: 7913 RVA: 0x00082AD7 File Offset: 0x00080ED7
		public override string ToString()
		{
			return string.Format("{0} ({1}): {2} points, {3}", new object[]
			{
				this.name,
				this.sbs_user_id,
				this.points,
				this.user_data.FBID
			});
		}

		// Token: 0x04004A9A RID: 19098
		public string name;

		// Token: 0x04004A9B RID: 19099
		public int points;

		// Token: 0x04004A9C RID: 19100
		public string sbs_user_id;

		// Token: 0x04004A9D RID: 19101
		public LeagueUserData user_data;

		// Token: 0x04004A9E RID: 19102
		public int updated_at;
	}
}
