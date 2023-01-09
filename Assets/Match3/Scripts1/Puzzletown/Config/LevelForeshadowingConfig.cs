using System;
using System.Collections.Generic;

namespace Match3.Scripts1.Puzzletown.Config
{
	// Token: 0x020007F3 RID: 2035
	[Serializable]
	public class LevelForeshadowingConfig
	{
		// Token: 0x04005AD4 RID: 23252
		public List<LevelForeshadowingConfig.ForeshadowingLevelConfig> level_config;

		// Token: 0x020007F4 RID: 2036
		[Serializable]
		public class ForeshadowingLevelConfig
		{
			// Token: 0x04005AD5 RID: 23253
			public int level;

			// Token: 0x04005AD6 RID: 23254
			public string type;

			// Token: 0x04005AD7 RID: 23255
			public string feature;

			// Token: 0x04005AD8 RID: 23256
			public string locaKey;
		}
	}
}
