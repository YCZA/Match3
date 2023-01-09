using System.Collections.Generic;
using System.Linq;
using Match3.Scripts1.Puzzletown.Config;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x020007FF RID: 2047
	public class SeasonServiceEventsDataSource
	{
		// Token: 0x06003283 RID: 12931 RVA: 0x000EE047 File Offset: 0x000EC447
		public SeasonServiceEventsDataSource(ConfigService configService)
		{
			this.configService = configService;
		}

		// Token: 0x06003284 RID: 12932 RVA: 0x000EE058 File Offset: 0x000EC458
		private List<SeasonConfig> GetAllRemoteSeasonConfigs()
		{
			if (this.configService.SbsConfig._events != null && this.configService.SbsConfig._events.seasons != null)
			{
				return this.configService.SbsConfig._events.seasons;
			}
			return null;
		}

		// Token: 0x06003285 RID: 12933 RVA: 0x000EE0AC File Offset: 0x000EC4AC
		public SeasonConfig GetActiveSeason()
		{
			List<SeasonConfig> allRemoteSeasonConfigs = this.GetAllRemoteSeasonConfigs();
			if (allRemoteSeasonConfigs == null)
			{
				return null;
			}
			return allRemoteSeasonConfigs.FirstOrDefault((SeasonConfig season) => season.IsSeasonActive);
		}

		// Token: 0x06003286 RID: 12934 RVA: 0x000EE0F0 File Offset: 0x000EC4F0
		public SeasonConfig GetLoadingScreenSeason(string loadingScreenSeason)
		{
			SeasonConfig activeSeason = this.GetActiveSeason();
			if (activeSeason == null)
			{
				return null;
			}
			if (activeSeason.Primary != loadingScreenSeason)
			{
				return null;
			}
			return activeSeason;
		}

		// Token: 0x04005B04 RID: 23300
		private ConfigService configService;
	}
}
