using System;
using Match3.Scripts1.Town;

// Token: 0x020008E9 RID: 2281
namespace Match3.Scripts1
{
	[Serializable]
	public class StartBuilding
	{
		// Token: 0x06003772 RID: 14194 RVA: 0x0010E1E8 File Offset: 0x0010C5E8
		public bool IsMatching(BuildingInstance.PersistentData building)
		{
			return this.building_id == building.blueprintName && this.pos_x == building.position.x && this.pos_y == building.position.y;
		}

		// Token: 0x04005FA7 RID: 24487
		public int area;

		// Token: 0x04005FA8 RID: 24488
		public int pos_x;

		// Token: 0x04005FA9 RID: 24489
		public int pos_y;

		// Token: 0x04005FAA RID: 24490
		public bool destroyed;

		// Token: 0x04005FAB RID: 24491
		public string building_id;

		// Token: 0x04005FAC RID: 24492
		public string deco_set;
	}
}
