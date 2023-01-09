using System.Collections.Generic;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts2.PlayerData;

// Token: 0x020008FA RID: 2298
namespace Match3.Scripts1
{
	public class BuildingServices
	{
		// Token: 0x17000898 RID: 2200
		// (get) Token: 0x060037FA RID: 14330 RVA: 0x00111D26 File Offset: 0x00110126
		public BuildingDataService DataService
		{
			get
			{
				return this.GameStateService.Buildings;
			}
		}

		// Token: 0x17000899 RID: 2201
		// (get) Token: 0x060037FB RID: 14331 RVA: 0x00111D33 File Offset: 0x00110133
		public BuildingConfigList BuildingConfigs
		{
			get
			{
				return this.ConfigService.buildingConfigList;
			}
		}

		// Token: 0x1700089A RID: 2202
		// (get) Token: 0x060037FC RID: 14332 RVA: 0x00111D40 File Offset: 0x00110140
		public IEnumerable<StartBuilding> StartBuildings
		{
			get
			{
				return this.ConfigService.startBuilding.GetAllOnIsland(this.CurrentIslandId);
			}
		}

		// Token: 0x1700089B RID: 2203
		// (get) Token: 0x060037FD RID: 14333 RVA: 0x00111D58 File Offset: 0x00110158
		public int CurrentIslandId
		{
			get
			{
				return this.GameStateService.Buildings.CurrentIsland;
			}
		}

		// Token: 0x060037FE RID: 14334 RVA: 0x00111D6A File Offset: 0x0011016A
		public void Init(ConfigService i_ConfigService, BuildingResourceServiceRoot i_BuildingResourceService, GameStateService i_GameStateService)
		{
			this.GameStateService = i_GameStateService;
			this.BuildingResourceService = i_BuildingResourceService;
			this.ConfigService = i_ConfigService;
			this.BuildingsController = new BuildingsController(this.DataService);
		}

		// Token: 0x04006021 RID: 24609
		public BuildingResourceServiceRoot BuildingResourceService;

		// Token: 0x04006022 RID: 24610
		public BuildingsController BuildingsController;

		// Token: 0x04006023 RID: 24611
		public GameStateService GameStateService;

		// Token: 0x04006024 RID: 24612
		public ConfigService ConfigService;
	}
}
