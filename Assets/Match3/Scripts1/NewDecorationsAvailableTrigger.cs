using System.Collections;
using Wooga.Coroutines;
using Match3.Scripts2.PlayerData;

// Token: 0x0200097C RID: 2428
namespace Match3.Scripts1
{
	public class NewDecorationsAvailableTrigger : PopupManager.Trigger
	{
		// Token: 0x06003B35 RID: 15157 RVA: 0x0012610C File Offset: 0x0012450C
		public NewDecorationsAvailableTrigger(TownBottomPanelRoot townBottomPanel, QuestService questService, GameStateService gameStateService)
		{
			this.townBottomPanel = townBottomPanel;
			this.questService = questService;
			this.gameStateService = gameStateService;
		}

		// Token: 0x06003B36 RID: 15158 RVA: 0x00126129 File Offset: 0x00124529
		public static string DecoSeenInChapterFlag(int chapter)
		{
			return string.Format("Chapter{0}_buildings", chapter);
		}

		// Token: 0x06003B37 RID: 15159 RVA: 0x0012613C File Offset: 0x0012453C
		public override bool ShouldTrigger()
		{
			return this.questService.questManager.CurrentQuestData != null && this.NotSeenCurrentChapterBuildings();
		}

		// Token: 0x06003B38 RID: 15160 RVA: 0x0012616C File Offset: 0x0012456C
		public override IEnumerator Run()
		{
			Wooroutine<bool> showNewDecoRoutine = WooroutineRunner.StartWooroutine<bool>(this.townBottomPanel.ShowNewDecoRoutine(this.currentChapter));
			yield return showNewDecoRoutine;
			bool shouldSaveSeenFlag = showNewDecoRoutine.ReturnValue;
			if (shouldSaveSeenFlag)
			{
				this.gameStateService.SetSeenFlag(NewDecorationsAvailableTrigger.DecoSeenInChapterFlag(this.currentChapter));
			}
			yield break;
		}

		// Token: 0x06003B39 RID: 15161 RVA: 0x00126187 File Offset: 0x00124587
		private bool NotSeenCurrentChapterBuildings()
		{
			this.currentChapter = this.questService.Chapter;
			return this.currentChapter >= 1 && !this.gameStateService.GetSeenFlag(NewDecorationsAvailableTrigger.DecoSeenInChapterFlag(this.currentChapter));
		}

		// Token: 0x04006333 RID: 25395
		private TownBottomPanelRoot townBottomPanel;

		// Token: 0x04006334 RID: 25396
		private QuestService questService;

		// Token: 0x04006335 RID: 25397
		private GameStateService gameStateService;

		// Token: 0x04006336 RID: 25398
		private int currentChapter;
	}
}
