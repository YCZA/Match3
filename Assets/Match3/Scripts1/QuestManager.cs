using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Puzzletown.Town;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.Services.ErrorAnalytics;
using Match3.Scripts1.Wooga.Signals;
using UnityEngine;

// Token: 0x02000930 RID: 2352
namespace Match3.Scripts1
{
	public class QuestManager
	{
		// Token: 0x06003918 RID: 14616 RVA: 0x0011932C File Offset: 0x0011772C
		public QuestManager(QuestData[] questConfigs, QuestsDataService questsData, QuestService questService)
		{
			this.questConfigs = questConfigs;
			this.questsData = questsData;
			this.questService = questService;
			if (this.questsProgressions.questProgressions.Count == 0 && this.questsProgressions.completedQuests.Count > 0)
			{
				QuestData quest = this.questConfigs.First((QuestData d) => d.id == this.questsProgressions.completedQuests[0]);
				QuestProgress item = QuestProgress.CreateCompleted(quest, QuestProgress.Status.completed);
				this.questsProgressions.questProgressions.Add(item);
			}
			while (this.questsProgressions.questProgressions.Count > 1)
			{
				this.questsProgressions.questProgressions.RemoveAt(1);
			}
			using (List<QuestProgress>.Enumerator enumerator = this.questsProgressions.questProgressions.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					QuestProgress questProgress = enumerator.Current;
					QuestData questData = this.questConfigs.FirstOrDefault((QuestData q) => q.id == questProgress.questID);
					if (questData != null && questData.data_override)
					{
						questProgress.configData = questData;
					}
				}
			}
		}

		// Token: 0x170008CA RID: 2250
		// (get) Token: 0x06003919 RID: 14617 RVA: 0x001194B0 File Offset: 0x001178B0
		private QuestProgressCollection questsProgressions
		{
			get
			{
				return this.questsData.Quests;
			}
		}

		// Token: 0x170008CB RID: 2251
		// (get) Token: 0x0600391A RID: 14618 RVA: 0x001194C0 File Offset: 0x001178C0
		public QuestProgress CurrentQuestProgress
		{
			get
			{
				string text = this.questsProgressions.completedQuests.FirstOrDefault<string>();
				QuestProgress questProgress = (from q in this.questsProgressions.questProgressions
					orderby (int)q.status descending
					select q).FirstOrDefault<QuestProgress>();
				if (text != null && questProgress != null)
				{
					QuestData questData = this[text];
					if (questProgress.questID != questData.id)
					{
						return QuestProgress.CreateCompleted(questData, QuestProgress.Status.completed);
					}
				}
				return questProgress;
			}
		}
	
		// Token: 0x170008CC RID: 2252
		// (get) Token: 0x0600391B RID: 14619 RVA: 0x00119544 File Offset: 0x00117944
		public QuestUIData CurrentQuestUIData
		{
			get
			{
				QuestProgress currentQuestProgress = this.CurrentQuestProgress;
				QuestData data = (currentQuestProgress == null) ? this.CurrentQuestData : currentQuestProgress.configData;
				return new QuestUIData(currentQuestProgress, data);
			}
		}

		// Token: 0x170008CD RID: 2253
		// (get) Token: 0x0600391C RID: 14620 RVA: 0x00119577 File Offset: 0x00117977
		public QuestData CurrentQuestData
		{
			get
			{
				return (this.CurrentQuestProgress != null) ? this.CurrentQuestProgress.configData : null;
			}
		}

		// Token: 0x0600391D RID: 14621 RVA: 0x00119595 File Offset: 0x00117995
		public void CollectQuestTask(QuestProgress progress, int idx)
		{
			// 601关是chp17_q4
			if (progress.configData.level < 600)
			{
				this.DoCollectTask(progress, idx);
			}
			else
			{
				// 可能是在检查相关bundle是否已下载
				WooroutineRunner.StartCoroutine(this.TryCollectQuestTaskRoutine(progress, idx), null);
			}
		}

