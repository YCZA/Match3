using System.Collections.Generic;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x0200074B RID: 1867
	public class UnlockedLevelPreloadTrigger : APreloadTrigger
	{
		// Token: 0x06002E38 RID: 11832 RVA: 0x000D795C File Offset: 0x000D5D5C
		public UnlockedLevelPreloadTrigger(string bundleName, int level, QuestService questService) : base(new List<string>
		{
			bundleName
		})
		{
			this.quests = questService;
			this.level = level;
		}

		// Token: 0x06002E39 RID: 11833 RVA: 0x000D798C File Offset: 0x000D5D8C
		public override bool ShouldPreload()
		{
			int unlockedLevelWithQuestAndEndOfContent = this.quests.UnlockedLevelWithQuestAndEndOfContent;
			return unlockedLevelWithQuestAndEndOfContent >= this.level;
		}

		// Token: 0x04005793 RID: 22419
		private readonly QuestService quests;

		// Token: 0x04005794 RID: 22420
		private readonly int level;
	}
}
