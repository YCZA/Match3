using System;
using System.Collections.Generic;
using System.Linq;
using Match3.Scripts1.Town;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x02000757 RID: 1879
	[Serializable]
	public class BuildingsData
	{
		// Token: 0x06002E9C RID: 11932 RVA: 0x000D9F50 File Offset: 0x000D8350
		public BuildingsData Clone()
		{
			BuildingsData buildingsData = new BuildingsData();
			buildingsData.Buildings = (from cBuilding in this.Buildings
			select cBuilding.Clone()).ToList<BuildingInstance.PersistentData>();
			buildingsData.ReviewedBuildings = new List<string>(this.ReviewedBuildings);
			buildingsData.LastDeployed = this.LastDeployed;
			buildingsData.StoredBuildings = this.StoredBuildings;
			return buildingsData;
		}

		// Token: 0x040057CF RID: 22479
		public List<BuildingInstance.PersistentData> Buildings = new List<BuildingInstance.PersistentData>();

		// Token: 0x040057D0 RID: 22480
		public List<string> ReviewedBuildings = new List<string>();

		// Token: 0x040057D1 RID: 22481
		public int LastDeployed;

		// Token: 0x040057D2 RID: 22482
		public string StoredBuildings;

		// Token: 0x040057D3 RID: 22483
		public int CurrentIsland;
	}
}