		// Token: 0x0600391E RID: 14622 RVA: 0x001195C8 File Offset: 0x001179C8
		public void DoCollectTask(QuestProgress progress, int idx)
		{
			QuestTaskHandler questTaskHandler = this.taskHandlers[progress.configData.task_type[idx]];
			questTaskHandler.OnCollected(progress, idx); // 设置任务进度:已收集, 并移动相机, 显示修复动画，显示对话，注意OnCollected方法被CollectAndInteractTaskHandler和CollectAndRepairTaskHandler重写了
			// eli key point 显示对话（屏蔽对话）
			this.OnQuestTaskCompleted.Dispatch(progress, idx);	// 执行完小任务后的事件(应该是和对话有关的事件)
		
			this.CheckQuest(progress); // 检查该大任务下的几个小任务，如果已全完成，则执行些事件
		}

		// Token: 0x0600391F RID: 14623 RVA: 0x0011960C File Offset: 0x00117A0C
		public IEnumerator TryCollectQuestTaskRoutine(QuestProgress progress, int idx)
		{
			bool isLandAvailable = true;
			if (progress.configData.task_type[idx] != QuestTaskType.reach_rank.ToString())
			{
				Wooroutine<bool> isLandAvailableRoutine = this.questService.IsIslandBundleForQuestCompletionAvailable(progress, -1);
				yield return isLandAvailableRoutine;
				isLandAvailable = isLandAvailableRoutine.ReturnValue;
			}
			if (isLandAvailable)
			{
				this.DoCollectTask(progress, idx);
			}
			else
			{
				string missingScene = TownMainRoot.GetSceneNameForIsland(1);
				Log.Warning("[QuestManager] Missing scene", missingScene, null);
				yield return PopupMissingAssetsRoot.TryShowRoutine(missingScene);
			}
			yield break;
		}

		// Token: 0x06003920 RID: 14624 RVA: 0x00119638 File Offset: 0x00117A38
		private QuestProgress GetProgress(QuestData quest)
		{
			QuestProgress questProgress = this.questsProgressions.Get(quest.id);
			if (questProgress != null)
			{
				return questProgress;
			}
			if (this.questsProgressions.IsComplete(quest.id))
			{
				return QuestProgress.CreateCompleted(quest, QuestProgress.Status.completed);
			}
			if (this.questsProgressions.IsCollected(quest.id))
			{
				return QuestProgress.CreateCompleted(quest, QuestProgress.Status.collected);
			}
			return null;
		}

		// Token: 0x06003921 RID: 14625 RVA: 0x001196A0 File Offset: 0x00117AA0
		public bool GetTaskComplete(QuestProgress progress, int idx)
		{
			return progress == this.CurrentQuestProgress && (progress.tasksProgress[idx].collected || this.taskHandlers[this.CurrentQuestData.task_type[idx]].TaskComplete(progress, idx));
		}

		// Token: 0x06003922 RID: 14626 RVA: 0x001196EF File Offset: 0x00117AEF
		public bool GetTaskCollected(QuestProgress progress, int idx)
		{
			return progress.tasksProgress[idx].collected;
		}

		// Token: 0x06003923 RID: 14627 RVA: 0x00119700 File Offset: 0x00117B00
		public int GetTaskProgress(QuestProgress progress, int idx)
		{
			QuestData configData = progress.configData;
			return this.taskHandlers[configData.task_type[idx]].GetProgress(progress, idx);
		}

		// Token: 0x06003924 RID: 14628 RVA: 0x0011972E File Offset: 0x00117B2E
		public void RegisterQuestTaskHandler(QuestTaskHandler newTaskHandler)
		{
			if (this.taskHandlers.ContainsKey(newTaskHandler.GetTaskType()))
			{
				throw new InvalidOperationException("Manager already contains handler of type: " + newTaskHandler.GetTaskType());
			}
			this.taskHandlers[newTaskHandler.GetTaskType()] = newTaskHandler;
		}

		// Token: 0x06003925 RID: 14629 RVA: 0x0011976E File Offset: 0x00117B6E
		public List<T> GetTaskHandlers<T>() where T : QuestTaskHandler
		{
			return (from cTaskKV in this.taskHandlers
				select cTaskKV.Value).OfType<T>().ToList<T>();
		}

