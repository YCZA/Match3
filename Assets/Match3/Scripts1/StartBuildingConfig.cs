using System;
using System.Collections.Generic;
using System.Linq;
using Match3.Scripts1.Puzzletown.Config;
using Match3.Scripts1.Puzzletown.Services;
using UnityEngine;

// Token: 0x020008EA RID: 2282
namespace Match3.Scripts1
{
	[Serializable]
	public class StartBuildingConfig: IInitializable
	{
		// Token: 0x06003773 RID: 14195 RVA: 0x0010E237 File Offset: 0x0010C637
		public StartBuildingConfig()
		{
		}

		public void Init()
		{
			islands.Add(islands1);
			islands.Add(islands2);
			islands.Add(islands3);
			islands.Add(islands4);
			islands.Add(islands5);
			islands.Add(islands6);
		}
	
		// Token: 0x06003774 RID: 14196 RVA: 0x0010E24C File Offset: 0x0010C64C
		public StartBuildingConfig(StartBuilding[] buildings, IslandAreaConfigs islandAreaConfigs)
		{
			foreach (StartBuilding startBuilding in buildings)
			{
				int num = islandAreaConfigs.IslandForArea(startBuilding.area);
				this.EnsureNumberOfIslands(num);
				this.islands[num].buildings.Add(startBuilding);
			}
		}

		// Token: 0x06003775 RID: 14197 RVA: 0x0010E2AF File Offset: 0x0010C6AF
		public IEnumerable<StartBuilding> GetAllOnIsland(int islandId)
		{
			// Debug.LogError("get all on island");
			this.EnsureNumberOfIslands(islandId);
			return this.islands[islandId].buildings;
		}

		// Token: 0x06003776 RID: 14198 RVA: 0x0010E2C9 File Offset: 0x0010C6C9
		public void UpdateIsland(int islandId, StartBuilding[] buildings)
		{
			this.EnsureNumberOfIslands(islandId);
			this.islands[islandId].buildings = buildings.ToList<StartBuilding>();
		}

		// Token: 0x06003777 RID: 14199 RVA: 0x0010E2E9 File Offset: 0x0010C6E9
		public IEnumerable<StartBuilding> GetAll()
		{
			return this.islands.SelectMany((StartBuildingConfig.Island i) => i.buildings);
		}

		// Token: 0x06003778 RID: 14200 RVA: 0x0010E313 File Offset: 0x0010C713
		private void EnsureNumberOfIslands(int islandId)
		{
			while (this.islands.Count <= islandId)
			{
				this.islands.Add(new StartBuildingConfig.Island());
			}
		}

		private List<StartBuildingConfig.Island> islands = new List<StartBuildingConfig.Island>();

		[SerializeField]
		private Island islands1 = new Island();
		[SerializeField]
		private Island islands2 = new Island();
		[SerializeField]
		private Island islands3 = new Island();
		[SerializeField]
		private Island islands4 = new Island();
		[SerializeField]
		private Island islands5 = new Island();
		[SerializeField]
		private Island islands6 = new Island();

		[Serializable]
		public class Island
		{
			// Token: 0x04005FAF RID: 24495
			public List<StartBuilding> buildings = new List<StartBuilding>();
		}
	}
}
