using System;
using System.Collections.Generic;
using Match3.Scripts1.Town;
using Match3.Scripts2.PlayerData;
using Wooga.Foundation.Json; //using Facebook.Unity;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x02000756 RID: 1878
	public class BuildingDataService : ADataService
	{
		// Token: 0x06002E83 RID: 11907 RVA: 0x000D996A File Offset: 0x000D7D6A
		public BuildingDataService(Func<GameState> getState, ConfigService configService) : base(getState)
		{
			this.configService = configService;
		}

		// Token: 0x17000731 RID: 1841
		// (get) Token: 0x06002E84 RID: 11908 RVA: 0x000D9997 File Offset: 0x000D7D97
		public IEnumerable<BuildingInstance.PersistentData> Buildings
		{
			get
			{
				return this.BuildingsList;
			}
		}

		// Token: 0x17000732 RID: 1842
		// (get) Token: 0x06002E85 RID: 11909 RVA: 0x000D999F File Offset: 0x000D7D9F
		private List<BuildingInstance.PersistentData> BuildingsList
		{
			get
			{
				return this.BuildingsData.Buildings;
			}
		}

		// Token: 0x17000733 RID: 1843
		// (get) Token: 0x06002E86 RID: 11910 RVA: 0x000D99AC File Offset: 0x000D7DAC
		// (set) Token: 0x06002E87 RID: 11911 RVA: 0x000D99BE File Offset: 0x000D7DBE
		public int CurrentIsland
		{
			get
			{
				return base.state.buildings.CurrentIsland;
			}
			set
			{
				base.state.buildings.CurrentIsland = value;
			}
		}

		// Token: 0x17000734 RID: 1844
		// (get) Token: 0x06002E88 RID: 11912 RVA: 0x000D99D4 File Offset: 0x000D7DD4
		public BuildingsData BuildingsData
		{
			get
			{
				if (this.CurrentIsland == 0)
				{
					return base.state.buildings;
				}
				int num = this.CurrentIsland - 1;
				if (num >= base.state.adventureIslands.Count)
				{
					BuildingsData item = new BuildingsData();
					base.state.adventureIslands.Add(item);
				}
				return base.state.adventureIslands[num];
			}
		}

		// Token: 0x06002E89 RID: 11913 RVA: 0x000D9A3F File Offset: 0x000D7E3F
		public Dictionary<string, int> GetAllStoredBuildings()
		{
			this.allStoredBuildings.Clear();
			// this.allStoredBuildings.AddAllKVPFrom(this.SharedStoredBuildings);
			// this.allStoredBuildings.AddAllKVPFrom(this.IslandSpecificStoredBuildings);
			foreach (var kvp in this.SharedStoredBuildings)
			{
				allStoredBuildings.Add(kvp.Key, kvp.Value);
			}

			foreach (var kvp in this.IslandSpecificStoredBuildings)
			{
				allStoredBuildings.Add(kvp.Key, kvp.Value);
			}
			return this.allStoredBuildings;
		}

		// Token: 0x06002E8A RID: 11914 RVA: 0x000D9A74 File Offset: 0x000D7E74
		public bool IsBuildingStored(string buildingName)
		{
			return this.GetAllStoredBuildings().ContainsKey(buildingName);
		}

		// Token: 0x06002E8B RID: 11915 RVA: 0x000D9A84 File Offset: 0x000D7E84
		public int GetCountOfAllStoredBuildings()
		{
			int num = 0;
			foreach (KeyValuePair<string, int> keyValuePair in this.GetAllStoredBuildings())
			{
				num += keyValuePair.Value;
			}
			return num;
		}

		// Token: 0x17000735 RID: 1845
		// (get) Token: 0x06002E8C RID: 11916 RVA: 0x000D9AE8 File Offset: 0x000D7EE8
		public Dictionary<string, int> SharedStoredBuildings
		{
			get
			{
				if (this.sharedStoredBuildings == null)
				{
					this.sharedStoredBuildings = new Dictionary<string, int>();
					if (!string.IsNullOrEmpty(base.state.SharedStoredBuildings))
					{
						this.sharedStoredBuildings = JSON.Deserialize<Dictionary<string, int>>(base.state.SharedStoredBuildings);
					}
				}
				return this.sharedStoredBuildings;
			}
		}

		// Token: 0x17000736 RID: 1846
		// (get) Token: 0x06002E8D RID: 11917 RVA: 0x000D9B3C File Offset: 0x000D7F3C
		private Dictionary<string, int> IslandSpecificStoredBuildings
		{
			get
			{
				if (this.currentStorageIsland != this.CurrentIsland)
				{
					this.currentStorageIsland = this.CurrentIsland;
					this.islandSpecificStoredBuildings.Clear();
					if (!string.IsNullOrEmpty(this.BuildingsData.StoredBuildings))
					{
						this.islandSpecificStoredBuildings = JSON.Deserialize<Dictionary<string, int>>(this.BuildingsData.StoredBuildings);
					}
					List<string> list = new List<string>();
					foreach (KeyValuePair<string, int> keyValuePair in this.islandSpecificStoredBuildings)
					{
						BuildingConfig config = this.configService.buildingConfigList.GetConfig(keyValuePair.Key);
						if (config != null && config.shared_storage == 1)
						{
							this.StoreBuilding(config, keyValuePair.Value);
							list.Add(keyValuePair.Key);
						}
					}
					foreach (string key in list)
					{
						this.islandSpecificStoredBuildings.Remove(key);
					}
					if (list.Count != 0)
					{
						this.BuildingsData.StoredBuildings = JSON.Serialize(this.islandSpecificStoredBuildings, false, 1, ' ');
					}
				}
				return this.islandSpecificStoredBuildings;
			}
		}

		// Token: 0x06002E8E RID: 11918 RVA: 0x000D9CAC File Offset: 0x000D80AC
		public void StoreBuilding(BuildingConfig blueprint, int amount = 1)
		{
			if (blueprint.shared_storage == 1)
			{
				this.StoreBuilding(this.SharedStoredBuildings, blueprint.name, amount);
				base.state.SharedStoredBuildings = JSON.Serialize(this.SharedStoredBuildings, false, 1, ' ');
			}
			else
			{
				this.StoreBuilding(this.IslandSpecificStoredBuildings, blueprint.name, amount);
				this.BuildingsData.StoredBuildings = JSON.Serialize(this.IslandSpecificStoredBuildings, false, 1, ' ');
			}
		}

		// Token: 0x06002E8F RID: 11919 RVA: 0x000D9D24 File Offset: 0x000D8124
		public int GetStoredBuildingsCount(BuildingConfig blueprint)
		{
			Dictionary<string, int> storage = (blueprint.shared_storage != 1) ? this.IslandSpecificStoredBuildings : this.SharedStoredBuildings;
			return this.GetStoredBuildingsCount(storage, blueprint.name);
		}

		// Token: 0x06002E90 RID: 11920 RVA: 0x000D9D5C File Offset: 0x000D815C
		public int CountOfBuilding(string name)
		{
			int num = 0;
			foreach (BuildingInstance.PersistentData persistentData in this.Buildings)
			{
				if (persistentData.IsSaved && persistentData.blueprintName == name)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06002E91 RID: 11921 RVA: 0x000D9DD4 File Offset: 0x000D81D4
		public void Reset()
		{
			base.state.buildings = new BuildingsData();
		}

		// Token: 0x06002E92 RID: 11922 RVA: 0x000D9DE6 File Offset: 0x000D81E6
		public void RegisterBuildingData(BuildingInstance.PersistentData data)
		{
			if (this.BuildingsData.Buildings.Contains(data))
			{
				return;
			}
			this.BuildingsData.Buildings.Add(data);
		}

		// Token: 0x06002E93 RID: 11923 RVA: 0x000D9E10 File Offset: 0x000D8210
		public void RegisterBuilding(BuildingInstance newBuilding)
		{
			BuildingInstance.PersistentData sv = newBuilding.sv;
			if (this.BuildingsData.Buildings.Contains(sv))
			{
				return;
			}
			this.BuildingsData.Buildings.Add(sv);
		}

		// Token: 0x06002E94 RID: 11924 RVA: 0x000D9E4C File Offset: 0x000D824C
		public void RemoveBuilding(BuildingInstance oldBuilding)
		{
			BuildingInstance.PersistentData sv = oldBuilding.sv;
			if (!sv.IsSaved)
			{
				return;
			}
			this.BuildingsData.Buildings.Remove(sv);
		}

		// Token: 0x06002E95 RID: 11925 RVA: 0x000D9E7E File Offset: 0x000D827E
		public bool IsBuildingReviewed(string id)
		{
			return base.state.buildings.ReviewedBuildings.IndexOf(id) != -1;
		}

		// Token: 0x06002E96 RID: 11926 RVA: 0x000D9E9C File Offset: 0x000D829C
		public void SetBuildingReviewed(string id)
		{
			if (this.IsBuildingReviewed(id))
			{
				return;
			}
			base.state.buildings.ReviewedBuildings.Add(id);
		}

		// Token: 0x06002E97 RID: 11927 RVA: 0x000D9EC1 File Offset: 0x000D82C1
		public bool IsAreaDeployed(int area)
		{
			return this.BuildingsData.LastDeployed >= area;
		}

		// Token: 0x06002E98 RID: 11928 RVA: 0x000D9ED4 File Offset: 0x000D82D4
		public void SetAreaDeployed(int area)
		{
			this.BuildingsData.LastDeployed = area;
		}

		// Token: 0x06002E99 RID: 11929 RVA: 0x000D9EE4 File Offset: 0x000D82E4
		private void StoreBuilding(IDictionary<string, int> storage, string name, int amount)
		{
			int storedBuildingsCount = this.GetStoredBuildingsCount(storage, name);
			int num = storedBuildingsCount + amount;
			if (num <= 0)
			{
				storage.Remove(name);
			}
			else
			{
				storage[name] = num;
			}
		}

		// Token: 0x06002E9A RID: 11930 RVA: 0x000D9F1A File Offset: 0x000D831A
		private int GetStoredBuildingsCount(IDictionary<string, int> storage, string buildingName)
		{
			if (storage.ContainsKey(buildingName))
			{
				return storage[buildingName];
			}
			return 0;
		}

		// Token: 0x040057C8 RID: 22472
		public const string UNIQUE_AREA_4_BUILDING = "iso_unique_spa_healing";

		// Token: 0x040057C9 RID: 22473
		public const string BUILDING_PRFIX = "iso_";

		// Token: 0x040057CA RID: 22474
		private Dictionary<string, int> sharedStoredBuildings;

		// Token: 0x040057CB RID: 22475
		private Dictionary<string, int> islandSpecificStoredBuildings = new Dictionary<string, int>();

		// Token: 0x040057CC RID: 22476
		private Dictionary<string, int> allStoredBuildings = new Dictionary<string, int>();

		// Token: 0x040057CD RID: 22477
		private int currentStorageIsland = -1;

		// Token: 0x040057CE RID: 22478
		private ConfigService configService;
	}
}
