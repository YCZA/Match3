using System;
using Match3.Scripts2.PlayerData;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x0200082E RID: 2094
	public class PirateBreakOutDataService : AWeeklyEventDataService
	{
		// Token: 0x06003416 RID: 13334 RVA: 0x000F82BD File Offset: 0x000F66BD
		public PirateBreakOutDataService(Func<GameState> getState) : base(getState)
		{
		}

		// Token: 0x17000837 RID: 2103
		// (get) Token: 0x06003417 RID: 13335 RVA: 0x000F82C6 File Offset: 0x000F66C6
		public int Set
		{
			get
			{
				return base.state.pirateBreakoutData.set;
			}
		}

		// Token: 0x17000838 RID: 2104
		// (get) Token: 0x06003418 RID: 13336 RVA: 0x000F82D8 File Offset: 0x000F66D8
		public override string ConfigKey
		{
			get
			{
				return "PirateBreakoutConfig";
			}
		}

		// Token: 0x17000839 RID: 2105
		// (get) Token: 0x06003419 RID: 13337 RVA: 0x000F82DF File Offset: 0x000F66DF
		public override WeeklyEventData EventData
		{
			get
			{
				return base.state.pirateBreakoutData;
			}
		}

		// Token: 0x0600341A RID: 13338 RVA: 0x000F82EC File Offset: 0x000F66EC
		public override string GetTrophyForLevel(int level)
		{
			if (level == 10)
			{
				return "iso_trophy_pirate_breakout";
			}
			return base.GetTrophyForLevel(level);
		}

		// Token: 0x04005BF5 RID: 23541
		private const string PIRATE_BREAKOUT_KEY = "PirateBreakoutConfig";
	}
}
