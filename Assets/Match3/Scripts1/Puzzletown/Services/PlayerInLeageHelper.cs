using System.Text.RegularExpressions;
using Match3.Scripts1.Wooga.Leagues;
using Match3.Scripts2.PlayerData;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x0200080F RID: 2063
	public class PlayerInLeageHelper
	{
		// Token: 0x060032F9 RID: 13049 RVA: 0x000F08E4 File Offset: 0x000EECE4
		public static bool TryExtractPlayerName(string nameAndFacebookID, LeagueUserData userData, out string name, out string fbID)
		{
			System.Text.RegularExpressions.Match match = Regex.Match(nameAndFacebookID, "^(?<name>.+?)(?:_FBID_)(?<fbID>.+\\z)");
			if (match.Success)
			{
				name = match.Groups["name"].Value;
				fbID = match.Groups["fbID"].Value;
				return true;
			}
			name = nameAndFacebookID;
			fbID = ((userData == null) ? string.Empty : userData.FBID);
			return false;
		}

		// Token: 0x060032FA RID: 13050 RVA: 0x000F0954 File Offset: 0x000EED54
		public static PlayerInLeague CreateNewUser(GameStateService gameStateService, FacebookService facebookService, int points)
		{
			return new PlayerInLeague(PlayerInLeageHelper.GetPlayerName(gameStateService), PlayerInLeageHelper.GetFacebookUserID(facebookService), points, string.Empty);
		}

		// Token: 0x060032FB RID: 13051 RVA: 0x000F0970 File Offset: 0x000EED70
		private static string GetPlayerName(GameStateService gameStateService)
		{
			string text = gameStateService.Facebook.loggedInUserFirstName;
			if (string.IsNullOrEmpty(text))
			{
				text = gameStateService.PlayerName;
			}
			if (string.IsNullOrEmpty(text))
			{
				text = "Player";
			}
			return text;
		}

		// Token: 0x060032FC RID: 13052 RVA: 0x000F09B0 File Offset: 0x000EEDB0
		private static string GetFacebookUserID(FacebookService facebookService)
		{
			string empty = string.Empty;
			if (facebookService.LoggedIn())
			{
				return facebookService.FB_MY_ID;
			}
			return empty;
		}

		// Token: 0x04005B48 RID: 23368
		public const string FB_ID_SEPARATOR = "_FBID_";

		// Token: 0x04005B49 RID: 23369
		public const string PLAYER_NAME_FORMAT = "^(?<name>.+?)(?:_FBID_)(?<fbID>.+\\z)";
	}
}
