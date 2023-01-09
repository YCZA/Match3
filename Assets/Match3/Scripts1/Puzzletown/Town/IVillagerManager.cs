using System.Collections.Generic;

namespace Match3.Scripts1.Puzzletown.Town
{
	// Token: 0x020008CD RID: 2253
	public interface IVillagerManager
	{
		// Token: 0x1700085E RID: 2142
		// (get) Token: 0x060036C5 RID: 14021
		IEnumerable<VillagerView> Villagers { get; }

		// Token: 0x060036C6 RID: 14022
		VillagerView CreateVillager(string name);
	}
}
