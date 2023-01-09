using Match3.Scripts1.Puzzletown.Match3;

namespace Match3.Scripts1
{
	// Token: 0x02000578 RID: 1400
	public struct PlayModeFlowParameters
	{
		// Token: 0x060024CD RID: 9421 RVA: 0x000A49E3 File Offset: 0x000A2DE3
		public PlayModeFlowParameters(string currentPath, string levelName, LevelConfig config)
		{
			this.currentPath = currentPath;
			this.config = config;
			this.levelName = levelName;
		}

		// Token: 0x04005062 RID: 20578
		public readonly string currentPath;

		// Token: 0x04005063 RID: 20579
		public readonly LevelConfig config;

		// Token: 0x04005064 RID: 20580
		public readonly string levelName;
	}
}
