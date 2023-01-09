using System;
using System.Collections.Generic;
using System.Linq;
using Match3.Scripts1.Puzzletown.Config;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Wooga.UI;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020006E7 RID: 1767
	public class M3_LevelSelectionDataSource : ATableViewDataSource<M3LevelSelectionMapItem, LevelSelectionData>
	{
		// Token: 0x06002BF1 RID: 11249 RVA: 0x000C9EF0 File Offset: 0x000C82F0
		public void Init(ConfigService configService, QuestService questService, ProgressionDataService.Service progressionService, M3ConfigService m3ConfigService, IFacebookFriendsList facebookFriends, SBSService sbsService, ContentUnlockService contentUnlockService)
		{
			this.configService = configService;
			this.questService = questService;
			this.progressionService = progressionService;
			this.m3ConfigService = m3ConfigService;
			this.facebookFriends = facebookFriends;
			this.sbsService = sbsService;
			this.contentUnlockService = contentUnlockService;
			this.currentQuestData = this.questService.questManager.CurrentQuestData;
			// eli key point 在关卡列表显示的关卡数
			this.endOfContent = contentUnlockService.EndOfContentLevel();
			this.levelMode = ((!this.AreTiersUnlocked()) ? LevelSelectionData.LevelMode.SingleTier : LevelSelectionData.LevelMode.Normal);
			this.levels = this.GetLevels();
			this.tableView.Reload();
		}

		// Token: 0x06002BF2 RID: 11250 RVA: 0x000C9F84 File Offset: 0x000C8384
		private List<AreaConfig> GetLevels()
		{
			List<AreaConfig> list = new List<AreaConfig>();
			AreaConfig item = new AreaConfig(this.endOfContent);
			bool flag = false;
			for (int i = 0; i < this.configService.general.tier_unlocked.last_area; i++)
			{
				foreach (AreaConfig areaConfig in this.configService.areas.areas[i].levels)
				{
					if (this.contentUnlockService.UnlockDateForLevel(areaConfig.level) != null)
					{
						item = areaConfig;
						flag = true;
						break;
					}
					list.Add(areaConfig);
				}
				if (flag)
				{
					break;
				}
			}
			list.Add(item);
			foreach (int num in this.GetChapterEnds())
			{
				int num2 = num - 1;
				if (num2 <= list.Count)
				{
					list.Insert(num2, list[num2]);
				}
			}
			return list;
		}

		// Token: 0x06002BF3 RID: 11251 RVA: 0x000CA0DC File Offset: 0x000C84DC
		private IEnumerable<int> GetChapterEnds()
		{
			yield return this.endOfContent;
			for (int i = this.configService.chapter.chapters.Length - 1; i > 0; i--)
			{
				int level = this.configService.chapter.chapters[i].first_level;
				if (level < this.endOfContent)
				{
					yield return level;
				}
			}
			yield break;
		}

		// Token: 0x06002BF4 RID: 11252 RVA: 0x000CA0FF File Offset: 0x000C84FF
		private bool AreTiersUnlocked()
		{
			return this.progressionService.UnlockedLevel >= this.configService.general.tier_unlocked.unlock_tier_c_at_level;
		}

		// Token: 0x06002BF5 RID: 11253 RVA: 0x000CA126 File Offset: 0x000C8526
		private bool IsLocked(AreaConfig level)
		{
			return this.progressionService.IsLocked(level.level) || this.m3ConfigService.IsLockedByQuest(level);
		}

		// Token: 0x06002BF6 RID: 11254 RVA: 0x000CA150 File Offset: 0x000C8550
		private string GetCollectable(AreaConfig level, int tier, QuestData questData)
		{
			string result = string.Empty;
			if (!this.IsLocked(level))
			{
				for (int i = tier; i < level.tiers.Length; i++)
				{
					string name = level.tiers[i].name;
					string value = level.level + name;
					if (questData != null)
					{
						for (int j = 0; j < questData.Tasks.Count; j++)
						{
							QuestTaskData questTaskData = questData.Tasks[j];
							if (questTaskData.levels.Contains(value))
							{
								result = questTaskData.item;
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06002BF7 RID: 11255 RVA: 0x000CA200 File Offset: 0x000C8600
		public override LevelSelectionData GetDataForIndex(int index)
		{
			bool flag = index < 5;
			AreaConfig level = this.GetLevelForIndex(Mathf.Max(0, index - 5));
			bool islockedByEndOfContent = this.contentUnlockService.IsLockedByEndOfContent(level.level);
			DateTime? unlockDate = this.contentUnlockService.UnlockDateForLevel(level.level);
			bool flag3 = unlockDate != null;
			int tier = Mathf.Clamp(this.progressionService.GetTier(level.level) + 1, 0, 3);
			string collectable = (!islockedByEndOfContent && !flag3) ? this.GetCollectable(level, tier, this.currentQuestData) : string.Empty;
			LevelSelectionData.LockReason lockReason = LevelSelectionData.LockReason.NotLocked;
			LevelSelectionData.LevelOrder levelOrder = LevelSelectionData.LevelOrder.Normal;
			List<LevelForeshadowingConfig.ForeshadowingLevelConfig> list = this.sbsService.SbsConfig.levelforeshadowingconfig.level_config.FindAll((LevelForeshadowingConfig.ForeshadowingLevelConfig config) => config.level == level.level);
			if (flag)
			{
				list.Clear();
			}
			if (islockedByEndOfContent)
			{
				lockReason = LevelSelectionData.LockReason.ComingSoon;
			}
			else if (flag3)
			{
				lockReason = LevelSelectionData.LockReason.DateLocked;
				unlockDate = this.contentUnlockService.UnlockDateForLevel(level.level);
			}
			else if (this.m3ConfigService.IsLockedByQuest(level))
			{
				lockReason = LevelSelectionData.LockReason.Quest;
			}
			else if (this.IsLocked(level) || (level.level == this.progressionService.UnlockedLevel && flag))
			{
				lockReason = LevelSelectionData.LockReason.NotReached;
			}
			if (level.level == this.progressionService.UnlockedLevel)
			{
				levelOrder = LevelSelectionData.LevelOrder.Latest;
			}
			else if (level.level + 1 == this.progressionService.UnlockedLevel)
			{
				levelOrder = LevelSelectionData.LevelOrder.NextLatest;
			}
			LevelSelectionData result = new LevelSelectionData(level, tier, flag, false, collectable, lockReason, levelOrder, this.levelMode, this.facebookFriends, list, this.sbsService.SbsConfig.feature_switches, unlockDate);
			if (!flag && this.GetLevelForIndex(index - 5 - 1) == level)
			{
				if (this.progressionService.IsLocked(level.level))
				{
					result.lockReason = LevelSelectionData.LockReason.NotReached;
				}
				else
				{
					result.lockReason = LevelSelectionData.LockReason.NotLocked;
				}
				if (level.level == this.progressionService.UnlockedLevel)
				{
					result.levelOrder = LevelSelectionData.LevelOrder.NextLatest;
				}
				else
				{
					result.levelOrder = LevelSelectionData.LevelOrder.Normal;
				}
				result.isSeparator = true;
			}
			return result;
		}

		// Token: 0x06002BF8 RID: 11256 RVA: 0x000CA474 File Offset: 0x000C8874
		private AreaConfig GetLevelForIndex(int index)
		{
			int num = this.levels.Count - index - 1;
			return (num >= this.levels.Count) ? null : this.levels[num];
		}

		// Token: 0x06002BF9 RID: 11257 RVA: 0x000CA4B4 File Offset: 0x000C88B4
		public override int GetNumberOfCellsForTableView()
		{
			return (this.levels != null) ? (this.levels.Count + 5) : 0;
		}

		// Token: 0x06002BFA RID: 11258 RVA: 0x000CA4D4 File Offset: 0x000C88D4
		public override int GetReusableIdForIndex(int index)
		{
			int numberOfCellsForTableView = this.GetNumberOfCellsForTableView();
			index = numberOfCellsForTableView - index - 1;
			if (index <= 0)
			{
				return 100;
			}
			return (index - 1) % (this.prototypeCells.Length - 1);
		}

		// Token: 0x04005510 RID: 21776
		private const int FILLER_CELLS_COUNT = 5;

		// Token: 0x04005511 RID: 21777
		private List<AreaConfig> levels;

		// Token: 0x04005512 RID: 21778
		private ConfigService configService;

		// Token: 0x04005513 RID: 21779
		private QuestService questService;

		// Token: 0x04005514 RID: 21780
		private ProgressionDataService.Service progressionService;

		// Token: 0x04005515 RID: 21781
		private M3ConfigService m3ConfigService;

		// Token: 0x04005516 RID: 21782
		private IFacebookFriendsList facebookFriends;

		// Token: 0x04005517 RID: 21783
		private SBSService sbsService;

		// Token: 0x04005518 RID: 21784
		private ContentUnlockService contentUnlockService;

		// Token: 0x04005519 RID: 21785
		private LevelSelectionData.LevelMode levelMode;

		// Token: 0x0400551A RID: 21786
		private int endOfContent;

		// Token: 0x0400551B RID: 21787
		private QuestData currentQuestData;
	}
}
