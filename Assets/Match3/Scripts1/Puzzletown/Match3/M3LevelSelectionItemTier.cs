using System.Collections.Generic;
using Match3.Scripts1.Puzzletown.Config;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020006FA RID: 1786
	public class M3LevelSelectionItemTier
	{
		// Token: 0x17000700 RID: 1792
		// (get) Token: 0x06002C56 RID: 11350 RVA: 0x000CC2F5 File Offset: 0x000CA6F5
		public bool HasTournamentToShow
		{
			get
			{
				return this.tournamentType != TournamentType.Undefined;
			}
		}

		// Token: 0x17000701 RID: 1793
		// (get) Token: 0x06002C57 RID: 11351 RVA: 0x000CC303 File Offset: 0x000CA703
		public string TournamentMultiplierAsString
		{
			get
			{
				return string.Format("x{0}", this.tournamentPointMultiplier);
			}
		}

		// Token: 0x04005584 RID: 21892
		public string name;

		// Token: 0x04005585 RID: 21893
		public List<MaterialAmount> rewards;

		// Token: 0x04005586 RID: 21894
		public LevelConfig level;

		// Token: 0x04005587 RID: 21895
		public int diamonds;

		// Token: 0x04005588 RID: 21896
		public int multiplier;

		// Token: 0x04005589 RID: 21897
		public int tier;

		// Token: 0x0400558A RID: 21898
		public TournamentType tournamentType;

		// Token: 0x0400558B RID: 21899
		public int tournamentPointMultiplier = 1;

		// Token: 0x0400558C RID: 21900
		public LevelPlayMode levelPlayMode;

		// Token: 0x0400558D RID: 21901
		public Sprite seasonSprite;

		// Token: 0x0400558E RID: 21902
		public int seasonalCurrencyRewardAmount;

		// Token: 0x0400558F RID: 21903
		public bool showSeasonRewards;
	}
}
