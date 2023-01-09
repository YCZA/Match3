using System;

// Token: 0x020008C9 RID: 2249
namespace Match3.Scripts1
{
	[Serializable]
	public class VillagerConfig
	{
		// Token: 0x060036C0 RID: 14016 RVA: 0x0010AE54 File Offset: 0x00109254
		public VillagerData GetVillager(string villagerName)
		{
			int i = 0;
			int num = this.characters.Length;
			while (i < num)
			{
				if (this.characters[i].id == villagerName)
				{
					return this.characters[i];
				}
				i++;
			}
			return null;
		}

		// Token: 0x04005EF3 RID: 24307
		public VillagerData[] characters;
	}
}
