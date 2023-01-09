using System;

// Token: 0x0200092B RID: 2347
namespace Match3.Scripts1
{
	public class QuestTaskData
	{
		// Token: 0x0600390B RID: 14603 RVA: 0x001190F4 File Offset: 0x001174F4
		public QuestTaskData(QuestData data, int index)
		{
			this.data = data;
			this.index = index;
		}

		// Token: 0x170008C4 RID: 2244
		// (get) Token: 0x0600390C RID: 14604 RVA: 0x0011910C File Offset: 0x0011750C
		public int count
		{
			get
			{
				int num = this.levels.Length;
				if (num > 0)
				{
					return num;
				}
				return this.data.task_count[this.index];
			}
		}

		// Token: 0x170008C5 RID: 2245
		// (get) Token: 0x0600390D RID: 14605 RVA: 0x0011913D File Offset: 0x0011753D
		public string item
		{
			get
			{
				return this.data.task_item[this.index];
			}
		}

		// Token: 0x170008C6 RID: 2246
		// (get) Token: 0x0600390E RID: 14606 RVA: 0x00119151 File Offset: 0x00117551
		public string action_target
		{
			get
			{
				return this.data.task_action_target[this.index];
			}
		}

		// Token: 0x170008C7 RID: 2247
		// (get) Token: 0x0600390F RID: 14607 RVA: 0x00119168 File Offset: 0x00117568
		public string[] levels
		{
			get
			{
				if (this._cachedLevels == null)
				{
					if (this.data.task_levels[this.index].ToLower() == "none")
					{
						this._cachedLevels = new string[0];
					}
					else
					{
						this._cachedLevels = this.data.task_levels[this.index].Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
					}
				}
				return this._cachedLevels;
			}
		}

		// Token: 0x170008C8 RID: 2248
		// (get) Token: 0x06003910 RID: 14608 RVA: 0x001191E5 File Offset: 0x001175E5
		public QuestTaskType type
		{
			get
			{
				if (this._type == QuestTaskType.unknown)
				{
					this._type = (QuestTaskType)Enum.Parse(typeof(QuestTaskType), this.data.task_type[this.index], true);
				}
				return this._type;
			}
		}

		// Token: 0x04006163 RID: 24931
		public readonly QuestData data;

		// Token: 0x04006164 RID: 24932
		public readonly int index;

		// Token: 0x04006165 RID: 24933
		private string[] _cachedLevels;

		// Token: 0x04006166 RID: 24934
		private QuestTaskType _type;
	}
}
