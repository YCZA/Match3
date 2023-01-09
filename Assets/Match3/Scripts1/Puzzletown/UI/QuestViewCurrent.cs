using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Match3.Scripts1.UnityEngine;
using Wooga.Coroutines;
using UnityEngine;
using UnityEngine.UI;

namespace Match3.Scripts1.Puzzletown.UI
{
	// Token: 0x02000A0B RID: 2571
	public class QuestViewCurrent : QuestView, IHandler<PopupOperation>
	{
		// Token: 0x17000922 RID: 2338
		// (get) Token: 0x06003DE4 RID: 15844 RVA: 0x00139793 File Offset: 0x00137B93
		public override QuestView.CellType questStatus
		{
			get
			{
				return QuestView.CellType.current;
			}
		}

		// Token: 0x06003DE5 RID: 15845 RVA: 0x00139798 File Offset: 0x00137B98
		public override void Show(QuestUIData data)
		{
			base.Show(data);
			if (base.root == null)
			{
				return;
			}
			string text = base.root.loc.GetText(base.GetTitleKey(base.Quest), new LocaParam[0]);
			string text2 = base.root.loc.GetText(base.GetDescriptionKey(base.Quest), new LocaParam[0]);
			PopupTextCell.SetText(this, TextType.Title, text);
			PopupTextCell.SetText(this, TextType.Content, text2);
			this.claimLabel.text = base.root.loc.GetText(base.GetClaimKey(base.Quest), new LocaParam[0]);
			// eli key point: 显示任务信息
			this.RefreshTaskItems();
			MaterialAmount[] reward = new MaterialAmount[]
			{
				new MaterialAmount(base.Quest.rewardItem, base.Quest.rewardCount, MaterialAmountUsage.Undefined, 0)
			};
			foreach (QuestRewardView questRewardView in base.GetComponentsInChildren<QuestRewardView>(true))
			{
				questRewardView.SetVisibility((questRewardView.state & base.Progress.status) != QuestProgress.Status.undefined);
				questRewardView.ExecuteOnChild(delegate(MaterialsDataSource source)
				{
					source.Show(reward);
				});
			}
			base.transform.HandleOnParent(data.data);
		}

		// Token: 0x06003DE6 RID: 15846 RVA: 0x001398EC File Offset: 0x00137CEC
		private void RefreshTaskItems()
		{
			IEnumerable<QuestViewCurrent.TaskProgressViewData> value = this.EnumerateTasks(base.Progress);
			this.ShowOnChildren(value, false, true);
		}

		// Token: 0x06003DE7 RID: 15847 RVA: 0x00139910 File Offset: 0x00137D10
		private IEnumerable<QuestViewCurrent.TaskProgressViewData> EnumerateTasks(QuestProgress progress)
		{
			List<QuestViewCurrent.TaskProgressViewData> taskList = new List<QuestViewCurrent.TaskProgressViewData>();
			int taskIdx = 0;
			QuestData quest = progress.configData;
			foreach (QuestTaskData questTaskData in quest.Tasks)
			{
				bool taskComplete = base.root.quests.questManager.GetTaskComplete(progress, taskIdx);
				int taskProgress = base.root.quests.questManager.GetTaskProgress(progress, taskIdx);
				QuestTaskData taskData = questTaskData;
				string label = this.StringForQuestTask(quest, questTaskData.index);
				QuestViewCurrent.TaskProgressViewData item = new QuestViewCurrent.TaskProgressViewData(taskData, progress, taskProgress, label, taskComplete);
				taskList.Add(item);
				taskIdx++;
			}
			int[] array;
			if (string.IsNullOrEmpty(progress.configData.visual_order))
			{
				array = new int[0];
			}
			else
			{
				IEnumerable<string> source = progress.configData.visual_order.Split(new char[]
				{
					','
				});
				if (QuestViewCurrent._003C_003Ef__mg_0024cache0 == null)
				{
					QuestViewCurrent._003C_003Ef__mg_0024cache0 = new Func<string, int>(Convert.ToInt32);
				}
				array = source.Select(QuestViewCurrent._003C_003Ef__mg_0024cache0).ToArray<int>();
			}
			int[] taskOrders = array;
			for (int idx = 0; idx < taskOrders.Length; idx++)
			{
				yield return taskList[taskOrders[idx]];
			}
			for (int idx2 = 0; idx2 < taskList.Count; idx2++)
			{
				if (!taskOrders.Contains(idx2))
				{
					yield return taskList[idx2];
				}
			}
			yield break;
		}

