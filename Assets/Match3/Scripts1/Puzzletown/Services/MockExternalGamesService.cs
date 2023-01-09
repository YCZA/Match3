namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x020007A6 RID: 1958
	public class MockExternalGamesService //: AService, IExternalGamesService, IService, IInitializable
	{
		// Token: 0x06003002 RID: 12290 RVA: 0x000E1839 File Offset: 0x000DFC39
		public MockExternalGamesService()
		{
			// base.OnInitialized.Dispatch();
		}

		// Token: 0x1700077C RID: 1916
		// (get) Token: 0x06003003 RID: 12291 RVA: 0x000E184C File Offset: 0x000DFC4C
		public virtual bool IsLoggedIn
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06003004 RID: 12292 RVA: 0x000E184F File Offset: 0x000DFC4F
		public void LogIn(bool checkForPreviousAttempt = true, bool silent = false)
		{
		}

		// Token: 0x06003005 RID: 12293 RVA: 0x000E1851 File Offset: 0x000DFC51
		public void LogOut()
		{
		}

		// Token: 0x06003006 RID: 12294 RVA: 0x000E1853 File Offset: 0x000DFC53
		public void ShowAchievementsUi()
		{
		}

		// Token: 0x06003007 RID: 12295 RVA: 0x000E1855 File Offset: 0x000DFC55
		public void ShowVillageRankAchievement(int villageRank)
		{
		}

		// Token: 0x06003008 RID: 12296 RVA: 0x000E1857 File Offset: 0x000DFC57
		public void ShowLevelAchievement(int level)
		{
		}

		// Token: 0x06003009 RID: 12297 RVA: 0x000E1859 File Offset: 0x000DFC59
		public void ShowLevelMasteryAchievement(int masteredLevels)
		{
		}

		// Token: 0x0600300A RID: 12298 RVA: 0x000E185B File Offset: 0x000DFC5B
		public void ShowWheelSpinAchievement()
		{
		}

		// Token: 0x0600300B RID: 12299 RVA: 0x000E185D File Offset: 0x000DFC5D
		public void ShowJoinTournamentAchievement()
		{
		}
	}
}
