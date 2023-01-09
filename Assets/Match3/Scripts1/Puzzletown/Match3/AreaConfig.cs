using System;
using System.Reflection;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020004E9 RID: 1257
	[Serializable]
	public class AreaConfig : ALevelCollectionConfig
	{
		// Token: 0x060022CC RID: 8908 RVA: 0x0009A36C File Offset: 0x0009876C
		public AreaConfig(int level)
		{
			this.level = level;
			this.tiers = new TierConfig[3];
		}

		// Token: 0x060022CD RID: 8909 RVA: 0x0009A388 File Offset: 0x00098788
		public AreaConfig(AreaConfigExcel config, bool canBeLevelOfDay)
		{
			this.level = config.level_number;
			this.file_name = config.file_name;
			this.unlocked_at_quest_completed = config.unlocked_at_quest_completed;
			this.wait_for_dialog = config.wait_for_dialog;
			this.objective = config.objective;
			this.lod = canBeLevelOfDay;
			this.tiers = new TierConfig[3];
			AreaConfig.Tier[] array = (AreaConfig.Tier[])Enum.GetValues(typeof(AreaConfig.Tier));
			for (int i = 0; i < 3; i++)
			{
				FieldInfo field = config.GetType().GetField("diamonds_" + array[i]);
				this.tiers[i] = new TierConfig(array[i], (int)field.GetValue(config));
			}
		}

		// Token: 0x04004E7B RID: 20091
		public string file_name;

		// Token: 0x04004E7C RID: 20092
		public string unlocked_at_quest_completed;

		// Token: 0x04004E7D RID: 20093
		public string wait_for_dialog;

		// Token: 0x04004E7E RID: 20094
		public bool lod;

		// Token: 0x020004EA RID: 1258
		public enum Tier
		{
			// Token: 0x04004E80 RID: 20096
			a,
			// Token: 0x04004E81 RID: 20097
			b,
			// Token: 0x04004E82 RID: 20098
			c,
			// Token: 0x04004E83 RID: 20099
			Count,
			// Token: 0x04004E84 RID: 20100
			undefined
		}
	}
}
