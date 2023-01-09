using System;
using System.Text.RegularExpressions;
using Match3.Scripts2.PlayerData;

namespace Match3.Scripts1.Puzzletown
{
	// Token: 0x02000833 RID: 2099
	public static class LoadingScreenConditionEvaluator
	{
		// Token: 0x06003432 RID: 13362 RVA: 0x000F8CA0 File Offset: 0x000F70A0
		public static bool Evaluate(string condition, GameStateService gameStateService)
		{
			if (condition.IsNullOrEmpty())
			{
				return true;
			}
			bool flag = false;
			return flag | LoadingScreenConditionEvaluator.EvaluateUnlockLevel(condition, gameStateService);
		}

		// Token: 0x06003433 RID: 13363 RVA: 0x000F8CC8 File Offset: 0x000F70C8
		public static bool EvaluateUnlockLevel(string condition, GameStateService gameStateService)
		{
			if (gameStateService == null || gameStateService.Progression == null)
			{
				return false;
			}
			string pattern = "^(?<predicate>unlock_level_)(?<level>[0-9]+)\\z";
			System.Text.RegularExpressions.Match match = Regex.Match(condition, pattern);
			if (match.Success)
			{
				int num = Convert.ToInt32(match.Groups["level"].Value);
				return gameStateService.Progression.UnlockedLevel >= num;
			}
			return false;
		}
	}
}