		// Token: 0x06003926 RID: 14630 RVA: 0x00119794 File Offset: 0x00117B94
		public virtual void ProcessFilteredTasks(Func<string, bool> pred, Func<QuestProgress, int, bool> taskComplete, Action<QuestProgress, int> taskHandler)
		{
			foreach (QuestProgress questProgress in this.questsProgressions.questProgressions)
			{
				bool flag = false;
				QuestData configData = questProgress.configData;
				if (questProgress.status == QuestProgress.Status.started)
				{
					for (int i = 0; i < configData.task_count.Length; i++)
					{
						if (pred(configData.task_type[i]))
						{
							if (!taskComplete(questProgress, i))
							{
								taskHandler(questProgress, i);
								flag = true;
								if (taskComplete(questProgress, i) && questProgress.tasksProgress[i].collected)
								{
									this.OnQuestTaskCompleted.Dispatch(questProgress, i);
								}
							}
						}
					}
					if (flag)
					{
						this.CheckQuest(questProgress);
					}
				}
			}
		}

		// Token: 0x06003927 RID: 14631 RVA: 0x00119898 File Offset: 0x00117C98
		protected void CheckQuest(QuestProgress questProgress)
		{
			if (questProgress == null)
			{
				return;
			}
			if (questProgress.tasksProgress == null)
			{
				return;
			}
			if (questProgress.status == QuestProgress.Status.completed)
			{
				return;
			}
			bool flag = true;
			QuestData configData = questProgress.configData;
			for (int i = 0; i < configData.task_count.Length; i++)
			{
				flag &= questProgress.tasksProgress[i] != null && questProgress.tasksProgress[i].collected;
			}
			if (flag)
			{
				// 几个小任务都完成了
				Debug.Log("几个小任务都完成了");
				questProgress.status = QuestProgress.Status.completed;
				this.OnQuestComplete.Dispatch(questProgress); // 弹出任务窗口，显示领取奖励按钮
				this.questsProgressions.SetCompleted(configData.id);
			}
		}

		// Token: 0x06003928 RID: 14632 RVA: 0x00119934 File Offset: 0x00117D34
		public int GetLevelForLastCollectedQuest()
		{
			int result = 0;
			if (this.questsProgressions.collectedQuests.Count > 0)
			{
				int index = this.questsProgressions.collectedQuests.Count - 1;
				string lastCompletedId = this.questsProgressions.collectedQuests[index];
				QuestData questData = this.questConfigs.FirstOrDefault((QuestData q) => q.id == lastCompletedId);
				if (questData != null)
				{
					result = questData.level;
				}
			}
			return result;
		}

		// Token: 0x06003929 RID: 14633 RVA: 0x001199B0 File Offset: 0x00117DB0
		public bool TryStartNextQuest()
		{
			QuestProgress nextAvailableQuest = this.GetNextAvailableQuest();
			if (nextAvailableQuest != null)
			{
				this.StartQuest(nextAvailableQuest);
			}
			return nextAvailableQuest != null;
		}

		// Token: 0x0600392A RID: 14634 RVA: 0x001199D8 File Offset: 0x00117DD8
		public QuestProgress GetNextAvailableQuest()
		{
			QuestData questData = this.questConfigs.FirstOrDefault((QuestData q) => !this.IsCollected(q.id));
			return (questData == null) ? null : QuestProgress.Create(questData);
		}

		// Token: 0x0600392B RID: 14635 RVA: 0x00119A0F File Offset: 0x00117E0F
		private bool IsCollected(string id)
		{
			return this.questsProgressions.collectedQuests.Contains(id);
		}

		// Token: 0x0600392C RID: 14636 RVA: 0x00119A24 File Offset: 0x00117E24
		public IEnumerable<QuestUIData> Filter(QuestProgress.Status status)
		{
			return from cQuest in this.questConfigs
				select new
				{
					cQuest = cQuest,
					questProgress = this.GetProgress(cQuest)
				} into __TranspIdent0
				where __TranspIdent0.questProgress != null && (__TranspIdent0.questProgress.status & status) != QuestProgress.Status.undefined
				select new QuestUIData(__TranspIdent0.questProgress, __TranspIdent0.cQuest);
		}

		// Token: 0x0600392D RID: 14637 RVA: 0x00119A8F File Offset: 0x00117E8F
		public IEnumerable<QuestUIData> QuestAndProgress()
		{
			return from cQuest in this.questConfigs
				select new
				{
					cQuest = cQuest,
					questProgress = this.GetProgress(cQuest)
				} into __TranspIdent1
				select new QuestUIData(__TranspIdent1.questProgress, __TranspIdent1.cQuest);
		}

