using System;
using System.Collections.Generic;
using System.Linq;

namespace Match3.Scripts1.Puzzletown.Config
{
	// Token: 0x02000499 RID: 1177
	[Serializable]
	public class PtEventsConfig
	{
		// Token: 0x17000527 RID: 1319
		// (get) Token: 0x0600215E RID: 8542 RVA: 0x0008C391 File Offset: 0x0008A791
		// (set) Token: 0x0600215F RID: 8543 RVA: 0x0008C399 File Offset: 0x0008A799
		public List<SeasonConfig> seasons { get; private set; }

		// Token: 0x06002160 RID: 8544 RVA: 0x0008C3A4 File Offset: 0x0008A7A4
		public void Init()
		{
			if (this.events == null)
			{
				WoogaDebug.LogWarning(new object[]
				{
					"No events in config"
				});
				return;
			}
			this.seasons = (from e in this.events
			where e != null && e.config != null && e.config.season_config != null && e.config.season_config.IsValid
			select e.config.season_config.Fixup(e)).ToList<SeasonConfig>();
		}

		// Token: 0x04004C78 RID: 19576
		public List<EventConfigContainer> events;

		// Token: 0x04004C79 RID: 19577
		public List<TournamentEventConfig> leagues;
	}
}
