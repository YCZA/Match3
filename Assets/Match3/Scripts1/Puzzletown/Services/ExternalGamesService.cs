using System;
using Match3.Scripts1.Wooga.Signals;
using Wooga.UnityFramework;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x020007A5 RID: 1957
	public class ExternalGamesService : AService, IExternalGamesService, IService, IInitializable
	{
		// Token: 0x06002FF4 RID: 12276 RVA: 0x000E1778 File Offset: 0x000DFB78
		public ExternalGamesService()
		{
			this.service = new GooglePlayGamesService();
		}

		// Token: 0x1700077A RID: 1914
		// (get) Token: 0x06002FF5 RID: 12277 RVA: 0x000E178B File Offset: 0x000DFB8B
		public new AwaitSignal OnInitialized
		{
			get
			{
				return this.service.OnInitialized;
			}
		}

		// Token: 0x1700077B RID: 1915
		// (get) Token: 0x06002FF6 RID: 12278 RVA: 0x000E1798 File Offset: 0x000DFB98
		public bool IsLoggedIn
		{
			get
			{
				return this.service.IsLoggedIn;
			}
		}

		// Token: 0x06002FF7 RID: 12279 RVA: 0x000E17A5 File Offset: 0x000DFBA5
		public void LogIn(bool checkForPreviousAttempt = true, bool silentLogin = false)
		{
			this.service.LogIn(checkForPreviousAttempt, silentLogin);
		}

		// Token: 0x06002FF8 RID: 12280 RVA: 0x000E17B4 File Offset: 0x000DFBB4
		public void LogOut()
		{
			this.service.LogOut();
		}

		// Token: 0x06002FF9 RID: 12281 RVA: 0x000E17C1 File Offset: 0x000DFBC1
		public void ShowAchievementsUi()
		{
			this.service.ShowAchievementsUi();
		}

		// Token: 0x06002FFA RID: 12282 RVA: 0x000E17CE File Offset: 0x000DFBCE
		public void ShowVillageRankAchievement(int villageRank)
		{
			this.service.ShowVillageRankAchievement(villageRank);
		}

		// Token: 0x06002FFB RID: 12283 RVA: 0x000E17DC File Offset: 0x000DFBDC
		public void ShowLevelAchievement(int level)
		{
			this.service.ShowLevelAchievement(level);
		}

		// Token: 0x06002FFC RID: 12284 RVA: 0x000E17EA File Offset: 0x000DFBEA
		public void ShowLevelMasteryAchievement(int masteredLevels)
		{
			this.service.ShowLevelMasteryAchievement(masteredLevels);
		}

		// Token: 0x06002FFD RID: 12285 RVA: 0x000E17F8 File Offset: 0x000DFBF8
		public void ShowWheelSpinAchievement()
		{
			this.service.ShowWheelSpinAchievement();
		}

		// Token: 0x06002FFE RID: 12286 RVA: 0x000E1805 File Offset: 0x000DFC05
		public void ShowJoinTournamentAchievement()
		{
			this.service.ShowJoinTournamentAchievement();
		}

		// Token: 0x06002FFF RID: 12287 RVA: 0x000E1812 File Offset: 0x000DFC12
		public new void DeInit()
		{
			this.service.DeInit();
		}

		// Token: 0x06003000 RID: 12288 RVA: 0x000E181F File Offset: 0x000DFC1F
		public new void OnSuspend()
		{
			this.service.OnSuspend();
		}

		// Token: 0x06003001 RID: 12289 RVA: 0x000E182C File Offset: 0x000DFC2C
		public new void OnResume()
		{
			this.service.OnResume();
		}

		// Token: 0x040058FE RID: 22782
		private readonly IExternalGamesService service;
		public void Init()
		{
			throw new NotImplementedException();
		}
	}
}