		// Token: 0x170008CE RID: 2254
		// (get) Token: 0x0600392E RID: 14638 RVA: 0x00119ACA File Offset: 0x00117ECA
		public IEnumerable<QuestData> QuestConfigs
		{
			get
			{
				return this.questConfigs;
			}
		}

		// Token: 0x170008CF RID: 2255
		public QuestData this[string questID]
		{
			get
			{
				return this.questConfigs.FirstOrDefault((QuestData cQuest) => cQuest.id == questID);
			}
		}

		// Token: 0x06003930 RID: 14640 RVA: 0x00119B08 File Offset: 0x00117F08
		private void StartQuest(QuestProgress currentProgress)
		{
			global::UnityEngine.Debug.Log("Starting a new quest");
			if (currentProgress.status < QuestProgress.Status.started)
			{
				this.questsProgressions.Add(currentProgress.questID, currentProgress);
				currentProgress.status = QuestProgress.Status.started;
				if (currentProgress.configData == null)
				{
					WoogaDebug.LogWarning(new object[]
					{
						"Found quest progress without data, assigning now"
					});
					QuestData configData = this.questConfigs.First((QuestData q) => q.id == currentProgress.questID);
					currentProgress.configData = configData;
				}
				this.OnQuestChanged.Dispatch(currentProgress);
			}
		}

		// Token: 0x06003931 RID: 14641 RVA: 0x00119BC0 File Offset: 0x00117FC0
		public void CollectQuest(QuestProgress questProgress)
		{
			if (questProgress.status < QuestProgress.Status.collected)
			{
				QuestData questData = this[questProgress.questID];
				if (questData != null)
				{
					questProgress.status = QuestProgress.Status.collected;
					this.OnQuestCollected.Dispatch(questProgress);
					this.questsProgressions.SetCollected(questData.id);
				}
				else
				{
					WoogaDebug.LogError(new object[]
					{
						"questProgress referes to a non existent quest: " + questProgress.questID
					});
				}
			}
		}

		// Token: 0x06003932 RID: 14642 RVA: 0x00119C35 File Offset: 0x00118035
		public bool IsLastQuest(string value)
		{
			return this.questConfigs.Last<QuestData>().id == value;
		}

		// Token: 0x06003933 RID: 14643 RVA: 0x00119C4D File Offset: 0x0011804D
		public void TriggerAction(IQuestAction questAction)
		{
			this.OnQuestActionTriggers.Dispatch(questAction);
		}

		// Token: 0x06003934 RID: 14644 RVA: 0x00119C5C File Offset: 0x0011805C
		public void DebugFinishQuest()
		{
			WoogaDebug.Log(new object[]
			{
				this.questsProgressions.questProgressions.Count
			});
			QuestProgress currentQuestProgress = this.CurrentQuestProgress;
			if (currentQuestProgress != null)
			{
				currentQuestProgress.status = QuestProgress.Status.completed;
				QuestData questData = this[currentQuestProgress.questID];
				this.questsProgressions.SetCompleted(questData.id);
				this.CollectQuest(currentQuestProgress);
			}
		}

		// Token: 0x04006183 RID: 24963
		private readonly Dictionary<string, QuestTaskHandler> taskHandlers = new Dictionary<string, QuestTaskHandler>();

		// Token: 0x04006184 RID: 24964
		public readonly Signal<QuestProgress> OnQuestChanged = new Signal<QuestProgress>();

		// Token: 0x04006185 RID: 24965
		public readonly Signal<QuestProgress> OnQuestComplete = new Signal<QuestProgress>();

		// Token: 0x04006186 RID: 24966
		public readonly Signal<QuestProgress> OnQuestCollected = new Signal<QuestProgress>();

		// Token: 0x04006187 RID: 24967
		public readonly Signal<QuestProgress, int> OnQuestTaskCompleted = new Signal<QuestProgress, int>();

		// Token: 0x04006188 RID: 24968
		public readonly Signal<IQuestAction> OnQuestActionTriggers = new Signal<IQuestAction>();

		// Token: 0x04006189 RID: 24969
		private readonly QuestData[] questConfigs;

		// Token: 0x0400618A RID: 24970
		private readonly QuestsDataService questsData;

		// Token: 0x0400618B RID: 24971
		private readonly QuestService questService;
	}
}
