using System;

// Token: 0x020008C6 RID: 2246
namespace Match3.Scripts1
{
	[Serializable]
	public class VillagerVillagerData
	{
		// Token: 0x1700085D RID: 2141
		// (get) Token: 0x060036B4 RID: 14004 RVA: 0x0010AA7C File Offset: 0x00108E7C
		public int Speaker
		{
			get
			{
				return (!(this.who == "chr1")) ? 1 : 0;
			}
		}

		// Token: 0x060036B5 RID: 14005 RVA: 0x0010AA9A File Offset: 0x00108E9A
		public bool VillagerLikesVillager(string a, string b)
		{
			return VillagerDecoData.Contains(a, this.chr1) && VillagerDecoData.Contains(b, this.chr2);
		}

		// Token: 0x04005EE1 RID: 24289
		public string dialogue_id;

		// Token: 0x04005EE2 RID: 24290
		public string chr1;

		// Token: 0x04005EE3 RID: 24291
		public string chr2;

		// Token: 0x04005EE4 RID: 24292
		public int sentence;

		// Token: 0x04005EE5 RID: 24293
		public string who;

		// Token: 0x04005EE6 RID: 24294
		public string text;

		// Token: 0x04005EE7 RID: 24295
		public string emo_reaction_chr1;

		// Token: 0x04005EE8 RID: 24296
		public string emo_reaction_chr2;

		// Token: 0x04005EE9 RID: 24297
		public int lvl_start;

		// Token: 0x04005EEA RID: 24298
		public int lvl_end;
	}
}
