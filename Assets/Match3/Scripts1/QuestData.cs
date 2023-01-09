using System;
using System.Collections.Generic;

// Token: 0x0200092C RID: 2348
namespace Match3.Scripts1
{
	[Serializable]
	public class QuestData
	{
		// Token: 0x170008C9 RID: 2249
		// (get) Token: 0x06003912 RID: 14610 RVA: 0x00119230 File Offset: 0x00117630
		public List<QuestTaskData> Tasks
		{
			get
			{
				if (this._tasks == null)
				{
					this._tasks = new List<QuestTaskData>();
					for (int i = 0; i < this.task_type.Length; i++)
					{
						this._tasks.Add(new QuestTaskData(this, i));
					}
				}
				return this._tasks;
			}
		}

		// Token: 0x04006167 RID: 24935
		public string character;

		// Token: 0x04006168 RID: 24936
		public string id;

		// Token: 0x04006169 RID: 24937
		public string loca_override;

		// Token: 0x0400616A RID: 24938
		public int level;

		// Token: 0x0400616B RID: 24939
		public int rewardCount;

		// Token: 0x0400616C RID: 24940
		public string rewardItem;

		// Token: 0x0400616D RID: 24941
		public string[] task_type;

		// Token: 0x0400616E RID: 24942
		public string[] task_levels;

		// Token: 0x0400616F RID: 24943
		public int[] task_count;

		// Token: 0x04006170 RID: 24944
		public string[] task_item;

		// Token: 0x04006171 RID: 24945
		public string[] task_action_target;

		// Token: 0x04006172 RID: 24946
		public bool data_override;

		// Token: 0x04006173 RID: 24947
		public string visual_order;

		// Token: 0x04006174 RID: 24948
		private List<QuestTaskData> _tasks;
	}
}
