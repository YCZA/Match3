using System;

// Token: 0x0200092E RID: 2350
namespace Match3.Scripts1
{
	[Serializable]
	public class QuestConfig
	{
		// Token: 0x06003915 RID: 14613 RVA: 0x00119294 File Offset: 0x00117694
		public static int QuestIndexForLevel(int levelNumber)
		{
			int num = levelNumber - 1;
			int num2;
			if (num < 10)
			{
				num2 = num / 5;
			}
			else
			{
				num2 = (num - 10) / 10;
				num2 += 2;
			}
			return num2;
		}

		// Token: 0x06003916 RID: 14614 RVA: 0x001192C8 File Offset: 0x001176C8
		public void Init()
		{
			foreach (QuestData questData in this.quests)
			{
				questData.rewardCount = this.reward.rewardCount;
				questData.rewardItem = this.reward.rewardItem;
			}
		}

		// Token: 0x04006177 RID: 24951
		[NonSerialized]
		public const string TASK_LEVEL_FORMAT = "^(?<level>([1-9]+[0-9]*))(?<tier>[a-c]\\z)";

		// Token: 0x04006178 RID: 24952
		[NonSerialized]
		public const string END_OF_CONTENT_ID = "end_of_content";

		// Token: 0x04006179 RID: 24953
		[NonSerialized]
		public const string CHAPTER_INTRO_ID = "chapter_intro";

		// Token: 0x0400617A RID: 24954
		[NonSerialized]
		public const int LEVELS_PER_REGULAR_QUEST = 10;

		// Token: 0x0400617B RID: 24955
		private const int LEVELS_PER_SHORT_QUEST = 5;

		// Token: 0x0400617C RID: 24956
		private const int SHORT_QUESTS = 2;

		// Token: 0x0400617D RID: 24957
		private const int REGULAR_QUEST_OFFSET = 10;

		// Token: 0x0400617E RID: 24958
		public QuestData[] quests;

		// Token: 0x0400617F RID: 24959
		public RewardData reward;
	}
}
