using System;
using System.Collections.Generic;
using System.Linq;

// Token: 0x020008E8 RID: 2280
namespace Match3.Scripts1
{
	[Serializable]
	public class BuildingConfigList
	{
		// Token: 0x17000883 RID: 2179
		// (get) Token: 0x0600376B RID: 14187 RVA: 0x0010DFB8 File Offset: 0x0010C3B8
		public Dictionary<string, BuildingConfig> Map
		{
			get
			{
				if (this._map == null)
				{
					this._map = new Dictionary<string, BuildingConfig>();
					foreach (BuildingConfig buildingConfig in this.buildings)
					{
						this._map[buildingConfig.name] = buildingConfig;
					}
				}
				return this._map;
			}
		}

		public BuildingConfig GetConfig(string name)
		{
			if (this.Map.ContainsKey(name))
			{
				return this.Map[name];
			}
			return null;
		}

		// Token: 0x0600376D RID: 14189 RVA: 0x0010E034 File Offset: 0x0010C434
		public IEnumerable<BuildingConfig> GetAllOnIsland(int islandId)
		{
			return from b in this.buildings
				where b.island_id == islandId
				select b;
		}

		// Token: 0x0600376E RID: 14190 RVA: 0x0010E068 File Offset: 0x0010C468
		public IEnumerable<BuildingConfig> GetAllForSeasons(string[] seasons)
		{
			return from b in this.buildings
				where seasons.Contains(b.season)
				select b;
		}

		// Token: 0x0600376F RID: 14191 RVA: 0x0010E09C File Offset: 0x0010C49C
		public BuildingConfig GetConfig(DecoTrophyItemWon decoTrophy)
		{
			string text = (decoTrophy != DecoTrophyItemWon.None) ? ("iso_trophy_" + decoTrophy.AsString()) : string.Empty;
			if (!string.IsNullOrEmpty(text))
			{
				return this.GetConfig(text);
			}
			return null;
		}

		// Token: 0x06003770 RID: 14192 RVA: 0x0010E0E0 File Offset: 0x0010C4E0
		public void SetupSeasonalsV3ABTest(bool enableEventCurrency)
		{
			foreach (BuildingConfig buildingConfig in this.buildings)
			{
				if (buildingConfig.costs.Length == 2 && (buildingConfig.costs[1].type == "season_currency" || buildingConfig.costs[1].type == "earned_season_currency"))
				{
					buildingConfig.costs = new MaterialAmount[]
					{
						buildingConfig.costs[(!enableEventCurrency) ? 0 : 1]
					};
				}
				if (enableEventCurrency && buildingConfig.harmony_seasonals_v3 > 0)
				{
					buildingConfig.harmony = buildingConfig.harmony_seasonals_v3;
				}
			}
		}

		// Token: 0x04005FA4 RID: 24484
		public BuildingConfig[] buildings;

		// Token: 0x04005FA5 RID: 24485
		private const string ISO_TROPHY_PREFIX = "iso_trophy_";

		// Token: 0x04005FA6 RID: 24486
		private Dictionary<string, BuildingConfig> _map;
	}
}
