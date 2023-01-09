using System;
using System.Collections;
using Match3.Scripts1.Puzzletown.Flows;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Wooga.Signals;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Town
{
	// Token: 0x020008BD RID: 2237
	public class StoryController : MonoBehaviour
	{
		// Token: 0x17000857 RID: 2135
		// (get) Token: 0x06003694 RID: 13972 RVA: 0x0010A37C File Offset: 0x0010877C
		public bool HasStoryRunning
		{
			get
			{
				return this.m_currentFlow != null && this.m_currentFlow.isActive;
			}
		}

		// eli key point 故事控制初始化
		public void Init(ConfigService config, QuestManager quests, ProgressionDataService.Service progression)
		{
			this.quests = quests;
			this.progression = progression;
			this.story = config.storyDialogue;
			this.spawnConfigurations = base.GetComponentsInChildren<VillagerSpawnConfiguration>();
			quests.OnQuestTaskCompleted.AddListener(new Action<QuestProgress, int>(this.HandleQuestTaskCompleted));
			quests.OnQuestCollected.AddListener(new Action<QuestProgress>(this.HandleQuestCollected));
			progression.onTierCompleted.AddListener(new Action<Level>(this.HandleTierCompleted));
		}

		// Token: 0x06003696 RID: 13974 RVA: 0x0010A410 File Offset: 0x00108810
		private void EnqueueStoryDialogue(DialogueTrigger trigger, string value)
		{
			// 审核版没有对话
			// #if REVIEW_VERSION
				// return;
			// #endif
			this.EnqueueStoryDialogue(BlockerManager.global, trigger, value);
		}

		// Token: 0x06003697 RID: 13975 RVA: 0x0010A420 File Offset: 0x00108820
		public IEnumerator StartAndAwaitStoryFlow(BlockerManager blockerManager, DialogueTrigger trigger, string value)
		{
			// 审核版没有对话
			// #if REVIEW_VERSION
				// yield break;
			// #endif
			StoryDialogueFlow story = this.EnqueueStoryDialogue(blockerManager, trigger, value);
			if (story != null)
			{
				yield return story.onCompleted;
			}
		}

		// Token: 0x06003698 RID: 13976 RVA: 0x0010A450 File Offset: 0x00108850
		private StoryDialogueFlow EnqueueStoryDialogue(BlockerManager blockerManager, DialogueTrigger trigger, string value)
		{
			Debug.Log("EnqueueStoryDialogue");
			DialogueSetupData dialogueSetupData = this.story.FindDialogue(trigger, value);
			if (dialogueSetupData == null)
			{
				WoogaDebug.Log(new object[]
				{
					"no dialogue found for",
					trigger,
					value
				});
				return null;
			}
			Debug.Log("append dialog: " + dialogueSetupData.dialogue_id + " " + dialogueSetupData.trigger_value);
			blockerManager.Append(this.m_currentFlow = new StoryDialogueFlow(dialogueSetupData));
			return this.m_currentFlow;
		}

		// Token: 0x06003699 RID: 13977 RVA: 0x0010A4B0 File Offset: 0x001088B0
		public IEnumerator PlayStoryDialogImmediate(DialogueTrigger trigger, string value)
		{
			Debug.Log("PlayStoryDialogImmediate");
			DialogueSetupData data = this.story.FindDialogue(value);
			if (data != null)
			{
				this.m_currentFlow = new StoryDialogueFlow(data);
				yield return this.m_currentFlow.ExecuteRoutine();
			}
			else
			{
				WoogaDebug.LogWarning(new object[]
				{
					"couldn't find dialogue",
					trigger,
					value
				});
			}
			yield break;
		}

		// Token: 0x0600369A RID: 13978 RVA: 0x0010A4DC File Offset: 0x001088DC
		public VillagerSpawnConfiguration FindSpawnConfiguration(int villagersCount)
		{
			return Array.Find<VillagerSpawnConfiguration>(this.spawnConfigurations, (VillagerSpawnConfiguration cfg) => cfg.SpawnPoints.Length == villagersCount);
		}

		// Token: 0x0600369B RID: 13979 RVA: 0x0010A50D File Offset: 0x0010890D
		private void HandleTierCompleted(Level level)
		{
			Debug.LogError("level_completed: " + level.ToString());
			this.EnqueueStoryDialogue(DialogueTrigger.level_completed, level.ToString());
		}

		// Token: 0x0600369C RID: 13980 RVA: 0x0010A524 File Offset: 0x00108924
		private void HandleQuestCollected(QuestProgress progress)
		{
			QuestData configData = progress.configData;
			this.EnqueueStoryDialogue(DialogueTrigger.quest_completed, configData.id);
		}

		// Token: 0x0600369D RID: 13981 RVA: 0x0010A548 File Offset: 0x00108948
		private void HandleQuestTaskCompleted(QuestProgress progress, int idx)
		{
			QuestData configData = progress.configData;
			int num = idx + 1;
			this.EnqueueStoryDialogue(DialogueTrigger.quest_task_completed, configData.id + "." + num);
		}

		// Token: 0x0600369E RID: 13982 RVA: 0x0010A580 File Offset: 0x00108980
		protected void OnDestroy()
		{
			if (this.quests != null)
			{
				this.quests.OnQuestTaskCompleted.RemoveListener(new Action<QuestProgress, int>(this.HandleQuestTaskCompleted));
				this.quests.OnQuestCollected.RemoveListener(new Action<QuestProgress>(this.HandleQuestCollected));
			}
			if (this.progression != null)
			{
				this.progression.onTierCompleted.RemoveListener(new Action<Level>(this.HandleTierCompleted));
			}
		}

		// Token: 0x04005EA4 RID: 24228
		private const float CAMERA_PAN_TIME = 1f;

		// Token: 0x04005EA5 RID: 24229
		private VillagerSpawnConfiguration[] spawnConfigurations;

		// Token: 0x04005EA6 RID: 24230
		private StoryDialogueConfig story;

		// Token: 0x04005EA7 RID: 24231
		private QuestManager quests;

		// Token: 0x04005EA8 RID: 24232
		private ProgressionDataService.Service progression;

		// Token: 0x04005EA9 RID: 24233
		private StoryDialogueFlow m_currentFlow;

		// Token: 0x04005EAA RID: 24234
		public readonly Signal<string> onStoryFinished = new Signal<string>();
	}
}
