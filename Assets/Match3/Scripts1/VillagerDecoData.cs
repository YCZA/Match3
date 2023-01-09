using System;

// Token: 0x020008C3 RID: 2243
namespace Match3.Scripts1
{
	[Serializable]
	public class VillagerDecoData
	{
		// Token: 0x060036AF RID: 13999 RVA: 0x0010A9AF File Offset: 0x00108DAF
		public bool VillagerLikesBuilding(VillagerData villager, BuildingConfig building)
		{
			return VillagerDecoData.Contains(villager.id, this.villager) && VillagerDecoData.Contains(building.name, this.deco);
		}

		// Token: 0x060036B0 RID: 14000 RVA: 0x0010A9DB File Offset: 0x00108DDB
		public static bool Contains(string what, string where)
		{
			return !string.IsNullOrEmpty(where) && (where == "any" || where.Contains(what, StringComparison.CurrentCultureIgnoreCase));
		}

		// Token: 0x04005ECC RID: 24268
		public string interaction_id;

		// Token: 0x04005ECD RID: 24269
		public string villager;

		// Token: 0x04005ECE RID: 24270
		public string deco;

		// Token: 0x04005ECF RID: 24271
		public string text;

		// Token: 0x04005ED0 RID: 24272
		public string reaction;

		// Token: 0x04005ED1 RID: 24273
		public int lvl_start;

		// Token: 0x04005ED2 RID: 24274
		public int lvl_end;
	}
}