		// Token: 0x06003DE8 RID: 15848 RVA: 0x0013993C File Offset: 0x00137D3C
		private string StringForQuestTask(QuestData quest, int taskIDX)
		{
			string text = string.Format("quest.task{0}.action.{1}", taskIDX + 1, quest.id);
			if (!quest.loca_override.IsNullOrEmpty())
			{
				text = text + "." + quest.loca_override;
			}
			string text2 = base.root.loc.GetText(text, new LocaParam[0]);
			if (text2.Contains("{0}"))
			{
				text2 = string.Format(text2, quest.task_count[taskIDX]);
			}
			return text2;
		}

		// Token: 0x06003DE9 RID: 15849 RVA: 0x001399C4 File Offset: 0x00137DC4
		public void ActionButton(QuestViewCurrent.TaskProgressViewData taskProgressViewData)
		{
			if (!taskProgressViewData.isComplete)
			{
				// 没有完成时
				base.GetComponentInParent<QuestsPopupRoot>().HandleShowMeButton(taskProgressViewData);
			}
			else if (!taskProgressViewData.IsCollected)
			{
				// 已完成时
				if (this.actionButtonDisabled)
				{
					return;
				}
				// 已完成时
				// 执行完成任务后的事件，如关闭窗口，播放声音
				base.GetComponentInParent<QuestsPopupRoot>().CollectTask(taskProgressViewData.progress, taskProgressViewData.taskIdx);
				// eli todo (如何没有剧情的话)不需要等1秒
				// WooroutineRunner.StartCoroutine(this.WaitToEnableButton(1f), null);
				WooroutineRunner.StartCoroutine(this.WaitToEnableButton(0.15f), null);
			}
			this.RefreshTaskItems();
		}

		// Token: 0x06003DEA RID: 15850 RVA: 0x00139A34 File Offset: 0x00137E34
		private IEnumerator WaitToEnableButton(float seconds)
		{
			this.actionButtonDisabled = true;
			yield return new WaitForSeconds(seconds);
			this.actionButtonDisabled = false;
			yield break;
		}

		// Token: 0x06003DEB RID: 15851 RVA: 0x00139A58 File Offset: 0x00137E58
		public void Handle(PopupOperation op)
		{
			if (op == PopupOperation.OK)
			{
				// 完成一组任务时
				Debug.Log("完成一组任务");
				if (this.actionButtonDisabled)
				{
					return;
				}
				base.root.CollectAndCloseMaterials(base.Progress);
				WooroutineRunner.StartCoroutine(this.WaitToEnableButton(2f), null);
			}
		}

		// Token: 0x040066D0 RID: 26320
		public Text claimLabel;

		// Token: 0x040066D1 RID: 26321
		private bool actionButtonDisabled;

		// Token: 0x040066D2 RID: 26322
		[CompilerGenerated]
		private static Func<string, int> _003C_003Ef__mg_0024cache0;

		// Token: 0x02000A0C RID: 2572
		public class TaskProgressViewData : QuestDataProgress
		{
			// Token: 0x06003DEC RID: 15852 RVA: 0x00139AA5 File Offset: 0x00137EA5
			public TaskProgressViewData(QuestTaskData taskData, QuestProgress progress, int countProgress, string label, bool complete) : base(taskData, progress, complete)
			{
				this.label = label;
				this.countProgress = countProgress;
			}

			// Token: 0x040066D3 RID: 26323
			public string label;

			// Token: 0x040066D4 RID: 26324
			public int countProgress;
		}
	}
}
