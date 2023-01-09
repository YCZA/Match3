namespace Match3.Scripts1.Wooga.Leagues
{
	// Token: 0x0200041F RID: 1055
	public class UserQueryResponse
	{
		// Token: 0x06001F05 RID: 7941 RVA: 0x00082F80 File Offset: 0x00081380
		public static UserQueryResponse Failure()
		{
			return new UserQueryResponse
			{
				success = false
			};
		}

		// Token: 0x04004AB6 RID: 19126
		public bool success;

		// Token: 0x04004AB7 RID: 19127
		public LeagueEntry userInfo;
	}
}
