using System;
using System.Collections;
using System.Collections.Generic;
using Wooga.Coroutines;
using Wooga.UnityFramework;
using UnityEngine;
// using GooglePlayGames;
// using GooglePlayGames.BasicApi;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x020007B5 RID: 1973
	public class GooglePlayGamesService : AService, IExternalGamesService, IService, IInitializable
	{
		// Token: 0x0600308E RID: 12430 RVA: 0x000E3C74 File Offset: 0x000E2074
		public GooglePlayGamesService()
		{
			WooroutineRunner.StartCoroutine(this.LoadRoutine(), null);
		}

		// Token: 0x170007A5 RID: 1957
		// (get) Token: 0x0600308F RID: 12431 RVA: 0x000E3C89 File Offset: 0x000E2089
		public bool IsLoggedIn
		{
			get
			{
				// if (this.wasPreviouslyLoggedIn && !PlayGamesPlatform.Instance.IsAuthenticated())
				// {
				// 	this.PreviousLoginSuccessful = false;
				// }
				// this.wasPreviouslyLoggedIn = PlayGamesPlatform.Instance.IsAuthenticated();
				return this.wasPreviouslyLoggedIn;
			}
		}

		// Token: 0x06003090 RID: 12432 RVA: 0x000E3CC4 File Offset: 0x000E20C4
		private IEnumerator LoadRoutine()
		{
			this.completedAchievements = new Dictionary<string, bool>();
			yield return ServiceLocator.Instance.Inject(this);
			// PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
			// PlayGamesPlatform.InitializeInstance(config);
			// PlayGamesPlatform.DebugLogEnabled = true;
			// PlayGamesPlatform.Activate();
			base.OnInitialized.Dispatch();
			yield break;
		}

		// Token: 0x06003091 RID: 12433 RVA: 0x000E3CE0 File Offset: 0x000E20E0
		public void LogIn(bool checkForPreviousAttempt = true, bool silent = false)
		{
			if (checkForPreviousAttempt && !this.PreviousLoginSuccessful)
			{
				return;
			}
			Action<bool> callback = delegate(bool success)
			{
				this.PreviousLoginSuccessful = success;
				if (success)
				{
					// PlayGamesPlatform.Instance.LoadAchievements(delegate(IAchievement[] achievements)
					// {
					// 	if (achievements != null)
					// 	{
					// 		foreach (IAchievement achievement in achievements)
					// 		{
					// 			this.completedAchievements[achievement.id] = achievement.completed;
					// 		}
					// 	}
					// });
				}
			};
			// PlayGamesPlatform.Instance.Authenticate(callback, silent);
		}

		// Token: 0x06003092 RID: 12434 RVA: 0x000E3D18 File Offset: 0x000E2118
		public void LogOut()
		{
			this.PreviousLoginSuccessful = false;
			// PlayGamesPlatform.Instance.SignOut();
		}

		// Token: 0x06003093 RID: 12435 RVA: 0x000E3D2B File Offset: 0x000E212B
		public void ShowAchievementsUi()
		{
			// PlayGamesPlatform.Instance.ShowAchievementsUI();
		}

		// Token: 0x170007A6 RID: 1958
		// (get) Token: 0x06003094 RID: 12436 RVA: 0x000E3D37 File Offset: 0x000E2137
		// (set) Token: 0x06003095 RID: 12437 RVA: 0x000E3D58 File Offset: 0x000E2158
		private bool PreviousLoginSuccessful
		{
			get
			{
				return !PlayerPrefs.HasKey("previousLoginSuccessful") || PlayerPrefs.GetInt("previousLoginSuccessful") == 1;
			}
			set
			{
				PlayerPrefs.SetInt("previousLoginSuccessful", (!value) ? 0 : 1);
			}
		}

		// Token: 0x06003096 RID: 12438 RVA: 0x000E3D74 File Offset: 0x000E2174
		private void CheckAndReportProgress(string achievementId, int incrementSteps = 0)
		{
			if (incrementSteps > 0)
			{
				// PlayGamesPlatform.Instance.IncrementAchievement(achievementId, incrementSteps, delegate(bool success)
				// {
				// });
			}
			else if (!this.completedAchievements.ContainsKey(achievementId) || !this.completedAchievements[achievementId])
			{
				//Social.ReportProgress(achievementId, 100.0, delegate(bool success)
				//{
				//	this.completedAchievements[achievementId] = success;
				//});
			}
		}

		// Token: 0x06003097 RID: 12439 RVA: 0x000E3E1C File Offset: 0x000E221C
		public void ShowVillageRankAchievement(int villageRank)
		{
			if (villageRank >= 3)
			{
				this.CheckAndReportProgress("CgkIwp-a8c0aEAIQAg", 0);
			}
			if (villageRank >= 7)
			{
				this.CheckAndReportProgress("CgkIwp-a8c0aEAIQCA", 0);
			}
			if (villageRank >= 12)
			{
				this.CheckAndReportProgress("CgkIwp-a8c0aEAIQEQ", 0);
			}
			if (villageRank >= 25)
			{
				this.CheckAndReportProgress("CgkIwp-a8c0aEAIQFA", 0);
			}
		}

		// Token: 0x06003098 RID: 12440 RVA: 0x000E3E78 File Offset: 0x000E2278
		public void ShowLevelAchievement(int level)
		{
			if (level >= 3)
			{
				this.CheckAndReportProgress("CgkIwp-a8c0aEAIQAQ", 0);
			}
			if (level >= 25)
			{
				this.CheckAndReportProgress("CgkIwp-a8c0aEAIQBg", 0);
			}
			if (level >= 75)
			{
				this.CheckAndReportProgress("CgkIwp-a8c0aEAIQCg", 0);
			}
			if (level >= 125)
			{
				this.CheckAndReportProgress("CgkIwp-a8c0aEAIQDA", 0);
			}
			if (level >= 225)
			{
				this.CheckAndReportProgress("CgkIwp-a8c0aEAIQDg", 0);
			}
			if (level >= 325)
			{
				this.CheckAndReportProgress("CgkIwp-a8c0aEAIQEw", 0);
			}
		}

		// Token: 0x06003099 RID: 12441 RVA: 0x000E3F04 File Offset: 0x000E2304
		public void ShowLevelMasteryAchievement(int masteredLevels)
		{
			if (masteredLevels >= 10)
			{
				this.CheckAndReportProgress("CgkIwp-a8c0aEAIQAw", 0);
			}
			if (masteredLevels >= 25)
			{
				this.CheckAndReportProgress("CgkIwp-a8c0aEAIQCQ", 0);
			}
			if (masteredLevels >= 50)
			{
				this.CheckAndReportProgress("CgkIwp-a8c0aEAIQDw", 0);
			}
			if (masteredLevels >= 100)
			{
				this.CheckAndReportProgress("CgkIwp-a8c0aEAIQFQ", 0);
			}
		}

		// Token: 0x0600309A RID: 12442 RVA: 0x000E3F61 File Offset: 0x000E2361
		public void ShowWheelSpinAchievement()
		{
			this.CheckAndReportProgress("CgkIwp-a8c0aEAIQFg", 1);
			this.CheckAndReportProgress("CgkIwp-a8c0aEAIQEA", 1);
			this.CheckAndReportProgress("CgkIwp-a8c0aEAIQCw", 1);
			this.CheckAndReportProgress("CgkIwp-a8c0aEAIQBA", 1);
		}

		// Token: 0x0600309B RID: 12443 RVA: 0x000E3F93 File Offset: 0x000E2393
		public void ShowJoinTournamentAchievement()
		{
			this.CheckAndReportProgress("CgkIwp-a8c0aEAIQBQ", 0);
			this.CheckAndReportProgress("CgkIwp-a8c0aEAIQFw", 1);
			this.CheckAndReportProgress("CgkIwp-a8c0aEAIQEg", 1);
			this.CheckAndReportProgress("CgkIwp-a8c0aEAIQDQ", 1);
			this.CheckAndReportProgress("CgkIwp-a8c0aEAIQBw", 1);
		}

		// Token: 0x0400596C RID: 22892
		private const string PREVIOUS_LOGIN_KEY = "previousLoginSuccessful";

		// Token: 0x0400596D RID: 22893
		private Dictionary<string, bool> completedAchievements;

		// Token: 0x0400596E RID: 22894
		private bool wasPreviouslyLoggedIn;
		public void Init()
		{
			throw new NotImplementedException();
		}
	}
}
