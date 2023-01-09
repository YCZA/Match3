using System;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Config
{
	// Token: 0x020004A6 RID: 1190
	[Serializable]
	public class TournamentConfig
	{
		// Token: 0x06002195 RID: 8597 RVA: 0x0008CC6C File Offset: 0x0008B06C
		public static string GetLocaKeyForEventQualifyInfo(TournamentType tournamentType)
		{
			switch (tournamentType)
			{
			case TournamentType.Bomb:
				return "ui.tournaments.qualifying.text_type1";
			case TournamentType.Butterfly:
				return "ui.tournaments.qualifying.text_type2";
			case TournamentType.Line:
				return "ui.tournaments.qualifying.text_type3";
			case TournamentType.Strawberry:
				return "ui.tournaments.qualifying.text_type4_strawberry";
			case TournamentType.Banana:
				return "ui.tournaments.qualifying.text_type5_banana";
			case TournamentType.Plum:
				return "ui.tournaments.qualifying.text_type6_plum";
			case TournamentType.Apple:
				return "ui.tournaments.qualifying.text_type7_apple";
			case TournamentType.Starfruit:
				return "ui.tournaments.qualifying.text_type8_starfruit";
			case TournamentType.Grape:
				return "ui.tournaments.qualifying.text_type9_grape";
			default:
				return string.Empty;
			}
		}

		// Token: 0x06002196 RID: 8598 RVA: 0x0008CCE8 File Offset: 0x0008B0E8
		public static string GetLocaKeyForTournamentType(TournamentType tournamentType)
		{
			switch (tournamentType)
			{
			case TournamentType.Bomb:
				return "ui.tournaments.header_type1";
			case TournamentType.Butterfly:
				return "ui.tournaments.header_type2";
			case TournamentType.Line:
				return "ui.tournaments.header_type3";
			case TournamentType.Strawberry:
				return "ui.tournaments.header_type4_strawberry";
			case TournamentType.Banana:
				return "ui.tournaments.header_type5_banana";
			case TournamentType.Plum:
				return "ui.tournaments.header_type6_plum";
			case TournamentType.Apple:
				return "ui.tournaments.header_type7_apple";
			case TournamentType.Starfruit:
				return "ui.tournaments.header_type8_starfruit";
			case TournamentType.Grape:
				return "ui.tournaments.header_type9_grape";
			default:
				return string.Empty;
			}
		}

		// Token: 0x06002197 RID: 8599 RVA: 0x0008CD64 File Offset: 0x0008B164
		public static string GetTournamentIllustrationPath(TournamentType tournamentType)
		{
			string nameFor = MaterialName.GetNameFor(tournamentType);
			return string.Format("Assets/Puzzletown/Town/Ui/Art/Tournaments/TournamentIllustrations/ui_tournament_image_{0}.png", nameFor);
		}

		// Token: 0x06002198 RID: 8600 RVA: 0x0008CD83 File Offset: 0x0008B183
		public static bool IsFruitTournament(TournamentType tournamentType)
		{
			return tournamentType >= TournamentType.Strawberry && tournamentType <= TournamentType.Grape;
		}

		// Token: 0x06002199 RID: 8601 RVA: 0x0008CD97 File Offset: 0x0008B197
		public static bool IsGemColorMatchingFruitTournament(TournamentType tournamentType, GemColor gemColor)
		{
			return TournamentConfig.IsFruitTournament(tournamentType) && gemColor == TournamentConfig.TournamentTypeToGemColor(tournamentType);
		}

		// Token: 0x0600219A RID: 8602 RVA: 0x0008CDAF File Offset: 0x0008B1AF
		public static GemColor TournamentTypeToGemColor(TournamentType tournamentType)
		{
			switch (tournamentType)
			{
			case TournamentType.Strawberry:
				return GemColor.Red;
			case TournamentType.Banana:
				return GemColor.Yellow;
			case TournamentType.Plum:
				return GemColor.Blue;
			case TournamentType.Apple:
				return GemColor.Orange;
			case TournamentType.Starfruit:
				return GemColor.Green;
			case TournamentType.Grape:
				return GemColor.Purple;
			default:
				return GemColor.Undefined;
			}
		}

		// Token: 0x0600219B RID: 8603 RVA: 0x0008CDE4 File Offset: 0x0008B1E4
		public TournamentConfig Copy()
		{
			return new TournamentConfig
			{
				name = this.name,
				tournamentType = this.tournamentType,
				pointsToQualify = this.pointsToQualify,
				rewards = this.rewards.Copy()
			};
		}

		// Token: 0x0600219C RID: 8604 RVA: 0x0008CE2D File Offset: 0x0008B22D
		public int GetScoreMultiplierForZeroBasedTier(int tier, bool shouldIgnoreMultiplier)
		{
			return (!shouldIgnoreMultiplier) ? Mathf.Max(1, tier * 5) : 1;
		}

		// Token: 0x04004CB9 RID: 19641
		public const string TOURNAMENT_ILLUSTRATION_BUNDLE_NAME = "tournament_illustrations";

		// Token: 0x04004CBA RID: 19642
		public string name;

		// Token: 0x04004CBB RID: 19643
		public TournamentType tournamentType;

		// Token: 0x04004CBC RID: 19644
		public int pointsToQualify;

		// Token: 0x04004CBD RID: 19645
		public TournamentRewardConfig rewards;
	}
}
