using Match3.Scripts1.Puzzletown.Config;
using Match3.Scripts1.Puzzletown.Match3;

// Token: 0x020007E6 RID: 2022
namespace Match3.Scripts1
{
	public static class MaterialName
	{
		// Token: 0x0600320B RID: 12811 RVA: 0x000EBDC4 File Offset: 0x000EA1C4
		public static string GetNameFor(TournamentType type)
		{
			switch (type)
			{
				case TournamentType.Bomb:
					return "bomb";
				case TournamentType.Butterfly:
					return "butterfly";
				case TournamentType.Line:
					return "line";
				case TournamentType.Strawberry:
					return "strawberry";
				case TournamentType.Banana:
					return "banana";
				case TournamentType.Plum:
					return "plum";
				case TournamentType.Apple:
					return "apple";
				case TournamentType.Starfruit:
					return "starfruit";
				case TournamentType.Grape:
					return "grape";
				default:
					return string.Empty;
			}
		}

		// Token: 0x0600320C RID: 12812 RVA: 0x000EBE40 File Offset: 0x000EA240
		public static string GetNameFor(Boosts boostType)
		{
			switch (boostType)
			{
				case Boosts.boost_hammer:
					return "boost_hammer";
				case Boosts.boost_star:
					return "boost_star";
				case Boosts.boost_rainbow:
					return "boost_rainbow";
				case Boosts.boost_pre_rainbow:
					return "boost_pre_rainbow";
				case Boosts.boost_pre_bomb_linegem:
					return "boost_pre_bomb_linegem";
				case Boosts.boost_pre_double_fish:
					return "boost_pre_double_fish";
			}
			return string.Empty;
		}

		// Token: 0x04005A7C RID: 23164
		public const string COINS = "coins";

		// Token: 0x04005A7D RID: 23165
		public const string DIAMONDS = "diamonds";

		// Token: 0x04005A7E RID: 23166
		public const string PAWS = "paws";

		// Token: 0x04005A7F RID: 23167
		public const string LIVES = "lives";

		// Token: 0x04005A80 RID: 23168
		public const string UNLIMITED_LIVES = "UnlimitedLives";

		// Token: 0x04005A81 RID: 23169
		public const string LIVES_UNLIMITED_LOWERCASE = "lives_unlimited";

		// Token: 0x04005A82 RID: 23170
		public const string HARMONY = "harmony";

		// Token: 0x04005A83 RID: 23171
		public const string TILES = "tiles";

		// Token: 0x04005A84 RID: 23172
		public const string DROPPABLE = "droppable";

		// Token: 0x04005A85 RID: 23173
		public const string MOVES = "moves";

		// Token: 0x04005A86 RID: 23174
		public const string CLIMBER = "climber";

		// Token: 0x04005A87 RID: 23175
		public const string TREASURE = "treasure";

		// Token: 0x04005A88 RID: 23176
		public const string WATER = "water";

		// Token: 0x04005A89 RID: 23177
		public const string RESISTANT_BLOCKER = "resistant_blocker";

		// Token: 0x04005A8A RID: 23178
		public const string SEASON_CURRENCY = "season_currency";

		// Token: 0x04005A8B RID: 23179
		public const string EARNED_SEASON_CURRENCY = "earned_season_currency";

		// Token: 0x04005A8C RID: 23180
		public const string CHAMELEON = "chameleon";

		// Token: 0x04005A8D RID: 23181
		public const string BANK_DIAMONDS = "bank_diamonds";

		// Token: 0x04005A8E RID: 23182
		public const string KEY = "key";

		// Token: 0x04005A8F RID: 23183
		public const string MATCH_LINEGEM_RAINBOW = "linegem_rainbow";

		// Token: 0x04005A90 RID: 23184
		public const string MATCH_LINEGEM_BOMB = "linegem_bomb";

		// Token: 0x04005A91 RID: 23185
		public const string MATCH_LINEGEM_LINGEM = "linegem_linegem";

		// Token: 0x04005A92 RID: 23186
		public const string MATCH_BOMB_BOMB = "bomb_bomb";

		// Token: 0x04005A93 RID: 23187
		public const string MATCH_BOMB_RAINBOW = "bomb_rainbow";

		// Token: 0x04005A94 RID: 23188
		public const string MATCH_RAINBOW_RAINBOW = "rainbow_rainbow";

		// Token: 0x04005A95 RID: 23189
		public const string BEFORE_LAST_HURRAY = "_before_hurray";

		// Token: 0x04005A96 RID: 23190
		public const string CREATE_RAINBOW = "rainbow";

		// Token: 0x04005A97 RID: 23191
		public const string CREATE_LINEGEM = "linegem";

		// Token: 0x04005A98 RID: 23192
		public const string CREATE_FISH = "fish";

		// Token: 0x04005A99 RID: 23193
		public const string CREATE_BOMB = "bomb";

		// Token: 0x04005A9A RID: 23194
		private const string TOURNAMENT_TYPE_UNDEFINED = "";

		// Token: 0x04005A9B RID: 23195
		private const string TOURNAMENT_TYPE_BOMBS = "bomb";

		// Token: 0x04005A9C RID: 23196
		private const string TOURNAMENT_TYPE_BUTTERFLIES = "butterfly";

		// Token: 0x04005A9D RID: 23197
		private const string TOURNAMENT_TYPE_LINES = "line";

		// Token: 0x04005A9E RID: 23198
		private const string TOURNAMENT_TYPE_STRAWBERRY = "strawberry";

		// Token: 0x04005A9F RID: 23199
		private const string TOURNAMENT_TYPE_BANANA = "banana";

		// Token: 0x04005AA0 RID: 23200
		private const string TOURNAMENT_TYPE_PLUM = "plum";

		// Token: 0x04005AA1 RID: 23201
		private const string TOURNAMENT_TYPE_APPLE = "apple";

		// Token: 0x04005AA2 RID: 23202
		private const string TOURNAMENT_TYPE_STARFRUIT = "starfruit";

		// Token: 0x04005AA3 RID: 23203
		private const string TOURNAMENT_TYPE_GRAPE = "grape";

		// Token: 0x04005AA4 RID: 23204
		public const string INGAME_BOOST_STAR = "boost_star";

		// Token: 0x04005AA5 RID: 23205
		public const string INGAME_BOOST_HAMMER = "boost_hammer";

		// Token: 0x04005AA6 RID: 23206
		public const string INGAME_BOOST_RAINBOW = "boost_rainbow";

		// Token: 0x04005AA7 RID: 23207
		public const string PREGAME_BOOST_RAINBOW = "boost_pre_rainbow";

		// Token: 0x04005AA8 RID: 23208
		public const string PREGAME_BOOST_BOMB_LINE = "boost_pre_bomb_linegem";

		// Token: 0x04005AA9 RID: 23209
		public const string PREGAME_BOOST_DOUBLE_FISH = "boost_pre_double_fish";
	}
}
