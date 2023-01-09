using System;
using UnityEngine;

// Token: 0x020008C5 RID: 2245
namespace Match3.Scripts1
{
	[Serializable]
	public class VillagerPlayerData
	{
		// Token: 0x1700085C RID: 2140
		// (get) Token: 0x060036B2 RID: 14002 RVA: 0x0010AA10 File Offset: 0x00108E10
		public VillagerPlayerTrigger Trigger
		{
			get
			{
				if (string.IsNullOrEmpty(this.trigger))
				{
					return VillagerPlayerTrigger.unknown;
				}
				VillagerPlayerTrigger result;
				try
				{
					result = EnumExtensions.EnumParse<VillagerPlayerTrigger>(this.trigger, true);
				}
				catch (ArgumentException)
				{
					WoogaDebug.LogWarningFormatted("Could not find VillagerPlayerTrigger {0}", new object[]
					{
						this.trigger
					});
					result = VillagerPlayerTrigger.unknown;
				}
				return result;
			}
		}

		// Token: 0x04005ED9 RID: 24281
		public string interaction_id;

		// Token: 0x04005EDA RID: 24282
		public int sentence;

		// Token: 0x04005EDB RID: 24283
		[SerializeField]
		private string trigger;

		// Token: 0x04005EDC RID: 24284
		public string text;

		// Token: 0x04005EDD RID: 24285
		public string reaction;

		// Token: 0x04005EDE RID: 24286
		public string character;

		// Token: 0x04005EDF RID: 24287
		public int lvl_start;

		// Token: 0x04005EE0 RID: 24288
		public int lvl_end;
	}
}
