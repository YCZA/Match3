using Wooga.UnityFramework;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x020007A4 RID: 1956
	public interface IExternalGamesService : IService, IInitializable
	{
		// Token: 0x17000779 RID: 1913
		// (get) Token: 0x06002FEB RID: 12267
		bool IsLoggedIn { get; }

		// Token: 0x06002FEC RID: 12268
		void LogIn(bool checkForPreviousAttempt = true, bool silent = false);

		// Token: 0x06002FED RID: 12269
		void LogOut();

		// Token: 0x06002FEE RID: 12270
		void ShowAchievementsUi();

		// Token: 0x06002FEF RID: 12271
		void ShowVillageRankAchievement(int villageRank);

		// Token: 0x06002FF0 RID: 12272
		void ShowLevelAchievement(int level);

		// Token: 0x06002FF1 RID: 12273
		void ShowLevelMasteryAchievement(int masteredLevels);

		// Token: 0x06002FF2 RID: 12274
		void ShowWheelSpinAchievement();

		// Token: 0x06002FF3 RID: 12275
		void ShowJoinTournamentAchievement();
	}
}
