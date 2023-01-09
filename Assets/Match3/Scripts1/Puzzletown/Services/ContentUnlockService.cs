using System;
using System.Collections;
using System.Linq;
using Match3.Scripts1.Puzzletown.Match3;
using Wooga.Coroutines;
using Wooga.UnityFramework;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x02000762 RID: 1890
	public class ContentUnlockService : AService
	{
		// Token: 0x06002EE5 RID: 12005 RVA: 0x000DB25F File Offset: 0x000D965F
		public ContentUnlockService()
		{
			WooroutineRunner.StartCoroutine(this.InitRoutine(), null);
		}

		// Token: 0x1700074F RID: 1871
		// (get) Token: 0x06002EE6 RID: 12006 RVA: 0x000DB274 File Offset: 0x000D9674
		// (set) Token: 0x06002EE7 RID: 12007 RVA: 0x000DB27C File Offset: 0x000D967C
		public bool IsUnlockDisabled
		{
			get
			{
				return this.isUnlockDisabled;
			}
			set
			{
				this.isUnlockDisabled = value;
			}
		}

		// Token: 0x06002EE8 RID: 12008 RVA: 0x000DB288 File Offset: 0x000D9688
		private IEnumerator InitRoutine()
		{
			yield return ServiceLocator.Instance.Inject(this);
			base.OnInitialized.Dispatch();
			yield break;
		}

		// Token: 0x06002EE9 RID: 12009 RVA: 0x000DB2A4 File Offset: 0x000D96A4
		public int EndOfContentLevel()
		{
			// eli key point 审核版本只有330关(405关可用，只开放前5个area, 正好330关)
			// #if REVIEW_VERSION
			// {
			// 	return 331;
			// }
			// #endif
			int last_area = this.configService.general.tier_unlocked.last_area;
			return this.configService.areas.areas[last_area - 1].levels.Last<AreaConfig>().level + 1;
		}

		// Token: 0x06002EEA RID: 12010 RVA: 0x000DB2F4 File Offset: 0x000D96F4
		public bool IsLockedByEndOfContent(int level)
		{
			if (this.UnlockDateForLevel(level) != null)
			{
				return !this.timeService.IsTimeValid;
			}
			return level == this.EndOfContentLevel();
		}

		// Token: 0x06002EEB RID: 12011 RVA: 0x000DB330 File Offset: 0x000D9730
		public DateTime? UnlockDateForIsland(int islandId)
		{
			int num = this.configService.SbsConfig.islandareaconfig.FirstGlobalArea(islandId);
			if (num > this.configService.general.tier_unlocked.last_area)
			{
				return this.UnlockDateForQuest("end_of_content");
			}
			int firstLevelOfArea = this.m3ConfigService.GetFirstLevelOfArea(num);
			return this.UnlockDateForLevel(firstLevelOfArea);
		}

		// Token: 0x06002EEC RID: 12012 RVA: 0x000DB390 File Offset: 0x000D9790
		public DateTime? UnlockDateForLevel(int level)
		{
			if (level < this.progressionService.UnlockedLevel)
			{
				return null;
			}
			// QuestData questData = this.questService.QuestForLevel(level);
			// return this.UnlockDateForQuest(questData.id);
			return null;
		}

		// Token: 0x06002EED RID: 12013 RVA: 0x000DB3D4 File Offset: 0x000D97D4
		public DateTime? UnlockDateForQuest(string questId)
		{
			// if (this.questService.IsCompleted(questId))
			// {
			// 	return null;
			// }
			if (this.isUnlockDisabled)
			{
				return null;
			}
			// QuestData currentQuestData = this.questService.questManager.CurrentQuestData;
			// if (currentQuestData != null && currentQuestData.id.Equals(questId))
			// {
				// return null;
			// }
			DateTime? result = this.sbsService.SbsConfig.content_unlock.UnlockDateFor(questId);
			if (result != null && this.timeService.IsTimeValid && result.Value < this.timeService.LocalNow)
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06002EEE RID: 12014 RVA: 0x000DB4A0 File Offset: 0x000D98A0
		public bool IsNextQuestLockedByDate()
		{
			return this.NextQuestUnlockDate() != null;
		}

		// Token: 0x06002EEF RID: 12015 RVA: 0x000DB4BC File Offset: 0x000D98BC
		public DateTime? NextQuestUnlockDate()
		{
			// QuestProgress nextAvailableQuest = this.questService.questManager.GetNextAvailableQuest();
			// string questId = (nextAvailableQuest == null) ? "end_of_content" : nextAvailableQuest.questID;
			// return this.UnlockDateForQuest(questId);
			return null;
		}

		// Token: 0x06002EF0 RID: 12016 RVA: 0x000DB4F8 File Offset: 0x000D98F8
		public int GetLockedByContentIslandId()
		{
			int num = -1;
			// if (this.questService.questManager.CurrentQuestData == null && !this.IsNextQuestLockedByDate())
			// {
			// 	int globalArea = this.progressionService.LastUnlockedArea + 1;
			// 	num = this.configService.SbsConfig.islandareaconfig.IslandForArea(globalArea);
			// 	if (this.configService.SbsConfig.islandareaconfig.FirstGlobalArea(num) > this.configService.general.tier_unlocked.last_area)
			// 	{
			// 		num = -1;
			// 	}
			// }
			return num;
		}

		// Token: 0x04005819 RID: 22553
		[WaitForService(true, true)]
		private ConfigService configService;

		// Token: 0x0400581A RID: 22554
		[WaitForService(true, true)]
		private M3ConfigService m3ConfigService;

		// Token: 0x0400581B RID: 22555
		[WaitForService(false, true)]
		private ProgressionDataService.Service progressionService;

		// Token: 0x0400581C RID: 22556
		// [WaitForService(false, true)]
		// private QuestService questService;

		// Token: 0x0400581D RID: 22557
		[WaitForService(true, true)]
		private SBSService sbsService;

		// Token: 0x0400581E RID: 22558
		[WaitForService(true, true)]
		private TimeService timeService;

		// Token: 0x0400581F RID: 22559
		private bool isUnlockDisabled;
	}
}
