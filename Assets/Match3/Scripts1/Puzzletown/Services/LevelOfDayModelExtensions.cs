using System;
using System.Collections.Generic;
using System.Text;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x020007D0 RID: 2000
	public static class LevelOfDayModelExtensions
	{
		// Token: 0x06003133 RID: 12595 RVA: 0x000E743A File Offset: 0x000E583A
		public static int GetTryCount(this LevelOfDayModel model)
		{
			return (model != null) ? model.triesSoFar : 0;
		}

		// Token: 0x06003134 RID: 12596 RVA: 0x000E744E File Offset: 0x000E584E
		public static bool HasTriesLeft(this LevelOfDayModel model)
		{
			return model != null && model.triesSoFar < 3;
		}

		// Token: 0x06003135 RID: 12597 RVA: 0x000E7464 File Offset: 0x000E5864
		public static int GetRemainingSeconds(this LevelOfDayModel model, int utcNow)
		{
			int result = -1;
			if (model != null)
			{
				int endUTCTime = model.endUTCTime;
				result = endUTCTime - utcNow;
			}
			return result;
		}

		// Token: 0x06003136 RID: 12598 RVA: 0x000E7488 File Offset: 0x000E5888
		public static bool CanPlayerStillTry(this LevelOfDayModel model, int utcNow, out int remainingSeconds)
		{
			bool result = false;
			remainingSeconds = -1;
			if (model != null)
			{
				remainingSeconds = model.GetRemainingSeconds(utcNow);
				result = (model.HasTriesLeft() && !model.isCompleted && remainingSeconds > 0);
			}
			return result;
		}

		// Token: 0x06003137 RID: 12599 RVA: 0x000E74C9 File Offset: 0x000E58C9
		public static List<int> GetHistory(this LevelOfDayModel model)
		{
			if (model != null && model.lodHistory != null)
			{
				return model.lodHistory;
			}
			return new List<int>();
		}

		// Token: 0x06003138 RID: 12600 RVA: 0x000E74E8 File Offset: 0x000E58E8
		public static string AsString(this LevelOfDayModel model)
		{
			int num = (model != null) ? model.level : 0;
			int num2 = (model != null) ? model.triesSoFar : 0;
			bool flag = model != null && model.isCompleted;
			DateTime dateTime = (model != null) ? Scripts1.DateTimeExtensions.FromUnixTimeStamp(model.endUTCTime, DateTimeKind.Utc) : DateTime.MinValue;
			DateTime dateTime2 = dateTime.ToLocalTime();
			int num3 = (model != null) ? model.currentDay : 1;
			StringBuilder stringBuilder = new StringBuilder();
			if (model != null && model.lodHistory != null)
			{
				foreach (int num4 in model.lodHistory)
				{
					stringBuilder.Append(string.Format("{0}; ", num4));
				}
			}
			else
			{
				stringBuilder.Append("-");
			}
			string text = stringBuilder.ToString();
			return string.Format("Level: {0}\nTries: {1} // Completed: {2}\nEnd: {3:g}\nLocal end: {4:g}\n History: {5}\n Streak: {6}", new object[]
			{
				num,
				num2,
				flag,
				dateTime,
				dateTime2,
				text,
				num3
			});
		}
	}
}
