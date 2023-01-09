using System;
using Match3.Scripts2.PlayerData;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x02000769 RID: 1897
	public class DiveForTreasureDataService : AWeeklyEventDataService
	{
		// Token: 0x06002F12 RID: 12050 RVA: 0x000DBF4D File Offset: 0x000DA34D
		public DiveForTreasureDataService(Func<GameState> getState) : base(getState)
		{
		}

		// Token: 0x17000757 RID: 1879
		// (get) Token: 0x06002F13 RID: 12051 RVA: 0x000DBF56 File Offset: 0x000DA356
		public int Set
		{
			get
			{
				return base.state.diveForTreasureData.set;
			}
		}

		// Token: 0x17000758 RID: 1880
		// (get) Token: 0x06002F14 RID: 12052 RVA: 0x000DBF68 File Offset: 0x000DA368
		public override string ConfigKey
		{
			get
			{
				return "DiveForTreasureConfig";
			}
		}

		// Token: 0x17000759 RID: 1881
		// (get) Token: 0x06002F15 RID: 12053 RVA: 0x000DBF6F File Offset: 0x000DA36F
		public override WeeklyEventData EventData
		{
			get
			{
				return base.state.diveForTreasureData;
			}
		}

		// Token: 0x06002F16 RID: 12054 RVA: 0x000DBF7C File Offset: 0x000DA37C
		public override string GetTrophyForLevel(int level)
		{
			if (level == 8)
			{
				return "iso_trophy_treasure_dive";
			}
			return base.GetTrophyForLevel(level);
		}

		// Token: 0x04005830 RID: 22576
		public const string DIVE_FOR_TREASURE_KEY = "DiveForTreasureConfig";
	}
}
